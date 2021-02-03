using CounterStrike.Models.Players.Contracts;
using CounterStrike.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CounterStrike.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private List<IPlayer> models;

        public PlayerRepository()
        {
            this.models = new List<IPlayer>();
        }
        public IReadOnlyCollection<IPlayer> Models
        {
            get
            {
                return this.models.AsReadOnly();
            }
        }

        public void Add(IPlayer model)
        {
            if (model is null)
            {
                throw new ArgumentException("Cannot add null in Player Repository");
            }

            models.Add(model);
        }

        public IPlayer FindByName(string name)
        {
            foreach (var item in models)
            {
                if (item.Username == name)
                {
                    return item;
                }
            }
            return null;
        }

        public bool Remove(IPlayer model)
        {
            return models.Remove(model);
        }
    }
}
