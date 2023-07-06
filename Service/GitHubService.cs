using Microsoft.Extensions.Options;
using Octokit;
using GitHubIntegration.DataEntities;

namespace GitHubIntegration
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubIntegrationOptions _options;
        private readonly GitHubClient _client;

        public GitHubService(IOptions<GitHubIntegrationOptions> options)
        {
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"));
            _options = options.Value;
            _client.Credentials = new Credentials(_options.Token);
        }

        public async Task<List<Repo>> SearchRepositories(string? repoName, string? programmingLanguage, string? userName)
        {
            if (string.IsNullOrWhiteSpace(repoName))
                repoName = "*";
            var request = new SearchRepositoriesRequest(repoName) { };
            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                Language parsedLanguage;
                if (Enum.TryParse(programmingLanguage, out parsedLanguage))
                    request.Language = parsedLanguage;
            }
            if (!string.IsNullOrWhiteSpace(userName))
                request.User = userName;
            var repositories = await _client.Search.SearchRepo(request);

            var result = new List<Repo>();
            foreach (var repository in repositories.Items)
            {
                var repo = new Repo()
                {
                    Id = repository.Id,
                    Name = repository.Name,
                    UserName = repository.Owner.Login,
                    CretedAt = repository.CreatedAt,
                    Status = repository.Private ? "private" : "public",
                    LastCommitMessage = _client.Repository.Commit.GetAll(repository.Id).Result.First().Commit.Message,
                    LastCommitDateTime = _client.Repository.Commit.GetAll(repository.Id).Result.First().Commit.Committer.Date,
                    PullRequestCount = _client.Repository.PullRequest.GetAllForRepository(repository.Id).Result.Count,
                    StargazersCount = repository.StargazersCount,
                    Url = repository.HtmlUrl
                };
                foreach (var language in _client.Repository.GetAllLanguages(repository.Id).Result)
                    repo.Languages.Add(language.Name);
                result.Add(repo);
            }
            return result;
        }

        public async Task<Portfolio> GetUserPortfolio()
        {
            var portfolio = new Portfolio();
            var repositories = (await _client.Repository.GetAllForCurrent()).ToList();
            foreach (var repository in repositories)
            {
                var repo = new Repo()
                {
                    Id = repository.Id,
                    Name = repository.Name,
                    UserName = _options.UserName,
                    CretedAt = repository.CreatedAt,
                    Status = repository.Private ? "private" : "public",
                    LastCommitMessage = _client.Repository.Commit.GetAll(repository.Id).Result.First().Commit.Message,
                    LastCommitDateTime = _client.Repository.Commit.GetAll(repository.Id).Result.First().Commit.Committer.Date,
                    PullRequestCount = _client.Repository.PullRequest.GetAllForRepository(repository.Id).Result.Count,
                    StargazersCount = repository.StargazersCount,
                    Url = repository.HtmlUrl
                };
                foreach (var language in _client.Repository.GetAllLanguages(repository.Id).Result)
                    repo.Languages.Add(language.Name);
                portfolio.repositories.Add(repo);
            }
            return portfolio;
        }
    }
}