using FluentAssertions;
using LoanService.Application.Commands;
using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;
using LoanService.Domain.Enums;
using Moq;
using Xunit;

namespace LoanService.Tests.Application;

public class CreateLoanHandlerTests
{
    // Mocks de las dependencias
    private readonly Mock<ILoanRepository> _repoMock     = new();
    private readonly Mock<IEventPublisher> _publisherMock = new();
    private readonly CreateLoanHandler     _handler;

    public CreateLoanHandlerTests()
    {
        _handler = new CreateLoanHandler(_repoMock.Object, _publisherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_DatosValidos_DebeGuardarPrestamoYPublicarEvento()
    {
        // Arrange
        var request = new CreateLoanRequest(Guid.NewGuid(), Guid.NewGuid(), 14);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);
        _publisherMock.Setup(p => p.PublishAsync(It.IsAny<object>()))
                      .Returns(Task.CompletedTask);

        // Act
        var loan = await _handler.HandleAsync(request);

        // Assert — el préstamo se creó correctamente
        loan.Should().NotBeNull();
        loan.BookId.Should().Be(request.BookId);
        loan.UserId.Should().Be(request.UserId);
        loan.Status.Should().Be(LoanStatus.Active);

        // Assert — se llamó al repositorio exactamente una vez
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(),         Times.Once);

        // Assert — se publicó el evento exactamente una vez
        _publisherMock.Verify(
            p => p.PublishAsync(It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_FallaRepositorio_NoDebePublicarEvento()
    {
        // Arrange — el repositorio lanza excepción
        var request = new CreateLoanRequest(Guid.NewGuid(), Guid.NewGuid());

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .ThrowsAsync(new Exception("Error de base de datos"));

        // Act & Assert
        await _handler.Invoking(h => h.HandleAsync(request))
              .Should().ThrowAsync<Exception>()
              .WithMessage("Error de base de datos");

        // El evento NO debe publicarse si el repositorio falla
        _publisherMock.Verify(
            p => p.PublishAsync(It.IsAny<object>()),
            Times.Never);
    }
}
