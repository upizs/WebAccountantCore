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
        public async static void CheckDate(IDateKeeper dateKeeper)
        {
            bool success;
            var exist = await dateKeeper.Exists(1);
            var todaysDate = DateTime.Now.Date;
            if (!exist)
            {
                var lastVisit = new DateKeeper
                {
                    LastStarted = todaysDate
                };

                success = await dateKeeper.Create(lastVisit);
            }
            else
            {
                //Had to use Result because otherwise the connection would close, will inspect this closer later
                var lastVisit = dateKeeper.FindById(1).Result;
                if (lastVisit.LastStarted.GetValueOrDefault().Month == todaysDate.Month)
                {
                    lastVisit.LastStarted = todaysDate;
                    await dateKeeper.Update(lastVisit);
                }
                //This is where I call RecordBalanceData method
                else
                {

                }

            }
        }
    }
}
