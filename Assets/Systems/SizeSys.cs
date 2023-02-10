public class SizeSys : IPhysicSystem
{
    public string Name => nameof(SizeSys);

    public void UpdateSystem()
    {

        Utils.PhysicsForEach<CollidingWith>((entity, collidingWith) =>
        {

            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;
            if (collidingWith.HasValue)
            {

                //Debug.Log($"size:{collidingWith.Value.CollidedShapesSize[0].Radius}");
                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                for (int i = 0; i < collidedShapes.Count; i++)
                {
                    var isStatic2 = World.Instance.GetComponent<IsStatic>(new Entity(collidedShapes[i]));
                    if (isStatic2.HasValue)
                    {
                        return;
                    }
                    var scale2 = collidedShapesSize[i].Scale;
                    //Debug.Log("deb");

                    if (scale2 > scale)
                    {
                        --scale;
                        World.Instance.SetComponent<Size>(entity, new Size(scale));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 + 1));
                    }

                    if (scale2 < scale)
                    {
                        ++scale;
                        World.Instance.SetComponent<Size>(entity, new Size(scale));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 - 1));
                    }

                    if (scale < 1)
                    {
                        World.Instance.DeleteEntity(entity);
                        ECSManager.Instance.DestroyShape(entity.Id);
                        //return;
                    }

                }
            }
        });
    }
}
