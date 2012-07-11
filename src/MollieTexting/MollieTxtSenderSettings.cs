using System.Configuration;

namespace MollieTexting
{
	public class MollieTxtSenderSettings : ConfigurationSection
	{
		private static readonly MollieTxtSenderSettings _settings = ConfigurationManager.GetSection("MollieTxtSenderSettings") as MollieTxtSenderSettings;

		public static MollieTxtSenderSettings Settings
		{
			get
			{
				return _settings;
			}
		}

		[ConfigurationProperty("username", IsRequired = true)]
		public string Username
		{
			get { return (string)this["username"]; }
			set { this["username"] = value; }
		}


		[ConfigurationProperty("passwordMd5", IsRequired = true)]
		//[StringValidator(MinLength = 1, MaxLength = 256)]
		public string PasswordMd5
		{
			get { return (string)this["passwordMd5"]; }
			set { this["passwordMd5"] = value; }
		}

		[ConfigurationProperty("gateway", DefaultValue = 2, IsRequired = false)]
		[IntegerValidator]
		public int Gateway
		{
			get { return (int)this["gateway"]; }
			set { this["gateway"] = value; }
		}
	}
}
