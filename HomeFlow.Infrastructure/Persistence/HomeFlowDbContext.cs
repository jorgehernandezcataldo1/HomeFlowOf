using Microsoft.EntityFrameworkCore;
using HomeFlow.Domain.Entities.Agenda;
using HomeFlow.Domain.Entities.Checklist;
using HomeFlow.Domain.Entities.Clientes;
using HomeFlow.Domain.Entities.Contratos;
using HomeFlow.Domain.Entities.Documentos;
using HomeFlow.Domain.Entities.Notificaciones;
using HomeFlow.Domain.Entities.Propiedades;
using HomeFlow.Domain.Entities.Seguridad;

namespace HomeFlow.Infrastructure.Persistence;

/// <summary>
/// DbContext principal de HomeFlow
/// </summary>
public class HomeFlowDbContext : DbContext
{
    public HomeFlowDbContext(DbContextOptions<HomeFlowDbContext> options) : base(options)
    {
    }

    // =================== SEGURIDAD ===================
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    // =================== CLIENTES ===================
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<InformacionArrendatario> InformacionesArrendatarios { get; set; }
    public DbSet<RequerimientoBusqueda> Requerimientos { get; set; }

    // =================== PROPIEDADES ===================
    public DbSet<TipoPropiedadCatalogo> TipoPropiedadCatalogos { get; set; }
    public DbSet<CategoriaPropiedad> CategoriaPropiedades { get; set; }
    public DbSet<Propiedad> Propiedades { get; set; }

    // =================== AGENDA ===================
    public DbSet<Visita> Visitas { get; set; }

    // =================== CHECKLISTS ===================
    public DbSet<ChecklistPlantilla> ChecklistPlantillas { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<ChecklistRespuesta> ChecklistRespuestas { get; set; }
    public DbSet<ChecklistRespuestaDetalle> ChecklistRespuestaDetalles { get; set; }

    // =================== CONTRATOS ===================
    public DbSet<Contrato> Contratos { get; set; }

    // =================== DOCUMENTOS ===================
    public DbSet<PlantillaDocumento> PlantillaDocumentos { get; set; }
    public DbSet<DocumentoGenerado> DocumentosGenerados { get; set; }

    // =================== NOTIFICACIONES ===================
    public DbSet<Notificacion> Notificaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones de entidades
        ConfigureSeguridad(modelBuilder);
        ConfigureClientes(modelBuilder);
        ConfigurePropiedades(modelBuilder);
        ConfigureAgenda(modelBuilder);
        ConfigureChecklists(modelBuilder);
    }

    private void ConfigureSeguridad(ModelBuilder modelBuilder)
    {
        // Empresa
        modelBuilder.Entity<Empresa>(e =>
        {
            e.HasKey(x => x.IdEmpresa);
            e.Property(x => x.RazonSocial).IsRequired().HasMaxLength(200);
            e.Property(x => x.Rut).IsRequired().HasMaxLength(15);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(u =>
        {
            u.HasKey(x => x.IdUsuario);
            u.Property(x => x.Rut).IsRequired().HasMaxLength(15);
            u.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
            u.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
            u.Property(x => x.Correo).IsRequired().HasMaxLength(150);
            u.Property(x => x.PasswordHash).IsRequired();
            u.Property(x => x.PasswordSalt).IsRequired();
            u.HasIndex(x => x.Correo).IsUnique();
        });
    }

    private void ConfigureClientes(ModelBuilder modelBuilder)
    {
        // Cliente
        modelBuilder.Entity<Cliente>(c =>
        {
            c.HasKey(x => x.IdCliente);
            c.Property(x => x.Rut).IsRequired().HasMaxLength(15);
            c.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
            c.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
            c.Property(x => x.Correo).IsRequired().HasMaxLength(150);
            c.HasIndex(x => x.Rut).IsUnique();
            c.HasIndex(x => x.Correo).IsUnique();
        });

        // InformacionArrendatario
        modelBuilder.Entity<InformacionArrendatario>(i =>
        {
            i.HasKey(x => x.IdInformacionArrendatario);
        });

        // RequerimientoBusqueda
        modelBuilder.Entity<RequerimientoBusqueda>(r =>
        {
            r.HasKey(x => x.IdRequerimientoBusqueda);
        });
    }

    private void ConfigurePropiedades(ModelBuilder modelBuilder)
    {
        // TipoPropiedadCatalogo
        modelBuilder.Entity<TipoPropiedadCatalogo>(t =>
        {
            t.HasKey(x => x.IdTipoPropiedadCatalogo);
            t.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            t.HasData(
                new TipoPropiedadCatalogo { IdTipoPropiedadCatalogo = 1, Nombre = "Casa" },
                new TipoPropiedadCatalogo { IdTipoPropiedadCatalogo = 2, Nombre = "Departamento" },
                new TipoPropiedadCatalogo { IdTipoPropiedadCatalogo = 3, Nombre = "Edificio" }
            );
        });

        // CategoriaPropiedad
        modelBuilder.Entity<CategoriaPropiedad>(c =>
        {
            c.HasKey(x => x.IdCategoriaPropiedad);
            c.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
        });

        // Propiedad
        modelBuilder.Entity<Propiedad>(p =>
        {
            p.HasKey(x => x.IdPropiedad);
            p.Property(x => x.Direccion).IsRequired().HasMaxLength(300);
            p.Property(x => x.Comuna).HasMaxLength(100);
            p.Property(x => x.Region).HasMaxLength(100);
            p.Property(x => x.Torre).HasMaxLength(50);
            p.Property(x => x.NumeroDepartamento).HasMaxLength(50);
            p.Property(x => x.NombreMetroCercano).HasMaxLength(200);
            p.Property(x => x.NombreColegioCercano).HasMaxLength(200);
            p.Property(x => x.DistanciaMetroMetros).HasPrecision(10, 2);
            p.Property(x => x.DistanciaColegioMetros).HasPrecision(10, 2);
            p.Property(x => x.MetrosCuadrados).HasPrecision(10, 2);
            p.Property(x => x.GastosComunes).HasPrecision(18, 2);
            p.Property(x => x.GastosBasicosEstimados).HasPrecision(18, 2);
            p.Property(x => x.PrecioArriendo).HasPrecision(18, 2);
            p.Property(x => x.PrecioVenta).HasPrecision(18, 2);
            p.Property(x => x.Descripcion).HasMaxLength(1000);
            p.HasMany(x => x.Visitas).WithOne(v => v.Propiedad);
        });
    }

    private void ConfigureAgenda(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Visita>(v =>
        {
            v.HasKey(x => x.IdVisita);
        });
    }

    private void ConfigureChecklists(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChecklistPlantilla>(cp =>
        {
            cp.HasKey(x => x.IdChecklistPlantilla);
            cp.Property(x => x.Nombre).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<ChecklistItem>(ci =>
        {
            ci.HasKey(x => x.IdChecklistItem);
            ci.Property(x => x.Descripcion).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<ChecklistRespuesta>(cr =>
        {
            cr.HasKey(x => x.IdChecklistRespuesta);
        });

        modelBuilder.Entity<ChecklistRespuestaDetalle>(crd =>
        {
            crd.HasKey(x => x.IdChecklistRespuestaDetalle);
        });
    }
}
