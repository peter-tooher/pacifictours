using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The User class is a model that represents a user in the system.
    public class User
    {
        // The UserId property uniquely identifies each user.
        public int UserId { get; set; }

        // The UserName property represents the username chosen by the user.
        public string UserName { get; set; } = string.Empty;

        // The FirstName and LastName properties represent the user's real name.
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // The UserEmail property represents the user's email address.
        public string UserEmail { get; set; } = string.Empty;

        // The PassportNumber property represents the user's passport number.
        public int PassportNumber { get; set; }

        // The ContactNumber property represents the user's contact number.
        public string ContactNumber { get; set; } = string.Empty;

        // The PasswordHash and PasswordSalt properties are used for storing the user's password securely.
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // The DateCreated property represents the date when the user's account was created.
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // The Role property represents the user's role in the system. By default, it is "Customer".
        public string Role { get; set; } = "Customer";
    }
}