using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RoomexTechnicalTest.Api.IntegrationTests.Models.Responses;
using RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance;
using RoomexTechnicalTest.Application.Models.GeoLocation;
using RoomexTechnicalTest.Application.Models.Validation;
using Xunit;

namespace RoomexTechnicalTest.Api.IntegrationTests.Controllers {
    public class GeoLocationControllerTests {
        const string GEO_DISTANCE_END_POINT = "/api/v1/GeoLocation/GeoDistanceInKm";
        private readonly HttpClient _client;

        public GeoLocationControllerTests() { _client = new WebApplicationFactory<Program>().CreateClient(); }

        [Fact]
        public async Task ReturnsSuccessResult() {
            var calculateGeoDistanceCommand = new CalculateGeoDistanceCommand(
                new GeoCoordinate(53.297975, -6.372663),
                new GeoCoordinate(41.385101, -81.440440)
            );

            var response = await _client.PostAsJsonAsync(GEO_DISTANCE_END_POINT, calculateGeoDistanceCommand);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<double>();

            content.Should().Be(5536.34);
        }

        [Fact]
        public async Task ReturnsBadRequestResult_InvalidLatitude() {
            var calculateGeoDistanceCommand = new CalculateGeoDistanceCommand(
                new GeoCoordinate(200, -6.372663),
                new GeoCoordinate(220, -81.440440)
            );

            var expectedOriginGeoCoordinates = new ValidationError("Origin.Latitude",
                CalculateGeoDistanceValidator.LATITUDE_ERROR_MESSAGE, 200);

            var expectedDestinationGeoCoordinates = new ValidationError("Destination.Latitude",
                CalculateGeoDistanceValidator.LATITUDE_ERROR_MESSAGE, 220);

            var response = await _client.PostAsJsonAsync(GEO_DISTANCE_END_POINT, calculateGeoDistanceCommand);
            var validationResponse = JsonConvert.DeserializeObject<ValidationResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            validationResponse.Details.Count.Should().Be(2);
            validationResponse.Details.First().Should().BeEquivalentTo(expectedOriginGeoCoordinates);
            validationResponse.Details.Last().Should().BeEquivalentTo(expectedDestinationGeoCoordinates);
        }

        [Fact]
        public async Task ReturnsBadRequestResult_InvalidLongitude() {
            var calculateGeoDistanceCommand = new CalculateGeoDistanceCommand(
                new GeoCoordinate(53.297975, -200),
                new GeoCoordinate(41.385101, -220)
            );

            var expectedOriginGeoCoordinates = new ValidationError("Origin.Longitude",
                CalculateGeoDistanceValidator.LONGITUDE_ERROR_MESSAGE, -200);

            var expectedDestinationGeoCoordinates = new ValidationError("Destination.Longitude",
                CalculateGeoDistanceValidator.LONGITUDE_ERROR_MESSAGE, -220);

            var response = await _client.PostAsJsonAsync(GEO_DISTANCE_END_POINT, calculateGeoDistanceCommand);
            var validationResponse = JsonConvert.DeserializeObject<ValidationResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            validationResponse.Details.Count.Should().Be(2);
            validationResponse.Details.First().Should().BeEquivalentTo(expectedOriginGeoCoordinates);
            validationResponse.Details.Last().Should().BeEquivalentTo(expectedDestinationGeoCoordinates);
        }
    }
}