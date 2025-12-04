using WebBackend.Api.Extensions;
using WebBackend.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseRouting();
app.UseCors("frontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorizationMiddleware();

app.MapHub<UserHub>("/user/hub");
app.MapHub<CallHub>("/call/hub");

app.MapControllers();
app.Run();