using UnityEngine;

public class ColorSys : ISystem
{
    public string Name => "ColorSys";

    public void UpdateSystem()
    {


        World.Instance.ForEach<Position>((entity, position) =>
        {
            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            //var isClicked = World.Instance.GetComponent<IsClicked>(entity);
            var bornOfClick = World.Instance.GetComponent<BornOfClick>(entity);
            var isColliding = World.Instance.GetComponent<IsColliding>(entity);
            var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);
            var scale= World.Instance.GetComponent<Size>(entity).Value.Scale;
            var isCollided = World.Instance.GetComponent<IsCollided>(entity);

            if (isStatic.HasValue)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.red));
            }
            else if(collidingWith.HasValue){
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.green));
            }
            else if(scale==ECSManager.Instance.Config.explosionSize-1){
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(1,0.5f,0.01f)));
            }
            else if(bornOfClick.HasValue){
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(1,0.50f,0.7f)));
            }
            else{
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.blue));
            }


        });
    }
}
