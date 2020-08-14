using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectOneMVC.Data;
using ProjectOneMVC.Web.ViewModels;

namespace ProjectOneMVC.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClassesController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        // GET: ClassesController
        public ActionResult Index()
        {
            var classes = _context.Classes.ToList();
            var classesToRetrun = _mapper.Map<List<ClassViewModel>>(classes);

            return View(classesToRetrun);
        }
    }
}
