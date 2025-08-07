var builder = WebApplication.CreateBuilder(args);

builder.AddWebApi();

var app = builder.Build();

app.UseWebApi();

app.Run();
