using UnityEngine;

public class ColorSys : IPhysicSystem
{
    public string Name => nameof(ColorSys);

    public void UpdateSystem()
    {


        Utils.PhysicsForEach<Position>((entity, position) =>
        {

            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            //var isClicked = World.Instance.GetComponent<IsClicked>(entity);
            var bornOfClick = World.Instance.GetComponent<BornOfClick>(entity);
            var isColliding = World.Instance.GetComponent<IsColliding>(entity);
            var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;
            //var isCollided = World.Instance.GetComponent<IsCollided>(entity);
            var isProtected = World.Instance.GetComponent<IsProtected>(entity);

            //If the object is static, its color is red
            if (isStatic.HasValue)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.red));
            }
            //If the object collid with other object, its color is green
            else if (collidingWith.HasValue)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.green));
            }
            //If the object will explose the next time its size increases, its color is orange
            else if (scale == ECSManager.Instance.Config.explosionSize - 1)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(1, 0.5f, 0.01f)));
            }
            //If the object is born from an explosion caused by a user's click, its color is pink
            else if (bornOfClick.HasValue)
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(1, 0.50f, 0.7f)));
            }
            //Default color (blue)
            else
            {
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(Color.blue));
            }
            
            // TODO remove (just for debug)
            if (isProtected.HasValue)
            {
                Color color = World.Instance.GetComponent<ColorCompo>(entity).Value.ShapeColor;
                // World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(color.r, color.g, color.b, 0.5f)));
                World.Instance.SetComponent<ColorCompo>(entity, new ColorCompo(new Color(204, 59, 43)));
            }
        });
    }
}
