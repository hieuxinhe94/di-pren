namespace Di.Common.Cache
{
    public interface IObjectCache
    {
        object Get(string cacheKey);
        void Insert(string cacheKey, object value);
    }
}