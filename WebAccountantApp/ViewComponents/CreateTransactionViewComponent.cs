using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;
using WebAccountantApp.Models;

namespace WebAccountantApp.ViewComponents
{
    public class CreateTransactionViewComponent : ViewComponent
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public CreateTransactionViewComponent(IAccountRepository accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var accounts = await _accountRepo.FindAll();
            var mappedAccounts = _mapper.Map<List<AccountVM>>(accounts);
            var model = new CreateTransactionVM
            {
                Accounts = mappedAccounts
            };

            return View(model);
        }
    }
}
