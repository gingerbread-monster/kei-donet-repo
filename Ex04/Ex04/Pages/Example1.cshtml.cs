using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ex04.DataAccess.Example1;

namespace Ex04.Pages
{
    public class Example1Model : PageModel
    {
        readonly Example1DbContext _dbContext;
        public List<string> StudentsNames { get; set; }

        public Example1Model(Example1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGet()
        {
            StudentsNames = await _dbContext
                .Students
                .Select(student => student.Name)
                .ToListAsync();
        }
    }
}
