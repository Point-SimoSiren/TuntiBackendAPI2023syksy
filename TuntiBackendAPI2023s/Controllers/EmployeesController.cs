using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuntiBackendAPI2023s.Models;

namespace TuntiBackendAPI2023s.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private tuntidbContext db = new tuntidbContext();

        [HttpGet]
        public ActionResult getAll()
        {
            var emp = db.Employees.Where(emp => emp.Active == true).ToList();

            return Ok(emp);
        }


    }
}
