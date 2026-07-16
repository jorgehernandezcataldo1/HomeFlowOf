# Instrucciones para Applicar Migraciones de Entity Framework Core - HomeFlow

## Opción 1: Migraciones automáticas en startup (RECOMENDADO)

El archivo `Program.cs` ya está configurado para aplicar migraciones automáticamente al iniciar la aplicación.

```csharp
// En Program.cs (ya configurado)
app.ApplyMigrations();
```

Solo tienes que ejecutar la aplicación y las migraciones se aplicarán automáticamente.

---

## Opción 2: Migraciones manuales con Package Manager Console

### Paso 1: Abrir Package Manager Console
- En Visual Studio: `Tools > NuGet Package Manager > Package Manager Console`

### Paso 2: Crear la migración inicial
```powershell
# Selecciona HomeFlow.Infrastructure como proyecto por defecto
Add-Migration Initial

# Esto creará una carpeta "Migrations" en HomeFlow.Infrastructure
# con la clase Initial.cs que contiene todos los cambios
```

### Paso 3: Aplicar la migración a la BD
```powershell
Update-Database
```

### Paso 4: Verificar que se creó correctamente
```sql
-- En SQL Server Management Studio
USE HomeFlow;
SELECT * FROM dbo.__EFMigrationsHistory;
-- Deberías ver un registro con la migración "Initial"
```

---

## Opción 3: CLI de Entity Framework Core

Si prefieres usar la línea de comandos:

```bash
# Crear la migración
dotnet ef migrations add Initial --project HomeFlow.Infrastructure

# Aplicar la migración
dotnet ef database update --project HomeFlow.Infrastructure
```

---

## Próximos pasos si necesitas hacer cambios

Si cambias una entidad en Domain o agregas una nueva y necesitas actualizar la BD:

```powershell
# 1. Crea una nueva migración
Add-Migration MiCambioDescriptivo

# 2. revisa los cambios en la carpeta Migrations
# 3. Aplica los cambios
Update-Database

# 4. Si necesitas deshacer el último cambio
Update-Database -Migration NombreDeLaMigracionAnterior
```

---

## Strings de conexión esperados

En `appsettings.json` debe existir:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HomeFlow;Trusted_Connection=true;"
  }
}
```

O para SQL Server Express:
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=HomeFlow;Trusted_Connection=true;"
```

O para SQL Server con autenticación:
```json
"DefaultConnection": "Server=localhost;Database=HomeFlow;User Id=sa;Password=tuPassword;"
```

---

## Troubleshooting

### "No migrations have been applied"
- Asegúrate de que HomeFlow.Infrastructure está seleccionado como proyecto por defecto
- Verifica que el DbContextFactory existe y está bien configurado

### "The migration 'Initial' has already been applied"
- Significa que ya se aplicó. Ejecuta `dotnet ef migrations list` para ver el historial

### "Unable to create an object of type 'HomeFlowDbContext'"
- Verifica que la conexión en appsettings.json es correcta
- Asegúrate que SQL Server está corriendo

---

## Ver historial de migraciones

```powershell
# Ver todas las migraciones
Get-Migration

# Ver état actual
$update-Database ?WhatIf
```
