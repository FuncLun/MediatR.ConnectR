using Example.AspNetCore.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Example.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test1Controller : ControllerBase
    {
        //[HttpGet]
        //public ActionResult<Test1Response> Get(string dataIn, TestDto testDto)
        //{
        //    return new Test1Response()
        //    {
        //        DataOut = dataIn,
        //    };
        //}

        [HttpPost]
        public ActionResult<Test1Response> Post([FromBody]Test1Query query)
        {
            return new Test1Response()
            {
                DataOut = query.DataIn,
            };
        }
    }
}
