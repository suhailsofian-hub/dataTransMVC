using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthProject.Models;
using static AuthProject.Helpers.Helper;
using AuthProject.Data;
using AuthProject.Helpers;
using AuthProject.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AuthProject.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
         private ITransactionRepository _transactionRepository;
        // private readonly ApplicationDbContext _context;
        // private readonly TransactionDbContext _context;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _transactionRepository.GetAllAsync());
        }

        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new TransactionModel());
            else
            {
                var transactionModel = await _transactionRepository.GetByIdAsync(id);
                if (transactionModel == null)
                {
                    return NotFound();
                }
                return View(transactionModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] TransactionModel transactionModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    transactionModel.Date = DateTime.Now;
                    _transactionRepository.Add(transactionModel);
                    // await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _transactionRepository.Update(transactionModel);
                        // await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(transactionModel.TransactionId))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                // Console.WriteLine("result--> "+Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _transactionRepository.GetAll()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionModel) });
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionModel = await _transactionRepository.GetByIdAsync(id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            return View(transactionModel);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionModel = await _transactionRepository.GetByIdAsync(id);
           _transactionRepository.Delete(transactionModel);
            // await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _transactionRepository.GetAll()) });
        }

        private bool TransactionModelExists(int id)
        {
            return _transactionRepository.exist(id);
        }
    }
}
