using ProjectOneMVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOneMVC.Data.Data.Repository
{
    public class UserClassRepository : Repository<UserClass, ApplicationDbContext>
    {
        public UserClassRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
