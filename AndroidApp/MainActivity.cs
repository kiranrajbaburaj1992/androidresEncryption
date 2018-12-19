using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using System;
using Android.Content.Res;
using System.Security.Cryptography;

namespace AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            string publicKey;
            string privateKey;

            AssetManager assets = this.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("publicKey.pem")))
            {
                publicKey = sr.ReadToEnd();
            }
            using (StreamReader sr = new StreamReader(assets.Open("privateKey.pem")))
            {
                privateKey = sr.ReadToEnd();
            }

            //publicKey = File.ReadAllText(@"D://publicKey.pem");
            //privateKey = File.ReadAllText(@"D://privateKey.pem");


            EditText editText = (EditText)FindViewById(Resource.Id.inputEt);
            Button button = (Button)FindViewById(Resource.Id.button1);

            button.Click += delegate
            {
                string plain = editText.Text.ToString();
                //string encrypted = RSAProvider.Encrypt(publicKey, plain);
                //string decrypted = RSAProvider.Decrypt(privateKey, encrypted);



                DateTime t1; //To use for timings

                #region Load key from file and encrypt test
                t1 = DateTime.Now;
                Console.WriteLine("*** Load key from file and encrypt test ***");
                Console.WriteLine("Loading premade private keys..");
                //string loadedRSA = File.ReadAllText("keys/private.rsa.pem");
                string loadedRSA = privateKey;
                RSACryptoServiceProvider privateRSAkey = Crypto.Crypto.DecodeRsaPrivateKey(loadedRSA);

                Console.WriteLine("Loading premade public key..");
                //string loadedX509 = File.ReadAllText("keys/public.x509.pem");
                string loadedX509 = publicKey;
                RSACryptoServiceProvider publicX509key = Crypto.Crypto.DecodeX509PublicKey(loadedX509);

                string secret = "Hello world - 1";
                Console.WriteLine("Using public key, encrypt \"" + secret + "\"..");
                string supersecret = Crypto.Crypto.EncryptString(secret, publicX509key);
                Console.WriteLine("Encrypted: " + supersecret);

                Console.WriteLine("Using private key, decrypt..");
                string decodesecret = Crypto.Crypto.DecryptString(supersecret, privateRSAkey);
                Console.WriteLine("Decrypted: " + decodesecret);
                #endregion

                editText.Text = decodesecret;
            };

        }
    }
}


//http://travistidwell.com/blog/2013/09/06/an-online-rsa-public-and-private-key-generator/
//http://travistidwell.com/jsencrypt/demo/
//https://www.cnblogs.com/adylee/p/3611461.html

//https://github.com/jrnker/CSharp-easy-RSA-PEM