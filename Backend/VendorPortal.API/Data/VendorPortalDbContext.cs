using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace VentorPortal.API.Data
{
    public class VentorPortalDbContext : IdentityDbContext
    {
        public VentorPortalDbContext(DbContextOptions<VentorPortalDbContext> options) : base(options)
        {
        }
    }
}
