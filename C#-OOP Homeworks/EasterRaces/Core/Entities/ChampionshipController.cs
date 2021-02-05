using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Cars.Entities;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Drivers.Entities;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Models.Races.Entities;
using EasterRaces.Repositories.Contracts;
using EasterRaces.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasterRaces.Core.Entities
{
    public class ChampionshipController : IChampionshipController
    {
        private readonly IRepository<IDriver> driverRepository;
        private readonly IRepository<ICar> carRepository;
        private readonly IRepository<IRace> raceRepository;
        public ChampionshipController()
        {
            driverRepository = new DriverRepository();
            carRepository = new CarRepository();
            raceRepository = new RaceRepository();
        }

        public string AddCarToDriver(string driverName, string carModel)
        {
            IDriver driver = driverRepository.GetByName(driverName);
            ICar car = carRepository.GetByName(carModel);
            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }
            if (car == null)
            {
                throw new InvalidOperationException($"Car {carModel} could not be found.");
            }

            driver.AddCar(car);
            return $"Driver {driverName} received car {carModel}.";
        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            IRace race = raceRepository.GetByName(raceName);
            IDriver driver = driverRepository.GetByName(driverName);
            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }
            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }

            race.AddDriver(driver);
            return $"Driver {driverName} added in {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            var carExist = carRepository.GetByName(model);
            if (carExist != null)
            {
                throw new ArgumentException($"Car {model} is already created.");
            }

            Car car;
            if (type == "Muscle")
            {
                car = new MuscleCar(model, horsePower);
            }
            else if (type == "Sports")
            {
                car = new SportsCar(model, horsePower);
            }
            else
            {
                return "Greshen tip";
            }

            carRepository.Add(car);
            return $"{car.GetType().Name} {model} is created.";
        }

        public string CreateDriver(string driverName)
        {
            var driverExist = driverRepository.GetByName(driverName);
            var driver = new Driver(driverName);
            if (driverExist != null)
            {
                throw new ArgumentException($"Driver {driverName} is already created.");
            }

            driverRepository.Add(driver);
            return $"Driver {driverName} is created.";
        }

        public string CreateRace(string name, int laps)
        {
            var raceExist = raceRepository.GetByName(name);
            if (raceExist != null)
            {
                throw new InvalidOperationException($"Race {name} is already created.");
            }

            var race = new Race(name, laps);
            raceRepository.Add(race);
            return $"Race {name} is created.";
        }

        public string StartRace(string raceName)
        {
            IRace race = raceRepository.GetByName(raceName);
          
            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }
            if (race.Drivers.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than 3 participants.");
            }

            var sortedDrivers = race.Drivers.OrderByDescending(x => x.Car.CalculateRacePoints(race.Laps)).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Driver {sortedDrivers[0].Name} wins {raceName} race.");
            sb.AppendLine($"Driver {sortedDrivers[1].Name} is second in {raceName} race.");
            sb.AppendLine($"Driver {sortedDrivers[2].Name} is third in {raceName} race.");

            this.raceRepository.Remove(race); 

            return sb.ToString().Trim();
        }
    }
}
