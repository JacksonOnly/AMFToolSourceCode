using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AMFTool.Ext4;

public sealed class Ext4FileSystem
{
	[CompilerGenerated]
	private sealed class _003CEnumerateDirectory_003Ed__27 : IEnumerable<string>, IEnumerator<string>, IEnumerable, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string path;

		public string _003C_003E3__path;

		private bool recursive;

		public bool _003C_003E3__recursive;

		public Ext4FileSystem _003C_003E4__this;

		private string _003Cnormalized_003E5__1;

		private Ext4Inode _003Cinode_003E5__2;

		private IEnumerator<Ext4DirectoryEntry> _003C_003Es__3;

		private Ext4DirectoryEntry _003Centry_003E5__4;

		private string _003CfullPath_003E5__5;

		private IEnumerator<string> _003C_003Es__6;

		private string _003Csub_003E5__7;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEnumerateDirectory_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			object obj = null;
			int num = -2;
			int num2 = _003C_003E1__state;
			if ((uint)(num2 - -4) <= 1u || (uint)(num2 - 1) <= 1u)
			{
				try
				{
					if (num2 == -4 || num2 == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cnormalized_003E5__1 = (string)obj;
			_003Cinode_003E5__2 = (Ext4Inode)obj;
			_003C_003Es__3 = (IEnumerator<Ext4DirectoryEntry>)obj;
			_003Centry_003E5__4 = (Ext4DirectoryEntry)obj;
			_003CfullPath_003E5__5 = (string)obj;
			_003C_003Es__6 = (IEnumerator<string>)obj;
			_003Csub_003E5__7 = (string)obj;
			_003C_003E1__state = num;
		}

		private bool MoveNext()
		{
			string text = "/";
			object obj = null;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cnormalized_003E5__1 = CanonicalizePath(path);
					_003Cinode_003E5__2 = _003C_003E4__this.ResolveInodeByPath(_003Cnormalized_003E5__1);
					if (_003Cinode_003E5__2 != null && _003Cinode_003E5__2.IsDirectory)
					{
						_003C_003Es__3 = _003C_003E4__this.ListDirectory(_003Cnormalized_003E5__1).GetEnumerator();
						_003C_003E1__state = -3;
						break;
					}
					return false;
				case 1:
					_003C_003E1__state = -3;
					if (recursive && _003Centry_003E5__4.IsDirectory)
					{
						_003C_003Es__6 = _003C_003E4__this.EnumerateDirectory(_003CfullPath_003E5__5, recursive: true).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00cc;
					}
					goto IL_0113;
				case 2:
					{
						_003C_003E1__state = -4;
						_003Csub_003E5__7 = (string)obj;
						goto IL_00cc;
					}
					IL_0113:
					_003CfullPath_003E5__5 = (string)obj;
					_003Centry_003E5__4 = (Ext4DirectoryEntry)obj;
					break;
					IL_00cc:
					if (!_003C_003Es__6.MoveNext())
					{
						_003C_003Em__Finally2();
						_003C_003Es__6 = (IEnumerator<string>)obj;
						goto IL_0113;
					}
					_003Csub_003E5__7 = _003C_003Es__6.Current;
					_003C_003E2__current = _003Csub_003E5__7;
					_003C_003E1__state = 2;
					return true;
				}
				if (!_003C_003Es__3.MoveNext())
				{
					_003C_003Em__Finally1();
					_003C_003Es__3 = (IEnumerator<Ext4DirectoryEntry>)obj;
					return false;
				}
				_003Centry_003E5__4 = _003C_003Es__3.Current;
				_003CfullPath_003E5__5 = ((_003Cnormalized_003E5__1 == text) ? (text + _003Centry_003E5__4.Name) : (_003Cnormalized_003E5__1 + text + _003Centry_003E5__4.Name));
				_003C_003E2__current = _003CfullPath_003E5__5;
				_003C_003E1__state = 1;
				return true;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003Es__3 != null)
			{
				_003C_003Es__3.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003Es__6 != null)
			{
				_003C_003Es__6.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CEnumerateDirectory_003Ed__27 _003CEnumerateDirectory_003Ed__;
			if (_003C_003E1__state == -2)
			{
				if (_003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
				{
					_003C_003E1__state = 0;
					_003CEnumerateDirectory_003Ed__ = this;
					_003CEnumerateDirectory_003Ed__.path = _003C_003E3__path;
					_003CEnumerateDirectory_003Ed__.recursive = _003C_003E3__recursive;
					return _003CEnumerateDirectory_003Ed__;
				}
				_003CEnumerateDirectory_003Ed__ = new _003CEnumerateDirectory_003Ed__27(0);
			}
			else
			{
				_003CEnumerateDirectory_003Ed__ = new _003CEnumerateDirectory_003Ed__27(0);
			}
			_003CEnumerateDirectory_003Ed__._003C_003E4__this = _003C_003E4__this;
			_003CEnumerateDirectory_003Ed__.path = _003C_003E3__path;
			_003CEnumerateDirectory_003Ed__.recursive = _003C_003E3__recursive;
			return _003CEnumerateDirectory_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private const ushort EXT4_MAGIC = 61267;

	private const ushort EXTENT_MAGIC = 62218;

	private Stream BaseStream;

	private long BaseOffset;

	private uint BlockSizeField;

	private uint InodeSize;

	private uint InodesPerGroup;

	private uint BlocksPerGroup;

	private uint InodeCount;

	private bool Has64;

	private bool HasFtype;

	private GroupDescriptor[] Groups;

	public uint BlockSize => BlockSizeField;

	public static Ext4FileSystem Open(Stream source)
	{
		return new Ext4FileSystem(source, 0L);
	}

	public static Ext4FileSystem Open(Stream source, long offset)
	{
		return new Ext4FileSystem(source, offset);
	}

	private Ext4FileSystem(Stream source, long offset)
	{
		object obj = null;
		string paramName = "source";
		string message = "Stream must be readable and seekable.";

		if (source == obj)
		{
			throw new ArgumentNullException(paramName);
		}
		if (!source.CanRead || !source.CanSeek)
		{
			throw new ArgumentException(message);
		}
		BaseStream = source;
		BaseOffset = offset;
		LoadSuperblock();
		LoadGroupDescriptors();
	}

	public bool FileExists(string path)
	{
		object obj = null;
		return ResolveInodeByPath(path) !=null;
	}

	public bool Stat(string path, out Ext4Stat stat)
	{
		object obj = (stat = null);
		Ext4Inode ext4Inode = ResolveInodeByPath(path);
		if (ext4Inode != obj)
		{
			stat = new Ext4Stat
			{
				Path = CanonicalizePath(path),
				Inode = ext4Inode.InodeNumber,
				Size = ext4Inode.Size,
				IsDirectory = ext4Inode.IsDirectory,
				IsFile = ext4Inode.IsFile,
				IsSymlink = ext4Inode.IsSymlink
			};
			return true;
		}
		return false;
	}

	public bool IsDirectory(string path)
	{
		return ResolveInodeByPath(path)?.IsDirectory ?? false;
	}

	public bool IsFile(string path)
	{
		return ResolveInodeByPath(path)?.IsFile ?? false;
	}

	public Stream OpenRead(string path)
	{
		Ext4Inode ext4Inode = ResolveInodeByPath(path);
		if (ext4Inode == null || !ext4Inode.IsFile)
		{
			return Stream.Null;
		}
		return new Ext4BlockReader(this, ext4Inode);
	}

	public bool ReadFileData(string path, out byte[] data)
	{
		object obj = (data = null);
		Ext4Inode ext4Inode = ResolveInodeByPath(path);
		if (ext4Inode == null || !ext4Inode.IsFile)
		{
			return false;
		}
		using (Ext4BlockReader ext4BlockReader = new Ext4BlockReader(this, ext4Inode))
		{
			data = ext4BlockReader.ReadAll();
		}
		return data != null;
	}

	public byte[] ReadBuildProp()
	{
		string text = "/system/build.prop";
		string text2 = "/system/etc/build.prop";
		string text3 = "/vendor/build.prop";
		string text4 = "/product/etc/build.prop";
		object result = null;
		string[] array = new string[4] { text, text2, text3, text4 };
		int num = 0;
		byte[] array2;
		while (true)
		{
			if (num >= array.Length)
			{
				return (byte[])result;
			}
			array2 = ReadFile(array[num]);
			if (array2 != null && array2.Length != 0)
			{
				break;
			}
			num++;
		}
		return array2;
	}

	public byte[] ReadFile(string path)
	{
		ReadFileData(path, out var data);
		return data;
	}

	public bool ReadText(string path, Encoding encoding, out string text)
	{
		object obj = null;
		int result = 0;
		text = (string)obj;
		if (encoding == obj)
		{
			encoding = Encoding.UTF8;
		}
		if (!ReadFileData(path, out var data) || data == obj)
		{
			return false;
		}
		try
		{
			text = encoding.GetString(data);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public IReadOnlyList<Ext4DirectoryEntry> ListDirectory(string path)
	{
		Ext4Inode ext4Inode = ResolveInodeByPath(path);
		if (ext4Inode == null || !ext4Inode.IsDirectory)
		{
			return Array.Empty<Ext4DirectoryEntry>();
		}
		using Ext4BlockReader reader = new Ext4BlockReader(this, ext4Inode);
		return ParseDirectoryEntriesFromStream(reader);
	}

	[IteratorStateMachine(typeof(_003CEnumerateDirectory_003Ed__27))]
	public IEnumerable<string> EnumerateDirectory(string path, bool recursive)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateDirectory_003Ed__27(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__path = path,
			_003C_003E3__recursive = recursive
		};
	}

	public void Walk(string path, Action<Ext4Stat> visitor)
	{
		object obj = null;
		string text = "/";
		if (visitor == obj)
		{
			return;
		}
		string text2 = CanonicalizePath(path);
		if (!Stat(text2, out var stat))
		{
			return;
		}
		visitor(stat);
		if (stat.IsDirectory)
		{
			IReadOnlyList<Ext4DirectoryEntry> readOnlyList = ListDirectory(text2);
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				Ext4DirectoryEntry ext4DirectoryEntry = readOnlyList[i];
				string path2 = ((!(text2 == text)) ? (text2 + text + ext4DirectoryEntry.Name) : (text + ext4DirectoryEntry.Name));
				Walk(path2, visitor);
			}
		}
	}

	private void LoadSuperblock()
	{
		string message = "Not EXT4 (superblock magic mismatch).";
		byte[] array = new byte[1024];
		ReadAt(1024L, array, array.Length);
		int offset = 56;
		ushort num = Binary.ReadU16LE(array, ref offset);
		if (num != 61267)
		{
			throw new InvalidDataException(message);
		}
		offset = 0;
		InodeCount = Binary.ReadU32LE(array, ref offset);
		offset = 24;
		uint num2 = Binary.ReadU32LE(array, ref offset);
		BlockSizeField = (uint)(1 << (int)(10 + num2));
		offset = 32;
		BlocksPerGroup = Binary.ReadU32LE(array, ref offset);
		offset = 40;
		InodesPerGroup = Binary.ReadU32LE(array, ref offset);
		offset = 88;
		InodeSize = Binary.ReadU16LE(array, ref offset);
		if (InodeSize == 0)
		{
			InodeSize = 128u;
			offset = 96;
			uint num3 = Binary.ReadU32LE(array, ref offset);
			Has64 = (num3 & 0x80) != 0;
			HasFtype = (num3 & 2) != 0;
		}
		else
		{
			offset = 96;
			uint num3 = Binary.ReadU32LE(array, ref offset);
			Has64 = (num3 & 0x80) != 0;
			HasFtype = (num3 & 2) != 0;
		}
	}

	private void LoadGroupDescriptors()
	{
		uint num = (InodeCount + InodesPerGroup - 1) / InodesPerGroup;
		Groups = new GroupDescriptor[num];
		long num2 = ((BlockSize == 1024) ? 2048L : BlockSize);
		int num3 = ((!Has64) ? 32 : 64);
		byte[] array = new byte[num3];
		for (int i = 0; i < Groups.Length; i++)
		{
			long offset = num2 + (long)i * (long)num3;
			ReadAt(offset, array, array.Length);
			Groups[i] = GroupDescriptor.Parse(array, Has64);
		}
	}

	private Ext4Inode ResolveInodeByPath(string path)
	{
		object obj = null;
		string text = "/";
		try
		{
			if (!string.IsNullOrEmpty(path))
			{
				string text2 = CanonicalizePath(path);
				if (!(text2 == text))
				{
					Ext4Inode ext4Inode = ReadInode(2u);
					if (ext4Inode != null && ext4Inode.IsDirectory)
					{
						string[] array = text2.Trim('/').Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						int num = 0;
						while (true)
						{
							if (num >= array.Length)
							{
								return ext4Inode;
							}
							string text3 = array[num];
							IReadOnlyList<Ext4DirectoryEntry> readOnlyList = ParseDirectoryEntriesFromStream(new Ext4BlockReader(this, ext4Inode));
							Ext4DirectoryEntry ext4DirectoryEntry = (Ext4DirectoryEntry)obj;
							for (int i = 0; i < readOnlyList.Count; i++)
							{
								if (readOnlyList[i].Name == text3)
								{
									ext4DirectoryEntry = readOnlyList[i];
									break;
								}
							}
							if (ext4DirectoryEntry != obj)
							{
								ext4Inode = ReadInode(ext4DirectoryEntry.Inode);
								if (ext4Inode == obj)
								{
									break;
								}
								num++;
								continue;
							}
							return (Ext4Inode)obj;
						}
						return (Ext4Inode)obj;
					}
					return (Ext4Inode)obj;
				}
				return ReadInode(2u);
			}
			return (Ext4Inode)obj;
		}
		catch (ArgumentOutOfRangeException)
		{
			return (Ext4Inode)obj;
		}
		catch (EndOfStreamException)
		{
			return (Ext4Inode)obj;
		}
	}

	private IReadOnlyList<Ext4DirectoryEntry> ParseDirectoryEntriesFromStream(Ext4BlockReader reader)
	{
		string text = ".";
		string text2 = "..";
		List<Ext4DirectoryEntry> list = new List<Ext4DirectoryEntry>();
		byte[] array = new byte[BlockSize];
		long num = reader.Length;
		while (num > 0L)
		{
			int count = (int)Math.Min(array.Length, num);
			int num2 = reader.Read(array, 0, count);
			if (num2 <= 0)
			{
				break;
			}
			ushort num4;
			for (int i = 0; i + 8 <= num2; i += num4)
			{
				int offset = i;
				uint num3 = Binary.ReadU32LE(array, ref offset);
				offset = i + 4;
				num4 = Binary.ReadU16LE(array, ref offset);
				byte b = array[i + 6];
				byte fileType = array[i + 7];
				if (num4 < 8 || i + num4 > num2)
				{
					break;
				}
				if (num3 != 0 && b > 0 && 8 + b <= num4)
				{
					string text3 = Encoding.UTF8.GetString(array, i + 8, b);
					if (text3 != text && text3 != text2)
					{
						ResolveDirentFileType(num3, fileType, out var isDirectory, out var isFile, out var isSymlink);
						list.Add(new Ext4DirectoryEntry
						{
							Inode = num3,
							Name = text3,
							FileType = fileType,
							IsDirectory = isDirectory,
							IsFile = isFile,
							IsSymlink = isSymlink
						});
					}
				}
			}
			num -= num2;
		}
		return list;
	}

	private void ResolveDirentFileType(uint dirInode, byte fileType, out bool isDirectory, out bool isFile, out bool isSymlink)
	{
		object obj = null;
		isDirectory = false;
		isFile = false;
		isSymlink = false;
		if (!HasFtype)
		{
			Ext4Inode ext4Inode = ReadInode(dirInode);
			if (ext4Inode != obj)
			{
				isDirectory = ext4Inode.IsDirectory;
				isFile = ext4Inode.IsFile;
				isSymlink = ext4Inode.IsSymlink;
			}
		}
		else if (fileType == 0)
		{
			Ext4Inode ext4Inode = ReadInode(dirInode);
			if (ext4Inode != obj)
			{
				isDirectory = ext4Inode.IsDirectory;
				isFile = ext4Inode.IsFile;
				isSymlink = ext4Inode.IsSymlink;
			}
		}
		else
		{
			isDirectory = fileType == 2;
			isFile = fileType == 1;
			isSymlink = fileType == 7;
		}
	}

	public Ext4Inode ReadInode(uint inode)
	{
		object result = null;
		if (inode != 0)
		{
			uint num = inode - 1;
			uint num2 = num / InodesPerGroup;
			uint num3 = num % InodesPerGroup;
			if (num2 < Groups.Length)
			{
				ulong inodeTable = Groups[num2].InodeTable;
				long num4 = (long)(inodeTable * BlockSize);
				long offset = num4 + (long)num3 * (long)InodeSize;
				byte[] array = new byte[InodeSize];
				ReadAt(offset, array, array.Length);
				return Ext4Inode.Parse(array, inode);
			}
			return (Ext4Inode)result;
		}
		return (Ext4Inode)result;
	}

	public List<Ext4Extent> BuildBlockMap(Ext4Inode inode)
	{
		object obj = null;
		List<Ext4Extent> list = new List<Ext4Extent>();
		if (inode == null || inode.IBlock == obj)
		{
			return list;
		}
		if (!inode.HasInlineData)
		{
			if (inode.HasExtents && inode.IBlock.Length >= 12)
			{
				int offset = 0;
				ushort num = Binary.ReadU16LE(inode.IBlock, ref offset);
				if (num == 62218)
				{
					offset = 2;
					ushort num2 = Binary.ReadU16LE(inode.IBlock, ref offset);
					offset = 6;
					ushort num3 = Binary.ReadU16LE(inode.IBlock, ref offset);
					if (num3 == 0)
					{
						ReadLeafExtentRecords(list, inode.IBlock, num2);
						CoalesceAdjacentExtents(list);
						return list;
					}
					ReadExtentIndexNode(list, inode.IBlock, num2, num3);
					CoalesceAdjacentExtents(list);
					return list;
				}
			}
			for (int i = 0; i < 12; i++)
			{
				int offset2 = i * 4;
				uint num4 = Binary.ReadU32LE(inode.IBlock, ref offset2);
				if (num4 != 0)
				{
					list.Add(new Ext4Extent
					{
						LogicalBlock = (uint)i,
						BlockCount = 1u,
						PhysicalBlock = num4
					});
				}
			}
			CoalesceAdjacentExtents(list);
			return list;
		}
		return list;
	}

	private void ReadExtentIndexNode(List<Ext4Extent> mapping, byte[] nodebuff, ushort entryCount, ushort depth)
	{
		int num = 12;
		for (int i = 0; i < entryCount; i++)
		{
			if (num + 12 > nodebuff.Length)
			{
				break;
			}
			int offset = num + 4;
			uint num2 = Binary.ReadU32LE(nodebuff, ref offset);
			offset = num + 8;
			ushort num3 = Binary.ReadU16LE(nodebuff, ref offset);
			ulong num4 = ((ulong)num3 << 32) | num2;
			num += 12;
			byte[] array = new byte[BlockSize];
			ReadAt((long)(num4 * BlockSize), array, array.Length);
			int offset2 = 0;
			ushort num5 = Binary.ReadU16LE(array, ref offset2);
			if (num5 == 62218)
			{
				offset2 = 2;
				ushort num6 = Binary.ReadU16LE(array, ref offset2);
				offset2 = 6;
				ushort num7 = Binary.ReadU16LE(array, ref offset2);
				if (num7 == 0)
				{
					ReadLeafExtentRecords(mapping, array, num6);
				}
				else
				{
					ReadExtentIndexNode(mapping, array, num6, num7);
				}
			}
		}
	}

	private void ReadLeafExtentRecords(List<Ext4Extent> mapping, byte[] leafBuffer, ushort extentCount)
	{
		int num = 12;
		for (int i = 0; i < extentCount; i++)
		{
			if (num + 12 > leafBuffer.Length)
			{
				break;
			}
			int offset = num;
			uint logicalBlock = Binary.ReadU32LE(leafBuffer, ref offset);
			offset = num + 4;
			ushort num2 = Binary.ReadU16LE(leafBuffer, ref offset);
			offset = num + 6;
			ushort num3 = Binary.ReadU16LE(leafBuffer, ref offset);
			offset = num + 8;
			uint num4 = Binary.ReadU32LE(leafBuffer, ref offset);
			uint num5 = (uint)(num2 & 0x7FFF);
			ulong physicalBlock = ((ulong)num3 << 32) | num4;
			if (num5 != 0)
			{
				mapping.Add(new Ext4Extent
				{
					LogicalBlock = logicalBlock,
					BlockCount = num5,
					PhysicalBlock = (long)physicalBlock
				});
			}
			num += 12;
		}
	}

	public long ResolvePhysicalBlockForLogicalBlock(List<Ext4Extent> mapping, uint fileblock)
	{
		if (mapping != null && mapping.Count != 0)
		{
			int num = 0;
			Ext4Extent ext4Extent;
			uint logicalBlock;
			while (true)
			{
				if (num >= mapping.Count)
				{
					return -1L;
				}
				ext4Extent = mapping[num];
				logicalBlock = ext4Extent.LogicalBlock;
				uint num2 = logicalBlock + ext4Extent.BlockCount;
				if (fileblock >= logicalBlock && fileblock < num2)
				{
					break;
				}
				num++;
			}
			uint num3 = fileblock - logicalBlock;
			return ext4Extent.PhysicalBlock + num3;
		}
		return -1L;
	}

	private static void CoalesceAdjacentExtents(List<Ext4Extent> Extents)
	{
		if (Extents == null || Extents.Count == 0)
		{
			return;
		}
		Extents.Sort(delegate(Ext4Extent a, Ext4Extent b)
		{
			if (a.LogicalBlock < b.LogicalBlock)
			{
				return -1;
			}
			return (a.LogicalBlock > b.LogicalBlock) ? 1 : 0;
		});
		int num = 0;
		while (num + 1 < Extents.Count)
		{
			Ext4Extent value = Extents[num];
			Ext4Extent ext4Extent = Extents[num + 1];
			bool flag = value.LogicalBlock + value.BlockCount == ext4Extent.LogicalBlock;
			bool flag2 = value.PhysicalBlock + value.BlockCount == ext4Extent.PhysicalBlock;
			if (!(flag && flag2))
			{
				num++;
				continue;
			}
			value.BlockCount += ext4Extent.BlockCount;
			Extents[num] = value;
			Extents.RemoveAt(num + 1);
		}
	}

	public void ReadAt(long offset, byte[] buffer, int count)
	{
		BaseStream.Seek(BaseOffset + offset, SeekOrigin.Begin);
		int num = 0;
		if (0 >= count)
		{
			return;
		}
		int num2 = BaseStream.Read(buffer, 0, count);
		if (num2 > 0)
		{
			num = 0 + num2;
			int num3 = 442829546;
			while (true)
			{
				_ = num3 ^ -1175587005;
				if (num < count)
				{
					num2 = BaseStream.Read(buffer, num, count - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 442829546;
						continue;
					}
					throw new EndOfStreamException();
				}
				break;
			}
			return;
		}
		throw new EndOfStreamException();
	}

	private static string CanonicalizePath(string path)
	{
		string text = "/";
		string text2 = "//";
		string text3 = path.Replace('\\', '/').Trim();
		if (text3.Length != 0)
		{
			bool flag;
			if (text3[0] == '/')
			{
				flag = text3.Contains(text2);
			}
			else
			{
				text3 = text + text3;
				flag = text3.Contains(text2);
			}
			while (flag)
			{
				text3 = text3.Replace(text2, text);
				flag = text3.Contains(text2);
			}
			if (text3.Length <= 1 || text3[text3.Length - 1] != '/')
			{
				return text3;
			}
			return text3.Substring(0, text3.Length - 1);
		}
		return text;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
