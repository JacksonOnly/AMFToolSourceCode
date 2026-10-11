using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMFTool.Library.Lz4;

namespace AMFTool.Library.Odin.Tar;

public class Tar
{
	public IList<TarEntryInfo> GetEntriesFromBytes(byte[] tarBytes)
	{
		return TarArchiveCore.ReadEntriesFromBytes(tarBytes);
	}

	public IList<TarEntryInfo> GetEntries(string tarPath)
	{
		return TarArchiveCore.ReadEntries(tarPath);
	}

	public byte[] ExtractEntry(string tarPath, string entryName)
	{
		TarEntryInfo tarEntryInfo = GetEntries(tarPath).FirstOrDefault((TarEntryInfo item) => string.Equals(item.Name, entryName, StringComparison.Ordinal));
		if (tarEntryInfo != null)
		{
			if (tarEntryInfo.Size > 0L)
			{
				return TarArchiveCore.ExtractEntry(tarPath, tarEntryInfo);
			}
			UI.Progresbar1(0L, 0L);
			return Array.Empty<byte>();
		}
		UI.Progresbar1(0L, 0L);
		return Array.Empty<byte>();
	}

	public long GetLz4DecodedSizeFromEntry(string tarPath, string entryName)
	{
		TarEntryInfo tarEntryInfo = GetEntries(tarPath).FirstOrDefault((TarEntryInfo item) => string.Equals(item.Name, entryName, StringComparison.Ordinal));
		if (tarEntryInfo == null || tarEntryInfo.Size <= 0L)
		{
			return 0L;
		}
		using FileStream innerStream = File.OpenRead(tarPath);
		using LimitedReadStream source = new LimitedReadStream(innerStream, tarEntryInfo.DataStartOffset, tarEntryInfo.Size);
		return SizeCalculator.CalculateDecodedSize(source);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
