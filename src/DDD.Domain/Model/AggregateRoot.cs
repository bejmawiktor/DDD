using System;

namespace DDD.Domain.Model;

public abstract class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    protected AggregateRoot(TIdentifier id)
        : base(id) { }
}

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
