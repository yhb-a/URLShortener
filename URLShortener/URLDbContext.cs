using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

/// <summary>
/// AppDbContext is the bridge between your application and the PostgreSQL database.
/// It manages the connection, tracks changes to your data, and allows you to query and save data.
/// </summary>
public class URLDbContext : DbContext
{
    /// <summary>
    /// This property represents a table in the database.
    /// Each DbSet corresponds to a table, and each UrlMapping object corresponds to a row in that table.
    /// EF Core uses this to perform CRUD (Create, Read, Update, Delete) operations.
    /// </summary>
    public DbSet<Url> URLs { get; set; }

    /// <summary>
    /// This is the constructor for AppDbContext.
    /// It receives configuration options (like connection strings) from the application and passes them to the base DbContext.
    /// These options tell EF Core how to connect to the database.
    /// </summary>
    /// <param name="options">Configuration options for the DbContext</param>
    public URLDbContext(DbContextOptions<URLDbContext> options)
        : base(options)
    {
        // The base constructor handles the options for us.
    }

    /// <summary>
    /// This method configures how the DbContext connects to the database.
    /// Here, we are telling EF Core to use PostgreSQL and providing the connection string.
    /// You only need this method if you aren't using dependency injection to provide options.
    /// </summary>
    /// <param name="optionsBuilder">Builder used to configure database options</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connect to PostgreSQL using Npgsql provider
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=local;Username=admin;Password=admin;");
    }

    /// <summary>
    /// This method is used to customize the model (database schema) that EF Core generates.
    /// Here, we are specifying that all tables created by this DbContext should be in the "url" schema instead of the default "public" schema.
    /// </summary>
    /// <param name="modelBuilder">Builder used to configure entities and their mappings to database tables</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Database → Like a filing cabinet.
        // Schema → Like a folder inside that cabinet.
        // Table → A file inside the folder.

        // Set the default schema for all tables in this context
        modelBuilder.HasDefaultSchema("url");

        // Call the base method to ensure any default behavior is applied
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Url>().HasIndex(entity => entity.ShortCode);
    }
}
