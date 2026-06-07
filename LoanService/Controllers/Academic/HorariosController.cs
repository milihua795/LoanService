using LoanService.Domain.Academic;
using LoanService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers.Academic;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HorariosController : ControllerBase
{
    private readonly AcademicDbContext _db;
    public HorariosController(AcademicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Horarios.Include(h => h.Curso).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var h = await _db.Horarios.Include(h => h.Curso).FirstOrDefaultAsync(h => h.HorarioId == id);
        return h is null ? NotFound() : Ok(h);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Horario horario)
    {
        _db.Horarios.Add(horario);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = horario.HorarioId }, horario);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var h = await _db.Horarios.FindAsync(id);
        if (h is null) return NotFound();
        _db.Horarios.Remove(h);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
