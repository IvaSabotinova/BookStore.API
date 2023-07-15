using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext context;
        private readonly IMapper mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            // return await context.Books.Select(x => new BookModel
            // {
            //     Id = x.Id,
            //     Title = x.Title,
            //     Description = x.Description,
            // })
            //.ToListAsync();

            List<Book> books = await this.context.Books.ToListAsync();
            return this.mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            //return await context.Books.Where(x => x.Id == bookId).Select(x => new BookModel
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description,
            //})
            // .FirstOrDefaultAsync();
            Book book = await this.context.Books.FindAsync(bookId);
            return this.mapper.Map<BookModel>(book); ;

        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            Book newBook = new Book()
            {
                Title = bookModel.Title,
                Description = bookModel.Description,
            };
            await this.context.Books.AddAsync(newBook);
            await this.context.SaveChangesAsync();

            return newBook.Id;
        }

        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            //Book book = await this.context.Books.FindAsync(bookId);
            // if (book != null)
            // {
            //     book.Title = bookModel.Title;
            //     book.Description = bookModel.Description;
            // }
            // await this.context.SaveChangesAsync();

            Book newBook = new Book()
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description,
            };
            this.context.Books.Update(newBook);
            await this.context.SaveChangesAsync();

        }

        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            Book book = await this.context.Books.FindAsync(bookId);

            if (book != null)
            {
                bookModel.ApplyTo(book);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(int bookId)
        {
            Book book = await context.Books.FindAsync(bookId);

            if (book == null)
            {
                throw new Exception();
            }
            this.context.Books.Remove(book);
            await this.context.SaveChangesAsync();
            //Book book = new Book() { Id = bookId };
            //this.context.Books.Remove(book);
            //await this.context.SaveChangesAsync();
        }

    }
}
