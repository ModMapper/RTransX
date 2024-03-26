using RTransX.Models;
using RTransX.Modules;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSingleton(new TokenChecker());
        builder.Services.AddSingleton(DeepLPool.CreatePoolAsync(8).Result);
        builder.Services.AddCors((options) =>
        {
            options.AddPolicy("AllowAll", (builder) => {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });
        });
        var app = builder.Build();
        app.UseCors("AllowAll");
        app.MapControllers();
        app.Run();
    }


}