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

	public class AccountController : Controller
	{
		#region Dependency Injuction
	
		private readonly IAccountRepository _accountRepo;
		private readonly IMapper _mapper;


		public AccountController(IAccountRepository accountRepo, IMapper mapper)
		{
			_accountRepo = accountRepo;
			_mapper = mapper;
		}
		#endregion

		// GET: AccountController
		public async Task<ActionResult> Index()
		{
			var accounts = await _accountRepo.FindAll();
			var model = _mapper.Map<List<AccountVM>>(accounts);

			return View(model);
		}


		// POST: AccountController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(CreateAccountVM model)
		{
			try
			{
				
				var account = model.Account;

				var mappedAccount = _mapper.Map<Account>(account);
				var success = await _accountRepo.Create(mappedAccount);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: AccountController/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			var account = await _accountRepo.FindById(id);
			var accountTypes = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().ToList();
			var mappedAccount = _mapper.Map<AccountVM>(account);
			var model = new CreateAccountVM
			{
				Account = mappedAccount,
				AccountTypes = accountTypes
			};

			return View(model);
		}

		// POST: AccountController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(CreateAccountVM model)
		{
			try
			{
				var account = _mapper.Map<Account>(model.Account);
				var success = await _accountRepo.Update(account);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return RedirectToAction(nameof(Index));
			}
		}

		public async Task<ActionResult> Delete(int id)
		{
			try
			{
				var account = await _accountRepo.FindById(id);
				var success = await _accountRepo.Delete(account);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return RedirectToAction(nameof(Index));
			}
		}
	}
}
