using LoanService.Domain.Academic;
using LoanService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers.Academic;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MatriculasController : ControllerBase
{
    private readonly AcademicDbContext _db;
    public MatriculasController(AcademicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Matriculas.Include(m => m.Estudiante).Include(m => m.Curso).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var m = await _db.Matriculas
            .Include(m => m.Estudiante).Include(m => m.Curso)
            .FirstOrDefaultAsync(m => m.MatriculaId == id);
        return m is null ? NotFound() : Ok(m);
    }

    [HttpGet("estudiante/{estudianteId:int}")]
    public async Task<IActionResult> GetByEstudiante(int estudianteId)
        => Ok(await _db.Matriculas
            .Include(m => m.Curso)
            .Where(m => m.EstudianteId == estudianteId)
            .ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Matricula matricula)
    {
        _db.Matriculas.Add(matricula);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = matricula.MatriculaId }, matricula);
    }

    [HttpPut("{id:int}/retirar")]
    public async Task<IActionResult> Retirar(int id)
    {
        var m = await _db.Matriculas.FindAsync(id);
        if (m is null) return NotFound();
        m.Estado = "Retirada";
        await _db.SaveChangesAsync();
        return Ok(m);
    }
}
