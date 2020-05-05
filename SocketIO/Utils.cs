using System.Text;

namespace SocketIO
{
    class Utils
    {
        public static string Buffer2String(byte[] buffer)
        {
            string result;
            int endIndex;

            result = Encoding.ASCII.GetString(buffer);
            endIndex = result.IndexOf('\0');
            if (endIndex >= 0)
                result = result.Substring(0, endIndex);
            return result;
        }
    }
}
