public class ClickSys : ISystem
{
    public string Name => "ClickSys";

    public void UpdateSystem()
    {
        var mousePosition = Input.mousePosition;
        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Radius;

            if (Mathf.abs(mousePosition.x - position.X) < radius && Mathf.abs(mousePosition.y - position.Y) < radius)
            {
                World.Instance.SetComponent<IsClicked>(entity, new IsClicked());
            }
        });

    }
}
