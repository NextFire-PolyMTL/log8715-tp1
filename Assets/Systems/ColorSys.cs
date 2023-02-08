using UnityEngine;

public class ColorSys : ISystem
{
    public string Name => "ColorSys";

    public void UpdateSystem()
    {


        World.Instance.ForEach<Position>((entity, position) =>
        {
            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            var isClicked = World.Instance.GetComponent<IsClicked>(entity);
            var isColliding = World.Instance.GetComponent<IsClicked>(entity);


            if (isStatic.HasValue)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.red));
            }


        });
    }
}
