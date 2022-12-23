using Squirrel.Responses.Transaction;

namespace Squirrel.Responses.Category
{
    public sealed record class CategoryViewModel(
        int Id,
        string Name,
        string Color,
        DataAccess.Entities.Type Type,
        IReadOnlyCollection<TransactionViewModel> Transactions,
        bool IsBaseCategory);
}
