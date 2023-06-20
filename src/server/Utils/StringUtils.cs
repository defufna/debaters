using System.Text.RegularExpressions;
namespace Debaters.Server.Utils;

public static class StringUtils
{
	private static readonly Regex AlphanumericRegex = new Regex("^[a-zA-Z0-9]+$");
	public static bool IsAlphanumeric(this string s)
	{
		return AlphanumericRegex.IsMatch(s);
	}
}