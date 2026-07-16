-- ============================================================
-- SCRIPT DE CREACIÓN - Sistema de Gestión de Propiedades HomeFlow
-- SQL Server LocalDB
-- Fecha: 2024
-- Descripción: Diseño completo de base de datos con normalizacion 3NF
-- ============================================================

-- ============================================================
-- 1. TABLAS DE REFERENCIA / CATÁLOGOS
-- ============================================================

-- Tabla: Estados de Propiedad
IF OBJECT_ID('dbo.EstadoPropiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.EstadoPropiedad;

CREATE TABLE dbo.EstadoPropiedad (
	EstadoPropiedadId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL UNIQUE,
	Descripcion NVARCHAR(255),
	EsActivo BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla: Categorías de Propiedad
IF OBJECT_ID('dbo.CategoriaPropiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.CategoriaPropiedad;

CREATE TABLE dbo.CategoriaPropiedad (
	CategoriaPropiedadId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL UNIQUE,
	Descripcion NVARCHAR(255),
	EsActivo BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla: Tipos de Documento
IF OBJECT_ID('dbo.TipoDocumento', 'U') IS NOT NULL 
	DROP TABLE dbo.TipoDocumento;

CREATE TABLE dbo.TipoDocumento (
	TipoDocumentoId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(80) NOT NULL UNIQUE,
	Descripcion NVARCHAR(255),
	PlantillaHtml NVARCHAR(MAX),
	EsActivo BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla: Estados Civiles
IF OBJECT_ID('dbo.EstadoCivil', 'U') IS NOT NULL 
	DROP TABLE dbo.EstadoCivil;

CREATE TABLE dbo.EstadoCivil (
	EstadoCivilId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(30) NOT NULL UNIQUE,
	EsActivo BIT NOT NULL DEFAULT 1
);

-- Tabla: Tipos de Propiedad (Departamento, Casa, Oficina, etc.)
IF OBJECT_ID('dbo.TipoPropiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.TipoPropiedad;

CREATE TABLE dbo.TipoPropiedad (
	TipoPropiedadId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL UNIQUE,
	Descripcion NVARCHAR(255),
	EsActivo BIT NOT NULL DEFAULT 1
);

-- Tabla: Regiones de Chile (Para ubicación geográfica)
IF OBJECT_ID('dbo.Region', 'U') IS NOT NULL 
	DROP TABLE dbo.Region;

CREATE TABLE dbo.Region (
	RegionId INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(100) NOT NULL UNIQUE,
	Codigo NVARCHAR(10),
	EsActivo BIT NOT NULL DEFAULT 1
);

-- Tabla: Comunas
IF OBJECT_ID('dbo.Comuna', 'U') IS NOT NULL 
	DROP TABLE dbo.Comuna;

CREATE TABLE dbo.Comuna (
	ComunaId INT PRIMARY KEY IDENTITY(1,1),
	RegionId INT NOT NULL,
	Nombre NVARCHAR(100) NOT NULL,
	EsActivo BIT NOT NULL DEFAULT 1,
	FOREIGN KEY (RegionId) REFERENCES dbo.Region(RegionId),
	UNIQUE(RegionId, Nombre)
);

-- ============================================================
-- 2. TABLAS PRINCIPALES - USUARIOS Y CLIENTES
-- ============================================================

-- Tabla: Corredores (Usuarios del Sistema)
IF OBJECT_ID('dbo.Corredor', 'U') IS NOT NULL 
	DROP TABLE dbo.Corredor;

CREATE TABLE dbo.Corredor (
	CorredorId INT PRIMARY KEY IDENTITY(1,1),
	Rut NVARCHAR(15) NOT NULL UNIQUE,
	Nombre NVARCHAR(255) NOT NULL,
	Apellido NVARCHAR(255) NOT NULL,
	Correo NVARCHAR(255) NOT NULL UNIQUE,
	Telefono NVARCHAR(15),
	PasswordHash NVARCHAR(MAX) NOT NULL,
	PasswordSalt NVARCHAR(MAX),
	Licencia NVARCHAR(50),
	FotoPerfilUrl NVARCHAR(MAX),
	EsActivo BIT NOT NULL DEFAULT 1,
	EsAdmin BIT NOT NULL DEFAULT 0,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	INDEX IDX_Rut (Rut),
	INDEX IDX_Correo (Correo)
);

-- Tabla: Clientes base (Propietarios y Arrendatarios)
IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL 
	DROP TABLE dbo.Cliente;

CREATE TABLE dbo.Cliente (
	ClienteId INT PRIMARY KEY IDENTITY(1,1),
	Rut NVARCHAR(15) NOT NULL UNIQUE,
	Nombre NVARCHAR(255) NOT NULL,
	Apellido NVARCHAR(255) NOT NULL,
	Correo NVARCHAR(255),
	Telefono NVARCHAR(15),
	EstadoCivilId INT,
	Direccion NVARCHAR(255),
	ComunaId INT,
	FotoCarnetUrl NVARCHAR(MAX),
	Notas NVARCHAR(MAX),
	EsActivo BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (EstadoCivilId) REFERENCES dbo.EstadoCivil(EstadoCivilId),
	FOREIGN KEY (ComunaId) REFERENCES dbo.Comuna(ComunaId),
	INDEX IDX_Rut (Rut),
	INDEX IDX_Correo (Correo),
	INDEX IDX_ComunaId (ComunaId)
);

-- Tabla: Propietarios (Subclase de Cliente)
IF OBJECT_ID('dbo.Propietario', 'U') IS NOT NULL 
	DROP TABLE dbo.Propietario;

CREATE TABLE dbo.Propietario (
	PropietarioId INT PRIMARY KEY,
	ClienteId INT NOT NULL UNIQUE,
	BancoPreferido NVARCHAR(100),
	CuentaBancaria NVARCHAR(20),
	TipoCuenta NVARCHAR(20),
	DocumentoIdentidadVerificado BIT NOT NULL DEFAULT 0,
	FechaVerificacion DATETIME,
	RequisitosCompletos BIT NOT NULL DEFAULT 0,
	FechaRequisitosCompletos DATETIME,
	CorredorAsignadoId INT,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente(ClienteId) ON DELETE CASCADE,
	FOREIGN KEY (CorredorAsignadoId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_ClienteId (ClienteId),
	INDEX IDX_CorredorAsignado (CorredorAsignadoId)
);

-- Tabla: Arrendatarios (Subclase de Cliente)
IF OBJECT_ID('dbo.Arrendatario', 'U') IS NOT NULL 
	DROP TABLE dbo.Arrendatario;

CREATE TABLE dbo.Arrendatario (
	ArrendatarioId INT PRIMARY KEY,
	ClienteId INT NOT NULL UNIQUE,
	LiquidoMensual DECIMAL(12,2),
	Empleador NVARCHAR(255),
	AntiguedadLaboral INT, -- en meses
	TieneHijos BIT NOT NULL DEFAULT 0,
	NumeroHijos INT DEFAULT 0,
	TieneMascota BIT NOT NULL DEFAULT 0,
	TipoMascota NVARCHAR(100),
	PreAprobacionCredito BIT NOT NULL DEFAULT 0,
	DocumentacionCompleta BIT NOT NULL DEFAULT 0,
	RequisitosCompletos BIT NOT NULL DEFAULT 0,
	FechaRequisitosCompletos DATETIME,
	CorredorAsignadoId INT,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente(ClienteId) ON DELETE CASCADE,
	FOREIGN KEY (CorredorAsignadoId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_ClienteId (ClienteId),
	INDEX IDX_CorredorAsignado (CorredorAsignadoId)
);

-- ============================================================
-- 3. TABLAS - PROPIEDADES
-- ============================================================

-- Tabla: Propiedades
IF OBJECT_ID('dbo.Propiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.Propiedad;

CREATE TABLE dbo.Propiedad (
	PropiedadId INT PRIMARY KEY IDENTITY(1,1),
	PropietarioId INT NOT NULL,
	TipoPropiedadId INT NOT NULL,
	CategoriaPropiedadId INT NOT NULL,
	EstadoPropiedadId INT NOT NULL DEFAULT 1, -- Pendiente por defecto
	Direccion NVARCHAR(255) NOT NULL,
	ComunaId INT NOT NULL,
	Piso INT,
	Torre NVARCHAR(10),
	CodigoPostal NVARCHAR(10),
	Latitud DECIMAL(10,8),
	Longitud DECIMAL(11,8),
	DescripcionGeneral NVARCHAR(MAX),
	PrecioArriendo DECIMAL(12,2),
	PrecioVenta DECIMAL(15,2),
	Habitaciones INT NOT NULL DEFAULT 1,
	Banos INT NOT NULL DEFAULT 1,
	MetrosCuadrados DECIMAL(8,2),
	Bodega BIT NOT NULL DEFAULT 0,
	Estacionamiento INT DEFAULT 0,
	Terraza BIT NOT NULL DEFAULT 0,
	Jardin BIT NOT NULL DEFAULT 0,
	EquipamientoKitchen NVARCHAR(MAX),
	Ventanas NVARCHAR(255),
	Condominio BIT NOT NULL DEFAULT 0,
	GastosComunes DECIMAL(10,2),
	GastoAgua DECIMAL(10,2),
	GastoLuz DECIMAL(10,2),
	GastoGas DECIMAL(10,2),
	ContribucionesAFecto BIT NOT NULL DEFAULT 0,
	MontoContribuciones DECIMAL(10,2),
	PermiteArriendo BIT NOT NULL DEFAULT 1,
	PermiteVenta BIT NOT NULL DEFAULT 1,
	InfoMetro NVARCHAR(255),
	DistanciaMetro INT, -- en metros
	InfoColegio NVARCHAR(255),
	DistanciaColegio INT, -- en metros
	InfoSupermercado NVARCHAR(255),
	DistanciaSupermercado INT, -- en metros
	RequiereHipoteca BIT NOT NULL DEFAULT 0,
	CorredorAsignadoId INT NOT NULL,
	FotoUrl NVARCHAR(MAX),
	FotosCasaUrl NVARCHAR(MAX), -- JSON array of URLs
	DocumentosUrl NVARCHAR(MAX), -- JSON array of URLs
	Notas NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaPublicacion DATETIME,
	FOREIGN KEY (PropietarioId) REFERENCES dbo.Propietario(PropietarioId),
	FOREIGN KEY (TipoPropiedadId) REFERENCES dbo.TipoPropiedad(TipoPropiedadId),
	FOREIGN KEY (CategoriaPropiedadId) REFERENCES dbo.CategoriaPropiedad(CategoriaPropiedadId),
	FOREIGN KEY (EstadoPropiedadId) REFERENCES dbo.EstadoPropiedad(EstadoPropiedadId),
	FOREIGN KEY (ComunaId) REFERENCES dbo.Comuna(ComunaId),
	FOREIGN KEY (CorredorAsignadoId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_PropietarioId (PropietarioId),
	INDEX IDX_EstadoPropiedadId (EstadoPropiedadId),
	INDEX IDX_ComunaId (ComunaId),
	INDEX IDX_CorredorAsignado (CorredorAsignadoId),
	INDEX IDX_TipoPropiedad (TipoPropiedadId)
);

-- ============================================================
-- 4. TABLAS - PROCESOS Y CHECKLISTS
-- ============================================================

-- Tabla: Checklist Propietario
IF OBJECT_ID('dbo.ChecklistPropietario', 'U') IS NOT NULL 
	DROP TABLE dbo.ChecklistPropietario;

CREATE TABLE dbo.ChecklistPropietario (
	ChecklistPropietarioId INT PRIMARY KEY IDENTITY(1,1),
	PropietarioId INT NOT NULL,
	DocumentoIdentidadVerificado BIT NOT NULL DEFAULT 0,
	ComprobanteUbicacionVerificado BIT NOT NULL DEFAULT 0,
	CuentaBancariaVerificada BIT NOT NULL DEFAULT 0,
	AntecedentesLimpios BIT NOT NULL DEFAULT 0,
	FotoCarnetSubida BIT NOT NULL DEFAULT 0,
	EsCompl BIT NOT NULL DEFAULT 0,
	FechaCompleto DATETIME,
	Comentarios NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (PropietarioId) REFERENCES dbo.Propietario(PropietarioId),
	INDEX IDX_PropietarioId (PropietarioId)
);

-- Tabla: Checklist Propiedad
IF OBJECT_ID('dbo.ChecklistPropiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.ChecklistPropiedad;

CREATE TABLE dbo.ChecklistPropiedad (
	ChecklistPropiedadId INT PRIMARY KEY IDENTITY(1,1),
	PropiedadId INT NOT NULL,
	FotosCompletas BIT NOT NULL DEFAULT 0,
	DocumentacionCompleta BIT NOT NULL DEFAULT 0,
	ServiciosVerificados BIT NOT NULL DEFAULT 0,
	EspaciosVerificados BIT NOT NULL DEFAULT 0,
	CondicioesEstructurales BIT NOT NULL DEFAULT 0,
	DisponiblePublicar BIT NOT NULL DEFAULT 0,
	FechaDisponiblePublicar DATETIME,
	Comentarios NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (PropiedadId) REFERENCES dbo.Propiedad(PropiedadId),
	INDEX IDX_PropiedadId (PropiedadId)
);

-- Tabla: Orden de Visita
IF OBJECT_ID('dbo.OrdenVisita', 'U') IS NOT NULL 
	DROP TABLE dbo.OrdenVisita;

CREATE TABLE dbo.OrdenVisita (
	OrdenVisitaId INT PRIMARY KEY IDENTITY(1,1),
	PropiedadId INT NOT NULL,
	ArrendatarioId INT NOT NULL,
	CorredorId INT NOT NULL,
	FechaVisita DATETIME,
	Direccion NVARCHAR(255),
	HoraInicio DATETIME,
	HoraFin DATETIME,
	Notificado BIT NOT NULL DEFAULT 0,
	FechaNotificacion DATETIME,
	DocumentoUrl NVARCHAR(MAX),
	Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente', -- Pendiente, Confirmada, Completada, Cancelada
	Observaciones NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (PropiedadId) REFERENCES dbo.Propiedad(PropiedadId),
	FOREIGN KEY (ArrendatarioId) REFERENCES dbo.Arrendatario(ArrendatarioId),
	FOREIGN KEY (CorredorId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_PropiedadId (PropiedadId),
	INDEX IDX_ArrendatarioId (ArrendatarioId),
	INDEX IDX_FechaVisita (FechaVisita)
);

-- Tabla: Checklist Arrendatario
IF OBJECT_ID('dbo.ChecklistArrendatario', 'U') IS NOT NULL 
	DROP TABLE dbo.ChecklistArrendatario;

CREATE TABLE dbo.ChecklistArrendatario (
	ChecklistArrendatarioId INT PRIMARY KEY IDENTITY(1,1),
	OrdenVisitaId INT NOT NULL,
	ArrendatarioId INT NOT NULL,
	PropiedadGusta BIT,
	ConformidadEspacios BIT NOT NULL DEFAULT 0,
	ConformidadServicios BIT NOT NULL DEFAULT 0,
	PreguntasRespondidas BIT NOT NULL DEFAULT 0,
	InteresContratar BIT NOT NULL DEFAULT 0,
	FechaCompleto DATETIME,
	Comentarios NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (OrdenVisitaId) REFERENCES dbo.OrdenVisita(OrdenVisitaId),
	FOREIGN KEY (ArrendatarioId) REFERENCES dbo.Arrendatario(ArrendatarioId),
	INDEX IDX_OrdenVisitaId (OrdenVisitaId),
	INDEX IDX_ArrendatarioId (ArrendatarioId)
);

-- Tabla: Contrato Arriendo
IF OBJECT_ID('dbo.ContratoArriendo', 'U') IS NOT NULL 
	DROP TABLE dbo.ContratoArriendo;

CREATE TABLE dbo.ContratoArriendo (
	ContratoArriendoId INT PRIMARY KEY IDENTITY(1,1),
	PropiedadId INT NOT NULL,
	PropietarioId INT NOT NULL,
	ArrendatarioId INT NOT NULL,
	CorredorId INT NOT NULL,
	FechaInicio DATETIME NOT NULL,
	FechaTermino DATETIME NOT NULL,
	MontoMensual DECIMAL(12,2) NOT NULL,
	DepositoGarantia DECIMAL(12,2),
	DiasPago INT DEFAULT 5, -- Día del mes de pago
	Estado NVARCHAR(50) NOT NULL DEFAULT 'Activo', -- Activo, Finalizado, Cancelado, Suspendido
	DocumentoUrl NVARCHAR(MAX),
	FechaFirma DATETIME,
	Comentarios NVARCHAR(MAX),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (PropiedadId) REFERENCES dbo.Propiedad(PropiedadId),
	FOREIGN KEY (PropietarioId) REFERENCES dbo.Propietario(PropietarioId),
	FOREIGN KEY (ArrendatarioId) REFERENCES dbo.Arrendatario(ArrendatarioId),
	FOREIGN KEY (CorredorId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_PropiedadId (PropiedadId),
	INDEX IDX_PropietarioId (PropietarioId),
	INDEX IDX_ArrendatarioId (ArrendatarioId),
	INDEX IDX_Estado (Estado)
);

-- ============================================================
-- 5. TABLA - MATCHING AUTOMÁTICO
-- ============================================================

-- Tabla: Matching Clientes-Propiedades
IF OBJECT_ID('dbo.MatchingClientePropiedad', 'U') IS NOT NULL 
	DROP TABLE dbo.MatchingClientePropiedad;

CREATE TABLE dbo.MatchingClientePropiedad (
	MatchingClientePropiedadId INT PRIMARY KEY IDENTITY(1,1),
	ArrendatarioId INT NOT NULL,
	PropiedadId INT NOT NULL,
	PorcentajeCoincidencia INT NOT NULL DEFAULT 0, -- 0-100
	RequisitosMaxNivelArriendo DECIMAL(12,2),
	RequistosHabitaciones INT,
	RequisitosZona NVARCHAR(255),
	RequistosCondominio BIT,
	RequistosGaraje BIT,
	EsNotificado BIT NOT NULL DEFAULT 0,
	FechaNotificacion DATETIME,
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FechaActualizacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (ArrendatarioId) REFERENCES dbo.Arrendatario(ArrendatarioId),
	FOREIGN KEY (PropiedadId) REFERENCES dbo.Propiedad(PropiedadId),
	UNIQUE(ArrendatarioId, PropiedadId),
	INDEX IDX_ArrendatarioId (ArrendatarioId),
	INDEX IDX_PropiedadId (PropiedadId),
	INDEX IDX_PorcentajeCoincidencia (PorcentajeCoincidencia)
);

-- ============================================================
-- 6. TABLA - AUDITORÍA Y LOGS
-- ============================================================

-- Tabla: Auditoría General
IF OBJECT_ID('dbo.LogAuditoria', 'U') IS NOT NULL 
	DROP TABLE dbo.LogAuditoria;

CREATE TABLE dbo.LogAuditoria (
	LogAuditoriaId BIGINT PRIMARY KEY IDENTITY(1,1),
	CorredorId INT,
	Accion NVARCHAR(100) NOT NULL,
	Tabla NVARCHAR(50),
	RegistroId INT,
	ValoresAnteriores NVARCHAR(MAX),
	ValoresNuevos NVARCHAR(MAX),
	DireccionIP NVARCHAR(45),
	FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (CorredorId) REFERENCES dbo.Corredor(CorredorId),
	INDEX IDX_FechaCreacion (FechaCreacion),
	INDEX IDX_CorredorId (CorredorId)
);

-- ============================================================
-- 7. INSERCIÓN DE DATOS DE REFERENCIA (CATÁLOGOS)
-- ============================================================

-- Estados de Propiedad
INSERT INTO dbo.EstadoPropiedad (Nombre, Descripcion) VALUES
('Pendiente', 'Propiedad registrada, en fase de revisión'),
('En Trámite', 'Propiedad en proceso de publicación o gestión'),
('Disponible', 'Propiedad disponible para arriendo o venta'),
('Arrendada', 'Propiedad actualmente arrendada'),
('Vendida', 'Propiedad vendida'),
('Mantenimiento', 'Propiedad en mantenimiento o reparación'),
('Retirada', 'Propiedad retirada del sistema');

-- Categorías de Propiedad
INSERT INTO dbo.CategoriaPropiedad (Nombre, Descripcion) VALUES
('Departamento', 'Unidad habitacional en edificio de apartamentos'),
('Casa', 'Vivienda independiente con terreno'),
('Oficina', 'Espacio comercial o administrativo'),
('Local Comercial', 'Espacio para negocio al público'),
('Bodega', 'Espacio de almacenamiento');

-- Tipos de Propiedad
INSERT INTO dbo.TipoPropiedad (Nombre, Descripcion) VALUES
('Residencial', 'Propiedad con fines residenciales'),
('Comercial', 'Propiedad con fines comerciales'),
('Industrial', 'Propiedad con destino industrial'),
('Mixto', 'Propiedad con uso residencial y comercial');

-- Estados Civiles
INSERT INTO dbo.EstadoCivil (Nombre) VALUES
('Soltero'),
('Casado'),
('Divorciado'),
('Viudo'),
('Convivencia');

-- Tipos de Documento
INSERT INTO dbo.TipoDocumento (Nombre, Descripcion) VALUES
('Orden de Visita', 'Documento para agendar visita a propiedad'),
('Contrato de Arriendo', 'Contrato de arriendo de propiedad'),
('Certificado de Arriendo', 'Certificado de pago de arriendo'),
('Contrato de Compraventa', 'Contrato de venta de propiedad'),
('Informe de Inspección', 'Informe técnico de inspección de propiedad');

-- Regiones de Chile (principales)
INSERT INTO dbo.Region (Nombre, Codigo) VALUES
('Región de Arica y Parinacota', 'I'),
('Región de Tarapacá', 'II'),
('Región de Antofagasta', 'III'),
('Región de Atacama', 'IV'),
('Región de Coquimbo', 'V'),
('Región de Valparaíso', 'V+'),
('Región del Libertador General Bernardo O''Higgins', 'VI'),
('Región del Maule', 'VII'),
('Región de Ñuble', 'XVI'),
('Región de Los Lagos', 'X'),
('Región de Los Ríos', 'XIV'),
('Región de La Araucanía', 'IX'),
('Metropolitana de Santiago', 'XIII'),
('Región de Magallanes', 'XII');

-- Comunas de Santiago (ejemplo - añadir más según necesidad)
INSERT INTO dbo.Comuna (RegionId, Nombre) VALUES
(13, 'Santiago'),
(13, 'La Florida'),
(13, 'Puente Alto'),
(13, 'San Bernardo'),
(13, 'La Pintana'),
(13, 'Peñalolén'),
(13, 'Las Condes'),
(13, 'Lo Barnechea'),
(13, 'Providencia'),
(13, 'Ñuñoa'),
(13, 'Macul'),
(13, 'Recoleta'),
(13, 'Conchalí'),
(13, 'Renca'),
(13, 'Pudahuel');

-- ============================================================
-- 8. ÍNDICES ADICIONALES PARA OPTIMIZACIÓN
-- ============================================================

-- Índices compuestos para búsquedas comunes
CREATE INDEX IDX_Cliente_RutCorreo ON dbo.Cliente(Rut, Correo) WHERE EsActivo = 1;
CREATE INDEX IDX_Propiedad_EstadoComunaTipo ON dbo.Propiedad(EstadoPropiedadId, ComunaId, TipoPropiedadId) WHERE PermiteArriendo = 1;
CREATE INDEX IDX_ContratoArriendo_Periodo ON dbo.ContratoArriendo(FechaInicio, FechaTermino, Estado);
CREATE INDEX IDX_MatchingCoincidencia ON dbo.MatchingClientePropiedad(PorcentajeCoincidencia DESC, EsNotificado);

-- ============================================================
-- 9. VISTAS ÚTILES PARA REPORTES
-- ============================================================

-- Vista: Propiedades Disponibles por Comuna
IF OBJECT_ID('dbo.vw_PropiedadesDisponibles', 'V') IS NOT NULL 
	DROP VIEW dbo.vw_PropiedadesDisponibles;
GO

CREATE VIEW dbo.vw_PropiedadesDisponibles AS
SELECT 
	p.PropiedadId,
	p.Direccion,
	c.Nombre AS Comuna,
	r.Nombre AS Region,
	tp.Nombre AS TipoPropiedad,
	cp.Nombre AS Categoria,
	p.Habitaciones,
	p.Banos,
	p.PrecioArriendo,
	ep.Nombre AS Estado,
	p.FechaCreacion
FROM dbo.Propiedad p
INNER JOIN dbo.Comuna c ON p.ComunaId = c.ComunaId
INNER JOIN dbo.Region r ON c.RegionId = r.RegionId
INNER JOIN dbo.TipoPropiedad tp ON p.TipoPropiedadId = tp.TipoPropiedadId
INNER JOIN dbo.CategoriaPropiedad cp ON p.CategoriaPropiedadId = cp.CategoriaPropiedadId
INNER JOIN dbo.EstadoPropiedad ep ON p.EstadoPropiedadId = ep.EstadoPropiedadId
WHERE p.PermiteArriendo = 1 AND ep.Nombre IN ('Disponible', 'En Trámite');
GO

-- Vista: Contratos Activos por Corredor
IF OBJECT_ID('dbo.vw_ContratosActivos', 'V') IS NOT NULL 
	DROP VIEW dbo.vw_ContratosActivos;
GO

CREATE VIEW dbo.vw_ContratosActivos AS
SELECT 
	ca.ContratoArriendoId,
	cor.Nombre + ' ' + cor.Apellido AS NombreCorredor,
	prop.Nombre + ' ' + prop.Apellido AS NombrePropietario,
	arr.Nombre + ' ' + arr.Apellido AS NombreArrendatario,
	p.Direccion,
	ca.MontoMensual,
	ca.FechaInicio,
	ca.FechaTermino,
	ca.Estado
FROM dbo.ContratoArriendo ca
INNER JOIN dbo.Corredor cor ON ca.CorredorId = cor.CorredorId
INNER JOIN dbo.Propietario prop ON ca.PropietarioId = prop.PropietarioId
INNER JOIN dbo.Cliente propcliente ON prop.ClienteId = propcliente.ClienteId
INNER JOIN dbo.Arrendatario arr ON ca.ArrendatarioId = arr.ArrendatarioId
INNER JOIN dbo.Cliente arrcliente ON arr.ClienteId = arrcliente.ClienteId
INNER JOIN dbo.Propiedad p ON ca.PropiedadId = p.PropiedadId
WHERE ca.Estado = 'Activo';
GO

-- ============================================================
-- 10. PROCEDIMIENTO PARA CREAR USUARIO ADMIN POR DEFECTO
-- ============================================================

-- Nota: Ejecutar después de que la aplicación esté lista con hash de password
-- Se recomienda hacerlo desde la aplicación para mantener consistencia en hash
-- INSERT INTO dbo.Corredor (Rut, Nombre, Apellido, Correo, PasswordHash, EsActivo, EsAdmin)
-- VALUES ('11111111-1', 'Admin', 'Sistema', 'admin@homeflow.cl', '[PASSWORD_HASH]', 1, 1);

-- ============================================================
-- FIN DEL SCRIPT
-- ============================================================

PRINT 'Base de datos creada exitosamente con todas las tablas, índices y datos de referencia.';
PRINT 'Próximo paso: Crear entidades en Domain y DbContext en Infrastructure.';
