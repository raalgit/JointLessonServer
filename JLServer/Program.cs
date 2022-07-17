using JL_Migrator;
using JL_MongoDB;
using JL_MongoDB.Repository;
using JL_MSSQLServer;
using JL_Service;
using JL_SignalR;
using JL_Utility;
using JL_Utility.Logger;
using JL_Utility.Models;
using JLServer;
using JLServer.Consts;
using JLServer.Middleware;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(o => o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new NullReferenceException("ASPNETCORE_ENVIRONMENT");
var configuration = JLConfigurationManager.GetConfiguration(environment);
builder.Services.AddDbContext<ApplicationContext>(options => 
options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 11))));
builder.Services.AddIRepository();
builder.Services.AddSignalR();
//builder.Services.AddScoped<IUserIdProvider, SignalRUserProvider>();
builder.Services.AddScoped<IMongoRepository, MongoRepository>();
builder.Services.AddSingleton<IJLLogger, JLLogger>();
builder.Services.AddScoped<IFileUtility, FileUtility>();
builder.Services.AddScoped<ISignalRUtility, SignalRUtility>();
builder.Services.AddIService();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

var app = builder.Build();

app.UseMiddleware<JWTMiddleware>();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AnyOrigin");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalHub>("/signalHub", x =>
    {
    });
});

app.Run();

