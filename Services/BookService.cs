using BookShelf.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Services;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }
    public DbSet<Book> Books { get; set; }

    public List<Book> GetAll() => Books.ToList();

    public Book? Get(int id) => Books.FirstOrDefault(p => p.BookId == id);

    public void Update(Book book)
    {
        var existingBook = Books.FirstOrDefault(b => b.BookId == book.BookId);

        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.PublishDate = book.PublishDate;

            SaveChanges();
        }
        else
        {
            throw new ArgumentException($"El libro con ID {book.BookId} no existe en la base de datos.");
        }
    }

    public void Delete(int id)
    {
        var book = Books.Find(id);
        if (book == null)
        {
            throw new ArgumentException("Book not found", nameof(id));
        }

        Books.Remove(book);
        SaveChanges();
    }

}