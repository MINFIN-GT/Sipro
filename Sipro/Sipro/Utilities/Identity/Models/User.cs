﻿using System;
using System.Collections.Generic;

namespace Sipro.Utilities.Identity.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; }
  }
}