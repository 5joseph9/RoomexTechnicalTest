namespace RoomexTechnicalTest.Application.Models.Validation {
    public record ValidationError(string PropertyName, string ErrorMessage, object AttemptedValue);
}