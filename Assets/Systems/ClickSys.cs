using UnityEngine;

public class ClickSys : ISystem
{
    public string Name => "ClickSys";

    public void UpdateSystem()
    {
        var mousePosition = Input.mousePosition;
        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Value.Radius;

            if (Mathf.Abs(mousePosition.x - position.Value.X) < radius && Mathf.Abs(mousePosition.y - position.Value.Y) < radius)
            {
                World.Instance.SetComponent<IsClicked>(entity, new IsClicked());
            }
        });

    }
}
