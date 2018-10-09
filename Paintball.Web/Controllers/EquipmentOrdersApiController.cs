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
    [Route("papi/equipmentorders")]
    public class EquipmentOrdersApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public EquipmentOrdersApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: api/EquipmentOrderApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int? gameId, int? pageNumber = null,
            int? pageSize = null,
            bool? descending = null)
        {
            if(gameId != null && gameId.HasValue)
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
                var result = await _manager.GetEquipmentOrders(gameId.Value, _pageSize, _pageNumber, _descending);

                if (result.Result == true)
                {
                    return Ok(new { count = result.Count, data = result.MultipleResult });
                }
            }
            return BadRequest();
        }

        // GET: api/EquipmentOrderApi/5
        [Route("papi/equipmentorders/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadEquipmentOrder(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/EquipmentOrderApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]EquipmentOrder order)
        {
            if (order != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();
                
                var result = await _manager.CreateEquipmentOrder(order);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: api/EquipmentOrderApi/5
        [Route("papi/equipmentorders/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]EquipmentOrder order)
        {
            if (order != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                order.Id = id;
                var result = await _manager.UpdateEquipmentOrder(order);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: api/EquipmentOrderApi/5
        [Route("papi/equipmentorders/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteEquipmentOrder(id);
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
