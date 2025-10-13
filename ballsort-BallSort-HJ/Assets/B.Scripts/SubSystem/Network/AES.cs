using System;
using System.Security.Cryptography;
using System.Text;

public class AES
{
    private static string key = "luckyscratch12iluckyscratch12i33";
    public AES()
    {
    }
    public static string Encode(String data)
    {
        string result;
        try
        {
            if (string.IsNullOrEmpty(data))
            {
                result = null;
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                RijndaelManaged rijndaelManaged = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
                byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                result = Convert.ToBase64String(array, 0, array.Length);

                return result;
            }
        }
        catch (Exception ex)
        {
            result = null;
            System.Console.WriteLine(ex);
        }
        return result;
    }

    public static string Decode(String data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }
        string result;
        try
        {
            byte[] array = Convert.FromBase64String(data);

            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
            byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);

            result = Encoding.UTF8.GetString(bytes);
        }
        catch (Exception ex)
        {
            result = null;
            System.Console.WriteLine(ex);
        }
        return result;
    }
}
