using Auth0.AuthenticationApi;
using Main.Service.Auth0.Contract;
using Main.Service.Auth0.Repository;
using Main.Service.RabbitMQ;
using Main.Service.Services;
using Main.Service.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthenticationConnection, HttpClientAuthenticationConnection>();


//Add Htttpclient for services
builder.Services.AddHttpClient<IWeatherService, WeatherService>();


builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Main Service API",
        Description = "To demo Auth0, Rabbit MQ"
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Open Id" }
                },
                AuthorizationUrl = new Uri(StaticConfigurationManager.AppSetting["Authentication:Domain"] + "authorize?audience=" + StaticConfigurationManager.AppSetting["Authentication:Audience"])
            }
        }
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = StaticConfigurationManager.AppSetting["Authentication:Domain"];
    options.Audience = StaticConfigurationManager.AppSetting["Authentication:Audience"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.OAuthClientId(StaticConfigurationManager.AppSetting["Authentication:ClientId"]);
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
