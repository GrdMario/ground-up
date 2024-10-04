namespace GroundUp.Api.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class RequiredNotEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is string ? !string.IsNullOrEmpty((string)value) : base.IsValid(value);
        }
    }
}
