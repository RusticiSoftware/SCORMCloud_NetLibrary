using System.Text.RegularExpressions;

namespace RusticiSoftware.HostedEngine.Client
{
	/// <summary>
	/// Provides utility functions.
	/// </summary>
	public class ScormEngineUtilities
	{
		/// <summary>
		/// Helper function that takes an organization/company name, application name, and application version string
		/// and returns the canonicalized form that can be used as the origin parameter in Cloud API requests.
		/// </summary>
		/// <param name="organization">The name of the organization/company that created the software using the 
		/// .NET Cloud library.</param>
		/// <param name="applicationName">The name of the application.</param>
		/// <param name="applicationVersion">The application version string.</param>
		/// <returns></returns>
		public static string GetCanonicalOriginString(string organization, string applicationName, string applicationVersion)
		{
			Regex nameRegex = new Regex("[^a-z0-9]");
			Regex versionRegex = new Regex("[^\\w\\.\\-]");
			string organizationComponent = nameRegex.Replace(organization.ToLower(), string.Empty);
			string applicationComponent = nameRegex.Replace(applicationName.ToLower(), string.Empty);
			string versionComponent = versionRegex.Replace(applicationVersion.ToLower(), string.Empty);

			return string.Format("{0}.{1}.{2}", organizationComponent, applicationComponent, versionComponent);
		}
	}
}