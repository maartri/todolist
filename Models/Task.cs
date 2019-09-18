using System.ComponentModel.DataAnnotations.Schema;


namespace todolist.Models{
    public class Task {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        public string TaskContent { get; set; }
        public bool Status { get; set; } = false;
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}