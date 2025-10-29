using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Infra.Helper.Extensions;
using Application;
using BookyApp.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

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


builder.Services.AddControllers();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
