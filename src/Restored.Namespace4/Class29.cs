using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Restored.Namespace4;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "18.5.0.0")]
internal sealed class Class29 : ApplicationSettingsBase
{
	private static Class29 field_01 = (Class29)SettingsBase.Synchronized(new Class29());

	public static Class29 Default => field_01;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Email
	{
		get
		{
			string propertyName = "Email";
			return (string)this[propertyName];
		}
		set
		{
			string propertyName = "Email";
			this[propertyName] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Password
	{
		get
		{
			string propertyName = "Password";
			return (string)this[propertyName];
		}
		set
		{
			string propertyName = "Password";
			this[propertyName] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool Remember
	{
		get
		{
			string propertyName = "Remember";
			return (bool)this[propertyName];
		}
		set
		{
			string propertyName = "Remember";
			this[propertyName] = value;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
