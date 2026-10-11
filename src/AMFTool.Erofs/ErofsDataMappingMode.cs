namespace AMFTool.Erofs;

public enum ErofsDataMappingMode
{
	FlatCompressionLegacy = 1,
	FlatCompression = 3,
	EROFS_INODE_FLAT_PLAIN = 0,
	EROFS_INODE_FLAT_INLINE = 2,
	EROFS_INODE_LAYOUT_MAX = 4
}
