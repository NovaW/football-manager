using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManager.Models;

namespace FootballManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<Stadium>> GetAllStadiums()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<Stadium> GetStadium(int stadiumId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult AddStadium(Stadium stadium)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult RemoveStadium(int stadiumId)
        {
            throw new NotImplementedException();
        }
    }
}
