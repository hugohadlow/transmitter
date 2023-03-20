namespace Transmitter.Tools
{
    public class Utils
    {
        //Use Base32 for safe filenames.
        public static string Base64ToBase32(string input) {
            var bytes = Convert.FromBase64String(input);
            return Base32Encoding.ToString(bytes);
        }
    }
}
