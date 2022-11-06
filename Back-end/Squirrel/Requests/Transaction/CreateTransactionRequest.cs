namespace Squirrel.Requests.Transaction
{
    public record CreateTransactionRequest(double Amount, string Description, int CategoryId, DateTime Time);
}
