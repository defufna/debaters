using System.Security.Cryptography;
using System.Text;

namespace Debaters.Server.Utils;

internal class PasswordUtils
{
	private static readonly byte[] pepper = GetPepper();
	private const double RequiredEntropy = 50.0;
	private const int SaltSize = 16;
	private const string PepperEnvironmentVariable = "DEBATERS_PEPPER";
	private static void GenerateSalt(Span<byte> salt)
	{
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}
	}
	public static byte[] HashPassword(string password)
	{
		byte[] passBytes = Encoding.UTF8.GetBytes(password);
		byte[] result = new byte[SHA256.HashSizeInBytes + SaltSize];
		Span<byte> salt = new Span<byte>(result, SHA256.HashSizeInBytes, SaltSize);
		GenerateSalt(salt);

		HashPasswordInternal(passBytes, salt, new Span<byte>(result, 0, SHA256.HashSizeInBytes));
		return result;
	}

	public static bool CheckPassword(string password, byte[] hashed)
	{
		byte[] passBytes = Encoding.UTF8.GetBytes(password);
		ReadOnlySpan<byte> salt = new ReadOnlySpan<byte>(hashed, SHA256.HashSizeInBytes, SaltSize);

		Span<Byte> toCheck = new byte[SHA256.HashSizeInBytes];
		HashPasswordInternal(passBytes, salt, toCheck);

		return toCheck.SequenceEqual(new ReadOnlySpan<byte>(hashed, 0, SHA256.HashSizeInBytes));
	}

	public static bool CheckStrength(string password)
	{
		return CalculateEntropy(password) > RequiredEntropy;
	}

	public static double CalculateEntropy(string password)
	{
		int charsetSize = 0;

		bool hasLowerCase = false;
		bool hasUpperCase = false;
		bool hasNumber = false;
		bool hasSpecialChar = false;

		foreach (char c in password)
		{
			if (char.IsLower(c) && !hasLowerCase)
			{
				charsetSize += 26; // Add lowercase letters
				hasLowerCase = true;
			}
			else if (char.IsUpper(c) && !hasUpperCase)
			{
				charsetSize += 26; // Add uppercase letters
				hasUpperCase = true;
			}
			else if (char.IsDigit(c) && !hasNumber)
			{
				charsetSize += 10; // Add numbers
				hasNumber = true;
			}
			else if (!char.IsLetterOrDigit(c) && !hasSpecialChar)
			{
				charsetSize += 33; // Add special characters
				hasSpecialChar = true;
			}
		}

		double entropy = Math.Log(charsetSize, 2) * password.Length;
		return entropy;
	}

	private static void HashPasswordInternal(ReadOnlySpan<byte> password, ReadOnlySpan<byte> salt, Span<byte> result)
	{
		IncrementalHash sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

		sha256.AppendData(password);
		sha256.AppendData(salt);
		sha256.AppendData(pepper);

		sha256.GetHashAndReset(result);
	}

	private static byte[] GetPepper()
	{
		string? pepper = Environment.GetEnvironmentVariable(PepperEnvironmentVariable);
		if(pepper == null)
		{
			return Array.Empty<byte>();
		}

		return Encoding.UTF8.GetBytes(pepper);
	}

}
