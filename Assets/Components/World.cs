using System;
using System.Collections.Generic;

public readonly struct Entity
{
    public Entity(int id)
    {
        Id = id;
    }

    public readonly int Id;
}

public class World
{
    const int MAX_ENTITIES = 1024;

    #region Singleton
    private static World s_instance;
    public static World Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new World();
            }
            return s_instance;
        }
    }
    #endregion

    private Dictionary<Type, IComponent[]> components = new Dictionary<Type, IComponent[]>();
    private bool[] isEntityActive = new bool[MAX_ENTITIES];

    private World() { }

    public Entity CreateEntity()
    {
        for (var i = 0; i < MAX_ENTITIES; i++)
        {
            if (!isEntityActive[i])
            {
                isEntityActive[i] = true;
                return new Entity(i);
            }
        }
        throw new Exception("Max entities reached");
    }

    public void DeleteEntity(Entity entity)
    {
        isEntityActive[entity.Id] = false;
    }

    public void SetComponent<T>(Entity entity, T component) where T : IComponent
    {
        if (!isEntityActive[entity.Id])
        {
            throw new Exception("Entity is not active");
        }
        if (!components.ContainsKey(typeof(T)))
        {
            components[typeof(T)] = new IComponent[MAX_ENTITIES];
        }
        components[typeof(T)][entity.Id] = component;
    }

    public void RemoveComponent<T>(Entity entity) where T : IComponent
    {
        if (!isEntityActive[entity.Id])
        {
            throw new Exception("Entity is not active");
        }
        if (components.ContainsKey(typeof(T)))
        {
            components[typeof(T)][entity.Id] = null;
        }
    }
}
