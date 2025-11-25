using MarquitoUtils.Main.Api.Cryptography;

namespace MarquitoUtils.Main.Api.Configuration
{
    /// <summary>
    /// Api configuration class for api settings file
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// Public and private api key for secure access
        /// </summary>
        public ApiKey ApiKey { get; set; }
        /// <summary>
        /// Issuer of the api
        /// </summary>
        public string Issuer { get; set; }
    }
}
