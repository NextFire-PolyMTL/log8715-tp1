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

            //If an entity has collide with others
            if (collidingWith.HasValue)
            {

                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                for (int i = 0; i < collidedShapes.Count; i++)
                {
                    var isStatic2 = World.Instance.GetComponent<IsStatic>(new Entity(collidedShapes[i]));
                    //If an entity collides with a static one, its size doesn't change
                    if (isStatic2.HasValue)
                    {
                        return;
                    }

                    var scale2 = collidedShapesSize[i].Scale;
                    bool isProtected2 = World.Instance.GetComponent<IsProtected>(new Entity(collidedShapes[i])).HasValue;
                    //If an entity collides with another which has a bigger size and is protected, we reduce the size of the entity
                    if (scale2 > scale && !isProtected2)
                    {
                        --scale;
                        World.Instance.SetComponent<Size>(entity, new Size(scale));
                    }

                    //If an entity collides with another which has a smaller size,
                    if (scale2 < scale)
                    {
                        //If the other enitty is protected, the entity lose some of its size
                        if (isProtected2)
                        {
                            --scale;
                        }
                        //Otherwise, we increment its size
                        else
                        {
                            ++scale;
                        }
                        World.Instance.SetComponent<Size>(entity, new Size(scale));
                    }

                    //If the size of an enity is less than 1, we destroy the entity
                    if (scale < 1)
                    {
                        //We delete the entity after the execution of other systems in another system which execute the singleton CommandBuffer
                        Utils.AddCommandToBuffer(() =>
                        {
                            World.Instance.DeleteEntity(entity);
                            ECSManager.Instance.DestroyShape(entity.Id);
                        });
                        return;
                    }

                }
            }
        });
    }
}
