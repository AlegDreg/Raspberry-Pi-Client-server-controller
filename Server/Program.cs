using Microsoft.AspNetCore.SignalR;
using Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddMvc();
var app = builder.Build();


ControllerActions._hubContext = app.Services.GetService<IHubContext<ChatHub>>();

app.UseRouting();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
});
app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/bot");

new AI(new RobotActions());

app.Run();