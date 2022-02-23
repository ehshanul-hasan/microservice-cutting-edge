using Catalog.API.Model.Entity;
using Catalog.API.Services;
using Common.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/package")]
    [Produces("application/json")]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageService _packageService;
        public PackageController(IPackageService packageService, ILogger<PackageController> logger)
        {
            _packageService = packageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Package package)
        {
            var result = await _packageService.CreateAsync(package);

            return result.ToOkResult("Package Created successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _packageService.GetByIdAsync(id);

            return result.ToOkResult();
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var result = await _packageService.ListAsync();
            return result.ToOkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _packageService.DeleteAsync(id);
            return result.ToOkResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Package request)
        {
            request.Id = id;
            var result = await _packageService.UpdateAsync(request);
            return result.ToOkResult("Package updated successfully");
        }
    }
}