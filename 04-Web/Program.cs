using Application.Commands.CreateTranslationRequest;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

// MVC + MediatR
builder.Services.AddControllersWithViews();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateTranslationRequestHandler).Assembly);
});

//FluentValidations
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserValidator).Assembly);

//Repositories and Services
builder.Services.AddScoped<ITranslationRequestRepository, TranslationRequestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseRouting();


app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["AuthToken"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Authorization = $"Bearer {token}";
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
