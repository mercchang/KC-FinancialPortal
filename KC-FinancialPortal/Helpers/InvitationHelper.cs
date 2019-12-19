using KC_FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_FinancialPortal.Helpers
{
    public class InvitationHelper
    {
        public static async void MarkAsInvalid(int id)
        {
            var db = new ApplicationDbContext();
            var invitation = db.Invitations.Find(id);
            invitation.IsValid = false;
            await db.SaveChangesAsync();
        }
    }
}