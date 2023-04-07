using Passwordless_Authenticator.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Passwordless_Authenticator.Services.Crypto
{
    internal class FileEncryptionService
    {

        public static void ImportKeys()
        {
            string dec_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");

        }

        public static string EncryptFile(string outputFile, string skey)
        {
            string inputFile = AppConstants.ENC_DB_PATH;
            try
            {
                using (var aes = Aes.Create("AesManaged"))
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }

                return "File encrypted successfully.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "Encryption Failed";
            }
        }

        public static string DecryptFile(string inputFile, string skey)
        {
            string outputFile = AppConstants.IMP_DB_PATH;
            try
            {
                using (var aes = Aes.Create("AesManaged"))
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }

                return "File decrypted Successfully.\n";
            }
            catch (Exception ex)
            {
                // failed to decrypt file
                Debug.WriteLine(ex);
                return "Decryption failed.";
                //Debug.WriteLine(ex);
            }
        }

    }
}
