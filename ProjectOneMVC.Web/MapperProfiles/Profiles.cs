using AutoMapper;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
