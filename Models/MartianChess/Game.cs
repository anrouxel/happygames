using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class Game : InterfaceGame
    {
        private int nswg; // Number of shots without a grip
        private int mnswg; // Maximum number of shots without a grip
        private Coordinate? originCoordinate; // Coordinate of origin
        private Coordinate? destinationCoordinate; // Coordinate of destination
        private Player?[] players = new Player[2]; // List of players
        private Player? currentPlayer;
        private Board board = new Board();
        private Pawn? backPawn = null;
        private bool isDisplace;

        public GameData Clone()
        {
            PlayerData?[] playersData = new PlayerData[2];
            for (int i = 0; i < players.Count(); i++)
            {
                playersData[i] = players[i]!.Clone();
            }
            return new GameData { nswg = nswg, mnswg = mnswg, originCoordinate = originCoordinate!.Clone(), destinationCoordinate = destinationCoordinate!.Clone(), players = playersData, currentPlayer = currentPlayer!.Clone(), board = board.Clone(), backPawn = backPawn!.Clone(), isDisplace = isDisplace };
        }

        public Coordinate getCoordOriginDisplacement()
        {
            return originCoordinate!;
        }

        public Coordinate getCoordDestinationDisplacement()
        {
            return destinationCoordinate!;
        }

        public void setCoordOriginDisplacement(Coordinate coordinate)
        {
            originCoordinate = coordinate;
        }

        public void setCoordDestinationDisplacement(Coordinate coordinate)
        {
            destinationCoordinate = coordinate;
        }

        public Player? getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void initializeGame(int mnswg)
        {
            this.mnswg = mnswg;
            nswg = 0;
            board = new Board();
            board.initialize();
            currentPlayer = players[0];
            backPawn = null;
            isDisplace = false;
            originCoordinate = null;
            destinationCoordinate = null;
            for (int y = 0; y < board.getVerticalSize() / 2; y++)
            {
                for (int x = 0; x < board.getHorizontalSize(); x++)
                {
                    board.getBoxes()[y, x].setPlayer(players[0]!);
                    board.getBoxes()[board.getVerticalSize() - 1 - y, x].setPlayer(players[1]!);
                }
            }
        }

        public void initializePlayer(Player player)
        {
            players[Array.IndexOf(players, null)] = player;
        }

        public void removePlayer(Player player)
        {
            players[Array.IndexOf(players, player)] = null;
        }

        public bool isPlayerCompleted()
        {
            return !players.Contains(null);
        }

        public bool possibleDisplacement(int coordOriginX, int coordOriginY)
        {
            for (int y = coordOriginY - 1; y <= coordOriginY + 1; y++)
            {
                for (int x = coordOriginX - 1; x <= coordOriginX + 1; x++)
                {
                    if (x >= 0 && x < board.getHorizontalSize() && y >= 0 && y < board.getVerticalSize() && (coordOriginX != x || coordOriginY != y))
                    {
                        if (board.getBoxes()[y, x].getPawn() == null)
                        {
                            try
                            {
                                board.getBoxes()[coordOriginY, coordOriginX].getPawn()!.getDisplacement(new Displacement(new Coordinate(coordOriginX, coordOriginY), new Coordinate(x, y)));
                                return true;
                            }
                            catch (DisplacementException) { }
                        }
                    }
                }
            }
            return false;
        }

        public bool possibleDisplacement(int coordOriginX, int coordOriginY, int coordDestinationX, int coordDestinationY, Player? player)
        {
            if (player == currentPlayer)
            {
                if (coordOriginX >= 0 && coordOriginX < board.getHorizontalSize() && coordDestinationX >= 0 && coordDestinationX < board.getHorizontalSize() && coordOriginY >= 0 && coordOriginY < board.getVerticalSize() && coordDestinationY >= 0 && coordDestinationY < board.getVerticalSize())
                {
                    if (player == board.getBoxes()[coordOriginY, coordOriginX].getPlayer() && board.getBoxes()[coordOriginY, coordOriginX].getPawn() is Pawn && (board.getBoxes()[coordOriginY, coordOriginX].getPawn() != backPawn || player == board.getBoxes()[coordDestinationY, coordDestinationX].getPlayer()))
                    {
                        List<Coordinate>? displacement = null;
                        try
                        {
                            displacement = board.getBoxes()[coordOriginY, coordOriginX].getPawn()!.getDisplacement(new Displacement(new Coordinate(coordOriginX, coordOriginY), new Coordinate(coordDestinationX, coordDestinationY)));
                            for (int i = 1; i < displacement.Count() - 1; i++)
                            {
                                if (board.getBoxes()[displacement[i].getY(), displacement[i].getX()].getPawn() is Pawn)
                                {
                                    throw new DisplacementException("Il y a pion sur ton chemin");
                                }
                            }
                            if (board.getBoxes()[displacement.Last().getY(), displacement.Last().getX()].getPlayer() == player && board.getBoxes()[displacement.Last().getY(), displacement.Last().getX()].getPawn() is Pawn)
                            {
                                throw new DisplacementException("Tu ne peux pas déplacer ce pion sur un de tes pions");
                            }
                            return true;
                        }
                        catch (DisplacementException) { }
                    }
                }
            }
            return false;
        }

        public void displace(int coordOriginX, int coordOriginY, int coordDestinationX, int coordDestinationY, Player? player)
        {
            if (possibleDisplacement(coordOriginX, coordOriginY, coordDestinationX, coordDestinationY, player))
            {
                if (board.getBoxes()[coordDestinationY, coordDestinationX].getPawn() == null)
                {
                    nswg += 1;
                }
                else
                {
                    currentPlayer?.addCapturedPawn(board.getBoxes()[coordDestinationY, coordDestinationX].getPawn()!);
                }
                backPawn = board.getBoxes()[coordOriginY, coordOriginX].getPawn();
                board.getBoxes()[coordDestinationY, coordDestinationX].setPawn(board.getBoxes()[coordOriginY, coordOriginX].getPawn()!);
                board.getBoxes()[coordOriginY, coordOriginX].setPawn(null);
            }
        }

        public Player? winnerPlayer()
        {
            if (players[0]?.getScore() > players[1]?.getScore())
            {
                return players[0];
            }
            else if (players[0]?.getScore() < players[1]?.getScore())
            {
                return players[1];
            }
            else
            {
                return null;
            }
        }

        public void changePlayer()
        {
            currentPlayer = players[(Array.IndexOf(players, currentPlayer) + 1) % 2];
        }

        public bool stopGame()
        {
            int pawnPlayer1 = 0;
            int pawnPlayer2 = 0;
            for (int y = 0; y < board.getVerticalSize() / 2; y++)
            {
                for (int x = 0; x < board.getHorizontalSize(); x++)
                {
                    if (board.getBoxes()[y, x].getPawn() != null)
                    {
                        pawnPlayer1 += 1;
                    }
                    if (board.getBoxes()[board.getVerticalSize() - 1 - y, x].getPawn() != null)
                    {
                        pawnPlayer2 += 1;
                    }
                }
            }
            if (pawnPlayer1 <= 0 || pawnPlayer2 <= 0 || nswg >= mnswg)
            {
                return true;
            }
            return false;
        }

        public Board getBoard()
        {
            return board;
        }

        public bool getIsDisplace()
        {
            return isDisplace;
        }

        public void setIsDisplace(bool isDisplace)
        {
            this.isDisplace = isDisplace;
        }
    }
}