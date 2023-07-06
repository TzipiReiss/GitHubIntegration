using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace GitHubIntegration.DataEntities
{
    public class Portfolio
    {
        public List<Repo> repositories { get; set; } = new List<Repo>();
    }
}
