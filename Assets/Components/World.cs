using System;
using System.Collections.Generic;

public readonly struct Entity
{
    public Entity(uint id)
    {
        Id = id;
    }

    public readonly uint Id;
}

public class World
{
    const uint POOL_SIZE = 1024;

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

        set => s_instance = value;
    }
    #endregion

    private Dictionary<Type, IComponent[]> _components = new Dictionary<Type, IComponent[]>();
    private Dictionary<uint, uint> _idToIndex = new Dictionary<uint, uint>();
    private Dictionary<uint, Entity> _indexToEntity = new Dictionary<uint, Entity>();
    private uint _nextId = 0;
    private uint _nextIndex = 0;
    private Stack<uint> _freeIndexes = new Stack<uint>();

    private Dictionary<Type, IComponent> _singletons = new Dictionary<Type, IComponent>();

    private World() { }

    public Entity CreateEntity()
    {
        var id = _nextId++;
        var index = _nextIndex < POOL_SIZE ? _nextIndex++ : _freeIndexes.Pop();
        var entity = new Entity(id);
        _idToIndex[id] = index;
        _indexToEntity[index] = entity;
        foreach (var component in _components.Values)
        {
            component[index] = null;
        }
        //Debug.Log($"[CreateEntity] id={id} index={index}");
        return entity;
    }

    public void DeleteEntity(Entity entity)
    {
        var index = _idToIndex[entity.Id];
        _freeIndexes.Push(index);
        //Debug.Log($"[DeleteEntity] deleted: id={entity.Id} index={index}");
    }

    public T? GetComponent<T>(Entity entity) where T : struct, IComponent
    {
        if (_components.ContainsKey(typeof(T)))
        {
            var index = _idToIndex[entity.Id];
            return (T?)_components[typeof(T)][index];
        }
        return null;
    }

    public void SetComponent<T>(Entity entity, T component) where T : struct, IComponent
    {
        if (!_components.ContainsKey(typeof(T)))
        {
            _components[typeof(T)] = new IComponent[POOL_SIZE];
        }
        var index = _idToIndex[entity.Id];
        _components[typeof(T)][index] = component;
    }

    public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
    {
        if (_components.ContainsKey(typeof(T)))
        {
            var index = _idToIndex[entity.Id];
            _components[typeof(T)][index] = null;
        }
    }

    public void ForEach<T>(Action<Entity, T?> action) where T : struct, IComponent
    {
        if (_components.ContainsKey(typeof(T)))
        {
            var componentArray = _components[typeof(T)];
            for (var i = 0u; i < _nextIndex; i++)
            {
                if (!_freeIndexes.Contains(i))
                {
                    action(_indexToEntity[i], (T?)componentArray[i]);
                }
            }
        }
    }

    public T? GetSingleton<T>() where T : struct, IComponent
    {
        if (_singletons.ContainsKey(typeof(T)))
        {
            return (T?)_singletons[typeof(T)];
        }
        return null;
    }

    public void SetSingleton<T>(T value) where T : struct, IComponent
    {
        _singletons[typeof(T)] = value;
    }

    public World Clone(Func<IComponent, IComponent> componentCloner)
    {
        var clone = new World();

        // Simple value attributes
        clone._idToIndex = new Dictionary<uint, uint>(_idToIndex);
        clone._indexToEntity = new Dictionary<uint, Entity>(_indexToEntity);
        clone._nextId = _nextId;
        clone._nextIndex = _nextIndex;
        clone._freeIndexes = new Stack<uint>(_freeIndexes);

        // Components
        foreach (var kvp in _components)
        {
            var componentArray = kvp.Value;
            var cloneArray = new IComponent[POOL_SIZE];
            for (var i = 0u; i < _nextIndex; i++)
            {
                if (componentArray[i] is IComponent component && !_freeIndexes.Contains(i))
                {
                    cloneArray[i] = componentCloner(component);
                }
            }
            clone._components[kvp.Key] = cloneArray;
        }

        // Singletons
        foreach (var kvp in _singletons)
        {
            if (kvp.Value is IComponent component)
            {
                clone._singletons[kvp.Key] = componentCloner(component);
            }
        }

        return clone;
    }
}
