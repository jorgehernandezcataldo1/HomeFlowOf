using HomeFlow.Infrastructure;
using HomeFlow.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Por defecto se exige un usuario autenticado en TODOS los controladores
// (web y api). Las acciones públicas (login, etc.) se marcan explícitamente
// con [AllowAnonymous]. Esto es intencional: es más seguro partir "cerrado"
// y abrir puntualmente, que partir abierto y olvidar proteger algo.
builder.Services.AddControllersWithViews(options =>
{
    var requireAuthenticatedUser = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUser));
});

// Add Infrastructure - Entity Framework y Repositorios
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString);

// Add Application Services - Servicios y Validadores
builder.Services.AddApplication();

// Autenticación por cookies (login del corredor / usuario de la empresa)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = "HomeFlow.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// CORS (útil si más adelante consumes la API desde otra app / móvil)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Aplica migraciones EF Core automáticamente al iniciar
app.ApplyMigrations();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowLocalhost");

// El orden importa: Authentication SIEMPRE antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
