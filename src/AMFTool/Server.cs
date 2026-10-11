using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace AMFTool;

public class Server
{
	public class ModelConfigEDL
	{
		public string Model { get; set; }

		public string SOS { get; set; }

		public List<string> Operation { get; set; } = new List<string>();

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class ModelConfigMTK
	{
		public string Model { get; set; }

		public bool Customda { get; set; }

		public bool Danofarse { get; set; }

		public bool AuthFile { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class ModelConfigSPD
	{
		public string Model { get; set; }

		public string SOC { get; set; }
	}

	public class EmiServer
	{
		public string Type { get; set; }

		public string url { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public static string BrnadLoader;

	public static string amtusername;

	public static string amtpassword;

	public static string urlemi;

	public static string Keyfile;

	private static string Key;

	private static string IV;

	public static string DataCenter;

	private static readonly HttpClient http;

	private static Dictionary<string, List<ModelConfigEDL>> cacheEdl;

	private static Dictionary<string, List<ModelConfigMTK>> cacheMtk;

	private static Dictionary<string, List<ModelConfigSPD>> cacheSpd;

	public static string jsonmenemi { get; set; }

	private static HttpClient CreateClient()
	{
		string input = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
		string input2 = "identity";
		HttpClientHandler handler = new HttpClientHandler
		{
			AutomaticDecompression = DecompressionMethods.None,
			UseProxy = false,
			AllowAutoRedirect = false
		};
		HttpClient httpClient = new HttpClient(handler);
		httpClient.Timeout = Timeout.InfiniteTimeSpan;
		httpClient.DefaultRequestHeaders.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(input);
		httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd(input2);
		httpClient.DefaultRequestHeaders.ConnectionClose = true;
		return httpClient;
	}

	public static byte[] Downloadfile(string url)
	{
		string text = "URL : ";
		Console.WriteLine(text + url);
		using HttpResponseMessage httpResponseMessage = http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
		httpResponseMessage.EnsureSuccessStatusCode();
		long num = httpResponseMessage.Content.Headers.ContentLength ?? (-1L);
		long num2 = 0L;
		using Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
		using MemoryStream memoryStream = new MemoryStream();
		byte[] array = new byte[32768];
		while (true)
		{
			int num3 = stream.Read(array, 0, array.Length);
			if (num3 <= 0)
			{
				break;
			}
			memoryStream.Write(array, 0, num3);
			num2 += num3;
			if (num > 0L)
			{
				UI.Progresbar1(num2, num);
			}
		}
		if (num > 0L)
		{
			UI.Progresbar1(num, num);
		}
		return memoryStream.ToArray();
	}

	public static string FindLoader(string fileName)
	{
		string key = "namafile";
		string text = "/Qualcomm/Auto/auto.php";
		string propertyName = "url";
		object result = null;
		string result2 = "Kosong";
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
		using FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[1]
		{
			new KeyValuePair<string, string>(key, fileName)
		});
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, DataCenter + text)
		{
			Content = content
		};
		using HttpResponseMessage httpResponseMessage = http.SendAsync(request).GetAwaiter().GetResult();
		httpResponseMessage.EnsureSuccessStatusCode();
		string result3 = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
		using JsonDocument jsonDocument = JsonDocument.Parse(result3);
		if (jsonDocument.RootElement.TryGetProperty(propertyName, out var value))
		{
			string text2 = value.GetString();
			if (string.IsNullOrEmpty(text2))
			{
				return result2;
			}
			return text2;
		}
		return (string)result;
	}

	public static byte[] LoadFile(string filename)
	{
		string path = "SDM";
		string path2 = Path.Combine(AppContext.BaseDirectory, path);
		return File.ReadAllBytes(Path.Combine(path2, filename));
	}

	public static byte[] LoadOfflineFile(string path)
	{
		object result = null;
		byte[] array = LoadFile(path);
		if (array == null)
		{
			return (byte[])result;
		}
		if (array.Length != 0)
		{
			array = DecryptLoader(array);
			if (array != null && array.Length != 0)
			{
				return array;
			}
			return (byte[])result;
		}
		return (byte[])result;
	}

	public static byte[] Downloadbyte(string filename)
	{
		string text = "URL : ";
		string method = "GET";
		string text2 = DataCenter + filename;
		Console.WriteLine(text + text2);
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text2);
		httpWebRequest.Method = method;
		using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		long contentLength = httpWebResponse.ContentLength;
		long num = 0L;
		using Stream stream = httpWebResponse.GetResponseStream();
		using MemoryStream memoryStream = new MemoryStream();
		byte[] array = new byte[32768];
		while (true)
		{
			int num2 = stream.Read(array, 0, array.Length);
			if (num2 <= 0)
			{
				break;
			}
			memoryStream.Write(array, 0, num2);
			num += num2;
			if (contentLength > 0L)
			{
				UI.ProgressBar1(num, contentLength);
			}
		}
		if (contentLength > 0L)
		{
			UI.ProgressBar1(contentLength, contentLength);
		}
		return memoryStream.ToArray();
	}

	public static string DownloadXML(string filename)
	{
		string input = "application/xml, text/xml, */*";
		string text = DataCenter + filename;
		Console.WriteLine(text);
		using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, text);
		httpRequestMessage.Headers.Accept.ParseAdd(input);
		using HttpResponseMessage httpResponseMessage = http.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
		httpResponseMessage.EnsureSuccessStatusCode();
		long num = httpResponseMessage.Content.Headers.ContentLength ?? (-1L);
		long num2 = 0L;
		using Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
		using MemoryStream memoryStream = ((num <= 0L || num > 2147483647L) ? new MemoryStream() : new MemoryStream((int)num));
		byte[] array = new byte[65536];
		while (true)
		{
			int num3 = stream.Read(array, 0, array.Length);
			if (num3 <= 0)
			{
				break;
			}
			memoryStream.Write(array, 0, num3);
			num2 += num3;
			UI.Progresbar1(num2, num);
		}
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}

	public static byte[] LoadAndDecrypt(string path)
	{
		byte[] array = Downloadbyte(path);
		if (array == null || array.Length == 0)
		{
			return Array.Empty<byte>();
		}
		array = DecryptLoader(array);
		if (array == null)
		{
			return Array.Empty<byte>();
		}
		if (array.Length != 0)
		{
			return array;
		}
		return Array.Empty<byte>();
	}

	private static string LoadModel(string protocol)
	{
		string text = "/models.json";
		string input = "application/json";
		string requestUri = DataCenter + protocol + text;
		using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
		httpRequestMessage.Headers.Accept.ParseAdd(input);
		using HttpResponseMessage httpResponseMessage = http.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
		httpResponseMessage.EnsureSuccessStatusCode();
		return httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
	}

	public static byte[] DecryptLoader(byte[] input)
	{
		int mode = 1;
		object result = null;
		string s = "Alpha28January";
		int num = 16;
		int num2 = 0;
		int keySize = 256;
		int blockSize = 128;
		int padding = 2;
		if (input == null || input.Length < 17)
		{
			return (byte[])result;
		}
		byte[] key;
		using (SHA256 sHA = SHA256.Create())
		{
			key = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));
		}
		byte[] array = new byte[num];
		input.AsSpan(num2, num).CopyTo(array.AsSpan(num2, num));
		byte[] array2 = new byte[input.Length - num];
		input.AsSpan(num, array2.Length).CopyTo(array2.AsSpan(num2, array2.Length));
		using Aes aes = Aes.Create();
		aes.KeySize = keySize;
		aes.BlockSize = blockSize;
		aes.Mode = (CipherMode)mode;
		aes.Padding = (PaddingMode)padding;
		aes.Key = key;
		aes.IV = array;
		using ICryptoTransform transform = aes.CreateDecryptor();
		using MemoryStream memoryStream = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, (CryptoStreamMode)mode);
		cryptoStream.Write(array2, num2, array2.Length);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	public static string EncryptAES(string data)
	{
		using Aes aes = Aes.Create();
		aes.Key = Encoding.UTF8.GetBytes(Key);
		aes.IV = Encoding.UTF8.GetBytes(IV);
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		using ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(data);
		}
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static string DecryptAES(string bash64)
	{
		byte[] buffer = Convert.FromBase64String(bash64);
		using Aes aes = Aes.Create();
		aes.Key = Encoding.UTF8.GetBytes(Key);
		aes.IV = Encoding.UTF8.GetBytes(IV);
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		using ICryptoTransform transform = aes.CreateDecryptor();
		using MemoryStream stream = new MemoryStream(buffer);
		using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using StreamReader streamReader = new StreamReader(stream2, Encoding.UTF8);
		return streamReader.ReadToEnd();
	}

	public static void LoadAllModel()
	{
		string protocol = "Qualcomm";
		object options = null;
		string protocol2 = "Mediatek";
		string protocol3 = "Spreadtrum";
		cacheEdl = JsonSerializer.Deserialize<Dictionary<string, List<ModelConfigEDL>>>(LoadModel(protocol), (JsonSerializerOptions?)options);
		cacheMtk = JsonSerializer.Deserialize<Dictionary<string, List<ModelConfigMTK>>>(LoadModel(protocol2), (JsonSerializerOptions?)options);
		cacheSpd = JsonSerializer.Deserialize<Dictionary<string, List<ModelConfigSPD>>>(LoadModel(protocol3), (JsonSerializerOptions?)options);
	}

	public static List<ModelConfigEDL> LoadEDLModel(string brand)
	{
		object obj = null;
		if (cacheEdl == obj)
		{
			return new List<ModelConfigEDL>();
		}
		if (cacheEdl.ContainsKey(brand))
		{
			return cacheEdl[brand];
		}
		return new List<ModelConfigEDL>();
	}

	public static List<ModelConfigMTK> LoadMTKModel(string brand)
	{
		object obj = null;
		if (cacheMtk == obj)
		{
			return new List<ModelConfigMTK>();
		}
		if (cacheMtk.ContainsKey(brand))
		{
			return cacheMtk[brand];
		}
		return new List<ModelConfigMTK>();
	}

	public static List<ModelConfigSPD> LoadSPDModel(string brand)
	{
		object obj = null;
		if (cacheSpd != obj)
		{
			if (!cacheSpd.ContainsKey(brand))
			{
				return new List<ModelConfigSPD>(0);
			}
			return cacheSpd[brand];
		}
		return new List<ModelConfigSPD>();
	}

	static Server()
	{
		string text = "";
		string keyfile = "Alpha28January";
		string key = "Q9tHsX4fWbR6pC0aUyM8nKjDzE1vAgBg";
		string iV = "Z3yX7pB2qL1mDeg8";
		string hex = "4461746143656e7465722f";
		amtusername = text;
		amtpassword = text;
		jsonmenemi = string.Empty;
		urlemi = string.Empty;
		Keyfile = keyfile;
		Key = key;
		IV = iV;
		DataCenter = UI.Server + Encoding.UTF8.GetString(hex.HexToBytes());
		http = CreateClient();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
