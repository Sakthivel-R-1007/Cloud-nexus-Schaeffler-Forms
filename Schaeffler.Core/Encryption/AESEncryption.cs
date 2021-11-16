using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Schaeffler.Core.Encryption
{
    public static class AESEncryption
    {
        #region Private Encryption variables & Methods

        private static UTF8Encoding _enc;
        private static RijndaelManaged _rcipher;
        private static byte[] _key, _pwd, _ivBytes, _iv;
        private static string _password = "9LglpRIKiBrOSw3sTestIfidIYehabroTrIWEdipIdaTh4tRIkUwrodRoPRl0itr";
        private static string _initVector = "S92p#ik8flP=fesp";

        private enum EncryptMode { ENCRYPT, DECRYPT };

        private static readonly char[] CharacterMatrixForRandomIVStringGeneration = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        private static void InitEncryptor()
        {
            _enc = new UTF8Encoding();
            _rcipher = new RijndaelManaged();
            _rcipher.Mode = CipherMode.CBC;
            _rcipher.Padding = PaddingMode.PKCS7;
            _rcipher.KeySize = 256;
            _rcipher.BlockSize = 128;
            _key = new byte[32];
            _iv = new byte[_rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            _ivBytes = new byte[16];
        }

        #region Random IV

        private static string GenerateRandomIV(int length)
        {
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes); //Fills an array of bytes with a cryptographically strong sequence of random values. 
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
            }

            return new string(_iv);
        }

        #endregion

        #endregion

        #region Base64 Encoding & Decoding

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion

        #region String to Hex & Hex to String

        public static string ConvertStringToHex(String input)
        {
            Encoding encoding = Encoding.Unicode;
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public static string ConvertHexToString(String hexInput)
        {
            Encoding encoding = Encoding.Unicode;
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        #endregion

        #region Public Encryption & Decryption

        public static String EncryptString(string _input)
        {
            if (!String.IsNullOrEmpty(_input))
            {
                try
                {
                    _input = encryptDecrypt(_input, _password, EncryptMode.ENCRYPT, _initVector);
                }
                catch
                {
                    _input = string.Empty;
                }
            }

            return _input;
        }

        public static String DecryptString(string _cipherText)
        {
            if (!String.IsNullOrEmpty(_cipherText))
            {
                try
                {
                    _cipherText = encryptDecrypt(_cipherText, _password, EncryptMode.DECRYPT, _initVector);
                }
                catch
                {
                    _cipherText = string.Empty;
                }
            }

            return _cipherText;
        }

        #endregion

        public static string EncryptandEncodeUrl(string plainText, string individualPassphrase = null)
        {

            return HttpContext.Current.Server.UrlEncode(EncryptString(plainText)).Replace("%", "_");
        }
        public static string DecodeUrlandDecrypt(string cipherText, string individualPassphrase = null)
        {
            return DecryptString(HttpContext.Current.Server.UrlDecode(cipherText.Replace("_", "%")));
        }

        #region Encrypt & Decrypt Algorithm

        /**
		 * 
		 * @param _inputText
		 *            Text to be encrypted or decrypted
		 * @param _encryptionKey
		 *            Encryption key to used for encryption / decryption
		 * @param _mode
		 *            specify the mode encryption / decryption
		 * @param _initVector
		 * 			  initialization vector
		 * @return encrypted or decrypted string based on the mode
	 	*/
        private static string encryptDecrypt(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {
            InitEncryptor();
            string _out = "";// output string
            //_encryptionKey = MD5Hash (_encryptionKey);
            _pwd = Encoding.UTF8.GetBytes(_encryptionKey);
            _ivBytes = Encoding.UTF8.GetBytes(_initVector);

            int len = _pwd.Length;
            if (len > _key.Length)
            {
                len = _key.Length;
            }
            int ivLenth = _ivBytes.Length;
            if (ivLenth > _iv.Length)
            {
                ivLenth = _iv.Length;
            }

            Array.Copy(_pwd, _key, len);
            Array.Copy(_ivBytes, _iv, ivLenth);
            _rcipher.Key = _key;
            _rcipher.IV = _iv;

            if (_mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                byte[] plainText = _rcipher.CreateEncryptor().TransformFinalBlock(_enc.GetBytes(_inputText), 0, _inputText.Length);
                _out = Convert.ToBase64String(plainText);
            }
            if (_mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                byte[] plainText = _rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(_inputText), 0, Convert.FromBase64String(_inputText).Length);
                _out = _enc.GetString(plainText);
            }
            _rcipher.Dispose();
            return _out;// return encrypted/decrypted string
        }

        /***
         * This function decrypts the encrypted text to plain text using the key
         * provided. You'll have to use the same key which you used during
         * encryption
         * 
         * @param _encryptedText
         *            Encrypted/Cipher text to be decrypted
         * @param _key
         *            Encryption key which you used during encryption
         */
        public static string getHashSha256(string text, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x); //covert to hex string
            }
            if (length > hashString.Length)
                return hashString;
            else
                return hashString.Substring(0, length);
        }

        #endregion
    }
}
