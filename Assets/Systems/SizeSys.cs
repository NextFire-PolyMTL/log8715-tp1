using UnityEngine;

public class SizeSys : ISystem
{
    public string Name => "SizeSys";

    public void UpdateSystem()
    {


        World.Instance.ForEach<CollidingWith>((entity, collidingWith) =>
        {

            var size = World.Instance.GetComponent<Size>(entity).Value.Radius;
            if (collidingWith.HasValue)
            {

                //Debug.Log($"size:{collidingWith.Value.CollidedShapesSize[0].Radius}");
                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                for (var i = 0; i < collidedShapes.Count; i++)
                {
                    var size2 = collidedShapesSize[i].Radius;
                    //Debug.Log("deb");

                    if (size2 > size)
                    {
                        --size;
                        World.Instance.SetComponent<Size>(entity, new Size(size));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 + 1));
                    }

                    if (size2 < size)
                    {
                        ++size;
                        World.Instance.SetComponent<Size>(entity, new Size(size));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 - 1));
                    }

                    if (size < 1)
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
