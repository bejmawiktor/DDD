using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>
    : Model.Identifier<TIdentifierValue, TDeriviedIdentifier>,
        IValidationTarget<TDeriviedIdentifier, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
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
