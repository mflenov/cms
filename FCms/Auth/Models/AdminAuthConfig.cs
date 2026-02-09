using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FCms.Auth.Concrete;

public class CmsUserModel
{
    public System.Guid? Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash {get; set;}
    
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32);
        
        // Combine salt and hash: first 16 bytes are salt, remaining 32 bytes are hash
        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);
        
        return Convert.ToBase64String(hashBytes);
    }  
}
