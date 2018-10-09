using Microsoft.AspNet.Identity;
using Paintball.DAL.Entities;
using Paintball.Web.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Paintball.Web.Controllers
{
    [Authorize]
    [Route("papi/order")]
    public class OrderApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public OrderApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: api/OrderApi/5
        [Route("papi/order/{id}")]
        [HttpGet]
        [AcceptVerbs("GET")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get([FromUri]int id)
        {
            SetPrincipal();
            var response = await _manager.GetCompanyForOrder(id);
            if(response.Result == true)
            {
                return Ok(response.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
