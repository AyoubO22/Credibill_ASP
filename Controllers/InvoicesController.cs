using Credibill_ASP.Data.CrediBill_Web.Data;
using CrediBill_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Credibill_ASP.Controllers
{
    [Authorize (Roles = "Admin")]
    public class InvoicesController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string customerFilter, string orderBy)
        {
            var query = _context.Invoices.Include(i => i.Customer).AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(customerFilter))
            {
                query = query.Where(i => i.Customer.Email.Contains(customerFilter));
            }

            // Sorting
            switch (orderBy)
            {
                case "AmountAsc":
                    query = query.OrderBy(i => i.Amount);
                    break;

                case "AmountDesc":
                    query = query.OrderByDescending(i => i.Amount);
                    break;

                case "InvoiceDateAsc":
                    query = query.OrderBy(i => i.InvoiceDate);
                    break;

                case "InvoiceDateDesc":
                    query = query.OrderByDescending(i => i.InvoiceDate);
                    break;

                case "CustomerAsc":
                    query = query.OrderBy(i => i.Customer.Email);
                    break;

                case "CustomerDesc":
                    query = query.OrderByDescending(i => i.Customer.Email);
                    break;

                default:
                    query = query.OrderBy(i => i.Id);
                    break;
            }

            var invoices = await query.ToListAsync();
            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,InvoiceDate,Deleted,CustomerId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", invoice.CustomerId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Amount,InvoiceDate,Deleted,CustomerId")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", invoice.CustomerId);
            return View(invoice);
        }

        private async Task<bool> InvoiceExists(int id)
        {
            return await _context.Invoices.AnyAsync(e => e.Id == id);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                invoice.Deleted = DateTime.Now; // Markeer als verwijderd
                _context.Update(invoice);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}