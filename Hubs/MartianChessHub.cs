using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using happygames.Data.MartianChess;

namespace happygames.Hubs
{
    public class MartianChessHub : Hub
    {
        public const string HubUrl = "/martianchesshub";
        private static ConcurrentDictionary<string, Game> groups = new ConcurrentDictionary<string, Game>();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            string guid;
            if (groups.Where(item => !item.Value.isPlayerCompleted()).Count() != 0)
            {
                guid = groups.Where(item => !item.Value.isPlayerCompleted()).ToDictionary(item => item.Key, item => item.Value).First().Key;
            }
            else
            {
                guid = Guid.NewGuid().ToString();
                groups.TryAdd(guid, new Game());
            }
            Context.Items.Add("group", guid);
            Context.Items.Add("player", new Player(Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, guid.ToString());
            groups[guid].initializePlayer((Context.Items["player"] as Player)!);
            if (groups[guid].isPlayerCompleted())
            {
                groups[guid].initializeGame(3);
                await Clients.Group(guid).SendAsync("isGame", true);
                await OnBoard();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} disconnected");
            groups[(Context.Items["group"] as string)!].removePlayer((Context.Items["player"] as Player)!);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnBoard()
        {
            string guid = (Context.Items["group"] as string)!;
            Console.WriteLine(groups[guid].getBoard());
            await Clients.Group(guid).SendAsync("OnBoard", groups[guid].getBoard());
        }

        public async Task OnDisplace(int x, int y)
        {
            Coordinate coordinate = new Coordinate(x, y);
            string guid = (Context.Items["group"] as string)!;
            if (groups[guid].getIsDisplace() == true)
            {
                try
                {
                    groups[guid].setCoordDestinationDisplacement(coordinate);
                    groups[guid].displace(groups[guid].getCoordOriginDisplacement().getX(), groups[guid].getCoordOriginDisplacement().getY(),
                    groups[guid].getCoordDestinationDisplacement().getX(), groups[guid].getCoordDestinationDisplacement().getY(), (Context.Items["player"] as Player));
                    groups[guid].changePlayer();
                    await OnBoard();
                }
                catch (DisplacementException e)
                {
                    await Clients.Caller.SendAsync("OnShowError", groups[guid].getCurrentPlayer()!.getUsername(), e.Message);
                }
                groups[guid].setIsDisplace(false);
            }
            else
            {
                groups[guid].setCoordOriginDisplacement(coordinate);
                groups[guid].setIsDisplace(true);
            }
        }
    }
}