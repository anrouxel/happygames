namespace happygames.Data.MartianChess
{
    public class PawnData
    {
        public string pawn { get; set; }
        public int score { get; set; }

        public PawnData(string pawn, int score)
        {
            this.pawn = pawn;
            this.score = score;
        }
    }
}