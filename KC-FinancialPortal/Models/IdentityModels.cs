using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KC_FinancialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must have a minimum length of 1 and maximum length of 50.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must have a minimum length of 1 and maximum length of 50.")]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must have a minimum length of 1 and maximum length of 50.")]
        public string DisplayName { get; set; }

        public string AvatarPath { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public int? HouseholdId { get; set; }

        public ApplicationUser()
        {
            Notifications = new HashSet<Notification>();
            BankAccounts = new HashSet<BankAccount>();
            Transactions = new HashSet<Transaction>();
            Budgets = new HashSet<Budget>();

        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.Household> Households { get; set; }

        //public System.Data.Entity.DbSet<KC_FinancialPortal.Models.ApplicationUser> ApplicationUsers { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.Budget> Budgets { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.BudgetItem> BudgetItems { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.Invitation> Invitations { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.Transaction> Transactions { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }

        public System.Data.Entity.DbSet<KC_FinancialPortal.Models.UserProfileViewModel> UserProfileViewModels { get; set; }
    }
}