using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura;

[CompilerGenerated]
internal static class AssemblyLoader
{
	private static object field_01 = new object();

	private static Dictionary<string, bool> field_02 = new Dictionary<string, bool>();

	private static Dictionary<string, string> field_03 = new Dictionary<string, string>();

	private static Dictionary<string, string> field_04 = new Dictionary<string, string>();

	private static int field_05;

	private static string method_01(CultureInfo C54C0D2D)
	{
		if (C54C0D2D == null)
		{
			return string.Empty;
		}
		return C54C0D2D.Name;
	}

	private static Assembly method_02(AssemblyName A0FB4EE7)
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		Assembly[] assemblies = currentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		int num = 0;
		Assembly assembly;
		while (true)
		{
			if (num < array.Length)
			{
				assembly = array[num];
				AssemblyName name = assembly.GetName();
				if (string.Equals(name.Name, A0FB4EE7.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(method_01(name.CultureInfo), method_01(A0FB4EE7.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return assembly;
	}

	private static string method_03(AssemblyName B0708B61)
	{
		string text = B0708B61.Name.ToLowerInvariant();
		if (B0708B61.CultureInfo != null && !string.IsNullOrEmpty(B0708B61.CultureInfo.Name))
		{
			text = (method_01(B0708B61.CultureInfo) + "." + text).ToLowerInvariant();
		}
		return text;
	}

	private static void method_04(Stream C9D77F5F, Stream B5CD3C5B)
	{
		byte[] array = new byte[81920];
		int count;
		while ((count = C9D77F5F.Read(array, 0, array.Length)) != 0)
		{
			B5CD3C5B.Write(array, 0, count);
		}
	}

	private static Stream method_05(string B989A319)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		if (B989A319.EndsWith(".compressed"))
		{
			using (Stream stream = executingAssembly.GetManifestResourceStream(B989A319))
			{
				using DeflateStream c9D77F5F = new DeflateStream(stream, CompressionMode.Decompress);
				MemoryStream memoryStream = new MemoryStream();
				method_04(c9D77F5F, memoryStream);
				memoryStream.Position = 0L;
				return memoryStream;
			}
		}
		return executingAssembly.GetManifestResourceStream(B989A319);
	}

	private static Stream method_06(Dictionary<string, string> CB19F439, string AB504A90)
	{
		if (CB19F439.TryGetValue(AB504A90, out var value))
		{
			return method_05(value);
		}
		return null;
	}

	private static byte[] method_07(Stream E8CF54CE)
	{
		byte[] array = new byte[E8CF54CE.Length];
		E8CF54CE.Read(array, 0, array.Length);
		return array;
	}

	private static Assembly method_08(Dictionary<string, string> F594E1C4, Dictionary<string, string> CD7489F2, AssemblyName FF0301B1)
	{
		string aB504A = method_03(FF0301B1);
		byte[] rawAssembly;
		using (Stream stream = method_06(F594E1C4, aB504A))
		{
			if (stream == null)
			{
				return null;
			}
			rawAssembly = method_07(stream);
		}
		using (Stream stream2 = method_06(CD7489F2, aB504A))
		{
			if (stream2 != null)
			{
				byte[] rawSymbolStore = method_07(stream2);
				return Assembly.Load(rawAssembly, rawSymbolStore);
			}
		}
		return Assembly.Load(rawAssembly);
	}

	public static Assembly method_09(object D9981151, ResolveEventArgs AB88D144)
	{
		string name = AB88D144.Name;
		AssemblyName assemblyName = new AssemblyName(name);
		lock (field_01)
		{
			if (field_02.ContainsKey(name))
			{
				return null;
			}
		}
		Assembly assembly = method_02(assemblyName);
		if ((object)assembly != null)
		{
			return assembly;
		}
		assembly = method_08(field_03, field_04, assemblyName);
		if ((object)assembly == null)
		{
			lock (field_01)
			{
				field_02[name] = true;
			}
			if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
			{
				assembly = Assembly.Load(assemblyName);
			}
		}
		return assembly;
	}

	static AssemblyLoader()
	{
		field_03.Add("costura", "costura.costura.dll.compressed");
		field_04.Add("costura", "costura.costura.pdb.compressed");
		field_03.Add("devexpress.data.desktop.v25.2", "costura.devexpress.data.desktop.v25.2.dll.compressed");
		field_03.Add("devexpress.data.v25.2", "costura.devexpress.data.v25.2.dll.compressed");
		field_03.Add("devexpress.drawing.v25.2", "costura.devexpress.drawing.v25.2.dll.compressed");
		field_03.Add("devexpress.office.v25.2.core", "costura.devexpress.office.v25.2.core.dll.compressed");
		field_03.Add("devexpress.pdf.v25.2.core", "costura.devexpress.pdf.v25.2.core.dll.compressed");
		field_03.Add("devexpress.pdf.v25.2.drawing", "costura.devexpress.pdf.v25.2.drawing.dll.compressed");
		field_03.Add("devexpress.printing.v25.2.core", "costura.devexpress.printing.v25.2.core.dll.compressed");
		field_03.Add("devexpress.richedit.v25.2.core", "costura.devexpress.richedit.v25.2.core.dll.compressed");
		field_03.Add("devexpress.sparkline.v25.2.core", "costura.devexpress.sparkline.v25.2.core.dll.compressed");
		field_03.Add("devexpress.utils.v25.2", "costura.devexpress.utils.v25.2.dll.compressed");
		field_03.Add("devexpress.xtrabars.v25.2", "costura.devexpress.xtrabars.v25.2.dll.compressed");
		field_03.Add("devexpress.xtraeditors.v25.2", "costura.devexpress.xtraeditors.v25.2.dll.compressed");
		field_03.Add("devexpress.xtralayout.v25.2", "costura.devexpress.xtralayout.v25.2.dll.compressed");
		field_03.Add("devexpress.xtraprinting.v25.2", "costura.devexpress.xtraprinting.v25.2.dll.compressed");
		field_03.Add("devexpress.xtratreelist.v25.2", "costura.devexpress.xtratreelist.v25.2.dll.compressed");
		field_03.Add("downloader", "costura.downloader.dll.compressed");
		field_03.Add("file", "costura.file.dll.compressed");
		field_03.Add("libusbdotnet.libusbdotnet", "costura.libusbdotnet.libusbdotnet.dll.compressed");
		field_04.Add("libusbdotnet.libusbdotnet", "costura.libusbdotnet.libusbdotnet.pdb.compressed");
		field_03.Add("microsoft.bcl.asyncinterfaces", "costura.microsoft.bcl.asyncinterfaces.dll.compressed");
		field_03.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
		field_03.Add("system.buffers", "costura.system.buffers.dll.compressed");
		field_03.Add("system.io.pipelines", "costura.system.io.pipelines.dll.compressed");
		field_03.Add("system.memory", "costura.system.memory.dll.compressed");
		field_03.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
		field_03.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
		field_03.Add("system.text.encodings.web", "costura.system.text.encodings.web.dll.compressed");
		field_03.Add("system.text.json", "costura.system.text.json.dll.compressed");
		field_03.Add("system.threading.tasks.extensions", "costura.system.threading.tasks.extensions.dll.compressed");
	}

	public static void method_10(bool F0EBAB7E)
	{
		if (Interlocked.Exchange(ref field_05, 1) == 1 || !F0EBAB7E)
		{
			return;
		}
		AppDomain currentDomain = AppDomain.CurrentDomain;
		currentDomain.AssemblyResolve += delegate(object D9981151, ResolveEventArgs AB88D144)
		{
			string name = AB88D144.Name;
			AssemblyName assemblyName = new AssemblyName(name);
			lock (field_01)
			{
				if (field_02.ContainsKey(name))
				{
					return (Assembly)null;
				}
			}
			Assembly assembly = method_02(assemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = method_08(field_03, field_04, assemblyName);
			if ((object)assembly == null)
			{
				lock (field_01)
				{
					field_02[name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		};
	}
}
