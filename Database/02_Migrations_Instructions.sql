-- Script de migración manual para HomeFlow
-- Este script puede ejecutarse directamente en SQL Server si no se usan migraciones automáticas

-- Las migraciones EF Core se generarán con:
-- 1. Abrir Package Manager Console en Visual Studio
-- 2. Seleccionar HomeFlow.Infrastructure como proyecto por defecto
-- 3. Ejecutar: Add-Migration Initial
-- 4. Ejecutar: Update-Database

-- Alternativamente, si prefieres hacerlo manualmente:
-- 1. Ejecuta el script: Database/01_CreateDatabase_HomeFlow.sql
-- 2. Luego ejecuta este script para aplicar ajustes finales

-- ============================================================
-- Verificación post-migración
-- ============================================================
/* Ejecuta esto para verificar que todo está correcto:

USE HomeFlow;

-- Verificar número de tablas
SELECT COUNT(*) as TablasTotales 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo';

-- Verificar tablas de referencia con datos
SELECT 'EstadoPropiedad' as Tabla, COUNT(*) as Registros FROM dbo.EstadoPropiedad
UNION ALL
SELECT 'CategoriaPropiedad', COUNT(*) FROM dbo.CategoriaPropiedad
UNION ALL
SELECT 'TipoPropiedad', COUNT(*) FROM dbo.TipoPropiedad
UNION ALL
SELECT 'EstadoCivil', COUNT(*) FROM dbo.EstadoCivil
UNION ALL
SELECT 'Region', COUNT(*) FROM dbo.Region
UNION ALL
SELECT 'Comuna', COUNT(*) FROM dbo.Comuna;

-- Verificar índices
SELECT TABLE_NAME, INDEX_NAME, COLUMN_NAME
FROM INFORMATION_SCHEMA.STATISTICS
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME, INDEX_NAME;

*/
