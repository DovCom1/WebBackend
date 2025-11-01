using WebBackend.Api.Extensions;
using WebBackend.Api.Handlers;
using WebBackend.Api.Hubs;
using WebBackend.Api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "TokenScheme";
    options.DefaultChallengeScheme = "TokenScheme";
})
.AddScheme<TokenAuthOptions, TokenAuthHandler>("TokenScheme", options => { });
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
app.UseCors("frontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<UserHub>("/user/hub");
app.MapControllers();
app.Run();