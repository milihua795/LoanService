using FluentAssertions;
using LoanService.Domain.Entities;
using LoanService.Domain.Enums;
using Xunit;

namespace LoanService.Tests.Domain;

public class LoanEntityTests
{
    // ── Tests de creación ──────────────────────────────────────────────────────

    [Fact]
    public void Create_ConDatosValidos_DebeRetornarPrestamoActivo()
    {
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var loan = Loan.Create(bookId, userId, durationDays: 14);

        loan.LoanId.Should().NotBeEmpty();
        loan.BookId.Should().Be(bookId);
        loan.UserId.Should().Be(userId);
        loan.Status.Should().Be(LoanStatus.Active);
        loan.ReturnDate.Should().BeNull();
        loan.DueDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(14), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_DuracionPersonalizada_DebeCalcularFechaCorrecta()
    {
        var loan = Loan.Create(Guid.NewGuid(), Guid.NewGuid(), durationDays: 7);

        loan.DueDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromSeconds(5));
    }

    // ── Tests de devolución ────────────────────────────────────────────────────

    [Fact]
    public void RegisterReturn_PrestamoActivo_DebeActualizarEstadoYFecha()
    {
        var loan = Loan.Create(Guid.NewGuid(), Guid.NewGuid(), durationDays: 14);

        loan.RegisterReturn();

        loan.ReturnDate.Should().NotBeNull();
        loan.ReturnDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        loan.Status.Should().Be(LoanStatus.Returned);
    }

    [Fact]
    public void RegisterReturn_PrestamoYaDevuelto_DebeLanzarExcepcion()
    {
        var loan = Loan.Create(Guid.NewGuid(), Guid.NewGuid());
        loan.RegisterReturn();

        var act = () => loan.RegisterReturn();

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*ya fue devuelto*");
    }
}
