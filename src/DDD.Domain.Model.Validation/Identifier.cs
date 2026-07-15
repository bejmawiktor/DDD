using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>(TIdentifierValue value)
    : Identifier<TIdentifierValue, TDeriviedIdentifier>(value),
        IValidationTarget<TDeriviedIdentifier, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedIdentifier)this);

            return field;
        }
    } = new();
}
