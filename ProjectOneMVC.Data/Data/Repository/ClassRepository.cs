using ProjectOneMVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ProjectOneMVC.Data.Data.Repository
{
    public class ClassRepository : Repository<Class, ApplicationDbContext>
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
