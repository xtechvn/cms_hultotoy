using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class OrderDetail
{
    public long OrderDetailId { get; set; }

    public long OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductCode { get; set; }

    public double? Amount { get; set; }

    public double? Price { get; set; }

    public double? Profit { get; set; }

    public int? UserCreate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UserUpdated { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
