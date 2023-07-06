using GitHub.API.CachedServices;
using GitHubIntegration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    option => option.AddPolicy(
        "corsPolicy",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
    );

builder.Services.AddMemoryCache();
builder.Services.AddGitHubIntegration(
    options => builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)).Bind(options));
builder.Services.Decorate<IGitHubService, CachedGitHubService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("corsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();