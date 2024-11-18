using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Domain.Utils;

public class Error : IError
{
    public string Message { get; protected set; }

    protected Error()
    {
        this.Message = string.Empty;
    }

    public Error(string message)
    {
        ValidateMessage(message);

        this.Message = message;
    }

    protected static void ValidateMessage(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message == string.Empty)
        {
            throw new ArgumentException("Empty message given.");
        }
    }

    public override string ToString() => this.Message;
}

public class Error<TReason> : Error, IError<TReason>
{
    public IEnumerable<TReason> Reasons { get; protected set; }

    protected Error()
        : base()
    {
        this.Reasons = [];
    }

    public Error(string message)
        : base(message)
    {
        this.Reasons = [];
    }

    public Error(string message, IEnumerable<TReason> reasons)
        : base(message)
    {
        Error<TReason>.ValidateReasons(reasons);

        this.Reasons = reasons;
    }

    protected static void ValidateReasons(IEnumerable<TReason> reasons)
    {
        ArgumentNullException.ThrowIfNull(reasons);

        if (!reasons.Any())
        {
            throw new ArgumentException("Empty reasons given.");
        }
    }
}
