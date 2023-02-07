using UnityEngine;

public class CollisionSys : ISystem
{
    public string Name => "CollisionSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Value.Radius;

            //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            //(et en faire peut-être une cst....)
            var screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            if (Mathf.Abs(position.Value.X) + radius/2 >= screenBoundary.x || Mathf.Abs(position.Value.Y) + radius/2 >= screenBoundary.y)
            {
                World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
            }

        });

    }
}
