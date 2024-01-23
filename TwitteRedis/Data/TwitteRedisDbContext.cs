using Microsoft.EntityFrameworkCore;
using TwitteRedis.Models;

namespace TwitteRedis.Data
{
    public class TwitteRedisDbContext : DbContext
    {
        public TwitteRedisDbContext(DbContextOptions<TwitteRedisDbContext> options) : base(options)
        {
        }

        public DbSet<Tweet> Tweets { get; set; }
    }
}
