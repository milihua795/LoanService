using LoanService.Domain.Academic;
using LoanService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers.Academic;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EstudiantesController : ControllerBase
{
    private readonly AcademicDbContext _db;
    public EstudiantesController(AcademicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Estudiantes.ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var e = await _db.Estudiantes.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Estudiante estudiante)
    {
        _db.Estudiantes.Add(estudiante);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = estudiante.EstudianteId }, estudiante);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Estudiante estudiante)
    {
        if (id != estudiante.EstudianteId) return BadRequest();
        _db.Entry(estudiante).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Estudiantes.FindAsync(id);
        if (e is null) return NotFound();
        _db.Estudiantes.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
