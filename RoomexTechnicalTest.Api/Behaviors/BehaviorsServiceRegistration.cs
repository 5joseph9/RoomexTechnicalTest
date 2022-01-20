using MediatR;

namespace RoomexTechnicalTest.Api.Behaviors {
    public static class BehaviorsServiceRegistration {
        public static IServiceCollection AddValidationBehaviorService(this IServiceCollection services) {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}