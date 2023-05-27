using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine(DateTime.Now + " Клиент подключен");
            ControllerActions.ConId = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(DateTime.Now + " Клиент отключен");
            return base.OnDisconnectedAsync(exception);
        }
    }
}