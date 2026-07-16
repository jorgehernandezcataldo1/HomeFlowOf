using Microsoft.EntityFrameworkCore;
using HomeFlow.Domain.Entities.References;
using HomeFlow.Domain.Entities.Users;
using HomeFlow.Domain.Entities.Clients;
using HomeFlow.Domain.Entities.Properties;
using HomeFlow.Domain.Entities.Processes;
using HomeFlow.Domain.Entities.Audit;

namespace HomeFlow.Infrastructure.Data
{
    /// <summary>
    /// DbContext principal para HomeFlow - Configuración de todas las entidades y relaciones
    /// </summary>
    public class HomeFlowDbContext : DbContext
    {
        public HomeFlowDbContext(DbContextOptions<HomeFlowDbContext> options) : base(options)
        {
        }

        // DbSets - Catálogos de Referencia
        public DbSet<EstadoPropiedad> EstadoPropiedad { get; set; }
        public DbSet<CategoriaPropiedad> CategoriaPropiedad { get; set; }
        public DbSet<TipoPropiedad> TipoPropiedad { get; set; }
        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Comuna> Comunas { get; set; }

        // DbSets - Usuarios
        public DbSet<Corredor> Corredores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        // DbSets - Clientes
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Arrendatario> Arrendatarios { get; set; }

        // DbSets - Propiedades
        public DbSet<Propiedad> Propiedades { get; set; }

        // DbSets - Procesos
        public DbSet<ChecklistPropietario> ChecklistsPropietarios { get; set; }
        public DbSet<ChecklistPropiedad> ChecklistsPropiedades { get; set; }
        public DbSet<OrdenVisita> OrdenesVisita { get; set; }
        public DbSet<ChecklistArrendatario> ChecklistsArrendatarios { get; set; }
        public DbSet<ContratoArriendo> ContratosArriendo { get; set; }
        public DbSet<MatchingClientePropiedad> Matchings { get; set; }

        // DbSets - Auditoría
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== CONFIGURACIÓN ENTIDADES DE REFERENCIA ==========

            // EstadoPropiedad
            modelBuilder.Entity<EstadoPropiedad>(entity =>
            {
                entity.ToTable("EstadoPropiedad");
                entity.HasKey(e => e.EstadoPropiedadId);
                entity.Property(e => e.EstadoPropiedadId).HasColumnName("EstadoPropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(255);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // CategoriaPropiedad
            modelBuilder.Entity<CategoriaPropiedad>(entity =>
            {
                entity.ToTable("CategoriaPropiedad");
                entity.HasKey(e => e.CategoriaPropiedadId);
                entity.Property(e => e.CategoriaPropiedadId).HasColumnName("CategoriaPropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(255);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // TipoPropiedad
            modelBuilder.Entity<TipoPropiedad>(entity =>
            {
                entity.ToTable("TipoPropiedad");
                entity.HasKey(e => e.TipoPropiedadId);
                entity.Property(e => e.TipoPropiedadId).HasColumnName("TipoPropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(255);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // EstadoCivil
            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.ToTable("EstadoCivil");
                entity.HasKey(e => e.EstadoCivilId);
                entity.Property(e => e.EstadoCivilId).HasColumnName("EstadoCivilId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(30);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // TipoDocumento
            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");
                entity.HasKey(e => e.TipoDocumentoId);
                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumentoId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Descripcion).HasMaxLength(255);
                entity.Property(e => e.PlantillaHtml).HasColumnType("nvarchar(max)");
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // Region
            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");
                entity.HasKey(e => e.RegionId);
                entity.Property(e => e.RegionId).HasColumnName("RegionId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Codigo).HasMaxLength(10);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasMany(e => e.Comunas).WithOne(c => c.Region).HasForeignKey(c => c.RegionId).OnDelete(DeleteBehavior.Cascade);
            });

            // Comuna
            modelBuilder.Entity<Comuna>(entity =>
            {
                entity.ToTable("Comuna");
                entity.HasKey(e => e.ComunaId);
                entity.Property(e => e.ComunaId).HasColumnName("ComunaId").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Region).WithMany(r => r.Comunas).HasForeignKey(e => e.RegionId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.RegionId, e.Nombre }).IsUnique();
            });

            // ========== CONFIGURACIÓN ENTIDADES DE USUARIOS ==========

            // Corredor
            modelBuilder.Entity<Corredor>(entity =>
            {
                entity.ToTable("Corredor");
                entity.HasKey(e => e.CorredorId);
                entity.Property(e => e.CorredorId).HasColumnName("CorredorId").ValueGeneratedOnAdd();
                entity.Property(e => e.Rut).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Telefono).HasMaxLength(15);
                entity.Property(e => e.PasswordHash).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.PasswordSalt).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Licencia).HasMaxLength(50);
                entity.Property(e => e.FotoPerfilUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.Property(e => e.EsAdmin).HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Rut).IsUnique();
                entity.HasIndex(e => e.Correo).IsUnique();
            });

            // Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
                entity.HasKey(e => e.ClienteId);
                entity.Property(e => e.ClienteId).HasColumnName("ClienteId").ValueGeneratedOnAdd();
                entity.Property(e => e.Rut).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Correo).HasMaxLength(255);
                entity.Property(e => e.Telefono).HasMaxLength(15);
                entity.Property(e => e.Direccion).HasMaxLength(255);
                entity.Property(e => e.FotoCarnetUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Notas).HasColumnType("nvarchar(max)");
                entity.Property(e => e.EsActivo).HasDefaultValue(true);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Rut).IsUnique();
                entity.HasIndex(e => e.Correo);
                entity.HasIndex(e => e.ComunaId);
                entity.HasOne(e => e.EstadoCivil).WithMany(ec => ec.Clientes).HasForeignKey(e => e.EstadoCivilId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Comuna).WithMany(c => c.Clientes).HasForeignKey(e => e.ComunaId).OnDelete(DeleteBehavior.SetNull);
            });

            // ========== CONFIGURACIÓN ENTIDADES DE CLIENTES ==========

            // Propietario
            modelBuilder.Entity<Propietario>(entity =>
            {
                entity.ToTable("Propietario");
                entity.HasKey(e => e.PropietarioId);
                entity.Property(e => e.PropietarioId).HasColumnName("PropietarioId").ValueGeneratedOnAdd();
                entity.Property(e => e.BancoPreferido).HasMaxLength(100);
                entity.Property(e => e.CuentaBancaria).HasMaxLength(20);
                entity.Property(e => e.TipoCuenta).HasMaxLength(20);
                entity.Property(e => e.DocumentoIdentidadVerificado).HasDefaultValue(false);
                entity.Property(e => e.RequisitosCompletos).HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Cliente).WithOne(c => c.Propietario).HasForeignKey<Propietario>(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CorredorAsignado).WithMany(cor => cor.PropietariosAsignados).HasForeignKey(e => e.CorredorAsignadoId).OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(e => e.ClienteId).IsUnique();
                entity.HasIndex(e => e.CorredorAsignadoId);
            });

            // Arrendatario
            modelBuilder.Entity<Arrendatario>(entity =>
            {
                entity.ToTable("Arrendatario");
                entity.HasKey(e => e.ArrendatarioId);
                entity.Property(e => e.ArrendatarioId).HasColumnName("ArrendatarioId").ValueGeneratedOnAdd();
                entity.Property(e => e.LiquidoMensual).HasPrecision(12, 2);
                entity.Property(e => e.Empleador).HasMaxLength(255);
                entity.Property(e => e.AntiguedadLaboral);
                entity.Property(e => e.TieneHijos).HasDefaultValue(false);
                entity.Property(e => e.NumeroHijos).HasDefaultValue(0);
                entity.Property(e => e.TieneMascota).HasDefaultValue(false);
                entity.Property(e => e.TipoMascota).HasMaxLength(100);
                entity.Property(e => e.PreAprobacionCredito).HasDefaultValue(false);
                entity.Property(e => e.DocumentacionCompleta).HasDefaultValue(false);
                entity.Property(e => e.RequisitosCompletos).HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Cliente).WithOne(c => c.Arrendatario).HasForeignKey<Arrendatario>(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CorredorAsignado).WithMany(cor => cor.ArrendatariosAsignados).HasForeignKey(e => e.CorredorAsignadoId).OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(e => e.ClienteId).IsUnique();
                entity.HasIndex(e => e.CorredorAsignadoId);
            });

            // ========== CONFIGURACIÓN ENTIDADES DE PROPIEDADES ==========

            // Propiedad
            modelBuilder.Entity<Propiedad>(entity =>
            {
                entity.ToTable("Propiedad");
                entity.HasKey(e => e.PropiedadId);
                entity.Property(e => e.PropiedadId).HasColumnName("PropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.Direccion).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Piso);
                entity.Property(e => e.Torre).HasMaxLength(10);
                entity.Property(e => e.CodigoPostal).HasMaxLength(10);
                entity.Property(e => e.Latitud).HasPrecision(10, 8);
                entity.Property(e => e.Longitud).HasPrecision(11, 8);
                entity.Property(e => e.DescripcionGeneral).HasColumnType("nvarchar(max)");
                entity.Property(e => e.PrecioArriendo).HasPrecision(12, 2);
                entity.Property(e => e.PrecioVenta).HasPrecision(15, 2);
                entity.Property(e => e.MetrosCuadrados).HasPrecision(8, 2);
                entity.Property(e => e.GastosComunes).HasPrecision(10, 2);
                entity.Property(e => e.GastoAgua).HasPrecision(10, 2);
                entity.Property(e => e.GastoLuz).HasPrecision(10, 2);
                entity.Property(e => e.GastoGas).HasPrecision(10, 2);
                entity.Property(e => e.MontoContribuciones).HasPrecision(10, 2);
                entity.Property(e => e.DistanciaMetro);
                entity.Property(e => e.DistanciaColegio);
                entity.Property(e => e.DistanciaSupermercado);
                entity.Property(e => e.InfoMetro).HasMaxLength(255);
                entity.Property(e => e.InfoColegio).HasMaxLength(255);
                entity.Property(e => e.InfoSupermercado).HasMaxLength(255);
                entity.Property(e => e.EquipamientoKitchen).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Ventanas).HasMaxLength(255);
                entity.Property(e => e.FotoUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FotosCasaUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.DocumentosUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Notas).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.PermiteArriendo).HasDefaultValue(true);
                entity.Property(e => e.PermiteVenta).HasDefaultValue(true);
                entity.HasIndex(e => e.PropietarioId);
                entity.HasIndex(e => e.EstadoPropiedadId);
                entity.HasIndex(e => e.ComunaId);
                entity.HasIndex(e => e.CorredorAsignadoId);
                entity.HasIndex(e => e.TipoPropiedadId);
                entity.HasIndex(e => new { e.EstadoPropiedadId, e.ComunaId, e.TipoPropiedadId });

                entity.HasOne(e => e.Propietario).WithMany(p => p.Propiedades).HasForeignKey(e => e.PropietarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.TipoPropiedad).WithMany(tp => tp.Propiedades).HasForeignKey(e => e.TipoPropiedadId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.CategoriaPropiedad).WithMany(cp => cp.Propiedades).HasForeignKey(e => e.CategoriaPropiedadId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.EstadoPropiedad).WithMany(ep => ep.Propiedades).HasForeignKey(e => e.EstadoPropiedadId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Comuna).WithMany(c => c.Propiedades).HasForeignKey(e => e.ComunaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.CorredorAsignado).WithMany(cor => cor.PropiedadesAsignadas).HasForeignKey(e => e.CorredorAsignadoId).OnDelete(DeleteBehavior.Restrict);
            });

            // ========== CONFIGURACIÓN ENTIDADES DE PROCESOS ==========

            // ChecklistPropietario
            modelBuilder.Entity<ChecklistPropietario>(entity =>
            {
                entity.ToTable("ChecklistPropietario");
                entity.HasKey(e => e.ChecklistPropietarioId);
                entity.Property(e => e.ChecklistPropietarioId).HasColumnName("ChecklistPropietarioId").ValueGeneratedOnAdd();
                entity.Property(e => e.DocumentoIdentidadVerificado).HasDefaultValue(false);
                entity.Property(e => e.ComprobanteUbicacionVerificado).HasDefaultValue(false);
                entity.Property(e => e.CuentaBancariaVerificada).HasDefaultValue(false);
                entity.Property(e => e.AntecedentesLimpios).HasDefaultValue(false);
                entity.Property(e => e.FotoCarnetSubida).HasDefaultValue(false);
                entity.Property(e => e.EsCompleto).HasDefaultValue(false);
                entity.Property(e => e.Comentarios).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Propietario).WithOne(p => p.ChecklistPropietario).HasForeignKey<ChecklistPropietario>(e => e.PropietarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.PropietarioId);
            });

            // ChecklistPropiedad
            modelBuilder.Entity<ChecklistPropiedad>(entity =>
            {
                entity.ToTable("ChecklistPropiedad");
                entity.HasKey(e => e.ChecklistPropiedadId);
                entity.Property(e => e.ChecklistPropiedadId).HasColumnName("ChecklistPropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.FotosCompletas).HasDefaultValue(false);
                entity.Property(e => e.DocumentacionCompleta).HasDefaultValue(false);
                entity.Property(e => e.ServiciosVerificados).HasDefaultValue(false);
                entity.Property(e => e.EspaciosVerificados).HasDefaultValue(false);
                entity.Property(e => e.CondicionesEstructurales).HasDefaultValue(false);
                entity.Property(e => e.DisponiblePublicar).HasDefaultValue(false);
                entity.Property(e => e.Comentarios).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Propiedad).WithOne(p => p.ChecklistPropiedad).HasForeignKey<ChecklistPropiedad>(e => e.PropiedadId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.PropiedadId);
            });

            // OrdenVisita
            modelBuilder.Entity<OrdenVisita>(entity =>
            {
                entity.ToTable("OrdenVisita");
                entity.HasKey(e => e.OrdenVisitaId);
                entity.Property(e => e.OrdenVisitaId).HasColumnName("OrdenVisitaId").ValueGeneratedOnAdd();
                entity.Property(e => e.Direccion).HasMaxLength(255);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50).HasDefaultValue("Pendiente");
                entity.Property(e => e.DocumentoUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Observaciones).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Notificado).HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Propiedad).WithMany(p => p.OrdenesVisita).HasForeignKey(e => e.PropiedadId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Arrendatario).WithMany(a => a.OrdenesVisita).HasForeignKey(e => e.ArrendatarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Corredor).WithMany(c => c.OrdenesVisita).HasForeignKey(e => e.CorredorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.PropiedadId);
                entity.HasIndex(e => e.ArrendatarioId);
                entity.HasIndex(e => e.FechaVisita);
            });

            // ChecklistArrendatario
            modelBuilder.Entity<ChecklistArrendatario>(entity =>
            {
                entity.ToTable("ChecklistArrendatario");
                entity.HasKey(e => e.ChecklistArrendatarioId);
                entity.Property(e => e.ChecklistArrendatarioId).HasColumnName("ChecklistArrendatarioId").ValueGeneratedOnAdd();
                entity.Property(e => e.ConformidadEspacios).HasDefaultValue(false);
                entity.Property(e => e.ConformidadServicios).HasDefaultValue(false);
                entity.Property(e => e.PreguntasRespondidas).HasDefaultValue(false);
                entity.Property(e => e.InteresContratar).HasDefaultValue(false);
                entity.Property(e => e.Comentarios).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.OrdenVisita).WithOne(ov => ov.ChecklistArrendatario).HasForeignKey<ChecklistArrendatario>(e => e.OrdenVisitaId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Arrendatario).WithMany(a => a.ChecklistsArrendatario).HasForeignKey(e => e.ArrendatarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.OrdenVisitaId);
                entity.HasIndex(e => e.ArrendatarioId);
            });

            // ContratoArriendo
            modelBuilder.Entity<ContratoArriendo>(entity =>
            {
                entity.ToTable("ContratoArriendo");
                entity.HasKey(e => e.ContratoArriendoId);
                entity.Property(e => e.ContratoArriendoId).HasColumnName("ContratoArriendoId").ValueGeneratedOnAdd();
                entity.Property(e => e.MontoMensual).IsRequired().HasPrecision(12, 2);
                entity.Property(e => e.DepositoGarantia).HasPrecision(12, 2);
                entity.Property(e => e.DiasPago).HasDefaultValue(5);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50).HasDefaultValue("Activo");
                entity.Property(e => e.DocumentoUrl).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Comentarios).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Propiedad).WithMany(p => p.Contratos).HasForeignKey(e => e.PropiedadId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Propietario).WithMany(p => p.Contratos).HasForeignKey(e => e.PropietarioId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Arrendatario).WithMany(a => a.Contratos).HasForeignKey(e => e.ArrendatarioId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Corredor).WithMany(c => c.Contratos).HasForeignKey(e => e.CorredorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.PropiedadId);
                entity.HasIndex(e => e.PropietarioId);
                entity.HasIndex(e => e.ArrendatarioId);
                entity.HasIndex(e => new { e.FechaInicio, e.FechaTermino, e.Estado });
            });

            // MatchingClientePropiedad
            modelBuilder.Entity<MatchingClientePropiedad>(entity =>
            {
                entity.ToTable("MatchingClientePropiedad");
                entity.HasKey(e => e.MatchingClientePropiedadId);
                entity.Property(e => e.MatchingClientePropiedadId).HasColumnName("MatchingClientePropiedadId").ValueGeneratedOnAdd();
                entity.Property(e => e.PorcentajeCoincidencia).HasDefaultValue(0);
                entity.Property(e => e.RequisitosMaxNivelArriendo).HasPrecision(12, 2);
                entity.Property(e => e.RequisitosZona).HasMaxLength(255);
                entity.Property(e => e.EsNotificado).HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Arrendatario).WithMany(a => a.Matchings).HasForeignKey(e => e.ArrendatarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Propiedad).WithMany(p => p.Matchings).HasForeignKey(e => e.PropiedadId).HasConstraintName("FK_Matching_Propiedad").OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.ArrendatarioId, e.PropiedadId }).IsUnique();
                entity.HasIndex(e => e.ArrendatarioId);
                entity.HasIndex(e => e.PropiedadId);
                entity.HasIndex(e => e.PorcentajeCoincidencia);
            });

            // ========== CONFIGURACIÓN ENTIDADES DE AUDITORÍA ==========

            // LogAuditoria
            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.ToTable("LogAuditoria");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("LogAuditoriaId").ValueGeneratedOnAdd();
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Tabla).HasMaxLength(50);
                entity.Property(e => e.ValoresAnteriores).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ValoresNuevos).HasColumnType("nvarchar(max)");
                entity.Property(e => e.DireccionIP).HasMaxLength(45);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.Corredor).WithMany(c => c.LogsAuditoria).HasForeignKey(e => e.CorredorId).OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(e => e.FechaCreacion);
                entity.HasIndex(e => e.CorredorId);
            });

            // Seed initial data
            SeedDataBase(modelBuilder);
        }

        /// <summary>
        /// Carga datos iniciales en la base de datos
        /// </summary>
        private void SeedDataBase(ModelBuilder modelBuilder)
        {
            // Estados de Propiedad
            modelBuilder.Entity<EstadoPropiedad>().HasData(
                new EstadoPropiedad { EstadoPropiedadId = 1, Nombre = "Pendiente", Descripcion = "Propiedad registrada, en fase de revisión", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new EstadoPropiedad { EstadoPropiedadId = 2, Nombre = "En Trámite", Descripcion = "Propiedad en proceso de publicación o gestión", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new EstadoPropiedad { EstadoPropiedadId = 3, Nombre = "Disponible", Descripcion = "Propiedad disponible para arriendo o venta", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new EstadoPropiedad { EstadoPropiedadId = 4, Nombre = "Arrendada", Descripcion = "Propiedad actualmente arrendada", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new EstadoPropiedad { EstadoPropiedadId = 5, Nombre = "Vendida", Descripcion = "Propiedad vendida", EsActivo = true, FechaCreacion = DateTime.UtcNow }
            );

            // Categorías de Propiedad
            modelBuilder.Entity<CategoriaPropiedad>().HasData(
                new CategoriaPropiedad { CategoriaPropiedadId = 1, Nombre = "Departamento", Descripcion = "Unidad habitacional en edificio de apartamentos", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new CategoriaPropiedad { CategoriaPropiedadId = 2, Nombre = "Casa", Descripcion = "Vivienda independiente con terreno", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new CategoriaPropiedad { CategoriaPropiedadId = 3, Nombre = "Oficina", Descripcion = "Espacio comercial o administrativo", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new CategoriaPropiedad { CategoriaPropiedadId = 4, Nombre = "Local Comercial", Descripcion = "Espacio para negocio al público", EsActivo = true, FechaCreacion = DateTime.UtcNow }
            );

            // Tipos de Propiedad
            modelBuilder.Entity<TipoPropiedad>().HasData(
                new TipoPropiedad { TipoPropiedadId = 1, Nombre = "Residencial", Descripcion = "Propiedad con fines residenciales", EsActivo = true, FechaCreacion = DateTime.UtcNow },
                new TipoPropiedad { TipoPropiedadId = 2, Nombre = "Comercial", Descripcion = "Propiedad con fines comerciales", EsActivo = true, FechaCreacion = DateTime.UtcNow }
            );

            // Estados Civiles
            modelBuilder.Entity<EstadoCivil>().HasData(
                new EstadoCivil { EstadoCivilId = 1, Nombre = "Soltero", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new EstadoCivil { EstadoCivilId = 2, Nombre = "Casado", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new EstadoCivil { EstadoCivilId = 3, Nombre = "Divorciado", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new EstadoCivil { EstadoCivilId = 4, Nombre = "Viudo", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow }
            );

            // Región - Santiago (XIII)
            modelBuilder.Entity<Region>().HasData(
                new Region { RegionId = 1, Nombre = "Metropolitana de Santiago", Codigo = "XIII", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow }
            );

            // Comunas de Santiago
            modelBuilder.Entity<Comuna>().HasData(
                new Comuna { ComunaId = 1, RegionId = 1, Nombre = "Santiago", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new Comuna { ComunaId = 2, RegionId = 1, Nombre = "La Florida", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new Comuna { ComunaId = 3, RegionId = 1, Nombre = "Puente Alto", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new Comuna { ComunaId = 4, RegionId = 1, Nombre = "Las Condes", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new Comuna { ComunaId = 5, RegionId = 1, Nombre = "Providencia", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow },
                new Comuna { ComunaId = 6, RegionId = 1, Nombre = "Ñuñoa", EsActivo = true, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow }
            );
        }
    }
}
