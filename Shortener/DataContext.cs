global using Microsoft.EntityFrameworkCore;
namespace Shortener
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext>options): base(options)
        {

        }
        public DbSet<Url> Urls => Set<Url>();
    }
    
}
