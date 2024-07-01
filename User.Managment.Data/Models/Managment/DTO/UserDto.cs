using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }
    }
}
