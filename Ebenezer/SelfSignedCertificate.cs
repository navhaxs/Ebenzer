using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Ebenezer;

public static class SelfSignedCertificate
{
    public static X509Certificate2 GenerateSelfSignedCertificate()
    {
        string subjectName = "localhost";
        int keyStrength = 2048;
    
        // Generate a new RSA key pair
        using (RSA rsa = RSA.Create(keyStrength))
        {
            var request = new CertificateRequest(
                $"CN={subjectName}", 
                rsa, 
                HashAlgorithmName.SHA256, 
                RSASignaturePadding.Pkcs1);

            var certificate = request.CreateSelfSigned(
                DateTimeOffset.Now, 
                DateTimeOffset.Now.AddYears(1));

            // Create an exportable certificate
            return new X509Certificate2(certificate.Export(X509ContentType.Pfx), 
                (string)null, 
                X509KeyStorageFlags.Exportable);
        }
    }

}