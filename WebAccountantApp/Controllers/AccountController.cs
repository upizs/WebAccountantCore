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

		// GET: AccountController/Create
		public ActionResult Create()
		{
			var accountTypes = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().ToList();
			
			return View();
		}

		// POST: AccountController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(AccountVM account)
		{
			try
			{
				var mappedAccount = _mapper.Map<Account>(account);
				await _accountRepo.Create(mappedAccount);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: AccountController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: AccountController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
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

		// GET: AccountController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: AccountController/Delete/5
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
