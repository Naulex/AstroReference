using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;


public class Crypt : MonoBehaviour
{
    private static string pass = "RBORXHtfbRuQDnWw";
    private static string initVec = "56DUnNyAOqS1Z7OB";
    private static string sol = "Zr0W3S2qA43swgHb";
    private static int passIter = 512;
    public static string Encrypt(string ishText)
    {
        try
        {
            string cryptographicAlgorithm = "SHA-512";
            int keySize = 256;
            if (string.IsNullOrEmpty(ishText))
                return "";
            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);
            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;
            byte[] cipherTextBytes = null;
            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }
            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }
        catch
        {
            return "";
        }
    }
    public static string Decrypt(string ciphText)
    {
        try
        {
            string cryptographicAlgorithm = "SHA-512";
            int keySize = 256;
            if (string.IsNullOrEmpty(ciphText))
                return "";
            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);
            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;
            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }
            symmK.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
        catch
        {
            return "";
        }
    }
}
