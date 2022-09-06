using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using happygames.Data.MartianChess;
using happygames.Hubs;

namespace happygames.Pages.MartianChess
{
    public partial class MartianChess : ComponentBase
    {
        [Inject]
        NotificationService notificationService { get; set; } = default!;
        [Inject]
        NavigationManager navigationManager { get; set; } = default!;

        private HubConnection? hubConnection;
        private string? hubUrl;
        private BoardData? board;
        private bool isGame;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                string baseUrl = navigationManager.BaseUri;
                hubUrl = baseUrl.TrimEnd('/') + MartianChessHub.HubUrl;
                hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
                hubConnection.On<bool>("isGame", isGameSignalR);
                hubConnection.On<BoardData>("OnBoard", OnBoardSignalR);
                hubConnection.On<NotificationSeverity, string, string>("OnNotification", OnNotification);
                await hubConnection.StartAsync();
            }
            catch (Exception)
            {
                isGame = false;
            }
        }

        public void Dispose()
        {
            hubConnection!.StopAsync();
            hubConnection!.DisposeAsync();
        }

        private void isGameSignalR(bool isGame)
        {
            this.isGame = isGame;
            StateHasChanged();
        }

        private void OnBoardSignalR(BoardData board)
        {
            this.board = board;
            StateHasChanged();
        }

        private async Task displace(CoordinateData coordinate)
        {
            await hubConnection!.SendAsync("OnDisplace", coordinate);
        }

        private void OnNotification(NotificationSeverity severity, string summary, string detail)
        {
            NotificationMessage notificationMessage = new NotificationMessage
            {
                Severity = severity,
                Summary = summary,
                Detail = detail,
                Duration = 4000
            };
            notificationService.Notify(notificationMessage);
        }
    }
}