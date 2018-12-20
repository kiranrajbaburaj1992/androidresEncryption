using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidApp
{
    class Program
    {
        string publicKey;
        string privateKey;
        //RSAProvider.GenerateKeys(out publicKey, out privateKey);


        string plain = "testing";

        /*
        publicKey = File.ReadAllText(@"D://publicKey.pem");
        privateKey = File.ReadAllText(@"D://privateKey.pem");

        string encrypted = RSAProvider.Encrypt(publicKey, plain);
        string decrypted = RSAProvider.Decrypt(privateKey, encrypted);

        */

        //Encryption

        //X509Certificate2 combinedCertificate = new X509Certificate2(@"D:\ca.pfx");

        // Encrypt the file using the public key from the certificate.
        string encryptedData = EncryptData("test data", @"D:\ca.pfx");
        //string decryptedData = DecryptEncryptedData(encryptedData, @"D:\a.pfx");

        //Console.WriteLine("Encrypted text: \n\n" + encryptedData);
            //Console.WriteLine("\nDecrypted text: \n" + decryptedData);
            //Console.ReadKey();



            public static string EncryptData(string Base64EncryptedData, string PathToPrivateKeyFile)
        {
            X509Certificate2 myCertificate;
            try
            {
                myCertificate = new X509Certificate2(PathToPrivateKeyFile);
            }
            catch
            {
                throw new CryptographicException("Unable to open key file.");
            }

            RSACryptoServiceProvider rsaObj;
            if (myCertificate.HasPrivateKey)
            {
                rsaObj = (RSACryptoServiceProvider)myCertificate.PrivateKey;
            }
            else
                throw new CryptographicException("Private key not contained within certificate.");

            if (rsaObj == null)
                return String.Empty;

            byte[] decryptedBytes;
            try
            {
                decryptedBytes = rsaObj.Encrypt(Encoding.UTF8.GetBytes(Base64EncryptedData), true);
            }
            catch
            {
                throw new CryptographicException("Unable to decrypt data.");
            }

            //    Check to make sure we decrpyted the string 
            if (decryptedBytes.Length == 0)
                return String.Empty;
            else
                return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }



        public static string DecryptEncryptedData(string Base64EncryptedData, string PathToPrivateKeyFile)
        {
            X509Certificate2 myCertificate;
            try
            {
                myCertificate = new X509Certificate2(PathToPrivateKeyFile);
            }
            catch
            {
                throw new CryptographicException("Unable to open key file.");
            }

            RSACryptoServiceProvider rsaObj;
            if (myCertificate.HasPrivateKey)
            {
                rsaObj = (RSACryptoServiceProvider)myCertificate.PrivateKey;
            }
            else
                throw new CryptographicException("Private key not contained within certificate.");

            if (rsaObj == null)
                return String.Empty;

            byte[] decryptedBytes;
            try
            {
                decryptedBytes = rsaObj.Decrypt(Convert.FromBase64String(Base64EncryptedData), false);
            }
            catch
            {
                throw new CryptographicException("Unable to decrypt data.");
            }

            //    Check to make sure we decrpyted the string 
            if (decryptedBytes.Length == 0)
                return String.Empty;
            else
                return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }
    }

}


//http://travistidwell.com/blog/2013/09/06/an-online-rsa-public-and-private-key-generator/
//http://travistidwell.com/jsencrypt/demo/
//https://www.cnblogs.com/adylee/p/3611461.html

//https://github.com/jrnker/CSharp-easy-RSA-PEM


/*
 *  genrsa -out ca.key 4096
 * req -new -x509 -days 1826 -key ca.key -out ca.crt
 *  genrsa -out ia.key 4096
 * req -new -key ia.key -out ia.csr
 * pkcs12 -in ca.crt -inkey ca.key -export -out a.pfx
 * pkcs12 -in a.crt -inkey a.key -export -out a.pfx
 * 
 * */
