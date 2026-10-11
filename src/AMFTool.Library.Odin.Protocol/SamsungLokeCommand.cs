namespace AMFTool.Library.Odin.Protocol;

public sealed class SamsungLokeCommand
{
	public int CommandId { get; set; }

	public int SequenceId { get; set; }

	public long BinaryTypeId { get; set; }

	public int PayloadSize { get; set; }

	public int UnknownCode { get; set; }

	public int DeviceId { get; set; }

	public int PartitionId { get; set; }

	public int SessionEndFlag { get; set; }

	public int EfsClearFlag { get; set; }

	public int BootUpdateFlag { get; set; }

	public SamsungLokeCommand(int commandId = 0, int sequenceId = 0, long binaryTypeId = 0L, int payloadSize = 0, int unknownCode = 0, int deviceId = 0, int partitionId = 0, int sessionEndFlag = 0, int efsClearFlag = 0, int bootUpdateFlag = 0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		CommandId = commandId;
		SequenceId = sequenceId;
		BinaryTypeId = binaryTypeId;
		PayloadSize = payloadSize;
		UnknownCode = unknownCode;
		DeviceId = deviceId;
		PartitionId = partitionId;
		SessionEndFlag = sessionEndFlag;
		EfsClearFlag = efsClearFlag;
		BootUpdateFlag = bootUpdateFlag;
	}

	public SamsungLokeCommand Clone()
	{
		return new SamsungLokeCommand(CommandId, SequenceId, BinaryTypeId, PayloadSize, UnknownCode, DeviceId, PartitionId, SessionEndFlag, EfsClearFlag, BootUpdateFlag);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
