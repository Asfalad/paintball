using Paintball.Web.Providers;
using Paintball.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Paintball.Web.Controllers
{
    [Authorize]
    [Route("papi/images")]
    public class ImageApiController : ApiController
    {
        private ILoggingService _logger;
        private IList<string> AllowedFileExtensions = new List<string>() { ".jpg", ".gif", ".png" };

        public ImageApiController(ILoggingService logger)
        {
            _logger = logger;
        }
        // POST: papi/images
        public async System.Threading.Tasks.Task<IHttpActionResult> Post()
        {
            try
            {
                var request = this.Request;
                if (!request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = System.Web.HttpContext.Current.Server.MapPath("~/Images");
                var provider = new CustomMultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);

                string localName = provider.FileData.FirstOrDefault().LocalFileName;

                localName = "/images/" + localName.Replace(root, "");
                
                return Ok(new { name = localName });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
            return BadRequest();
        }
    }
}
