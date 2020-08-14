using ProjectOneMVC.Core.Entities;

namespace ProjectOneMVC.Data.Data.Repository
{
    public class ClassRepository : Repository<Class, ApplicationDbContext>
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
