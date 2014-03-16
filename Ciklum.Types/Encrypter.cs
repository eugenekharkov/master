using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Ciklum.Types
{
    public static class Encrypter
    {
        public static byte[] Encrypt(byte[] message, string password)
        {
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;


            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(message, 0, message.Length);
            cryptoStream.FlushFinalBlock();


            byte[] encryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

            return encryptedMessageBytes;
        }

        public static byte[] Decrypt(byte[] encryptedMessage, string password)
        {
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;

            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);
            cryptoStream.FlushFinalBlock();

            byte[] decryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

            return decryptedMessageBytes;
        }
    }
}