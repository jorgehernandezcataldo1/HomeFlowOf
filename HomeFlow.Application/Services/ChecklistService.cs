using HomeFlow.Application.Interfaces;
using HomeFlow.Domain;
using HomeFlow.Domain.Entities.Checklist;
using HomeFlow.Domain.Enums;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Shared.DTOs.Checklists;

namespace HomeFlow.Application.Services;

public sealed class ChecklistService(IUnitOfWork unitOfWork) : IChecklistService
{
    public async Task<Result<ChecklistPlantillaDto>> CrearPlantillaAsync(int empresaId, CrearChecklistPlantillaRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items.Count == 0)
            return Result<ChecklistPlantillaDto>.Fail("La plantilla debe tener al menos un ítem.");

        var plantilla = new ChecklistPlantilla
        {
            EmpresaId = empresaId,
            Nombre = request.Nombre.Trim(),
            TipoChecklist = request.TipoChecklist,
            Activo = true,
            Items = request.Items.Select(i => new ChecklistItem
            {
                Descripcion = i.Descripcion.Trim(),
                Orden = i.Orden,
                Obligatorio = i.Obligatorio
            }).ToList()
        };

        await unitOfWork.Repository<ChecklistPlantilla>().AddAsync(plantilla, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ChecklistPlantillaDto>.Ok(MapPlantilla(plantilla), "Plantilla creada.");
    }

    public async Task<Result<ChecklistPlantillaDto>> ObtenerPlantillaAsync(int empresaId, TipoChecklist tipo, CancellationToken cancellationToken = default)
    {
        var plantilla = await unitOfWork.Repository<ChecklistPlantilla>()
            .FirstOrDefaultAsync(p => p.EmpresaId == empresaId && p.TipoChecklist == tipo && p.Activo, cancellationToken);

        if (plantilla is null)
            return Result<ChecklistPlantillaDto>.Fail("No hay una plantilla de checklist configurada para este tipo todavía.");

        var items = (await unitOfWork.Repository<ChecklistItem>()
            .FindAsync(i => i.ChecklistPlantillaId == plantilla.IdChecklistPlantilla, cancellationToken))
            .OrderBy(i => i.Orden).ToList();

        return Result<ChecklistPlantillaDto>.Ok(new ChecklistPlantillaDto
        {
            IdChecklistPlantilla = plantilla.IdChecklistPlantilla,
            Nombre = plantilla.Nombre,
            Items = items.Select(i => new ChecklistItemDto
            {
                IdChecklistItem = i.IdChecklistItem,
                Descripcion = i.Descripcion,
                Orden = i.Orden,
                Obligatorio = i.Obligatorio
            }).ToList()
        });
    }

    public async Task<Result<ChecklistRespuestaDto>> RegistrarRespuestaAsync(int empresaId, int usuarioEvaluadorId, CreateChecklistRespuestaRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ClienteId is null && request.PropiedadId is null)
            return Result<ChecklistRespuestaDto>.Fail("Debe indicar un cliente o una propiedad a evaluar.");

        var plantilla = await unitOfWork.Repository<ChecklistPlantilla>()
            .FirstOrDefaultAsync(p => p.IdChecklistPlantilla == request.ChecklistPlantillaId && p.EmpresaId == empresaId, cancellationToken);
        if (plantilla is null) return Result<ChecklistRespuestaDto>.Fail("Plantilla no encontrada.");

        var items = (await unitOfWork.Repository<ChecklistItem>()
            .FindAsync(i => i.ChecklistPlantillaId == plantilla.IdChecklistPlantilla, cancellationToken)).ToList();
        if (items.Count == 0) return Result<ChecklistRespuestaDto>.Fail("La plantilla no tiene ítems configurados.");

        var detalles = items.Select(item =>
        {
            var respuesta = request.Detalles.FirstOrDefault(d => d.ChecklistItemId == item.IdChecklistItem);
            return new ChecklistRespuestaDetalle
            {
                ChecklistItemId = item.IdChecklistItem,
                Cumple = respuesta?.Cumple ?? false,
                Comentario = respuesta?.Comentario
            };
        }).ToList();

        var obligatoriosOk = items.Where(i => i.Obligatorio)
            .All(i => detalles.First(d => d.ChecklistItemId == i.IdChecklistItem).Cumple);
        var porcentaje = (int)Math.Round(detalles.Count(d => d.Cumple) * 100.0 / items.Count);

        var respuestaChecklist = new ChecklistRespuesta
        {
            EmpresaId = empresaId,
            ChecklistPlantillaId = plantilla.IdChecklistPlantilla,
            ClienteId = request.ClienteId,
            PropiedadId = request.PropiedadId,
            UsuarioEvaluadorId = usuarioEvaluadorId,
            FechaEvaluacion = DateTime.Now,
            Aprobado = obligatoriosOk,
            Observaciones = request.Observaciones,
            Detalles = detalles
        };

        await unitOfWork.Repository<ChecklistRespuesta>().AddAsync(respuestaChecklist, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ChecklistRespuestaDto>.Ok(new ChecklistRespuestaDto
        {
            IdChecklistRespuesta = respuestaChecklist.IdChecklistRespuesta,
            ChecklistPlantillaId = plantilla.IdChecklistPlantilla,
            ChecklistNombre = plantilla.Nombre,
            ClienteId = respuestaChecklist.ClienteId,
            PropiedadId = respuestaChecklist.PropiedadId,
            FechaEvaluacion = respuestaChecklist.FechaEvaluacion,
            Aprobado = respuestaChecklist.Aprobado,
            PorcentajeAvance = porcentaje,
            Observaciones = respuestaChecklist.Observaciones
        }, obligatoriosOk ? "Checklist aprobado." : "Checklist registrado, quedan ítems obligatorios pendientes.");
    }

    private static ChecklistPlantillaDto MapPlantilla(ChecklistPlantilla p) => new()
    {
        IdChecklistPlantilla = p.IdChecklistPlantilla,
        Nombre = p.Nombre,
        Items = p.Items.Select(i => new ChecklistItemDto
        {
            IdChecklistItem = i.IdChecklistItem,
            Descripcion = i.Descripcion,
            Orden = i.Orden,
            Obligatorio = i.Obligatorio
        }).ToList()
    };
}