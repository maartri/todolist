using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using todolist.Models;
namespace todolist.DatabaseContext {
    public class TodoContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<Note> Note { get; set; }
        public DbSet<Task> Task { get; set; }

        public TodoContext(IConfiguration config,DbContextOptions<TodoContext> options)
            :base(options){ 
                this._config = config;
            }
            
        
        /*
            https://docs.microsoft.com/en-us/ef/core/modeling/
         */
        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<Note>()
                    .ToTable("Note")
                    .HasKey(x => x.NoteId);

            builder.Entity<Task>()
                    .ToTable("Task")                    
                    .HasKey(x => x.TaskId);

            builder.Entity<Task>()
                    .ToTable("Task")       
                    .HasOne(x => x.Note)
                    .WithMany(t => t.Tasks)
                    .HasForeignKey(x => x.NoteId);



            // builder.Entity<Note>().HasData(
            //     new Note(){
            //         NoteId = 1,
            //         NoteTitle = "Household Chores",
            //         Tasks = new List<Task>() {
            //             new Task() { TaskId = 0, TaskContent = "Sweep Floor", NoteId =1, Note = new Note() { NoteId =1 , NoteTitle = "Household Chores"} }
            //         }
            //     }
            // );

                    
        }
    }
}