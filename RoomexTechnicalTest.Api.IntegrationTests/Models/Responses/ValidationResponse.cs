using System.Collections.Generic;
using RoomexTechnicalTest.Application.Models.Validation;

namespace RoomexTechnicalTest.Api.IntegrationTests.Models.Responses {
    public record ValidationResponse(string Message, List<ValidationError> Details);
}