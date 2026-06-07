using LoanService.Domain.Enums;

namespace LoanService.Domain.Entities;

public class Loan
{
    public Guid        LoanId     { get; private set; }
    public Guid        BookId     { get; private set; }
    public Guid        UserId     { get; private set; }
    public DateTime    LoanDate   { get; private set; }
    public DateTime    DueDate    { get; private set; }
    public DateTime?   ReturnDate { get; private set; }
    public LoanStatus  Status     { get; private set; }

    // Constructor privado para EF Core
    private Loan() { }

    // Factory method: único punto de creación
    public static Loan Create(Guid bookId, Guid userId, int durationDays = 14)
    {
        return new Loan
        {
            LoanId   = Guid.NewGuid(),
            BookId   = bookId,
            UserId   = userId,
            LoanDate = DateTime.UtcNow,
            DueDate  = DateTime.UtcNow.AddDays(durationDays),
            Status   = LoanStatus.Active
        };
    }

    // Método de dominio: registrar devolución
    public void RegisterReturn()
    {
        if (Status == LoanStatus.Returned)
            throw new InvalidOperationException("El préstamo ya fue devuelto.");

        ReturnDate = DateTime.UtcNow;
        Status     = ReturnDate > DueDate ? LoanStatus.Overdue : LoanStatus.Returned;
    }
}
