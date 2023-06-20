namespace Debaters.Server.Model;

using System;
using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex(CommunityNameIndex, true, nameof(Community.NameLower))]
public abstract class Community : DatabaseObject
{
	private const string CommunityNameIndex = "communityName";

	[DatabaseProperty]
	public abstract string Name { get; set; }

	[DatabaseProperty]
	public abstract string NameLower { get; set; }

	[DatabaseProperty]
	public abstract string Description { get; set;}

	[InverseReferences(nameof(Post.Community))]
	public abstract InverseReferenceSet<Post> Posts { get; }

	public static HashIndexReader<Community, string> GetCommunityNameIndex(ObjectModel om)
	{
		return om.GetHashIndex<Community, string>(CommunityNameIndex);
	}
}

