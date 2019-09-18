using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using todolist.DatabaseContext;

namespace todolist.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly TodoContext _context;
        public TaskController(
            TodoContext context
        )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
           using(var conn = _context.Database.GetDbConnection() as SqlConnection)
           {
               using(SqlCommand cmd = new SqlCommand(@"SELECT * FROM Task", conn))
               {
                   await conn.OpenAsync();
                   using(SqlDataReader rd = await cmd.ExecuteReaderAsync())
                   {
                       List<dynamic> list = new List<dynamic>();
                       while(await rd.ReadAsync())
                       {
                           list.Add(new {
                               TaskId = rd.GetInt32(0),
                               TaskContent = rd.GetString(1),
                               Status = rd.GetBoolean(2)
                           });
                       }
                       return Ok(list);
                   }
               }
           }
        }


    }
}
