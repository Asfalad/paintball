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
    [Route("papi/equipment")]
    public class EquipmentApiController : ApiController
    {
        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        private IManager _manager;
        public EquipmentApiController(IManager manager)
        {
            _manager = manager;
        }
        // GET: api/EquipmentApi
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
            var result = await _manager.GetEquipment(_pageSize, _pageNumber, _descending);

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/EquipmentApi/5
        [Route("papi/equipment/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadEquipment(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/EquipmentApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]Equipment equipment)
        {
            if(equipment != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                var result = await _manager.CreateEquipment(equipment);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: api/EquipmentApi/5
        [Route("papi/equipment/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Equipment equipment)
        {
            if(equipment != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                equipment.Id = id;
                var result = await _manager.UpdateEquipment(equipment);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: api/EquipmentApi/5
        [Route("papi/equipment/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteEquipment(id);
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
