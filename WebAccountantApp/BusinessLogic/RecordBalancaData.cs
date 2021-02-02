using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;

namespace WebAccountantApp.BusinessLogic
{
    public static class RecordBalancaData
    {
        //Had to use .Result because was having an "Disposed Object" error. 
        public static void CheckDate(IDateKeeper dateKeeper, IBalanceReport balanceRepo, IAccountRepository accountRepo)
        {
            bool success;
            var exist = dateKeeper.Exists(1).Result;
            var todaysDate = DateTime.Now.Date;

            if (!exist)
                success = CreateDateKeeper(dateKeeper, todaysDate);

            else
            {
                
                var lastVisit = dateKeeper.FindById(1).Result;
                //if the month has changed since last record, Make new record and update date. 
                if(lastVisit.LastStarted.GetValueOrDefault().Month != todaysDate.Month)
                {
                    success = RecordBalance(balanceRepo, accountRepo).Result;
                    success = dateKeeper.Update(lastVisit).Result;

                }

            }
        }

        //Create a new entry for DateKeeper if doesnt exist
        public static bool CreateDateKeeper(IDateKeeper dateKeeper, DateTime todaysDate)
        {
            
            var lastVisit = new DateKeeper
            {
                LastStarted = todaysDate
            };
            return dateKeeper.Create(lastVisit).Result;
        }

        //Record the state of all accounts if the month has changed
        public async static Task<bool> RecordBalance(IBalanceReport balanceRepo, IAccountRepository accountRepo)
        {
            //created a variable to keep async bool returns, set it to false just in case the code doesnt work, will return false
            bool success = false;
            var accounts = await accountRepo.GetDebitAndCredit();
            //Because the record is happening in the new month any first day the app is accessed, but
            //the report needs to be for the end of the last month. So the date is last months date. 
            var lastMonthsDate = DateTime.Now.AddMonths(-1);

            foreach (var acc in accounts)
            {
                var balanceReport = new BalanceReport
                {
                    AccountId = acc.Id,
                    Date = lastMonthsDate,
                    Value = acc.Value
                };
                success = await balanceRepo.Create(balanceReport);
            }

            return success;
        }
    }
}
