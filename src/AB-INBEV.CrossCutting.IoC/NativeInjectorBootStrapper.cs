using AB_INBEV.Domain.Commands;
using AB_INBEV.Domain.Core.Notifications;
using AB_INBEV.Infra.CrossCutting.Bus;
using AB_INBEV.Infra.CrossCutting.Identity.Services;
using AB_INBEV.Domain.EventHandlers;
using AB_INBEV.Domain.Events;
using AB_INBEV.Infra.CrossCutting.Identity.Authorization;
using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Infra.CrossCutting.Identity.Models;
using AB_INBEV.Domain.Core.Events;
using AB_INBEV.Infra.Data.Repository.EventSourcing;
using AB_INBEV.Domain.CommandHandlers;
using AB_INBEV.Infra.Data.UoW;
using AB_INBEV.Infra.Data.EventSourcing;
using AB_INBEV.Application.Interfaces;
using AB_INBEV.Application.Services;
using AB_INBEV.Infra.Data.Repository;
using AB_INBEV.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using AB_INBEV.Domain.Core.Interfaces;


namespace AB_INBEV.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddHttpContextAccessor();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<IEmployeeAppService, EmployeeAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<EmployeeRegisteredEvent>, EmployeeEventHandler>();
            services.AddScoped<INotificationHandler<EmployeeUpdatedEvent>, EmployeeEventHandler>();
            services.AddScoped<INotificationHandler<EmployeeRemovedEvent>, EmployeeEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<RegisterNewEmployeeCommand, bool>, EmployeeCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateEmployeeCommand, bool>, EmployeeCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveEmployeeCommand, bool>, EmployeeCommandHandler>();

            // Domain - 3rd parties
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IMailService, MailService>();

            // Infra - Data
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
        }
    }
}
