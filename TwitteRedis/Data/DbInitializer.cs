using StackExchange.Redis;
using TwitteRedis.Models;

namespace TwitteRedis.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(TwitteRedisDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Tweets.Any())
            {
                var tweets = new Tweet[]
                {
                    new Tweet("Olá TwitteRedis!", DateTime.Now),
                    new Tweet("Fui promovido a coronel!", DateTime.Now.AddDays(-1)),
                    new Tweet("Bora pra Califórnia fazer uma baguncinha?", DateTime.Now.AddDays(-2)),
                    new Tweet("Hoje eu vou tunar meu camaro amarelo", DateTime.Now.AddDays(-3)),
                    new Tweet("Esse é um teste muito loko", DateTime.Now.AddDays(-4)),
                    new Tweet("Hakuna Matata, é lindo dizer!", DateTime.Now.AddDays(-5))
                };

                context.Tweets.AddRange(tweets);
                context.SaveChanges();

                var redis = ConnectionMultiplexer.Connect("localhost");
                var db = redis.GetDatabase();

                foreach (var tweetId in tweets.OrderByDescending(x => x.CreatedOnDate).Select(x => x.Id))
                    db.ListLeftPush("user:1:recent_tweets", tweetId.ToString());

                db.ListTrim("user:1:recent_tweets", 0, 4);
            }
        }
    }
}
