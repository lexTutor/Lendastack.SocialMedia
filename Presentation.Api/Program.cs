using Application.Infrastructure.Data.DbContext;
using Application.Infrastructure.Data.Seed;
using Application.Infrastructure.Middleware;
using Application.Infrastructure.Models.Configurations;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Presentation.Api.Extensions;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfigurations();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true;
    options.MimeTypes =
        ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "text/html", "application/json", "text/plain" });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.Configure<JwtConfiguration>(o => builder.Configuration.GetSection(nameof(JwtConfiguration)).Bind(o));
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

builder.AddServiceDependencies();

builder.Logging.AddApplicationLogging();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<SecureHeadersMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    appContext.Database.Migrate();

    var seeder = scope.ServiceProvider.GetService<SeedDatabase>();
    seeder?.SeedData().Wait();
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
