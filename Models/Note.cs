using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace todolist.Models{
    public class Note {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public List<Task> Tasks { get; set; }
          
    }
}