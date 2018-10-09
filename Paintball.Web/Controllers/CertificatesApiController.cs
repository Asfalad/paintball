using Paintball.Web.Manager;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Paintball.DAL.Entities;
using System;
using System.Security.Claims;
using System.Net.Http;

namespace Paintball.Web.Controllers
{
    [Authorize]
    [Route("papi/certificates")]
    public class CertificatesApiController : ApiController
    {
        private IManager _manager;

        private void SetPrincipal()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            _manager.SetUser(principal.Identity.GetUserId());
        }
                
        public CertificatesApiController(IManager manager)
        {
            _manager = manager;
        }

        // GET: papi/certificates?pageNumber=1&pageSize=20&descending=false
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
            if(pageNumber.HasValue && pageNumber.Value > 0)
            {
                _pageNumber = pageNumber.Value;
            }
            if (descending.HasValue)
            {
                _descending = descending.Value;
            }
            var result = await _manager.GetCertificates(_pageSize, _pageNumber, _descending);

            if(result.Result == true)
            {
                return Ok(new { count = result.Count, data = result.MultipleResult });
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: papi/certificates/5
        [Route("papi/certificates/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Get(int id)
        {
            SetPrincipal();
            
            var result = await _manager.ReadCertificate(id);
            
            if (result.Result == true)
            {
                return Ok(result.SingleResult);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: papi/certificates
        public async System.Threading.Tasks.Task<IHttpActionResult> Post([FromBody]Certificate certificate)
        {
            if(certificate != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SetPrincipal();

                certificate.StartDate = DateTime.Now;

                var result = await _manager.CreateCertificate(certificate);
                if (result.Result == true)
                {
                    return Ok(result.SingleResult);
                }
            }
            return BadRequest();
        }

        // PUT: papi/certificates/5
        [Route("papi/certificates/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Put(int id, [FromBody]Certificate certificate)
        {
            if(certificate != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                SetPrincipal();

                certificate.Id = id;
                var result = await _manager.UpdateCertificate(certificate);
                if (result.Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        // DELETE: papi/certificates/5
        [Route("papi/certificates/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete(int id)
        {
            SetPrincipal();
            var result = await _manager.DeleteCertificate(id);
            if(result.Result == true)
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
