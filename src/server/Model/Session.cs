namespace Debaters.Server.Model;

using System;
using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex(SidHashIndex, true, nameof(Session.SidHigh), nameof(Session.SidLow))]
public abstract class Session : DatabaseObject
{
	private const string SidHashIndex = "sid";
	[DatabaseProperty]
	public abstract long SidHigh{ get; set; }

	[DatabaseProperty]
	public abstract long SidLow{ get; set; }

	[DatabaseProperty]
	public abstract DateTime LastAccessed { get; set; }

    [DatabaseReference(isNullable:false, deleteTargetAction:DeleteTargetAction.CascadeDelete)]
    public abstract User User { get; set; }

    public static HashIndexReader<Session, long, long> GetSidIndex(ObjectModel om) => om.GetHashIndex<Session, long, long>(SidHashIndex);
}

