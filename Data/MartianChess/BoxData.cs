namespace happygames.Data.MartianChess
{
    public class BoxData
    {
        public PlayerData? player { get; set; }
        public PawnData? pawn { get; set; }
        public bool isPossibleDisplace { get; set; }

        public BoxData(PlayerData? player, PawnData? pawn, bool isPossibleDisplace)
        {
            this.player = player;
            this.pawn = pawn;
            this.isPossibleDisplace = isPossibleDisplace;
        }
    }
}