using System.Security.Cryptography;

namespace ChatroomDesktop.Utilities;

public class Util
{
    public static string SaltHashPassword(string password)
    {
        //https://stackoverflow.com/questions/4181198/how-to-hash-a-password#10402129
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        string base64 = Convert.ToBase64String(hashBytes);
        return base64;
    }
}