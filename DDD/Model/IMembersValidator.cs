namespace DDD.Model
{
    public interface IMembersValidator
    {
        void ValidateMember<TValueType>(string name, TValueType value);
    }
}