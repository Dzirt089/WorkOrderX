using Microsoft.AspNetCore.SignalR;

namespace WorkOrderX.Application.Hubs
{
	public class ProcessRequestChangedHub : Hub
	{
		public async Task NotifyChangedProcessRequest(string message)
		{
			await Clients.All.SendAsync("NewProcessRequestCreated", message);
		}
	}
}
