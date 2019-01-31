using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

namespace ProERP.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [TableName("UserProfile")]
    public partial class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVersionUpdated { get; set; }
    }

    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public virtual UserProfile UserProfile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Role : IdentityRole<int, UserRole>
    {

    }

    public class UserRole : IdentityUserRole<int>
    {

    }

    public class UserLogin : IdentityUserLogin<int>
    {

    }

    public class UserClaim : IdentityUserClaim<int>
    {

    }

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("ProERPContext", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}

    public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext()
            : base("ProERPContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<Role>()
                .ToTable("Roles");

            modelBuilder.Entity<UserRole>()
                .ToTable("UserRoles");

            modelBuilder.Entity<UserClaim>()
                .ToTable("UserClaims");

            modelBuilder.Entity<UserLogin>()
                .ToTable("UserLogins");

            modelBuilder.Entity<UserProfile>()
                .ToTable("UserProfile");
        }
    }

    public class UserStore :
    UserStore<User, Role, int,
    UserLogin, UserRole, UserClaim>, IUserStore<User, int>, IDisposable
    {
        public UserStore()
            : this(new ApplicationDbContext())
        {
            base.DisposeContext = true;
        }

        public UserStore(DbContext context)
            : base(context)
        {
        }
    }

    //public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    //{
    //    public UserStore(ApplicationDbContext context)
    //        : base(context)
    //    {
    //    }

    //}

    public class RoleStore
: RoleStore<Role, int, UserRole>,
IQueryableRoleStore<Role, int>,
IRoleStore<Role, int>, IDisposable
    {
        public RoleStore()
            : base(new ApplicationDbContext())
        {
            base.DisposeContext = true;
        }

        public RoleStore(DbContext context)
            : base(context)
        {
        }

    }
    //public class RoleStore : RoleStore<Role, int, UserRole>
    //{
    //    public RoleStore(ApplicationDbContext context) : base(context)
    //    {
    //    }
    //}
}