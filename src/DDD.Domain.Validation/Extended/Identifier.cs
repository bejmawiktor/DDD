using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class Identifier<
    TDeriviedIdentifier,
    TIdentifierValue,
    TValidator,
    TValidationSource
>
    : Model.Identifier<TIdentifierValue, TDeriviedIdentifier>,
        IValidationTarget<TDeriviedIdentifier, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<
            TDeriviedIdentifier,
            TIdentifierValue,
            TValidator,
            TValidationSource
        >
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
    where TValidationSource : new()
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TDeriviedIdentifier)this);

            return this.validator;
        }
    }

    protected Identifier(TIdentifierValue value)
        : base(value) { }
}
