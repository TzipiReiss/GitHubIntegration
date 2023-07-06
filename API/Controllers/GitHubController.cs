using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using GitHubIntegration;
using GitHubIntegration.DataEntities;

namespace GitHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _githubService;
        private readonly IConfiguration _configuration;
        public GitHubController(IGitHubService gitHubService, IConfiguration configuration)
        {
            _githubService = gitHubService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<Portfolio> GetPortfolio()
        {
            return await _githubService.GetUserPortfolio();
        }

        [HttpGet("repositories")]
        public async Task<List<Repo>> SearchRepositories(string? repoName, string? programmingLanguage, string? userName)
        {
            return await _githubService.SearchRepositories(repoName,programmingLanguage,userName);
        }
    }
}
