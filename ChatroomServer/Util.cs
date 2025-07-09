using System.Security.Cryptography;

namespace ChatroomDesktop.Utilities;

public class Util
{ 
    //https://stackoverflow.com/questions/4181198/how-to-hash-a-password#10402129

    public static bool CheckPassword(string givenPassword, string actualPassword)
    {
        if (actualPassword == null)
        {
            return false;
        }
        Console.WriteLine("Checking password");
        byte[] hashBytes = Convert.FromBase64String(actualPassword);
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);
        var pbkdf2 = new Rfc2898DeriveBytes(givenPassword, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        for (int i = 0; i < 32; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }
        return true;
    }

    public static string GenrateRandomString()
    {
        var length = 5;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }
}