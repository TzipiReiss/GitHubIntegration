using Microsoft.Extensions.Caching.Memory;
using Octokit;
using GitHubIntegration;
using GitHubIntegration.DataEntities;

namespace GitHub.API.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        private const string userPortfolioKey = "userPortfolioKey";
        private const string publicRepositoriesKey = "publicRepositoriesKey";


        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        public async Task<Portfolio> GetUserPortfolio()
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out Portfolio portfolio))
                return portfolio;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                .SetSlidingExpiration(TimeSpan.FromSeconds(100));

            portfolio = await _gitHubService.GetUserPortfolio();

            _memoryCache.Set(userPortfolioKey, portfolio, cacheOptions);

            return portfolio;
        }

        public async Task<List<Repo>> SearchRepositories(string? repoName, string? programmingLanguage, string? userName)
        {
            if (_memoryCache.TryGetValue(publicRepositoriesKey, out List<Repo> repoList))
                return repoList;

            var cacheOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(3000))
               .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

            repoList = await _gitHubService.SearchRepositories(repoName, programmingLanguage, userName);

            _memoryCache.Set(publicRepositoriesKey, repoList, cacheOptions);

            return repoList;
        }
    }
}
