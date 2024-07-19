using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AutomaticPurchaseHistory
{
    public long Id { get; set; }

    public long AutomaticPurchaseId { get; set; }

    public long UserExcution { get; set; }

    public int PurchaseStatus { get; set; }

    public int? DeliveryStatus { get; set; }

    public DateTime CreateDate { get; set; }

    public string PurchaseLog { get; set; }
}
