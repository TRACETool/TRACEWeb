namespace SSCP.Web.Models.ReportModels
{
    public class RepositoryInsightsDto
    {
        public string RepositoryName { get; set; }
        public int TotalActions { get; set; }
        public List<ActionCountDto> ActionBreakdown { get; set; }
        public List<ContributorStatsDto> TopContributors { get; set; }
        public List<BranchStatsDto> BranchActivity { get; set; }
        public List<FileImpactDto> TopFiles { get; set; }
        public List<CommitTrendDto> CommitTrends { get; set; }
        public List<AnomalyDto> Anomalies { get; set; }
    }

    public class ActionCountDto
    {
        public string ActionType { get; set; }
        public int Count { get; set; }
    }

    public class ContributorStatsDto
    {
        public string Author { get; set; }
        public int ActionCount { get; set; }
        public int LinesAdded { get; set; }
        public int LinesDeleted { get; set; }
    }

    public class BranchStatsDto
    {
        public string BranchName { get; set; }
        public int ActionCount { get; set; }
    }

    public class FileImpactDto
    {
        public string FileName { get; set; }
        public int ModificationCount { get; set; }
    }

    public class CommitTrendDto
    {
        public DateTime Date { get; set; }
        public int CommitCount { get; set; }
    }

    public class AnomalyDto
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
