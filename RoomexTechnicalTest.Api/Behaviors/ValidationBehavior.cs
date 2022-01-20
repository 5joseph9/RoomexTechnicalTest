using FluentValidation;
using MediatR;

namespace RoomexTechnicalTest.Api.Behaviors {
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse> {
        private readonly IValidator<TRequest> _validator;
        public ValidationBehavior(IValidator<TRequest> validators) => _validator = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await _validator.ValidateAsync(context, cancellationToken);

            if (validationResult.Errors.Any()) {
                throw new ValidationException("One or more validation errors occurred.", validationResult.Errors);
            }

            return await next();
        }
    }
}