using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Models
{
    public class User
    {

        public User()
        {
            this.UserTrips = new HashSet<UserTrip>();
        }

        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public IEnumerable<UserTrip> UserTrips { get; set; }
    }
}
