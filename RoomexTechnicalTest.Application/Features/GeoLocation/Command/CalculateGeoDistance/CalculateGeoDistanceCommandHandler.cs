using MediatR;
using RoomexTechnicalTest.Application.Models.GeoLocation;

namespace RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance {
    public class CalculateGeoDistanceCommandHandler : IRequestHandler<CalculateGeoDistanceCommand, double> {
        const double EATRH_RADIUS_IN_KM = 6371;

        public Task<double> Handle(CalculateGeoDistanceCommand request, CancellationToken cancellationToken) => Task.FromResult(CalculateGeoDistance(request.Origin, request.Destination));

        private double CalculateGeoDistance(GeoCoordinate origin, GeoCoordinate destination) {
            var dLat = DegreeToRadian(destination.Latitude - origin.Latitude);
            var dLon = DegreeToRadian(destination.Longitude - origin.Longitude);

            // Haversine Formula
            var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                    Math.Cos(DegreeToRadian(origin.Latitude)) * Math.Cos(DegreeToRadian(destination.Latitude)) *
                    Math.Pow(Math.Sin(dLon / 2), 2);

            var c = 2 * Math.Asin(Math.Sqrt(a));

            return Math.Round(c * EATRH_RADIUS_IN_KM, 2);
        }

        private double DegreeToRadian(double degree) => degree * Math.PI / 180;
    }
}