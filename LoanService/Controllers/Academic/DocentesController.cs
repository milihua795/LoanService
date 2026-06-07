using LoanService.Domain.Academic;
using LoanService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers.Academic;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DocentesController : ControllerBase
{
    private readonly AcademicDbContext _db;
    public DocentesController(AcademicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Docentes.Include(d => d.Cursos).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await _db.Docentes.Include(d => d.Cursos).FirstOrDefaultAsync(d => d.DocenteId == id);
        return d is null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Docente docente)
    {
        _db.Docentes.Add(docente);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = docente.DocenteId }, docente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Docente docente)
    {
        if (id != docente.DocenteId) return BadRequest();
        _db.Entry(docente).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var d = await _db.Docentes.FindAsync(id);
        if (d is null) return NotFound();
        _db.Docentes.Remove(d);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
