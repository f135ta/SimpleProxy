namespace SimpleProxy.UnitOfWork
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Example Data Model
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserService
    {
        private readonly DatabaseContext context;

        public UserService(DatabaseContext context)
        {
            this.context = context;
        }

        public void Add(string url)
        {
            var blog = new User { Name = "Robert"};
            this.context.Users.Add(blog);
            this.context.SaveChanges();
        }

        public IEnumerable<User> Find(string term)
        {
            return this.context.Users
                .Where(b => b.Name.Contains(term))
                .OrderBy(b => b.Id)
                .ToList();
        }
    }
}