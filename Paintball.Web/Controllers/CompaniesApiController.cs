using Microsoft.AspNet.Identity;
using Paintball.DAL.Entities;
using Paintball.Web.Manager;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Paintball.Web.Controllers
{
    [Authorize]
    [Route("papi/companies")]
    public class CompaniesApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }

        public CompaniesApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: api/CompaniesApi
        [Authorize(Roles = "Admin")]
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
            var result = await _manager.GetCompanies(_pageSize, _pageNumber, _descending);

            if (result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: api/CompaniesApi/5
        [Authorize(Roles = "CompanyOwner, CompanyStaff")]
        [Route("papi/companies/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();

            var result = await _manager.ReadCompany(id);

            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/CompaniesApi
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]Company company)
        {
            if (company != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                var result = await _manager.CreateCompany(company);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: api/CompaniesApi/5
        [Authorize(Roles = "CompanyOwner, CompanyModify")]
        [Route("papi/companies/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Company company)
        {
            if (company != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                company.Id = id;
                var result = await _manager.UpdateCompany(company);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: api/CompaniesApi/5
        [Authorize(Roles = "CompanyOwner")]
        [Route("papi/companies/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteCompany(id);
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
