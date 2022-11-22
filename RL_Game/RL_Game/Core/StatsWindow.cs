using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL_Game.Components;
using RLNET;

namespace RL_Game.Core
{
    public class StatsWindow
    {
        //empty constructor
        public StatsWindow(GameMap level) 
        {

        }
        public void Draw(RLConsole statsConsole)
        {
            var yPos = 0;
            foreach (Entity entity in EntityManager.GetEntitiesWithComponentType((int)Component.ComponentTypeIds.Stat))
            {
                var stats = entity.GetFirstComponent((int)Component.ComponentTypeIds.Stat) as StatComponent;
                if (stats != null)
                {
                    string toWrite = $"{stats.EntityName}:{stats.HealthRemaining}/{stats.HealthTotal}";
                    for (int i = 0; i < toWrite.Length; i++)
                    {
                        statsConsole.Set(i, yPos, DefaultColors.ForegroundVisible, DefaultColors.BackgroundVisible, toWrite [i]);
                    }
                    yPos+=3;
                }
            }
        }
    }
}
