using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_.NETCore_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResultController : Controller
    {
      
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUser = HttpContext.User;
            string EmaillVal = "";

            if (currentUser.HasClaim(c => c.Type == "Email"))
            {
                EmaillVal = currentUser.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            }

            if (EmaillVal == "sadman@gmail.com")
            {
                return new string[] { "sadman" };

            }
            return new string[] { "admin" };




        }
    }
}
