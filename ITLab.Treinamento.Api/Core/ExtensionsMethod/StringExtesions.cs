using System;
using System.Security.Cryptography;
using System.Text;

namespace ITLab.Treinamento.Api.Core.ExtensionsMethod
{
    public static class StringExtesions
    {
        public static string GetHash(this String value)
        {
            HashAlgorithm algorithm = MD5.Create();
            var HashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));

            var HashString = new StringBuilder();
            foreach (byte b in HashBytes)
                HashString.Append(b.ToString("X2"));

            return HashString.ToString();
        }
    }
}