namespace LoanService.Domain.Academic;

public class Horario
{
    public int    HorarioId { get; set; }
    public int    CursoId   { get; set; }
    public string DiaSemana { get; set; } = string.Empty;   // Lunes, Martes, …
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin    { get; set; }
    public string Aula        { get; set; } = string.Empty;

    public Curso? Curso { get; set; }
}
