using FluentAssertions;
using LoanService.Domain.Academic;
using Xunit;

namespace LoanService.Tests.Domain;

public class EstudianteTests
{
    [Fact]
    public void Estudiante_NuevoRegistro_DebeInicializarseCorrectamente()
    {
        // Arrange & Act
        var estudiante = new Estudiante
        {
            EstudianteId      = 1,
            Nombre            = "Juan",
            Apellido          = "Pérez",
            Codigo            = "UAC-2026-001",
            CorreoElectronico = "juan@uac.edu.pe",
            Carrera           = "Ingeniería de Sistemas",
            Ciclo             = 8
        };

        // Assert
        estudiante.Nombre.Should().Be("Juan");
        estudiante.Ciclo.Should().Be(8);
        estudiante.Matriculas.Should().BeEmpty();
        estudiante.FechaRegistro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Estudiante_ColeccionMatriculas_DebePermitirAgregar()
    {
        var estudiante = new Estudiante { EstudianteId = 1, Nombre = "Ana" };
        var matricula  = new Matricula  { MatriculaId = 1, EstudianteId = 1, CursoId = 1 };

        estudiante.Matriculas.Add(matricula);

        estudiante.Matriculas.Should().HaveCount(1);
    }
}

public class DocenteTests
{
    [Fact]
    public void Docente_NuevoDocente_CursosDebeIniciarVacio()
    {
        var docente = new Docente
        {
            DocenteId    = 1,
            Nombre       = "Luis",
            Apellido     = "Monzón",
            Especialidad = "Ingeniería de Software"
        };

        docente.Cursos.Should().BeEmpty();
        docente.Nombre.Should().Be("Luis");
    }
}

public class CursoTests
{
    [Fact]
    public void Curso_NuevoCurso_DebeInicializarColecciones()
    {
        var curso = new Curso
        {
            CursoId  = 1,
            Nombre   = "Plataformas para el Desarrollo de Aplicaciones",
            Codigo   = "IS-801",
            Creditos = 4
        };

        curso.Horarios.Should().BeEmpty();
        curso.Matriculas.Should().BeEmpty();
        curso.Creditos.Should().Be(4);
    }
}

public class MatriculaTests
{
    [Fact]
    public void Matricula_NuevaMatricula_EstadoDebeSerActiva()
    {
        var matricula = new Matricula
        {
            MatriculaId  = 1,
            EstudianteId = 1,
            CursoId      = 1
        };

        matricula.Estado.Should().Be("Activa");
        matricula.FechaMatricula.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Matricula_CambioEstado_DebePermitirRetirada()
    {
        var matricula = new Matricula { MatriculaId = 1, Estado = "Activa" };

        matricula.Estado = "Retirada";

        matricula.Estado.Should().Be("Retirada");
    }
}

public class HorarioTests
{
    [Fact]
    public void Horario_NuevoHorario_DebeAsignarDiaYAula()
    {
        var horario = new Horario
        {
            HorarioId  = 1,
            CursoId    = 1,
            DiaSemana  = "Lunes",
            HoraInicio = new TimeOnly(8, 0),
            HoraFin    = new TimeOnly(10, 0),
            Aula       = "A-301"
        };

        horario.DiaSemana.Should().Be("Lunes");
        horario.Aula.Should().Be("A-301");
        horario.HoraFin.Should().BeGreaterThan(horario.HoraInicio);
    }
}
