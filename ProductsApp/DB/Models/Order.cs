﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProductsApp.DB.Models;

public partial class Order
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual User User { get; set; }
}