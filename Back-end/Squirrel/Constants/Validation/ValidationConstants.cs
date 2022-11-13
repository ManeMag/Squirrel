namespace Squirrel.Constants.Validation
{
    public static class ValidationConstants
    {
        public const string HexademicalColorRegexp = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        public const double TransactionMinValue = -1_000_000_000;

        public const double TransactionMaxValue = 1_000_000_000;

    }
}
