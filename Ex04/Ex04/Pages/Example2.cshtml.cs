using Ex04.DataAccess.Example2;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ex04.Pages
{
    public class Example2Model : PageModel
    {
        readonly Example2DbContext _dbContext;

        public Example2Model(Example2DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            ViewData["clients"] = _dbContext.Clients.AsNoTracking().ToList();
            ViewData["products"] = _dbContext.Products.AsNoTracking().ToList();

            ViewData["orders"] = _dbContext
                .Orders
                .AsNoTracking()
                .Include(order => order.Client)
                .Include(order => order.Product)
                .ToList();
        }
    }
}
