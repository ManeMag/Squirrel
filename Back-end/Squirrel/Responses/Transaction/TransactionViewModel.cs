namespace Squirrel.Responses.Transaction
{
    public sealed record TransactionViewModel(
        int Id,
        DateTime Time,
        double Amount,
        string Description,
        int CategoryId);
}
