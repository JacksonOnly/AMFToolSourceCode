using System;

namespace AMFTool.Library.Spreadtrum;

public class Partf
{
	public Guid Unique;

	public ulong FirstLba;

	public ulong LastLba;

	public ulong Flags;

	public ulong Sector;

	public ulong Sectors;

	public byte[] Type;

	public string Name;
}
