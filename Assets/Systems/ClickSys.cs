using UnityEngine;
public class ClickSys : ISystem
{
    public string Name => "ClickSys";

    public void UpdateSystem()
    {
        var mousePosition = Input.mousePosition;
        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Radius;

            if (Mathf.Abs(mousePosition.x - position.X) < radius && Mathf.Abs(mousePosition.y - position.Y) < radius)
            {
                World.Instance.SetComponent<IsClicked>(entity, new IsClicked());
            }
        });

    }
}
