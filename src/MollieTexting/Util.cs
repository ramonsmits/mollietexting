using System.Text;

namespace MollieTexting
{
	internal static class Util
	{
		public static string GenerateMd5Hex(string data)
		{
			using (var provider = System.Security.Cryptography.MD5.Create())
			{
				return ToHex(provider.ComputeHash(Encoding.UTF8.GetBytes(data)));
			}
		}
	
		/// <remarks>http://stackoverflow.com/questions/623104/c-sharp-byte-to-hex-string</remarks>
		static string ToHex(byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];

			byte b;

			for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
			{
				b = ((byte)(bytes[bx] >> 4));
				c[cx] = (char)(b > 9 ? b - 10 + 'A' : b + '0');

				b = ((byte)(bytes[bx] & 0x0F));
				c[++cx] = (char)(b > 9 ? b - 10 + 'A' : b + '0');
			}

			return new string(c);
		}
	}
}
