using Kira.Bfff.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddBffInfrastructure(builder.Configuration);
        
        var app = builder.Build();
        app.UseBffPipeline();

        app.Run();
    }
}