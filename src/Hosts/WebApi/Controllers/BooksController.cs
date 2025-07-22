using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAvailableBooks()
    {
        var books = await _bookService.GetAvailableBooksAsync();
        return Ok(books);
    }

    [HttpGet("author/{author}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthor(string author)
    {
        var books = await _bookService.GetBooksByAuthorAsync(author);
        return Ok(books);
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<ActionResult<BookDto>> GetBookByISBN(string isbn)
    {
        var book = await _bookService.GetBookByISBNAsync(isbn);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty");
        }

        var books = await _bookService.SearchBooksAsync(searchTerm);
        return Ok(books);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
    {
        var book = await _bookService.CreateBookAsync(createBookDto);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookDto>> UpdateBook(Guid id, UpdateBookDto updateBookDto)
    {
        var book = await _bookService.UpdateBookAsync(id, updateBookDto);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpHead("{id}")]
    public async Task<IActionResult> BookExists(Guid id)
    {
        var exists = await _bookService.BookExistsAsync(id);
        return exists ? Ok() : NotFound();
    }
}
