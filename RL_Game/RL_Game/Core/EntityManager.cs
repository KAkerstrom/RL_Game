using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL_Game.Core
{
    public static class EntityManager
    {
        public delegate void OnEntityAddDelegate(Entity entity);
        public static OnEntityAddDelegate? OnEntityAdd;

        public delegate void OnEntityRemoveDelegate(Entity entity);
        public static OnEntityRemoveDelegate? OnEntityRemove;

        public delegate void OnComponentAddDelegate(Entity entity, Component component);
        public static OnComponentAddDelegate? OnComponentAdd;

        public delegate void OnComponentRemoveDelegate(Entity entity, Component component);
        public static OnComponentRemoveDelegate? OnComponentRemove;

        /// <summary>
        /// A direct reference to the player entity, to save time.
        /// </summary>
        public static Entity PlayerEntity { get; private set; }

        /// <summary>
        /// All entities, by the entity's ID.
        /// </summary>
        private static Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();

        public static void InitializePlayer(Entity player)
        {
            PlayerEntity = player;
            _entities.Add(player.Id, player);
        }

        /// <summary>
        /// Add an empty entity to the game.
        /// </summary>
        /// <returns>The created entity's ID.</returns>
     /*   public static int AddEntity(bool suppressEvent = false)
        {
            return AddEntity(new List<Component>(), suppressEvent);
        }
     */
        /// <summary>
        /// Add an entity to the game.
        /// </summary>
        /// <param name="components">The list of components that define the entity.</param>
        /// <returns>The created entity's ID.</returns>
        public static int AddEntity(/*List<Component> components*/Entity entity, bool suppressEvent = false)
        {
            //var entity = new Entity(components);
            _entities.Add(entity.Id, entity);
            if (!suppressEvent)
            {
                OnEntityAdd?.Invoke(entity);
            }
            return entity.Id;
        }

        /// <summary>
        /// Create an entity from a JSON string.
        /// </summary>
        /// <param name="json">The entity as a JSON-formatted string.</param>
        /// <returns>The created entity's ID.</returns>
        public static int CreateEntityFromJson(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create multiple entities from a JSON list string.
        /// </summary>
        /// <param name="json">The JSON to parse.</param>
        /// <returns>The list of created entities.</returns>
        public static List<int> CreateMultipleFromJson(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entityId">The entity ID to remove.</param>
        public static void RemoveEntity(int entityId, bool suppressEvent = false)
        {
            var entity = GetEntity(entityId);
            _entities.Remove(entityId);
            // TODO - Remove all components?
            if (!suppressEvent)
            {
                OnEntityRemove?.Invoke(entity);
            }
        }

        /// <summary>
        /// Add a component to an entity.
        /// </summary>
        /// <param name="entityId">The entity ID to add the component to.</param>
        /// <param name="component">The component to add.</param>
        public static void AddComponent(int entityId, Component component, bool suppressEvent = false)
        {
            var entity = GetEntity(entityId);
            entity.Components.Add(component);
            entity.ComponentBitArray.Set(component.TypeId, true);
            if (!suppressEvent)
            {
                OnComponentAdd?.Invoke(entity, component);
            }
        }

        /// <summary>
        /// Add multiple components to an entity.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="components">A list of components to add to the entity.</param>
        public static void AddComponents(int entityId, IEnumerable<Component> components, bool suppressEvent = false)
        {
            var entity = GetEntity(entityId);
            entity.Components.AddRange(components);
            foreach (var component in components)
            {
                entity.ComponentBitArray.Set(component.TypeId, true);
                if (!suppressEvent)
                {
                    OnComponentAdd?.Invoke(entity, component);
                }
            }
        }

        /// <summary>
        /// Remove a component from an entity.
        /// </summary>
        /// <param name="entityId">The entity ID to remove a component from.</param>
        /// <param name="componentId">The component ID to remove.</param>
        public static void RemoveComponent(int entityId, Component component, bool suppressEvent = false)
        {
            var entity = GetEntity(entityId);
            entity.Components.Remove(component);
            if (entity.Components.Count(c => c.TypeId == component.TypeId) == 0)
            {
                entity.ComponentBitArray.Set(component.TypeId, true);
            }
            if (!suppressEvent)
            {
                OnComponentRemove?.Invoke(entity, component);
            }
        }

        /// <summary>
        /// Removes all components of a given type from the entity.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="componentTypeId">The component type ID.</param>
        public static void RemoveAllComponentsOfType(int entityId, int componentTypeId, bool suppressEvent = false)
        {
            var entity = GetEntity(entityId);
            var components = entity.Components.Where(c => c.TypeId == componentTypeId);
            entity.Components.RemoveAll(c => c.TypeId == componentTypeId);
            entity.ComponentBitArray.Set(componentTypeId, true);
            if (!suppressEvent)
            {
                foreach (var component in components)
                {
                    OnComponentRemove?.Invoke(entity, component);
                }
            }
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns>The entity.</returns>
        public static Entity GetEntity(int entityId)
        {
            if (_entities.TryGetValue(entityId, out var entity))
            {
                return entity;
            }
            throw new Exception($"No entity exists with the ID: {entityId}. Failed to get entity.");
        }

        /// <summary>
        /// Get whether an entity has a component type.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <param name="componentTypeId">The component type ID.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool EntityHasComponentType(int entityId, int componentTypeId)
        {
            return GetEntity(entityId).ComponentBitArray.Get(componentTypeId);
        }

        /// <summary>
        /// Get all entities with the given component type.
        /// </summary>
        /// <param name="componentTypeId">The component type ID.</param>
        /// <returns>A list of entities with the given component type.</returns>
        public static IEnumerable<Entity> GetEntitiesWithComponentType(int componentTypeId)
        {
            return _entities.Values.Where(e => e.ComponentBitArray.Get(componentTypeId));
        }
    }
}
