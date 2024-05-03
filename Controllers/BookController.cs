using BookShelf.Models;
using BookShelf.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly BookDbContext _bookService;

    public BookController(BookDbContext bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<List<Book>> GetAll() =>
        _bookService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = _bookService.Get(id);

        if (book == null)
            return NotFound();

        return book;
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Book updatedBook)
    {
        if (id != updatedBook.BookId)
        {
            return BadRequest("El ID del libro en la ruta no coincide con el ID del libro recibido en el cuerpo de la solicitud.");
        }

        try
        {
            _bookService.Update(updatedBook);
            return Ok("Libro actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al actualizar el libro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        book.PublishDate = DateTime.Now;

        _bookService.Books.Add(book);
        await _bookService.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _bookService.Delete(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}