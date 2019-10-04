using System.Runtime.CompilerServices;

namespace DDD.Model
{
    public interface IValidator<TValidatedMembersTuple>
        where TValidatedMembersTuple : ITuple
    {
        void Validate(TValidatedMembersTuple validatedMembers);
    }
}