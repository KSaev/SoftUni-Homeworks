using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Repositories
{
    public class GunRepository : IRepository<IGun>
    {
        private List<IGun> models;

        public GunRepository()
        {
            this.models = new List<IGun>();
        }
        public IReadOnlyCollection<IGun> Models
        {
            get
            {
                return this.models.AsReadOnly();
            }
        }

        public void Add(IGun model)
        {
            if (model is null)
            {
                throw new ArgumentException("Cannot add null in Gun Repository");
            }

            models.Add(model);
        }

        public IGun FindByName(string name)
        {
            foreach (var item in models)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public bool Remove(IGun model)
        {
            return models.Remove(model);
        }
    }
}
