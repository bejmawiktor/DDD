using System;

namespace DDD.CQRS
{
    [Serializable]
    public class CommandHandlerNotFoundException : Exception
    {
    }
}