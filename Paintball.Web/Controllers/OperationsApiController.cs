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
    [Route("papi/operations")]
    public class OperationsApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public OperationsApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: api/operations
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
            var result = await _manager.GetOperations(_pageSize, _pageNumber, _descending);

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: papi/operations/5
        [Route("papi/operations/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadOperation(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: papi/operations
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]Operation operation)
        {
            if (operation != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                operation.Date = DateTime.Now;

                var result = await _manager.CreateOperation(operation);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: papi/operations/5
        [Route("papi/operations/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Operation operation)
        {
            if (operation != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                operation.Id = id;
                var result = await _manager.UpdateOperation(operation);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: papi/operations/5
        [Route("papi/operations/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteOperation(id);
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
