using AutoMapper;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Web.ViewModels;

namespace ProjectOneMVC.Web.MapperProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            //Classes List
            CreateMap<Class, ClassViewModel>();
            CreateMap<Class, EnrollViewModel>();
        }
    }
}
