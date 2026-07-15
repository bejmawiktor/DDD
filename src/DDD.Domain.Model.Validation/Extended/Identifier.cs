using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class Identifier<
    TIdentifierValue,
    TDeriviedIdentifier,
    TValidator,
    TValidationSource
>(TIdentifierValue value)
    : Identifier<TIdentifierValue, TDeriviedIdentifier>(value),
        IValidationTarget<TDeriviedIdentifier, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<
            TIdentifierValue,
            TDeriviedIdentifier,
            TValidator,
            TValidationSource
        >
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
    where TValidationSource : new()
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
