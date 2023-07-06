using Octokit;
using GitHubIntegration.DataEntities;

namespace GitHubIntegration
{
    public interface IGitHubService
    {
        Task<Portfolio> GetUserPortfolio();
        Task<List<Repo>> SearchRepositories(string? repoName, string? programmingLanguage, string? userName);
    }
}