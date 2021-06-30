using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models;
using SharedTrip.Models.Trips;
using SharedTrip.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public TripsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse Add() => View();

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddTripFormModel model) 
        {
            var modelError = this.validator.ValidateTrip(model);

            if (modelError.Any())
            {
                return Error(modelError);
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                DepartureTime = Convert.ToDateTime(model.DepartureTime),
                Description = model.Description,

            };

            this.data.Trips.Add(trip);
            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }



        [Authorize]
        public HttpResponse All() 
        {
            var trips = data.Trips.Select(x => new AllTripsFormModel
            {
                Id = x.Id,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                Seats = x.Seats,
                DepartureTime = x.DepartureTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
            })
                .ToList();

            return View(trips);

        }

        [Authorize]
        public HttpResponse Details() 
        {
            var trips = data.Trips.Select(x => new DetailsTripsFormModel
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                Seats = x.Seats,
                DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                Description = x.Description,

            }).FirstOrDefault();
                

            return View(trips);
        }

      
        public HttpResponse AddUserToTrip(string tripId) 
        {
            var trip = data.Trips.Where(x => x.Id == tripId).FirstOrDefault();
            var user = data.Users.Where(x => x.Id == User.Id).FirstOrDefault();

            var userTrip = new UserTrip
            {
                Trip = trip,
                User = user,
            };

            if (data.UserTrips.FirstOrDefault(x => x.User == user) != null)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }


            data.UserTrips.Add(userTrip);
            data.SaveChanges();

            return Redirect("/Trips/All");
        }
    }
}
