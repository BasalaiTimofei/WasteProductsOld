using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WasteProducts.IdentityServer.Certificate
{
    public static class CertificateLoader
    {
        public static X509Certificate2 Load()
        {
            var assembly = typeof(CertificateLoader).Assembly;
            using (var stream = assembly.GetManifestResourceStream("WasteProducts.IdentityServer.Certificate.idsrv3test.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}