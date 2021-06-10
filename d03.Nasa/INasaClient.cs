namespace d03.Nasa
{
    public interface INasaClient<in TIn, out TOut>
    {
        Task<TOut> GetAsync(TIn input);
    }
}