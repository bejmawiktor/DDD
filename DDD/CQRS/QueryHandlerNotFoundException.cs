using System;

namespace DDD.CQRS
{
    [Serializable]
    public class QueryHandlerNotFoundException : Exception
    {
    }
}