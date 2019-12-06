using KC_FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KC_FinancialPortal.Controllers
{
    //public class ChartsController : Controller
    //{
    //    private ApplicationDbContext db = new ApplicationDbContext();
    //    // GET: Charts
    //    public JsonResult GetBudgetData(int houseId)
    //    {
    //        var budgets = db.Budgets.Where(b => b.HouseholdId == houseId);
    //        var budgetItems = budgets.SelectMany(b => b.BudgetItems);

    //        var data = new ChartData
    //        {
    //            Names = budgets.Select(b => b.Name).ToList(),
    //            //Targets = budgets.SelectMany(b => b.BudgetItems).Sum(b => b.TargetAmount)
    //        };


    //        return View();
    //    }
    //}
}