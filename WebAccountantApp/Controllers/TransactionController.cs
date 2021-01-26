using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;
using WebAccountantApp.Models;

namespace WebAccountantApp.Controllers
{
    public class TransactionController : Controller
    {
        #region Dependency Injuction

        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepo, IAccountRepository accountRepo, IMapper mapper)
        {
            _transactionRepo = transactionRepo;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }
        #endregion

        // GET: TransactionController
        public async Task<ActionResult> Index()
        {
            var transactions = await _transactionRepo.FindAll();
            //set transaction in order so that the latest transaction shows first
            var orderedTran = transactions.OrderByDescending(tran => tran.Date);
            var model = _mapper.Map<List<TransactionVM>>(orderedTran);

            return View(model);
        }

        

        public async Task<ActionResult> Add(CreateTransactionVM model)
        {
            try
            {
                //Map and save transaction
                var transaction = model.Transaction;
                transaction.Date = DateTime.Now;
                var mappedTransaction = _mapper.Map<Transaction>(transaction);
                var success = await _transactionRepo.Create(mappedTransaction);

                //Get and update account values
                var accountDebited = await _accountRepo.FindById(transaction.DebitId);
                var accountCredited = await _accountRepo.FindById(transaction.CreditId);

                //Update Debited Account, Expense Accounts gain value when debited 
                if (accountDebited.AccountType == AccountType.Debit || accountDebited.AccountType == AccountType.Expense)
                    accountDebited.Value += transaction.Value;
                //Credit account loose value if debited
                else
                    accountDebited.Value -= transaction.Value;

                //Update Credited Account, Credit account gains value when credited and Income gains value.
                if (accountCredited.AccountType == AccountType.Credit || accountCredited.AccountType == AccountType.Income)
                    accountCredited.Value += transaction.Value;
                else
                    accountCredited.Value -= transaction.Value;

                var success2 = await _accountRepo.Update(accountCredited);
                var success3 = await _accountRepo.Update(accountDebited);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //Since changes to transaction affects account balance, 
        //it will be easier to just delete the transaction in case of mistak

        // GET: TransactionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransactionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
