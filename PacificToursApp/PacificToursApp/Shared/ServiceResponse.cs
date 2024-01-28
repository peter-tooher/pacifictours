using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The ServiceResponse<T> class is a generic class that represents the response from a service method.
    public class ServiceResponse<T>
    {
        // The Data property represents the data returned by the service method. It is of type T, which means it can be any type.
        // It is nullable, which means it can be null if the service method does not return any data.
        public T? Data { get; set; }

        // The Success property indicates whether the service method was successful. By default, it is true.
        public bool Success { get; set; } = true;

        // The Message property represents a message returned by the service method. It can be used to return an error message or a success message.
        public string Message { get; set; } = string.Empty;
    }
}
