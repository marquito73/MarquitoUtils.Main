namespace MarquitoUtils.Main.Class.Cache
{
    /// <summary>
    /// Default cache class
    /// </summary>
    /// <typeparam name="T">The entity to store inside the cache</typeparam>
    public abstract class Cache<T> where T : class
    {
        /// <summary>
        /// The cache map
        /// </summary>
        public Dictionary<Type, List<T>> CacheMap { get; } = new Dictionary<Type, List<T>>();
    }
}
