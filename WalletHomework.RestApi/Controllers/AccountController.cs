using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;
using WalletHomework.Database.Models;

namespace WalletHomework.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
       public readonly AppDbContext _db=new AppDbContext();
        
        [HttpGet]
        public IActionResult GetAccount()
        {
            var list=_db.TblAccounts.AsNoTracking().ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetAccountByID(int id)
        {
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x=>x.AccountId==id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public IActionResult CreateAccount(TblAccount account)
        {
            _db.TblAccounts.Add(account);
            _db.SaveChanges();
           
            return Ok(account);
        }
        [HttpPut]
        public IActionResult UpdateAccount(int id,TblAccount account) 
        {
            var item=_db.TblAccounts.AsNoTracking().FirstOrDefault(x=>x.AccountId==id);
            if (item is null)
            {
                return NotFound();
            }
            item.UserName = account.UserName;
            item.Password = account.Password;
            item.TransactionId = account.TransactionId;
            item.Balance = account.Balance;
            item.PinNo = account.PinNo;
            _db.Entry(item).State=EntityState.Modified;
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpPatch]
        public IActionResult PatchAccount(int id, TblAccount account)
        {
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id);
            if (item is null)
            {
                return NotFound();
            }
            if(!string.IsNullOrEmpty(account.UserName))
            {
                item.UserName = account.UserName;
            }
            if (!string.IsNullOrEmpty(account.Password))
            {
                item.Password = account.Password;
            }
            if (!string.IsNullOrEmpty(account.TransactionId))
            {
                item.TransactionId = account.TransactionId;
            }
            if (account.Balance!=0)
            {
                item.Balance = account.Balance;
            }
            if (!string.IsNullOrEmpty(account.PinNo))
            {
                item.PinNo = account.PinNo;
            }
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id);
            if (item is null)
            {
                return NotFound();
            }
         var result = _db.TblAccounts.Remove(item);
            _db.Entry(item).State = EntityState.Deleted;
            _db.SaveChanges();
            return Ok(result);
        }
    }
}
