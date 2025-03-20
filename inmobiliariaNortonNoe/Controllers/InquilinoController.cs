using Microsoft.AspNetCore.Mvc;
using System.Linq;

[Route("api/inquilino")]
[ApiController]
public class InquilinoController : ControllerBase
{
    private readonly AppDbContext _context;

    public InquilinoController(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todos los inquilinos
    [HttpGet]
    public IActionResult GetInquilinos()
    {
        return Ok(_context.Inquilinos.ToList());
    }

    // Obtener un inquilino por ID
    [HttpGet("{id}")]
    public IActionResult GetInquilino(int id)
    {
        var inquilino = _context.Inquilinos.Find(id);
        if (inquilino == null)
            return NotFound();
        return Ok(inquilino);
    }

    // Crear un nuevo inquilino
    [HttpPost]
    public IActionResult CreateInquilino([FromBody] Inquilino inquilino)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Inquilinos.Add(inquilino);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetInquilino), new { id = inquilino.Id }, inquilino);
    }

    // Modificar un inquilino
    [HttpPut("{id}")]
    public IActionResult UpdateInquilino(int id, [FromBody] Inquilino updatedInquilino)
    {
        var inquilino = _context.Inquilinos.Find(id);
        if (inquilino == null)
            return NotFound();

        inquilino.Dni = updatedInquilino.Dni;
        inquilino.Nombre = updatedInquilino.Nombre;
        inquilino.Apellido = updatedInquilino.Apellido;
        inquilino.Telefono = updatedInquilino.Telefono;
        inquilino.Email = updatedInquilino.Email;

        _context.SaveChanges();
        return NoContent();
    }

    // Eliminar un inquilino
    [HttpDelete("{id}")]
    public IActionResult DeleteInquilino(int id)
    {
        var inquilino = _context.Inquilinos.Find(id);
        if (inquilino == null)
            return NotFound();

        _context.Inquilinos.Remove(inquilino);
        _context.SaveChanges();
        return NoContent();
    }
}
