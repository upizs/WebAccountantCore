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

        
        // TODO Subtransaction option, when if added another transaction under current would split the sum and place the values in correct accounts
        // EXAMPLE When spent 25 on groceries, but 5 of that was for sweets, then subtransaction would take those 5 out of total and add to sweets automatically.
        public async Task<ActionResult> Add(CreateTransactionVM model)
        {
            try
            {
                //Map and save transaction
                var transaction = model.Transaction;
                transaction.Date = DateTime.Now;
                var mappedTransaction = _mapper.Map<Transaction>(transaction);
                var success = await _transactionRepo.Create(mappedTransaction);
                if (!success)
                    throw new Exception("Failed to create transaction");

                //Get and update account values
                var accountDebited = await _accountRepo.FindById(transaction.DebitId);
                var accountCredited = await _accountRepo.FindById(transaction.CreditId);

                var success2 = await UpdateAccounts(accountDebited, accountCredited, transaction.Value);
                

                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //Since changes to transaction affects account balance, 
        //it will be easier to just delete the transaction in case of mistak

        
        
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //Map and save transaction
                var transaction = await _transactionRepo.FindById(id);

                //Get and update account values
                var accountDebited = await _accountRepo.FindById(transaction.DebitId);
                var accountCredited = await _accountRepo.FindById(transaction.CreditId);

                //Reverse the impact on the accounts that the transaction had by putting in negative value
                var success1 = await UpdateAccounts(accountDebited, accountCredited, -transaction.Value);

                var success = await _transactionRepo.Delete(transaction);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }
        //TODO need to find out why and how to add value to data and keeping the decimals at the same point and to find out how to fix already long decimals.
        public async Task<bool> UpdateAccounts(Account accountDebited, Account accountCredited, double value)
        {
            if (accountDebited.AccountType == AccountType.Debit || accountDebited.AccountType == AccountType.Expense)
                accountDebited.Value += value;
            //Credit account loose value if debited
            else
                accountDebited.Value -= value;
            var success = await _accountRepo.Update(accountDebited);
            if (!success)
                throw new Exception("Failed to update Account " + accountDebited.Name);

            //Update Credited Account, Credit account gains value when credited and Income gains value.
            if (accountCredited.AccountType == AccountType.Credit || accountCredited.AccountType == AccountType.Income)
                accountCredited.Value += value;
            else
                accountCredited.Value -= value;

            var success1 = await _accountRepo.Update(accountCredited);
            if (!success1)
                throw new Exception("Failed to update Account " + accountCredited.Name);

            return (success && success1);

        } 
    }
}
