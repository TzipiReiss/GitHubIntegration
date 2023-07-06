using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration.DataEntities
{
    public class Repo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset CretedAt { get; set; }
        public string Status { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public string LastCommitMessage { get; set; }
        public DateTimeOffset LastCommitDateTime { get; set; }
        public int PullRequestCount { get; set; }
        public int StargazersCount { get; set; }
        public string Url { get; set; }
    }
}
