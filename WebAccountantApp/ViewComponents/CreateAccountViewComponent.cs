using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;
using WebAccountantApp.Models;

namespace WebAccountantApp.ViewComponents
{
    public class CreateAccountViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var accountTypes = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().ToList();
            var model = new CreateAccountVM 
            { 
                AccountTypes = accountTypes
            };
            return View(model);
        }
    }
}
