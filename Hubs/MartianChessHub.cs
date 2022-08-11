using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using happygames.Models.MartianChess;
using happygames.Data.MartianChess;

namespace happygames.Hubs
{
    // [Authorize]
    public class MartianChessHub : Hub
    {
        public const string HubUrl = "/martianchesshub";
        private static ConcurrentDictionary<string, Game> groups = new ConcurrentDictionary<string, Game>();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.UserIdentifier} connected");
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
            Context.Items.Add("player", new Player(Context.UserIdentifier!, Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, guid.ToString());
            groups[guid].initializePlayer((Context.Items["player"] as Player)!);
            if (groups[guid].isPlayerCompleted())
            {
                groups[guid].initializeGame(3);
                await OnBoard();
                await Clients.Group(guid).SendAsync("isGame", true);
                await Clients.Group(guid).SendAsync("OnNotificationInfo", "", "La partie commence");
                await OnCurrentPlayer();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.UserIdentifier} disconnected");
            string guid = (Context.Items["group"] as string)!;
            if (groups[guid].isPlayerCompleted())
            {
                groups[guid].removePlayer((Context.Items["player"] as Player)!);
                await Clients.Group(guid).SendAsync("isGame", false);
                await Clients.Group(guid).SendAsync("OnNotificationWarning", "", $"Le joueur {(Context.Items["player"] as Player)!.getUsername()} a quitté la partie.");
            }
            else
            {
                groups.Remove(guid, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnBoard()
        {
            string guid = (Context.Items["group"] as string)!;
            await Clients.Group(guid).SendAsync("OnBoard", groups[guid].getBoard().Clone());
        }

        public async Task OnCurrentPlayer()
        {
            string guid = (Context.Items["group"] as string)!;
            await Clients.Client(groups[guid].getCurrentPlayer()!.getConnectionId()).SendAsync("OnNotificationInfo", groups[guid].getCurrentPlayer()!.getUsername(), "C'est à ton tour de jouer.");
        }

        public async Task OnDisplace(CoordinateData coordinateData)
        {
            Coordinate coordinate = new Coordinate(coordinateData.x, coordinateData.y);
            string guid = (Context.Items["group"] as string)!;
            if ((Context.Items["player"] as Player) == groups[guid].getCurrentPlayer())
            {
                if (groups[guid].getIsDisplace() == true)
                {
                    try
                    {
                        groups[guid].setCoordDestinationDisplacement(coordinate);
                        groups[guid].displace(groups[guid].getCoordOriginDisplacement().getX(), groups[guid].getCoordOriginDisplacement().getY(),
                        groups[guid].getCoordDestinationDisplacement().getX(), groups[guid].getCoordDestinationDisplacement().getY(), (Context.Items["player"] as Player));
                        groups[guid].changePlayer();
                        await OnBoard();
                        if (groups[guid].stopGame())
                        {
                            await Clients.Group(guid).SendAsync("OnNotificationSuccess", "Fin de la partie", $"Le vainqueur est {groups[guid].winnerPlayer()}.");
                        }
                        else
                        {
                            await OnCurrentPlayer();
                        }
                    }
                    catch (DisplacementException e)
                    {
                        await Clients.Caller.SendAsync("OnNotificationError", (Context.Items["player"] as Player)!.getUsername(), e.Message);
                    }
                    groups[guid].setIsDisplace(false);
                }
                else
                {
                    if (groups[guid].possibleDisplacement(coordinate.getX(), coordinate.getY()))
                    {
                        groups[guid].setCoordOriginDisplacement(coordinate);
                        groups[guid].setIsDisplace(true);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("OnNotificationError", (Context.Items["player"] as Player)!.getUsername(), "Le pion ne pourra pas bouger");
                    }
                }
                Console.WriteLine(groups[guid].stopGame());
            }
            else
            {
                await Clients.Caller.SendAsync("OnNotificationError", (Context.Items["player"] as Player)!.getUsername(), "Ce n'est pas à ton tour de jouer.");
            }
        }
    }
}