using UnityEngine;

public class ClickSys : ISystem
{
    public string Name => nameof(ClickSys);

    public void UpdateSystem()
    {


        //We get the actual value of the cooldown
        var cooldown = World.Instance.GetSingleton<ClickCooldown>().Value.Time;

        //If the cooldown is finished, we can detect the user click
        if (cooldown - UnityEngine.Time.deltaTime <= 0)
        {
            //We get mouse's position
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //We listen to user click
            var mouseClick = (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));

            World.Instance.ForEach<Position>((entity, position) =>
            {

                var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;

                //if the mouse of the user is on a shape and the user click on it
                if (Mathf.Abs(mousePosition.x - position.Value.X) < (scale >> 1)
                && Mathf.Abs(mousePosition.y - position.Value.Y) < (scale >> 1)
                && mouseClick)
                {
                    //We set a tag related to the target entity to know that the corresponding shape has been clicked
                    World.Instance.SetComponent<IsClicked>(entity, new IsClicked());

                    //We initialize the cooldown again until the next click
                    World.Instance.SetSingleton<ClickCooldown>(new ClickCooldown(0.5f));
                }
            });
        }

        //If the cooldown is not finished, we decrement the cooldown
        else
        {
            World.Instance.SetSingleton<ClickCooldown>(new ClickCooldown(cooldown - UnityEngine.Time.deltaTime));
        }

    }
}
