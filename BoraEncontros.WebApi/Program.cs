var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
//builder.Services.AddApplication();
builder.AddWebApi();

var app = builder.Build();

app.UseWebApi();

app.Run();
