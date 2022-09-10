namespace happygames.Data.MartianChess
{
    public class GameData
    {
        public int nswg { get; set; } // Number of shots without a grip
        public int mnswg { get; set; } // Maximum number of shots without a grip
        public CoordinateData? originCoordinate { get; set; } // Coordinate of origin
        public CoordinateData? destinationCoordinate { get; set; } // Coordinate of destination
        public PlayerData?[]? players { get; set; } // List of players
        public PlayerData? currentPlayer { get; set; }
        public BoardData? board { get; set; }
        public PawnData? backPawn { get; set; }
        public bool isDisplace { get; set; }
    }
}