namespace MarquitoUtils.Main.Api.Cryptography
{
    /// <summary>
    /// Represents an API key consisting of a public and private key pair
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// Gets or sets the public key used for encryption or verification purposes.
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Gets or sets the private key used for cryptographic operations.
        /// </summary>
        public string PrivateKey { get; set; }
    }
}
