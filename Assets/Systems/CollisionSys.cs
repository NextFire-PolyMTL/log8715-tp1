using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollisionSys : ISystem
{
    public string Name => "CollisionSys";

    public void UpdateSystem()
    {
        
        World.Instance.ForEach<IsColliding>((entity, IsColliding) =>
        {
            var collidedShapes=World.Instance.GetComponent<CollidingWith>(entity).CollidedShapes;
            
        });
    }
}
