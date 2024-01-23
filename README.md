
# TwitteRedis - Redis + Razor Page

Projeto simples que visa simular o uso do redis no Twitter para listagem de tweets mais recentes.

A ideia é que para cada usuário (nessa aplicação temos apenas um usuário) haverá uma lista ids de tweets recentes a serem exibidos em sua timeline. No redis usaremos a estrutura List para fazer esse armazenamento.

Por exemplo:

Key: **user:1:recent_tweets**
| Value                |
| :------------------------- |
| 544e47c3-7fd7-4c3f-affe-484c05f69c66 |
| 73b67ee3-041b-449e-ac4d-b8f79a18fb72 |
| 47e2bd7b-0db7-4408-8aac-2ce84a7f846e |

Cada vez que um novo tweet for adicionado será inserido no começo dessa lista. E o tamanho máximo dela será 5.

O usuário poderá buscar pelos tweets mais recentes ou por todos os tweets.

Caso a busca seja pelos mais recentes, primeiramente é feita uma busca no redis pelos ids dos tweets e logo depois realizada uma pesquisa no banco.

```bash
var tweetIds = _redisDatabase.ListRange("user:1:recent_tweets").Select(x => Guid.Parse(x));
Tweets = _context.Tweets.Where(x => tweetIds.Contains(x.Id)).OrderByDescending(x => x.CreatedOnDate).ToList();
```



![Animation](https://github.com/miguelarquejada/twitteRedis/blob/master/gif-animation.gif?raw=true)


## Tecnologias

- AspNet Core 7
- Razor Pages
- RedisStack
- Banco em memória


## Como rodar

Rode o container do Redis Stack, que contém o Redis Server e RedisInsight

```bash
  docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
```

Na pasta do projeto executar:

```bash
  dotnet run --project TwitteRedis
```

Aplicação disponível em:

```bash
  http://localhost:5201/
```
