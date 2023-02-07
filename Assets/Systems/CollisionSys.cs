using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollisionSys : ISystem
{
    public string Name => "CollisionSys";

    public void UpdateSystem()
    {


        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Radius;

            //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            //(et en faire peut-être une cst....)
            var screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            
            if (Mathf.Abs(position.X) + radius >= screenBoundary.x || Mathf.Abs(position.Y) + radius >= screenBoundary.y)
            {
                World.Instance.SetComponent<IsColliding>(entity, new IsColliding());  
            }
        });

    }
}
