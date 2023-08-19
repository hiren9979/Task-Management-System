namespace Task_Management_System.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; } // User who posted the comment
        public int TaskId { get; set; }

    }
}
