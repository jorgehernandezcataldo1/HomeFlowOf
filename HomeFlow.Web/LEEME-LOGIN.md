# Login - HomeFlow.Web

## Qué se agregó

```
HomeFlow.Web/
├── Controllers/
│   └── AuthController.cs          (nuevo) vistas de login/logout con cookie
├── Models/Auth/
│   └── LoginViewModel.cs          (nuevo) validación de formato del form
├── Views/
│   ├── _ViewImports.cshtml        (nuevo)
│   ├── _ViewStart.cshtml          (nuevo)
│   ├── Auth/
│   │   ├── Login.cshtml           (nuevo)
│   │   └── AccessDenied.cshtml    (nuevo)
│   └── Shared/
│       ├── _Layout.cshtml         (nuevo) shell interno (navbar + sidebar)
│       ├── _LoginLayout.cshtml    (nuevo) shell para pantallas públicas
│       └── _SidebarNav.cshtml     (nuevo) menú, con placeholders
├── wwwroot/css/
│   ├── site.css                   (nuevo) variables de marca
│   └── auth.css                   (nuevo) estilos del login
└── Program.cs                     (modificado, ver abajo)
```

Copia estas carpetas/archivos directamente sobre tu `HomeFlow.Web`
(respetan la misma estructura de carpetas). Revisa el `Program.cs` con un
diff antes de reemplazarlo, por si tienes cambios locales que no me
compartiste.

## Cambios clave en Program.cs

1. `LoginPath`, `LogoutPath` y `AccessDeniedPath` ahora apuntan a
   `/Auth/...` en lugar de `/Account/...`, porque `AccountController.cs`
   ya existe y es tu controlador **API** (`api/account`, responde JSON).
   Mezclar ambos en una sola clase `AccountController` no compila (nombre
   duplicado), así que separé responsabilidades:
   - `AccountController` (existente) → API JSON para clientes externos
     (apps móviles, Postman, un futuro front separado).
   - `AuthController` (nuevo) → vistas MVC que usa el navegador.
     Ambos llaman al mismo `IAuthService`, así la regla de negocio
     vive en un solo lugar.

2. Se agregó una **política de autorización global**
   (`RequireAuthenticatedUser`) sobre `AddControllersWithViews`. Esto
   significa que **todos** los controladores (incluyendo
   `PropertiesController`, `ClientesController` y las acciones de
   `AccountController` que no sean login/registro) ahora exigen sesión
   iniciada. Es la postura correcta para un sistema con datos de clientes
   (RUT, ingresos, fotos de carnet, etc.): por defecto todo cerrado, y se
   abre explícitamente con `[AllowAnonymous]` donde corresponda (ya lo
   tienen `Login` y `Register` del `AccountController` API).

   Si necesitas probar algún endpoint sin login mientras desarrollas,
   agrégale `[AllowAnonymous]` temporalmente en vez de sacar el filtro
   global.

## Cómo probar

1. Compila y corre `HomeFlow.Web`. Como todo quedó protegido por defecto,
   cualquier ruta (incluida `/`) te redirigirá a `/Auth/Login`.
2. Necesitas al menos un `Usuario` en la tabla `Usuarios` con un
   `PasswordHash` generado por `IAuthService.HashPassword(...)` (usa
   `PasswordHasher<Usuario>` de ASP.NET Identity, no MD5/SHA1 plano). Si
   aún no tienes ninguno, la forma más simple por ahora es un pequeño
   endpoint temporal o un seed en `ApplyMigrations()` que llame a
   `HashPassword` y lo inserte -avísame si quieres que te arme ese seed.
3. Al iniciar sesión con éxito, `AuthController` redirige a
   `Dashboard/Index`. Si tu página de inicio real tiene otro nombre,
   ajusta `RedirectToLocal` en `AuthController.cs`.

## Notas de seguridad ya cubiertas

- **Sin SQL crudo**: todo el acceso a datos pasa por EF Core con LINQ
  (`Repository<T>`), que parametriza las consultas automáticamente → no
  hay superficie para inyección SQL mientras no se agreguen `FromSqlRaw`
  con texto concatenado.
- **Contraseñas**: `PasswordHasher<Usuario>` (PBKDF2 + salt interno de
  ASP.NET Identity), nunca en texto plano.
- **CSRF**: `[ValidateAntiForgeryToken]` en los POST + `@Html.AntiForgeryToken()`
  en el form.
- **Cookie de sesión**: `HttpOnly`, `SameSite=Lax`, expira a las 8h con
  sliding expiration.
- **Autorización por defecto cerrada** (ver punto anterior).

## Pendientes que detecté (no bloquean el login, pero anótalos)

- `AccountController.ChangePassword` usa `int corredorId = 1;` como
  placeholder. Ahora que hay autenticación real, cámbialo por
  `int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))`.
- `ApplyMigrations()` usa `EnsureCreated()`, que **no** genera archivos de
  migración ni te deja versionar cambios de esquema. Está bien para el
  primer prototipo, pero antes de tocar producción conviene pasar a
  `Add-Migration` / `Update-Database` como indica tu propio
  `Database/MIGRACIONES_EFCORE.md`.
- Los links del sidebar (`Clientes`, `Propiedades`, `Visitas`,
  `Checklists`, `Contratos`) son placeholders: hoy esos módulos solo
  existen como controladores **API**, no como vistas MVC. Cuando
  construyamos cada módulo, se reemplazan por las rutas reales.
