using System;

namespace Exyll.MollieTexting
{
	public interface IMollieTxtSender : ITxtSender
	{
		void Send(string originator, string message, Uri deliveryReportUrl, string reference, DateTime? deliveryDate, params string[] recipients);
		void Send(MollieTextMessage message);
	}
}