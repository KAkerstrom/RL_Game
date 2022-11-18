using System.Collections;
using System.Reflection;

namespace RL_Game.Core
{
    public class Entity
    {
        /// <summary>
        /// The next available ID to use when creating an instance.
        /// </summary>
        private static int _nextId = 0;

        private readonly int _id;
        private List<Component> _components;
        private BitArray _componentBitArray;

        public int Id => _id;
        public string Tag="default";
        public List<Component> Components => _components;
        public BitArray ComponentBitArray => _componentBitArray;

        public Entity(List<Component> components)
        {
            _id = _nextId++;
            _components = components ?? new List<Component>();
            _componentBitArray = new BitArray(Component.ComponentTypeBitArrayLength);

            foreach (var component in Components)
            {
                _componentBitArray.Set(component.TypeId, true);
            }
        }

        public Component? GetFirstComponent(int componentTypeId)
        {
            return _components.FirstOrDefault(c => c.TypeId == componentTypeId);
        }
    }
}
