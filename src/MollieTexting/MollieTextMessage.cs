using System;

namespace MollieTexting
{
	public class MollieTextMessage
	{
		public string Originator { get; set; }
		public string Message { get; set; }
		public string[] Recipients { get; set; }
		public Uri DeliveryReportUrl { get; set; }

		public DateTime? DeliveryDate { get; set; }

		public string Reference { get; set; }
	}
}