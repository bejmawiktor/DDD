using System.Collections.Generic;

namespace DDD.Domain.Utils;

public interface IError
{
    string Message { get; }
}

public interface IError<out TReason> : IError
{
    IEnumerable<TReason> Reasons { get; }
}
