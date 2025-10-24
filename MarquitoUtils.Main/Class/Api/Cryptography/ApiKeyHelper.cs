using System.Security.Cryptography;

namespace MarquitoUtils.Main.Class.Api.Cryptography
{
    public static class ApiKeyHelper
    {
        public static ApiKey GenerateNewRsaApiKey()
        {
            RSA rsa = RSA.Create(2048);

            return new ApiKey
            {
                PrivateKey = ExportRsaPrivateKeyPem(rsa),
                PublicKey = ExportRsaPublicKeyPem(rsa),
            };
        }

        private static string ExportRsaPrivateKeyPem(RSA rsa)
        {
            byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();

            return PemEncoding.WriteString("PRIVATE KEY", privateKeyBytes);
        }

        private static string ExportRsaPublicKeyPem(RSA rsa)
        {
            byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();

            return PemEncoding.WriteString("PUBLIC KEY", publicKeyBytes);
        }

        public static ApiKey GenerateNewECDsaApiKey()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

            return new ApiKey
            {
                PrivateKey = ExportECDsaPrivateKeyPem(ecdsa),
                PublicKey = ExportECDsaPublicKeyPem(ecdsa),
            };
        }

        private static string ExportECDsaPrivateKeyPem(ECDsa ecdsa)
        {
            byte[] privateKeyBytes = ecdsa.ExportPkcs8PrivateKey();

            return Convert.ToBase64String(privateKeyBytes);
        }

        private static string ExportECDsaPublicKeyPem(ECDsa ecdsa)
        {
            byte[] publicKeyBytes = ecdsa.ExportSubjectPublicKeyInfo();

            return Convert.ToBase64String(publicKeyBytes);
        }
    }
}