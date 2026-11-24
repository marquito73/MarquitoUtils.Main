using System.Security.Cryptography;

namespace MarquitoUtils.Main.Api.Cryptography
{
    /// <summary>
    /// Helper class for generating and managing API keys using RSA and ECDsa cryptography.
    /// </summary>
    public static class ApiKeyHelper
    {
        /// <summary>
        ///Generates a new 2048-bit RSA key pair and packages it as an API key.
        /// </summary>
        /// <returns>An <see cref="ApiKey"/> containing the generated private and public RSA keys in PEM format.</returns>
        public static ApiKey GenerateNewRsaApiKey()
        {
            RSA rsa = RSA.Create(2048);

            return new ApiKey
            {
                PrivateKey = ExportRsaPrivateKeyPem(rsa),
                PublicKey = ExportRsaPublicKeyPem(rsa),
            };
        }

        /// <summary>
        /// Exports the RSA private key in PKCS#8 format and encodes it as a PEM string.
        /// </summary>
        /// <param name="rsa">The RSA instance that contains the private key to export.</param>
        /// <returns>A PEM-encoded string that contains the PKCS#8 private key with the "PRIVATE KEY" label.</returns>
        private static string ExportRsaPrivateKeyPem(RSA rsa)
        {
            byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();

            return PemEncoding.WriteString("PRIVATE KEY", privateKeyBytes);
        }

        /// <summary>
        /// Exports the RSA public key as a PEM-encoded SubjectPublicKeyInfo value.
        /// </summary>
        /// <param name="rsa">The RSA instance that provides the public key to export.</param>
        /// <returns>A PEM-formatted public key string with the "PUBLIC KEY" label.</returns>
        private static string ExportRsaPublicKeyPem(RSA rsa)
        {
            byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();

            return PemEncoding.WriteString("PUBLIC KEY", publicKeyBytes);
        }

        /// <summary>
        /// Generates a new ECDSA API key pair using the NIST P-256 elliptic curve.
        /// </summary>
        /// <remarks>Protect the private key material and store it securely.</remarks>
        /// <returns>An ApiKey containing the generated private and public keys in PEM format.</returns>
        public static ApiKey GenerateNewECDsaApiKey()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

            return new ApiKey
            {
                PrivateKey = ExportECDsaPrivateKeyPem(ecdsa),
                PublicKey = ExportECDsaPublicKeyPem(ecdsa),
            };
        }

        /// <summary>
        /// Exports the ECDSA private key in PKCS#8 format and encodes it as a Base64 string.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance that contains the private key to export.</param>
        /// <returns>A Base64-encoded string containing the PKCS#8 private key bytes.</returns>
        private static string ExportECDsaPrivateKeyPem(ECDsa ecdsa)
        {
            byte[] privateKeyBytes = ecdsa.ExportPkcs8PrivateKey();

            return Convert.ToBase64String(privateKeyBytes);
        }

        /// <summary>
        /// Exports the ECDSA public key as a Base64-encoded X.509 SubjectPublicKeyInfo string.
        /// </summary>
        /// <param name="ecdsa">ECDSA instance that provides the public key to export.</param>
        /// <returns>A Base64-encoded string containing the public key in X.509 SubjectPublicKeyInfo format.</returns>
        private static string ExportECDsaPublicKeyPem(ECDsa ecdsa)
        {
            byte[] publicKeyBytes = ecdsa.ExportSubjectPublicKeyInfo();

            return Convert.ToBase64String(publicKeyBytes);
        }
    }
}