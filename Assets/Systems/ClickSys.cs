using UnityEngine;

public class ClickSys : ISystem
{
    public string Name => nameof(ClickSys);

    public void UpdateSystem()
    {

        //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseClick = (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
        var cooldown = World.Instance.GetSingleton<Cooldown>().Value.Time;
        if (cooldown - UnityEngine.Time.deltaTime <= 0)
        {

            World.Instance.ForEach<Position>((entity, position) =>
            {

                var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;

                if (Mathf.Abs(mousePosition.x - position.Value.X) < (scale >> 1)
                && Mathf.Abs(mousePosition.y - position.Value.Y) < (scale >> 1)
                && mouseClick)
                {
                    World.Instance.SetComponent<IsClicked>(entity, new IsClicked());

                    World.Instance.SetSingleton<Cooldown>(new Cooldown(0.5f));
                }
            });
        }
        else
        {
            World.Instance.SetSingleton<Cooldown>(new Cooldown(cooldown - UnityEngine.Time.deltaTime));
        }

    }
}
