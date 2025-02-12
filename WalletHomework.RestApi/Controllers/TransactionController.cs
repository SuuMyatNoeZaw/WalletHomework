using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletHomework.Database.Models;

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
            
            var list = _db.TblTransactions.Where(x=>x.AccountID==id).ToList();
            if(list.Any())
            {
                return BadRequest("Use Account ID");
            }
            return Ok(list);
        }
        [HttpPost("{Deposit}")]
        public IActionResult Deposit(int id,decimal amount)
        {
            var item=_db.TblAccounts.AsNoTracking().FirstOrDefault(x=>x.AccountId==id);
            if (item is not null || amount > 0)
            {
                item.Balance += amount;
            }
            else return BadRequest("Something wrong");
            TblTransaction tblTransaction = new TblTransaction()
            {
                AccountID = item.AccountId,
                Amount = amount,
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction);
            _db.SaveChanges();

            return Ok(item);
        }
        [HttpPatch("{Withdraw}")]
        public IActionResult Withdraw(int id, decimal amount)
        {
            var item = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id);
            if (item is not null || amount > 0 || item.Balance >= amount)
            {
                item.Balance -= amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction = new TblTransaction()
            {
                AccountID = item.AccountId,
                Amount = amount,
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction);
            _db.SaveChanges();

            return Ok(item);
        }
        [HttpDelete("{Transfer}")]
        public IActionResult Transfer(int id1,int id2, decimal amount)
        {
            var item1 = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id1);
            var item2 = _db.TblAccounts.AsNoTracking().FirstOrDefault(x => x.AccountId == id2);
            if (item1 is not null || amount > 0 || item1.Balance >= amount)
            {
                item1.Balance -= amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction1 = new TblTransaction()
            {
                AccountID = item1.AccountId,
                Amount = amount,
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction1);
            _db.SaveChanges();
            if (item2 is not null || amount > 0 )
            {
                item2.Balance += amount;
            }
            else return BadRequest("Something Wrong");
            TblTransaction tblTransaction2 = new TblTransaction()
            {
                AccountID = item2.AccountId,
                Amount = amount,
                Date = DateTime.Now,

            };
            _db.TblTransactions.Add(tblTransaction2);
            _db.SaveChanges();

            return Ok(item1);
        }
    }
}
