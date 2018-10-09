using Microsoft.AspNet.Identity;
using Paintball.DAL.Entities;
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
    [Authorize(Roles = "CompanyStaff, CompanyOwner")]
    [Route("papi/games")]
    public class GamesApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public GamesApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: api/GamesApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int? pageNumber = null,
            int? pageSize = null,
            bool? descending = null,
            DateTime? date = null)
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
            OperationResult<Game> result = null;
            if(date != null && date.HasValue)
            {
                result = await _manager.GetGames(date.Value);
            }
            else
            {
                result = await _manager.GetGames(_pageSize, _pageNumber, _descending);
            }

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: api/GamesApi/5
        [Route("papi/games/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadGame(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/GamesApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]Game game)
        {
            if (game != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                var result = await _manager.CreateGame(game);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: api/GamesApi/5
        [Route("papi/games/{id}")]

        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Game game)
        {
            if (game != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                game.Id = id;
                var result = await _manager.UpdateGame(game);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: api/GamesApi/5
        [Route("papi/games/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteGame(id);
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
