using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>(TIdentifier id)
    : AggregateRoot<TIdentifier>(id),
        IValidationTarget<TDeriviedAggregateRoot, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedAggregateRoot)this);

            return field;
        }
    } = new();
}
