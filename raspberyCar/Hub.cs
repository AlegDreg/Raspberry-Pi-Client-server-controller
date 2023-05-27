using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace raspberyCar
{
    public class Hub
    {
        private HubConnection hub;
        private System.Timers.Timer Timer;
        private GpioControl GpioControl = new GpioControl();
        private Movements Movements;
        private Distance Distance;

        public Hub(string url)
        {
            Movements = new Movements(GpioControl);
            Distance = new Distance(GpioControl);
            Init(url);
        }

        public async void Init(string url)
        {
            if (hub != null)
            {
                try
                {
                    await hub.DisposeAsync();
                }
                catch { }
            }

            hub = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(url)
                .Build();

            Timer = new System.Timers.Timer(5000);
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();

            EndPoints();

            Connect();
        }

        private void EndPoints()
        {
            hub.Remove("NewAction");
            hub.On<string>("NewAction", (action) =>
            {
                try
                {
                    Movements.Move(action);
                    Console.WriteLine(action);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка move - " + ex.Message + " -- " + ex.InnerException);
                }
            });

            hub.Remove("NewActionStrong");
            hub.On<string, int>("NewActionStrong", (json, millsec) =>
            {
                try
                {
                    Movements.Move(Newtonsoft.Json.JsonConvert.DeserializeObject<List<ActionObj>>(json), millsec);
                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка move - " + ex.Message + " -- " + ex.InnerException);
                }
            });

            hub.Remove("online");
            hub.On<bool>("online", async () =>
            {
                return await Task.FromResult(true);
            });

            hub.Remove("len");
            hub.On<int>("len", async () =>
            {
                return await Task.FromResult(Distance.Get());
            });
        }

        private void Connect()
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await hub.StartAsync();
                    Console.WriteLine("Хаб подключен!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now + " - " + ex.Message + " -- " + ex.InnerException);
                }
            });
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (hub.State == HubConnectionState.Disconnected && hub.State != HubConnectionState.Connecting)
            {
                Console.WriteLine("Соединения с хабом нет. Подключаюсь");
                Connect();
            }
        }

        ~Hub()
        {
            try
            {
                hub.DisposeAsync();
            }
            catch { }

            try
            {
                Timer.Dispose();
            }
            catch { }

            GpioControl.Dispose();
        }
    }
}
