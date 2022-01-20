using MediatR;
using RoomexTechnicalTest.Application.Models.GeoLocation;

namespace RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance {
    public record CalculateGeoDistanceCommand(GeoCoordinate Origin, GeoCoordinate Destination) : IRequest<double>;
}