namespace DDD.Model
{
    public interface IValidator<TValidatedObject>
    {
        void Validate(TValidatedObject validatedObject);
    }
}