namespace Debaters.Server.Model;

using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract class Community : DatabaseObject
{
	[DatabaseProperty]
	public abstract string Name { get; set; }

	[DatabaseProperty]
	public abstract string Description { get; set;}

	[InverseReferences(nameof(Post.Community))]
	public abstract InverseReferenceSet<Post> Posts { get; }
}

