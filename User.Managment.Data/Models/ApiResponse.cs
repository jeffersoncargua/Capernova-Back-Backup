using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool isSuccess { get; set; } = true;
        public object? Result { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

    }
}
