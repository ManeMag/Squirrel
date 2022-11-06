namespace Squirrel.Requests.Transaction
{
    public record UpdateTransactionRequest(
        int Id,
        double Amount,
        string Description,
        int CategoryId,
        DateTime Time);
}
