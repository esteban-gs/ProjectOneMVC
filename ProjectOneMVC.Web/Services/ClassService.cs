using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Data.Data.Repository;
using ProjectOneMVC.Web.ViewModels;
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
            List<Class> dbClasses = new List<Class>();
            var userClasses = ucRepo.FindByCondition(uc => uc.SchoolUserId == userId).ToList();

            foreach (var userClass in userClasses)
            {
                var enrolledClass = classRepo.Get(userClass.ClassId).Result;

                dbClasses.Add(enrolledClass);
            }

            return dbClasses;
        }

        public async Task<ResultModel> AssignClassFor(string userId, List<EnrollViewModel> classes)
        {
            UserClass userClass = new UserClass();

            var results = new ResultModel();
            results.Success = false;

            var recordsUpdated = 0;

            foreach (var item in classes)
            {
                var ucExistsinDb = ucRepo.Exists(userId, item.Id).Result;
                if (item.Selected && !ucExistsinDb)
                {
                    userClass.ClassId = item.Id;
                    userClass.SchoolUserId = userId;
                    await ucRepo.Add(userClass);
                    recordsUpdated++;
                }
            }

            results.Success = recordsUpdated > 0;
            results.RecordsUpdated = recordsUpdated;

            return results;
        }


    }
}

public struct ResultModel
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int RecordsUpdated { get; set; }
}