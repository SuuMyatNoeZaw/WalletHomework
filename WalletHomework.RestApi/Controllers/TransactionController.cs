using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletHomework.Database.Models;
using WalletHomework.RestApi.Services;

namespace WalletHomework.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly AppDbContext _db=new AppDbContext();
        [HttpGet]
        public IActionResult Get()
        {
            var list=_db.TblTransactions.ToList();  
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            
            var list = _db.TblTransactions.Where(x=>x.AccountId==id).ToList();
            if(list is null)
            {
                return BadRequest("Use Account ID");
            }
            return Ok(list);
        }
        [HttpPost]
        public IActionResult Deposit(TblTransaction tran,string password)
        {
           WalletService walletService = new WalletService();
            decimal amount=Convert.ToDecimal(tran.Amount);
          int ans=  walletService.process(tran.AccountId, amount, tran.About, password);
            
            if(ans==1)
            {
                return Ok("You Deposit " + amount + " successfully");
            }
            return Ok();
        }
        [HttpPatch]
        public IActionResult Withdraw(int id, decimal amount, string password)
        {
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id && x.Password==password);
            if (item is not null && amount > 0 && item.Balance >= amount)
            {
                item.Balance -= amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction = new TblTransaction()
            {
                AccountId = item.AccountId,
                Amount = amount,
                About="Withdraw",
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction);
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok("You Withdraw "+ amount +" successfully");
        }
        [HttpDelete]
        public IActionResult Transfer(int id1,int id2, decimal amount, string password)
        {
            var item1 = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id1 && x.Password==password);
            var item2 = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id2);
            if (item1 is not null && amount > 0 && item1.Balance >= amount)
            {
                item1.Balance -= amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction1 = new TblTransaction()
            {
                AccountId = item1.AccountId,
                Amount = amount,
                About="Transfer",
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction1);
            _db.Entry(item1).State = EntityState.Modified;
            _db.SaveChanges();
            if (item2 is not null && amount > 0 )
            {
                item2.Balance += amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction2 = new TblTransaction()
            {
                AccountId = item2.AccountId,
                Amount = amount,
                About="Receive",
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction2);
            _db.Entry(item2).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok("Transfer " +amount + " from "+ item1.UserName+" to "+item2.UserName);
        }
    }
}
