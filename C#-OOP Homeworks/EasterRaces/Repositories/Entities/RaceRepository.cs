using EasterRaces.Models.Races.Contracts;
using EasterRaces.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EasterRaces.Repositories.Entities
{
    public class RaceRepository : IRepository<IRace>
    {
        private List<IRace> models;

        public RaceRepository()
        {
            models = new List<IRace>();
        }
        public void Add(IRace model)
        {
            models.Add(model);
        }

        public IReadOnlyCollection<IRace> GetAll()
        {
            return models.ToArray();
        }

        public IRace GetByName(string name)
        {
            return models.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(IRace model)
        {
            return models.Remove(model);
        }
    }
}
