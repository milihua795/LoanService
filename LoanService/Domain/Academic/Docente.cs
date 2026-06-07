namespace LoanService.Domain.Academic;

public class Docente
{
    public int    DocenteId  { get; set; }
    public string Nombre     { get; set; } = string.Empty;
    public string Apellido   { get; set; } = string.Empty;
    public string Especialidad{ get; set; } = string.Empty;
    public string Correo     { get; set; } = string.Empty;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
