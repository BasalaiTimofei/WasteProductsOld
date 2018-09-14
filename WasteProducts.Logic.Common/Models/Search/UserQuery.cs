namespace WasteProducts.Logic.Common.Models.Search
{
    public class UserQuery
    {
        public string QueryString { get; set; }

        public UserQuery(string query)
        {
            QueryString = query;
        }
    }
}
