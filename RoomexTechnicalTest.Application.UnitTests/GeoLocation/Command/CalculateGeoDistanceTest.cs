using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance;
using RoomexTechnicalTest.Application.Models.GeoLocation;
using Xunit;

namespace RoomexTechnicalTest.Application.UnitTests.GeoLocation.Command
{
    public class CalculateGeoDistanceTest
    {
        [Fact]
        public async Task Handle_CalculateDistance()
        {
            var handler = new CalculateGeoDistanceCommandHandler();

            var distance = await handler.Handle(new CalculateGeoDistanceCommand(
                new GeoCoordinate(53.297975, -6.372663),
                new GeoCoordinate(41.385101, -81.440440)
            ), CancellationToken.None);

            distance.Should().Be(5536.34);
        }
    }
}
