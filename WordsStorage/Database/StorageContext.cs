using Microsoft.EntityFrameworkCore;
using WordsStorage.Models;

namespace WordsStorage.Database;

public class StorageContext : DbContext 
{
    public StorageContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Word> Words { get; set; }
    public DbSet<Topic> Topics { get; set; }

}
