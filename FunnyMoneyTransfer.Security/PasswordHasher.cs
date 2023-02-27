using System.Security.Cryptography;

namespace FunnyMoneyTransfer.Security
{
    //https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    public class PasswordHasher
    {
        public static string GetPasswordHash(string password)
        {
            //STEP 1 Create the salt value with a cryptographic PRNG:
            byte[] salt;
#pragma warning disable SYSLIB0023
#pragma warning disable SYSLIB0041
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //STEP 2 Create the Rfc2898DeriveBytes and get the hash value:
            Rfc2898DeriveBytes pbkdf2 = new(password, salt, 100000);
#pragma warning restore SYSLIB0023
#pragma warning restore SYSLIB0041
            byte[] hash = pbkdf2.GetBytes(20);
            //STEP 3 Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            //STEP 4 Turn the combined salt+hash into a string for storage
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }

        public static bool IsPasswordValid(string password, string passwordHash)
        {
            //STEP 5 Verify the user-entered password against a stored password
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
#pragma warning disable SYSLIB0041
            Rfc2898DeriveBytes pbkdf2 = new(password, salt, 100000);
#pragma warning restore SYSLIB0041
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
        
        
        
        
        
        

    
    
