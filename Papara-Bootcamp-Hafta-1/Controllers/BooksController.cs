using Papara_Bootcamp_Hafta_1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;




namespace Papara_Bootcamp_Hafta_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private List<Book> books = new List<Book>
        {
            new Book { Id = 1, Name = "Suc ve Ceza", Author = "Fyodor Dostoyevski", PageCount = 600, Year = 1866 },
            new Book { Id = 2, Name = "Seker Portakalı", Author = "Jose Mauro de Vasconcelos", PageCount = 600, Year = 1968 },

        };

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<Book>>> GetBooks()
        {
            return Ok(new ApiResponse<IEnumerable<Book>>
            {
                Success= true,
                Message="Kitaplar başarılı bir şekilde getirildi.",
                Data= books,
                StatusCode = 200
            });
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<Book>> GetBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode=404
                });
            }
            return Ok(new ApiResponse<Book>
            {
                Success=true,
                Message="Kitap başarılı bir şekilde getirildi.",
                Data=book,
                StatusCode = 200
            });
        }

        [HttpPost]
        public ActionResult<ApiResponse<Book>> PostBook([FromBody] Book book)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Doğrulama hatası.",
                    Data=errors,
                    StatusCode = 400
                });
            }
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, new ApiResponse<Book>
            {
                Success=true,
                Message="Kitap başarılı bir şekilde eklendi.",
                Data=book,
                StatusCode = 201
            });
        }

        [HttpPut("{id}")]
        public IActionResult PutBook(int id,[FromBody] Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success=false,
                    Message="Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            existingBook.Name = book.Name;
            existingBook.Author = book.Author;
            existingBook.PageCount = book.PageCount;
            existingBook.Year = book.Year;

            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde güncellendi.",
                Data = existingBook,
                StatusCode = 200
            });
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBook(int id, [FromBody] Book updatedFields)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            if (!string.IsNullOrEmpty(updatedFields.Name))
            {
                existingBook.Name = updatedFields.Name;
            }
            if (!string.IsNullOrEmpty(updatedFields.Author))
            {
                existingBook.Author = updatedFields.Author;
            }
            if (updatedFields.PageCount != 0)
            {
                existingBook.PageCount = updatedFields.PageCount;
            }
            if (updatedFields.Year != 0)
            {
                existingBook.Year = updatedFields.Year;
            }

            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde güncellendi.",
                Data = existingBook,
                StatusCode = 200
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            books.Remove(book);
            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde silindi.",
                Data = book,
                StatusCode = 200
            });
        }

        [HttpGet("list")]
        public ActionResult<ApiResponse<IEnumerable<Book>>> ListBooks([FromQuery] string name) // name parametresi alarak listeleme
        {
            var filteredBooks = books.AsQueryable();

            if (string.IsNullOrEmpty(name))
            {

                return BadRequest(new ApiResponse<IEnumerable<Book>>
                {
                    Success = false,
                    Message = "Geçersiz parametre.",
                    StatusCode = 400
                });

            }
            else
            {
                filteredBooks = filteredBooks.Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                filteredBooks = filteredBooks.OrderBy(b => b.Id); // id'ye göre sıralama işlemi

                return Ok(new ApiResponse<IEnumerable<Book>>
                {
                    Success = true,
                    Message = "Kitap başarılı bir şekilde getirildi.",
                    Data = filteredBooks.ToList(),
                    StatusCode = 200
                });
            }
            
        }
    }
}
