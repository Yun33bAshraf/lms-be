using System.Security.Cryptography;
using System.Text;

namespace LMS.Application.Common.Utilities;
public class PasswordService
{
    public static string GenerateRandomPassword(int byteLength = 20)
    {
        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        byte[] bytes = new byte[byteLength];
        RandomNumberGenerator.Fill(bytes);

        StringBuilder result = new StringBuilder();

        int buffer = bytes[0];
        int bitsLeft = 8;
        int index = 1;

        while (result.Length < (int)(byteLength * 1.6)) // Base32 expands data ~1.6x
        {
            if (bitsLeft < 5)
            {
                if (index < bytes.Length)
                {
                    buffer <<= 8;
                    buffer |= bytes[index++];
                    bitsLeft += 8;
                }
                else
                {
                    int pad = 5 - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }

            int val = buffer >> bitsLeft - 5 & 0b11111;
            bitsLeft -= 5;
            result.Append(base32Chars[val]);
        }

        return result.ToString();
    }
}
