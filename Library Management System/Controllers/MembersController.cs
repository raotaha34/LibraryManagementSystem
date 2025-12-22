using Library_Management_System.Models;
using Library_Management_System.Services;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            return View(await _memberService.GetAllPaginatedAsync(page, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        { 
            var isAdded = await _memberService.AddAsync(member);
            if(isAdded == false)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "This member had already been registered.");
                    return View(member);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Member member)
        {
            await _memberService.UpdateAsync(member);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _memberService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
