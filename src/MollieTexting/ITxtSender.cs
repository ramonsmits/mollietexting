namespace MollieTexting
{
	public interface ITxtSender
	{
		void Send(string originator, string message, params string[] recipients);
	}
}