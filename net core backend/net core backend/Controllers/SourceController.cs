using Common.Models;
using Domain.Entity;
using Infractructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Repositories.Interfaces;
using System;

namespace net_core_backend.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SourceController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly IBaseRepository<OperationSource> _source;

        public SourceController(ISourceService sourceService, IBaseRepository<OperationSource> source)
        {
            _source = source;
            _sourceService = sourceService;
        }

        /// <summary>
        /// Return all sources.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("getall")]
        public JsonResult Get()
        {
            return new JsonResult(_source.GetAll());
        }

        /// <summary>
        /// Return source by name.
        /// </summary>
        /// <param Source_name = "name"></param>  
        /// <response code="200">Returns the item/items</response>
        /// <response code="400">If the item is missing</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("get")]
        public JsonResult Get(string name)
        {
            return new JsonResult(_sourceService.GetSource(name));
        }

        /// <summary>
        /// Add operation.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "type": 0,
        ///        "name": "Gas"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Post(SourceModel sourceModel)
        {
            _sourceService.AddSource(sourceModel);
            return new JsonResult("Source was successfully added");
        }

        /// <summary>
        /// Update source.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///        "Id": "94dc3f0c-6d1e-414c-869a-4b07a545780e"
        ///        "type": 0,
        ///        "name": "Gas",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Put(SourceModel sourceModel)
        {
            _sourceService.UpdateSource(sourceModel);
            return new JsonResult("Source was successfully updated");
        }

        /// <summary>
        /// Delete source.
        /// </summary>
        /// <param Id="Id"></param>  
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Delete([FromQuery] Guid id)
        {
            _sourceService.DeleteSource(id);
            return new JsonResult("Operation was successfully deleted");
        }
    }
}
