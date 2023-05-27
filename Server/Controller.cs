using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class ActionObj
    {
        public int Port;
        public bool R;
    }

    public static class ControllerActions
    {
        public static string ConId;
        public static IHubContext<ChatHub> _hubContext;
        private static CancellationToken _cancellationTokenSource = new CancellationToken();

        public static void Move(string action)
        {
            _hubContext.Clients.All.SendAsync("NewAction", action);
        }

        public static void Move(List<ActionObj> objs, int millisec)
        {
            _hubContext.Clients.All.SendAsync("NewActionStrong", Newtonsoft.Json.JsonConvert.SerializeObject(objs),
                millisec);
        }

        public static async Task<bool> IsOnline()
        {
            try
            {
                return await _hubContext.Clients.Client(ConId).InvokeAsync<bool>("online", _cancellationTokenSource);
            }
            catch
            {
                return false;
            }
        }

        public static async Task<int> GetLenght()
        {
            try
            {
                return await _hubContext.Clients.Client(ConId).InvokeAsync<int>("len", _cancellationTokenSource);
            }
            catch
            {
                return -1;
            }
        }
    }
}