using Microsoft.EntityFrameworkCore;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Data.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneMVC.Web.Services
{
    public class ClassService
    {
        private readonly ClassRepository classRepo;
        private readonly UserClassRepository ucRepo;

        public ClassService(ClassRepository classRepo,
            UserClassRepository ucRepo)
        {
            this.classRepo = classRepo;
            this.ucRepo = ucRepo;
        }

        public async Task<List<Class>> GetAll()
        {
            return await Task.Run(() =>
            {
                return classRepo.GetAll();
            });
        }

        public List<Class> GetAllForUser(string userId)
        {
            var userClass = ucRepo.FindByCondition(uc => uc.SchoolUserId == userId).SingleOrDefault();

            var dbClasses = classRepo.FindByCondition(c => c.Id == userClass.ClassId).ToList();

            return dbClasses;
        }
    }
}
