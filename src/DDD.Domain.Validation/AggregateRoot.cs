using DDD.Domain.Model;
using Utils.Validation;

namespace DDD.Domain.Validation;

public class AggregateRoot<TAggregateRoot, TIdentifier, TValidator, TValidationSource>
    : AggregateRoot<TIdentifier>,
        IValidationTarget<TAggregateRoot, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TAggregateRoot>, new()
    where TAggregateRoot : AggregateRoot<TAggregateRoot, TIdentifier, TValidator, TValidationSource>
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TAggregateRoot)this);

            return this.validator;
        }
    }

    protected AggregateRoot(TIdentifier id)
        : base(id) { }
}
