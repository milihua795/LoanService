using LoanService.Application.Commands;
using LoanService.Application.Queries;
using LoanService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoansController : ControllerBase
{
    private readonly CreateLoanHandler _createHandler;
    private readonly ReturnLoanHandler _returnHandler;
    private readonly GetLoanHandler    _getHandler;

    public LoansController(
        CreateLoanHandler createHandler,
        ReturnLoanHandler returnHandler,
        GetLoanHandler    getHandler)
    {
        _createHandler = createHandler;
        _returnHandler = returnHandler;
        _getHandler    = getHandler;
    }

    /// <summary>Registra un nuevo préstamo y publica el evento LoanCreated.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Loan), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanRequest request)
    {
        if (request.BookId == Guid.Empty || request.UserId == Guid.Empty)
            return BadRequest("BookId y UserId son requeridos.");

        var loan = await _createHandler.HandleAsync(request);
        return CreatedAtAction(nameof(GetLoan), new { id = loan.LoanId }, loan);
    }

    /// <summary>Registra la devolución de un libro y publica el evento BookReturned.</summary>
    [HttpPut("{id:guid}/return")]
    [ProducesResponseType(typeof(Loan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReturnLoan(Guid id)
    {
        try
        {
            var loan = await _returnHandler.HandleAsync(id);
            return Ok(loan);
        }
        catch (KeyNotFoundException ex)      { return NotFound(ex.Message); }
        catch (InvalidOperationException ex)  { return BadRequest(ex.Message); }
    }

    /// <summary>Obtiene un préstamo por su ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Loan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLoan(Guid id)
    {
        try   { return Ok(await _getHandler.HandleAsync(id)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    /// <summary>Lista todos los préstamos activos de un usuario.</summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<Loan>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await _getHandler.HandleByUserAsync(userId));
}
