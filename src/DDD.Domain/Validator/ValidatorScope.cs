using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validator;

internal sealed class ValidatorScope : Scope<Exception, ValidatorScope, ValidatorHandler> { }
