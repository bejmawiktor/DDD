using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidatorScope : Scope<Exception, ValidatorScope, ValidatorHandler> { }
