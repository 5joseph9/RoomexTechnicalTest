using FluentValidation;
using RoomexTechnicalTest.Application.Models.GeoLocation;

namespace RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance {
    public class CalculateGeoDistanceValidator : AbstractValidator<CalculateGeoDistanceCommand> {
        public const string LATITUDE_ERROR_MESSAGE = "Latitude must be greater or equal -90 and less or equal 90";
        public const string LONGITUDE_ERROR_MESSAGE = "Longitude must be greater or equal -180 and less or equal 180";

        public CalculateGeoDistanceValidator() {
            RuleFor(p => p.Origin.Latitude)
                .Must(GeoCoordinate.IsValidLatitude)
                .WithMessage(LATITUDE_ERROR_MESSAGE);

            RuleFor(p => p.Origin.Longitude)
                .Must(GeoCoordinate.IsValidLongitude)
                .WithMessage(LONGITUDE_ERROR_MESSAGE);

            RuleFor(p => p.Destination.Latitude)
                .Must(GeoCoordinate.IsValidLatitude)
                .WithMessage(LATITUDE_ERROR_MESSAGE);

            RuleFor(p => p.Destination.Longitude)
                .Must(GeoCoordinate.IsValidLongitude)
                .WithMessage(LONGITUDE_ERROR_MESSAGE);
        }
    }
}