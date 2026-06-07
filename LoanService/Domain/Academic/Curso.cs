namespace LoanService.Domain.Academic;

public class Curso
{
    public int    CursoId     { get; set; }
    public string Nombre      { get; set; } = string.Empty;
    public string Codigo      { get; set; } = string.Empty;
    public int    Creditos    { get; set; }
    public int    DocenteId   { get; set; }

    public Docente?            Docente    { get; set; }
    public ICollection<Horario>  Horarios   { get; set; } = new List<Horario>();
    public ICollection<Matricula>Matriculas { get; set; } = new List<Matricula>();
}
