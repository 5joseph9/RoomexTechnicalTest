using MediatR;
using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance;

namespace RoomexTechnicalTest.Application {
    public static class ApplicationServiceRegistration {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<CalculateGeoDistanceCommand>, CalculateGeoDistanceValidator>();

            return services;
        }
    }
}