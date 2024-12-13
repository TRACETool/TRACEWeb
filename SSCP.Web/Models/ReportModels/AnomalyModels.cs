using MudBlazor;

namespace SSCP.Web.Models.ReportModels
{
    public class ComprehensiveAnomalyReport
    {
        public string RepositoryName { get; set; }
        public string RepositoryUrl { get; set; }
        public int TotalAnomalies { get; set; }
        public Dictionary<string, int> SeverityDistribution { get; set; }
        public List<ContributorAnomalies> Contributors { get; set; }
        public List<AnomalyCommitDetails> AnomalyCommits { get; set; }
    }

    public class AnomalyCommitDetails
    {
        public long AnomalyId { get; set; }
        public string ContributorName { get; set; }
        public string Severity { get; set; }
        public string CommitSha { get; set; }
        public string CommitMessage { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int TotalChanges { get; set; }
        public DateTime CommitDate { get; set; }
        public string Description { get; set; }
    }

    public class ContributorAnomalies
    {
        /// <summary>
        /// The name of the contributor.
        /// </summary>
        public string ContributorName { get; set; }

        /// <summary>
        /// The total number of anomalies linked to this contributor.
        /// </summary>
        public int AnomalyCount { get; set; }

        /// <summary>
        /// Constructor for initializing ContributorAnomalies.
        /// </summary>
        /// <param name="contributorName">The name of the contributor.</param>
        /// <param name="anomalyCount">The count of anomalies associated with the contributor.</param>

    }
}
