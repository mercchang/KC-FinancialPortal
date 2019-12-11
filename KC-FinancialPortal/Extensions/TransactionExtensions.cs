using KC_FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_FinancialPortal.Extensions
{
    public static class TransactionExtensions
    {
        public static ApplicationDbContext db = new ApplicationDbContext();

        public static void UpdateBalance(this Transaction transaction)
        {
            UpdateBankBalance(transaction);
            UpdateBudgetBalance(transaction);
            UpdateBudgetItemBalance(transaction);
        }

        private static void UpdateBankBalance(Transaction transaction)
        {
            var bank = db.BankAccounts.Find(transaction.BankAccountId);
            if (transaction.TransactionType == Enums.TransactionType.Deposit)
                bank.CurrentBalance += transaction.Amount;
            else
                bank.CurrentBalance -= transaction.Amount;

            db.SaveChanges();
        }

        private static void UpdateBudgetBalance(Transaction transaction)
        {
            if (transaction.TransactionType == Enums.TransactionType.Deposit || transaction.BudgetItemId == null)
                return;

            var budgetId = db.BudgetItems.Find(transaction.BudgetItemId).BudgetId;
            var budget = db.Budgets.Find(budgetId);
            budget.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        private static void UpdateBudgetItemBalance(Transaction transaction)
        {
            if (transaction.TransactionType == Enums.TransactionType.Deposit || transaction.BudgetItemId == null)
                return;

            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            budgetItem.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }
    }
}