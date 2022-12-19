namespace Squirrel.Responses.Statistics
{
    public sealed class StatisticsViewModel
    {
        public record TransactionImpact(
            int CategoryId,
            decimal TotalPercentage,
            double TotalMoney,
            int TransactionsCount);

        public IEnumerable<TransactionImpact> Impact { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public double Income { get; set; }
        public double Outcome { get; set; }
    }
}
