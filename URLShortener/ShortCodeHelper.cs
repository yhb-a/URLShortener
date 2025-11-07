using SimpleBase;
using System.Text;

namespace URLShortener
{
    public static class ShortCodeHelper
    {
        public static string GetShortCode(int count)
        {
            return EncodeIntToBase62(count);
        }

        private static string EncodeIntToBase62(int value)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if(value < 0)
            {
                return "0";
            }

            var result = new StringBuilder();

            while (value > 0) 
            {
                result.Insert(0, chars[value % 62]);
                value = value / 62;
            }

            return result.ToString();
        }
    }
}
