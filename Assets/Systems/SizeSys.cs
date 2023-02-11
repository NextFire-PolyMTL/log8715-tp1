public class SizeSys : IPhysicSystem
{
    public string Name => nameof(SizeSys);

    public void UpdateSystem()
    {

        Utils.PhysicsForEach<CollidingWith>((entity, collidingWith) =>
        {
            // If the circle is protected its size won't change
            if (World.Instance.GetComponent<IsProtected>(entity).HasValue)
            {
                return;
            }
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
                    bool isProtected2 = World.Instance.GetComponent<IsProtected>(new Entity(collidedShapes[i])).HasValue;
                    //Debug.Log("deb");

                    if (scale2 > scale && !isProtected2)
                    {
                        --scale;
                        World.Instance.SetComponent<Size>(entity, new Size(scale));
                        //Changer CollidingWith pour éviter d'instencier des Entité à chaque fois
                        //World.Instance.SetComponent<Size>(new Entity(collidedShapes[i]), new Size(size2 + 1));
                    }

                    if (scale2 < scale)
                    {
                        // Somehow this causes obscure bugs
                        // TODO : Discuss & fix

                        // if (isProtected2)
                        // {
                        //     --scale;
                        // }
                        // else
                        // {
                        ++scale;
                        // }
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
