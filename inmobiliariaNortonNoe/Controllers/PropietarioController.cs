using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/propietario")]
[ApiController]
public class PropietarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public PropietarioController(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todos los propietarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios()
    {
        return await _context.Propietarios.ToListAsync();
    }

    // Obtener un propietario por Id
    [HttpGet("{Id}")]
    public async Task<ActionResult<Propietario>> GetPropietario(int Id)
    {
        var propietario = await _context.Propietarios.FindAsync(Id);

        if (propietario == null)
        {
            return NotFound();
        }

        return propietario;
    }

    // Crear un propietario
    [HttpPost]
    public async Task<ActionResult<Propietario>> PostPropietario(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPropietario), new { Id = propietario.Id }, propietario);
    }

    // Modificar un propietario
    [HttpPut("{Id}")]
    public async Task<IActionResult> PutPropietario(int Id, Propietario propietario)
    {
        if (Id != propietario.Id)
        {
            return BadRequest();
        }

        _context.Entry(propietario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Eliminar un propietario
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeletePropietario(int Id)
    {
        var propietario = await _context.Propietarios.FindAsync(Id);
        if (propietario == null)
        {
            return NotFound();
        }

        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
