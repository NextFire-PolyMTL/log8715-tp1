using System;
using System.Collections.Generic;

// Represents an entity in the world
public readonly struct Entity
{
    public Entity(uint id)
    {
        Id = id;
    }

    public readonly uint Id;
}

// Manage entities and components
public class World
{
    // Max number of simultaneous entities
    const uint POOL_SIZE = 1024;

    #region Singleton
    private static World s_instance;
    public static World Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new();
            }
            return s_instance;
        }

        set => s_instance = value;
    }
    #endregion

    // Components are stored in arrays (pool allocation), one array per component type
    private Dictionary<Type, IComponent[]> _components = new();
    // Maps entity id to index in the component arrays
    private Dictionary<uint, uint> _idToIndex = new();
    // Maps index in the component arrays to entity
    private Dictionary<uint, Entity> _indexToEntity = new();
    // Next id to assign to an entity
    private uint _nextId = 0;
    // Next index to use in the component arrays (pool growing phase)
    private uint _nextIndex = 0;
    // Indexes of deleted entities
    private Stack<uint> _freeIndexes = new();

    // Singletons are stored in a dictionary
    private Dictionary<Type, IComponent> _singletons = new();

    private World() { }

    public Entity CreateEntity()
    {
        var id = _nextId++;
        // If the pool is fully grown, reuse deleted entities indexes
        var index = _nextIndex < POOL_SIZE ? _nextIndex++ : _freeIndexes.Pop();
        var entity = new Entity(id);
        _idToIndex[id] = index;
        _indexToEntity[index] = entity;
        // Clear components of previous entity at this index
        foreach (var component in _components.Values)
        {
            component[index] = null;
        }
        //Debug.Log($"[CreateEntity] id={id} index={index}");
        return entity;
    }

    public Entity GetEntity(uint id)
    {
        return _indexToEntity[_idToIndex[id]];
    }

    public void DeleteEntity(Entity entity)
    {
        var index = _idToIndex[entity.Id];
        _freeIndexes.Push(index);
        //Debug.Log($"[DeleteEntity] deleted: id={entity.Id} index={index}");
        // Cleanup is done in CreateEntity
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
            // Create the component array if it doesn't exist
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
        // If the component array doesn't exist, yield null
        var componentArray = _components.GetValueOrDefault(typeof(T), null);
        for (uint i = 0; i < _nextIndex; i++)
        {
            if (!_freeIndexes.Contains(i))
            {
                action(_indexToEntity[i], (componentArray is null) ? null : (T?)componentArray[i]);
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

    // Deep clone the world, quite expensive
    public World Clone(Func<IComponent, IComponent> componentCloner)
    {
        var clone = new World();

        // Simple value attributes
        clone._idToIndex = new(_idToIndex);
        clone._indexToEntity = new(_indexToEntity);
        clone._nextId = _nextId;
        clone._nextIndex = _nextIndex;
        clone._freeIndexes = new(_freeIndexes);

        // Components
        foreach (var kvp in _components)
        {
            var componentArray = kvp.Value;
            var cloneArray = new IComponent[POOL_SIZE];
            for (uint i = 0; i < _nextIndex; i++)
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
