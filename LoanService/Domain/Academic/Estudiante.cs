namespace LoanService.Domain.Academic;

public class Estudiante
{
    public int    EstudianteId     { get; set; }
    public string Nombre           { get; set; } = string.Empty;
    public string Apellido         { get; set; } = string.Empty;
    public string Codigo           { get; set; } = string.Empty;
    public string CorreoElectronico{ get; set; } = string.Empty;
    public string Carrera          { get; set; } = string.Empty;
    public int    Ciclo            { get; set; }
    public DateTime FechaRegistro  { get; set; } = DateTime.UtcNow;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
