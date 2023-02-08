using System;
using System.Collections.Generic;
using UnityEngine;

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
    const int POOL_SIZE = 1024;

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
    private Dictionary<int, int> idToIndex = new Dictionary<int, int>();
    private Dictionary<int, Entity> indexToEntity = new Dictionary<int, Entity>();
    private Stack<int> freeIndexes = new Stack<int>();
    private int nextId = 0;
    private int nextIndex = 0;

    private World() { }

    public Entity CreateEntity()
    {
        var id = nextId++;
        var index = nextIndex < POOL_SIZE ? nextIndex++ : freeIndexes.Pop();
        var entity = new Entity(id);
        idToIndex[id] = index;
        indexToEntity[index] = entity;
        foreach (var component in components.Values)
        {
            component[index] = null;
        }
        Debug.Log($"[CreateEntity] id={id} index={index}");
        return entity;
    }

    public void DeleteEntity(Entity entity)
    {
        var index = idToIndex[entity.Id];
        freeIndexes.Push(index);
        Debug.Log($"[DeleteEntity] deleted: id={entity.Id} index={index}");
    }

    public T? GetComponent<T>(Entity entity) where T : struct, IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            var index = idToIndex[entity.Id];
            return (T?)components[typeof(T)][index];
        }
        return null;
    }

    public void SetComponent<T>(Entity entity, T component) where T : struct, IComponent
    {
        if (!components.ContainsKey(typeof(T)))
        {
            components[typeof(T)] = new IComponent[POOL_SIZE];
        }
        var index = idToIndex[entity.Id];
        components[typeof(T)][index] = component;
    }

    public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            var index = idToIndex[entity.Id];
            components[typeof(T)][index] = null;
        }
    }

    public void ForEach<T>(Action<Entity, T?> action) where T : struct, IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            var componentArray = components[typeof(T)];
            for (var i = 0; i < nextIndex; i++)
            {
                if (!freeIndexes.Contains(i))
                {
                    action(indexToEntity[i], (T?)componentArray[i]);
                }
            }
        }
    }
}
