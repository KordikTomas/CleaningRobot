using System.Web.Http;
using CleaningRobot.Json;

namespace CleaningRobot.WebApi.Controllers
{
    public class CleaningRobotController : ApiController
    {
        public IHttpActionResult Post(JsonInput input)
        {
            try
            {
                var robotController = new JsonController();
                var result = robotController.Execute(input);
                return Json(result);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
