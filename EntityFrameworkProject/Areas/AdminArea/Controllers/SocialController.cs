using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SocialController : Controller
    {
        private readonly AppDbContext _context;

        public SocialController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Social> socials = await _context.Socials.Where(m => !m.IsDeleted).AsNoTracking().OrderByDescending(m => m.Id).ToListAsync();
            return View(socials);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Social social = await _context.Socials.FindAsync(id);
            return View(social);        
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social social)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }


                bool isExist = await _context.Socials.AnyAsync(m => m.Name.Trim() == social.Name.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("Name", "Social already exist");
                    return View();
                }
               

                await _context.Socials.AddAsync(social);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Social social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);

                if (social is null) return NotFound();

                return View(social);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Social social)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(social);
                }

                Social dbSocial = await _context.Socials.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbSocial is null) return NotFound();

                if (dbSocial.Name.ToLower().Trim() == social.Name.ToLower().Trim() && dbSocial.Url.ToLower().Trim() == social.Url.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                _context.Socials.Update(social);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
