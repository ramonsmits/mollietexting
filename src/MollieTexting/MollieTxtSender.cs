using System;
using System.Globalization;
using System.Xml;
using System.Web;

namespace MollieTexting
{
	public class MollieTxtSender : IMollieTxtSender
	{
		private const string UrlFormat = "http://www.mollie.nl/xml/sms/?username={0}&md5_password={1}&originator={2}&recipients={3}&message={4}";
		private readonly string _username;
		private readonly string _passwordMd5;
		private readonly int _gateway;

		public MollieTxtSender()
		{
			var settings = MollieTxtSenderSettings.Settings;
			_username = settings.Username;
			_gateway = settings.Gateway;
			_passwordMd5 = settings.PasswordMd5;
		}

		public void Send(string originator, string message, Uri deliveryReportUrl, string reference, DateTime? deliveryDate, params string[] recipients)
		{
			if (string.IsNullOrEmpty(originator)) throw new ArgumentNullException("originator");
			if (string.IsNullOrEmpty(message)) throw new ArgumentNullException("message");
			if (recipients == null || recipients.Length == 0) throw new ArgumentNullException("recipients");

			string url = null;

			try
			{
				url = BuildUrl(originator, message, deliveryReportUrl, reference, deliveryDate, recipients);

				var d = new XmlDocument();
				d.Load(url);

				var selectSingleNode = d.SelectSingleNode("//success");
				if (selectSingleNode == null || selectSingleNode.InnerText != "true") throw new Exception("No success\n" + d.OuterXml);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to send text message.", ex)
				{
					Data = { { "Url", url } }
				};
			}
		}

		internal string BuildUrl(string originator, string message, Uri deliveryReportUrl, string reference,
														DateTime? deliveryDate, string[] recipients)
		{
			var url = string.Format(
				CultureInfo.InvariantCulture,
				UrlFormat,
				_username,
				_passwordMd5,
				originator,
				String.Join(",", recipients),
				HttpUtility.UrlEncode(message)
				);

			if (deliveryReportUrl != null)
			{
				url += "&dlrurl=" + HttpUtility.UrlEncode(deliveryReportUrl.ToString());
			}

			url += "&gateway=" + _gateway;

			if (!string.IsNullOrEmpty(reference))
			{
				url += "&reference=" + reference;
			}

			if (deliveryDate != null)
			{
				url += "&deliverydate=" + deliveryDate.Value.ToString("yyyyMMddhhmmss");
			}

			return url;
		}

		public static void Test()
		{
			var s = new MollieTxtSender();
			s.Send("Test", "Ik ben een testje", "+31642741699");
		}

		public void Send(string originator, string message, params string[] recipients)
		{
			Send(originator, message, null, null, null, recipients);
		}

		public void Send(MollieTextMessage msg)
		{
			Send(
				msg.Originator,
				msg.Message,
				msg.DeliveryReportUrl,
				msg.Reference,
				msg.DeliveryDate,
				msg.Recipients
				);
		}
	}
}