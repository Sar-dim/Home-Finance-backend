using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Interfaces;
using System;
using System.Security.Claims;

namespace net_core_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OperationController : Controller
    {
        private readonly IOperationService _operationService;
        private readonly IBaseRepository<Operation> _operations;

        public OperationController(IOperationService operationService, IBaseRepository<Operation> operation)
        {
            _operationService = operationService;
            _operations = operation;
        }

        /// <summary>
        /// Return all operations.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("getall")]
        public JsonResult Get()
        {
            return new JsonResult(_operations.GetAll());
        }

        /// <summary>
        /// Return operations for a date/for the period between dates.
        /// </summary>
        /// <param Person="PersonId"></param>  
        /// <param First_date_or_one_date="dateFirst"></param> 
        /// <param Second_date="dateSecond"></param> 
        /// <response code="200">Returns the item/items</response>
        /// <response code="400">If the item is missing</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("get")]
        public JsonResult Get([FromQuery] OperationRequest request)
        {
            return new JsonResult(_operationService.GetOperationForTime(request));
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
        ///        "source": "Gas",
        ///        "amount": 100,
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Post(OperationModel operation)
        {
            _operationService.AddOperation(operation, HttpContext.User.Identity as ClaimsIdentity);
            return new JsonResult("Operation was successfully added");
        }

        /// <summary>
        /// Update operation.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///        "Id": "94dc3f0c-6d1e-414c-869a-4b07a545780e"
        ///        "type": 0,
        ///        "source": "Gas",
        ///        "amount": 100,
        ///        "personId": "8b6e30c3-1ff6-410d-bdd5-968d98194b1c"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Put(OperationModel operation)
        {
            _operationService.UpdateOperation(operation, HttpContext.User.Identity as ClaimsIdentity);
            return new JsonResult("Operation was successfully updated");
        }

        /// <summary>
        /// Delete operation.
        /// </summary>
        /// <param Id="Id"></param>  
        /// <response code="200">Returns the confirmation.</response>
        /// <response code="400">If the item is null</response> 
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Delete([FromQuery] Guid id)
        {
            _operationService.DeleteOperation(id, HttpContext.User.Identity as ClaimsIdentity);
            return new JsonResult("Operation was successfully deleted");
        }
    }
}
