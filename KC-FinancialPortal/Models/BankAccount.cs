using KC_FinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KC_FinancialPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string OwnerId { get; set; }

        [StringLength(50, MinimumLength = 2), Required]
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public AccountType AccountType { get; set; }
        public float StartingBalance { get; set; }
        public float CurrentBalance { get; set; }
        public float? LowBalance { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}