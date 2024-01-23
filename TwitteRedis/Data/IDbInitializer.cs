namespace TwitteRedis.Data
{
    public interface IDbInitializer
    {
        public void Initialize(TwitteRedisDbContext context);
    }
}
