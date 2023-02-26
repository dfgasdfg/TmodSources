using System;

namespace ModTools.ModLoader;

public static class BuildInfo
{
	public enum BuildPurpose
	{
		Dev, // Personal Builds
		Preview, // Monthly preview builds from CI that modders develop against for compatibility
		Stable, // The 'stable' builds from CI that players are expected to play on.
	}

	public static readonly string BuildIdentifier = Config.BuildIdentifier;
	public static readonly Version tMLVersion;
	public static readonly Version StableVersion;
	public static readonly BuildPurpose Purpose;
	public static readonly string BranchName;
	public static readonly string CommitSHA;

	/// <summary>
	/// local time, for display purposes.
	/// </summary>
	public static readonly DateTime BuildDate;

	public static bool IsStable => Purpose == BuildPurpose.Stable;

	public static bool IsPreview => Purpose == BuildPurpose.Preview;

	public static bool IsDev => Purpose == BuildPurpose.Dev;

	public static readonly string VersionedName;

	public static readonly string VersionTag;
	public static readonly string VersionedNameDevFriendly;

	static BuildInfo()
	{
		var parts = BuildIdentifier[(BuildIdentifier.IndexOf('+') + 1)..].Split('|');
		int i = 0;

		tMLVersion = new Version(parts[i++]);
		StableVersion = new Version(parts[i++]);
		BranchName = parts[i++];
		Enum.TryParse(parts[i++], true, out Purpose);
		CommitSHA = parts[i++];
		BuildDate = DateTime.FromBinary(long.Parse(parts[i++])).ToLocalTime();

		// Version name for players
		VersionedName = $"tModLoader v{tMLVersion}";

		if (!string.IsNullOrEmpty(BranchName) && BranchName != "unknown"
			&& BranchName != "1.4-stable" && BranchName != "1.4-preview" && BranchName != "1.4")
		{
			VersionedName += $" {BranchName}";
		}

		if (Purpose != BuildPurpose.Stable)
		{
			VersionedName += $" {Purpose}";
		}

		// Version Tag for ???
		VersionTag = VersionedName["tModLoader ".Length..].Replace(' ', '-').ToLower();

		// Version name for modders
		VersionedNameDevFriendly = VersionedName;

		if (CommitSHA != "unknown")
		{
			VersionedNameDevFriendly += $" {CommitSHA.Substring(0, 8)}";
		}

		VersionedNameDevFriendly += $", built {BuildDate:g}";
	}
}