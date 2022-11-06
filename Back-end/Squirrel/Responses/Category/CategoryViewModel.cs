using System.Transactions;

namespace Squirrel.Responses.Category
{
    public sealed record class CategoryViewModel(
        int Id,
        string Name,
        string Color,
        IReadOnlyCollection<Transaction> Transactions);
}
