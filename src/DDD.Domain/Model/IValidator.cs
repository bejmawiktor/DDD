namespace DDD.Domain.Model
{
    public interface IValidator<in TValidatedObject>
    {
        void Validate(TValidatedObject validatedObject);
    }
}
