using Flurl;

namespace MarquitoUtils.Main.Api
{
    /// <summary>
    /// Provides a base class for API providers with a specified endpoint.
    /// </summary>
    /// <remarks>This abstract class serves as a foundation for creating API providers that require a specific
    /// endpoint URL. Derived classes should utilize the <see cref="Endpoint"/> property to access the configured
    /// endpoint.</remarks>
    public abstract class DefaultApiProvider
    {
        /// <summary>
        /// An URL for real use
        /// </summary>
        protected string Endpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiProvider"/> class with the specified API endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint URL. Cannot be null or empty.</param>
        public DefaultApiProvider(string endpoint)
        {
            this.Endpoint = endpoint;
        }



        #region Commons

        /// <summary>
        /// Get main url with the correct Endpoint
        /// </summary>
        /// <returns>The main url with the correct Endpoint</returns>
        protected Url GetMainUrl()
        {
            return new Url($"https://{this.Endpoint}");
        }

        #endregion Commons
    }
}
