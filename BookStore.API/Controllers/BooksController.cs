using BookStore.API.Models;
using BookStore.API.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            List<BookModel> books = await bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            BookModel book = await bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost("")]

        public async Task<IActionResult> AddNewBook([FromBody] BookModel bookModel)
        {
            int bookId = await this.bookRepository.AddBookAsync(bookModel);

            return CreatedAtAction(nameof(GetBookById), new { id = bookId, controller = "books"}, bookId);
        }

        [HttpPut("{bookId}")]

        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] BookModel bookModel)
        {
            await this.bookRepository.UpdateBookAsync(bookId, bookModel);
            return Ok();
        }

        [HttpPatch("{bookId}")]
        public async Task<IActionResult> UpdateBookPatch([FromRoute] int bookId, [FromBody] JsonPatchDocument bookModel)
        {
            await this.bookRepository.UpdateBookPatchAsync(bookId, bookModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {


            await this.bookRepository.DeleteBookAsync(id);
            return this.Ok();
        }

    }
}
