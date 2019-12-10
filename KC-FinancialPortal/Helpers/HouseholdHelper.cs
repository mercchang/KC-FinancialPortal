using KC_FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_FinancialPortal.Helpers
{
    public class HouseholdHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool IsConfigured(string userId)
        {
            var householdId = db.Users.Find(userId).HouseholdId ?? 0;
            if (householdId == 0)
                return false;
            var houseHold = db.Households.Find(householdId);
            var acctCount = houseHold.BankAccounts.Count();
            var budgetCount = houseHold.Budgets.Count();
            var itemCount = houseHold.Budgets.SelectMany(b => b.BudgetItems).Count();
            return (acctCount > 0 && budgetCount > 0 && itemCount > 0);
        }

        public string FindHouse(int householdId)
        {
            var houseName = "";
            if (householdId != 0)
            {
                houseName = db.Households.Find(householdId).Name;
                if (houseName != null)
                    return (houseName);
                else
                {
                    houseName = "";
                    return (houseName);
                }
            }

            return (houseName);
        }
    }
}