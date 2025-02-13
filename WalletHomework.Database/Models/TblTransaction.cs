using System;
using System.Collections.Generic;

namespace WalletHomework.Database.Models;

public partial class TblTransaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public decimal? Amount { get; set; }

    public string? About { get; set; }

    public DateTime Date { get; set; }
}
