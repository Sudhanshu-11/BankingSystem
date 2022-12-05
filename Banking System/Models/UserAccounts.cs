using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking_System.Models
{
    public class UserAccounts
    {
        public int Id { get; set; }
        public int UsersId { get; set; }
        public string UserAccName { get; set; }
        public decimal DepositAmt { get; set; }
        public decimal WithdrawlAmt { get; set; }
        public decimal AvailableBalance { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
