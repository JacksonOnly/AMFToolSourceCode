using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace AMFTool.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			object obj = null;
			string baseName = "AMFTool.Properties.Resources";
			if (resourceMan == obj)
			{
				ResourceManager resourceManager = new ResourceManager(baseName, typeof(Resources).Assembly);
				resourceMan = resourceManager;
				return resourceMan;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static byte[] _base
	{
		get
		{
			string name = "base";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (byte[])obj;
		}
	}

	public static byte[] allin_one_signature
	{
		get
		{
			string name = "allin_one_signature";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (byte[])obj;
		}
	}

	public static byte[] AMF
	{
		get
		{
			string name = "AMF";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (byte[])obj;
		}
	}

	public static Bitmap backup
	{
		get
		{
			string name = "backup";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap DATAERASE
	{
		get
		{
			string name = "DATAERASE";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static byte[] DigestsToSign_17151_persist_no_userdata_yes_bin
	{
		get
		{
			string name = "DigestsToSign_17151_persist_no_userdata_yes.bin";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (byte[])obj;
		}
	}

	public static Bitmap disable_micloud
	{
		get
		{
			string name = "disable_micloud";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap ERASE
	{
		get
		{
			string name = "ERASE";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap FACTORY
	{
		get
		{
			string name = "FACTORY";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap flash
	{
		get
		{
			string name = "flash";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap FRP
	{
		get
		{
			string name = "FRP";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap GPT_
	{
		get
		{
			string name = "GPT.";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap hide
	{
		get
		{
			string name = "hide";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap icons8_opened_folder_17px
	{
		get
		{
			string name = "icons8_opened_folder_17px";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap icons8_x_13px
	{
		get
		{
			string name = "icons8_x_13px";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap logoamftool
	{
		get
		{
			string name = "logoamftool";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap REboot
	{
		get
		{
			string name = "REboot";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap reset_mcloud
	{
		get
		{
			string name = "reset_mcloud";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap safeformat
	{
		get
		{
			string name = "safeformat";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap show
	{
		get
		{
			string name = "show";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap unlock
	{
		get
		{
			string name = "unlock";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static byte[] userdata
	{
		get
		{
			string name = "userdata";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (byte[])obj;
		}
	}

	public static Bitmap warning
	{
		get
		{
			string name = "warning";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap WRITE
	{
		get
		{
			string name = "WRITE";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	public static Bitmap write2
	{
		get
		{
			string name = "write2";
			object obj = ResourceManager.GetObject(name, resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal Resources()
	{
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
