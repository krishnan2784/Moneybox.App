namespace Moneybox.App
{
    public class AccountSettings
    {
        public AccountSettings()
        {
            
        }

        public AccountSettings(decimal lowFundsWarningAmount, decimal payInLimit, decimal payInLimitApproachWarningAmount)
        {
            LowFundsWarningAmount = lowFundsWarningAmount;
            PayInLimit = payInLimit;
            PayInLimitApproachWarningAmount = payInLimitApproachWarningAmount;
        }
        public decimal LowFundsWarningAmount { get; set; }
        public decimal PayInLimit { get; set; }
        public decimal PayInLimitApproachWarningAmount { get; set; }
    }
}