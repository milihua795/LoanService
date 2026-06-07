using System;
using FluentAssertions;
using Xunit;
using LoanService.Domain.Academic;

namespace LoanService.Tests.Domain
{
    public class AcademicModelsTests
    {
        [Fact]
        public void Estudiante_ShouldCreateValidInstance()
        {
            var estudiante = new Estudiante
            {
                Id = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Codigo = "20240001",
                CorreoElectronico = "juan.perez@universidad.edu",
                Carrera = "Ingeniería de Sistemas",
                Ciclo = 5,
                FechaRegistro = DateTime.Now
            };

            estudiante.Id.Should().Be(1);
            estudiante.Nombre.Should().Be("Juan");
            estudiante.Apellido.Should().Be("Perez");
            estudiante.Codigo.Should().Be("20240001");
            estudiante.CorreoElectronico.Should().Be("juan.perez@universidad.edu");
            estudiante.Carrera.Should().Be("Ingeniería de Sistemas");
            estudiante.Ciclo.Should().Be(5);
        }

        [Fact]
        public void Docente_ShouldCreateValidInstance()
        {
            var docente = new Docente
            {
                Id = 1,
                Nombre = "Maria",
                Apellido = "Gonzalez",
                Especialidad = "Matemáticas",
                Correo = "maria.gonzalez@universidad.edu"
            };

            docente.Nombre.Should().Be("Maria");
            docente.Apellido.Should().Be("Gonzalez");
            docente.Especialidad.Should().Be("Matemáticas");
            docente.Correo.Should().Be("maria.gonzalez@universidad.edu");
        }

        [Fact]
        public void Curso_ShouldCreateValidInstance()
        {
            var docente = new Docente { Id = 1, Nombre = "Maria", Apellido = "Gonzalez", Especialidad = "Matemáticas" };
            var curso = new Curso
            {
                Id = 1,
                Nombre = "Álgebra Lineal",
                Codigo = "MAT-101",
                Creditos = 4,
                DocenteId = 1,
                Docente = docente
            };

            curso.Nombre.Should().Be("Álgebra Lineal");
            curso.Codigo.Should().Be("MAT-101");
            curso.Creditos.Should().Be(4);
            curso.DocenteId.Should().Be(1);
            curso.Docente.Should().NotBeNull();
        }

        [Fact]
        public void Horario_ShouldCreateValidInstance()
        {
            var horario = new Horario
            {
                Id = 1,
                DiaSemana = "Lunes",
                HoraInicio = new TimeOnly(8, 0),
                HoraFin = new TimeOnly(10, 0),
                Aula = "A-101"
            };

            horario.DiaSemana.Should().Be("Lunes");
            horario.HoraInicio.Should().Be(new TimeOnly(8, 0));
            horario.HoraFin.Should().Be(new TimeOnly(10, 0));
            horario.Aula.Should().Be("A-101");
            // Comparación corregida:
            horario.HoraInicio.CompareTo(horario.HoraFin).Should().BeLessThan(0);
        }

        [Fact]
        public void Matricula_ShouldCreateValidInstance()
        {
            var estudiante = new Estudiante { Id = 1, Nombre = "Juan", Apellido = "Perez" };
            var curso = new Curso { Id = 1, Nombre = "Álgebra Lineal" };
            var matricula = new Matricula
            {
                Id = 1,
                EstudianteId = 1,
                CursoId = 1,
                Estudiante = estudiante,
                Curso = curso,
                FechaMatricula = DateTime.Now,
                Estado = "Activa"
            };

            matricula.EstudianteId.Should().Be(1);
            matricula.CursoId.Should().Be(1);
            matricula.Estado.Should().Be("Activa");
            matricula.Estudiante.Should().NotBeNull();
            matricula.Curso.Should().NotBeNull();
        }

        [Fact]
        public void Horario_ShouldValidateTimeRange()
        {
            var horario = new Horario
            {
                Id = 1,
                DiaSemana = "Martes",
                HoraInicio = new TimeOnly(14, 0),
                HoraFin = new TimeOnly(16, 0),
                Aula = "B-202"
            };

            // Comparación corregida
            (horario.HoraInicio < horario.HoraFin).Should().BeTrue();
        }

        [Fact]
        public void Matricula_ShouldChangeState()
        {
            var matricula = new Matricula
            {
                Id = 1,
                EstudianteId = 1,
                CursoId = 1,
                FechaMatricula = DateTime.Now,
                Estado = "Activa"
            };

            matricula.Estado = "Retirada";
            matricula.Estado.Should().Be("Retirada");
        }
    }
}
