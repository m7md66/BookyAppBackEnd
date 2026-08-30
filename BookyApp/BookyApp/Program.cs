using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Infra.Helper.Extensions;
using Application;
using BookyApp.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.ConfigureKestrel(options =>
//{
//    // ???? Kestrel ????? ??? ?? ????? IP ??? ??? ??????
//    options.ListenAnyIP(5212);

//    // ?? ????? ????? ????:
//    // options.Listen(IPAddress.Parse("192.168.1.100"), 5212);
//});
// Add Application Insights services
builder.Services.AddApplicationInsightsTelemetry();
// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
//    ;
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("sz8eI7OdHBrjrIasdfadsfasdfdfo8j9nTW/asdfasdfarQyO1OvY0pAQ2wDKQZw/0=", options))
//    ;

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddFluentEmail(builder.Configuration);
builder.Services.AddMapster();
builder.Services.AddApplicationRegitrations();


// Localization: shared .resx resources live in the Application project (Application/Resources).
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(Application.SharedResource)));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ar") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
    // Allow ?lang=ar in addition to the Accept-Language header (which is already handled by default).
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider
    {
        QueryStringKey = "lang",
        UIQueryStringKey = "lang"
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Resolve request culture from ?lang / Accept-Language before anything else runs.
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
