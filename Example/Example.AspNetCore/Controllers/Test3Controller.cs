using System.Collections.Generic;
using Example.AspNetCore.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Example.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test3Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult<Unit> Get(string dataIn, TestDto testDto)
        {
            return Unit.Value;
        }
    }
}
