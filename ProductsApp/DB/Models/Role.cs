﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProductsApp.DB.Models;

public partial class Role
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string ConcurrencyStamp { get; set; }

    public virtual ICollection<Roleclaim> Roleclaims { get; set; } = new List<Roleclaim>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}