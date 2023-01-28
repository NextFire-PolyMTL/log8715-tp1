using System;
using System.Collections.Generic;

public readonly struct Entity
{
    public Entity(uint id)
    {
        Id = id;
    }

    public readonly uint Id;

    public override string ToString()
    {
        return $"<Entity Id={Id}>";
    }
}

public class ComponentManager
{
    #region Singleton
    private static ComponentManager s_instance;
    public static ComponentManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new ComponentManager();
            }
            return s_instance;
        }
    }
    #endregion

    private uint currentId = 0;
    private Dictionary<uint, IComponent> components = new Dictionary<uint, IComponent>();
    private Dictionary<Type, List<Entity>> componentTypes = new Dictionary<Type, List<Entity>>();

    public Entity AddComponent<T>(T component) where T : IComponent
    {
        var entity = new Entity(id: currentId++);
        components.Add(entity.Id, component);
        if (!componentTypes.ContainsKey(typeof(T)))
        {
            componentTypes.Add(typeof(T), new List<Entity>());
        }
        componentTypes[typeof(T)].Add(entity);
        return entity;
    }

    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        return (T)components[entity.Id];
    }

    public IEnumerable<T> GetComponents<T>() where T : IComponent
    {
        if (componentTypes.ContainsKey(typeof(T)))
        {
            foreach (var entity in componentTypes[typeof(T)])
            {
                yield return (T)components[entity.Id];
            }
        }
    }

    public void ForEach<T>(Action<Entity, T> action) where T : IComponent
    {
        if (componentTypes.ContainsKey(typeof(T)))
        {
            foreach (var entity in componentTypes[typeof(T)])
            {
                action(entity, (T)components[entity.Id]);
            }
        }
    }

    public Entity UpdateComponent<T>(Entity entity, T component) where T : IComponent
    {
        components[entity.Id] = component;
        return entity;
    }

    public void RemoveComponent<T>(Entity entity) where T : IComponent
    {
        components.Remove(entity.Id);
        componentTypes[typeof(T)].Remove(entity);
    }

    public string DebugStr()
    {
        var str = "<color=green>ComponentManager</color>\n";
        str += $"Count={components.Count}\n";
        foreach (var components in componentTypes)
        {
            str += $"{components.Key}: {string.Join(", ", components.Value)}\n";
        }
        return str;
    }
}
