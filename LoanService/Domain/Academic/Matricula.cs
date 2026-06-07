namespace LoanService.Domain.Academic;

public class Matricula
{
    public int      MatriculaId  { get; set; }
    public int      EstudianteId { get; set; }
    public int      CursoId      { get; set; }
    public DateTime FechaMatricula { get; set; } = DateTime.UtcNow;
    public string   Estado       { get; set; } = "Activa"; // Activa | Retirada

    public Estudiante? Estudiante { get; set; }
    public Curso?      Curso      { get; set; }
}
