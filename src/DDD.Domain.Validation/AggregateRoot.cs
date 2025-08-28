using DDD.Domain.Model;
using Utils.Validation;

namespace DDD.Domain.Validation;

public class AggregateRoot<TDeriviedAggregateRoot, TIdentifier, TValidator, TValidationSource>
    : AggregateRoot<TIdentifier>,
        IValidationTarget<TDeriviedAggregateRoot, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<
            TDeriviedAggregateRoot,
            TIdentifier,
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
