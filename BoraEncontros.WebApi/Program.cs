var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.AddWebApi();

var app = builder.Build();

app.UseWebApi();

app.Run();
