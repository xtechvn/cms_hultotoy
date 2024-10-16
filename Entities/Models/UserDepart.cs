﻿using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserDepart
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? JoinDate { get; set; }

    public DateOnly? LeaveDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
