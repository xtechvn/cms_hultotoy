using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ProductOtherLink
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string LinkOrder { get; set; }

    public string Note { get; set; }

    /// <summary>
    /// Là giá checkout khi khách đã chuyển tiền và Sale bắt đầu tạo đơn
    /// </summary>
    public double? PriceCheckout { get; set; }

    /// <summary>
    /// Khi sửa giá do biến động giá khi đầu Mỹ tiến hành mua sau khi đã tạo đơn. Thí sẽ lưu giá sửa đó vào trường này
    /// </summary>
    public double? PriceDyamic { get; set; }

    public int? LabelId { get; set; }

    public string ProductOrderId { get; set; }
}
