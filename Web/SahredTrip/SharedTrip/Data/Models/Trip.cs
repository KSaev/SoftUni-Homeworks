using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Models
{
    public class Trip
    {

        public Trip()
        {
            this.UserTrips = new HashSet<UserTrip>();
        }

        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [MinLength(2)]
        [MaxLength(6)]
        public int Seats { get; set; }

        [Required]
        [MaxLength(60)]
        public string Description { get; set; }
        
        public string ImagePath { get; set; }

        public IEnumerable<UserTrip> UserTrips { get; set; }
    }
}
