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
    [Route("papi/events")]
    public class EventsApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public EventsApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: papi/events?pageNumber=1&pageSize=20&descending=false
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(
            int? pageNumber = null,
            int? pageSize = null,
            bool? descending = null)
        {
            SetPrincipal();
            int _pageSize = 20, _pageNumber = 1;
            bool _descending = false;
            if (pageSize.HasValue && pageSize.Value > 0)
            {
                _pageSize = pageSize.Value;
            }
            if (pageNumber.HasValue && pageNumber.Value > 0)
            {
                _pageNumber = pageNumber.Value;
            }
            if (descending.HasValue)
            {
                _descending = descending.Value;
            }
            var result = await _manager.GetEvents(_pageSize, _pageNumber, _descending);

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: papi/events/5
        [Route("papi/events/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadEvent(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: papi/events
        public async System.Threading.Tasks.Task<IHttpActionResult> Post(Event e)
        {
            if(e != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                var result = await _manager.CreateEvent(e);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: papi/events/5
        [Route("papi/events/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Event e)
        {
            if(e != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                e.Id = id;
                var result = await _manager.UpdateEvent(e);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: papi/events/5
        [Route("papi/events/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteEvent(id);
            if (result.Result == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
