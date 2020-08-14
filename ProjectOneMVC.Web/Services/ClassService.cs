using AutoMapper;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Data.Data.Repository;
using ProjectOneMVC.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneMVC.Web.Services
{
    public class ClassService
    {
        private readonly ClassRepository classRepo;
        private readonly UserClassRepository ucRepo;
        private readonly IMapper _mapper;

        public ClassService(ClassRepository classRepo,
            UserClassRepository ucRepo,
            IMapper mapper)
        {
            this.classRepo = classRepo;
            this.ucRepo = ucRepo;
            this._mapper = mapper;
        }


        /// <summary>
        /// Gets all the records in class repo, takes a return type.
        /// The type passed must be defined in the Automapper profiles: 
        /// From Class -> SomeClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of DB Class mapped to the type passed.</returns>
        public List<T> GetAll<T>() where T : class
        {
                var dbClasses = classRepo.GetAll().Result;
                return _mapper.Map<List<T>>(dbClasses);
        }


        /// <summary>
        /// Gets all the records in class repo, takes a return type.
        /// The type passed must be defined in the Automapper profiles: 
        /// From Class -> SomeClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of DB UserClass mapped to the type passed.</returns>
        public List<T> GetAllForUser<T>(string userId) where T : class
        {
            List<Class> dbClasses = new List<Class>();
            var userClasses = ucRepo.FindByCondition(uc => uc.SchoolUserId == userId).ToList();

            foreach (var userClass in userClasses)
            {
                var enrolledClass = classRepo.Get(userClass.ClassId).Result;

                dbClasses.Add(enrolledClass);
            }

            return _mapper.Map<List<T>>(dbClasses);
        }

        public async Task<ResultModel> AssignClassFor(string userId, List<EnrollViewModel> classes)
        {
            UserClass userClass = new UserClass();

            var results = new ResultModel() { Success = false};

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