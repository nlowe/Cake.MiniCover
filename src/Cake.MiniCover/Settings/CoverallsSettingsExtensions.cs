using System;

namespace Cake.MiniCover.Settings
{
    /// <summary>
    /// Extensions for <see cref="Cake.MiniCover.Settings.CoverallsSettings"/>
    /// </summary>
    public static class CoverallsSettingsExtensions
    {
        /// <summary>
        /// Set the root path to the git repo
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="rootPath">the path to the git repo</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithRootPath(this CoverallsSettings settings, string rootPath)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RootPath = rootPath;

            return settings;
        }
        
        /// <summary>
        /// Set the service_job_id to send to coveralls
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="serviceJobId">the service_job_id to send to coveralls</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithServiceJobId(this CoverallsSettings settings, string serviceJobId)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ServiceJobId = serviceJobId;

            return settings;
        }

        /// <summary>
        /// Set the service_name to send to coveralls
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="serviceName">the service_name to send to coveralls</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithServiceName(this CoverallsSettings settings, string serviceName)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ServiceName = serviceName;

            return settings;
        }

        /// <summary>
        /// Set the Repo Token to use when sending coverage to coveralls
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="repoToken">the Repo Token to use when sending coverage to coveralls</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithRepoToken(this CoverallsSettings settings, string repoToken)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RepoToken = repoToken;

            return settings;
        }

        /// <summary>
        /// Set the git commit message to use when sending coverage to coveralls
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="hash">the git commit hash to use when sending coverage to coverallss</param>
        /// 
        /// <param name="message">the git commit message to use when sending coverage to coverallss</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithCommit(this CoverallsSettings settings, string hash, string message)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.CommitHash = hash;
            settings.CommitMessage = message;

            return settings;
        }

        /// <summary>
        /// Set the commit author details
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="name">The name of the commit author</param>
        /// <param name="email">The email address of the commit author</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithCommitAuthor(this CoverallsSettings settings, string name, string email)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.CommitAuthorName = name;
            settings.CommitAuthorEmail = email;

            return settings;
        }

        /// <summary>
        /// Set the comitter etails
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="name">The name of the committer</param>
        /// <param name="email">The email address of the committer</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithCommitter(this CoverallsSettings settings, string name, string email)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.CommitterName = name;
            settings.CommitterEmail = email;

            return settings;
        }

        /// <summary>
        /// Set the git branch to send to coveralls
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="branch">The git branch to send to coveralls</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithBranch(this CoverallsSettings settings, string branch)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Branch = branch;

            return settings;
        }

        /// <summary>
        /// Set git remote details
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="name">The name of the git remote</param>
        /// <param name="url">The URL of the git remote</param>
        /// <returns>The <see cref="CoverallsSettingsExtensions"/> instance so that multiple calls can be chained</returns>
        public static CoverallsSettings WithRemote(this CoverallsSettings settings, string name, string url)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Remote = name;
            settings.RemoteUrl = url;

            return settings;
        }

        /// <summary>
        /// Populate the Coveralls Settings with information for publishing coverage from Travis CI
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="isTravisPro"></param>
        public static CoverallsSettings UseTravisDefaults(this CoverallsSettings settings, bool isTravisPro = false)
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") != "true")
            {
                throw new InvalidOperationException("Not running on travis-ci");
            }

            settings.WithServiceName(isTravisPro ? "travis-pro" : "travvis-ci");
            settings.WithServiceJobId(Environment.GetEnvironmentVariable("TRAVIS_JOB_ID"));

            return settings;
        }
    }
}