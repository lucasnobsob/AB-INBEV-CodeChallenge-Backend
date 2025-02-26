using System;
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
    [Route("employees")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeeController(
            IEmployeeAppService customerAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _employeeAppService = customerAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management")]
        public IActionResult Get()
        {
            return Response(_employeeAppService.GetAll());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var customerViewModel = _employeeAppService.GetById(id);

            return Response(customerViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanWriteCustomerData", Roles = Roles.Admin)]
        [Route("management")]
        public IActionResult Post([FromBody] EmployeeViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(customerViewModel);
            }

            _employeeAppService.Register(customerViewModel);

            return Response(customerViewModel);
        }

        [HttpPut]
        [Authorize(Policy = "CanWriteCustomerData", Roles = Roles.Admin)]
        [Route("management")]
        public IActionResult Put([FromBody] EmployeeViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(customerViewModel);
            }

            _employeeAppService.Update(customerViewModel);

            return Response(customerViewModel);
        }

        [HttpDelete]
        [Authorize(Policy = "CanRemoveCustomerData", Roles = Roles.Admin)]
        [Route("management")]
        public IActionResult Delete(Guid id)
        {
            _employeeAppService.Remove(id);

            return Response();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/history/{id:guid}")]
        public IActionResult History(Guid id)
        {
            var customerHistoryData = _employeeAppService.GetAllHistory(id);
            return Response(customerHistoryData);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("management/pagination")]
        public IActionResult Pagination(int skip, int take)
        {
            return Response(_employeeAppService.GetAll(skip, take));
        }
    }
}
