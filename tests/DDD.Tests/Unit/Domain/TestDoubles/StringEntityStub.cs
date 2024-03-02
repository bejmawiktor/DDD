using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class StringEntityStub : Entity<string>
    {
        public new string Id 
        { 
            get => base.Id; 
            set => base.Id = value;
        }

        public StringEntityStub(string id) : base(id)
        {
        }
    }
}