using UnityEngine;

public class SizeSys : ISystem
{
    public string Name => "SizeSys";

    public void UpdateSystem()
    {

        World.Instance.ForEach<CollidingWith>((entity, collidingWith) =>
        {

            var scame = World.Instance.GetComponent<Size>(entity).Value.Scale;
            if (collidingWith.HasValue)
            {

                //Debug.Log($"size:{collidingWith.Value.CollidedShapesSize[0].Radius}");
                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                for (var i = 0; i < collidedShapes.Count; i++)
                {
                    var scale2 = collidedShapesSize[i].Scale;
                    //Debug.Log("deb");

                    if (scale2 > scame)
                    {
                        --scame;
                        World.Instance.SetComponent<Size>(entity, new Size(scame));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 + 1));
                    }

                    if (scale2 < scame)
                    {
                        ++scame;
                        World.Instance.SetComponent<Size>(entity, new Size(scame));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 - 1));
                    }

                    if (scame < 1)
                    {
                        World.Instance.DeleteEntity(entity);
                        ECSManager.Instance.DestroyShape((uint)entity.Id);
                        //return;
                    }

                }
            }
        });
    }
}
