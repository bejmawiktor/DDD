namespace DDD.Model
{
    public interface IMembersValidator
    {
        void ValidateMember(string name, object value);
    }
}