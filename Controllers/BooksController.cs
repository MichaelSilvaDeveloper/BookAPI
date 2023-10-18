using BookAPI.Model;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }   

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var searchToBook = await _bookRepository.Get(id);
            if (searchToBook == null)
                return NotFound();
            return await _bookRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (id != book.Id)
                return BadRequest();
            await _bookRepository.Update(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);

            if(bookToDelete == null)
                return NotFound();
            await _bookRepository.Delete(bookToDelete.Id);
            return NoContent();
        }
    }
}