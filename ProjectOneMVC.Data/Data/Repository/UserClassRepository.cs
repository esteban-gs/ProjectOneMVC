using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectOneMVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOneMVC.Data.Data.Repository
{
    public class UserClassRepository : Repository<UserClass, ApplicationDbContext>
    {
        private readonly ApplicationDbContext _context;

        public UserClassRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<bool> Exists(string userId, int classId)
        {
            var userClass = await _context
                                .UserClasses
                                .FirstOrDefaultAsync(uc => uc.ClassId == classId &&
                                            uc.SchoolUserId == userId);
            if (userClass != null)
                return true;

            return false;
        }
    }
}
