using MarquitoUtils.Main.Class.Api.Cryptography;

namespace MarquitoUtils.Main.Class.Api.Configuration
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
    }
}
