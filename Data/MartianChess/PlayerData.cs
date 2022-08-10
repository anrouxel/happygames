namespace happygames.Data.MartianChess
{
    public class PlayerData
    {
        public string? username {get; set;}
        public List<PawnData>? capturedPawns {get; set;}

        public PlayerData(string username, List<PawnData> capturedPawns)
        {
            this.username = username;
            this.capturedPawns = capturedPawns;
        }
    }
}