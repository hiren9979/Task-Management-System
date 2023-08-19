

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Management_System.Models;

namespace Task_Management_System.taskManagement.Context
{
    public class TaskManagementDbContext : IdentityDbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Task_Management_System.Models.Task> Task { get; set; }
        public DbSet<Task_Management_System.Models.Comment> Comments { get; set; }


    }
}
