﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectOneMVC.Core.Entities
{
    public class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
