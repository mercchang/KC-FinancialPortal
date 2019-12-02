using KC_FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_FinancialPortal.Helpers
{
    public class RoleHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        public ICollection<string> ListDemoRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public bool IsDemoUser(string userId)
        {
            return userManager.GetRoles(userId).FirstOrDefault().Contains("Demo");
        }

        // Gets the role name and removes "Demo" from the demo role name, and also adds a space between project manager
        public string GetRoleName(string userId)
        {
            var userRole = userManager.GetRoles(userId).FirstOrDefault().ToString();
            var role = "";

            if (userRole != null)
            {
                switch (userRole)
                {
                    case "Admin":
                        role = "Administrator";
                        break;
                    case "HeadOfHousehold":
                        role = "Head Of Household";
                        break;
                    case "Member":
                        role = "Member";
                        break;
                    default:
                        role = userRole;
                        break;
                }
            }
            return role;
        }
    }
}