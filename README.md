# DDD

[![Build and test](https://github.com/bejmawiktor/DDD/actions/workflows/build-test.yml/badge.svg)](https://github.com/bejmawiktor/DDD/actions/workflows/build-test.yml)

A set of .NET (net10.0) libraries providing tactical Domain-Driven Design patterns: entities, aggregates, value objects, domain events, plus an optional validation layer and ASP.NET Core / MediatR integrations.

Example usage: https://github.com/bejmawiktor/Identity

## Packages

| Package | Description |
|---|---|
| `DDD.Domain` | Core library: `Entity`, `AggregateRoot`, `ValueObject`, `Identifier`, `Enumeration`, the domain events mechanism (`EventManager`, `IEvent`), and repository interfaces (`IRepository`, `IAsyncRepository`). |
| `DDD.Domain.Model.Validation` | Extends `DDD.Domain` with validation support for entities and aggregates built on `Utils.Validation` (`DomainObjectValidator`, plus `AggregateRoot`/`Entity`/`ValueObject`/`Identifier` variants with an attached validator). |
| `DDD.Domain.Model.Validation.AspNetCore` | Integrates domain validation results with ASP.NET Core (converts validation errors/`Result` into MVC responses). |
| `DDD.Domain.Events.MediatR` | MediatR-based domain event dispatcher — publishes domain events as MediatR notifications. |
| `DDD.Domain.Events.AspNetCore` | `WebApplication` extension for configuring the `CompositeEventDispatcher` on ASP.NET Core startup. |
| `DDD.Application` | Application layer for hexagonal architecture: domain DTOs (`IDomainObjectDto`, `IAggregateRootDto`), DTO converters, and repository adapters (`IRepositoryAdapter`, `IAsyncRepositoryAdapter`). |

Each project is published as an independent NuGet package.

## Key concepts

### Domain model (`DDD.Domain`)

- **`Entity<TIdentifier>`** / **`AggregateRoot<TIdentifier>`** — objects with identity; equality is based solely on type and `Id`.
- **`ValueObject`** / **`ValueObject<TValue>`** — value objects with structural equality (based on `GetEqualityMembers`).
- **`Identifier<TIdentifierValue, TDerivedIdentifier>`** — strongly-typed identifiers built on top of `ValueObject`.
- **`Enumeration<TValue, TEnumeration>`** — typed enumerations with implicit conversion to/from their underlying value.
- **Domain events** — `Event`/`IEvent` represent an event; `EventManager` (a singleton, scoped via `EventsScope`) collects events and dispatches them through a configured `IEventDispatcher`/`ICompositeEventDispatcher`.
- **Repositories** — `IRepository<TAggregateRoot, TIdentifier>` and `IAsyncRepository<TAggregateRoot, TIdentifier>` define the contract for accessing aggregates.

### Validation (`DDD.Domain.Model.Validation`)

`AggregateRoot`/`Entity`/`ValueObject`/`Identifier` variants that take an extra `TValidator` generic parameter deriving from `DomainObjectValidator<TValidator, TTarget>`, letting you define validation rules for a domain object using `Utils.Validation`.

### Domain events + MediatR/ASP.NET Core

`DDD.Domain.Events.MediatR` provides an `EventDispatcher` that publishes events as `EventNotification<TEvent>` through `IMediator`. `DDD.Domain.Events.AspNetCore` exposes `UseCompositeEventDispatcher` to register dispatchers on application startup, e.g.:

```csharp
app.UseCompositeEventDispatcher((config, services) =>
    config.WithMediatRDispatcher(services.GetRequiredService<IMediator>()));
```

### Application layer (`DDD.Application`)

Supports mapping domain aggregates/entities to DTOs (`IDomainObjectDto`, `IAggregateRootDto`, converters) and repository adapters that bridge domain repositories with DTO-based repositories — in line with hexagonal architecture.

## Usage examples

### Value objects

```csharp
using DDD.Domain.Model;

public class Email : ValueObject<string>
{
    public Email(string value) : base(value) { }

    protected override void ValidateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("Invalid email address.", nameof(value));
    }
}

Email a = new("john@example.com");
Email b = new("john@example.com");

Console.WriteLine(a == b); // True — equality is structural
```

### Identifiers

```csharp
using DDD.Domain.Model;

public class CustomerId : Identifier<Guid, CustomerId>
{
    public CustomerId(Guid value) : base(value) { }

    protected override void ValidateValue(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Customer id cannot be empty.", nameof(value));
    }
}
```

### Entities and aggregate roots

```csharp
using DDD.Domain.Model;

public class Customer : AggregateRoot<CustomerId>
{
    public Email Email { get; private set; }

    public Customer(CustomerId id, Email email) : base(id)
    {
        this.Email = email;
    }

    public void ChangeEmail(Email newEmail) => this.Email = newEmail;
}
```

Two `Customer` instances are equal when they share the same runtime type and `Id` — field values are irrelevant for identity comparison.

### Enumerations

```csharp
using DDD.Domain.Model;

public class OrderStatus : Enumeration<string, OrderStatus>
{
    public static readonly OrderStatus Pending = new(nameof(Pending));
    public static readonly OrderStatus Shipped = new(nameof(Shipped));
    public static readonly OrderStatus Cancelled = new(nameof(Cancelled));

    protected override string DefaultValue => nameof(Pending);

    public OrderStatus() { }

    private OrderStatus(string value) : base(value) { }
}

OrderStatus status = "Shipped"; // implicit conversion from the underlying value
string value = OrderStatus.Pending; // implicit conversion back to the underlying value
```

### Domain events

Raise events from inside your aggregates and dispatch them once a unit of work completes, using `EventManager` and `EventsScope` to batch dispatch until the scope is disposed/published:

```csharp
using DDD.Domain.Events;

public class CustomerRegistered : Event
{
    public CustomerId CustomerId { get; }

    public CustomerRegistered(CustomerId customerId) => this.CustomerId = customerId;
}

// Configure a dispatcher once at startup.
EventManager.Instance.Dispatcher = myEventDispatcher;

// Batch events raised within a unit of work and publish them together.
using (EventsScope scope = new())
{
    EventManager.Instance.Notify(new CustomerRegistered(customer.Id));
    // ... more domain work, more events ...

    scope.Publish(); // or `await scope.PublishAsync();`
}
```

Without an active `EventsScope`, `Notify`/`NotifyAsync` dispatch events immediately.

### Repositories

```csharp
using DDD.Domain.Persistence;

public interface ICustomerRepository : IRepository<Customer, CustomerId> { }
```

### Domain validation (`DDD.Domain.Model.Validation`)

Define a validator describing the rules for an aggregate/entity/value object, then use it from the domain object through the inherited `Validator` property:

```csharp
using DDD.Domain.Model;
using DDD.Domain.Model.Validation;
using Utils.Functional;
using Utils.Validation;

public class CustomerValidator : DomainObjectValidator<CustomerValidator, Customer>
{
    public string? Name { get; set; }

    protected override CustomerValidator ValidationSource => this;

    public CustomerValidator()
    {
        this.Configuration.WithValidation(
            nameof(this.Name),
            source => string.IsNullOrWhiteSpace(source.Name)
                ? new ValidationError(nameof(this.Name), "Name is required.")
                : null
        );
    }

    protected override void UpdateSource(Customer validationTarget) =>
        this.Name = validationTarget.Name;
}

public class Customer : AggregateRoot<CustomerId, Customer, CustomerValidator>
{
    private string name;

    public string Name
    {
        get => this.name;
        set
        {
            this.Validator.Validate(nameof(this.Name), source => source.Name = value).ThrowIfFailed();
            this.name = value;
        }
    }

    public Customer(CustomerId id, string name) : base(id)
    {
        this.name = name;
    }
}
```

### ASP.NET Core: converting validation results to responses (`DDD.Domain.Model.Validation.AspNetCore`)

```csharp
using DDD.Domain.Validation.AspNetCore;

[HttpPost]
public IActionResult Register(RegisterCustomerRequest request)
{
    IResult<Customer, IError> result = this.customerService.Register(request);

    return result.ToActionResult(this.HttpContext);
}
```

A failed result is converted into a `ProblemDetails` response with the matching HTTP status code; a successful result becomes `200 OK` (optionally with the returned value serialized as the body).

### Dispatching domain events through MediatR (`DDD.Domain.Events.MediatR`)

```csharp
using DDD.Domain.Events.MediatR;

public class CustomerRegisteredHandler : IEventHandler<CustomerRegistered>
{
    public Task Handle(CustomerRegistered @event, CancellationToken cancellationToken)
    {
        // react to the event
        return Task.CompletedTask;
    }
}
```

### Wiring it up on ASP.NET Core startup (`DDD.Domain.Events.AspNetCore`)

```csharp
using DDD.Domain.Events.AspNetCore;
using DDD.Domain.Events.MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

app.UseCompositeEventDispatcher((config, services) =>
    config.WithMediatRDispatcher(services.GetRequiredService<IMediator>()));
```

### Application layer: DTOs, converters and repository adapters (`DDD.Application`)

```csharp
using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Application.Persistence;
using DDD.Application.Persistence.Adapters;
using DDD.Domain.Persistence;

public class CustomerDto : IAggregateRootDto<Customer, CustomerId>
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public Customer ToDomainObject() => new(new CustomerId(Guid.Parse(this.Id)), this.Name);
}

public class CustomerDtoConverter
    : IAggregateRootDtoConverter<Customer, CustomerId, CustomerDto, string>
{
    public CustomerDto ToDto(Customer customer) =>
        new() { Id = customer.Id.Value.ToString(), Name = customer.Name };

    public string ToDtoIdentifier(CustomerId identifier) => identifier.Value.ToString();
}

public interface ICustomerDtoRepository : IDtoRepository<CustomerDto, string> { }

// Bridges a DTO-based repository (e.g. backed by EF Core, a REST client, etc.)
// with the domain-facing `IRepository<Customer, CustomerId>` contract.
public class CustomerRepository
    : IRepositoryAdapter<
        CustomerDto,
        string,
        ICustomerDtoRepository,
        CustomerDtoConverter,
        Customer,
        CustomerId
    >,
        ICustomerRepository
{
    ICustomerDtoRepository IRepositoryAdapter<
        CustomerDto, string, ICustomerDtoRepository, CustomerDtoConverter, Customer, CustomerId
    >.DtoRepository => this.DtoRepository;

    private ICustomerDtoRepository DtoRepository { get; }

    public CustomerRepository(ICustomerDtoRepository dtoRepository) =>
        this.DtoRepository = dtoRepository;
}
```

`CustomerRepository` now implements `IRepository<Customer, CustomerId>` (`Get`, `Add`, `Update`, `Remove`) purely by delegating to `ICustomerDtoRepository` through `CustomerDtoConverter` — no manual mapping code required in the adapter itself.

## Requirements

- .NET SDK 10.0
- The [Utils](https://www.nuget.org/packages/Utils) package (v3.3.0) for the domain and validation modules

## Building and testing

```bash
dotnet restore DDD.sln
dotnet build DDD.sln
dotnet test DDD.sln
```

Unit tests live in `tests/DDD.Tests`, with one directory per module.

## Installation

Packages are available on NuGet:

```bash
dotnet add package DDD.Domain
dotnet add package DDD.Domain.Model.Validation
dotnet add package DDD.Domain.Model.Validation.AspNetCore
dotnet add package DDD.Domain.Events.MediatR
dotnet add package DDD.Domain.Events.AspNetCore
dotnet add package DDD.Application
```

## License

Author: Wiktor Bejma.
