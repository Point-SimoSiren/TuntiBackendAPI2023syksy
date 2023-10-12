using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuntiBackendAPI2023s.Models;

namespace TuntiBackendAPI2023s.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkassignmentsController : ControllerBase
    {
        private tuntidbContext db = new tuntidbContext();

        [HttpGet]
        public ActionResult GetAll()
        {
            var wa = db.WorkAssignments.Where(wa => wa.Active == true && wa.Completed == false);
            return Ok(wa);
        }


        [HttpPost]
        public bool StartStop(Operation op)
        {

            try
            {

                if (op.OperationType == "start")

                {
                    // START

                    var wa = db.WorkAssignments.Find(op.WorkAssignmentID);

                    if (wa != null)
                    { // Jos työ löytyy

                        if (wa.InProgress == true) // Jos yrittää käynnistää jo aloitettua työtä
                        {
                            return false;
                        }

                        else
                        {
                            // WA riviä muokataa nyt start tilanteessa
                            wa.InProgress = true;
                            wa.WorkStartedAt = DateTime.Now.AddHours(-1);
                            //wa.CompletedAt = wa.CompletedAt;
                            //wa.DeletedAt = wa.DeletedAt;
                            //wa.CreatedAt = wa.CreatedAt;
                            //wa.LastModifiedAt = wa.LastModifiedAt;
                            db.SaveChanges();
                        }
                    }
                    else // Jos koko työtä ei löydy
                    {
                        return false;
                    }

                    // Uuden timesheet rivin luominen kun työ alkaa
                    Timesheet ts = new Timesheet();
                    ts.Comments = op.Comment;
                    ts.IdEmployee = op.EmployeeID;
                    ts.IdCustomer = wa.IdCustomer;
                    ts.IdWorkAssignment = op.WorkAssignmentID;
                    ts.Comments = op.Comment;
                    ts.CreatedAt = DateTime.Now.AddHours(-1);
                    ts.StartTime = DateTime.Now.AddHours(-1);
                    ts.Latitude = op.Latitude;
                    ts.Longitude = op.Longitude;
                    ts.Active = true;
                 
                    db.Timesheets.Add(ts);
                    db.SaveChanges();
                    return true;
                }

                // STOP
                else
                {
                    // WA riviä muokataan nyt stop vaiheessa
                    WorkAssignment wa = db.WorkAssignments.Find(op.WorkAssignmentID);

                    if (wa.InProgress != true) {

                        return false;
                    }
                    // Jos työ on aloitettu niin voidaan jatkaa lopetusrutiinia:

                    wa.InProgress = false;
                    wa.Completed = true;
                    wa.CompletedAt = DateTime.Now.AddHours(-1);
                    db.SaveChanges();

                    // Start tilanteessa luotua timesheet riviä muokataan nyt stop vaiheessa
                    Timesheet ts = db.Timesheets.Where(ts => ts.IdWorkAssignment == wa.IdWorkAssignment).FirstOrDefault();
                    ts.Comments = op.Comment;
                    ts.Latitude = op.Latitude;
                    ts.StopTime = DateTime.Now.AddHours(-1);
                    ts.Longitude = op.Longitude;

                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return(false);
            }
        }


    }
}
