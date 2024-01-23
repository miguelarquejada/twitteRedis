using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using TwitteRedis.Data;
using TwitteRedis.Models;

namespace TwitteRedis.Pages;

public class IndexModel : PageModel
{
    private readonly IDatabase _redisDatabase;
    private readonly TwitteRedisDbContext _context;

    public List<Tweet> Tweets { get; private set; }

    [BindProperty]
    public Tweet? Tweet { get; set; }

    public IndexModel(TwitteRedisDbContext context)
    {
        _context = context;
        var redis = ConnectionMultiplexer.Connect("localhost");
        _redisDatabase = redis.GetDatabase();
    }

    public void OnGet(int? display)
    {
        ViewData["Title"] = "Home";
        ViewData["Display"] = display ?? 0;
        
        if (display == 0 || display == null)
        {
            var tweetIds = _redisDatabase.ListRange("user:1:recent_tweets").Select(x => Guid.Parse(x));
            Tweets = _context.Tweets.Where(x => tweetIds.Contains(x.Id)).OrderByDescending(x => x.CreatedOnDate).ToList();
            return;
        }

        Tweets = _context.Tweets.OrderByDescending(x => x.CreatedOnDate).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return RedirectToPage("./Index");
        }

        if (Tweet != null)
        {
            Tweet.CreatedOnDate = DateTime.Now;
            _context.Tweets.Add(Tweet);
            await _context.SaveChangesAsync();

            _redisDatabase.ListLeftPush("user:1:recent_tweets", Tweet.Id.ToString());
            _redisDatabase.ListTrim("user:1:recent_tweets", 0, 4);
        }

        return RedirectToPage("./Index");
    }
}