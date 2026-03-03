using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FCms.Auth.Concrete;

public class CmsUserModel
{
    public System.Guid? Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash {get; set;}
    
    private string ComputeHash(string password, byte[] salt)
    {
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32);

        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    public string HashPassword(string password)
         => ComputeHash(password, RandomNumberGenerator.GetBytes(16));

    public bool VerifyPassword(string password)
    {
        byte[] salt = Convert.FromBase64String(PasswordHash).AsSpan(0, 16).ToArray();
        return ComputeHash(password, salt) == PasswordHash;
    }
}
