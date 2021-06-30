using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharedTrip.Services
{
    public interface IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFromModel model);

        public ICollection<string> ValidateTrip(AddTripFormModel model);
    }
}
