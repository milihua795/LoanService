using FluentAssertions;
using LoanService.Application.Commands;
using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;
using LoanService.Domain.Enums;
using Moq;
using Xunit;

namespace LoanService.Tests.Application;

public class ReturnLoanHandlerTests
{
    private readonly Mock<ILoanRepository> _repoMock      = new();
    private readonly Mock<IEventPublisher> _publisherMock = new();
    private readonly ReturnLoanHandler     _handler;

    public ReturnLoanHandlerTests()
    {
        _handler = new ReturnLoanHandler(_repoMock.Object, _publisherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_PrestamoExistente_DebeDevolver_YPublicarEvento()
    {
        // Arrange
        var loan = Loan.Create(Guid.NewGuid(), Guid.NewGuid(), durationDays: 14);

        _repoMock.Setup(r => r.GetByIdAsync(loan.LoanId))
                 .ReturnsAsync(loan);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);
        _publisherMock.Setup(p => p.PublishAsync(It.IsAny<object>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(loan.LoanId);

        // Assert
        result.ReturnDate.Should().NotBeNull();
        result.Status.Should().Be(LoanStatus.Returned);

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Loan>()), Times.Once);
        _publisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_PrestamoNoExiste_DebeLanzarKeyNotFoundException()
    {
        // Arrange — repositorio devuelve null (no encontrado)
        var loanId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(loanId))
                 .ReturnsAsync((Loan?)null);

        // Act & Assert
        await _handler.Invoking(h => h.HandleAsync(loanId))
              .Should().ThrowAsync<KeyNotFoundException>()
              .WithMessage($"*{loanId}*");

        _publisherMock.Verify(
            p => p.PublishAsync(It.IsAny<object>()),
            Times.Never);
    }
}
