namespace happygames.Data.MartianChess
{
    public class PlayerData
    {
        public string? username { get; set; }
        public string? connectionId { get; set; }
        public List<PawnData>? capturedPawns { get; set; }
    }
}