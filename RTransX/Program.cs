using RTransX.Models;
using RTransX.Modules;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSingleton(new TokenChecker());
        builder.Services.AddSingleton(DeepLPool.CreatePoolAsync(8).Result);
        var app = builder.Build();
        app.UseCors((builder) => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.MapControllers();
        app.Run();
    }


}