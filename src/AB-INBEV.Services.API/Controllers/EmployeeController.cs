using AB_INBEV.Application.EventSourcedNormalizers;
using AB_INBEV.Application.Interfaces;
using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Core.Interfaces;
using AB_INBEV.Domain.Core.Notifications;
using AB_INBEV.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AB_INBEV.Services.Api.Controllers
{
    [Authorize]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeAppService employeeAppService,
            INotificationHandler<DomainNotification> notifications,
            ILogger<EmployeeController> logger,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _employeeAppService = employeeAppService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<EmployeeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeeAppService.GetAll();
            return Response(employees);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Guid recebido: {@guid}", id);

            var employeeViewModel = await _employeeAppService.GetById(id);

            return Response(employeeViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanWriteEmployeeData", Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] EmployeeViewModel employeeViewModel)
        {
            _logger.LogInformation("Objeto recebido: {@employeeViewModel}", employeeViewModel);

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(employeeViewModel);
            }

            await _employeeAppService.Register(employeeViewModel);

            return Response(employeeViewModel);
        }

        [HttpPut]
        [Authorize(Policy = "CanWriteEmployeeData", Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromBody] EmployeeViewModel employeeViewModel)
        {
            _logger.LogInformation("Objeto recebido: {@employeeViewModel}", employeeViewModel);

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(employeeViewModel);
            }

            await _employeeAppService.Update(employeeViewModel);

            return Response(employeeViewModel);
        }

        [HttpDelete]
        [Authorize(Policy = "CanRemoveEmployeeData", Roles = Roles.Admin)]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Guid recebido: {@guid}", id);

            await _employeeAppService.Remove(id);

            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("history/{id:guid}")]
        [ProducesResponseType(typeof(IList<EmployeeHistoryData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> History(Guid id)
        {
            var employeeHistoryData = await _employeeAppService.GetAllHistory(id);
            return Response(employeeHistoryData);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("pagination")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Pagination(int skip, int take)
        {
            var employees = await _employeeAppService.GetAll(skip, take);
            return Response(employees);
        }
    }
}
