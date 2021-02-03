using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players;
using CounterStrike.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Models.Maps
{
    public class Map : IMap
    {
        private List<IPlayer> counterTerrorists;
        private List<IPlayer> terrorists;

        public Map()
        {
            counterTerrorists = new List<IPlayer>();
            terrorists = new List<IPlayer>();
        }
        public string Start(ICollection<IPlayer> players)
        {

            SplitTeams(players);

            while (IsTeamAlive(terrorists) && IsTeamAlive(counterTerrorists))
            {
                Attack(terrorists, counterTerrorists);
                Attack(counterTerrorists, terrorists);
            }

            if (IsTeamAlive(counterTerrorists))
            {
                return "Counter Terrorist wins!";
            }
            else if (IsTeamAlive(terrorists))
            {
                return "Terrorist wins!";
            }
            else
            {
                return "Something went wrong";
            }
            
        }

        private void Attack(List<IPlayer> attackingTeam, List<IPlayer> attackedTeam)
        {
            foreach (var attacker in attackingTeam)
            {
                //if (!(attacker.IsAlive))
                //{
                //    continue;
                //}
                foreach (var attacked in attackedTeam)
                {
                    if (attacked.IsAlive)
                    {
                        attacked.TakeDamage(attacker.Gun.Fire());
                    }
                }
            }
        }
        private bool IsTeamAlive(List<IPlayer> team)
        {
            return team.Any(p => p.IsAlive);
        }

        private void SplitTeams(ICollection<IPlayer> players)
        {
            foreach (var item in players)
            {
                if (item is CounterTerrorist)
                {
                    counterTerrorists.Add(item);
                }
                else if(item is Terrorist)
                {
                    terrorists.Add(item);
                }
            }

        }
    }
}
