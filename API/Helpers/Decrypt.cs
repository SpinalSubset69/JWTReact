﻿using System.Text;
using System.Security.Cryptography;

namespace API.Helpers
{
    public static class Decrypt
    {
        public static string DecryptPLainText(string plainText, string securityKey)
        {
            //Get Array from the plain text
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            //instance of the service to encrypt
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettnig bytes from the securityKey and compute the hash value of the SecurityArray
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));

            //De-Allocating the memory
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the security key to tje TripleDes Service Provider
            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the crypto servie is Electronic Code Book
            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there's any extra byte is added
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCryptoTransform = objTripleDESCryptoService.CreateDecryptor();

            //Transform the bytes array to result array
            byte[] resultArray = objCryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            
            objTripleDESCryptoService.Clear();
            

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
