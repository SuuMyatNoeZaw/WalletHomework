using System;
using System.Collections.Generic;

namespace WalletHomework.Database.Models;

public partial class TblAccount
{
    public int AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string PinNo { get; set; } = null!;

    public decimal? Balance { get; set; }

    public string TransactionId { get; set; } = null!;
}
