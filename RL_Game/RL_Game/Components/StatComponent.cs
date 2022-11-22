using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL_Game.Core;

namespace RL_Game.Components
{
    internal class StatComponent : Component
    {
        private int _healthTotal = 0, _healthRemaining=0;
        private string _entityName = "";
        public StatComponent(int health,string entityname) :base((int)ComponentTypeIds.Stat) 
        { 
            _healthTotal=_healthRemaining= health;
            _entityName = entityname;
        }
        public StatComponent(int healthTotal, int healthRemaining,string entityname) : base((int)ComponentTypeIds.Stat)
        {
            if(healthRemaining>healthTotal)
            {
                throw new Exception("Entity can't have more health than their total");
            }
            _healthTotal = healthTotal;
            _healthRemaining = healthRemaining;
            _entityName = entityname;
        }
        public void TakeDamage(int damage)
        {
           if(_healthRemaining>0+damage)
            {
                _healthRemaining-=damage;
            }
           else _healthRemaining= 0;
        }
        public void HealDamage(int heal)
        {
            if (_healthRemaining < _healthTotal - heal)
            {
                _healthRemaining += heal;
            }
            else _healthRemaining = _healthTotal;
        }
        public string EntityName {  get { return _entityName; }  }
        public int HealthTotal { get { return _healthTotal;} }
        public int HealthRemaining { get { return _healthRemaining;} }
    }
}
