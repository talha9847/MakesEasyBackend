using System.Text;
using MakesEasy.Interfaces;
using MakesEasy.Repo;
using MakesEasy.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.MapInboundClaims = false; // <--- Add this line here



    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                context.Token = context.Request.Cookies["AuthToken"];
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: **Bearer eyJhbGciOiJI...**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // Set session timeout to 30 days
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Scoped Services for Repositories and JWT Service
var conn = builder.Configuration.GetConnectionString("conn");
builder.Services.AddScoped<IUserInterface, UserRepo>(provider => new UserRepo(conn));
builder.Services.AddScoped<ILocationInterface, LocationRepo>(provider => new LocationRepo(conn));
builder.Services.AddScoped<IPeopleInterface, PeopleRepo>(provider => new PeopleRepo(conn));
builder.Services.AddScoped<IMessageInterface, MessageRepo>(provider => new MessageRepo(conn));
builder.Services.AddScoped<IFourMonthInterface, FourMonthRepo>(provider => new FourMonthRepo(conn));
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<EmailService>();

// Add controllers and endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}












//  var token = _jwtService.GenerateJwtToken(user);

//                 HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
//                 {
//                     HttpOnly = true,
//                     Secure = true,
//                     SameSite = SameSiteMode.Strict,
//                     Expires = DateTime.UtcNow.AddDays(30)
//                 }); 