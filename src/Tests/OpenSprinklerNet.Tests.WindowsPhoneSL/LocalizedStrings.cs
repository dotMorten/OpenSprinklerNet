using OpenSprinklerNet.Tests.WindowsPhoneSL.Resources;

namespace OpenSprinklerNet.Tests.WindowsPhoneSL
{
	/// <summary>
	/// Provides access to string resources.
	/// </summary>
	public class LocalizedStrings
	{
		private static AppResources _localizedResources = new AppResources();

		public AppResources LocalizedResources { get { return _localizedResources; } }
	}
}