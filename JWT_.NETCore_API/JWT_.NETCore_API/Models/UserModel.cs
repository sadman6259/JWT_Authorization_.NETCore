using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_.NETCore_API.Models
{
    public class UserModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public Nullable<DateTime> DateOfJoing { get; set; }



    }
}
