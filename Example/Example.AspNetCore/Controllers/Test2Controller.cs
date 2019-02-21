using System.Collections.Generic;
using Example.AspNetCore.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Example.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test2Controller : ControllerBase
    {
        [HttpPost]
        public void Post(Test2Command command)
        {
        }
    }
}
