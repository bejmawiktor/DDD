namespace DDD.Persistence
{
    public class Pagination
    {
        public uint Page { get; }
        public uint ItemsPerPage { get; }

        public Pagination(uint page, uint itemsPerPage)
        {
            this.Page = page;
            this.ItemsPerPage = itemsPerPage;
        }
    }
}