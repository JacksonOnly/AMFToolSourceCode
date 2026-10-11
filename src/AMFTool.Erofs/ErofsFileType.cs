namespace AMFTool.Erofs;

public enum ErofsFileType : byte
{
	RegularFile = 1,
	CharDevice = 3,
	Fifo = 5,
	Symlink = 7,
	EROFS_FT_UNKNOWN = 0,
	EROFS_FT_DIR = 2,
	EROFS_FT_BLKDEV = 4,
	EROFS_FT_SOCK = 6,
	EROFS_FT_MAX = 8
}
