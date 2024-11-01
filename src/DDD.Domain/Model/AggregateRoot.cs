using System;

namespace DDD.Domain.Model;

public abstract class AggregateRoot<TIdentifier>(TIdentifier id)
    : Entity<TIdentifier>(id),
        IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier> { }

public abstract class AggregateRoot<TIdentifier, TValidatedObject, TValidator>
    : AggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
    where TValidator : IValidator<TValidatedObject>, new()
{
    protected TValidator Validator { get; }

    protected AggregateRoot(TIdentifier id, TValidatedObject validatedObject)
        : base(id)
    {
        this.Validator = new TValidator();

        this.Validator.Validate(validatedObject);
    }
}
