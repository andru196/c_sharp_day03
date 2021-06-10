namespace d03.Nasa
{
    public abstract class ApiClientBase
    {
        protected string ApiKey{get; init;}
        protected ApiClientBase(string apiKey);
        
        protected T HttpGetAsync<T>(string url);
    }
}