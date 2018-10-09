using Microsoft.AspNet.Identity;
using Paintball.Web.Constants;
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
    [Authorize(Roles = "Admin, CompanyOwner")]
    [Route("papi/users")]
    public class UsersApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public UsersApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: papi/users?pageNumber=1&pageSize=20&descending=false
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
            var result = await _manager.GetUsers(_pageSize, _pageNumber, _descending);

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: papi/users/5
        [Route("papi/users/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(string id)
        {
            SetPrincipal();

            var result = await _manager.ReadUser(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: papi/users
        [HttpPost]
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]CreateStaffViewModel vm)
        {
            if (vm != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                var result = await _manager.CreateUser(vm);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: papi/users/5
        [Route("papi/users/{id}")]
        [HttpPut]
        [AcceptVerbs("PUT")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(string id,[FromBody]UpdateStaffViewModel vm)
        {
            if (vm != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                vm.UserName = id;
                var result = await _manager.UpdateUser(vm);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: papi/users/5
        [Route("papi/users/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(string id)
        {
            SetPrincipal();
            var result = await _manager.DeleteUser(id);
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
