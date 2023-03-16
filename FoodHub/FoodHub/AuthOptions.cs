using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FoodHub
{
	public class AuthOptions
	{
		public const string ISSUER = "MyAuthServer"; // видавець токена
		public const string AUDIENCE = "MyAuthClient"; // споживач токена
		const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрування
		public const int LIFETIME = 5; // час життя токена - 5 хвилин
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
		}
	}
}
