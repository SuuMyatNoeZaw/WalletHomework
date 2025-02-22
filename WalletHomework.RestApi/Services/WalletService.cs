using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel;
using WalletHomework.Database.Models;

namespace WalletHomework.RestApi.Services
{
    public class WalletService
    {
        AppDbContext _db=new AppDbContext();
        public int process(int id,decimal amount,string password,string about)
        {
            int result;
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id && x.Password == password);
            if (item is not null && amount > 0)
            {
                if (about == "Deposit")
                {
                    item.Balance += amount;
                }
                else if(about == "WithDraw")
                {
                    item.Balance -= amount;
                }
                TblTransaction tblTransaction = new TblTransaction()
                {
                    AccountId = item.AccountId,
                    Amount = amount,
                    About = about,
                    Date = DateTime.Now,

                };
                _db.TblTransactions.Add(tblTransaction);
                _db.Entry(item).State = EntityState.Modified;
              result = _db.SaveChanges();
            }
            else throw new NotImplementedException();
           
           
          
            return result;
        }

        
    }
}
