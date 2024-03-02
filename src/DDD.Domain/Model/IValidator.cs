namespace DDD.Domain.Model
{
    public interface IValidator<TValidatedObject>
    {
        void Validate(TValidatedObject validatedObject);
    }
}
