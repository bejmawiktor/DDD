using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>
    : AggregateRoot<TIdentifier>,
        IValidationTarget<TDeriviedAggregateRoot, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TDeriviedAggregateRoot)this);

            return this.validator;
        }
    }

    protected AggregateRoot(TIdentifier id)
        : base(id) { }
}
