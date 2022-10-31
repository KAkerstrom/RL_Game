using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL_Game.Core
{
    public partial class Component
    {
        /// <summary>
        /// This is the upper bound for the BitArray used for tracking components.
        /// If we have >32 component types, this will need to be increased.
        /// </summary>
        public const int ComponentTypeBitArrayLength = 32;

        /// <summary>
        /// The next available ID to use when creating an instance.
        /// </summary>
        static int _nextId = 0;

        public enum ComponentTypeIds
        {
            Draw,
            Position,
        }
    }

    public partial class Component
    {
        /// <summary>
        /// This component's unique ID.
        /// </summary>
        public readonly int Id;
        public readonly int TypeId;

        public Component(int componentTypeId)
        {
            Id = _nextId++;
            TypeId = componentTypeId;
        }
    }
}
