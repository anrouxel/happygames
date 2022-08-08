using Microsoft.AspNetCore.SignalR;
using happygames.Data.MartianChess;

namespace happygames.Hubs
{
    public class MartianChessHub : Hub
    {
        public const string HubUrl = "/martianchesshub";
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Broadcast(Board board)
        {
            await Clients.All.SendAsync("Broadcast", board);
        }
    }
}