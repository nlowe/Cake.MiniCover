namespace Cake.MiniCover.Settings
{
    /// <summary>
    /// Settings for the <see cref="Cake.MiniCover.ReportType.COVERALLS"/> report type
    /// </summary>
    public class CoverallsSettings
    {
        /// <summary>
        /// The git root path
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// The service_job_id to send to coveralls
        /// </summary>
        public string ServiceJobId { get; set; }

        /// <summary>
        /// The service_name to send to coveralls
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The Repo Token to use when sending coverage to coveralls
        /// </summary>
        public string RepoToken { get; set; }

        /// <summary>
        /// The git commit hash to use when sending coverage to coveralls
        /// </summary>
        public string CommitHash { get; set; }

        /// <summary>
        /// The git commit message to send to coveralls
        /// </summary>
        public string CommitMessage { get; set; }

        /// <summary>
        /// The name of the commit author to send to coveralls
        /// </summary>
        public string CommitAuthorName { get; set; }

        /// <summary>
        /// The Email of the commit author to send to coveralls
        /// </summary>
        public string CommitAuthorEmail { get; set; }

        /// <summary>
        /// The name of the comitter to send to coveralls
        /// </summary>
        public string CommitterName { get; set; }

        /// <summary>
        /// The email of the comitter to send to coveralls
        /// </summary>
        public string CommitterEmail { get; set; }

        /// <summary>
        /// The git branch to send to coveralls
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// The name of the git remote to send to coveralls
        /// </summary>
        public string Remote { get; set; }

        /// <summary>
        /// The url of the git remote to send to coveralls
        /// </summary>
        public string RemoteUrl { get; set; }
    }
}