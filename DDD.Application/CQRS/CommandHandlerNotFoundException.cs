using System;

namespace DDD.Application.CQRS
{
    [Serializable]
    public class CommandHandlerNotFoundException : Exception
    {
    }
}