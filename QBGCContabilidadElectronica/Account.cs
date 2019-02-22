using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QB_App
{
    public class Account
    {
        public CatalogoCtas satAccount { get; set; }
        public string type { get; set; }
        public string listId { get; set; }
        public string parentListId { get; set; }
        public double initialAmount { get; set; }
        public double debit { get; set; }
        public double finalAmount { get; set; }
        public double credit { get; set; }
        public string GetNature()
        {
            switch (type) {
                case "Income":
                case "Loan":
                case "CreditCard":
                case "Equity":
                case "AccountsPayable":
                case "OtherCurrentLiability":
                case "LongTermLiability":
                case "OtherIncome":
                    return "A";
            }
            return "D";
        }
    }
}
