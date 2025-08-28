using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class AggregateRoot<
    TIdentifier,
    TDeriviedAggregateRoot,
    TValidator,
    TValidationSource
> : AggregateRoot<TIdentifier>, IValidationTarget<TDeriviedAggregateRoot, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<
            TIdentifier,
            TDeriviedAggregateRoot,
            TValidator,
            TValidationSource
        >
    where TValidationSource : new()
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
