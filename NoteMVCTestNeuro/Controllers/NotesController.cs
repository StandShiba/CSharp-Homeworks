using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteMVCTestNeuro.Data;
using NoteMVCTestNeuro.Models;

namespace NoteMVCTestNeuro.Controllers
{
    public class NotesController : Controller
    {
        private readonly AppDbContext _db;

        public NotesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var notes = await _db.Notes
                .OrderByDescending(n => n.UpdatedUtc)
                .ToListAsync();

            return View(notes);
        }

        public IActionResult Create()
        {
            return View(new Note());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Note note)
        {
            if (!ModelState.IsValid)
                return View(note);

            note.CreatedUtc = DateTime.UtcNow;
            note.UpdatedUtc = DateTime.UtcNow;

            _db.Notes.Add(note);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var note = await _db.Notes.FindAsync(id);
            if (note == null) return NotFound();

            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Note input)
        {
            if (id != input.Id) return BadRequest();
            if (!ModelState.IsValid) return View(input);

            var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return NotFound();

            note.Title = input.Title;
            note.Content = input.Content;
            note.UpdatedUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}