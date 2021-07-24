using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagement.Api.Controllers
{
    /// <summary>
    /// IsAliveController is for checking that the server is alive and accessible from client
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IsAliveController : ControllerBase
    {

        /// <summary>
        /// By calling this method successfuly it can be sured that the server is alive and on  
        /// </summary>
        /// <response code="200">Return nothing</response>
        [HttpHead]
        [ProducesResponseType(typeof(DateTime), 200)]
        public ActionResult<DateTime> Head()
        {
            return Ok();
        }
    }
}
