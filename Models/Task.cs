using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Task_Management_System.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public string? AssignedUserId { get; set; }
     
        [Required]
        public TaskPriority Priority { get; set; }


        public enum TaskStatus
        {
            Todo,
            InProgress,
            Done
        }

        public enum TaskPriority
        {
            High,
            Medium,
            Low
        }

    }
}
