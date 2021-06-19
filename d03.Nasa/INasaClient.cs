namespace d03.Nasa
{
    public interface INasaClient<in TIn, out TOut>
    {
        TOut GetAsync(TIn input);
    }
}