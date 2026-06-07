using LoanService.Domain.Academic;
using LoanService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers.Academic;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CursosController : ControllerBase
{
    private readonly AcademicDbContext _db;
    public CursosController(AcademicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Cursos.Include(c => c.Docente).Include(c => c.Horarios).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await _db.Cursos
            .Include(c => c.Docente)
            .Include(c => c.Horarios)
            .FirstOrDefaultAsync(c => c.CursoId == id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Curso curso)
    {
        _db.Cursos.Add(curso);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = curso.CursoId }, curso);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Curso curso)
    {
        if (id != curso.CursoId) return BadRequest();
        _db.Entry(curso).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Cursos.FindAsync(id);
        if (c is null) return NotFound();
        _db.Cursos.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
