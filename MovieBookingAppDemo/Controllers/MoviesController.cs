using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieBookingAppDemo.Data;
using MovieBookingAppDemo.Models;
using MovieBookingAppDemo.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieBookingAppDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DataContext _context;
        private readonly BlobService _blobService;

        //constructor 
        public MoviesController(DataContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile imageFile)
        {
            //checks if user selected a file
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image file.");
            }

            //only continues if theres no validation error 
            if (ModelState.IsValid)
            {
                try
                {
                    // uploads file (image) to local blob storage
                    movie.ImageUrl = await _blobService.UploadFileAsync(imageFile, "movie-images");

                    //checks if file upload failed 
                    if (string.IsNullOrEmpty(movie.ImageUrl))
                    {
                        ModelState.AddModelError("", "Image upload failed. No URL was returned.");
                        return View(movie);  //shows error on the same page 
                    }

                    //saves entry to the database (including the image url)
                    _context.Add(movie);
                    await _context.SaveChangesAsync();

                    //redirect to index page after successful 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // If something goes wrong when uploading, show the error message
                    ModelState.AddModelError("", "Upload failed: " + ex.Message);
                }
            }
            return View(movie);
            // If validation fails, return the same view with entered data and errors

        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Genre,ImageUrl")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //searches the DB 
            var movie = await _context.Movies
        .Include(m => m.TicketBookings)
        .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            // PREVENT DELETION IF BOOKINGS EXIST
            if (movie.TicketBookings.Any())
            {
                TempData["Error"] = "Cannot delete movie with active bookings.";
                return View(movie);
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }

    }
}
