using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;
using WebAccountantApp.Models;

namespace WebAccountantApp.Mapping
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Account, AccountVM>().ReverseMap();
            CreateMap<Transaction, TransactionVM>().ReverseMap();
            CreateMap<BalanceReport, BalanceReportVM>().ReverseMap();
        }
    }
}
