namespace AMFTool.Library.Android;

public class AdbDevice
{
	public string SerialNumber { get; private set; }

	public string Product { get; private set; }

	public string Model { get; private set; }

	public string Device { get; private set; }

	public AdbDevice(string id, string product, string model, string device)
	{
		SerialNumber = id;
		Product = product;
		Model = model;
		Device = device;
	}

	public override string ToString()
	{
		string text = "serialNumber:";
		string text2 = ", product:";
		string text3 = ", model:";
		string text4 = ", device:";
		return text + SerialNumber + text2 + Product + text3 + Model + text4 + Device;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
