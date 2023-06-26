using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RedAction.Models;

namespace RedAction.Controllers
{
    public class ArticuloController : Controller
    {
        private readonly RedActionDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ArticuloController(RedActionDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Articulo
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var redActionDBContext = await _context.Articulo.Include(a => a.autor).ToListAsync(); // HAGO UNA LISTA POR AUTOR
            var listaArticulos = redActionDBContext.Where(a => a.estado == EstadoArticulo.ESPERANDO_APROBACION || a.estado == EstadoArticulo.PUBLICADO).ToList();
                                                                                                                                                                                                                    
            return View(listaArticulos);
        }

        // GET: Articulo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articulo == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulo
                .Include(a => a.autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulo/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Usuario, "Id", "Dni");
            return View();
        }

        // POST: Articulo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AutorId,contenido,seccion,observaciones")] Articulo articulo)
        {
            //Acá selecciono el usuario que está registrado
            var user = await _userManager.GetUserAsync(User);

            //Acá valido que el user no sea null y que el Usuario tampoco
            if (user == null || _context.Usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            //Busco al Usuario a partir del mail
            var usuario = await _context.Usuario.Where(u => u.mail.ToUpper() == user.NormalizedEmail).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            var redActionDBContext = await _context.Articulo.Include(a => a.autor).ToListAsync(); // HAGO UNA LISTA POR AUTOR
            //Filtro por los artículos de autoría de este usuario pero que NO estén en ESPERANDO_APROBACION
            var listaArticulos = redActionDBContext.Where(a => a.AutorId == usuario.Id && a.estado != EstadoArticulo.ESPERANDO_APROBACION).ToList();

            if (ModelState.IsValid)
            {
                articulo.AutorId = usuario.Id;
                articulo.estado = EstadoArticulo.BORRADOR;
                _context.Add(articulo);
                await _context.SaveChangesAsync();

                return RedirectToAction("ArticulosPropios", listaArticulos);
            }
            ViewData["AutorId"] = new SelectList(_context.Usuario, "Id", "Dni", articulo.AutorId);

            return View(listaArticulos);
        }

        // GET: Articulo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articulo == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulo.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Usuario, "Id", "Dni", articulo.AutorId);
            return View(articulo);
        }

        // POST: Articulo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AutorId,contenido,seccion,estado,observaciones")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            //Acá selecciono el usuario que está registrado
            var user = await _userManager.GetUserAsync(User);

            //Acá valido que el user no sea null y que el Usuario tampoco
            if (user == null || _context.Usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            //Busco al Usuario a partir del mail
            var usuario = await _context.Usuario.Where(u => u.mail.ToUpper() == user.NormalizedEmail).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            var redActionDBContext = await _context.Articulo.Include(a => a.autor).ToListAsync(); // HAGO UNA LISTA POR AUTOR
            //Filtro por los artículos de autoría de este usuario pero que NO estén en ESPERANDO_APROBACION
            var listaArticulos = redActionDBContext.Where(a => a.AutorId == usuario.Id && a.estado != EstadoArticulo.ESPERANDO_APROBACION).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (usuario.tipo == TipoUsuario.REDACTOR)
                {
                    return RedirectToAction("ArticulosPropios", listaArticulos);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AutorId"] = new SelectList(_context.Usuario, "Id", "Dni", articulo.AutorId);
            if (usuario.tipo == TipoUsuario.REDACTOR)
            {
                return View(listaArticulos);
            }
            else
            {
                return View(articulo);
            }
        }

        // GET: Articulo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articulo == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulo
                .Include(a => a.autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articulo == null)
            {
                return Problem("Entity set 'RedActionDBContext.Articulo'  is null.");
            }
            var articulo = await _context.Articulo.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulo.Remove(articulo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "REDACTOR")]
        [HttpGet("Articulo/MisArticulos")]
        public async Task<IActionResult> ArticulosPropios()
        {
            //Acá selecciono el usuario que está registrado
            var user = await _userManager.GetUserAsync(User);

            //Acá valido que el user no sea null y que el Usuario tampoco
            if (user == null || _context.Usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            //Busco al Usuario a partir del mail
            var usuario = await _context.Usuario.Where(u => u.mail.ToUpper() == user.NormalizedEmail).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return RedirectToAction("MensajeError", "Home");
            }

            var redActionDBContext = await _context.Articulo.Include(a => a.autor).ToListAsync(); // HAGO UNA LISTA POR AUTOR
            //Filtro por los artículos de autoría de este usuario pero que NO estén en ESPERANDO_APROBACION
            var listaArticulos = redActionDBContext.Where(a => a.AutorId == usuario.Id && a.estado != EstadoArticulo.ESPERANDO_APROBACION).ToList();

            return View("Index",listaArticulos);

        }

        private bool ArticuloExists(int id)
        {
          return (_context.Articulo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
