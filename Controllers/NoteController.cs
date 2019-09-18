using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using todolist.DatabaseContext;
namespace todolist.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : Controller
    {

        private readonly TodoContext _context;
        public NoteController(
            TodoContext context
        )
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetNote()
        {
           using(var conn = _context.Database.GetDbConnection() as SqlConnection)
           {
               using(SqlCommand cmd = new SqlCommand(@"SELECT * FROM Note", conn))
               {
                   await conn.OpenAsync();
                   using(SqlDataReader rd = await cmd.ExecuteReaderAsync())
                   {
                       List<dynamic> list = new List<dynamic>();
                       while(await rd.ReadAsync())
                       {
                           list.Add(new {
                               NoteId = rd.GetInt32(0),
                               NoteTitle = rd.GetString(1)
                           });
                       }
                       return Ok(list);
                   }
               }
           }
        }

        [HttpGet("note-tasks")]
        public async Task<IActionResult> GetNoteAndTasks()
        {
           try
           {
                using(var conn = _context.Database.GetDbConnection() as SqlConnection)
                {
                    using(SqlCommand cmd = new SqlCommand(@"SELECT * FROM Note n INNER JOIN task t ON n.NoteId = t.NoteId", conn))
                    {
                        await conn.OpenAsync();
                        using(SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();                   
                            while(await rd.ReadAsync())
                            {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();

                                    dic.Add("NoteId", rd.GetInt32(0));
                                    dic.Add("NoteTitle", rd.GetString(1));
                                    dic.Add("TaskId", rd.GetInt32(2));
                                    dic.Add("TaskContent", rd.GetString(3));
                                    dic.Add("Status", rd.GetBoolean(4));

                                    list.Add(dic);                               
                            }

                            // Separate Distinct NoteId to separate object per noteid
                            var distintNoteId = list.Select(x => x["NoteId"]).Distinct();

                            List<dynamic> listFinal  = new List<dynamic>();

                            foreach (int note in distintNoteId){
                                var tasks = list.Where(x => Convert.ToInt32(x["NoteId"]) == note).Select(x => x);

                                listFinal.Add(new {
                                    Note = (tasks.First()["NoteTitle"]).ToString(),
                                    NoteId = (tasks.First()["NoteId"]),
                                    Tasks = tasks.Select(x => new {
                                        TaskId = x["TaskId"],
                                        TaskContent = x["TaskContent"],
                                        Status = x["Status"]
                                    })
                                });                                
                            }

                            return Ok(listFinal);
                        }
                    }
           }
           } catch(Exception ex)
           {
               return BadRequest(ex);
           }
        }

    }
}
