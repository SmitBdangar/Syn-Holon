using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstMvcApp.Models;
using FirstMvcApp.Data;

namespace FirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger,
                              AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Expense()
        {
            var allExpenses = _context.Expenses.ToList();
            var totalValue = allExpenses.Sum(x => x.Value);

            ViewBag.Expenses = totalValue;  
            return View(allExpenses);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreateEditExpenses(int? id)
        {
            if (id != null && id > 0)
            {
                var expense = _context.Expenses.FirstOrDefault(x => x.Id == id);
                if (expense == null) return NotFound();

                return View(expense);
            }
            return View(new Expense());
        }

        // Save Create or Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    _context.Expenses.Add(model);
                }
                else
                {
                    _context.Expenses.Update(model);
                }

                _context.SaveChanges();
                return RedirectToAction("Expense");
            }

            return View("CreateEditExpenses", model);
        }

        public IActionResult DeleteExpenses(int id)
        {
            var expenseDb = _context.Expenses.FirstOrDefault(y => y.Id == id);
            if (expenseDb == null) return NotFound();

            _context.Expenses.Remove(expenseDb);
            _context.SaveChanges();

            return RedirectToAction("Expense");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
