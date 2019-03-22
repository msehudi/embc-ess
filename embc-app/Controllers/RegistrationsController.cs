using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface dataInterface;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;
        private readonly IUrlHelper urlHelper;

        public RegistrationsController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IDataInterface dataInterface,
            IUrlHelper urlHelper
        )
        {
            Configuration = configuration;
            this.dataInterface = dataInterface;
            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(RegistrationsController));
            this.env = env;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAll))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters queryParameters)
        {
            try
            {
                var results = await dataInterface.GetRegistrationsAsync(queryParameters);

                var toReturn = await PaginatedList<ViewModels.Registration>.CreateAsync(results, queryParameters.Offset, queryParameters.Limit);

                // TODO: provide values for pagination metadata...
                var paginationMetadata = new PaginationMetadata()
                {
                    CurrentPage = toReturn.GetCurrentPage(),
                    PageSize = toReturn.Limit,
                    TotalCount = toReturn.TotalItemCount,
                    TotalPages = toReturn.GetTotalPages()
                };

                return Json(new
                {
                    data = toReturn,
                    metadata = paginationMetadata
                });
            }
            catch (Exception error)
            {
                // TODO: Remove error payload when live in PROD
                return BadRequest(error);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await dataInterface.GetRegistrationAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ViewModels.Registration item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (item != null && item.Id != null) item.Id = null;
                var result = await dataInterface.CreateRegistrationAsync(item);
                return Json(result);
            }
            catch (Exception error)
            {
                return BadRequest(error.ToString());
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            if (id != null && item.Id != null && id != item.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await dataInterface.UpdateRegistrationAsync(item);
                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
            }
        }
    }
}
