using LoanService.Domain.Academic;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure.Persistence;

public class AcademicDbContext : DbContext
{
    public AcademicDbContext(DbContextOptions<AcademicDbContext> options)
        : base(options) { }

    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Docente>    Docentes    { get; set; }
    public DbSet<Curso>      Cursos      { get; set; }
    public DbSet<Horario>    Horarios    { get; set; }
    public DbSet<Matricula>  Matriculas  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación Matricula (Estudiante – Curso)
        modelBuilder.Entity<Matricula>()
            .HasOne(m => m.Estudiante)
            .WithMany(e => e.Matriculas)
            .HasForeignKey(m => m.EstudianteId);

        modelBuilder.Entity<Matricula>()
            .HasOne(m => m.Curso)
            .WithMany(c => c.Matriculas)
            .HasForeignKey(m => m.CursoId);

        // Relación Curso – Docente
        modelBuilder.Entity<Curso>()
            .HasOne(c => c.Docente)
            .WithMany(d => d.Cursos)
            .HasForeignKey(c => c.DocenteId);

        // Relación Horario – Curso
        modelBuilder.Entity<Horario>()
            .HasOne(h => h.Curso)
            .WithMany(c => c.Horarios)
            .HasForeignKey(h => h.CursoId);
    }
}
