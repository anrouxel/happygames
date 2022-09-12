using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using happygames.Models.MartianChess;
using happygames.Data.MartianChess;
using Radzen;

namespace happygames.Hubs
{
    // [Authorize]
    public class MartianChessHub : Hub
    {
        public const string HubUrl = "/martianchesshub";
        private static ConcurrentDictionary<string, Game> groups = new ConcurrentDictionary<string, Game>();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.User?.Identity?.IsAuthenticated} connected");
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
                await Clients.Group(guid).SendAsync("OnNotification", NotificationSeverity.Info, "", "La partie commence");
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
                await Clients.Group(guid).SendAsync("OnNotification", NotificationSeverity.Warning, "", $"Le joueur {(Context.Items["player"] as Player)!.getUsername()} a quitté la partie.");
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
            await Clients.Client(groups[guid].getCurrentPlayer()!.getConnectionId()).SendAsync("OnNotification", NotificationSeverity.Info, groups[guid].getCurrentPlayer()!.getUsername(), "C'est à ton tour de jouer.");
        }

        public async Task OnDisplace(CoordinateData coordinateData)
        {
            Coordinate coordinate = new Coordinate(coordinateData.x, coordinateData.y);
            string guid = (Context.Items["group"] as string)!;
            if ((Context.Items["player"] as Player) == groups[guid].getCurrentPlayer())
            {
                if (groups[guid].getIsDisplace() == true)
                {
                    groups[guid].setCoordDestinationDisplacement(coordinate);
                    if (groups[guid].possibleDisplacement(groups[guid].getCoordOriginDisplacement().getX(), groups[guid].getCoordOriginDisplacement().getY(), groups[guid].getCoordDestinationDisplacement().getX(), groups[guid].getCoordDestinationDisplacement().getY(), (Context.Items["player"] as Player)))
                    {
                        groups[guid].displace(groups[guid].getCoordOriginDisplacement().getX(), groups[guid].getCoordOriginDisplacement().getY(),
                        groups[guid].getCoordDestinationDisplacement().getX(), groups[guid].getCoordDestinationDisplacement().getY(), (Context.Items["player"] as Player));
                        groups[guid].changePlayer();
                        if (groups[guid].stopGame())
                        {
                            await Clients.Group(guid).SendAsync("OnNotification", NotificationSeverity.Success, "Fin de la partie", $"Le vainqueur est {groups[guid].winnerPlayer()}.");
                        }
                        else
                        {
                            await OnCurrentPlayer();
                        }
                    }
                    await OnBoard();
                    Console.WriteLine($"{(Context.Items["player"] as Player)!.getScore()}");
                    groups[guid].setIsDisplace(false);
                }
                else
                {
                    if (groups[guid].possibleDisplacement(coordinate.getX(), coordinate.getY()) && groups[guid].getBoard().getBoxes()[coordinate.getY(), coordinate.getX()].getPlayer() == (Context.Items["player"] as Player))
                    {
                        groups[guid].setCoordOriginDisplacement(coordinate);
                        groups[guid].setIsDisplace(true);
                        BoardData board = groups[guid].getBoard().Clone();
                        for (int y = 0; y < board.verticalSize; y++)
                        {
                            for (int x = 0; x < board.horizontalSize; x++)
                            {
                                board.boxes![y][x].isPossibleDisplace = groups[guid].possibleDisplacement(groups[guid].getCoordOriginDisplacement().getX(), groups[guid].getCoordOriginDisplacement().getY(), x, y, (Context.Items["player"] as Player));
                            }
                        }
                        board.boxes![coordinate.getY()][coordinate.getX()].isPossibleDisplace = true;
                        await Clients.Caller.SendAsync("OnBoard", board);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("OnNotification", NotificationSeverity.Error, (Context.Items["player"] as Player)!.getUsername(), "Le pion ne pourra pas bouger ou ce n'est pas ton pion");
                    }
                }
            }
            else
            {
                await Clients.Caller.SendAsync("OnNotification", NotificationSeverity.Error, (Context.Items["player"] as Player)!.getUsername(), "Ce n'est pas à ton tour de jouer.");
            }
        }
    }
}