using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AMFTool.Library.Fastboot;
using AMFTool.Library.Mediatek;
using AMFTool.Library.Odin;
using AMFTool.Library.Odin.Tar;
using AMFTool.Library.Qualcomm;
using AMFTool.Library.Usb;
using AMFTool.Properties;
using AMFTool.ThreadWorker;
using DevExpress.Utils;
using DevExpress.Utils.Behaviors;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTab;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace AMFTool;

public class Main : XtraForm
{
	[CompilerGenerated]
	private static class _003C_003Eo__37
	{
		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

		public static CallSite<Action<CallSite, ComboBoxItemCollection, object>> _003C_003Ep__1;

		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__2;

		public static CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__3;

		public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__4;

		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__5;

		public static CallSite<Func<CallSite, object, string>> _003C_003Ep__6;
	}

	[CompilerGenerated]
	private static class _003C_003Eo__88
	{
		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

		public static CallSite<Action<CallSite, ComboBoxItemCollection, object>> _003C_003Ep__1;

		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__2;

		public static CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__3;

		public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__4;

		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__5;

		public static CallSite<Func<CallSite, object, string>> _003C_003Ep__6;
	}

	[CompilerGenerated]
	private static class _003C_003Eo__89
	{
		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

		public static CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__1;

		public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

		public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;

		public static CallSite<Func<CallSite, object, string>> _003C_003Ep__4;
	}

	[CompilerGenerated]
	private sealed class _003CRunOperation_003Ed__81 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public Main _003C_003E4__this;

		private Exception _003Cex_003E5__1;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = 0;
			int num2 = -2;
			int num3 = _003C_003E1__state;
			try
			{
				if (num3 != 0)
				{
					UI.ClearLog();
					_003C_003E4__this.Init();
					JobStatus.Init();
					UI.watch.Reset();
					UI.watch.Start();
					JobStatus.Busy = true;
				}
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num3 != 0)
					{
						awaiter = Task.Run(() => _003C_003E4__this.operationsuccess = _003C_003E4__this.thmain.Init(), JobStatus.Token).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num3 = 0;
							_003C_003E1__state = 0;
							_003C_003Eu__1 = awaiter;
							_003CRunOperation_003Ed__81 stateMachine = this;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num3 = -1;
						_003C_003E1__state = -1;
					}
					awaiter.GetResult();
				}
				catch (Exception ex)
				{
					_003Cex_003E5__1 = ex;
					Console.WriteLine(_003Cex_003E5__1.Message);
				}
				finally
				{
					if (num3 < num)
					{
						JobStatus.Busy = (byte)num != 0;
						METAConfig.METAReady = (byte)num != 0;
						_003C_003E4__this.ElapsedTime(UI.watch);
					}
				}
			}
			catch (Exception ex)
			{
				_003C_003E1__state = num2;
				_003C_003Et__builder.SetException(ex);
				return;
			}
			_003C_003E1__state = num2;
			_003C_003Et__builder.SetResult();
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	[CompilerGenerated]
	private sealed class _003CRunReboot_003Ed__82 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public Main _003C_003E4__this;

		private Exception _003Cex_003E5__1;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			int num = 0;
			int num2 = -2;
			int num3 = _003C_003E1__state;
			try
			{
				if (num3 != 0)
				{
					JobStatus.Init();
					JobStatus.Busy = true;
					DeviceState.Reboot = true;
				}
				try
				{
					TaskAwaiter<bool> awaiter;
					if (num3 != 0)
					{
						awaiter = Task.Run(() => _003C_003E4__this.operationsuccess = _003C_003E4__this.thmain.Init(), JobStatus.Token).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num3 = 0;
							_003C_003E1__state = 0;
							_003C_003Eu__1 = awaiter;
							_003CRunReboot_003Ed__82 stateMachine = this;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num3 = -1;
						_003C_003E1__state = -1;
					}
					awaiter.GetResult();
				}
				catch (Exception ex)
				{
					_003Cex_003E5__1 = ex;
					Console.WriteLine(_003Cex_003E5__1.Message);
				}
				finally
				{
					if (num3 < num)
					{
						JobStatus.Busy = (byte)num != 0;
					}
				}
			}
			catch (Exception ex)
			{
				_003C_003E1__state = num2;
				_003C_003Et__builder.SetException(ex);
				return;
			}
			_003C_003E1__state = num2;
			_003C_003Et__builder.SetResult();
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private List<Server.ModelConfigMTK> ListModelMTK;

	private List<Server.ModelConfigEDL> ListModelEDL;

	private List<Server.ModelConfigSPD> ListModelSPD;

	private UsbWatcher watcher;

	public string Versiontool;

	private bool operationsuccess;

	public RichTextBox RichTextBoxJSON;

	private ThreadMain thmain;

	private bool sideBar_Expand;

	public List<clsIcons> list_pldr_emi;

	public static Main Instance;

	private readonly Dictionary<string, Job> Operation;

	private IContainer components;

	private RichTextBox RLog;

	private PanelControl PLog;

	private SimpleButton STOP;

	private ProgressBarControl PBar;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

	private DataGridViewTextBoxColumn Column6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn39;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column5;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewTextBoxColumn Column4;

	private XtraTabControl TabMenu;

	private XtraTabPage TabEDL;

	private XtraTabPage TabMTK;

	private XtraTabControl TabSubMTK;

	private XtraTabPage TabMTKFlashUnlock;

	private SimpleButton MTKSafeFormat;

	public CheckEdit CMTKServer;

	private ComboBoxEdit MTKModel;

	private ComboBoxEdit MTKBrand;

	private LabelControl LMTKBrand;

	private LabelControl LMTKModel;

	private SimpleButton MTKReadback;

	private SimpleButton MTKPatchVB;

	private SimpleButton MTKOrangestate;

	private SimpleButton MTKDMVerity;

	private SimpleButton MTKLBL;

	private SimpleButton MTKUBL;

	private SimpleButton MTKIMEIRepair;

	private SimpleButton MTKResetFrp;

	private SimpleButton MTKResetLock;

	private SimpleButton MTKPayjoy;

	private SimpleButton MTKSecPlugin;

	private SimpleButton MTKHuaweiID;

	private SimpleButton MTKOPPOID;

	private SimpleButton MTKResetMICloud;

	private SimpleButton MTKDisableMiCloud;

	private SimpleButton MTKRPMBRestore;

	private SimpleButton MTKRPMBBackup;

	private SimpleButton MTKRPMBErase;

	private SimpleButton MTKNVRestore;

	private SimpleButton MTKNVBackup;

	private SimpleButton MTKNVErase;

	public CheckEdit CMTKDA;

	public CheckEdit CMTKEMI;

	public ComboBoxEdit MTKFlashMode;

	private LabelControl LMTKDA;

	private TextEdit MTKDA;

	private LabelControl LMTKEMI;

	private TextEdit MTKEMI;

	public TextEdit MTKFirmware;

	private LabelControl LMTKFirmware;

	public CheckEdit MTKRepartition;

	public CheckEdit Highspeed;

	private SimpleButton MTKFlash;

	private SimpleButton MTKReboot;

	private SimpleButton MTKReadGPT;

	private XtraTabPage TabHisi;

	private XtraTabPage TabSpd;

	private XtraTabPage TabSamsung;

	private XtraTabPage TabAdbFastboot;

	private XtraTabControl TabSubEDL;

	private XtraTabPage TabEDLFlashing;

	private XtraTabPage TabEDLNetwork;

	private PanelControl PBUEDL;

	private ComboBoxEdit BrandUEDL;

	private ListBoxControl LMEDL;

	private SimpleButton EDLFlash;

	private SimpleButton EDLReboot;

	private SimpleButton EDLReadGPT;

	private SimpleButton EDLReadBack;

	private SimpleButton EDLResetFrp;

	private SimpleButton EDLResetLock;

	private SimpleButton EDLMIReset;

	private SimpleButton EDLMIDisable;

	private SimpleButton EDLEFSRestore;

	private SimpleButton EDLEFSBackup;

	private SimpleButton EDLEFSErase;

	private SimpleButton EDLLoadLoader;

	private SimpleButton EDLClearLoader;

	private LabelControl LEDLLoader;

	public TextEdit EDLLoader;

	public ComboBoxEdit EDLStorageMode;

	private SimpleButton EDLLoadFirmware;

	public TextEdit EDLFirmware;

	private LabelControl LEDLFirmware;

	private SimpleButton EDLLoadPatch;

	private SimpleButton EDLClearPatch;

	private LabelControl LEDLPatch;

	public TextEdit EDLPatch;

	public CheckEdit CEDLServer;

	private ComboBoxEdit EDLModel;

	private ComboBoxEdit EDLBrand;

	private LabelControl LEDLBrand;

	private LabelControl LEDLModel;

	private GroupControl groupControl2;

	private SimpleButton DiagEnableXiaomi;

	private SimpleButton DiagEnable;

	private SimpleButton DiagEnableRoot;

	private SimpleButton DiagCriticalWrite;

	private SimpleButton DiagCriticalRead;

	private SimpleButton DiagQCNWrite;

	private SimpleButton DiagQCNRead;

	private SimpleButton DiagWriteIMEI;

	private SimpleButton HISIBLUnlock;

	private SimpleButton HISIResetFrp;

	private SimpleButton HISIResetlock;

	private SimpleButton HISIResetID;

	private ComboBoxEdit CCPUHisi;

	private LabelControl LHisiCPU;

	private XtraTabControl TabSubSpreadtrum;

	private XtraTabPage TabSpdFlash;

	private SimpleButton SPDResetFrp;

	private SimpleButton SPDResetLock;

	private LabelControl LSPDCPU;

	private ComboBoxEdit CBrandSPD;

	private SimpleButton SPDReboot;

	private SimpleButton SPDReadGPT;

	private XtraTabControl TabSubSamsung;

	private XtraTabPage TabSamMTP;

	private XtraTabPage TabSamsOdin;

	private XtraTabPage xtraTabPage2;

	private GroupControl GADBMode;

	private SimpleButton ADBSamFrp;

	private SimpleButton ADBSamId;

	private GroupControl GMTPMode;

	private SimpleButton MTPRebootOdin;

	private SimpleButton MTPSN;

	private SimpleButton MTPReset;

	private SimpleButton MTPFrp16;

	private SimpleButton MTPFrp;

	private SimpleButton MTPInfo;

	private GroupControl groupControl1;

	private SimpleButton OdinInfo;

	private SimpleButton OdinReboot;

	private SimpleButton OdinSoftbrick;

	private SimpleButton OdinFrp;

	private SimpleButton OdinReset;

	private SimpleButton OdinWritePit;

	private SimpleButton OdinReadPit;

	private SimpleButton ClearOdinFirmware;

	public CheckEdit COdinEraseEFS;

	public CheckEdit COdinRepartition;

	private SimpleButton OdinFlash;

	private SimpleButton LoadHome;

	private SimpleButton LoadCSC;

	private SimpleButton LoadCP;

	private SimpleButton LoadAP;

	private SimpleButton LoadBL;

	public TextEdit txtHOME;

	public TextEdit txtCSC;

	public TextEdit txtCP;

	public TextEdit txtAP;

	public TextEdit txtBL;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn55;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn56;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn57;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn58;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn59;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn60;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn61;

	public ComboBoxEdit ComboPort;

	private Panel panel1;

	private Panel panel3;

	private Label label1;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage3;

	private SimpleButton ADBEnableApk;

	private SimpleButton ADBInstallApk;

	private SimpleButton ADBUninstallApk;

	private SimpleButton ADBDisableApk;

	private SimpleButton ADBLoadApk;

	private XtraTabPage xtraTabPage4;

	private SimpleButton FBUnlockBL;

	private SimpleButton FBRebootRecovery;

	private SimpleButton FBRebootEDL;

	private SimpleButton FBReadGPT;

	private SimpleButton FBSlotA;

	private SimpleButton FBEFSErase;

	private SimpleButton FBReboot;

	private SimpleButton FBResetFrp;

	private SimpleButton FastbootClearFirmware;

	private LabelControl labelControl1;

	private SimpleButton FBFlash;

	private SimpleButton FastbootLoadFirmware;

	private TextEdit FastbootFirmware;

	private XtraTabPage TabMeta;

	private XtraTabPage TabNVEditor;

	private SimpleButton METAWriteIMEI;

	private SimpleButton METADisableADB;

	private SimpleButton METAEnableADB;

	private SimpleButton METASecplugin;

	private SimpleButton METAPayjoy;

	private SimpleButton METAFrp;

	private SimpleButton METAReset;

	private SimpleButton METAReboot;

	private SimpleButton METAReadGPT;

	private SimpleButton NVDataEdit;

	private SimpleButton NVRAMEdit;

	private SimpleButton MTKRemoveDemo;

	public DataGridView MTKDataGrid;

	private CheckBox CMTKPart;

	private Button MTKClearDA;

	private Button MTKClearEMI;

	private Button MTKLoadEMI;

	private Button MTKLoadDA;

	private Button MTKLoadFirmware;

	private SimpleButton DidableMicloud;

	private SimpleButton SamsungID;

	private SimpleButton EraseFRP16;

	private SimpleButton EraseFRP;

	private SimpleButton Disable_osvivo;

	private SimpleButton EnablediagXiaomi;

	private SimpleButton FixAssistant;

	private SimpleButton FixAssistantWIFI;

	private SimpleButton Remove_Payjoy;

	private SimpleButton Reset_lock;

	private SimpleButton EnableDiagRoot;

	private SimpleButton EnableDiagOperation;

	private SimpleButton DisablePengecualian;

	private SimpleButton Disable_OS_Sam;

	private SimpleButton Disable_OSMI;

	private SimpleButton Disaba_OSOppo;

	private SimpleButton rebootdevice;

	private SimpleButton RemovePlugin;

	private SimpleButton ResetHuaweiID;

	private SimpleButton DisabaleOSRealme;

	private SimpleButton DisableDemo;

	private SimpleButton DisableOSItel;

	private SimpleButton DisableOSTecno;

	private SimpleButton Disable_OSOnePlus;

	private SimpleButton RemoveMDM;

	private SimpleButton OppoSwith;

	private SimpleButton SamsungAccount;

	private SimpleButton DisableKnox;

	private SimpleButton ADBDisableHU;

	private SimpleButton RemoveDemo;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private ComboBoxEdit DaCustum;

	public CheckEdit CarbonaraExploit;

	private LabelControl labelControl3;

	private ComboBoxEdit CModelSPD;

	public CheckEdit ShowALLMTK;

	private SimpleButton MTKReadFlash;

	private SimpleButton MTKReadDUMP;

	private SimpleButton MTKWiteemi;

	private SimpleButton simpleButton14;

	private SimpleButton RebuildUserDATA;

	public CheckEdit InfoFW;

	public CheckEdit checkEdit2;

	public CheckEdit Skippacth;

	private CheckBox CEDLPart;

	public DataGridView EDLDataGrid;

	private CheckBox CFastbootPart;

	public DataGridView FastbootDataGrid;

	public CheckEdit EDLinfofirmware;

	private TextEdit txtSearch;

	private CheckBox CAndroidPart;

	public DataGridView AndroidDataGrid;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn13;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;

	public CheckEdit FBinfofir;

	private LabelControl labelControl5;

	private LabelControl PanelEMAIL;

	private LabelControl expred;

	private LabelControl Expired;

	private PanelControl panelControl2;

	private BehaviorManager behaviorManager1;

	private Panel panel2;

	private SimpleButton simpleButton4;

	private System.Windows.Forms.Timer Timer_Sidebar_Menu;

	private PanelControl SideBar;

	private SimpleButton NVModule;

	private SimpleButton AndroidModule;

	private SimpleButton HisiModule;

	private SimpleButton METAModule;

	private SimpleButton EDLModule;

	private SimpleButton MTKModule;

	private SimpleButton UnisocModule;

	private SimpleButton SamsungModule;

	public CheckEdit ShowALLqc;

	private SimpleButton QCOppoID;

	private SearchControl SearchMEDL;

	private SimpleButton FRPRemove16;

	private Button device;

	private XtraTabPage Unlock;

	private Panel panel4;

	private Panel panel5;

	private SimpleButton UnlockUniv;

	private SimpleButton Reset2024;

	private SimpleButton simpleButton1;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;

	private DataGridViewTextBoxColumn Column7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn40;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn69;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;

	private DataGridViewTextBoxColumn Column9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn70;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn71;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn72;

	private DataGridViewTextBoxColumn Column8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn73;

	private XtraTabPage xtraTabPage1;

	private SimpleButton SDresetLock;

	private SimpleButton SDdisabaleMI;

	private SimpleButton SDresetMI;

	private SimpleButton ResetFRPMIasis;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private BarButtonItem MTKWrite;

	private BarButtonItem MTKRead;

	private BarButtonItem MTKFormat;

	private BarButtonItem WriteCheck;

	private BarButtonItem FormatCheck;

	private BarButtonItem ReadPartition;

	private Bar bar1;

	private PopupMenu PopMTK;

	private BarButtonItem EDLFormat;

	private BarButtonItem EDLWrite;

	private BarButtonItem EDLRead;

	private PopupMenu PopQc;

	private BarButtonItem FBWrite;

	private BarButtonItem FBFormat;

	private PopupMenu PopFB;

	private BarButtonItem SPDWrite;

	private BarButtonItem SPDFormat;

	private BarButtonItem SPDRead;

	private PopupMenu PopSPD;

	private CheckBox CSPDPart;

	public DataGridView SPDDataGrid;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn12;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn62;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn63;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn64;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn65;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn66;

	private CheckBox CheckMeta;

	public DataGridView METADataGrid;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn10;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn49;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn50;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn51;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn52;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn53;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn54;

	private CheckBox CheckOdin;

	public DataGridView OdinGridView;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn45;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn47;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn48;

	private DataGridViewTextBoxColumn Column10;

	private SimpleButton QCN_Editor;

	private LabelControl LSpeed;

	private LabelControl labelControl6;

	private ComboBoxEdit LayoutStore;

	internal LabelControl LabelControl10;

	private ComboBoxEdit SuperMap;

	private SimpleButton SetSlotB;

	private SimpleButton SetSlotA;

	private XtraTabPage SPDATdiag;

	private GroupControl groupControl3;

	private SimpleButton FactoryReset;

	private SimpleButton FactoryReset2;

	private SimpleButton simpleButton16;

	private SimpleButton MTKResetFrpNew;

	private SimpleButton simpleButton17;

	private SimpleButton simpleButton15;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage Andproid;

	private XtraTabPage Transsion;

	private GroupControl groupControl4;

	private SimpleButton simpleButton2;

	private SimpleButton SafeFormat;

	private SimpleButton metaFactoryReset;

	public Main(string email, string password, string hwid, string expired, string Version)
	{
		string key = "Reset Lock";
		string key2 = "Reset FRP";
		string key3 = "Backup EFS";
		string key4 = "Erase EFS";
		string key5 = "Restore EFS";
		string key6 = "Disable MiAccount";
		string key7 = "Reset MiAccount";
		object obj = null;
		string text = "AMFTool ";
		string text2 = " | ";
		string text3 = " | ID : ";
		double field_ = Class38.field_543;
		string text4 = "Your account is expired!";
		string text5 = "dd";
		string text6 = " Days";
		string text7 = " (";
		string text8 = ")";
		RichTextBoxJSON = new RichTextBox();
		sideBar_Expand = true;
		list_pldr_emi = new List<clsIcons>();
		Operation = new Dictionary<string, Job>
		{
			{
				key,
				Job.Reset
			},
			{
				key2,
				Job.Frp
			},
			{
				key3,
				Job.NVBackup
			},
			{
				key4,
				(Job)42
			},
			{
				key5,
				(Job)40
			},
			{
				key6,
				(Job)56
			},
			{
				key7,
				(Job)58
			}
		};
		components = (IContainer)obj;

		Instance = this;
		InitializeComponent();
		Initializingother();
		Versiontool = Version;
		Text = text + Version + text2 + UI.Server + text3 + hwid;
		if (string.IsNullOrEmpty(expired))
		{
			string text9 = text4;
			PanelEMAIL.Text = email + text7 + text9 + text8;
			Expired.Text = expired;
			CMTKServer.Checked = true;
			Timer_Sidebar_Menu.Start();
			base.Load += Main_Load;
			return;
		}
		TimeSpan timeSpan = DateTime.Parse(expired).Subtract(DateTime.Now);
		if (!(timeSpan.TotalSeconds <= field_))
		{
			string text10 = timeSpan.ToString(text5);
			string text9 = text10 + text6;
			PanelEMAIL.Text = email + text7 + text9 + text8;
			Expired.Text = expired;
			CMTKServer.Checked = true;
			Timer_Sidebar_Menu.Start();
			base.Load += Main_Load;
		}
		else
		{
			string text9 = text4;
			PanelEMAIL.Text = email + text7 + text9 + text8;
			Expired.Text = expired;
			CMTKServer.Checked = true;
			Timer_Sidebar_Menu.Start();
			base.Load += Main_Load;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		watcher = new UsbWatcher(method_02());
	}

	private void Initializingother()
	{
		UI.Richtextbox = RLog;
		UI.Progressbar = PBar;
		UI.LabelSpeed = LSpeed;
		thmain = new ThreadMain();
	}

	private void Main_Load(object sender, EventArgs e)
	{
		string item = "Auto";
		string item2 = "Oneplus";
		string item3 = "Oppo";
		string item4 = "Realme";
		string item5 = "Samsung";
		string item6 = "Xiaomi";
		string item7 = "OPLUS VIP";
		string item8 = "Asus";
		string item9 = "Huawei";
		string item10 = "Lenovo";
		string item11 = "Meizu";
		string item12 = "Nokia";
		string item13 = "Vivo";
		EDLBrand.Properties.Items.Add(item);
		EDLBrand.Properties.Items.Add(item2);
		EDLBrand.Properties.Items.Add(item3);
		EDLBrand.Properties.Items.Add(item4);
		EDLBrand.Properties.Items.Add(item5);
		EDLBrand.Properties.Items.Add(item6);
		EDLBrand.Properties.Items.Add(item7);
		EDLBrand.Properties.Items.Add(item8);
		EDLBrand.Properties.Items.Add(item9);
		EDLBrand.Properties.Items.Add(item10);
		EDLBrand.Properties.Items.Add(item11);
		EDLBrand.Properties.Items.Add(item12);
		EDLModel.Properties.Items.Add(item);
		EDLBrand.Properties.Items.Add(item13);
		EDLBrand.SelectedIndex = 0;
		EDLModel.SelectedIndex = 0;
		TabMenu.SelectedTabPageIndex = 0;
		UI.LoadFont();
		Server.LoadAllModel();
		UsbMonitor.GetComInfo();
	}

	private void AddButton(PanelControl panel, string text)
	{
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string objB = "Reset Lock";
		string objB2 = "Reset FRP";
		string objB3 = "Backup EFS";
		string objB4 = "Erase EFS";
		string objB5 = "Restore EFS";
		string objB6 = "Disable MiAccount";
		string objB7 = "Reset MiAccount";
		string objB8 = "Reset EFS Method 2";
		string objB9 = "Safe Format";
		string objB10 = "Unlock Bootloader";
		int count = panel.Controls.Count;
		int y = 10 + count * 32;
		using StringReader stringReader = new StringReader(text);
		while (stringReader.Peek() != -1)
		{
			text = stringReader.ReadLine();
			if (object.Equals(text, string.Empty))
			{
				continue;
			}
			SimpleButton simpleButton = new SimpleButton();
			simpleButton.Appearance.Font = new Font(familyName, field_);
			simpleButton.Appearance.Options.UseFont = true;
			simpleButton.Appearance.ForeColor = Color.FromArgb(64, 64, 64);
			simpleButton.Appearance.Options.UseFont = true;
			simpleButton.Appearance.Options.UseForeColor = true;
			simpleButton.AllowFocus = false;
			if (!object.Equals(text, objB))
			{
				if (object.Equals(text, objB2))
				{
					simpleButton.ImageOptions.Image = Resources.FRP;
				}
				else if (!object.Equals(text, objB3))
				{
					if (!object.Equals(text, objB4))
					{
						if (object.Equals(text, objB5))
						{
							simpleButton.ImageOptions.Image = Resources.WRITE;
						}
						else if (!object.Equals(text, objB6))
						{
							if (!object.Equals(text, objB7))
							{
								if (!object.Equals(text, objB8))
								{
									if (!object.Equals(text, objB9))
									{
										if (!object.Equals(text, objB10))
										{
											simpleButton.ImageOptions.Image = Resources.warning;
										}
										else
										{
											simpleButton.ImageOptions.Image = Resources.unlock;
										}
									}
									else
									{
										simpleButton.ImageOptions.Image = Resources.safeformat;
									}
								}
								else
								{
									simpleButton.ImageOptions.Image = Resources.DATAERASE;
								}
							}
							else
							{
								simpleButton.ImageOptions.Image = Resources.reset_mcloud;
							}
						}
						else
						{
							simpleButton.ImageOptions.Image = Resources.disable_micloud;
						}
					}
					else
					{
						simpleButton.ImageOptions.Image = Resources.DATAERASE;
					}
				}
				else
				{
					simpleButton.ImageOptions.Image = Resources.backup;
				}
			}
			else
			{
				simpleButton.ImageOptions.Image = Resources.FACTORY;
			}
			simpleButton.ImageOptions.ImageToTextAlignment = ImageAlignToText.LeftCenter;
			simpleButton.Location = new Point(17, y);
			simpleButton.ShowFocusRectangle = DefaultBoolean.False;
			simpleButton.Text = text;
			panel.Controls.Add(simpleButton);
			simpleButton.Size = new Size(panel.Size.Width - 33, 27);
			simpleButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			simpleButton.Click += delegate
			{
				UnlockHandler(text);
			};
			panel.Controls.Add(simpleButton);
		}
	}

	private void UnlockHandler(string operation)
	{
		object obj = null;
		string filter = "Backup EFS|*.AMF";
		string title = "Load EFS Backup";
		int unlock = 1;
		int mode = 3;
		if (JobStatus.Busy)
		{
			return;
		}
		List<Server.ModelConfigEDL> listModelEDL = ListModelEDL;
		Server.ModelConfigEDL modelConfigEDL = (Server.ModelConfigEDL)((listModelEDL != null) ? listModelEDL.FirstOrDefault((Server.ModelConfigEDL x) => x.Model == (string)LMEDL.SelectedItem) : obj);
		if (modelConfigEDL == obj)
		{
			return;
		}
		List<string> operation2 = modelConfigEDL.Operation;
		if (operation2.Count != 0)
		{
			State.Job = Job.Xml;
		}
		else
		{
			if (!Operation.TryGetValue(operation, out var value))
			{
				return;
			}
			State.Job = value;
		}
		if (State.Job == (Job)40)
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = filter,
				Title = title,
				InitialDirectory = AppContext.BaseDirectory
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			State.FilePath = openFileDialog.FileName;
		}
		DeviceState.Unlock = (byte)unlock != 0;
		State.Operation = operation;
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void BrandUEDL_SelectedIndexChanged(object sender, EventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		ListModelEDL = Server.LoadEDLModel((string)BrandUEDL.SelectedItem);
		if (ListModelEDL.Count != 0)
		{
			LMEDL.DataSource = ListModelEDL.Select((Server.ModelConfigEDL m) => m.Model).ToArray();
		}
	}

	private void LMEDL_SelectedIndexChanged(object sender, EventArgs e)
	{
		object obj = null;
		string text = "Xiaomi";
		string text2 = "Samsung";
		string text3 = "Reset Lock";
		string text4 = "Reset FRP";
		string text5 = "Backup EFS";
		string text6 = "Erase EFS";
		string text7 = "Restore EFS";
		string text8 = "Disable MiAccount";
		string text9 = "Reset MiAccount";
		string text10 = "reset";
		string text11 = "frp";
		string text12 = "efs";
		string text13 = "efs2";
		string text14 = "safe";
		string text15 = "ubl";
		string text16 = "Reset EFS";
		string text17 = "Reset EFS Method 2";
		string text18 = "Safe Format";
		string text19 = "Unlock Bootloader";
		if (ListModelEDL == null || ListModelEDL.Count == 0)
		{
			return;
		}
		List<Server.ModelConfigEDL> listModelEDL = ListModelEDL;
		Server.ModelConfigEDL modelConfigEDL = (Server.ModelConfigEDL)((listModelEDL != null) ? listModelEDL.FirstOrDefault((Server.ModelConfigEDL x) => x.Model == (string)LMEDL.SelectedItem) : obj);
		if (modelConfigEDL == obj)
		{
			return;
		}
		PBUEDL.Controls.Clear();
		List<string> operation = modelConfigEDL.Operation;
		if (operation.Count != 0)
		{
			{
				foreach (string item in operation)
				{
					string text20 = ((item == text10) ? text3 : ((item == text11) ? text4 : ((item == text12) ? text16 : ((item == text13) ? text17 : ((item == text14) ? text18 : ((!(item == text15)) ? item.ToUpper() : text19))))));
					string text21 = text20;
					AddButton(PBUEDL, text21);
				}
				return;
			}
		}
		string text22 = (string)BrandUEDL.SelectedItem;
		string text23 = text22;
		if (!(text23 == text))
		{
			if (!(text23 == text2))
			{
				AddButton(PBUEDL, text3);
				AddButton(PBUEDL, text4);
				AddButton(PBUEDL, text5);
				AddButton(PBUEDL, text6);
				AddButton(PBUEDL, text7);
			}
			else
			{
				AddButton(PBUEDL, text3);
				AddButton(PBUEDL, text4);
			}
		}
		else
		{
			AddButton(PBUEDL, text3);
			AddButton(PBUEDL, text4);
			AddButton(PBUEDL, text5);
			AddButton(PBUEDL, text6);
			AddButton(PBUEDL, text7);
			AddButton(PBUEDL, text8);
			AddButton(PBUEDL, text9);
		}
	}

	private void SearchMEDL_EditValueChanged(object sender, EventArgs e)
	{
		if (ListModelEDL == null || ListModelEDL.Count == 0)
		{
			return;
		}
		string key = SearchMEDL.Text.Trim().ToLower();
		if (key.Length == 0)
		{
			LMEDL.DataSource = ListModelEDL.Select((Server.ModelConfigEDL x) => x.Model).ToArray();
			return;
		}
		string[] dataSource = (from m in ListModelEDL
			where m.Model.ToLower().Contains(key)
			select m.Model).ToArray();
		LMEDL.DataSource = dataSource;
	}

	private void CEDLPart_CheckedChanged(object sender, EventArgs e)
	{
		if (CEDLPart.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)EDLDataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)EDLDataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void CEDLServer_CheckedChanged(object sender, EventArgs e)
	{
		string item = "Auto";
		string item2 = "Oneplus";
		string item3 = "Oppo";
		string item4 = "Vivo";
		string item5 = "Realme";
		string item6 = "Samsung";
		string item7 = "Xiaomi";
		string item8 = "OPLUS VIP";
		string item9 = "Asus";
		string item10 = "Huawei";
		string item11 = "Lenovo";
		string item12 = "Meizu";
		string item13 = "Nokia";
		if (!CEDLServer.Checked)
		{
			EDLConfig.EDLServer = false;
			EDLModel.Text = string.Empty;
			EDLBrand.Text = string.Empty;
			EDLBrand.Properties.Items.Clear();
			EDLModel.Properties.Items.Clear();
			return;
		}
		EDLConfig.EDLServer = true;
		EDLBrand.Properties.Items.Add(item);
		EDLModel.Properties.Items.Add(item);
		EDLBrand.Properties.Items.Add(item2);
		EDLBrand.Properties.Items.Add(item3);
		EDLBrand.Properties.Items.Add(item4);
		EDLBrand.Properties.Items.Add(item5);
		EDLBrand.Properties.Items.Add(item6);
		EDLBrand.Properties.Items.Add(item7);
		EDLBrand.Properties.Items.Add(item8);
		EDLBrand.Properties.Items.Add(item9);
		EDLBrand.Properties.Items.Add(item10);
		EDLBrand.Properties.Items.Add(item11);
		EDLBrand.Properties.Items.Add(item12);
		EDLBrand.Properties.Items.Add(item13);
		EDLBrand.SelectedIndex = 0;
		EDLModel.SelectedIndex = 0;
	}

	private void EDLBrand_SelectedIndexChanged(object sender, EventArgs e)
	{
		ListModelEDL = Server.LoadEDLModel((string)EDLBrand.SelectedItem);
		if (ListModelEDL.Count != 0)
		{
			EDLModel.Properties.Items.Clear();
			ComboBoxItemCollection items = EDLModel.Properties.Items;
			object[] items2 = ListModelEDL.Select((Server.ModelConfigEDL m) => m.Model).ToArray();
			items.AddRange(items2);
			EDLModel.SelectedIndex = 0;
		}
	}

	private void EDLStorageMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		string storage = EDLStorageMode.Text.Trim().ToLower();
		EDLConfig.Storage = storage;
	}

	private void EDLDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == EDLDataGrid.Columns[0].Index)
		{
			EDLDataGrid.EndEdit();
		}
	}

	private void EDLDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!JobStatus.Busy && e.Button == MouseButtons.Right && e.RowIndex >= 0)
		{
			EDLDataGrid.ClearSelection();
			EDLDataGrid.Rows[e.RowIndex].Selected = true;
			EDLDataGrid.CurrentCell = EDLDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
			PopQc.ShowPopup(Cursor.Position);
		}
	}

	private void EDLOption(object sender, ItemClickEventArgs e)
	{
		object obj = null;
		string text = "";
		string text2 = "EDLWrite";
		string text3 = "EDLRead";
		string text4 = "EDLFormat";
		int mode = 3;
		string text5 = "Select file want to write to ";
		string filter = "All Files|*.*";
		int num = 1;
		int num2 = 0;
		int job = 8;
		string text6 = "Save ";
		string text7 = ".img";
		int job2 = 9;
		int job3 = 11;
		if (JobStatus.Busy)
		{
			return;
		}
		BarItem item = e.Item;
		if (item == obj)
		{
			return;
		}
		string text8 = text;
		PopQc.HidePopup();
		string name = item.Name;
		string text9 = name;
		if (!(text9 == text2))
		{
			if (!(text9 == text3))
			{
				if (text9 == text4)
				{
					foreach (DataGridViewRow selectedRow in EDLDataGrid.SelectedRows)
					{
						DeviceState.Partitiontarget.Add((string)selectedRow.Cells[3].Value);
					}
					State.Job = (Job)job3;
				}
			}
			else
			{
				foreach (DataGridViewRow selectedRow2 in EDLDataGrid.SelectedRows)
				{
					DeviceState.Partitiontarget.Add((string)selectedRow2.Cells[3].Value);
					text8 = (string)selectedRow2.Cells[3].Value;
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Title = text6 + text8,
					Filter = filter,
					FileName = text8 + text7
				})
				{
					if (((saveFileDialog.ShowDialog() == (DialogResult)num) ? 1 : 0) == num2)
					{
						return;
					}
					State.SavePath = saveFileDialog.FileName;
				}
				State.Job = (Job)job2;
			}
		}
		else
		{
			foreach (DataGridViewRow selectedRow3 in EDLDataGrid.SelectedRows)
			{
				DeviceState.Partitiontarget.Add((string)selectedRow3.Cells[3].Value);
				text8 = (string)selectedRow3.Cells[3].Value;
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = text5 + text8,
				Filter = filter
			})
			{
				if (((openFileDialog.ShowDialog() == (DialogResult)num) ? 1 : 0) == num2)
				{
					return;
				}
				State.FilePath = openFileDialog.FileName;
			}
			State.Job = (Job)job;
		}
		State.Operation = item.ToString();
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void EDLLoad(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 1;
		string text = "EDLLoadFirmware";
		string text2 = "EDLLoadPatch";
		string text3 = "EDLLoadLoader";
		string description = "Select Firmware Folder";
		string searchPattern = "*.xml";
		string header = "Select RawProgram";
		int num3 = 4;
		int num4 = 2;
		int num5 = 3;
		string text4 = "Select Patch";
		string searchPattern2 = "patch*.xml";
		int count = 5;
		string filter = "patch (*.xml)|*.xml|All Files (*.*)|*.*";
		string title = "Select Loader";
		string filter2 = "Firehose Loader (*.mbn;*.bin;*.elf)|*.mbn;*.bin;*.elf|All Files (*.*)|*.*";
		CEDLPart.Checked = false;
		EDLDataGrid.Rows.Clear();
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			FilterIndex = 1,
			RestoreDirectory = true,
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
		};
		string name = simpleButton.Name;
		string text5 = name;
		if (text5 == text)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				Description = description,
				ShowNewFolderButton = false
			})
			{
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					string text6 = (EDLConfig.PathFirmware = folderBrowserDialog.SelectedPath);
					DirectoryInfo directoryInfo = new DirectoryInfo(text6);
					List<FileInfo> list = directoryInfo.EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly).Where(delegate(FileInfo f)
					{
						string value = "raw";
						return f.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
					}).ToList();
					Load load = new Load(Array.Empty<object>(), header);
					foreach (FileInfo item in list)
					{
						load.AddRawProgram(new object[4]
						{
							false,
							Path.GetFileNameWithoutExtension(item.Name),
							item.Name,
							item.FullName
						});
					}
					load.ShowDialog();
					Load load2 = new Load(Array.Empty<object>(), text4);
					List<FileInfo> list2 = directoryInfo.EnumerateFiles(searchPattern2, (SearchOption)num).Take(count).ToList();
					foreach (FileInfo item2 in list2)
					{
						object[] array = new object[num3];
						array[num] = (byte)num != 0;
						array[num2] = Path.GetFileNameWithoutExtension(item2.Name);
						array[num4] = item2.Name;
						array[num5] = item2.FullName;
						load2.AddPatch(array);
					}
					if (list2.Count > num)
					{
						load2.ShowDialog();
					}
					CEDLPart.Checked = (byte)num2 != 0;
					EDLFirmware.Text = text6;
					if (list2.Count > num)
					{
						EDLPatch.Text = text6;
					}
				}
				return;
			}
		}
		if (!(text5 == text2))
		{
			if (text5 == text3)
			{
				openFileDialog.Title = title;
				openFileDialog.Filter = filter2;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					EDLConfig.PathLoader = openFileDialog.FileName;
					EDLLoader.Text = EDLConfig.PathLoader;
				}
			}
		}
		else
		{
			openFileDialog.Title = text4;
			openFileDialog.Filter = filter;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				EDLConfig.PatchXML.Clear();
				EDLConfig.PatchXML.Add(openFileDialog.FileName);
				EDLPatch.Text = openFileDialog.FileName;
			}
		}
	}

	private void EDLClear(object sender, EventArgs e)
	{
		string text = "EDLClearLoader";
		string text2 = "EDLClearPatch";
		SimpleButton simpleButton = sender as SimpleButton;
		if (simpleButton != null && !JobStatus.Busy)
		{
			string name = simpleButton.Name;
			string text3 = name;
			if (text3 == text)
			{
				EDLLoader.Clear();
				EDLConfig.PathLoader = string.Empty;
			}
			else if (text3 == text2)
			{
				EDLPatch.Clear();
				EDLConfig.PatchXML.Clear();
			}
		}
	}

	private void EDLHandler(object sender, EventArgs e)
	{
		string text = "EDLReadGPT";
		string text2 = "EDLReboot";
		string text3 = "EDLFlash";
		string text4 = "EDLEFSErase";
		string text5 = "EDLEFSBackup";
		string text6 = "EDLEFSRestore";
		string text7 = "EDLMIReset";
		string text8 = "EDLMIDisable";
		string text9 = "QCOppoID";
		string text10 = "EDLResetLock";
		string text11 = "EDLResetFrp";
		string text12 = "EDLReadBack";
		string text13 = "SetSlotA";
		string text14 = "SetSlotB";
		int num = 0;
		object obj = null;
		int mode = 3;
		string text15 = "Please select firmware and check partition to flash";
		string caption = "Information";
		int icon = 64;
		string description = "Select Folder";
		string filter = "Backup EFS|*.AMF";
		string title = "Load EFS Backup";
		string text16 = "This Operation only for samsung!, Click Ok to continue procedure";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		QCList.Partition.Clear();
		string name = simpleButton.Name;
		uint num2 = ImplementationDetails.method_01(name);
		Job job;
		if (num2 <= 1688214497)
		{
			if (num2 > 871929887)
			{
				if (num2 <= 1374886522)
				{
					if (num2 != 907415766)
					{
						if (num2 == 1374886522 && name == text3)
						{
							job = Job.Flash;
							goto IL_01e7;
						}
					}
					else if (name == text9)
					{
						job = Job.QCOppoID;
						goto IL_01e7;
					}
				}
				else if (num2 != 1435551905)
				{
					if (num2 == 1688214497 && name == text11)
					{
						job = Job.Frp;
						goto IL_01e7;
					}
				}
				else if (name == text2)
				{
					job = (Job)80;
					goto IL_01e7;
				}
			}
			else if (num2 != 368502170)
			{
				if (num2 != 713852250)
				{
					if (num2 == 871929887 && name == text12)
					{
						job = Job.Readback;
						goto IL_01e7;
					}
				}
				else if (name == text4)
				{
					job = (Job)42;
					goto IL_01e7;
				}
			}
			else if (name == text6)
			{
				job = (Job)40;
				goto IL_01e7;
			}
		}
		else if (num2 <= 3176083384u)
		{
			if (num2 != 1875721981)
			{
				if (num2 != 2313838102u)
				{
					if (num2 == 3176083384u && name == text10)
					{
						job = Job.Reset;
						goto IL_01e7;
					}
				}
				else if (name == text5)
				{
					job = Job.NVBackup;
					goto IL_01e7;
				}
			}
			else if (name == text)
			{
				job = (Job)2;
				goto IL_01e7;
			}
		}
		else if (num2 <= 3882301738u)
		{
			if (num2 != 3865524119u)
			{
				if (num2 == 3882301738u && name == text13)
				{
					job = Job.SetSlotA;
					goto IL_01e7;
				}
			}
			else if (name == text14)
			{
				job = (Job)60;
				goto IL_01e7;
			}
		}
		else if (num2 != 4095305538u)
		{
			if (num2 == 4281238429u && name == text7)
			{
				job = (Job)58;
				goto IL_01e7;
			}
		}
		else if (name == text8)
		{
			job = (Job)56;
			goto IL_01e7;
		}
		job = (Job)0;
		goto IL_01e7;
		IL_01e7:
		State.Job = job;
		DeviceState.Unlock = false;
		if (State.Job != (Job)2)
		{
			if (State.Job == Job.Flash)
			{
				bool flag = default(bool);
				foreach (DataGridViewRow item in (IEnumerable)EDLDataGrid.Rows)
				{
					object value = item.Cells[0].Value;
					int num3;
					if (value is bool)
					{
						flag = (bool)value;
						num3 = 1;
					}
					else
					{
						num3 = 0;
					}
					if (((uint)num3 & (flag ? 1u : 0u)) != 0)
					{
						object value2 = item.Cells[8].Value;
						string text17 = (string)((value2 != null) ? value2.ToString() : obj);
						if (!string.IsNullOrEmpty(text17) && File.Exists(text17))
						{
							List<QCList> partition = QCList.Partition;
							object value3 = item.Cells[2].Value;
							object sectorSize = ((value3 != null) ? value3.ToString() : obj);
							object value4 = item.Cells[3].Value;
							object label = ((value4 != null) ? value4.ToString() : obj);
							object value5 = item.Cells[6].Value;
							object numPartitionSectors = ((value5 != null) ? value5.ToString() : obj);
							object value6 = item.Cells[1].Value;
							object physicalPartitionNumber = ((value6 != null) ? value6.ToString() : obj);
							object value7 = item.Cells[7].Value;
							object startSector = ((value7 != null) ? value7.ToString() : obj);
							object value8 = item.Cells[5].Value;
							partition.Add(new QCList((string)sectorSize, text17, (string)label, (string)numPartitionSectors, (string)physicalPartitionNumber, (string)startSector, (string)((value8 != null) ? value8.ToString() : obj)));
						}
					}
				}
				if (QCList.Partition.Count == num)
				{
					XtraMessageBox.Show(text15, caption, (MessageBoxButtons)num, (MessageBoxIcon)icon);
					return;
				}
			}
			else if (State.Job == Job.Readback)
			{
				using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
				{
					Description = description,
					ShowNewFolderButton = true
				};
				if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				State.SavePath = folderBrowserDialog.SelectedPath;
			}
			else if (State.Job == (Job)40)
			{
				using OpenFileDialog openFileDialog = new OpenFileDialog
				{
					Filter = filter,
					Title = title,
					InitialDirectory = AppContext.BaseDirectory
				};
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				State.FilePath = openFileDialog.FileName;
			}
			else if (State.Job == (Job)74 || State.Job == Job.KnoxGuardUnload || State.Job == Job.Lostmode || State.Job == (Job)76 || State.Job == Job.SerialWrite)
			{
				DialogResult dialogResult = XtraMessageBox.Show(text16, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
				if (dialogResult != DialogResult.OK)
				{
					return;
				}
				if (State.Job == Job.SerialWrite)
				{
					using SerialNumber serialNumber = UI.Showcust(new SerialNumber());
					if (serialNumber.ShowDialog() != DialogResult.Yes)
					{
						return;
					}
				}
			}
		}
		else
		{
			EDLDataGrid.Rows.Clear();
			CEDLPart.Checked = false;
			EDLFirmware.Clear();
			EDLPatch.Clear();
		}
		State.Mode = (SelectMode)mode;
		State.Operation = simpleButton.Text;
		if (!simpleButton.Name.Contains(text2))
		{
			RunOperation();
		}
		else
		{
			RunReboot();
		}
	}

	private void DiagHandler(object sender, EventArgs e)
	{
		string text = "DiagWriteIMEI";
		string text2 = "DiagHandler";
		string text3 = "DiagQCNRead";
		string text4 = "DiagQCNWrite";
		string text5 = "DiagCriticalRead";
		string text6 = "DiagCriticalWrite";
		string text7 = "DiagEnable";
		string text8 = "QCN_Editor";
		string text9 = "DiagEnableRoot";
		string text10 = "DiagEnableXiaomi";
		string title = "Load Critical Data";
		string filter = "Critical|*.*";
		string title2 = "Load QCN";
		string filter2 = "QCN|*.*";
		string description = "Select Folder";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = (SelectMode)4;
		string name = simpleButton.Name;
		uint num = ImplementationDetails.method_01(name);
		Job job;
		if (num > 2802598509u)
		{
			if (num <= 3170399757u)
			{
				if (num == 2952259259u)
				{
					if (name == text4)
					{
						job = (Job)50;
						goto IL_00b6;
					}
				}
				else if (num == 3170399757u && name == text9)
				{
					job = (Job)64;
					goto IL_00b6;
				}
			}
			else if (num != 3395688877u)
			{
				if (num == 3691601054u)
				{
					if (name == text2)
					{
						job = (Job)52;
						goto IL_00b6;
					}
				}
				else if (num == 3965283244u && name == text10)
				{
					job = (Job)66;
					goto IL_00b6;
				}
			}
			else if (name == text)
			{
				job = Job.ImeiWrite;
				goto IL_00b6;
			}
		}
		else if (num <= 696582435)
		{
			if (num == 311116096)
			{
				if (name == text3)
				{
					job = Job.QCNRead;
					goto IL_00b6;
				}
			}
			else if (num == 696582435 && name == text8)
			{
				job = (Job)50;
				goto IL_00b6;
			}
		}
		else if (num == 1838019919)
		{
			if (name == text5)
			{
				job = (Job)46;
				goto IL_00b6;
			}
		}
		else if (num == 2722098670u)
		{
			if (name == text6)
			{
				job = (Job)48;
				goto IL_00b6;
			}
		}
		else if (num == 2802598509u && name == text7)
		{
			job = Job.DiagEnable;
			goto IL_00b6;
		}
		job = (Job)0;
		goto IL_00b6;
		IL_00b6:
		State.Job = job;
		if (State.Job == Job.DiagEnable || State.Job == (Job)64 || State.Job == (Job)66)
		{
			State.Mode = SelectMode.Adb;
		}
		else if (State.Job == (Job)48)
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = title,
				Filter = filter
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			State.FilePath = openFileDialog.FileName;
		}
		else if (State.Job == (Job)50)
		{
			if (object.Equals(simpleButton.Name, text8))
			{
				QCNeditoIMEI qCNeditoIMEI = new QCNeditoIMEI();
				qCNeditoIMEI.ShowDialog();
			}
			else
			{
				using OpenFileDialog openFileDialog2 = new OpenFileDialog
				{
					Title = title2,
					Filter = filter2
				};
				if (openFileDialog2.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				State.FilePath = openFileDialog2.FileName;
			}
		}
		else if (State.Job == Job.QCNRead)
		{
			using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				Description = description,
				ShowNewFolderButton = true
			};
			if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			State.FilePath = folderBrowserDialog.SelectedPath;
		}
		else if (State.Job == Job.ImeiWrite)
		{
			using IMEI iMEI = UI.Showcust(new IMEI(edl: true));
			if (iMEI.ShowDialog() != DialogResult.Yes)
			{
				return;
			}
		}
		else if (State.Job == (Job)52)
		{
			using IMEI iMEI2 = UI.Showcust(new IMEI(edl: true));
			if (iMEI2.ShowDialog() != DialogResult.Yes)
			{
				return;
			}
		}
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void MTKDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == MTKDataGrid.Columns[0].Index)
		{
			MTKDataGrid.EndEdit();
		}
	}

	private void MTKDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!JobStatus.Busy && e.Button == MouseButtons.Right && e.RowIndex >= 0)
		{
			MTKDataGrid.ClearSelection();
			MTKDataGrid.Rows[e.RowIndex].Selected = true;
			MTKDataGrid.CurrentCell = MTKDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
			PopMTK.ShowPopup(Cursor.Position);
		}
	}

	private void Highspeed_CheckedChanged(object sender, EventArgs e)
	{
		MTKConfig.Highspeed = Highspeed.Checked;
	}

	private void MTKRepartition_CheckedChanged(object sender, EventArgs e)
	{
		MTKConfig.Repartition = MTKRepartition.Checked;
	}

	private void CarbonaraExploit_CheckedChanged(object sender, EventArgs e)
	{
		MTKConfig.UsingCarbonara = CarbonaraExploit.Checked;
	}

	private void CMTKEMI_CheckedChanged(object sender, EventArgs e)
	{
		int num = 1;
		string text = "Controllers/preloader.json";
		int num2 = 0;
		string value = "[";
		string name = "Add";
		object obj = null;
		int num3 = 2;
		string name2 = "Type";
		int operation = 83;
		string name3 = "ConditionalCompareObjectEqual";
		int num4 = 4;
		int flags = 33;
		int num5 = 3;
		string name4 = "url";
		if (CMTKEMI.Checked)
		{
			MTKLoadEMI.Enabled = true;
			MTKConfig.SerlEMI = true;
			MTKConfig.LocalEMI = true;
			string address = UI.Server + text;
			if (list_pldr_emi != null && list_pldr_emi.Count > 0)
			{
				if (list_pldr_emi.Count > 0)
				{
					foreach (clsIcons item in list_pldr_emi)
					{
						if (_003C_003Eo__37._003C_003Ep__1 == null)
						{
							_003C_003Eo__37._003C_003Ep__1 = CallSite<Action<CallSite, ComboBoxItemCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, name, (IEnumerable<Type>)obj, typeof(Main), new CSharpArgumentInfo[2]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string)obj),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj)
							}));
						}
						Action<CallSite, ComboBoxItemCollection, object> target = _003C_003Eo__37._003C_003Ep__1.Target;
						CallSite<Action<CallSite, ComboBoxItemCollection, object>> _003C_003Ep__ = _003C_003Eo__37._003C_003Ep__1;
						ComboBoxItemCollection items = DaCustum.Properties.Items;
						if (_003C_003Eo__37._003C_003Ep__0 == null)
						{
							_003C_003Eo__37._003C_003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, name2, typeof(Main), new CSharpArgumentInfo[1] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj) }));
						}
						target(_003C_003Ep__, items, _003C_003Eo__37._003C_003Ep__0.Target(_003C_003Eo__37._003C_003Ep__0, item));
						DaCustum.SelectedIndex = 0;
					}
					{
						foreach (clsIcons item2 in list_pldr_emi)
						{
							if (_003C_003Eo__37._003C_003Ep__4 == null)
							{
								Type typeFromHandle = typeof(Main);
								CSharpArgumentInfo[] array = new CSharpArgumentInfo[num];
								array[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
								_003C_003Eo__37._003C_003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation((CSharpBinderFlags)num2, (ExpressionType)operation, typeFromHandle, array));
							}
							Func<CallSite, object, bool> target2 = _003C_003Eo__37._003C_003Ep__4.Target;
							CallSite<Func<CallSite, object, bool>> _003C_003Ep__2 = _003C_003Eo__37._003C_003Ep__4;
							if (_003C_003Eo__37._003C_003Ep__3 == null)
							{
								Type typeFromHandle2 = typeof(Main);
								CSharpArgumentInfo[] array2 = new CSharpArgumentInfo[num4];
								array2[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)flags, (string)obj);
								array2[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
								array2[num3] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
								array2[num5] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num5, (string)obj);
								_003C_003Eo__37._003C_003Ep__3 = CallSite<Func<CallSite, Type, object, string, bool, object>>.Create(Binder.InvokeMember((CSharpBinderFlags)num2, name3, (IEnumerable<Type>)obj, typeFromHandle2, array2));
							}
							Func<CallSite, Type, object, string, bool, object> target3 = _003C_003Eo__37._003C_003Ep__3.Target;
							CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__3 = _003C_003Eo__37._003C_003Ep__3;
							Type typeFromHandle3 = typeof(Operators);
							if (_003C_003Eo__37._003C_003Ep__2 == null)
							{
								Type typeFromHandle4 = typeof(Main);
								CSharpArgumentInfo[] array3 = new CSharpArgumentInfo[num];
								array3[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
								_003C_003Eo__37._003C_003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember((CSharpBinderFlags)num2, name2, typeFromHandle4, array3));
							}
							if (target2(_003C_003Ep__2, target3(_003C_003Ep__3, typeFromHandle3, _003C_003Eo__37._003C_003Ep__2.Target(_003C_003Eo__37._003C_003Ep__2, item2), DaCustum.Text, (byte)num2 != 0)))
							{
								if (_003C_003Eo__37._003C_003Ep__6 == null)
								{
									_003C_003Eo__37._003C_003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert((CSharpBinderFlags)num2, typeof(string), typeof(Main)));
								}
								Func<CallSite, object, string> target4 = _003C_003Eo__37._003C_003Ep__6.Target;
								CallSite<Func<CallSite, object, string>> _003C_003Ep__4 = _003C_003Eo__37._003C_003Ep__6;
								if (_003C_003Eo__37._003C_003Ep__5 == null)
								{
									Type typeFromHandle5 = typeof(Main);
									CSharpArgumentInfo[] array4 = new CSharpArgumentInfo[num];
									array4[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
									_003C_003Eo__37._003C_003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember((CSharpBinderFlags)num2, name4, typeFromHandle5, array4));
								}
								Server.jsonmenemi = target4(_003C_003Ep__4, _003C_003Eo__37._003C_003Ep__5.Target(_003C_003Eo__37._003C_003Ep__5, item2));
							}
						}
						return;
					}
				}
				CMTKEMI.Checked = false;
			}
			else
			{
				if (string.IsNullOrEmpty(Server.jsonmenemi))
				{
					Server.jsonmenemi = new WebClient().DownloadString(address);
				}
				if (!Server.jsonmenemi.StartsWith(value))
				{
					Thread.Sleep(200);
					Server.jsonmenemi = new WebClient().DownloadString(address);
				}
				Console.WriteLine(Server.jsonmenemi);
				list_pldr_emi = JsonConvert.DeserializeObject<List<clsIcons>>(Server.jsonmenemi);
			}
		}
		else
		{
			MTKLoadEMI.Enabled = false;
			DaCustum.Properties.Items.Clear();
			DaCustum.Text = string.Empty;
			MTKConfig.LocalEMI = false;
			MTKConfig.SerlEMI = false;
		}
	}

	private void CMTKServer_CheckedChanged(object sender, EventArgs e)
	{
		string item = "Oppo-Realme";
		string item2 = "Samsung";
		string item3 = "Vivo";
		string item4 = "Infinix-Itel-Tecno";
		string item5 = "Xiaomi";
		string item6 = "Huawei-Honor";
		string item7 = "MTK_AllInOne_DA_V1";
		string item8 = "MTK_AllInOne_DA_V2";
		string item9 = "MTK_AllInOne_DA_V3";
		string item10 = "MTK_AllInOne_DA_Oppo";
		string item11 = "MTK_AllInOne_DA_Xiaomi";
		string item12 = "MTK_AllInOne_DA_MT_V6";
		if (!CMTKServer.Checked)
		{
			MTKConfig.MTKServer = false;
			MTKBrand.Text = string.Empty;
			MTKModel.Text = string.Empty;
			MTKModel.Properties.Items.Clear();
			MTKBrand.Properties.Items.Clear();
			MTKModel.Properties.Items.Add(item7);
			MTKModel.Properties.Items.Add(item8);
			MTKModel.Properties.Items.Add(item9);
			MTKModel.Properties.Items.Add(item10);
			MTKModel.Properties.Items.Add(item11);
			MTKModel.Properties.Items.Add(item12);
			MTKModel.SelectedIndex = 0;
		}
		else
		{
			MTKBrand.Text = string.Empty;
			MTKModel.Text = string.Empty;
			MTKModel.Properties.Items.Clear();
			MTKBrand.Properties.Items.Clear();
			CMTKDA.Checked = !CMTKServer.Checked;
			MTKConfig.MTKServer = CMTKServer.Checked;
			MTKBrand.Properties.Items.Add(item);
			MTKBrand.Properties.Items.Add(item2);
			MTKBrand.Properties.Items.Add(item3);
			MTKBrand.Properties.Items.Add(item4);
			MTKBrand.Properties.Items.Add(item5);
			MTKBrand.Properties.Items.Add(item6);
			MTKBrand.SelectedIndex = 0;
		}
	}

	private void CMTKDA_CheckedChanged(object sender, EventArgs e)
	{
		CMTKServer.Checked = !CMTKDA.Checked;
		MTKConfig.LocalDA = CMTKDA.Checked;
	}

	private void MTKFlashMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		string value = "Format Download";
		if (!MTKFlashMode.Text.Contains(value))
		{
			MTKConfig.FormatALL = false;
			if (!JobStatus.Busy)
			{
				switch (MTKFlashMode.SelectedIndex)
				{
				default:
					State.FlashingMode = (Downloadmode)0;
					break;
				case 0:
					State.FlashingMode = (Downloadmode)0;
					break;
				case 1:
					State.FlashingMode = Downloadmode.FormatDownload;
					break;
				case 2:
					State.FlashingMode = (Downloadmode)2;
					break;
				}
			}
			return;
		}
		MTKConfig.FormatALL = true;
		if (!JobStatus.Busy)
		{
			switch (MTKFlashMode.SelectedIndex)
			{
			default:
				State.FlashingMode = (Downloadmode)0;
				break;
			case 0:
				State.FlashingMode = (Downloadmode)0;
				break;
			case 1:
				State.FlashingMode = Downloadmode.FormatDownload;
				break;
			case 2:
				State.FlashingMode = (Downloadmode)2;
				break;
			}
		}
	}

	private void MTKBrand_SelectedIndexChanged(object sender, EventArgs e)
	{
		MTKModel.Properties.Items.Clear();
		ListModelMTK = Server.LoadMTKModel((string)MTKBrand.SelectedItem);
		if (ListModelMTK.Count != 0)
		{
			ComboBoxItemCollection items = MTKModel.Properties.Items;
			object[] items2 = ListModelMTK.Select((Server.ModelConfigMTK m) => m.Model).ToArray();
			items.AddRange(items2);
			MTKModel.SelectedIndex = 0;
		}
	}

	private void MTKModel_SelectedIndexChanged(object sender, EventArgs e)
	{
		object obj = null;
		string text = "";
		if (!CMTKServer.Checked)
		{
			DeviceState.ModelDa = MTKModel.Text;
			Console.WriteLine(DeviceState.ModelDa ?? text);
			return;
		}
		Server.ModelConfigMTK modelConfigMTK = ListModelMTK.FirstOrDefault((Server.ModelConfigMTK x) => x.Model == (string)MTKModel.SelectedItem);
		if (modelConfigMTK != obj)
		{
			ApplyConfigMTK(modelConfigMTK);
		}
	}

	private void MTKOptions(object sender, ItemClickEventArgs e)
	{
		object obj = null;
		string text = "";
		int num = 0;
		string text2 = "MTKWrite";
		string text3 = "MTKRead";
		string text4 = "MTKFormat";
		string text5 = "WriteCheck";
		string text6 = "FormatCheck";
		string text7 = "ReadPartition";
		int num2 = 1;
		string text8 = "Select file want to write to ";
		string filter = "All Files|*.*";
		int job = 8;
		string text9 = "Save ";
		string text10 = ".bin";
		int job2 = 9;
		int job3 = 11;
		string text11 = "Please Select Partition First!";
		string caption = "AMFTool";
		int icon = 64;
		string text12 = "preloader";
		int job4 = 7;
		int job5 = 10;
		if (JobStatus.Busy)
		{
			return;
		}
		BarItem item = e.Item;
		if (item == obj)
		{
			return;
		}
		string text13 = text;
		PopMTK.HidePopup();
		string name = item.Name;
		string text14 = name;
		if (!(text14 == text2))
		{
			if (!(text14 == text3))
			{
				if (!(text14 == text4))
				{
					if (!(text14 == text5))
					{
						if (!(text14 == text6))
						{
							if (text14 == text7)
							{
								FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
								folderBrowserDialog.ShowNewFolderButton = true;
								if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
								{
									State.SavePath = folderBrowserDialog.SelectedPath;
								}
								foreach (DataGridViewRow item2 in (IEnumerable)MTKDataGrid.Rows)
								{
									if ((bool)item2.Cells[0].Value)
									{
										DeviceState.Partitiontarget.Add((string)item2.Cells[1].Value);
									}
								}
								if (((MTKDataGrid.Rows.Count > num) ? 1 : 0) == num)
								{
									XtraMessageBox.Show(text11, caption, (MessageBoxButtons)num, (MessageBoxIcon)icon);
									return;
								}
								State.Job = (Job)job5;
							}
						}
						else
						{
							foreach (DataGridViewRow item3 in (IEnumerable)MTKDataGrid.Rows)
							{
								if ((bool)item3.Cells[0].Value)
								{
									DeviceState.Partitiontarget.Add((string)item3.Cells[1].Value);
								}
							}
							if (((MTKDataGrid.Rows.Count > num) ? 1 : 0) == num)
							{
								XtraMessageBox.Show(text11, caption, (MessageBoxButtons)num, (MessageBoxIcon)icon);
								return;
							}
							State.Job = (Job)job3;
						}
					}
					else
					{
						MTKList.Partition.Clear();
						if (MTKDataGrid.Rows.Count <= 0)
						{
							XtraMessageBox.Show(text11, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							return;
						}
						if (State.FlashingMode == (Downloadmode)0)
						{
							foreach (DataGridViewRow item4 in (IEnumerable)MTKDataGrid.Rows)
							{
								if (File.Exists((string)item4.Cells[MTKDataGrid.Columns[6].Index].Value))
								{
									if ((string)item4.Cells[MTKDataGrid.Columns[1].Index].Value == text12)
									{
										MTKConfig.PathPreloader = (string)item4.Cells[MTKDataGrid.Columns[6].Index].Value;
									}
									MTKList.Partition.Add(new MTKList((string)item4.Cells[MTKDataGrid.Columns[1].Index].Value, (string)item4.Cells[MTKDataGrid.Columns[2].Index].Value, (string)item4.Cells[MTKDataGrid.Columns[4].Index].Value, (string)item4.Cells[MTKDataGrid.Columns[5].Index].Value, (string)item4.Cells[MTKDataGrid.Columns[6].Index].Value));
								}
							}
						}
						State.Job = (Job)job4;
					}
				}
				else
				{
					foreach (DataGridViewRow selectedRow in MTKDataGrid.SelectedRows)
					{
						DeviceState.Partitiontarget.Add((string)selectedRow.Cells[1].Value);
					}
					State.Job = (Job)job3;
				}
			}
			else
			{
				foreach (DataGridViewRow selectedRow2 in MTKDataGrid.SelectedRows)
				{
					DeviceState.Partitiontarget.Add((string)selectedRow2.Cells[1].Value);
					text13 = (string)selectedRow2.Cells[1].Value;
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Title = text9 + text13,
					Filter = filter,
					FileName = text13 + text10
				})
				{
					if (((saveFileDialog.ShowDialog() == (DialogResult)num2) ? 1 : 0) == num)
					{
						return;
					}
					State.SavePath = saveFileDialog.FileName;
				}
				State.Job = (Job)job2;
			}
		}
		else
		{
			foreach (DataGridViewRow selectedRow3 in MTKDataGrid.SelectedRows)
			{
				DeviceState.Partitiontarget.Add((string)selectedRow3.Cells[1].Value);
				text13 = (string)selectedRow3.Cells[1].Value;
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = text8 + text13,
				Filter = filter
			})
			{
				if (((openFileDialog.ShowDialog() == (DialogResult)num2) ? 1 : 0) == num)
				{
					return;
				}
				State.FilePath = openFileDialog.FileName;
			}
			State.Job = (Job)job;
		}
		State.Operation = item.ToString();
		State.Mode = (SelectMode)num2;
		RunOperation();
	}

	private void MTKLoad(object sender, EventArgs e)
	{
		int num = 1;
		int num2 = 0;
		string text = "MTKLoadFirmware";
		string text2 = "MTKLoadEMI";
		string text3 = "MTKLoadDA";
		string value = "Dump";
		string text4 = ".bin";
		int num3 = 7;
		int num4 = 2;
		int num5 = 3;
		int num6 = 4;
		string text5 = "0x00000";
		int num7 = 5;
		string text6 = "0x";
		int num8 = 6;
		string text7 = ".img";
		string value2 = "Scatter";
		string title = "Select Scatter or XML";
		string filter = "Scatter or XML |*.xml;*.txt";
		string text8 = "flash.xml";
		string xpath = "/flash-mode/scatter";
		string xpath2 = "/flash-mode/dagent";
		string text9 = "invalid flash.xml!";
		string caption = "Error";
		int icon = 16;
		string text10 = "Failed to parse flash.xml: ";
		string text11 = "Scatter file not found: ";
		string value3 = "EMMC";
		string newValue = "-verified.bin";
		string newValue2 = "-verified.img";
		string text12 = "preloader";
		string text13 = "true";
		string text14 = "Failed to load scatter file: ";
		string title2 = "Select EMI|Preloader";
		string filter2 = "EMI / Preloader (*.bin;*.img)|*.bin;*.img|All Files (*.*)|*.*";
		string title3 = "Select Download-Agent";
		string filter3 = "DA (*.bin)|*.bin|All Files (*.*)|*.*";
		if (!(sender is Button button) || JobStatus.Busy)
		{
			return;
		}
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			FilterIndex = 1,
			RestoreDirectory = true,
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
		};
		string name = button.Name;
		string text15 = name;
		if (text15 == text)
		{
			if (!SuperMap.Text.Contains(value))
			{
				if (!SuperMap.Text.Contains(value2))
				{
					return;
				}
				openFileDialog.Title = title;
				openFileDialog.Filter = filter;
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				MTKDataGrid.Rows.Clear();
				MtkSettings.ListMtk = new List<MtkSettings>();
				GptAddr.Addr = new List<GptAddr>();
				MTKFirmware.Text = openFileDialog.FileName;
				string fileName = openFileDialog.FileName;
				string text16 = fileName;
				if (Path.GetFileName(fileName).ToLower() == text8)
				{
					try
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(fileName);
						XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
						if (xmlNode == null || string.IsNullOrEmpty(xmlNode.InnerText))
						{
							XtraMessageBox.Show(text9, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						string directoryName = Path.GetDirectoryName(fileName);
						text16 = Path.Combine(directoryName, xmlNode.InnerText);
						XmlNode xmlNode2 = xmlDocument.SelectSingleNode(xpath2);
						if (xmlNode2 != null && !string.IsNullOrEmpty(xmlNode2.InnerText))
						{
							string text17 = Path.Combine(directoryName, xmlNode2.InnerText);
							if (File.Exists(text17))
							{
								MTKConfig.PathDownloadAgent = text17;
							}
						}
					}
					catch (Exception ex)
					{
						XtraMessageBox.Show(text10 + ex.Message, caption, (MessageBoxButtons)num2, (MessageBoxIcon)icon);
						return;
					}
				}
				if ((File.Exists(text16) ? 1 : 0) == num2)
				{
					XtraMessageBox.Show(text11 + text16, caption, (MessageBoxButtons)num2, (MessageBoxIcon)icon);
					return;
				}
				try
				{
					MTKDataGrid.Rows.Clear();
					MTKConfig.PathFirmware = text16;
					MtkSettings.ListMtk = Settings.ScatterTable(text16);
					MtkSettings.PathScatter = Path.GetDirectoryName(text16);
					string storage = Settings.Storage;
					if (!storage.Contains(value3))
					{
						LayoutStore.SelectedIndex = num;
					}
					else
					{
						LayoutStore.SelectedIndex = num2;
					}
					foreach (MtkSettings item in MtkSettings.ListMtk)
					{
						if (!ShowALLMTK.Checked)
						{
							if (!(item.IsDownload.ToLower() == text13))
							{
								continue;
							}
							GptAddr.Addr.Add(new GptAddr(item.PartitionName, item.LinearStartAddr.ToHex(), item.PartitionSize.ToHex()));
							string text18 = Path.Combine(MtkSettings.PathScatter, item.FileName.Replace(text4, newValue).Replace(text7, newValue2));
							string text19 = Path.Combine(MtkSettings.PathScatter, item.FileName);
							if (!File.Exists(text18))
							{
								if (!File.Exists(text19))
								{
									DataGridViewRowCollection rows = MTKDataGrid.Rows;
									object[] array = new object[num3];
									array[num2] = (byte)num2 != 0;
									array[num] = item.PartitionName;
									array[num4] = string.Empty;
									array[num5] = item.PartitionSize;
									array[num6] = item.LinearStartAddr;
									array[num7] = item.PartitionSize;
									array[num8] = string.Empty;
									rows.Add(array);
								}
								else
								{
									DataGridViewRowCollection rows2 = MTKDataGrid.Rows;
									object[] array2 = new object[num3];
									array2[num2] = (byte)num != 0;
									array2[num] = item.PartitionName;
									array2[num4] = Path.GetFileName(text19);
									array2[num5] = utils.FileSize(new FileInfo(text19).Length);
									array2[num6] = item.LinearStartAddr;
									array2[num7] = item.PartitionSize;
									array2[num8] = text19;
									rows2.Add(array2);
								}
							}
							else
							{
								DataGridViewRowCollection rows3 = MTKDataGrid.Rows;
								object[] array3 = new object[num3];
								array3[num2] = (byte)num != 0;
								array3[num] = item.PartitionName;
								array3[num4] = Path.GetFileName(text18);
								array3[num5] = utils.FileSize(new FileInfo(text18).Length);
								array3[num6] = item.LinearStartAddr;
								array3[num7] = item.PartitionSize;
								array3[num8] = text18;
								rows3.Add(array3);
							}
							if (item.PartitionName.ToLower() == text12)
							{
								string text20 = ((!File.Exists(text18)) ? text19 : text18);
								if (File.Exists(text20))
								{
									MTKConfig.PathPreloader = text20;
									MTKEMI.Text = MTKConfig.PathPreloader;
								}
							}
							continue;
						}
						GptAddr.Addr.Add(new GptAddr(item.PartitionName, item.LinearStartAddr.ToHex(), item.PartitionSize.ToHex()));
						string text21 = Path.Combine(MtkSettings.PathScatter, item.FileName.Replace(text4, newValue).Replace(text7, newValue2));
						string text22 = Path.Combine(MtkSettings.PathScatter, item.FileName);
						if (!File.Exists(text21))
						{
							if (!File.Exists(text22))
							{
								DataGridViewRowCollection rows4 = MTKDataGrid.Rows;
								object[] array4 = new object[num3];
								array4[num2] = (byte)num2 != 0;
								array4[num] = item.PartitionName;
								array4[num4] = string.Empty;
								array4[num5] = item.PartitionSize;
								array4[num6] = item.LinearStartAddr;
								array4[num7] = item.PartitionSize;
								array4[num8] = string.Empty;
								rows4.Add(array4);
							}
							else
							{
								DataGridViewRowCollection rows5 = MTKDataGrid.Rows;
								object[] array5 = new object[num3];
								array5[num2] = (byte)num != 0;
								array5[num] = item.PartitionName;
								array5[num4] = Path.GetFileName(text22);
								array5[num5] = utils.FileSize(new FileInfo(text22).Length);
								array5[num6] = item.LinearStartAddr;
								array5[num7] = item.PartitionSize;
								array5[num8] = text22;
								rows5.Add(array5);
							}
						}
						else
						{
							DataGridViewRowCollection rows6 = MTKDataGrid.Rows;
							object[] array6 = new object[num3];
							array6[num2] = (byte)num != 0;
							array6[num] = item.PartitionName;
							array6[num4] = Path.GetFileName(text21);
							array6[num5] = utils.FileSize(new FileInfo(text21).Length);
							array6[num6] = item.LinearStartAddr;
							array6[num7] = item.PartitionSize;
							array6[num8] = text21;
							rows6.Add(array6);
						}
						if (item.PartitionName.ToLower() == text12)
						{
							string text23 = ((!File.Exists(text21)) ? text22 : text21);
							if (File.Exists(text23))
							{
								MTKConfig.PathPreloader = text23;
								MTKEMI.Text = MTKConfig.PathPreloader;
							}
						}
					}
					return;
				}
				catch (Exception ex2)
				{
					MessageBox.Show(text14 + ex2.Message, caption, (MessageBoxButtons)num2, (MessageBoxIcon)icon);
					return;
				}
			}
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.ShowNewFolderButton = true;
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				MTKFirmware.Text = folderBrowserDialog.SelectedPath;
			}
			MTKDataGrid.Rows.Clear();
			ListBox listBox = new ListBox();
			listBox.Items.Add(MTKFirmware.Text);
			Path.GetDirectoryName(MTKFirmware.Text);
			Console.WriteLine(MTKFirmware.Text);
			for (int i = 0; i < listBox.Items.Count; i += num)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(listBox.Items[i].ToString());
				foreach (FileInfo item2 in directoryInfo.EnumerateFiles())
				{
					Console.WriteLine(item2);
					if (item2.Name.Contains(text4))
					{
						DataGridViewRowCollection rows7 = MTKDataGrid.Rows;
						object[] array7 = new object[num3];
						array7[num2] = (byte)num != 0;
						array7[num] = Path.GetFileName(item2.Name);
						array7[num4] = Path.GetFileName(item2.Name);
						array7[num5] = utils.FileSize(item2.Length);
						array7[num6] = text5;
						array7[num7] = text6 + Conversion.Hex(item2.Length);
						array7[num8] = item2.FullName;
						rows7.Add(array7);
					}
					if (item2.Name.Contains(text7))
					{
						DataGridViewRowCollection rows8 = MTKDataGrid.Rows;
						object[] array8 = new object[num3];
						array8[num2] = (byte)num != 0;
						array8[num] = Path.GetFileName(item2.Name);
						array8[num4] = Path.GetFileName(item2.Name);
						array8[num5] = utils.FileSize(item2.Length);
						array8[num6] = text5;
						array8[num7] = text6 + Conversion.Hex(item2.Length);
						array8[num8] = item2.FullName;
						rows8.Add(array8);
					}
				}
			}
		}
		else if (!(text15 == text2))
		{
			if (text15 == text3)
			{
				openFileDialog.Title = title3;
				openFileDialog.Filter = filter3;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					MTKConfig.PathDownloadAgent = openFileDialog.FileName;
					MTKDA.Text = MTKConfig.PathDownloadAgent;
				}
			}
		}
		else
		{
			openFileDialog.Title = title2;
			openFileDialog.Filter = filter2;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				MTKConfig.PathPreloader = openFileDialog.FileName;
				MTKEMI.Text = MTKConfig.PathPreloader;
			}
		}
	}

	private void MTKClear(object sender, EventArgs e)
	{
		string text = "MTKClearEMI";
		string text2 = "MTKClearDA";
		if (sender is SimpleButton simpleButton && !JobStatus.Busy)
		{
			string name = simpleButton.Name;
			string text3 = name;
			if (text3 == text)
			{
				MTKConfig.PathPreloader = string.Empty;
				MTKEMI.Text = string.Empty;
				MTKEMI.Clear();
			}
			else if (text3 == text2)
			{
				MTKConfig.PathDownloadAgent = string.Empty;
				MTKEMI.Text = string.Empty;
				MTKDA.Clear();
			}
		}
	}

	private void MTKHandler(object sender, EventArgs e)
	{
		int num = 1;
		string text = "MTKReadGPT";
		string text2 = "MTKReadFlash";
		string text3 = "MTKReboot";
		string text4 = "MTKFlash";
		string text5 = "MTKReadback";
		string text6 = "MTKNVErase";
		string text7 = "MTKNVBackup";
		string text8 = "MTKNVRestore";
		string text9 = "MTKRPMBErase";
		string text10 = "MTKRPMBBackup";
		string text11 = "MTKRPMBRestore";
		string text12 = "MTKDisableMiCloud";
		string text13 = "MTKResetMICloud";
		string text14 = "MTKOPPOID";
		string text15 = "MTKHuaweiID";
		string text16 = "MTKSecPlugin";
		string text17 = "MTKPayjoy";
		string text18 = "MTKResetLock";
		string text19 = "MTKSafeFormat";
		string text20 = "MTKResetFrp";
		string text21 = "MTKIMEIRepair";
		string text22 = "MTKUNetwork";
		string text23 = "MTKUBL";
		string text24 = "MTKLBL";
		string text25 = "MTKDMVerity";
		string text26 = "MTKOrangestate";
		string text27 = "MTKPatchVB";
		string text28 = "MTKCriticalRead";
		string text29 = "MTKCriticalWrite";
		string text30 = "MTKRemoveDemo";
		string text31 = "MTKWiteemi";
		string text32 = "MTKReadDUMP";
		string text33 = "RebuildUserDATA";
		int num2 = 2;
		int num3 = 6;
		int index = 5;
		int num4 = 0;
		string value = "Dump";
		int index2 = 4;
		string text34 = "Please Select Partition First!";
		string caption = "AMFTool";
		int icon = 64;
		string value2 = "Scatter";
		string text35 = "preloader";
		string description = "Select Path";
		string description2 = "Select Folder";
		string title = "Choice Preloader File !";
		string filter = "Preloader |*.bin";
		string title2 = "Save RPMB";
		string filter2 = "All Files|*.*";
		string fileName = "rpmb.bin";
		string title3 = "Select RPMB";
		string filter3 = "RPMB Files All Files (*.*)|*.*";
		string title4 = "Select NV Backup";
		string filter4 = "NV Backup All Files (*.*)|*.*";
		string text36 = "This Operation only for Xiaomi!, Click Ok to continue procedure";
		string caption2 = "Information";
		string title5 = "Select Critical Data";
		string filter5 = "Critical All Files (*.*)|*.*";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = SelectMode.Mediatek;
		MTKList.Partition = new List<MTKList>();
		string name = simpleButton.Name;
		uint num5 = ImplementationDetails.method_01(name);
		Job job;
		if (num5 <= 2196423080u)
		{
			if (num5 > 895799437)
			{
				if (num5 <= 1204375351)
				{
					if (num5 > 1131482798)
					{
						if (num5 != 1185907423)
						{
							if (num5 == 1204375351 && name == text13)
							{
								job = (Job)58;
								goto IL_03b6;
							}
						}
						else if (name == text18)
						{
							job = Job.Reset;
							goto IL_03b6;
						}
					}
					else if (num5 != 981763301)
					{
						if (num5 == 1131482798 && name == text9)
						{
							job = Job.RPMBErase;
							goto IL_03b6;
						}
					}
					else if (name == text16)
					{
						job = (Job)70;
						goto IL_03b6;
					}
				}
				else if (num5 <= 1666959507)
				{
					if (num5 != 1299799217)
					{
						if (num5 == 1666959507 && name == text19)
						{
							job = (Job)14;
							goto IL_03b6;
						}
					}
					else if (name == text4)
					{
						job = Job.Flash;
						goto IL_03b6;
					}
				}
				else if (num5 != 1741917827)
				{
					if (num5 == 2196423080u && name == text20)
					{
						job = Job.Frp;
						goto IL_03b6;
					}
				}
				else if (name == text31)
				{
					job = Job.WiteEMI;
					goto IL_03b6;
				}
			}
			else if (num5 <= 73540309)
			{
				if (num5 > 24484375)
				{
					if (num5 != 28999045)
					{
						if (num5 == 73540309 && name == text27)
						{
							job = Job.Vbmeta;
							goto IL_03b6;
						}
					}
					else if (name == text15)
					{
						job = Job.ID;
						goto IL_03b6;
					}
				}
				else if (num5 != 11389469)
				{
					if (num5 == 24484375 && name == text7)
					{
						job = Job.NVBackup;
						goto IL_03b6;
					}
				}
				else if (name == text25)
				{
					job = (Job)78;
					goto IL_03b6;
				}
			}
			else if (num5 <= 515354566)
			{
				if (num5 != 227796916)
				{
					if (num5 == 515354566 && name == text14)
					{
						job = Job.ID;
						goto IL_03b6;
					}
				}
				else if (name == text26)
				{
					job = (Job)68;
					goto IL_03b6;
				}
			}
			else if (num5 != 570603378)
			{
				if (num5 == 895799437 && name == text6)
				{
					job = (Job)42;
					goto IL_03b6;
				}
			}
			else if (name == text12)
			{
				job = (Job)56;
				goto IL_03b6;
			}
		}
		else if (num5 <= 3317157179u)
		{
			if (num5 <= 2800131119u)
			{
				if (num5 > 2468785508u)
				{
					if (num5 != 2505184072u)
					{
						if (num5 == 2800131119u && name == text32)
						{
							job = (Job)72;
							goto IL_03b6;
						}
					}
					else if (name == text21)
					{
						job = Job.ImeiWrite;
						goto IL_03b6;
					}
				}
				else if (num5 != 2279199430u)
				{
					if (num5 == 2468785508u && name == text28)
					{
						job = (Job)46;
						goto IL_03b6;
					}
				}
				else if (name == text30)
				{
					job = Job.RemoveDemo;
					goto IL_03b6;
				}
			}
			else if (num5 <= 3115473578u)
			{
				if (num5 != 2944691513u)
				{
					if (num5 == 3115473578u && name == text)
					{
						job = (Job)2;
						goto IL_03b6;
					}
				}
				else if (name == text8)
				{
					job = (Job)40;
					goto IL_03b6;
				}
			}
			else if (num5 != 3228338926u)
			{
				if (num5 == 3317157179u && name == text33)
				{
					job = Job.RebuildUserDATA;
					goto IL_03b6;
				}
			}
			else if (name == text11)
			{
				job = Job.RPMBRestore;
				goto IL_03b6;
			}
		}
		else if (num5 <= 3550912729u)
		{
			if (num5 <= 3343940966u)
			{
				if (num5 != 3341985086u)
				{
					if (num5 == 3343940966u && name == text23)
					{
						job = (Job)54;
						goto IL_03b6;
					}
				}
				else if (name == text22)
				{
					job = Job.Network;
					goto IL_03b6;
				}
			}
			else if (num5 != 3364919536u)
			{
				if (num5 == 3550912729u && name == text17)
				{
					job = Job.Payjoy;
					goto IL_03b6;
				}
			}
			else if (name == text3)
			{
				job = (Job)80;
				goto IL_03b6;
			}
		}
		else if (num5 <= 3779944305u)
		{
			if (num5 != 3671412746u)
			{
				if (num5 == 3779944305u && name == text2)
				{
					job = (Job)6;
					goto IL_03b6;
				}
			}
			else if (name == text5)
			{
				job = Job.Readback;
				goto IL_03b6;
			}
		}
		else if (num5 != 3936877855u)
		{
			if (num5 != 4088133466u)
			{
				if (num5 == 4230285613u && name == text24)
				{
					job = Job.BLLock;
					goto IL_03b6;
				}
			}
			else if (name == text10)
			{
				job = (Job)44;
				goto IL_03b6;
			}
		}
		else if (name == text29)
		{
			job = (Job)48;
			goto IL_03b6;
		}
		job = (Job)0;
		goto IL_03b6;
		IL_03b6:
		State.Job = job;
		if (State.Job != (Job)2)
		{
			if (State.Job == Job.Flash)
			{
				if (SuperMap.Text.Contains(value))
				{
					foreach (DataGridViewRow item in (IEnumerable)MTKDataGrid.Rows)
					{
						if ((bool)item.Cells[0].Value && File.Exists((string)item.Cells[MTKDataGrid.Columns[6].Index].Value))
						{
							MTKList.Partition.Add(new MTKList((string)item.Cells[MTKDataGrid.Columns[1].Index].Value, (string)item.Cells[MTKDataGrid.Columns[2].Index].Value, (string)item.Cells[MTKDataGrid.Columns[4].Index].Value, (string)item.Cells[MTKDataGrid.Columns[5].Index].Value, (string)item.Cells[MTKDataGrid.Columns[6].Index].Value));
						}
					}
					if (((MTKList.Partition.Count > num4) ? 1 : 0) == num4)
					{
						XtraMessageBox.Show(text34, caption, (MessageBoxButtons)num4, (MessageBoxIcon)icon);
						return;
					}
				}
				else if (SuperMap.Text.Contains(value2))
				{
					foreach (DataGridViewRow item2 in (IEnumerable)MTKDataGrid.Rows)
					{
						if ((bool)item2.Cells[0].Value && File.Exists((string)item2.Cells[MTKDataGrid.Columns[6].Index].Value))
						{
							if ((string)item2.Cells[MTKDataGrid.Columns[1].Index].Value == text35)
							{
								MTKConfig.PathPreloader = (string)item2.Cells[MTKDataGrid.Columns[6].Index].Value;
							}
							MTKList.Partition.Add(new MTKList((string)item2.Cells[MTKDataGrid.Columns[1].Index].Value, (string)item2.Cells[MTKDataGrid.Columns[2].Index].Value, (string)item2.Cells[MTKDataGrid.Columns[4].Index].Value, (string)item2.Cells[MTKDataGrid.Columns[5].Index].Value, (string)item2.Cells[MTKDataGrid.Columns[6].Index].Value));
						}
					}
					if (((MTKList.Partition.Count > num4) ? 1 : 0) == num4)
					{
						XtraMessageBox.Show(text34, caption, (MessageBoxButtons)num4, (MessageBoxIcon)icon);
						return;
					}
					if ((State.FlashingMode == (Downloadmode)num) ? ((byte)num != 0) : (State.FlashingMode == (Downloadmode)num2))
					{
						foreach (DataGridViewRow item3 in (IEnumerable)MTKDataGrid.Rows)
						{
							if (File.Exists((string)item3.Cells[MTKDataGrid.Columns[num3].Index].Value))
							{
								if ((string)item3.Cells[MTKDataGrid.Columns[num].Index].Value == text35)
								{
									MTKConfig.PathPreloader = (string)item3.Cells[MTKDataGrid.Columns[num3].Index].Value;
								}
								MTKList.Partition.Add(new MTKList((string)item3.Cells[MTKDataGrid.Columns[num].Index].Value, (string)item3.Cells[MTKDataGrid.Columns[num2].Index].Value, (string)item3.Cells[MTKDataGrid.Columns[index2].Index].Value, (string)item3.Cells[MTKDataGrid.Columns[index].Index].Value, (string)item3.Cells[MTKDataGrid.Columns[num3].Index].Value));
							}
						}
					}
				}
			}
			else if (State.Job == Job.NVBackup)
			{
				using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
				{
					Description = description,
					ShowNewFolderButton = true
				};
				if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				State.SavePath = folderBrowserDialog.SelectedPath;
			}
			else if (State.Job == Job.Readback)
			{
				using FolderBrowserDialog folderBrowserDialog2 = new FolderBrowserDialog
				{
					Description = description2,
					ShowNewFolderButton = true
				};
				if (folderBrowserDialog2.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				State.SavePath = folderBrowserDialog2.SelectedPath;
			}
			else if (State.Job != Job.WiteEMI)
			{
				if (State.Job == (Job)6)
				{
					using FolderBrowserDialog folderBrowserDialog3 = new FolderBrowserDialog
					{
						Description = description2,
						ShowNewFolderButton = true
					};
					if (folderBrowserDialog3.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.SavePath = folderBrowserDialog3.SelectedPath;
				}
				else if (State.Job == (Job)44)
				{
					using SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						Title = title2,
						Filter = filter2,
						FileName = fileName
					};
					if (saveFileDialog.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.SavePath = saveFileDialog.FileName;
				}
				else if (State.Job == Job.RPMBRestore)
				{
					using OpenFileDialog openFileDialog = new OpenFileDialog
					{
						FilterIndex = 1,
						RestoreDirectory = true,
						InitialDirectory = AppContext.BaseDirectory
					};
					openFileDialog.Title = title3;
					openFileDialog.Filter = filter3;
					if (openFileDialog.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.FilePath = openFileDialog.FileName;
				}
				else if (State.Job == (Job)40)
				{
					using OpenFileDialog openFileDialog2 = new OpenFileDialog
					{
						FilterIndex = 1,
						RestoreDirectory = true,
						InitialDirectory = AppContext.BaseDirectory
					};
					openFileDialog2.Title = title4;
					openFileDialog2.Filter = filter4;
					if (openFileDialog2.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.FilePath = openFileDialog2.FileName;
				}
				else if (State.Job == Job.ImeiWrite)
				{
					using (FolderBrowserDialog folderBrowserDialog4 = new FolderBrowserDialog
					{
						Description = description,
						ShowNewFolderButton = true
					})
					{
						if (folderBrowserDialog4.ShowDialog() != DialogResult.OK)
						{
							return;
						}
						State.SavePath = folderBrowserDialog4.SelectedPath;
					}
					using IMEI iMEI = UI.Showcust(new IMEI((byte)num4 != 0, (byte)num4 != 0, (byte)num4 != 0));
					if (((iMEI.ShowDialog() == (DialogResult)num3) ? 1 : 0) == num4)
					{
						return;
					}
				}
				else if (State.Job == (Job)72)
				{
					using FolderBrowserDialog folderBrowserDialog5 = new FolderBrowserDialog
					{
						Description = description2,
						ShowNewFolderButton = true
					};
					if (folderBrowserDialog5.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.SavePath = folderBrowserDialog5.SelectedPath;
					using ReadDump readDump = UI.Showcust(new ReadDump());
					if (readDump.ShowDialog() != DialogResult.Yes)
					{
						return;
					}
				}
				else if (State.Job == (Job)48 || State.Job == (Job)46)
				{
					DialogResult dialogResult = XtraMessageBox.Show(text36, caption2, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
					if (dialogResult != DialogResult.OK)
					{
						return;
					}
					if (State.Job == (Job)48)
					{
						using OpenFileDialog openFileDialog3 = new OpenFileDialog
						{
							FilterIndex = 1,
							RestoreDirectory = true,
							InitialDirectory = AppContext.BaseDirectory
						};
						openFileDialog3.Title = title5;
						openFileDialog3.Filter = filter5;
						if (openFileDialog3.ShowDialog() != DialogResult.OK)
						{
							return;
						}
					}
				}
			}
			else
			{
				if (!CMTKEMI.Checked)
				{
					OpenFileDialog openFileDialog4 = new OpenFileDialog
					{
						Title = title,
						Filter = filter
					};
					if (openFileDialog4.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.Data = openFileDialog4.FileName;
				}
				else if (!string.IsNullOrWhiteSpace(DaCustum.Text))
				{
					State.Data = DaCustum.Text;
				}
				else if (string.IsNullOrEmpty(MTKEMI.Text))
				{
					OpenFileDialog openFileDialog5 = new OpenFileDialog
					{
						Title = title,
						Filter = filter
					};
					if (openFileDialog5.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					State.Data = openFileDialog5.FileName;
				}
				else
				{
					State.Data = MTKEMI.Text;
				}
				DeviceState.Partitiontarget.Add(text35);
			}
		}
		else
		{
			MTKDataGrid.Rows.Clear();
			CMTKPart.Checked = false;
			MTKFirmware.Clear();
			MtkSettings.ListMtk = new List<MtkSettings>();
			GptAddr.Addr = new List<GptAddr>();
		}
		State.Operation = simpleButton.Text;
		if (!simpleButton.Name.Contains(text3))
		{
			RunOperation();
		}
		else
		{
			RunReboot();
		}
	}

	private void METAHandler(object sender, EventArgs e)
	{
		string text = "METAReadGPT";
		string text2 = "METAReboot";
		string text3 = "METAWriteIMEI";
		string text4 = "METAReset";
		string text5 = "METAFrp";
		string text6 = "METAPayjoy";
		string text7 = "METASecplugin";
		string text8 = "METAEnableADB";
		string text9 = "METADisableADB";
		string text10 = "metaFactoryReset";
		string text11 = "SafeFormat";
		string metaMode = "Andproid";
		string metaMode2 = "Transsion";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = (SelectMode)2;
		string name = simpleButton.Name;
		uint num = ImplementationDetails.method_01(name);
		Job job;
		if (num <= 1429602552)
		{
			if (num <= 152714802)
			{
				if (num == 120981099)
				{
					if (name == text3)
					{
						job = Job.ImeiWrite;
						goto IL_006f;
					}
				}
				else if (num == 152714802 && name == text7)
				{
					job = (Job)70;
					goto IL_006f;
				}
			}
			else if (num == 463749593)
			{
				if (name == text2)
				{
					job = (Job)80;
					goto IL_006f;
				}
			}
			else if (num == 1284709456)
			{
				if (name == text8)
				{
					job = Job.AdbEnable;
					goto IL_006f;
				}
			}
			else if (num == 1429602552 && name == text6)
			{
				job = Job.Payjoy;
				goto IL_006f;
			}
		}
		else if (num > 2781246745u)
		{
			if (num != 2982384952u)
			{
				if (num == 3080822523u)
				{
					if (name == text11)
					{
						job = (Job)14;
						goto IL_006f;
					}
				}
				else if (num == 3809626407u && name == text4)
				{
					job = Job.Reset;
					goto IL_006f;
				}
			}
			else if (name == text5)
			{
				job = Job.Frp;
				goto IL_006f;
			}
		}
		else if (num == 1806473497)
		{
			if (name == text10)
			{
				job = Job.Erase;
				goto IL_006f;
			}
		}
		else if (num == 2163647237u)
		{
			if (name == text)
			{
				job = (Job)2;
				goto IL_006f;
			}
		}
		else if (num == 2781246745u && name == text9)
		{
			job = Job.AdbEnable;
			goto IL_006f;
		}
		job = (Job)0;
		goto IL_006f;
		IL_006f:
		State.Job = job;
		if (State.Job == Job.ImeiWrite)
		{
			using IMEI iMEI = UI.Showcust(new IMEI(edl: false, meta: true));
			if (iMEI.ShowDialog() != DialogResult.Yes)
			{
				return;
			}
		}
		State.Operation = simpleButton.Text;
		if (!object.Equals(simpleButton.Name, text10))
		{
			if (object.Equals(simpleButton.Name, text11))
			{
				DeviceState.MetaMode = metaMode;
			}
			else
			{
				DeviceState.MetaMode = metaMode2;
			}
		}
		else
		{
			DeviceState.MetaMode = metaMode;
		}
		RunOperation();
	}

	private void HisiHandler(object sender, EventArgs e)
	{
		string text = "HISIResetID";
		string text2 = "HISIResetFrp";
		string text3 = "HISIResetlock";
		string text4 = "HISIBLUnlock";
		SimpleButton simpleButton = sender as SimpleButton;
		if (simpleButton == null || JobStatus.Busy)
		{
			return;
		}
		string name = simpleButton.Name;
		string text5;
		if (!(name == text))
		{
			if (!(name == text2))
			{
				if (name == text3)
				{
					State.Job = Job.Reset;
				}
				else if (name == text4)
				{
					State.Job = (Job)54;
				}
				else
				{
					State.Job = (Job)0;
				}
				text5 = (string)CCPUHisi.SelectedItem;
				HisiConfig.CPUType = text5.Substring(text5.LastIndexOf(' ') + 1);
				State.Mode = (SelectMode)6;
				State.Operation = simpleButton.Text;
				RunOperation();
				return;
			}
			State.Job = Job.Frp;
		}
		else
		{
			State.Job = Job.ID;
		}
		text5 = (string)CCPUHisi.SelectedItem;
		HisiConfig.CPUType = text5.Substring(text5.LastIndexOf(' ') + 1);
		State.Mode = (SelectMode)6;
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void SPDDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == SPDDataGrid.Columns[0].Index)
		{
			MTKDataGrid.EndEdit();
		}
	}

	private void SPDDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!JobStatus.Busy && e.Button == MouseButtons.Right && e.RowIndex >= 0)
		{
			SPDDataGrid.ClearSelection();
			SPDDataGrid.Rows[e.RowIndex].Selected = true;
			SPDDataGrid.CurrentCell = SPDDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
			PopSPD.ShowPopup(Cursor.Position);
		}
	}

	private void CBrandSPD_SelectedIndexChanged(object sender, EventArgs e)
	{
		CModelSPD.Properties.Items.Clear();
		ListModelSPD = Server.LoadSPDModel((string)CBrandSPD.SelectedItem);
		if (ListModelSPD.Count != 0)
		{
			ComboBoxItemCollection items = CModelSPD.Properties.Items;
			object[] items2 = ListModelSPD.Select((Server.ModelConfigSPD m) => m.Model).ToArray();
			items.AddRange(items2);
			CModelSPD.SelectedIndex = 0;
		}
	}

	private void CModelSPD_SelectedIndexChanged(object sender, EventArgs e)
	{
		object obj = null;
		Server.ModelConfigSPD modelConfigSPD = ListModelSPD.FirstOrDefault((Server.ModelConfigSPD x) => x.Model == (string)CModelSPD.SelectedItem);
		if (modelConfigSPD != obj)
		{
			ApplyConfigSPD(modelConfigSPD);
		}
	}

	private void SPDOption(object sender, ItemClickEventArgs e)
	{
		object obj = null;
		string text = "";
		string text2 = "SPDWrite";
		string text3 = "SPDRead";
		string text4 = "SPDFormat";
		int num = 1;
		string text5 = "Select file want to write to ";
		string filter = "All Files|*.*";
		int num2 = 0;
		int job = 8;
		string text6 = "Save ";
		string text7 = ".bin";
		int job2 = 9;
		int job3 = 11;
		int mode = 5;
		if (JobStatus.Busy)
		{
			return;
		}
		BarItem item = e.Item;
		if (item == obj)
		{
			return;
		}
		string text8 = text;
		PopSPD.HidePopup();
		string name = item.Name;
		string text9 = name;
		if (!(text9 == text2))
		{
			if (!(text9 == text3))
			{
				if (text9 == text4)
				{
					foreach (DataGridViewRow selectedRow in SPDDataGrid.SelectedRows)
					{
						DeviceState.Partitiontarget.Add((string)selectedRow.Cells[1].Value);
					}
					State.Job = (Job)job3;
				}
			}
			else
			{
				foreach (DataGridViewRow selectedRow2 in SPDDataGrid.SelectedRows)
				{
					DeviceState.Partitiontarget.Add((string)selectedRow2.Cells[1].Value);
					text8 = (string)selectedRow2.Cells[1].Value;
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Title = text6 + text8,
					Filter = filter,
					FileName = text8 + text7
				})
				{
					if (((saveFileDialog.ShowDialog() == (DialogResult)num) ? 1 : 0) == num2)
					{
						return;
					}
					State.SavePath = saveFileDialog.FileName;
				}
				State.Job = (Job)job2;
			}
		}
		else
		{
			foreach (DataGridViewRow selectedRow3 in SPDDataGrid.SelectedRows)
			{
				DeviceState.Partitiontarget.Add((string)selectedRow3.Cells[1].Value);
				text8 = (string)selectedRow3.Cells[1].Value;
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = text5 + text8,
				Filter = filter
			})
			{
				if (((openFileDialog.ShowDialog() == (DialogResult)num) ? 1 : 0) == num2)
				{
					return;
				}
				State.FilePath = openFileDialog.FileName;
			}
			State.Job = (Job)job;
		}
		State.Operation = item.ToString();
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void SpreadtrumATDiag(object sender, EventArgs e)
	{
		string text = "FactoryReset";
		string text2 = "FactoryReset2";
		SimpleButton simpleButton = sender as SimpleButton;
		if (simpleButton == null || JobStatus.Busy)
		{
			return;
		}
		State.Mode = SelectMode.ATDiag;
		string name = simpleButton.Name;
		if (!(name == text))
		{
			if (name == text2)
			{
				State.Job = (Job)4;
			}
			else
			{
				State.Job = (Job)0;
			}
		}
		else
		{
			State.Job = Job.ATReset;
		}
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void SpreadtrumHandler(object sender, EventArgs e)
	{
		string text = "SPDReadGPT";
		string text2 = "SPDReboot";
		string text3 = "SPDResetLock";
		string text4 = "SPDResetFrp";
		if (sender is SimpleButton simpleButton && !JobStatus.Busy)
		{
			State.Mode = SelectMode.Spreadtrum;
			string name = simpleButton.Name;
			Job job = ((name == text) ? ((Job)2) : ((name == text2) ? ((Job)80) : ((name == text3) ? Job.Reset : ((name == text4) ? Job.Frp : ((Job)0)))));
			State.Job = job;
			if (State.Job == (Job)2)
			{
				SPDDataGrid.Rows.Clear();
			}
			State.Operation = simpleButton.Text;
			RunOperation();
		}
	}

	private string LoadTar(TextEdit txtfirmware, string filename)
	{
		string filter = "TAR or TAR.MD5|*.tar;*.tar.md5";
		string text = "Select ";
		string text2 = " File";
		string type = "BL";
		string type2 = "AP";
		string type3 = "CP";
		string type4 = "CSC";
		string type5 = "HOME";
		string result = "";
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = filter;
		openFileDialog.Title = text + filename + text2;
		openFileDialog.RestoreDirectory = true;
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return result;
		}
		string path = (txtfirmware.Text = openFileDialog.FileName);
		string fileName2 = Path.GetFileName(path);
		OdinGridView.Rows.Clear();
		AddTarEntries(txtBL.Text, type);
		AddTarEntries(txtAP.Text, type2);
		AddTarEntries(txtCP.Text, type3);
		AddTarEntries(txtCSC.Text, type4);
		AddTarEntries(txtHOME.Text, type5);
		return fileName2;
	}

	private void AddTarEntries(string tarPath, string type)
	{
		if (string.IsNullOrWhiteSpace(tarPath) || !File.Exists(tarPath))
		{
			return;
		}
		Tar tar = new Tar();
		IList<TarEntryInfo> entries = tar.GetEntries(tarPath);
		if (entries == null || entries.Count == 0)
		{
			return;
		}
		foreach (TarEntryInfo item in entries)
		{
			if (IsFlashableEntry(item, includePit: true))
			{
				string text = NormalizePartitionName(item.Name);
				OdinGridView.Rows.Add(true, type, text, item.Name, utils.FileSize(item.Size), item.DataStartOffset, item.DataEndOffset, tarPath);
			}
		}
	}

	private string NormalizePartitionName(string filename)
	{
		string value = ".lz4";
		if (!string.IsNullOrWhiteSpace(filename))
		{
			string text = filename.Replace('\\', '/');
			int num = text.LastIndexOf('/');
			bool flag;
			if (num < 0 || num >= text.Length - 1)
			{
				flag = text.EndsWith(value, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				text = text.Substring(num + 1);
				flag = text.EndsWith(value, StringComparison.OrdinalIgnoreCase);
			}
			if (!flag)
			{
				return text;
			}
			return text.Substring(0, text.Length - 4);
		}
		return string.Empty;
	}

	private bool IsFlashableEntry(TarEntryInfo entry, bool includePit)
	{
		object obj = null;
		string b = "Directory";
		string text = ".img";
		string text2 = ".bin";
		string text3 = ".lz4";
		string text4 = ".pit";
		string b2 = "pit";
		if (entry == obj)
		{
			return false;
		}
		if (!string.Equals(entry.Type, b, StringComparison.OrdinalIgnoreCase))
		{
			if (!string.IsNullOrWhiteSpace(entry.Name))
			{
				string text5 = Path.GetExtension(entry.Name).ToLowerInvariant();
				if (!(text5 == text) && !(text5 == text2) && !(text5 == text3))
				{
					if (includePit)
					{
						if (text5 == text4)
						{
							return true;
						}
						string fileName = Path.GetFileName(entry.Name);
						if (!string.Equals(fileName, b2, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
						return true;
					}
					return false;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	private void OdinClearfirmware(object sender, EventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		txtAP.Clear();
		txtBL.Clear();
		txtCP.Clear();
		txtCSC.Clear();
		txtHOME.Clear();
		OdinGridView.Rows.Clear();
	}

	private void OdinLoad(object sender, EventArgs e)
	{
		string text = "LoadBL";
		string text2 = "LoadAP";
		string text3 = "LoadCP";
		string text4 = "LoadCSC";
		string text5 = "LoadHome";
		string filename = "BL";
		string filename2 = "AP";
		string filename3 = "CP";
		string filename4 = "CSC";
		string filename5 = "HOME";
		SimpleButton simpleButton = sender as SimpleButton;
		if (simpleButton == null || JobStatus.Busy)
		{
			return;
		}
		string name = simpleButton.Name;
		string text6 = name;
		if (!(text6 == text))
		{
			if (text6 == text2)
			{
				LoadTar(txtAP, filename2);
			}
			else if (text6 == text3)
			{
				LoadTar(txtCP, filename3);
			}
			else if (!(text6 == text4))
			{
				if (text6 == text5)
				{
					LoadTar(txtHOME, filename5);
				}
			}
			else
			{
				LoadTar(txtCSC, filename4);
			}
		}
		else
		{
			LoadTar(txtBL, filename);
		}
	}

	private List<OdinList> GetCheckedSelection()
	{
		List<OdinList> list = new List<OdinList>();
		foreach (DataGridViewRow item in (IEnumerable)OdinGridView.Rows)
		{
			if (item == null || item.IsNewRow)
			{
				continue;
			}
			bool flag = false;
			bool result;
			if (item.Cells[0].Value is bool flag2)
			{
				flag = flag2;
			}
			else if (item.Cells[0].Value != null && bool.TryParse(item.Cells[0].Value.ToString(), out result))
			{
				flag = result;
			}
			if (flag)
			{
				string text = Convert.ToString(item.Cells[1].Value);
				string text2 = Convert.ToString(item.Cells[2].Value);
				string text3 = Convert.ToString(item.Cells[3].Value);
				string text4 = Convert.ToString(item.Cells[7].Value);
				if (!string.IsNullOrWhiteSpace(text3) && !string.IsNullOrWhiteSpace(text4))
				{
					long result2 = 0L;
					long result3 = 0L;
					long.TryParse(Convert.ToString(item.Cells[5].Value), out result2);
					long.TryParse(Convert.ToString(item.Cells[6].Value), out result3);
					list.Add(new OdinList
					{
						Type = (text ?? string.Empty),
						PartitionName = (text2 ?? string.Empty),
						EntryName = text3,
						TarPath = text4,
						BeginAddress = result2,
						EndAddress = result3
					});
				}
			}
		}
		return list.OrderBy((OdinList x) => GetTypeOrder(x.Type)).ThenBy((OdinList x) => x.EntryName, StringComparer.OrdinalIgnoreCase).ToList();
		static int GetTypeOrder(string type)
		{
			object obj = null;
			string text5 = "BL";
			string text6 = "AP";
			string text7 = "CP";
			string text8 = "CSC";
			string text9 = "HOME";
			if (type == obj)
			{
				return 100;
			}
			string text10 = type.Trim().ToUpperInvariant();
			if (text10 == text5)
			{
				return 0;
			}
			if (text10 == text6)
			{
				return 1;
			}
			if (text10 == text7)
			{
				return 2;
			}
			if (text10 == text8)
			{
				return 3;
			}
			if (!(text10 == text9))
			{
				return 100;
			}
			return 4;
		}
	}

	private void OdinHandler(object sender, EventArgs e)
	{
		string text = "OdinFlash";
		string text2 = "OdinFrp";
		string text3 = "OdinReset";
		string text4 = "OdinReadPit";
		string text5 = "OdinWritePit";
		string text6 = "OdinSoftbrick";
		string text7 = "Reset2024";
		string text8 = "OdinReboot";
		int num = 7;
		string title = "Load PIT File";
		string filter = "Pit|*.*";
		int num2 = 0;
		string text9 = "No selected entry in firmware table.";
		string caption = "Warning";
		int icon = 48;
		int mode = 8;
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		string name = simpleButton.Name;
		string text10 = name;
		uint num3 = ImplementationDetails.method_01(text10);
		if (num3 <= 2625764918u)
		{
			if (num3 <= 413662204)
			{
				if (num3 == 226062132)
				{
					if (text10 == text8)
					{
						State.Job = (Job)80;
					}
				}
				else if (num3 == 413662204 && text10 == text7)
				{
					State.Job = Job.FRP2024;
				}
			}
			else if (num3 == 1974645820)
			{
				if (text10 == text3)
				{
					State.Job = Job.Reset;
				}
			}
			else if (num3 == 2625764918u && text10 == text6)
			{
				State.Job = (Job)88;
			}
		}
		else if (num3 > 2690601359u)
		{
			if (num3 != 3252744213u)
			{
				if (num3 == 4096338670u && text10 == text4)
				{
					State.Job = (Job)86;
				}
			}
			else if (text10 == text5)
			{
				State.Job = Job.PitWrite;
			}
		}
		else
		{
			switch (num3)
			{
			case 2690601359u:
				if (text10 == text2)
				{
					State.Job = Job.Frp;
				}
				break;
			case 2628564253u:
				if (text10 == text)
				{
					State.Job = Job.Flash;
				}
				break;
			}
		}
		if (State.Job == Job.PitWrite)
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = title,
				Filter = filter
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			OdinConfig.PitPath = openFileDialog.FileName;
		}
		if (State.Job == (Job)num)
		{
			List<OdinList> checkedSelection = GetCheckedSelection();
			if (checkedSelection.Count == num2)
			{
				XtraMessageBox.Show(text9, caption, (MessageBoxButtons)num2, (MessageBoxIcon)icon);
				return;
			}
			OdinFlashRequest.ConfigureFlash(checkedSelection);
		}
		State.Operation = simpleButton.Text;
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void COdinRepartition_CheckedChanged(object sender, EventArgs e)
	{
		OdinConfig.Repartition = COdinRepartition.Checked;
	}

	private void COdinEraseEFS_CheckedChanged(object sender, EventArgs e)
	{
		OdinConfig.EraseEFS = COdinEraseEFS.Checked;
	}

	private void MTPHandler(object sender, EventArgs e)
	{
		string text = "MTPInfo";
		string text2 = "MTPFrp";
		string text3 = "MTPFrp16";
		string text4 = "MTPReset";
		string text5 = "MTPSN";
		string text6 = "MTPRebootOdin";
		int mode = 7;
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		string name = simpleButton.Name;
		Job job = ((name == text) ? Job.Readinfo : ((name == text2) ? Job.Frp : ((name == text3) ? ((Job)16) : ((name == text4) ? Job.Reset : ((name == text5) ? Job.SerialWrite : ((name == text6) ? ((Job)80) : State.Job))))));
		State.Job = job;
		if (State.Job == Job.SerialWrite)
		{
			using SerialNumber serialNumber = UI.Showcust(new SerialNumber());
			if (serialNumber.ShowDialog() != DialogResult.Yes)
			{
				return;
			}
		}
		State.Operation = simpleButton.Text;
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void CAndroidPart_CheckedChanged(object sender, EventArgs e)
	{
		if (JobStatus.Busy)
		{
			return;
		}
		foreach (DataGridViewRow item in (IEnumerable)AndroidDataGrid.Rows)
		{
			item.Cells[0].Value = CAndroidPart.Checked;
		}
	}

	private void SideloadHandler(object sender, EventArgs e)
	{
		string text = "ResetFRPMIasis";
		string text2 = "SDresetLock";
		string text3 = "SDresetMI";
		string text4 = "SDdisabaleMI";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = (SelectMode)12;
		string name = simpleButton.Name;
		if (!(name == text))
		{
			if (!(name == text2))
			{
				if (name == text3)
				{
					State.Job = (Job)58;
				}
				else
				{
					if (!(name == text4))
					{
						State.Job = (Job)0;
						State.Operation = simpleButton.Text;
						RunOperation();
						return;
					}
					State.Job = Job.Micloud;
				}
				State.Operation = simpleButton.Text;
				RunOperation();
				return;
			}
			State.Job = Job.Reset;
		}
		else
		{
			State.Job = Job.Frp;
		}
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void ADBHandler(object sender, EventArgs e)
	{
		string text = "ADBLoadApk";
		string text2 = "ADBInstallApk";
		string text3 = "ADBUninstallApk";
		string text4 = "ADBDisableApk";
		string text5 = "ADBEnableApk";
		string text6 = "ADBSamId";
		string text7 = "EraseFRP";
		string text8 = "ADBSamFrp";
		string text9 = "EraseFRP16";
		string text10 = "SamsungID";
		string text11 = "DidableMicloud";
		string text12 = "EnablediagXiaomi";
		string text13 = "EnableDiagRoot";
		string text14 = "EnableDiagOperation";
		string text15 = "DisablePengecualian";
		string text16 = "FixAssistantWIFI";
		string text17 = "Disaba_OSOppo";
		string text18 = "Disable_OSMI";
		string text19 = "Disable_OS_Sam";
		string text20 = "Disable_osvivo";
		string text21 = "Reset_lock";
		string text22 = "Remove_Payjoy";
		string text23 = "RemovePlugin";
		string text24 = "ResetHuaweiID";
		string text25 = "DisabaleOSRealme";
		string text26 = "DisableDemo";
		string text27 = "DisableOSItel";
		string text28 = "DisableOSTecno";
		string text29 = "RemoveMDM";
		string text30 = "OppoSwith";
		string text31 = "rebootdevice";
		string text32 = "RemoveDemo";
		string text33 = "ADBDisableHU";
		string text34 = "DisableKnox";
		string text35 = "SamsungAccount";
		string text36 = "ResetFRPMIasis";
		string title = "Load Apk";
		string filter = "Apk|*.apk";
		object obj = null;
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = SelectMode.Adb;
		string name = simpleButton.Name;
		uint num = ImplementationDetails.method_01(name);
		Job job;
		if (num > 1697360712)
		{
			if (num <= 3017901098u)
			{
				if (num > 2385470990u)
				{
					if (num <= 2700667416u)
					{
						if (num != 2577657968u)
						{
							if (num == 2700667416u && name == text6)
							{
								job = Job.ID;
								goto IL_038a;
							}
						}
						else if (name == text24)
						{
							job = Job.ResetHuaweiID;
							goto IL_038a;
						}
					}
					else if (num != 2718862101u)
					{
						if (num != 2790545155u)
						{
							if (num == 3017901098u && name == text36)
							{
								job = (Job)38;
								goto IL_038a;
							}
						}
						else if (name == text7)
						{
							job = Job.Frp;
							goto IL_038a;
						}
					}
					else if (name == text16)
					{
						job = (Job)18;
						goto IL_038a;
					}
				}
				else if (num > 2154005912u)
				{
					if (num != 2290355928u)
					{
						if (num == 2385470990u && name == text22)
						{
							job = Job.RemovePayjoy;
							goto IL_038a;
						}
					}
					else if (name == text28)
					{
						job = Job.DisableOSTECNO;
						goto IL_038a;
					}
				}
				else if (num != 1941180611)
				{
					if (num == 2154005912u && name == text18)
					{
						job = (Job)20;
						goto IL_038a;
					}
				}
				else if (name == text29)
				{
					job = (Job)30;
					goto IL_038a;
				}
			}
			else if (num <= 3390101684u)
			{
				if (num > 3305980080u)
				{
					if (num != 3353244820u)
					{
						if (num == 3390101684u && name == text)
						{
							job = (Job)90;
							goto IL_038a;
						}
					}
					else if (name == text17)
					{
						job = Job.DisabaOSOppo;
						goto IL_038a;
					}
				}
				else if (num != 3024323646u)
				{
					if (num == 3305980080u && name == text12)
					{
						job = (Job)66;
						goto IL_038a;
					}
				}
				else if (name == text3)
				{
					job = (Job)92;
					goto IL_038a;
				}
			}
			else if (num <= 3520339128u)
			{
				if (num != 3423517675u)
				{
					if (num == 3520339128u && name == text25)
					{
						job = (Job)26;
						goto IL_038a;
					}
				}
				else if (name == text8)
				{
					job = Job.Frp;
					goto IL_038a;
				}
			}
			else if (num != 3650613754u)
			{
				if (num != 3663779849u)
				{
					if (num == 3668163013u && name == text11)
					{
						job = Job.Micloud;
						goto IL_038a;
					}
				}
				else if (name == text13)
				{
					job = (Job)64;
					goto IL_038a;
				}
			}
			else if (name == text21)
			{
				job = Job.ResetLock;
				goto IL_038a;
			}
		}
		else if (num <= 620379357)
		{
			if (num > 204129132)
			{
				if (num <= 377647682)
				{
					if (num != 311311896)
					{
						if (num == 377647682 && name == text23)
						{
							job = (Job)24;
							goto IL_038a;
						}
					}
					else if (name == text30)
					{
						job = (Job)32;
						goto IL_038a;
					}
				}
				else if (num != 531740046)
				{
					if (num != 608697138)
					{
						if (num == 620379357 && name == text2)
						{
							job = Job.ApkInstall;
							goto IL_038a;
						}
					}
					else if (name == text31)
					{
						job = Job.RebootADB;
						goto IL_038a;
					}
				}
				else if (name == text32)
				{
					job = (Job)34;
					goto IL_038a;
				}
			}
			else if (num <= 155789426)
			{
				if (num != 76420386)
				{
					if (num == 155789426 && name == text19)
					{
						job = Job.DisableOSSamsung;
						goto IL_038a;
					}
				}
				else if (name == text14)
				{
					job = Job.DiagEnable;
					goto IL_038a;
				}
			}
			else if (num != 172763541)
			{
				if (num == 204129132 && name == text4)
				{
					job = Job.ApkDisable;
					goto IL_038a;
				}
			}
			else if (name == text5)
			{
				job = (Job)94;
				goto IL_038a;
			}
		}
		else if (num <= 1289609386)
		{
			if (num <= 1024353176)
			{
				if (num != 822606477)
				{
					if (num == 1024353176 && name == text20)
					{
						job = (Job)22;
						goto IL_038a;
					}
				}
				else if (name == text15)
				{
					job = Job.DisablePengecualian;
					goto IL_038a;
				}
			}
			else if (num != 1266474898)
			{
				if (num == 1289609386 && name == text35)
				{
					job = Job.ADBSamsungAccount;
					goto IL_038a;
				}
			}
			else if (name == text10)
			{
				job = Job.ID;
				goto IL_038a;
			}
		}
		else if (num <= 1521390893)
		{
			if (num != 1317618304)
			{
				if (num == 1521390893 && name == text33)
				{
					job = Job.ADBDisableOSHU;
					goto IL_038a;
				}
			}
			else if (name == text9)
			{
				job = (Job)16;
				goto IL_038a;
			}
		}
		else if (num != 1545956361)
		{
			if (num != 1592000061)
			{
				if (num == 1697360712 && name == text26)
				{
					job = Job.ErAasedemo;
					goto IL_038a;
				}
			}
			else if (name == text34)
			{
				job = (Job)36;
				goto IL_038a;
			}
		}
		else if (name == text27)
		{
			job = Job.ErAasedemo;
			goto IL_038a;
		}
		job = (Job)0;
		goto IL_038a;
		IL_038a:
		State.Job = job;
		if (State.Job == Job.ApkInstall)
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = title,
				Filter = filter
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			State.FilePath = openFileDialog.FileName;
		}
		else if (State.Job == Job.ApkDisable || State.Job == (Job)92 || State.Job == (Job)94)
		{
			bool flag = default(bool);
			foreach (DataGridViewRow item in (IEnumerable)AndroidDataGrid.Rows)
			{
				object value = item.Cells[0].Value;
				int num2;
				if (value is bool)
				{
					flag = (bool)value;
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
				if (((uint)num2 & (flag ? 1u : 0u)) != 0)
				{
					List<string> partitiontarget = DeviceState.Partitiontarget;
					object value2 = item.Cells[1].Value;
					partitiontarget.Add((string)((value2 != null) ? value2.ToString() : obj));
				}
			}
		}
		else if (State.Job == (Job)2)
		{
			AndroidDataGrid.Rows.Clear();
		}
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void FastbootDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == FastbootDataGrid.Columns[0].Index)
		{
			FastbootDataGrid.EndEdit();
		}
	}

	private void FastbootDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!JobStatus.Busy && e.Button == MouseButtons.Right && e.RowIndex >= 0)
		{
			FastbootDataGrid.ClearSelection();
			FastbootDataGrid.Rows[e.RowIndex].Selected = true;
			FastbootDataGrid.CurrentCell = FastbootDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
			PopFB.ShowPopup(Cursor.Position);
		}
	}

	private void FastbootOption(object sender, ItemClickEventArgs e)
	{
		object obj = null;
		string text = "";
		string text2 = "FBWrite";
		string text3 = "FBFormat";
		int num = 1;
		string text4 = "Select file want to write to ";
		string filter = "All Files|*.*";
		int num2 = 0;
		int job = 8;
		int job2 = 11;
		int mode = 10;
		if (JobStatus.Busy)
		{
			return;
		}
		BarItem item = e.Item;
		if (item == obj)
		{
			return;
		}
		string text5 = text;
		PopFB.HidePopup();
		string name = item.Name;
		string text6 = name;
		if (!(text6 == text2))
		{
			if (text6 == text3)
			{
				foreach (DataGridViewRow selectedRow in FastbootDataGrid.SelectedRows)
				{
					DeviceState.Partitiontarget.Add((string)selectedRow.Cells[1].Value);
				}
				State.Job = (Job)job2;
			}
		}
		else
		{
			foreach (DataGridViewRow selectedRow2 in FastbootDataGrid.SelectedRows)
			{
				DeviceState.Partitiontarget.Add((string)selectedRow2.Cells[1].Value);
				text5 = (string)selectedRow2.Cells[1].Value;
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = text4 + text5,
				Filter = filter
			})
			{
				if (((openFileDialog.ShowDialog() == (DialogResult)num) ? 1 : 0) == num2)
				{
					return;
				}
				State.FilePath = openFileDialog.FileName;
			}
			State.Job = (Job)job;
		}
		State.Operation = item.ToString();
		State.Mode = (SelectMode)mode;
		RunOperation();
	}

	private void FastbootClear(object sender, EventArgs e)
	{
		FastbootFirmware.Clear();
	}

	private void FastbootLoad(object sender, EventArgs e)
	{
		string title = "Load Firmware";
		string filter = "Bat Files (*.bat)|*.bat";
		string path = "images";
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = title,
			Filter = filter
		};
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		State.FilePath = openFileDialog.FileName;
		Path.Combine(State.FilePath, path);
		FastbootDataGrid.Rows.Clear();
		FastbootFirmware.Text = Path.GetDirectoryName(openFileDialog.FileName);
		List<ParseBatFile.BatEntry> list = ParseBatFile.ParseFastbootBat(openFileDialog.FileName);
		foreach (ParseBatFile.BatEntry item in list)
		{
			if (File.Exists(item.Location))
			{
				FastbootDataGrid.Rows.Add(true, item.Partition, item.FileName, utils.FileSize(item.FileSize), item.Location, item.Command);
			}
			else
			{
				FastbootDataGrid.Rows.Add(false, item.Partition, item.FileName, utils.FileSize(item.FileSize), item.Location, item.Command);
			}
		}
	}

	private void CPartFastboot_CheckedChanged(object sender, EventArgs e)
	{
		if (CFastbootPart.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)FastbootDataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)FastbootDataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void FastbootHandler(object sender, EventArgs e)
	{
		int mode = 10;
		string text = "FBReadGPT";
		string text2 = "FBEFSErase";
		string text3 = "FBResetFrp";
		string text4 = "FBResetLock";
		string text5 = "FBSlotA";
		string text6 = "FBSlotB";
		string text7 = "FBDMVerity";
		string text8 = "FBReboot";
		string text9 = "FBFlash";
		string text10 = "FBRebootBL";
		string text11 = "FBRebootEDL";
		string text12 = "FBRebootRecovery";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		State.Mode = (SelectMode)10;
		string name = simpleButton.Name;
		uint num = ImplementationDetails.method_01(name);
		Job job;
		if (num > 2248697763u)
		{
			if (num <= 3357867931u)
			{
				if (num != 2338743913u)
				{
					if (num != 2714525498u)
					{
						if (num == 3357867931u && name == text11)
						{
							job = Job.RebootEDL;
							goto IL_015e;
						}
					}
					else if (name == text10)
					{
						job = (Job)84;
						goto IL_015e;
					}
				}
				else if (name == text7)
				{
					job = (Job)78;
					goto IL_015e;
				}
			}
			else if (num != 3384557675u)
			{
				if (num != 4103305942u)
				{
					if (num == 4211370765u && name == text9)
					{
						job = Job.Flash;
						goto IL_015e;
					}
				}
				else if (name == text)
				{
					job = (Job)2;
					goto IL_015e;
				}
			}
			else if (name == text2)
			{
				job = (Job)42;
				goto IL_015e;
			}
		}
		else if (num <= 1357886188)
		{
			if (num != 95962843)
			{
				if (num != 571648484)
				{
					if (num == 1357886188 && name == text5)
					{
						job = Job.SetBootSlotA;
						goto IL_015e;
					}
				}
				else if (name == text8)
				{
					job = (Job)80;
					goto IL_015e;
				}
			}
			else if (name == text12)
			{
				job = (Job)82;
				goto IL_015e;
			}
		}
		else if (num != 1408219045)
		{
			if (num != 1753164700)
			{
				if (num == 2248697763u && name == text4)
				{
					job = Job.Reset;
					goto IL_015e;
				}
			}
			else if (name == text3)
			{
				job = Job.Frp;
				goto IL_015e;
			}
		}
		else if (name == text6)
		{
			job = (Job)96;
			goto IL_015e;
		}
		job = (Job)0;
		goto IL_015e;
		IL_015e:
		State.Job = job;
		if (State.Job == Job.Flash)
		{
			bool flag = default(bool);
			foreach (DataGridViewRow item in (IEnumerable)FastbootDataGrid.Rows)
			{
				object value = item.Cells[0].Value;
				int num2;
				if (!(value is bool))
				{
					num2 = 0;
				}
				else
				{
					flag = (bool)value;
					num2 = 1;
				}
				if (((uint)num2 & (flag ? 1u : 0u)) != 0)
				{
					FastbootList.Partitions.Add(new FastbootList((string)item.Cells[FastbootDataGrid.Columns[1].Index].Value, (string)item.Cells[FastbootDataGrid.Columns[2].Index].Value, (string)item.Cells[FastbootDataGrid.Columns[4].Index].Value, (string)item.Cells[FastbootDataGrid.Columns[5].Index].Value));
				}
			}
		}
		else if (State.Job == (Job)2)
		{
			FastbootDataGrid.Rows.Clear();
		}
		State.Operation = simpleButton.Text;
		State.Mode = (SelectMode)mode;
		if (object.Equals(simpleButton.Name, text8))
		{
			RunReboot();
		}
		else
		{
			RunOperation();
		}
	}

	private void ExtraHandler(object sender, EventArgs e)
	{
		string filter = "All Files (*.*)|*.*";
		string title = "Select NV File";
		string title2 = "Save File";
		string text = "NVDataEdit";
		string text2 = "NVRAMEdit";
		int mode = 11;
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = filter,
			Title = title
		};
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		State.FilePath = openFileDialog.FileName;
		using SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Filter = filter,
			Title = title2,
			FileName = Path.GetFileName(openFileDialog.FileName)
		};
		if (saveFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		State.SavePath = saveFileDialog.FileName;
		string name = simpleButton.Name;
		Job job = ((name == text) ? Job.ImeiWrite : ((name == text2) ? Job.ImeiWrite : ((Job)0)));
		State.Job = job;
		using (IMEI iMEI = UI.Showcust(new IMEI(edl: false, meta: false, universal: true)))
		{
			if (iMEI.ShowDialog() != DialogResult.Yes)
			{
				return;
			}
		}
		State.Mode = (SelectMode)mode;
		State.Operation = simpleButton.Text;
		RunOperation();
	}

	private void ApplyConfigMTK(Server.ModelConfigMTK cfg)
	{
		MTKConfig.Customda = cfg.Customda;
		MTKConfig.Needauthfile = cfg.AuthFile;
		MTKConfig.Danofarse = cfg.Danofarse;
	}

	private void ApplyConfigSPD(Server.ModelConfigSPD cfg)
	{
		string text = "CPU Conf : ";
		string text2 = "Model : ";
		string text3 = "CPU Current  : ";
		Console.WriteLine(text + cfg.SOC);
		Console.WriteLine(text2 + cfg.Model);
		SPDConfig.CPUType = cfg.SOC;
		Console.WriteLine(text3 + SPDConfig.CPUType);
	}

	private void Init()
	{
		string text = "Local DA";
		string text2 = "Custom DA = ";
		string text3 = "Need Auth File  = ";
		string text4 = "Preloader Method = ";
		string text5 = "Local DA = ";
		string text6 = "MTK Server = ";
		string text7 = "Repartition = ";
		string text8 = "High Speed USB = ";
		string brand = "Infinix-Itel-Tecno";
		string text9 = "Universal";
		string brand2 = "ADB Universal";
		string brand3 = "Sideload Universal";
		string text10 = "Local Firehose";
		string text11 = "Qualcomm Universal";
		string brand4 = "Huawei/Honor";
		string brand5 = "Samsung";
		string msg = "Operation : ";
		string msg2 = "Brand : ";
		string msg3 = "Model : ";
		string msg4 = "Selected Mode : ";
		string msg5 = "";
		if (State.Mode != SelectMode.Mediatek)
		{
			if (State.Mode != (SelectMode)2)
			{
				if (State.Mode != SelectMode.Adb)
				{
					if (State.Mode != (SelectMode)12)
					{
						if (State.Mode == SelectMode.ATDiag)
						{
							DeviceState.Brand = text9;
							DeviceState.Model = text9;
						}
						else if (State.Mode == SelectMode.Qualcomm)
						{
							if (DeviceState.Unlock)
							{
								DeviceState.Brand = (string)BrandUEDL.SelectedItem;
								DeviceState.Model = (string)LMEDL.SelectedItem;
							}
							else if (EDLConfig.EDLServer)
							{
								DeviceState.Brand = (string)EDLBrand.SelectedItem;
								DeviceState.Model = (string)EDLModel.SelectedItem;
							}
							else
							{
								DeviceState.Brand = text10;
								DeviceState.Model = text10;
							}
						}
						else if (State.Mode == (SelectMode)4)
						{
							DeviceState.Brand = text11;
							DeviceState.Model = text11;
						}
						else if (State.Mode == (SelectMode)6)
						{
							DeviceState.Brand = brand4;
							DeviceState.Model = (string)CCPUHisi.SelectedItem;
						}
						else if (State.Mode != SelectMode.MTP && State.Mode != (SelectMode)8)
						{
							if (State.Mode == (SelectMode)10)
							{
								DeviceState.Brand = text9;
								DeviceState.Model = text9;
							}
							else if (State.Mode == SelectMode.Spreadtrum)
							{
								DeviceState.Brand = (string)CBrandSPD.SelectedItem;
								DeviceState.Model = (string)CModelSPD.SelectedItem;
							}
						}
						else
						{
							DeviceState.Brand = brand5;
							DeviceState.Model = text9;
						}
					}
					else
					{
						DeviceState.Brand = brand3;
						DeviceState.Model = text9;
					}
				}
				else
				{
					DeviceState.Brand = brand2;
					DeviceState.Model = text9;
				}
			}
			else
			{
				DeviceState.Brand = brand;
				DeviceState.Model = text9;
			}
		}
		else
		{
			if (!MTKConfig.MTKServer)
			{
				DeviceState.Brand = text;
				DeviceState.Model = text;
			}
			else
			{
				DeviceState.Brand = (string)MTKBrand.SelectedItem;
				DeviceState.Model = (string)MTKModel.SelectedItem;
			}
			Console.WriteLine(text2 + MTKConfig.Customda);
			Console.WriteLine(text3 + MTKConfig.Needauthfile);
			Console.WriteLine(text4 + MTKConfig.PreloaderMethod);
			Console.WriteLine(text5 + MTKConfig.LocalDA);
			Console.WriteLine(text6 + MTKConfig.MTKServer);
			Console.WriteLine(text7 + MTKConfig.Repartition);
			Console.WriteLine(text8 + MTKConfig.Highspeed);
		}
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(State.Operation, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg2, UI.log);
		UI.RichLogs(DeviceState.Brand, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		UI.RichLogs(DeviceState.Model, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg4, UI.log);
		UI.RichLogs(State.Mode.ToString(), UI.info, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log, nextline: true, bold: true);
	}

	[AsyncStateMachine(typeof(_003CRunOperation_003Ed__81))]
	[DebuggerStepThrough]
	private Task RunOperation()
	{
		_003CRunOperation_003Ed__81 stateMachine = new _003CRunOperation_003Ed__81();
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		stateMachine._003C_003E4__this = this;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return stateMachine._003C_003Et__builder.Task;
	}

	[AsyncStateMachine(typeof(_003CRunReboot_003Ed__82))]
	[DebuggerStepThrough]
	private Task RunReboot()
	{
		_003CRunReboot_003Ed__82 stateMachine = new _003CRunReboot_003Ed__82();
		stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		stateMachine._003C_003E4__this = this;
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start(ref stateMachine);
		return stateMachine._003C_003Et__builder.Task;
	}

	private void ElapsedTime(Stopwatch Watch)
	{
		string format = "{0:00M}:{1:00S}";
		string msg = "————————————————————————————————————————————————————————";
		string text = "AMFTool ";
		string text2 = " ";
		string text3 = "ddd, dd MMM yyyy HH:mm:ss";
		string name = "id-ID";
		string text4 = "Elapsed Time : ";
		TimeSpan elapsed = UI.watch.Elapsed;
		string text5 = string.Format(format, elapsed.Minutes, elapsed.Seconds);
		UI.RichLogs(msg, UI.log, nextline: true, bold: true);
		UI.RichLogs(text + Versiontool.ToString() + text2 + DateTime.Now.ToString(text3, new CultureInfo(name)), UI.log, nextline: true, bold: true);
		UI.RichLogs(text4 + text5, UI.log, nextline: true, bold: true);
		Watch.Stop();
		UI.Progresbar1(100L, 100L);
		UI.KillProcess();
		DeviceState.Partitiontarget.Clear();
	}

	private void Stop_Click(object sender, EventArgs e)
	{
		SerialEDL.Close();
		JobStatus.Cancel();
		SerialMTK.Close();
		SerialMETA.Close();
		SerialSPD.Close();
		JobStatus.Busy = false;
	}

	private void CMTKPart_CheckedChanged(object sender, EventArgs e)
	{
		if (CMTKPart.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)MTKDataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)MTKDataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void MTKDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == MTKDataGrid.Columns[0].Index)
		{
			MTKDataGrid.EndEdit();
		}
	}

	private void MTKDataGrid_CellMouseDown_1(object sender, DataGridViewCellMouseEventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!JobStatus.Busy && e.Button == MouseButtons.Right && e.RowIndex >= 0)
		{
			MTKDataGrid.ClearSelection();
			MTKDataGrid.Rows[e.RowIndex].Selected = true;
			MTKDataGrid.CurrentCell = MTKDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
			PopMTK.ShowPopup(Cursor.Position);
		}
	}

	private void Main_Shown(object sender, EventArgs e)
	{
		string item = "Huawei-Honor";
		string item2 = "Infinix-Itel-Tecno";
		string item3 = "Oppo-Realme";
		string item4 = "Samsung";
		string item5 = "Vivo";
		string item6 = "Xiaomi";
		int num = 0;
		string text = "Controllers/preloader.json";
		int num2 = 1;
		string value = "[";
		int millisecondsTimeout = 200;
		int flags = 256;
		string name = "Add";
		object obj = null;
		int num3 = 2;
		string name2 = "Type";
		int operation = 83;
		string name3 = "ConditionalCompareObjectEqual";
		int num4 = 4;
		int flags2 = 33;
		int num5 = 3;
		string name4 = "url";
		MTKBrand.Properties.Items.Add(item);
		MTKBrand.Properties.Items.Add(item2);
		MTKBrand.Properties.Items.Add(item3);
		MTKBrand.Properties.Items.Add(item4);
		MTKBrand.Properties.Items.Add(item5);
		MTKBrand.Properties.Items.Add(item6);
		MTKModel.SelectedIndex = 0;
		CMTKServer.Checked = false;
		foreach (string brand in DeviceManager.GetBrands())
		{
			BrandUEDL.Properties.Items.Add(brand);
		}
		string address = UI.Server + text;
		if (((list_pldr_emi != null) ? ((((list_pldr_emi.Count > num) ? 1 : 0) == num) ? 1 : 0) : num2) == 0)
		{
			if (((list_pldr_emi.Count > num) ? 1 : 0) != num)
			{
				foreach (clsIcons item7 in list_pldr_emi)
				{
					if (_003C_003Eo__88._003C_003Ep__1 == null)
					{
						Type typeFromHandle = typeof(Main);
						CSharpArgumentInfo[] array = new CSharpArgumentInfo[num3];
						array[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
						array[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
						_003C_003Eo__88._003C_003Ep__1 = CallSite<Action<CallSite, ComboBoxItemCollection, object>>.Create(Binder.InvokeMember((CSharpBinderFlags)flags, name, (IEnumerable<Type>)obj, typeFromHandle, array));
					}
					Action<CallSite, ComboBoxItemCollection, object> target = _003C_003Eo__88._003C_003Ep__1.Target;
					CallSite<Action<CallSite, ComboBoxItemCollection, object>> _003C_003Ep__ = _003C_003Eo__88._003C_003Ep__1;
					ComboBoxItemCollection items = DaCustum.Properties.Items;
					if (_003C_003Eo__88._003C_003Ep__0 == null)
					{
						Type typeFromHandle2 = typeof(Main);
						CSharpArgumentInfo[] array2 = new CSharpArgumentInfo[num2];
						array2[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
						_003C_003Eo__88._003C_003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember((CSharpBinderFlags)num, name2, typeFromHandle2, array2));
					}
					target(_003C_003Ep__, items, _003C_003Eo__88._003C_003Ep__0.Target(_003C_003Eo__88._003C_003Ep__0, item7));
					DaCustum.SelectedIndex = num;
				}
				{
					foreach (clsIcons item8 in list_pldr_emi)
					{
						if (_003C_003Eo__88._003C_003Ep__4 == null)
						{
							Type typeFromHandle3 = typeof(Main);
							CSharpArgumentInfo[] array3 = new CSharpArgumentInfo[num2];
							array3[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
							_003C_003Eo__88._003C_003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation((CSharpBinderFlags)num, (ExpressionType)operation, typeFromHandle3, array3));
						}
						Func<CallSite, object, bool> target2 = _003C_003Eo__88._003C_003Ep__4.Target;
						CallSite<Func<CallSite, object, bool>> _003C_003Ep__2 = _003C_003Eo__88._003C_003Ep__4;
						if (_003C_003Eo__88._003C_003Ep__3 == null)
						{
							Type typeFromHandle4 = typeof(Main);
							CSharpArgumentInfo[] array4 = new CSharpArgumentInfo[num4];
							array4[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)flags2, (string)obj);
							array4[num2] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
							array4[num3] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num2, (string)obj);
							array4[num5] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num5, (string)obj);
							_003C_003Eo__88._003C_003Ep__3 = CallSite<Func<CallSite, Type, object, string, bool, object>>.Create(Binder.InvokeMember((CSharpBinderFlags)num, name3, (IEnumerable<Type>)obj, typeFromHandle4, array4));
						}
						Func<CallSite, Type, object, string, bool, object> target3 = _003C_003Eo__88._003C_003Ep__3.Target;
						CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__3 = _003C_003Eo__88._003C_003Ep__3;
						Type typeFromHandle5 = typeof(Operators);
						if (_003C_003Eo__88._003C_003Ep__2 == null)
						{
							Type typeFromHandle6 = typeof(Main);
							CSharpArgumentInfo[] array5 = new CSharpArgumentInfo[num2];
							array5[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
							_003C_003Eo__88._003C_003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember((CSharpBinderFlags)num, name2, typeFromHandle6, array5));
						}
						if (target2(_003C_003Ep__2, target3(_003C_003Ep__3, typeFromHandle5, _003C_003Eo__88._003C_003Ep__2.Target(_003C_003Eo__88._003C_003Ep__2, item8), DaCustum.Text, (byte)num != 0)))
						{
							if (_003C_003Eo__88._003C_003Ep__6 == null)
							{
								_003C_003Eo__88._003C_003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert((CSharpBinderFlags)num, typeof(string), typeof(Main)));
							}
							Func<CallSite, object, string> target4 = _003C_003Eo__88._003C_003Ep__6.Target;
							CallSite<Func<CallSite, object, string>> _003C_003Ep__4 = _003C_003Eo__88._003C_003Ep__6;
							if (_003C_003Eo__88._003C_003Ep__5 == null)
							{
								Type typeFromHandle7 = typeof(Main);
								CSharpArgumentInfo[] array6 = new CSharpArgumentInfo[num2];
								array6[num] = CSharpArgumentInfo.Create((CSharpArgumentInfoFlags)num, (string)obj);
								_003C_003Eo__88._003C_003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember((CSharpBinderFlags)num, name4, typeFromHandle7, array6));
							}
							Server.jsonmenemi = target4(_003C_003Ep__4, _003C_003Eo__88._003C_003Ep__5.Target(_003C_003Eo__88._003C_003Ep__5, item8));
						}
					}
					return;
				}
			}
			CMTKEMI.Checked = (byte)num != 0;
		}
		else
		{
			if (string.IsNullOrEmpty(Server.jsonmenemi))
			{
				Server.jsonmenemi = new WebClient().DownloadString(address);
			}
			if ((Server.jsonmenemi.StartsWith(value) ? 1 : 0) == num)
			{
				Thread.Sleep(millisecondsTimeout);
				Server.jsonmenemi = new WebClient().DownloadString(address);
			}
			list_pldr_emi = JsonConvert.DeserializeObject<List<clsIcons>>(Server.jsonmenemi);
		}
	}

	private void DaCustum_SelectedIndexChanged(object sender, EventArgs e)
	{
		object obj = null;
		string name = "ConditionalCompareObjectEqual";
		string name2 = "Type";
		string name3 = "url";
		if (!CMTKEMI.Checked)
		{
			return;
		}
		try
		{
			List<clsIcons> list = list_pldr_emi;
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (clsIcons item in list)
			{
				if (_003C_003Eo__89._003C_003Ep__2 == null)
				{
					_003C_003Eo__89._003C_003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Main), new CSharpArgumentInfo[1] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj) }));
				}
				Func<CallSite, object, bool> target = _003C_003Eo__89._003C_003Ep__2.Target;
				CallSite<Func<CallSite, object, bool>> _003C_003Ep__ = _003C_003Eo__89._003C_003Ep__2;
				if (_003C_003Eo__89._003C_003Ep__1 == null)
				{
					_003C_003Eo__89._003C_003Ep__1 = CallSite<Func<CallSite, Type, object, string, bool, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, name, (IEnumerable<Type>)obj, typeof(Main), new CSharpArgumentInfo[4]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string)obj),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string)obj),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string)obj)
					}));
				}
				Func<CallSite, Type, object, string, bool, object> target2 = _003C_003Eo__89._003C_003Ep__1.Target;
				CallSite<Func<CallSite, Type, object, string, bool, object>> _003C_003Ep__2 = _003C_003Eo__89._003C_003Ep__1;
				Type typeFromHandle = typeof(Operators);
				if (_003C_003Eo__89._003C_003Ep__0 == null)
				{
					_003C_003Eo__89._003C_003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, name2, typeof(Main), new CSharpArgumentInfo[1] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj) }));
				}
				if (target(_003C_003Ep__, target2(_003C_003Ep__2, typeFromHandle, _003C_003Eo__89._003C_003Ep__0.Target(_003C_003Eo__89._003C_003Ep__0, item), DaCustum.Text, arg5: false)))
				{
					if (_003C_003Eo__89._003C_003Ep__3 == null)
					{
						_003C_003Eo__89._003C_003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, name3, typeof(Main), new CSharpArgumentInfo[1] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string)obj) }));
					}
					Server.urlemi = (dynamic)_003C_003Eo__89._003C_003Ep__3.Target(_003C_003Eo__89._003C_003Ep__3, item);
				}
			}
			Console.WriteLine(Server.urlemi);
		}
		catch (Exception)
		{
		}
	}

	private void ShowALLMTK_CheckedChanged(object sender, EventArgs e)
	{
		string oldValue = ".bin";
		string newValue = "-verified.bin";
		string oldValue2 = ".img";
		string newValue2 = "-verified.img";
		string text = "preloader";
		string text2 = "true";
		MTKDataGrid.Rows.Clear();
		foreach (MtkSettings item in MtkSettings.ListMtk)
		{
			if (ShowALLMTK.Checked)
			{
				GptAddr.Addr.Add(new GptAddr(item.PartitionName, item.LinearStartAddr.ToHex(), item.PartitionSize.ToHex()));
				string text3 = Path.Combine(MtkSettings.PathScatter, item.FileName.Replace(oldValue, newValue).Replace(oldValue2, newValue2));
				string text4 = Path.Combine(MtkSettings.PathScatter, item.FileName);
				if (!File.Exists(text3))
				{
					if (!File.Exists(text4))
					{
						MTKDataGrid.Rows.Add(false, item.PartitionName, string.Empty, item.PartitionSize, item.LinearStartAddr, item.PartitionSize, string.Empty);
					}
					else
					{
						MTKDataGrid.Rows.Add(true, item.PartitionName, Path.GetFileName(text4), utils.FileSize(new FileInfo(text4).Length), item.LinearStartAddr, item.PartitionSize, text4);
					}
				}
				else
				{
					MTKDataGrid.Rows.Add(true, item.PartitionName, Path.GetFileName(text3), utils.FileSize(new FileInfo(text3).Length), item.LinearStartAddr, item.PartitionSize, text3);
				}
				if (item.PartitionName.ToLower() == text)
				{
					string text5 = ((!File.Exists(text3)) ? text4 : text3);
					if (File.Exists(text5))
					{
						MTKConfig.PathPreloader = text5;
						MTKEMI.Text = MTKConfig.PathPreloader;
					}
				}
			}
			else
			{
				if (!(item.IsDownload.ToLower() == text2))
				{
					continue;
				}
				GptAddr.Addr.Add(new GptAddr(item.PartitionName, item.LinearStartAddr.ToHex(), item.PartitionSize.ToHex()));
				string text6 = Path.Combine(MtkSettings.PathScatter, item.FileName.Replace(oldValue, newValue).Replace(oldValue2, newValue2));
				string text7 = Path.Combine(MtkSettings.PathScatter, item.FileName);
				if (File.Exists(text6))
				{
					MTKDataGrid.Rows.Add(true, item.PartitionName, Path.GetFileName(text6), utils.FileSize(new FileInfo(text6).Length), item.LinearStartAddr, item.PartitionSize, text6);
				}
				else if (File.Exists(text7))
				{
					MTKDataGrid.Rows.Add(true, item.PartitionName, Path.GetFileName(text7), utils.FileSize(new FileInfo(text7).Length), item.LinearStartAddr, item.PartitionSize, text7);
				}
				else
				{
					MTKDataGrid.Rows.Add(false, item.PartitionName, string.Empty, item.PartitionSize, item.LinearStartAddr, item.PartitionSize, string.Empty);
				}
				if (item.PartitionName.ToLower() == text)
				{
					string text8 = ((!File.Exists(text6)) ? text7 : text6);
					if (File.Exists(text8))
					{
						MTKConfig.PathPreloader = text8;
						MTKEMI.Text = MTKConfig.PathPreloader;
					}
				}
			}
		}
	}

	private void MTKDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		string format = "Choice {0}  file !";
		string format2 = "{0}  |*.*|Other|*.*";
		if (e.ColumnIndex == 2)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = string.Format(format, MTKDataGrid.CurrentRow.Cells[2].Value),
				Filter = string.Format(format2, MTKDataGrid.CurrentRow.Cells[2].Value)
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				new FileInfo(openFileDialog.FileName);
				MTKDataGrid.CurrentRow.Cells[2].Value = openFileDialog.SafeFileName;
				MTKDataGrid.CurrentRow.Cells[6].Value = openFileDialog.FileName;
				MTKDataGrid.CurrentRow.Cells[0].Value = true;
			}
		}
	}

	private void InfoFW_CheckedChanged(object sender, EventArgs e)
	{
		if (!InfoFW.Checked)
		{
			MTKConfig.Infirmware = false;
			Console.WriteLine(MTKConfig.Infirmware);
		}
		else
		{
			MTKConfig.Infirmware = true;
			Console.WriteLine(MTKConfig.Infirmware);
		}
	}

	private void Skippacth_CheckedChanged(object sender, EventArgs e)
	{
		if (!Skippacth.Checked)
		{
			PSDConfig.Pactloader = false;
			Console.WriteLine(PSDConfig.Pactloader);
		}
		else
		{
			PSDConfig.Pactloader = true;
			Console.WriteLine(PSDConfig.Pactloader);
		}
	}

	private void CFastbootPart_CheckedChanged(object sender, EventArgs e)
	{
		if (JobStatus.Busy)
		{
			return;
		}
		foreach (DataGridViewRow item in (IEnumerable)FastbootDataGrid.Rows)
		{
			item.Cells[0].Value = CFastbootPart.Checked;
		}
	}

	private void EDLinfofirmware_CheckedChanged(object sender, EventArgs e)
	{
		if (EDLinfofirmware.Checked)
		{
			MTKConfig.Infirmware = true;
			Console.WriteLine(MTKConfig.Infirmware);
		}
		else
		{
			MTKConfig.Infirmware = false;
			Console.WriteLine(MTKConfig.Infirmware);
		}
	}

	private void txtSearch_EditValueChanged(object sender, EventArgs e)
	{
		object currentCell = null;
		string text = "";
		string value = txtSearch.Text.Trim().ToLower();
		AndroidDataGrid.CurrentCell = (DataGridViewCell)currentCell;
		AndroidDataGrid.SuspendLayout();
		foreach (DataGridViewRow item in (IEnumerable)AndroidDataGrid.Rows)
		{
			if (!item.IsNewRow)
			{
				if (string.IsNullOrEmpty(value))
				{
					item.Visible = true;
				}
				else
				{
					string text2 = item.Cells[1].Value.ToString().ToLower() ?? text;
					item.Visible = text2.Contains(value);
				}
				AndroidDataGrid.ResumeLayout();
			}
		}
	}

	private void CAndroidPart_CheckedChanged_1(object sender, EventArgs e)
	{
		if (CAndroidPart.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)AndroidDataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)AndroidDataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void FastbootDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		string format = "Choice {0}  file !";
		string format2 = "{0}  |*.*|Other|*.*";
		if (e.ColumnIndex == 2)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = string.Format(format, FastbootDataGrid.CurrentRow.Cells[2].Value),
				Filter = string.Format(format2, FastbootDataGrid.CurrentRow.Cells[2].Value)
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				new FileInfo(openFileDialog.FileName);
				FastbootDataGrid.CurrentRow.Cells[2].Value = openFileDialog.SafeFileName;
				FastbootDataGrid.CurrentRow.Cells[4].Value = openFileDialog.FileName;
				FastbootDataGrid.CurrentRow.Cells[0].Value = true;
			}
		}
	}

	private void FBinfofir_CheckedChanged(object sender, EventArgs e)
	{
		if (!FBinfofir.Checked)
		{
			MTKConfig.Infirmware = false;
			Console.WriteLine(MTKConfig.Infirmware);
		}
		else
		{
			MTKConfig.Infirmware = true;
			Console.WriteLine(MTKConfig.Infirmware);
		}
	}

	private void TabChange(object sender, EventArgs e)
	{
		string text = "UnlockUniv";
		string text2 = "EDLModule";
		string text3 = "MTKModule";
		string text4 = "UnisocModule";
		string text5 = "HisiModule";
		string text6 = "METAModule";
		string text7 = "SamsungModule";
		string text8 = "AndroidModule";
		string text9 = "NVModule";
		if (!(sender is SimpleButton simpleButton) || JobStatus.Busy)
		{
			return;
		}
		Timer_Sidebar_Menu.Start();
		string name = simpleButton.Name;
		uint num = ImplementationDetails.method_01(name);
		int num2;
		if (num <= 2223729138u)
		{
			if (num <= 1517426072)
			{
				if (num == 1327498800)
				{
					if (name == text6)
					{
						num2 = 5;
						goto IL_0057;
					}
				}
				else if (num == 1517426072 && name == text4)
				{
					num2 = 3;
					goto IL_0057;
				}
			}
			else if (num == 1645578117)
			{
				if (name == text3)
				{
					num2 = 2;
					goto IL_0057;
				}
			}
			else if (num == 2223729138u && name == text5)
			{
				num2 = 4;
				goto IL_0057;
			}
		}
		else if (num > 2483307901u)
		{
			if (num != 2706473540u)
			{
				if (num == 2841345163u)
				{
					if (name == text7)
					{
						num2 = 6;
						goto IL_0057;
					}
				}
				else if (num == 3049274280u && name == text2)
				{
					num2 = 1;
					goto IL_0057;
				}
			}
			else if (name == text8)
			{
				num2 = 7;
				goto IL_0057;
			}
		}
		else if (num == 2302208381u)
		{
			if (name == text)
			{
				num2 = 0;
				goto IL_0057;
			}
		}
		else if (num == 2483307901u && name == text9)
		{
			num2 = 8;
			goto IL_0057;
		}
		num2 = TabMenu.SelectedTabPageIndex;
		goto IL_0057;
		IL_0057:
		int selectedTabPageIndex = num2;
		TabMenu.SelectedTabPageIndex = selectedTabPageIndex;
	}

	private void Timer_Sidebar_Menu_Tick(object sender, EventArgs e)
	{
		if (!sideBar_Expand)
		{
			SideBar.Width += 3000;
			if (SideBar.Width == SideBar.MaximumSize.Width)
			{
				Timer_Sidebar_Menu.Stop();
				sideBar_Expand = true;
			}
		}
		else
		{
			SideBar.Width -= 3000;
			if (SideBar.Width == SideBar.MinimumSize.Width)
			{
				sideBar_Expand = false;
				Timer_Sidebar_Menu.Stop();
			}
		}
	}

	private void simpleButton4_Click(object sender, EventArgs e)
	{
		if (!JobStatus.Busy)
		{
			Timer_Sidebar_Menu.Start();
		}
	}

	private void device_Click(object sender, EventArgs e)
	{
		string fileName = "devmgmt.msc";
		Process.Start(fileName);
	}

	private void CSPDPart_CheckedChanged(object sender, EventArgs e)
	{
		if (CSPDPart.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)SPDDataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)SPDDataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void CheckMeta_CheckedChanged(object sender, EventArgs e)
	{
		if (CheckMeta.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)METADataGrid.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)METADataGrid.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void CheckOdin_CheckedChanged(object sender, EventArgs e)
	{
		if (CheckOdin.CheckState == CheckState.Checked)
		{
			foreach (DataGridViewRow item in (IEnumerable)OdinGridView.Rows)
			{
				int i = 0;
				for (int num = item.Cells.Count - 1; i <= num; i++)
				{
					item.Cells[0].Value = true;
				}
			}
			return;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)OdinGridView.Rows)
		{
			int j = 0;
			for (int num2 = item2.Cells.Count - 1; j <= num2; j++)
			{
				item2.Cells[0].Value = false;
			}
		}
	}

	private void MTKDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		string text = "Read [";
		object obj = null;
		string text2 = ":";
		string text3 = "]";
		string text4 = "Write [";
		string text5 = "Format [";
		string text6 = "Read [Tot:";
		string text7 = " Part]";
		string text8 = "Erase [Tot:";
		string text9 = "Write [Tot:";
		BarButtonItem mTKRead = MTKRead;
		string[] obj2 = new string[5] { text, null, null, null, null };
		object value = MTKDataGrid.CurrentRow.Cells[1].Value;
		obj2[1] = (string)((value != null) ? value.ToString() : obj);
		obj2[2] = text2;
		object value2 = MTKDataGrid.CurrentRow.Cells[6].Value;
		obj2[3] = (string)((value2 != null) ? value2.ToString() : obj);
		obj2[4] = text3;
		mTKRead.Caption = string.Concat(obj2);
		BarButtonItem mTKWrite = MTKWrite;
		string[] obj3 = new string[5] { text4, null, null, null, null };
		object value3 = MTKDataGrid.CurrentRow.Cells[1].Value;
		obj3[1] = (string)((value3 != null) ? value3.ToString() : obj);
		obj3[2] = text2;
		object value4 = MTKDataGrid.CurrentRow.Cells[6].Value;
		obj3[3] = (string)((value4 != null) ? value4.ToString() : obj);
		obj3[4] = text3;
		mTKWrite.Caption = string.Concat(obj3);
		BarButtonItem mTKFormat = MTKFormat;
		string[] obj4 = new string[5] { text5, null, null, null, null };
		object value5 = MTKDataGrid.CurrentRow.Cells[1].Value;
		obj4[1] = (string)((value5 != null) ? value5.ToString() : obj);
		obj4[2] = text2;
		object value6 = MTKDataGrid.CurrentRow.Cells[6].Value;
		obj4[3] = (string)((value6 != null) ? value6.ToString() : obj);
		obj4[4] = text3;
		mTKFormat.Caption = string.Concat(obj4);
		UI.totalchecked = 0;
		foreach (DataGridViewRow item in (IEnumerable)MTKDataGrid.Rows)
		{
			if (Convert.ToBoolean(item.Cells[0].Value))
			{
				UI.totalchecked++;
			}
		}
		foreach (DataGridViewRow item2 in (IEnumerable)MTKDataGrid.Rows)
		{
			_ = item2;
			ReadPartition.Caption = text6 + UI.totalchecked + text7;
			FormatCheck.Caption = text8 + UI.totalchecked + text7;
			WriteCheck.Caption = text9 + UI.totalchecked + text7;
		}
	}

	protected override void Dispose(bool disposing)
	{
		object obj = null;
		if (disposing && components !=null)
		{
			components.Dispose();
			base.Dispose(disposing);
		}
		else
		{
			base.Dispose(disposing);
		}
	}

	private void InitializeComponent()
	{
		string name = "RLog";
		string text = "";
		string name2 = "PLog";
		string name3 = "SideBar";
		string name4 = "simpleButton1.ImageOptions.Image";
		string name5 = "simpleButton1";
		string text2 = " Settings";
		string name6 = "UnlockUniv.ImageOptions.Image";
		string name7 = "UnlockUniv";
		string text3 = "  Unlock";
		string name8 = "UnisocModule.ImageOptions.Image";
		string name9 = "UnisocModule";
		string text4 = "Spreadtrum";
		string name10 = "SamsungModule.ImageOptions.Image";
		string name11 = "SamsungModule";
		string text5 = "  Samsung";
		string name12 = "NVModule.ImageOptions.Image";
		string name13 = "NVModule";
		string text6 = "  IMEI Editor";
		string name14 = "AndroidModule.ImageOptions.Image";
		string name15 = "AndroidModule";
		string text7 = "  Fastboot ADB";
		string name16 = "HisiModule.ImageOptions.Image";
		string name17 = "HisiModule";
		string text8 = "  Hisilicon";
		string name18 = "METAModule.ImageOptions.Image";
		string name19 = "METAModule";
		string text9 = "  Meta Transsion";
		string name20 = "EDLModule.ImageOptions.Image";
		string name21 = "EDLModule";
		string text10 = " Qualcomm";
		string name22 = "MTKModule.ImageOptions.Image";
		string name23 = "MTKModule";
		string text11 = " Mediatek";
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string text12 = "STOP";
		string name24 = "PBar";
		float field_2 = Class38.field_1267;
		string name25 = "dataGridViewCheckBoxColumn3";
		float field_3 = Class38.field_1269;
		string headerText = "LUN";
		string name26 = "dataGridViewTextBoxColumn13";
		float field_4 = Class38.field_1272;
		string headerText2 = "Sector Size";
		string name27 = "Column6";
		float field_5 = Class38.field_1275;
		string headerText3 = "Partition";
		string name28 = "dataGridViewTextBoxColumn14";
		string headerText4 = "Filename";
		string name29 = "dataGridViewTextBoxColumn15";
		string headerText5 = "Filesize";
		string name30 = "dataGridViewTextBoxColumn16";
		float field_6 = Class38.field_1282;
		string headerText6 = "Begin Address";
		string name31 = "dataGridViewTextBoxColumn17";
		float field_7 = Class38.field_1285;
		string headerText7 = "End Address";
		string name32 = "dataGridViewTextBoxColumn18";
		float field_8 = Class38.field_1288;
		string headerText8 = "Location";
		string name33 = "dataGridViewTextBoxColumn19";
		float field_9 = Class38.field_1290;
		string name34 = "dataGridViewCheckBoxColumn2";
		float field_10 = Class38.field_1292;
		string name35 = "dataGridViewTextBoxColumn1";
		string name36 = "dataGridViewTextBoxColumn8";
		string name37 = "dataGridViewTextBoxColumn9";
		string name38 = "dataGridViewTextBoxColumn10";
		string name39 = "dataGridViewTextBoxColumn11";
		string name40 = "dataGridViewTextBoxColumn12";
		string name41 = "dataGridViewCheckBoxColumn1";
		string name42 = "dataGridViewTextBoxColumn2";
		string name43 = "dataGridViewTextBoxColumn3";
		string name44 = "dataGridViewTextBoxColumn5";
		string name45 = "dataGridViewTextBoxColumn6";
		string name46 = "dataGridViewTextBoxColumn7";
		string name47 = "dataGridViewTextBoxColumn39";
		float field_11 = Class38.field_1306;
		string name48 = "dataGridViewCheckBoxColumn6";
		float field_12 = Class38.field_1308;
		string headerText9 = "Type";
		string name49 = "dataGridViewTextBoxColumn4";
		float field_13 = Class38.field_1310;
		string name50 = "dataGridViewTextBoxColumn22";
		string name51 = "dataGridViewTextBoxColumn23";
		string name52 = "dataGridViewTextBoxColumn24";
		float field_14 = Class38.field_1314;
		string name53 = "dataGridViewTextBoxColumn25";
		float field_15 = Class38.field_1316;
		string name54 = "dataGridViewTextBoxColumn26";
		string name55 = "dataGridViewTextBoxColumn27";
		float field_16 = Class38.field_1319;
		string name56 = "dataGridViewCheckBoxColumn4";
		float field_17 = Class38.field_1321;
		string headerText10 = "Package Name";
		string name57 = "dataGridViewTextBoxColumn20";
		float field_18 = Class38.field_1324;
		string name58 = "dataGridViewTextBoxColumn21";
		string name59 = "dataGridViewCheckBoxColumn5";
		string name60 = "Column1";
		string headerText11 = "File Name";
		string name61 = "Column5";
		string headerText12 = "File Size";
		string name62 = "Column2";
		string name63 = "Column3";
		string headerText13 = "Command";
		string name64 = "Column4";
		string skinName = "WXI";
		string name65 = "TabMenu";
		string name66 = "TabEDL";
		string text13 = "Qualcomm";
		string name67 = "TabSubEDL";
		string name68 = "TabEDLFlashing";
		string text14 = "Flashing";
		string name69 = "SetSlotB";
		string text15 = "Set Boot Slot B";
		string name70 = "SetSlotA";
		string text16 = "Set Boot Slot A";
		string name71 = "QCOppoID.ImageOptions.Image";
		string name72 = "QCOppoID";
		string text17 = "Oppo ID";
		string name73 = "ShowALLqc";
		string caption = "Show ALL";
		string name74 = "EDLinfofirmware";
		string caption2 = "Firmware Info";
		string name75 = "CEDLPart";
		string name76 = "EDLDataGrid";
		string familyName2 = "Microsoft Sans Serif";
		string headerText14 = "Index";
		string name77 = "dataGridViewCheckBoxColumn14";
		string familyName3 = "Consolas";
		float field_19 = Class38.field_375;
		string name78 = "dataGridViewTextBoxColumn41";
		string headerText15 = "Sector";
		string name79 = "Column9";
		string name80 = "dataGridViewTextBoxColumn42";
		string name81 = "dataGridViewTextBoxColumn70";
		string name82 = "dataGridViewTextBoxColumn71";
		string name83 = "dataGridViewTextBoxColumn72";
		string name84 = "Column8";
		string name85 = "dataGridViewTextBoxColumn73";
		string name86 = "EDLFlash";
		string text18 = "Flash";
		string name87 = "EDLReboot";
		string text19 = "Reboot";
		string name88 = "EDLReadGPT";
		string text20 = "Boot Info";
		string name89 = "EDLReadBack.ImageOptions.Image";
		string name90 = "EDLReadBack";
		string text21 = "Read Back";
		string name91 = "EDLResetFrp";
		string text22 = "Reset Frp";
		string name92 = "EDLResetLock";
		string text23 = "Factory Reset";
		string name93 = "EDLMIReset";
		string text24 = "Reset MiCloud";
		string name94 = "EDLMIDisable.ImageOptions.Image";
		string name95 = "EDLMIDisable";
		string text25 = "Disable MiCloud";
		string name96 = "EDLEFSRestore";
		string text26 = "EFS Restore";
		string name97 = "EDLEFSBackup";
		string text27 = "EFS Backup";
		string name98 = "EDLEFSErase";
		string text28 = "EFS Erase";
		string name99 = "EDLLoadLoader";
		string text29 = "Load";
		string name100 = "EDLClearLoader";
		string text30 = "Clear";
		string familyName4 = "Arial";
		string name101 = "LEDLLoader";
		string text31 = "Firehose";
		string name102 = "EDLLoader";
		string text32 = "EMMC";
		string name103 = "EDLStorageMode";
		string text33 = "UFS";
		string name104 = "EDLLoadFirmware";
		string name105 = "EDLFirmware";
		string name106 = "LEDLFirmware";
		string text34 = "Firmware";
		string name107 = "EDLLoadPatch";
		string name108 = "EDLClearPatch";
		string name109 = "LEDLPatch";
		string text35 = "Patch";
		string name110 = "EDLPatch";
		string name111 = "CEDLServer";
		string caption3 = "Server";
		string name112 = "EDLModel";
		string name113 = "EDLBrand";
		string name114 = "LEDLBrand";
		string text36 = "Brand";
		string name115 = "LEDLModel";
		string text37 = "Model";
		string name116 = "TabEDLNetwork";
		string text38 = "Network Repair";
		string name117 = "groupControl2";
		string text39 = "Diag Mode";
		string name118 = "QCN_Editor";
		string text40 = "QCN Editor";
		string name119 = "DiagEnableXiaomi";
		string text41 = "Diag Enable [ Xiaomi ]";
		string name120 = "DiagEnable";
		string text42 = "Diag Enable ";
		string name121 = "DiagEnableRoot";
		string text43 = "Diag Enable [ Root ]";
		string name122 = "DiagCriticalWrite";
		string text44 = "Critical Write";
		string name123 = "DiagCriticalRead";
		string text45 = "Critical Read";
		string name124 = "DiagQCNWrite";
		string text46 = "QCN Write";
		string name125 = "DiagQCNRead";
		string text47 = "QCN Read";
		string name126 = "DiagWriteIMEI";
		string text48 = "Write IMEI";
		string text49 = "Unlock";
		string editValue = "Select Brand";
		string name127 = "BrandUEDL";
		string text50 = "Asus";
		string text51 = "Huawei";
		string text52 = "Lenovo";
		string text53 = "Meizu";
		string text54 = "Oneplus";
		string text55 = "Oppo";
		string text56 = "Realme";
		string text57 = "Samsung";
		string text58 = "Vivo";
		string text59 = "Xiaomi";
		string text60 = "Nokia";
		string text61 = "OPLUS VIP";
		string name128 = "PBUEDL";
		string name129 = "SearchMEDL";
		string name130 = "LMEDL";
		string name131 = "panel4";
		string name132 = "TabMTK";
		string text62 = "Mediatek";
		string name133 = "TabSubMTK";
		string name134 = "TabMTKFlashUnlock";
		string text63 = "Flash Unlock";
		string name135 = "InfoFW";
		string name136 = "simpleButton16.ImageOptions.Image";
		string name137 = "simpleButton16";
		string text64 = "Auth Bypass";
		string name138 = "RebuildUserDATA.ImageOptions.Image";
		string name139 = "RebuildUserDATA";
		string text65 = "Rebuild Userdata";
		string name140 = "MTKWiteemi.ImageOptions.Image";
		string name141 = "MTKWiteemi";
		string text66 = "Write EMI";
		string name142 = "MTKReadDUMP.ImageOptions.Image";
		string name143 = "MTKReadDUMP";
		string text67 = "Read Back DUMP";
		string name144 = "MTKReadFlash.ImageOptions.Image";
		string name145 = "MTKReadFlash";
		string text68 = "Read Back ROM";
		string name146 = "ShowALLMTK";
		string name147 = "CarbonaraExploit";
		string caption4 = "Carbonara Exploit ";
		string name148 = "DaCustum";
		string name149 = "MTKLoadDA";
		string name150 = "MTKLoadEMI";
		string name151 = "MTKClearDA";
		string name152 = "MTKClearEMI";
		string name153 = "CMTKPart";
		string name154 = "MTKResetFrpNew.ImageOptions.Image";
		string name155 = "MTKResetFrpNew";
		string text69 = "Set Boot Slot";
		string name156 = "MTKRemoveDemo.ImageOptions.Image";
		string name157 = "MTKRemoveDemo";
		string text70 = "Erase Demo";
		string name158 = "MTKSafeFormat.ImageOptions.Image";
		string name159 = "MTKSafeFormat";
		string text71 = "Safe Format";
		string name160 = "CMTKServer";
		string name161 = "MTKModel";
		string name162 = "MTKBrand";
		string name163 = "LMTKBrand";
		string name164 = "LMTKModel";
		string name165 = "MTKReadback.ImageOptions.Image";
		string name166 = "MTKReadback";
		string text72 = "Read Back FULL";
		string name167 = "MTKPatchVB.ImageOptions.Image";
		string name168 = "MTKPatchVB";
		string text73 = "Patch VB-Meta";
		string name169 = "MTKOrangestate.ImageOptions.Image";
		string name170 = "MTKOrangestate";
		string text74 = "Fix Orge State";
		string name171 = "MTKDMVerity.ImageOptions.Image";
		string name172 = "MTKDMVerity";
		string text75 = "Patch DM-Verify";
		string name173 = "MTKLBL.ImageOptions.Image";
		string name174 = "MTKLBL";
		string text76 = "BL Lock";
		string name175 = "MTKUBL.ImageOptions.Image";
		string name176 = "MTKUBL";
		string text77 = "BL Unlock";
		string name177 = "MTKResetFrp";
		string name178 = "MTKResetLock";
		string name179 = "MTKSecPlugin.ImageOptions.Image";
		string name180 = "MTKSecPlugin";
		string text78 = "Security Plugin";
		string name181 = "MTKRPMBRestore";
		string text79 = "RPMB Restore";
		string name182 = "MTKRPMBBackup";
		string text80 = "RPMB Backup";
		string name183 = "MTKRPMBErase";
		string text81 = "RPMB Erase";
		string name184 = "MTKNVRestore";
		string text82 = "NV Restore";
		string name185 = "MTKNVBackup";
		string text83 = "NV Backup";
		string name186 = "MTKNVErase";
		string text84 = "NV Erase";
		string name187 = "CMTKDA";
		string caption5 = "Custum DA";
		string name188 = "CMTKEMI";
		string caption6 = "Server EMI";
		string name189 = "LMTKDA";
		string text85 = "DA Flash | FILE";
		string name190 = "MTKDA";
		string name191 = "LMTKEMI";
		string text86 = "Preloader | EMI";
		string name192 = "MTKEMI";
		string name193 = "LMTKFirmware";
		string name194 = "MTKRepartition";
		string caption7 = "Re-Partition";
		string name195 = "Highspeed";
		string caption8 = "Hish-Speed";
		string name196 = "MTKFlash";
		string name197 = "MTKReboot.ImageOptions.Image";
		string name198 = "MTKReboot";
		string name199 = "MTKReadGPT";
		string name200 = "panel5";
		string text87 = "eMMC";
		string name201 = "LayoutStore";
		string caption9 = "Write";
		string name202 = "MTKWrite";
		string caption10 = "Read";
		string name203 = "MTKRead";
		string caption11 = "Erase";
		string name204 = "MTKFormat";
		string caption12 = "Write Check Tot";
		string name205 = "WriteCheck";
		string caption13 = "Erase Check Tot";
		string name206 = "FormatCheck";
		string caption14 = "Read Check Tot";
		string name207 = "ReadPartition";
		string name208 = "EDLFormat";
		string name209 = "EDLWrite";
		string name210 = "EDLRead";
		string name211 = "FBWrite";
		string name212 = "FBFormat";
		string name213 = "SPDWrite";
		string name214 = "SPDFormat";
		string name215 = "SPDRead";
		string name216 = "simpleButton17.ImageOptions.Image";
		string name217 = "simpleButton17";
		string text88 = " Rorce Brom";
		string name218 = "simpleButton14.ImageOptions.Image";
		string name219 = "simpleButton14";
		string text89 = "Rorce Brom";
		string name220 = "simpleButton15.ImageOptions.Image";
		string name221 = "simpleButton15";
		string text90 = "Fix DL Error";
		string name222 = "LabelControl10";
		string text91 = "Layout";
		string text92 = "Scatter";
		string name223 = "SuperMap";
		string text93 = "Dump";
		string name224 = "labelControl6";
		string text94 = "Super Map";
		string text95 = "Download Only";
		string name225 = "MTKFlashMode";
		string text96 = "Format Download";
		string text97 = "Firmware Upgrade";
		string name226 = "MTKLoadFirmware";
		string name227 = "MTKFirmware";
		string name228 = "MTKDataGrid";
		string name229 = "dataGridViewCheckBoxColumn8";
		string name230 = "dataGridViewTextBoxColumn36";
		string headerText16 = "Image";
		string name231 = "dataGridViewTextBoxColumn37";
		string name232 = "Column7";
		string name233 = "dataGridViewTextBoxColumn38";
		string name234 = "dataGridViewTextBoxColumn40";
		string name235 = "dataGridViewTextBoxColumn69";
		string name236 = "MTKResetMICloud.ImageOptions.Image";
		string name237 = "MTKResetMICloud";
		string text98 = "Reset Mi";
		string name238 = "MTKDisableMiCloud.ImageOptions.Image";
		string name239 = "MTKDisableMiCloud";
		string text99 = "Disable Mi";
		string name240 = "MTKHuaweiID.ImageOptions.Image";
		string name241 = "MTKHuaweiID";
		string text100 = "Huawei ID";
		string name242 = "MTKIMEIRepair.ImageOptions.Image";
		string name243 = "MTKIMEIRepair";
		string text101 = "Repair IMEI";
		string name244 = "MTKPayjoy.ImageOptions.Image";
		string name245 = "MTKPayjoy";
		string text102 = "Payjoy";
		string name246 = "MTKOPPOID.ImageOptions.Image";
		string name247 = "MTKOPPOID";
		string name248 = "TabSpd";
		string name249 = "TabSubSpreadtrum";
		string name250 = "TabSpdFlash";
		string name251 = "CSPDPart";
		string name252 = "SPDDataGrid";
		string name253 = "dataGridViewCheckBoxColumn12";
		string name254 = "dataGridViewTextBoxColumn35";
		string name255 = "dataGridViewTextBoxColumn62";
		string name256 = "dataGridViewTextBoxColumn63";
		string name257 = "dataGridViewTextBoxColumn64";
		string name258 = "dataGridViewTextBoxColumn65";
		string name259 = "dataGridViewTextBoxColumn66";
		string name260 = "checkEdit2";
		string caption15 = "Info Firmware";
		string name261 = "Skippacth";
		string caption16 = "Skip Pacth";
		string name262 = "labelControl3";
		string name263 = "CModelSPD";
		string text103 = "SC7731E";
		string text104 = "SC9832E";
		string text105 = "SC9863A";
		string text106 = "SC9863A_64";
		string text107 = "Tiger T310_64";
		string text108 = "Tiger T606_64";
		string text109 = "Tiger T610_64";
		string text110 = "Tiger T612_64";
		string text111 = "Tiger T616_64";
		string text112 = "Tiger T700_64";
		string name264 = "SPDResetFrp";
		string name265 = "SPDResetLock";
		string text113 = "Reset Lock";
		string name266 = "LSPDCPU";
		string editValue2 = "Select Type";
		string name267 = "CBrandSPD";
		string text114 = "Sprd Generic";
		string text115 = "Infinix";
		string text116 = "Tecno";
		string text117 = "ZTE";
		string text118 = "Itel";
		string text119 = "Wiko";
		string text120 = "Hisense";
		string name268 = "SPDReboot";
		string name269 = "SPDReadGPT";
		string name270 = "SPDATdiag";
		string text121 = "AT Diag";
		string name271 = "groupControl3";
		string text122 = "ATDiag Mode";
		string name272 = "FactoryReset2";
		string text123 = "Factory Reset [2]";
		string name273 = "FactoryReset";
		string text124 = "Factory Reset [1]";
		string name274 = "TabHisi";
		string text125 = "Hisilicon";
		string name275 = "HISIBLUnlock";
		string name276 = "HISIResetFrp";
		string name277 = "HISIResetlock";
		string name278 = "HISIResetID";
		string editValue3 = "Select CPU Type";
		string name279 = "CCPUHisi";
		string text126 = "KIRIN 65x_1";
		string text127 = "KIRIN 65x_2";
		string text128 = "KIRIN 620_1";
		string text129 = "KIRIN 620_2";
		string text130 = "KIRIN 710_emmc";
		string text131 = "KIRIN 710_ufs";
		string text132 = "KIRIN 925";
		string text133 = "KIRIN 935";
		string text134 = "KIRIN 950";
		string text135 = "KIRIN 960";
		string text136 = "KIRIN 970";
		string text137 = "KIRIN 980";
		string name280 = "LHisiCPU";
		string text138 = "CPU Type";
		string name281 = "TabMeta";
		string text139 = "META";
		string name282 = "xtraTabControl2";
		string name283 = "Transsion";
		string text140 = "Meta Transsion";
		string name284 = "METAWriteIMEI";
		string text141 = "IMEI Repair";
		string name285 = "METASecplugin";
		string name286 = "METAReboot";
		string name287 = "CheckMeta";
		string name288 = "METAReset";
		string name289 = "METAEnableADB";
		string text142 = "Enable ADB";
		string name290 = "METADisableADB";
		string text143 = "Disable ADB";
		string name291 = "METAPayjoy";
		string name292 = "METAFrp";
		string name293 = "METAReadGPT";
		string name294 = "METADataGrid";
		string name295 = "dataGridViewCheckBoxColumn10";
		string name296 = "dataGridViewTextBoxColumn49";
		string name297 = "dataGridViewTextBoxColumn50";
		string name298 = "dataGridViewTextBoxColumn51";
		string name299 = "dataGridViewTextBoxColumn52";
		string name300 = "dataGridViewTextBoxColumn53";
		string name301 = "dataGridViewTextBoxColumn54";
		string name302 = "Andproid";
		string text144 = "Meta Android";
		string name303 = "groupControl4";
		string text145 = "META Mode";
		string name304 = "simpleButton2";
		string text146 = "Reset FRP";
		string name305 = "SafeFormat";
		string name306 = "metaFactoryReset";
		string name307 = "TabSamsung";
		string name308 = "TabSubSamsung";
		string name309 = "TabSamMTP";
		string text147 = "MTP | ADB";
		string name310 = "GADBMode";
		string text148 = "ADB Mode";
		string name311 = "FRPRemove16";
		string text149 = "FRP Remove 16";
		string name312 = "ADBSamFrp";
		string text150 = "FRP Remove";
		string name313 = "ADBSamId";
		string text151 = "Remove Samsung Account";
		string name314 = "GMTPMode";
		string text152 = "MTP Mode";
		string name315 = "MTPRebootOdin";
		string text153 = "Reboot To Download Mode";
		string name316 = "MTPSN";
		string text154 = "Change SN";
		string name317 = "MTPReset";
		string name318 = "MTPFrp16";
		string text155 = "Remove FRP OS 16";
		string name319 = "MTPFrp";
		string text156 = "Remove FRP 2024";
		string name320 = "MTPInfo";
		string text157 = "Read Info";
		string name321 = "TabSamsOdin";
		string text158 = "Odin Mode";
		string name322 = "groupControl1";
		string text159 = "Download Mode | Odin Mode";
		string name323 = "Reset2024";
		string text160 = "Reset Frp MTK 2024 V1";
		string name324 = "OdinInfo";
		string name325 = "OdinReboot";
		string name326 = "OdinSoftbrick";
		string text161 = "Fix Softbrick";
		string name327 = "OdinFrp";
		string text162 = "Reset Frp MTK 2024 V2";
		string name328 = "OdinReset";
		string name329 = "OdinWritePit";
		string text163 = "PIT Write";
		string name330 = "OdinReadPit";
		string text164 = "PIT Read";
		string name331 = "xtraTabPage2";
		string text165 = "Odin Flash";
		string name332 = "CheckOdin";
		string name333 = "OdinGridView";
		string name334 = "dataGridViewCheckBoxColumn9";
		string name335 = "dataGridViewTextBoxColumn43";
		string name336 = "dataGridViewTextBoxColumn44";
		string name337 = "dataGridViewTextBoxColumn45";
		string name338 = "dataGridViewTextBoxColumn46";
		string name339 = "dataGridViewTextBoxColumn47";
		string name340 = "dataGridViewTextBoxColumn48";
		string name341 = "Column10";
		string name342 = "label6";
		string text166 = "HOME File";
		string name343 = "label5";
		string text167 = "CSC File";
		string name344 = "label4";
		string text168 = "CP File";
		string name345 = "label3";
		string text169 = "AP File";
		string name346 = "label2";
		string text170 = "BL File";
		string name347 = "ClearOdinFirmware";
		string name348 = "COdinEraseEFS";
		string caption17 = "Erase EFS";
		string name349 = "COdinRepartition";
		string caption18 = "Repartition";
		string name350 = "OdinFlash";
		string text171 = "Flash Firmware";
		string name351 = "LoadHome";
		string name352 = "LoadCSC";
		string name353 = "LoadCP";
		string name354 = "LoadAP";
		string name355 = "LoadBL";
		string name356 = "txtHOME";
		string nullText = "Home Firmware";
		string text172 = "CSC Firmware";
		string name357 = "txtCSC";
		string text173 = "CP Firmware";
		string name358 = "txtCP";
		string name359 = "txtAP";
		string nullText2 = "AP Firmware";
		string name360 = "txtBL";
		string nullText3 = "BL Firmware";
		string name361 = "TabAdbFastboot";
		string text174 = "Android";
		string name362 = "xtraTabControl1";
		string name363 = "xtraTabPage4";
		string text175 = "Fastboot Mode";
		string name364 = "FBinfofir";
		string name365 = "CFastbootPart";
		string name366 = "FastbootDataGrid";
		string name367 = "dataGridViewCheckBoxColumn7";
		string name368 = "dataGridViewTextBoxColumn28";
		string name369 = "dataGridViewTextBoxColumn29";
		string name370 = "dataGridViewTextBoxColumn30";
		string name371 = "dataGridViewTextBoxColumn31";
		string name372 = "dataGridViewTextBoxColumn33";
		string name373 = "FBUnlockBL";
		string text176 = "Unlock BL";
		string name374 = "FBRebootRecovery";
		string text177 = "Reboot Recovery";
		string name375 = "FBRebootEDL";
		string text178 = "Reboot EDL";
		string name376 = "FBReadGPT";
		string name377 = "FBSlotA";
		string name378 = "FBEFSErase";
		string name379 = "FBReboot";
		string name380 = "FBResetFrp";
		string name381 = "FastbootClearFirmware";
		string name382 = "labelControl1";
		string name383 = "FBFlash";
		string name384 = "FastbootLoadFirmware";
		string name385 = "FastbootFirmware";
		string name386 = "xtraTabPage3";
		string name387 = "CAndroidPart";
		string name388 = "AndroidDataGrid";
		string name389 = "dataGridViewCheckBoxColumn13";
		string name390 = "dataGridViewTextBoxColumn32";
		string name391 = "dataGridViewTextBoxColumn34";
		string name392 = "txtSearch";
		string name393 = "txtSearch.Properties.ContextImageOptions.Image";
		string toolTip = "Types For Search Package";
		string name394 = "SamsungAccount";
		string text179 = " Disable Samsung Account";
		string name395 = "DisableKnox";
		string text180 = "Samsung Disable Knox";
		string name396 = "ADBDisableHU";
		string text181 = "Disable OS Hu";
		string name397 = "RemoveDemo";
		string text182 = "Remove Demo";
		string name398 = "OppoSwith";
		string text183 = "Oppo Swith Carrier Mode";
		string name399 = "RemoveMDM";
		string text184 = "Remove MDM (O-Guard)";
		string name400 = "Disable_OSOnePlus";
		string text185 = "Disable OS One";
		string name401 = "DisableOSTecno";
		string text186 = "Disable OS Tecno";
		string name402 = "DisableOSItel";
		string text187 = "Disable OS Itel";
		string name403 = "DisableDemo";
		string text188 = "Disable Demo";
		string name404 = "DisabaleOSRealme";
		string text189 = "Disable OS Real";
		string name405 = "ResetHuaweiID";
		string text190 = "Reset Huawei ID";
		string name406 = "RemovePlugin";
		string text191 = "Remove Security Plugin";
		string name407 = "rebootdevice";
		string name408 = "FixAssistantWIFI";
		string text192 = "Fix Wireless Test Assistant";
		string name409 = "Remove_Payjoy";
		string text193 = "Remove Payjoy";
		string name410 = "Reset_lock";
		string name411 = "EnableDiagRoot";
		string text194 = "Enable Diag Root";
		string name412 = "EnableDiagOperation";
		string text195 = "Enable Diag";
		string name413 = "DisablePengecualian";
		string text196 = "Disable Pengecualian";
		string name414 = "Disable_OS_Sam";
		string text197 = "Disable OS Sam";
		string name415 = "Disable_OSMI";
		string text198 = "Disable OS MI";
		string name416 = "Disaba_OSOppo";
		string text199 = "Disable OS Oppo";
		string name417 = "Disable_osvivo";
		string text200 = "Disable OS VIvo";
		string name418 = "EnablediagXiaomi";
		string text201 = "Enable diag MI";
		string name419 = "FixAssistant";
		string text202 = "Test Assistant";
		string name420 = "DidableMicloud";
		string text203 = "Didable Micloud";
		string name421 = "SamsungID";
		string text204 = "Samsung ID";
		string name422 = "EraseFRP16";
		string text205 = "Erase FRP 16";
		string name423 = "EraseFRP";
		string text206 = "Erase FRP";
		string name424 = "ADBEnableApk";
		string text207 = "Enable APK";
		string name425 = "ADBInstallApk";
		string text208 = "Install APK";
		string name426 = "ADBUninstallApk";
		string text209 = "Uninstall APK";
		string name427 = "ADBDisableApk";
		string text210 = "Disable APK";
		string name428 = "ADBLoadApk";
		string text211 = "Boot APK";
		string name429 = "xtraTabPage1";
		string text212 = "Mi Assistant";
		string name430 = "SDresetLock";
		string name431 = "SDdisabaleMI";
		string text213 = "DIsable Micloud";
		string name432 = "SDresetMI";
		string text214 = "Reset Micloud";
		string name433 = "ResetFRPMIasis";
		string name434 = "TabNVEditor";
		string text215 = "IMEI Editor";
		string name435 = "NVRAMEdit";
		string text216 = "NVRAM IMEI Editor";
		string name436 = "NVDataEdit";
		string text217 = "NVDATA IMEI Editor";
		string name437 = "dataGridViewCheckBoxColumn11";
		string name438 = "dataGridViewTextBoxColumn55";
		string name439 = "dataGridViewTextBoxColumn56";
		string name440 = "dataGridViewTextBoxColumn57";
		string name441 = "dataGridViewTextBoxColumn58";
		string name442 = "dataGridViewTextBoxColumn59";
		string name443 = "dataGridViewTextBoxColumn60";
		string name444 = "dataGridViewTextBoxColumn61";
		string name445 = "LSpeed";
		string text218 = "File Part Size : 0MiB/0MB |  USB Speed : 0MiB/s";
		string editValue4 = "Waiting for Device Automatic";
		string name446 = "ComboPort";
		string name447 = "ComboPort.Properties.ContextImageOptions.Image";
		string name448 = "panel1";
		string name449 = "expred";
		string text219 = "Expired :";
		string name450 = "Expired";
		string text220 = "...";
		string name451 = "labelControl5";
		string text221 = "Email :";
		string name452 = "PanelEMAIL";
		string name453 = "panelControl2";
		string name454 = "panel3";
		string name455 = "device.Image";
		string name456 = "device";
		string name457 = "simpleButton4.ImageOptions.Image";
		string name458 = "simpleButton4";
		string name459 = "label1";
		string text222 = "USB :";
		string name460 = "panel2";
		string text223 = "Custom 2";
		string name461 = "PopMTK";
		string name462 = "PopQc";
		string name463 = "PopFB";
		string name464 = "PopSPD";
		float field_20 = Class38.field_392;
		float field_21 = Class38.field_393;
		string name465 = "Main";
		string text224 = "AMFTool";
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMFTool.Main));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle55 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle56 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle57 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle58 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle59 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle60 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle61 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle62 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle63 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle64 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle65 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle66 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle67 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle68 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle69 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle70 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle71 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle72 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle73 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle74 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle75 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle76 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle77 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle78 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle79 = new System.Windows.Forms.DataGridViewCellStyle();
		this.RLog = new System.Windows.Forms.RichTextBox();
		this.PLog = new DevExpress.XtraEditors.PanelControl();
		this.SideBar = new DevExpress.XtraEditors.PanelControl();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.UnlockUniv = new DevExpress.XtraEditors.SimpleButton();
		this.UnisocModule = new DevExpress.XtraEditors.SimpleButton();
		this.SamsungModule = new DevExpress.XtraEditors.SimpleButton();
		this.NVModule = new DevExpress.XtraEditors.SimpleButton();
		this.AndroidModule = new DevExpress.XtraEditors.SimpleButton();
		this.HisiModule = new DevExpress.XtraEditors.SimpleButton();
		this.METAModule = new DevExpress.XtraEditors.SimpleButton();
		this.EDLModule = new DevExpress.XtraEditors.SimpleButton();
		this.MTKModule = new DevExpress.XtraEditors.SimpleButton();
		this.STOP = new DevExpress.XtraEditors.SimpleButton();
		this.PBar = new DevExpress.XtraEditors.ProgressBarControl();
		this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn39 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewCheckBoxColumn6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewCheckBoxColumn5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TabMenu = new DevExpress.XtraTab.XtraTabControl();
		this.TabEDL = new DevExpress.XtraTab.XtraTabPage();
		this.TabSubEDL = new DevExpress.XtraTab.XtraTabControl();
		this.TabEDLFlashing = new DevExpress.XtraTab.XtraTabPage();
		this.SetSlotB = new DevExpress.XtraEditors.SimpleButton();
		this.SetSlotA = new DevExpress.XtraEditors.SimpleButton();
		this.QCOppoID = new DevExpress.XtraEditors.SimpleButton();
		this.ShowALLqc = new DevExpress.XtraEditors.CheckEdit();
		this.EDLinfofirmware = new DevExpress.XtraEditors.CheckEdit();
		this.CEDLPart = new System.Windows.Forms.CheckBox();
		this.EDLDataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn14 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn70 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn71 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn72 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn73 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.EDLFlash = new DevExpress.XtraEditors.SimpleButton();
		this.EDLReboot = new DevExpress.XtraEditors.SimpleButton();
		this.EDLReadGPT = new DevExpress.XtraEditors.SimpleButton();
		this.EDLReadBack = new DevExpress.XtraEditors.SimpleButton();
		this.EDLResetFrp = new DevExpress.XtraEditors.SimpleButton();
		this.EDLResetLock = new DevExpress.XtraEditors.SimpleButton();
		this.EDLMIReset = new DevExpress.XtraEditors.SimpleButton();
		this.EDLMIDisable = new DevExpress.XtraEditors.SimpleButton();
		this.EDLEFSRestore = new DevExpress.XtraEditors.SimpleButton();
		this.EDLEFSBackup = new DevExpress.XtraEditors.SimpleButton();
		this.EDLEFSErase = new DevExpress.XtraEditors.SimpleButton();
		this.EDLLoadLoader = new DevExpress.XtraEditors.SimpleButton();
		this.EDLClearLoader = new DevExpress.XtraEditors.SimpleButton();
		this.LEDLLoader = new DevExpress.XtraEditors.LabelControl();
		this.EDLLoader = new DevExpress.XtraEditors.TextEdit();
		this.EDLStorageMode = new DevExpress.XtraEditors.ComboBoxEdit();
		this.EDLLoadFirmware = new DevExpress.XtraEditors.SimpleButton();
		this.EDLFirmware = new DevExpress.XtraEditors.TextEdit();
		this.LEDLFirmware = new DevExpress.XtraEditors.LabelControl();
		this.EDLLoadPatch = new DevExpress.XtraEditors.SimpleButton();
		this.EDLClearPatch = new DevExpress.XtraEditors.SimpleButton();
		this.LEDLPatch = new DevExpress.XtraEditors.LabelControl();
		this.EDLPatch = new DevExpress.XtraEditors.TextEdit();
		this.CEDLServer = new DevExpress.XtraEditors.CheckEdit();
		this.EDLModel = new DevExpress.XtraEditors.ComboBoxEdit();
		this.EDLBrand = new DevExpress.XtraEditors.ComboBoxEdit();
		this.LEDLBrand = new DevExpress.XtraEditors.LabelControl();
		this.LEDLModel = new DevExpress.XtraEditors.LabelControl();
		this.TabEDLNetwork = new DevExpress.XtraTab.XtraTabPage();
		this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
		this.QCN_Editor = new DevExpress.XtraEditors.SimpleButton();
		this.DiagEnableXiaomi = new DevExpress.XtraEditors.SimpleButton();
		this.DiagEnable = new DevExpress.XtraEditors.SimpleButton();
		this.DiagEnableRoot = new DevExpress.XtraEditors.SimpleButton();
		this.DiagCriticalWrite = new DevExpress.XtraEditors.SimpleButton();
		this.DiagCriticalRead = new DevExpress.XtraEditors.SimpleButton();
		this.DiagQCNWrite = new DevExpress.XtraEditors.SimpleButton();
		this.DiagQCNRead = new DevExpress.XtraEditors.SimpleButton();
		this.DiagWriteIMEI = new DevExpress.XtraEditors.SimpleButton();
		this.Unlock = new DevExpress.XtraTab.XtraTabPage();
		this.BrandUEDL = new DevExpress.XtraEditors.ComboBoxEdit();
		this.PBUEDL = new DevExpress.XtraEditors.PanelControl();
		this.SearchMEDL = new DevExpress.XtraEditors.SearchControl();
		this.LMEDL = new DevExpress.XtraEditors.ListBoxControl();
		this.panel4 = new System.Windows.Forms.Panel();
		this.TabMTK = new DevExpress.XtraTab.XtraTabPage();
		this.TabSubMTK = new DevExpress.XtraTab.XtraTabControl();
		this.TabMTKFlashUnlock = new DevExpress.XtraTab.XtraTabPage();
		this.InfoFW = new DevExpress.XtraEditors.CheckEdit();
		this.simpleButton16 = new DevExpress.XtraEditors.SimpleButton();
		this.RebuildUserDATA = new DevExpress.XtraEditors.SimpleButton();
		this.MTKWiteemi = new DevExpress.XtraEditors.SimpleButton();
		this.MTKReadDUMP = new DevExpress.XtraEditors.SimpleButton();
		this.MTKReadFlash = new DevExpress.XtraEditors.SimpleButton();
		this.ShowALLMTK = new DevExpress.XtraEditors.CheckEdit();
		this.CarbonaraExploit = new DevExpress.XtraEditors.CheckEdit();
		this.DaCustum = new DevExpress.XtraEditors.ComboBoxEdit();
		this.MTKLoadDA = new System.Windows.Forms.Button();
		this.MTKLoadEMI = new System.Windows.Forms.Button();
		this.MTKClearDA = new System.Windows.Forms.Button();
		this.MTKClearEMI = new System.Windows.Forms.Button();
		this.CMTKPart = new System.Windows.Forms.CheckBox();
		this.MTKResetFrpNew = new DevExpress.XtraEditors.SimpleButton();
		this.MTKRemoveDemo = new DevExpress.XtraEditors.SimpleButton();
		this.MTKSafeFormat = new DevExpress.XtraEditors.SimpleButton();
		this.CMTKServer = new DevExpress.XtraEditors.CheckEdit();
		this.MTKModel = new DevExpress.XtraEditors.ComboBoxEdit();
		this.MTKBrand = new DevExpress.XtraEditors.ComboBoxEdit();
		this.LMTKBrand = new DevExpress.XtraEditors.LabelControl();
		this.LMTKModel = new DevExpress.XtraEditors.LabelControl();
		this.MTKReadback = new DevExpress.XtraEditors.SimpleButton();
		this.MTKPatchVB = new DevExpress.XtraEditors.SimpleButton();
		this.MTKOrangestate = new DevExpress.XtraEditors.SimpleButton();
		this.MTKDMVerity = new DevExpress.XtraEditors.SimpleButton();
		this.MTKLBL = new DevExpress.XtraEditors.SimpleButton();
		this.MTKUBL = new DevExpress.XtraEditors.SimpleButton();
		this.MTKResetFrp = new DevExpress.XtraEditors.SimpleButton();
		this.MTKResetLock = new DevExpress.XtraEditors.SimpleButton();
		this.MTKSecPlugin = new DevExpress.XtraEditors.SimpleButton();
		this.MTKRPMBRestore = new DevExpress.XtraEditors.SimpleButton();
		this.MTKRPMBBackup = new DevExpress.XtraEditors.SimpleButton();
		this.MTKRPMBErase = new DevExpress.XtraEditors.SimpleButton();
		this.MTKNVRestore = new DevExpress.XtraEditors.SimpleButton();
		this.MTKNVBackup = new DevExpress.XtraEditors.SimpleButton();
		this.MTKNVErase = new DevExpress.XtraEditors.SimpleButton();
		this.CMTKDA = new DevExpress.XtraEditors.CheckEdit();
		this.CMTKEMI = new DevExpress.XtraEditors.CheckEdit();
		this.LMTKDA = new DevExpress.XtraEditors.LabelControl();
		this.MTKDA = new DevExpress.XtraEditors.TextEdit();
		this.LMTKEMI = new DevExpress.XtraEditors.LabelControl();
		this.MTKEMI = new DevExpress.XtraEditors.TextEdit();
		this.LMTKFirmware = new DevExpress.XtraEditors.LabelControl();
		this.MTKRepartition = new DevExpress.XtraEditors.CheckEdit();
		this.Highspeed = new DevExpress.XtraEditors.CheckEdit();
		this.MTKFlash = new DevExpress.XtraEditors.SimpleButton();
		this.MTKReboot = new DevExpress.XtraEditors.SimpleButton();
		this.MTKReadGPT = new DevExpress.XtraEditors.SimpleButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.LayoutStore = new DevExpress.XtraEditors.ComboBoxEdit();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.MTKWrite = new DevExpress.XtraBars.BarButtonItem();
		this.MTKRead = new DevExpress.XtraBars.BarButtonItem();
		this.MTKFormat = new DevExpress.XtraBars.BarButtonItem();
		this.WriteCheck = new DevExpress.XtraBars.BarButtonItem();
		this.FormatCheck = new DevExpress.XtraBars.BarButtonItem();
		this.ReadPartition = new DevExpress.XtraBars.BarButtonItem();
		this.EDLFormat = new DevExpress.XtraBars.BarButtonItem();
		this.EDLWrite = new DevExpress.XtraBars.BarButtonItem();
		this.EDLRead = new DevExpress.XtraBars.BarButtonItem();
		this.FBWrite = new DevExpress.XtraBars.BarButtonItem();
		this.FBFormat = new DevExpress.XtraBars.BarButtonItem();
		this.SPDWrite = new DevExpress.XtraBars.BarButtonItem();
		this.SPDFormat = new DevExpress.XtraBars.BarButtonItem();
		this.SPDRead = new DevExpress.XtraBars.BarButtonItem();
		this.simpleButton17 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton14 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton15 = new DevExpress.XtraEditors.SimpleButton();
		this.LabelControl10 = new DevExpress.XtraEditors.LabelControl();
		this.SuperMap = new DevExpress.XtraEditors.ComboBoxEdit();
		this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
		this.MTKFlashMode = new DevExpress.XtraEditors.ComboBoxEdit();
		this.MTKLoadFirmware = new System.Windows.Forms.Button();
		this.MTKFirmware = new DevExpress.XtraEditors.TextEdit();
		this.MTKDataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn69 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MTKResetMICloud = new DevExpress.XtraEditors.SimpleButton();
		this.MTKDisableMiCloud = new DevExpress.XtraEditors.SimpleButton();
		this.MTKHuaweiID = new DevExpress.XtraEditors.SimpleButton();
		this.MTKIMEIRepair = new DevExpress.XtraEditors.SimpleButton();
		this.MTKPayjoy = new DevExpress.XtraEditors.SimpleButton();
		this.MTKOPPOID = new DevExpress.XtraEditors.SimpleButton();
		this.TabSpd = new DevExpress.XtraTab.XtraTabPage();
		this.TabSubSpreadtrum = new DevExpress.XtraTab.XtraTabControl();
		this.TabSpdFlash = new DevExpress.XtraTab.XtraTabPage();
		this.CSPDPart = new System.Windows.Forms.CheckBox();
		this.SPDDataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn12 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn62 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn63 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn65 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn66 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
		this.Skippacth = new DevExpress.XtraEditors.CheckEdit();
		this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
		this.CModelSPD = new DevExpress.XtraEditors.ComboBoxEdit();
		this.SPDResetFrp = new DevExpress.XtraEditors.SimpleButton();
		this.SPDResetLock = new DevExpress.XtraEditors.SimpleButton();
		this.LSPDCPU = new DevExpress.XtraEditors.LabelControl();
		this.CBrandSPD = new DevExpress.XtraEditors.ComboBoxEdit();
		this.SPDReboot = new DevExpress.XtraEditors.SimpleButton();
		this.SPDReadGPT = new DevExpress.XtraEditors.SimpleButton();
		this.SPDATdiag = new DevExpress.XtraTab.XtraTabPage();
		this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
		this.FactoryReset2 = new DevExpress.XtraEditors.SimpleButton();
		this.FactoryReset = new DevExpress.XtraEditors.SimpleButton();
		this.TabHisi = new DevExpress.XtraTab.XtraTabPage();
		this.HISIBLUnlock = new DevExpress.XtraEditors.SimpleButton();
		this.HISIResetFrp = new DevExpress.XtraEditors.SimpleButton();
		this.HISIResetlock = new DevExpress.XtraEditors.SimpleButton();
		this.HISIResetID = new DevExpress.XtraEditors.SimpleButton();
		this.CCPUHisi = new DevExpress.XtraEditors.ComboBoxEdit();
		this.LHisiCPU = new DevExpress.XtraEditors.LabelControl();
		this.TabMeta = new DevExpress.XtraTab.XtraTabPage();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.Transsion = new DevExpress.XtraTab.XtraTabPage();
		this.METAWriteIMEI = new DevExpress.XtraEditors.SimpleButton();
		this.METASecplugin = new DevExpress.XtraEditors.SimpleButton();
		this.METAReboot = new DevExpress.XtraEditors.SimpleButton();
		this.CheckMeta = new System.Windows.Forms.CheckBox();
		this.METAReset = new DevExpress.XtraEditors.SimpleButton();
		this.METAEnableADB = new DevExpress.XtraEditors.SimpleButton();
		this.METADisableADB = new DevExpress.XtraEditors.SimpleButton();
		this.METAPayjoy = new DevExpress.XtraEditors.SimpleButton();
		this.METAFrp = new DevExpress.XtraEditors.SimpleButton();
		this.METAReadGPT = new DevExpress.XtraEditors.SimpleButton();
		this.METADataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn10 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn49 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn53 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn54 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Andproid = new DevExpress.XtraTab.XtraTabPage();
		this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
		this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
		this.SafeFormat = new DevExpress.XtraEditors.SimpleButton();
		this.metaFactoryReset = new DevExpress.XtraEditors.SimpleButton();
		this.TabSamsung = new DevExpress.XtraTab.XtraTabPage();
		this.TabSubSamsung = new DevExpress.XtraTab.XtraTabControl();
		this.TabSamMTP = new DevExpress.XtraTab.XtraTabPage();
		this.GADBMode = new DevExpress.XtraEditors.GroupControl();
		this.FRPRemove16 = new DevExpress.XtraEditors.SimpleButton();
		this.ADBSamFrp = new DevExpress.XtraEditors.SimpleButton();
		this.ADBSamId = new DevExpress.XtraEditors.SimpleButton();
		this.GMTPMode = new DevExpress.XtraEditors.GroupControl();
		this.MTPRebootOdin = new DevExpress.XtraEditors.SimpleButton();
		this.MTPSN = new DevExpress.XtraEditors.SimpleButton();
		this.MTPReset = new DevExpress.XtraEditors.SimpleButton();
		this.MTPFrp16 = new DevExpress.XtraEditors.SimpleButton();
		this.MTPFrp = new DevExpress.XtraEditors.SimpleButton();
		this.MTPInfo = new DevExpress.XtraEditors.SimpleButton();
		this.TabSamsOdin = new DevExpress.XtraTab.XtraTabPage();
		this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
		this.Reset2024 = new DevExpress.XtraEditors.SimpleButton();
		this.OdinInfo = new DevExpress.XtraEditors.SimpleButton();
		this.OdinReboot = new DevExpress.XtraEditors.SimpleButton();
		this.OdinSoftbrick = new DevExpress.XtraEditors.SimpleButton();
		this.OdinFrp = new DevExpress.XtraEditors.SimpleButton();
		this.OdinReset = new DevExpress.XtraEditors.SimpleButton();
		this.OdinWritePit = new DevExpress.XtraEditors.SimpleButton();
		this.OdinReadPit = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		this.CheckOdin = new System.Windows.Forms.CheckBox();
		this.OdinGridView = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn9 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.ClearOdinFirmware = new DevExpress.XtraEditors.SimpleButton();
		this.COdinEraseEFS = new DevExpress.XtraEditors.CheckEdit();
		this.COdinRepartition = new DevExpress.XtraEditors.CheckEdit();
		this.OdinFlash = new DevExpress.XtraEditors.SimpleButton();
		this.LoadHome = new DevExpress.XtraEditors.SimpleButton();
		this.LoadCSC = new DevExpress.XtraEditors.SimpleButton();
		this.LoadCP = new DevExpress.XtraEditors.SimpleButton();
		this.LoadAP = new DevExpress.XtraEditors.SimpleButton();
		this.LoadBL = new DevExpress.XtraEditors.SimpleButton();
		this.txtHOME = new DevExpress.XtraEditors.TextEdit();
		this.txtCSC = new DevExpress.XtraEditors.TextEdit();
		this.txtCP = new DevExpress.XtraEditors.TextEdit();
		this.txtAP = new DevExpress.XtraEditors.TextEdit();
		this.txtBL = new DevExpress.XtraEditors.TextEdit();
		this.TabAdbFastboot = new DevExpress.XtraTab.XtraTabPage();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
		this.FBinfofir = new DevExpress.XtraEditors.CheckEdit();
		this.CFastbootPart = new System.Windows.Forms.CheckBox();
		this.FastbootDataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FBUnlockBL = new DevExpress.XtraEditors.SimpleButton();
		this.FBRebootRecovery = new DevExpress.XtraEditors.SimpleButton();
		this.FBRebootEDL = new DevExpress.XtraEditors.SimpleButton();
		this.FBReadGPT = new DevExpress.XtraEditors.SimpleButton();
		this.FBSlotA = new DevExpress.XtraEditors.SimpleButton();
		this.FBEFSErase = new DevExpress.XtraEditors.SimpleButton();
		this.FBReboot = new DevExpress.XtraEditors.SimpleButton();
		this.FBResetFrp = new DevExpress.XtraEditors.SimpleButton();
		this.FastbootClearFirmware = new DevExpress.XtraEditors.SimpleButton();
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.FBFlash = new DevExpress.XtraEditors.SimpleButton();
		this.FastbootLoadFirmware = new DevExpress.XtraEditors.SimpleButton();
		this.FastbootFirmware = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
		this.CAndroidPart = new System.Windows.Forms.CheckBox();
		this.AndroidDataGrid = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn13 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.txtSearch = new DevExpress.XtraEditors.TextEdit();
		this.SamsungAccount = new DevExpress.XtraEditors.SimpleButton();
		this.DisableKnox = new DevExpress.XtraEditors.SimpleButton();
		this.ADBDisableHU = new DevExpress.XtraEditors.SimpleButton();
		this.RemoveDemo = new DevExpress.XtraEditors.SimpleButton();
		this.OppoSwith = new DevExpress.XtraEditors.SimpleButton();
		this.RemoveMDM = new DevExpress.XtraEditors.SimpleButton();
		this.Disable_OSOnePlus = new DevExpress.XtraEditors.SimpleButton();
		this.DisableOSTecno = new DevExpress.XtraEditors.SimpleButton();
		this.DisableOSItel = new DevExpress.XtraEditors.SimpleButton();
		this.DisableDemo = new DevExpress.XtraEditors.SimpleButton();
		this.DisabaleOSRealme = new DevExpress.XtraEditors.SimpleButton();
		this.ResetHuaweiID = new DevExpress.XtraEditors.SimpleButton();
		this.RemovePlugin = new DevExpress.XtraEditors.SimpleButton();
		this.rebootdevice = new DevExpress.XtraEditors.SimpleButton();
		this.FixAssistantWIFI = new DevExpress.XtraEditors.SimpleButton();
		this.Remove_Payjoy = new DevExpress.XtraEditors.SimpleButton();
		this.Reset_lock = new DevExpress.XtraEditors.SimpleButton();
		this.EnableDiagRoot = new DevExpress.XtraEditors.SimpleButton();
		this.EnableDiagOperation = new DevExpress.XtraEditors.SimpleButton();
		this.DisablePengecualian = new DevExpress.XtraEditors.SimpleButton();
		this.Disable_OS_Sam = new DevExpress.XtraEditors.SimpleButton();
		this.Disable_OSMI = new DevExpress.XtraEditors.SimpleButton();
		this.Disaba_OSOppo = new DevExpress.XtraEditors.SimpleButton();
		this.Disable_osvivo = new DevExpress.XtraEditors.SimpleButton();
		this.EnablediagXiaomi = new DevExpress.XtraEditors.SimpleButton();
		this.FixAssistant = new DevExpress.XtraEditors.SimpleButton();
		this.DidableMicloud = new DevExpress.XtraEditors.SimpleButton();
		this.SamsungID = new DevExpress.XtraEditors.SimpleButton();
		this.EraseFRP16 = new DevExpress.XtraEditors.SimpleButton();
		this.EraseFRP = new DevExpress.XtraEditors.SimpleButton();
		this.ADBEnableApk = new DevExpress.XtraEditors.SimpleButton();
		this.ADBInstallApk = new DevExpress.XtraEditors.SimpleButton();
		this.ADBUninstallApk = new DevExpress.XtraEditors.SimpleButton();
		this.ADBDisableApk = new DevExpress.XtraEditors.SimpleButton();
		this.ADBLoadApk = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.SDresetLock = new DevExpress.XtraEditors.SimpleButton();
		this.SDdisabaleMI = new DevExpress.XtraEditors.SimpleButton();
		this.SDresetMI = new DevExpress.XtraEditors.SimpleButton();
		this.ResetFRPMIasis = new DevExpress.XtraEditors.SimpleButton();
		this.TabNVEditor = new DevExpress.XtraTab.XtraTabPage();
		this.NVRAMEdit = new DevExpress.XtraEditors.SimpleButton();
		this.NVDataEdit = new DevExpress.XtraEditors.SimpleButton();
		this.dataGridViewCheckBoxColumn11 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn55 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn56 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn57 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn58 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn59 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn60 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn61 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LSpeed = new DevExpress.XtraEditors.LabelControl();
		this.ComboPort = new DevExpress.XtraEditors.ComboBoxEdit();
		this.panel1 = new System.Windows.Forms.Panel();
		this.expred = new DevExpress.XtraEditors.LabelControl();
		this.Expired = new DevExpress.XtraEditors.LabelControl();
		this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
		this.PanelEMAIL = new DevExpress.XtraEditors.LabelControl();
		this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
		this.panel3 = new System.Windows.Forms.Panel();
		this.device = new System.Windows.Forms.Button();
		this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
		this.Timer_Sidebar_Menu = new System.Windows.Forms.Timer(this.components);
		this.bar1 = new DevExpress.XtraBars.Bar();
		this.PopMTK = new DevExpress.XtraBars.PopupMenu(this.components);
		this.PopQc = new DevExpress.XtraBars.PopupMenu(this.components);
		this.PopFB = new DevExpress.XtraBars.PopupMenu(this.components);
		this.PopSPD = new DevExpress.XtraBars.PopupMenu(this.components);
		((System.ComponentModel.ISupportInitialize)this.PLog).BeginInit();
		this.PLog.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.SideBar).BeginInit();
		this.SideBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PBar.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.TabMenu).BeginInit();
		this.TabMenu.SuspendLayout();
		this.TabEDL.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TabSubEDL).BeginInit();
		this.TabSubEDL.SuspendLayout();
		this.TabEDLFlashing.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ShowALLqc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLinfofirmware.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLDataGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLLoader.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLStorageMode.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLFirmware.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLPatch.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CEDLServer.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLModel.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.EDLBrand.Properties).BeginInit();
		this.TabEDLNetwork.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl2).BeginInit();
		this.groupControl2.SuspendLayout();
		this.Unlock.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BrandUEDL.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PBUEDL).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.SearchMEDL.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.LMEDL).BeginInit();
		this.TabMTK.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TabSubMTK).BeginInit();
		this.TabSubMTK.SuspendLayout();
		this.TabMTKFlashUnlock.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.InfoFW.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ShowALLMTK.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CarbonaraExploit.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.DaCustum.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKServer.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKModel.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKBrand.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKDA.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKEMI.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKDA.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKEMI.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKRepartition.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Highspeed.Properties).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.LayoutStore.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.SuperMap.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKFlashMode.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKFirmware.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MTKDataGrid).BeginInit();
		this.TabSpd.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TabSubSpreadtrum).BeginInit();
		this.TabSubSpreadtrum.SuspendLayout();
		this.TabSpdFlash.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.SPDDataGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.checkEdit2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Skippacth.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CModelSPD.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CBrandSPD.Properties).BeginInit();
		this.SPDATdiag.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl3).BeginInit();
		this.groupControl3.SuspendLayout();
		this.TabHisi.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CCPUHisi.Properties).BeginInit();
		this.TabMeta.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.Transsion.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.METADataGrid).BeginInit();
		this.Andproid.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl4).BeginInit();
		this.groupControl4.SuspendLayout();
		this.TabSamsung.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TabSubSamsung).BeginInit();
		this.TabSubSamsung.SuspendLayout();
		this.TabSamMTP.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GADBMode).BeginInit();
		this.GADBMode.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GMTPMode).BeginInit();
		this.GMTPMode.SuspendLayout();
		this.TabSamsOdin.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl1).BeginInit();
		this.groupControl1.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.OdinGridView).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.COdinEraseEFS.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.COdinRepartition.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtHOME.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCSC.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCP.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtAP.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtBL.Properties).BeginInit();
		this.TabAdbFastboot.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.FBinfofir.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.FastbootDataGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.FastbootFirmware.Properties).BeginInit();
		this.xtraTabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.AndroidDataGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtSearch.Properties).BeginInit();
		this.xtraTabPage1.SuspendLayout();
		this.TabNVEditor.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ComboPort.Properties).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
		this.panelControl2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.behaviorManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PopMTK).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PopQc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PopFB).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PopSPD).BeginInit();
		base.SuspendLayout();
		this.RLog.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.RLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.RLog.Dock = System.Windows.Forms.DockStyle.Fill;
		this.RLog.ForeColor = System.Drawing.Color.White;
		this.RLog.Location = new System.Drawing.Point(2, 2);
		this.RLog.Name = name;
		this.RLog.Size = new System.Drawing.Size(443, 515);
		this.RLog.TabIndex = 3;
		this.RLog.Text = text;
		this.PLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.PLog.Controls.Add(this.SideBar);
		this.PLog.Controls.Add(this.RLog);
		this.PLog.Location = new System.Drawing.Point(4, 72);
		this.PLog.Name = name2;
		this.PLog.Size = new System.Drawing.Size(447, 519);
		this.PLog.TabIndex = 4;
		this.SideBar.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.SideBar.Appearance.Options.UseBackColor = true;
		this.SideBar.Controls.Add(this.simpleButton1);
		this.SideBar.Controls.Add(this.UnlockUniv);
		this.SideBar.Controls.Add(this.UnisocModule);
		this.SideBar.Controls.Add(this.SamsungModule);
		this.SideBar.Controls.Add(this.NVModule);
		this.SideBar.Controls.Add(this.AndroidModule);
		this.SideBar.Controls.Add(this.HisiModule);
		this.SideBar.Controls.Add(this.METAModule);
		this.SideBar.Controls.Add(this.EDLModule);
		this.SideBar.Controls.Add(this.MTKModule);
		this.SideBar.Location = new System.Drawing.Point(2, 2);
		this.SideBar.MaximumSize = new System.Drawing.Size(217, 600);
		this.SideBar.Name = name3;
		this.SideBar.Size = new System.Drawing.Size(217, 600);
		this.SideBar.TabIndex = 4;
		this.simpleButton1.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.simpleButton1.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name4);
		this.simpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.simpleButton1.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.simpleButton1.Location = new System.Drawing.Point(17, 522);
		this.simpleButton1.Name = name5;
		this.simpleButton1.Size = new System.Drawing.Size(178, 49);
		this.simpleButton1.TabIndex = 9;
		this.simpleButton1.Text = text2;
		this.UnlockUniv.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.UnlockUniv.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name6);
		this.UnlockUniv.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.UnlockUniv.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.UnlockUniv.Location = new System.Drawing.Point(17, 27);
		this.UnlockUniv.Name = name7;
		this.UnlockUniv.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
		this.UnlockUniv.Size = new System.Drawing.Size(178, 49);
		this.UnlockUniv.TabIndex = 8;
		this.UnlockUniv.Text = text3;
		this.UnlockUniv.Click += new System.EventHandler(TabChange);
		this.UnisocModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.UnisocModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name8);
		this.UnisocModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.UnisocModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.UnisocModule.Location = new System.Drawing.Point(17, 192);
		this.UnisocModule.Name = name9;
		this.UnisocModule.Size = new System.Drawing.Size(178, 49);
		this.UnisocModule.TabIndex = 7;
		this.UnisocModule.Text = text4;
		this.UnisocModule.Click += new System.EventHandler(TabChange);
		this.SamsungModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.SamsungModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name10);
		this.SamsungModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.SamsungModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.SamsungModule.Location = new System.Drawing.Point(17, 247);
		this.SamsungModule.Name = name11;
		this.SamsungModule.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.SamsungModule.Size = new System.Drawing.Size(178, 49);
		this.SamsungModule.TabIndex = 6;
		this.SamsungModule.Text = text5;
		this.SamsungModule.Click += new System.EventHandler(TabChange);
		this.NVModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.NVModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name12);
		this.NVModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.NVModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.NVModule.Location = new System.Drawing.Point(17, 467);
		this.NVModule.Name = name13;
		this.NVModule.Size = new System.Drawing.Size(178, 49);
		this.NVModule.TabIndex = 5;
		this.NVModule.Text = text6;
		this.NVModule.Click += new System.EventHandler(TabChange);
		this.AndroidModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.AndroidModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name14);
		this.AndroidModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.AndroidModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.AndroidModule.Location = new System.Drawing.Point(17, 412);
		this.AndroidModule.Name = name15;
		this.AndroidModule.Size = new System.Drawing.Size(178, 49);
		this.AndroidModule.TabIndex = 4;
		this.AndroidModule.Text = text7;
		this.AndroidModule.Click += new System.EventHandler(TabChange);
		this.HisiModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.HisiModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name16);
		this.HisiModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.HisiModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.HisiModule.Location = new System.Drawing.Point(17, 302);
		this.HisiModule.Name = name17;
		this.HisiModule.Size = new System.Drawing.Size(178, 49);
		this.HisiModule.TabIndex = 3;
		this.HisiModule.Text = text8;
		this.HisiModule.Click += new System.EventHandler(TabChange);
		this.METAModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.METAModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name18);
		this.METAModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.METAModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.METAModule.Location = new System.Drawing.Point(17, 357);
		this.METAModule.Name = name19;
		this.METAModule.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.True;
		this.METAModule.Size = new System.Drawing.Size(178, 49);
		this.METAModule.TabIndex = 2;
		this.METAModule.Text = text9;
		this.METAModule.Click += new System.EventHandler(TabChange);
		this.EDLModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.EDLModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name20);
		this.EDLModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.EDLModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.EDLModule.Location = new System.Drawing.Point(17, 137);
		this.EDLModule.Name = name21;
		this.EDLModule.Size = new System.Drawing.Size(178, 49);
		this.EDLModule.TabIndex = 1;
		this.EDLModule.Text = text10;
		this.EDLModule.Click += new System.EventHandler(TabChange);
		this.MTKModule.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.MTKModule.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name22);
		this.MTKModule.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.MTKModule.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.MTKModule.Location = new System.Drawing.Point(17, 82);
		this.MTKModule.Name = name23;
		this.MTKModule.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
		this.MTKModule.Size = new System.Drawing.Size(178, 49);
		this.MTKModule.TabIndex = 0;
		this.MTKModule.Text = text11;
		this.MTKModule.Click += new System.EventHandler(TabChange);
		this.STOP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.STOP.Appearance.BackColor = System.Drawing.Color.Red;
		this.STOP.Appearance.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.STOP.Appearance.Options.UseBackColor = true;
		this.STOP.Appearance.Options.UseFont = true;
		this.STOP.Location = new System.Drawing.Point(1193, 597);
		this.STOP.Name = text12;
		this.STOP.Size = new System.Drawing.Size(75, 23);
		this.STOP.TabIndex = 7;
		this.STOP.Text = text12;
		this.STOP.Click += new System.EventHandler(Stop_Click);
		this.PBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.PBar.Location = new System.Drawing.Point(7, 599);
		this.PBar.Name = name24;
		this.PBar.Properties.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
		this.PBar.Properties.Name = name24;
		this.PBar.Size = new System.Drawing.Size(1180, 20);
		this.PBar.TabIndex = 6;
		this.dataGridViewCheckBoxColumn3.FillWeight = field_2;
		this.dataGridViewCheckBoxColumn3.HeaderText = text;
		this.dataGridViewCheckBoxColumn3.Name = name25;
		this.dataGridViewTextBoxColumn13.FillWeight = field_3;
		this.dataGridViewTextBoxColumn13.HeaderText = headerText;
		this.dataGridViewTextBoxColumn13.Name = name26;
		this.Column6.FillWeight = field_4;
		this.Column6.HeaderText = headerText2;
		this.Column6.Name = name27;
		this.dataGridViewTextBoxColumn14.FillWeight = field_5;
		this.dataGridViewTextBoxColumn14.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn14.Name = name28;
		this.dataGridViewTextBoxColumn15.FillWeight = field_5;
		this.dataGridViewTextBoxColumn15.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn15.Name = name29;
		this.dataGridViewTextBoxColumn16.FillWeight = field_5;
		this.dataGridViewTextBoxColumn16.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn16.Name = name30;
		this.dataGridViewTextBoxColumn17.FillWeight = field_6;
		this.dataGridViewTextBoxColumn17.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn17.Name = name31;
		this.dataGridViewTextBoxColumn18.FillWeight = field_7;
		this.dataGridViewTextBoxColumn18.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn18.Name = name32;
		this.dataGridViewTextBoxColumn19.FillWeight = field_8;
		this.dataGridViewTextBoxColumn19.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn19.Name = name33;
		this.dataGridViewTextBoxColumn19.Visible = false;
		this.dataGridViewCheckBoxColumn2.FillWeight = field_9;
		this.dataGridViewCheckBoxColumn2.HeaderText = text;
		this.dataGridViewCheckBoxColumn2.Name = name34;
		this.dataGridViewTextBoxColumn1.FillWeight = field_10;
		this.dataGridViewTextBoxColumn1.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn1.Name = name35;
		this.dataGridViewTextBoxColumn8.FillWeight = field_10;
		this.dataGridViewTextBoxColumn8.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn8.Name = name36;
		this.dataGridViewTextBoxColumn9.FillWeight = field_10;
		this.dataGridViewTextBoxColumn9.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn9.Name = name37;
		this.dataGridViewTextBoxColumn10.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn10.Name = name38;
		this.dataGridViewTextBoxColumn11.FillWeight = field_8;
		this.dataGridViewTextBoxColumn11.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn11.Name = name39;
		this.dataGridViewTextBoxColumn12.FillWeight = field_8;
		this.dataGridViewTextBoxColumn12.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn12.Name = name40;
		this.dataGridViewTextBoxColumn12.Visible = false;
		this.dataGridViewCheckBoxColumn1.FillWeight = field_9;
		this.dataGridViewCheckBoxColumn1.HeaderText = text;
		this.dataGridViewCheckBoxColumn1.Name = name41;
		this.dataGridViewTextBoxColumn2.FillWeight = field_10;
		this.dataGridViewTextBoxColumn2.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn2.Name = name42;
		this.dataGridViewTextBoxColumn3.FillWeight = field_10;
		this.dataGridViewTextBoxColumn3.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn3.Name = name43;
		this.dataGridViewTextBoxColumn5.FillWeight = field_10;
		this.dataGridViewTextBoxColumn5.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn5.Name = name44;
		this.dataGridViewTextBoxColumn6.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn6.Name = name45;
		this.dataGridViewTextBoxColumn7.FillWeight = field_8;
		this.dataGridViewTextBoxColumn7.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn7.Name = name46;
		this.dataGridViewTextBoxColumn39.FillWeight = field_8;
		this.dataGridViewTextBoxColumn39.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn39.Name = name47;
		this.dataGridViewTextBoxColumn39.Visible = false;
		this.dataGridViewCheckBoxColumn6.FillWeight = field_11;
		this.dataGridViewCheckBoxColumn6.HeaderText = text;
		this.dataGridViewCheckBoxColumn6.Name = name48;
		this.dataGridViewTextBoxColumn4.FillWeight = field_12;
		this.dataGridViewTextBoxColumn4.HeaderText = headerText9;
		this.dataGridViewTextBoxColumn4.Name = name49;
		this.dataGridViewTextBoxColumn22.FillWeight = field_13;
		this.dataGridViewTextBoxColumn22.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn22.Name = name50;
		this.dataGridViewTextBoxColumn23.FillWeight = field_13;
		this.dataGridViewTextBoxColumn23.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn23.Name = name51;
		this.dataGridViewTextBoxColumn24.FillWeight = field_13;
		this.dataGridViewTextBoxColumn24.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn24.Name = name52;
		this.dataGridViewTextBoxColumn25.FillWeight = field_14;
		this.dataGridViewTextBoxColumn25.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn25.Name = name53;
		this.dataGridViewTextBoxColumn26.FillWeight = field_15;
		this.dataGridViewTextBoxColumn26.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn26.Name = name54;
		this.dataGridViewTextBoxColumn27.FillWeight = field_8;
		this.dataGridViewTextBoxColumn27.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn27.Name = name55;
		this.dataGridViewTextBoxColumn27.Visible = false;
		this.dataGridViewCheckBoxColumn4.FillWeight = field_16;
		this.dataGridViewCheckBoxColumn4.HeaderText = text;
		this.dataGridViewCheckBoxColumn4.Name = name56;
		this.dataGridViewTextBoxColumn20.FillWeight = field_17;
		this.dataGridViewTextBoxColumn20.HeaderText = headerText10;
		this.dataGridViewTextBoxColumn20.Name = name57;
		this.dataGridViewTextBoxColumn21.FillWeight = field_18;
		this.dataGridViewTextBoxColumn21.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn21.Name = name58;
		this.dataGridViewCheckBoxColumn5.FillWeight = field_9;
		this.dataGridViewCheckBoxColumn5.HeaderText = text;
		this.dataGridViewCheckBoxColumn5.Name = name59;
		this.Column1.HeaderText = headerText3;
		this.Column1.Name = name60;
		this.Column5.HeaderText = headerText11;
		this.Column5.Name = name61;
		this.Column2.HeaderText = headerText12;
		this.Column2.Name = name62;
		this.Column3.HeaderText = headerText8;
		this.Column3.Name = name63;
		this.Column4.HeaderText = headerText13;
		this.Column4.Name = name64;
		this.Column4.Visible = false;
		this.TabMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TabMenu.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.TabMenu.Appearance.Options.UseBackColor = true;
		this.TabMenu.Location = new System.Drawing.Point(453, 5);
		this.TabMenu.LookAndFeel.SkinName = skinName;
		this.TabMenu.LookAndFeel.UseDefaultLookAndFeel = false;
		this.TabMenu.MultiLine = DevExpress.Utils.DefaultBoolean.False;
		this.TabMenu.Name = name65;
		this.TabMenu.SelectedTabPage = this.TabEDL;
		this.TabMenu.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
		this.TabMenu.Size = new System.Drawing.Size(815, 588);
		this.TabMenu.TabIndex = 8;
		this.TabMenu.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[9] { this.Unlock, this.TabEDL, this.TabMTK, this.TabSpd, this.TabHisi, this.TabMeta, this.TabSamsung, this.TabAdbFastboot, this.TabNVEditor });
		this.TabEDL.Controls.Add(this.TabSubEDL);
		this.TabEDL.Name = name66;
		this.TabEDL.Size = new System.Drawing.Size(813, 586);
		this.TabEDL.Text = text13;
		this.TabSubEDL.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.TabSubEDL.Appearance.Options.UseBackColor = true;
		this.TabSubEDL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabSubEDL.Location = new System.Drawing.Point(0, 0);
		this.TabSubEDL.LookAndFeel.SkinName = skinName;
		this.TabSubEDL.LookAndFeel.UseDefaultLookAndFeel = false;
		this.TabSubEDL.Name = name67;
		this.TabSubEDL.SelectedTabPage = this.TabEDLFlashing;
		this.TabSubEDL.Size = new System.Drawing.Size(813, 586);
		this.TabSubEDL.TabIndex = 0;
		this.TabSubEDL.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[2] { this.TabEDLFlashing, this.TabEDLNetwork });
		this.TabEDLFlashing.Controls.Add(this.SetSlotB);
		this.TabEDLFlashing.Controls.Add(this.SetSlotA);
		this.TabEDLFlashing.Controls.Add(this.QCOppoID);
		this.TabEDLFlashing.Controls.Add(this.ShowALLqc);
		this.TabEDLFlashing.Controls.Add(this.EDLinfofirmware);
		this.TabEDLFlashing.Controls.Add(this.CEDLPart);
		this.TabEDLFlashing.Controls.Add(this.EDLDataGrid);
		this.TabEDLFlashing.Controls.Add(this.EDLFlash);
		this.TabEDLFlashing.Controls.Add(this.EDLReboot);
		this.TabEDLFlashing.Controls.Add(this.EDLReadGPT);
		this.TabEDLFlashing.Controls.Add(this.EDLReadBack);
		this.TabEDLFlashing.Controls.Add(this.EDLResetFrp);
		this.TabEDLFlashing.Controls.Add(this.EDLResetLock);
		this.TabEDLFlashing.Controls.Add(this.EDLMIReset);
		this.TabEDLFlashing.Controls.Add(this.EDLMIDisable);
		this.TabEDLFlashing.Controls.Add(this.EDLEFSRestore);
		this.TabEDLFlashing.Controls.Add(this.EDLEFSBackup);
		this.TabEDLFlashing.Controls.Add(this.EDLEFSErase);
		this.TabEDLFlashing.Controls.Add(this.EDLLoadLoader);
		this.TabEDLFlashing.Controls.Add(this.EDLClearLoader);
		this.TabEDLFlashing.Controls.Add(this.LEDLLoader);
		this.TabEDLFlashing.Controls.Add(this.EDLLoader);
		this.TabEDLFlashing.Controls.Add(this.EDLStorageMode);
		this.TabEDLFlashing.Controls.Add(this.EDLLoadFirmware);
		this.TabEDLFlashing.Controls.Add(this.EDLFirmware);
		this.TabEDLFlashing.Controls.Add(this.LEDLFirmware);
		this.TabEDLFlashing.Controls.Add(this.EDLLoadPatch);
		this.TabEDLFlashing.Controls.Add(this.EDLClearPatch);
		this.TabEDLFlashing.Controls.Add(this.LEDLPatch);
		this.TabEDLFlashing.Controls.Add(this.EDLPatch);
		this.TabEDLFlashing.Controls.Add(this.CEDLServer);
		this.TabEDLFlashing.Controls.Add(this.EDLModel);
		this.TabEDLFlashing.Controls.Add(this.EDLBrand);
		this.TabEDLFlashing.Controls.Add(this.LEDLBrand);
		this.TabEDLFlashing.Controls.Add(this.LEDLModel);
		this.TabEDLFlashing.Name = name68;
		this.TabEDLFlashing.Size = new System.Drawing.Size(811, 555);
		this.TabEDLFlashing.Text = text14;
		this.SetSlotB.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SetSlotB.ImageOptions.Image = AMFTool.Properties.Resources.disable_micloud;
		this.SetSlotB.Location = new System.Drawing.Point(317, 512);
		this.SetSlotB.Name = name69;
		this.SetSlotB.Size = new System.Drawing.Size(111, 28);
		this.SetSlotB.TabIndex = 485;
		this.SetSlotB.Text = text15;
		this.SetSlotB.Click += new System.EventHandler(EDLHandler);
		this.SetSlotA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SetSlotA.ImageOptions.Image = AMFTool.Properties.Resources.disable_micloud;
		this.SetSlotA.Location = new System.Drawing.Point(317, 478);
		this.SetSlotA.Name = name70;
		this.SetSlotA.Size = new System.Drawing.Size(110, 28);
		this.SetSlotA.TabIndex = 484;
		this.SetSlotA.Text = text16;
		this.SetSlotA.Click += new System.EventHandler(EDLHandler);
		this.QCOppoID.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.QCOppoID.Enabled = false;
		this.QCOppoID.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name71);
		this.QCOppoID.Location = new System.Drawing.Point(113, 478);
		this.QCOppoID.Name = name72;
		this.QCOppoID.Size = new System.Drawing.Size(90, 28);
		this.QCOppoID.TabIndex = 483;
		this.QCOppoID.Text = text17;
		this.QCOppoID.Click += new System.EventHandler(EDLHandler);
		this.ShowALLqc.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ShowALLqc.Location = new System.Drawing.Point(6, 298);
		this.ShowALLqc.Name = name73;
		this.ShowALLqc.Properties.AllowFocused = false;
		this.ShowALLqc.Properties.Caption = caption;
		this.ShowALLqc.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.ShowALLqc.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.ShowALLqc.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.ShowALLqc.Size = new System.Drawing.Size(74, 22);
		this.ShowALLqc.TabIndex = 482;
		this.EDLinfofirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLinfofirmware.EditValue = true;
		this.EDLinfofirmware.Location = new System.Drawing.Point(86, 298);
		this.EDLinfofirmware.Name = name74;
		this.EDLinfofirmware.Properties.AllowFocused = false;
		this.EDLinfofirmware.Properties.Caption = caption2;
		this.EDLinfofirmware.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.EDLinfofirmware.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.EDLinfofirmware.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.EDLinfofirmware.Size = new System.Drawing.Size(94, 22);
		this.EDLinfofirmware.TabIndex = 481;
		this.EDLinfofirmware.CheckedChanged += new System.EventHandler(EDLinfofirmware_CheckedChanged);
		this.CEDLPart.AutoSize = true;
		this.CEDLPart.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CEDLPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CEDLPart.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CEDLPart.Location = new System.Drawing.Point(6, 4);
		this.CEDLPart.Name = name75;
		this.CEDLPart.Size = new System.Drawing.Size(12, 11);
		this.CEDLPart.TabIndex = 464;
		this.CEDLPart.UseVisualStyleBackColor = false;
		this.CEDLPart.CheckedChanged += new System.EventHandler(CEDLPart_CheckedChanged);
		this.EDLDataGrid.AllowDrop = true;
		this.EDLDataGrid.AllowUserToAddRows = false;
		this.EDLDataGrid.AllowUserToDeleteRows = false;
		this.EDLDataGrid.AllowUserToResizeColumns = false;
		this.EDLDataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.EDLDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.EDLDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.EDLDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.EDLDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.EDLDataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle2.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Gray;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.EDLDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.EDLDataGrid.ColumnHeadersHeight = 17;
		this.EDLDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.EDLDataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn14, this.dataGridViewTextBoxColumn41, this.Column9, this.dataGridViewTextBoxColumn42, this.dataGridViewTextBoxColumn70, this.dataGridViewTextBoxColumn71, this.dataGridViewTextBoxColumn72, this.Column8, this.dataGridViewTextBoxColumn73);
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle3.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.EDLDataGrid.DefaultCellStyle = dataGridViewCellStyle3;
		this.EDLDataGrid.EnableHeadersVisualStyles = false;
		this.EDLDataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.EDLDataGrid.Location = new System.Drawing.Point(1, 1);
		this.EDLDataGrid.Name = name76;
		this.EDLDataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle4.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.EDLDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.EDLDataGrid.RowHeadersVisible = false;
		this.EDLDataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.EDLDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle5;
		this.EDLDataGrid.RowTemplate.Height = 20;
		this.EDLDataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.EDLDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.EDLDataGrid.Size = new System.Drawing.Size(799, 292);
		this.EDLDataGrid.StandardTab = true;
		this.EDLDataGrid.TabIndex = 463;
		this.EDLDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(EDLDataGrid_CellContentClick);
		this.EDLDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(EDLDataGrid_CellMouseDown);
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle6.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle6.NullValue = false;
		dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn14.DefaultCellStyle = dataGridViewCellStyle6;
		this.dataGridViewCheckBoxColumn14.Frozen = true;
		this.dataGridViewCheckBoxColumn14.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn14.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn14.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn14.Name = name77;
		this.dataGridViewCheckBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn14.Width = 75;
		dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle7.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle7.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn41.DefaultCellStyle = dataGridViewCellStyle7;
		this.dataGridViewTextBoxColumn41.Frozen = true;
		this.dataGridViewTextBoxColumn41.HeaderText = headerText;
		this.dataGridViewTextBoxColumn41.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn41.Name = name78;
		this.dataGridViewTextBoxColumn41.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn41.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn41.Width = 30;
		this.Column9.HeaderText = headerText15;
		this.Column9.Name = name79;
		this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Column9.Width = 50;
		dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle8.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle8.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn42.DefaultCellStyle = dataGridViewCellStyle8;
		this.dataGridViewTextBoxColumn42.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn42.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn42.Name = name80;
		this.dataGridViewTextBoxColumn42.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn42.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn42.Width = 140;
		this.dataGridViewTextBoxColumn70.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle9.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn70.DefaultCellStyle = dataGridViewCellStyle9;
		this.dataGridViewTextBoxColumn70.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn70.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn70.Name = name81;
		this.dataGridViewTextBoxColumn70.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle10.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn71.DefaultCellStyle = dataGridViewCellStyle10;
		this.dataGridViewTextBoxColumn71.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn71.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn71.Name = name82;
		this.dataGridViewTextBoxColumn71.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn71.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn71.Width = 85;
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle11.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn72.DefaultCellStyle = dataGridViewCellStyle11;
		this.dataGridViewTextBoxColumn72.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn72.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn72.Name = name83;
		this.dataGridViewTextBoxColumn72.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn72.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn72.Width = 85;
		this.Column8.HeaderText = headerText7;
		this.Column8.Name = name84;
		dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle12.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn73.DefaultCellStyle = dataGridViewCellStyle12;
		this.dataGridViewTextBoxColumn73.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn73.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn73.Name = name85;
		this.dataGridViewTextBoxColumn73.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn73.Visible = false;
		this.EDLFlash.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLFlash.ImageOptions.Image = AMFTool.Properties.Resources.flash;
		this.EDLFlash.Location = new System.Drawing.Point(509, 300);
		this.EDLFlash.Name = name86;
		this.EDLFlash.Size = new System.Drawing.Size(90, 28);
		this.EDLFlash.TabIndex = 449;
		this.EDLFlash.Text = text18;
		this.EDLFlash.Click += new System.EventHandler(EDLHandler);
		this.EDLReboot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLReboot.ImageOptions.Image = AMFTool.Properties.Resources.REboot;
		this.EDLReboot.Location = new System.Drawing.Point(605, 300);
		this.EDLReboot.Name = name87;
		this.EDLReboot.Size = new System.Drawing.Size(90, 28);
		this.EDLReboot.TabIndex = 448;
		this.EDLReboot.Text = text19;
		this.EDLReboot.Click += new System.EventHandler(EDLHandler);
		this.EDLReadGPT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLReadGPT.ImageOptions.Image = AMFTool.Properties.Resources.GPT_;
		this.EDLReadGPT.Location = new System.Drawing.Point(701, 300);
		this.EDLReadGPT.Name = name88;
		this.EDLReadGPT.Size = new System.Drawing.Size(90, 28);
		this.EDLReadGPT.TabIndex = 447;
		this.EDLReadGPT.Text = text20;
		this.EDLReadGPT.Click += new System.EventHandler(EDLHandler);
		this.EDLReadBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLReadBack.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name89);
		this.EDLReadBack.Location = new System.Drawing.Point(409, 300);
		this.EDLReadBack.Name = name90;
		this.EDLReadBack.Size = new System.Drawing.Size(90, 28);
		this.EDLReadBack.TabIndex = 446;
		this.EDLReadBack.Text = text21;
		this.EDLReadBack.Click += new System.EventHandler(EDLHandler);
		this.EDLResetFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLResetFrp.ImageOptions.Image = AMFTool.Properties.Resources.FRP;
		this.EDLResetFrp.Location = new System.Drawing.Point(434, 512);
		this.EDLResetFrp.Name = name91;
		this.EDLResetFrp.Size = new System.Drawing.Size(103, 28);
		this.EDLResetFrp.TabIndex = 440;
		this.EDLResetFrp.Text = text22;
		this.EDLResetFrp.Click += new System.EventHandler(EDLHandler);
		this.EDLResetLock.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLResetLock.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.EDLResetLock.Location = new System.Drawing.Point(433, 477);
		this.EDLResetLock.Name = name92;
		this.EDLResetLock.Size = new System.Drawing.Size(103, 28);
		this.EDLResetLock.TabIndex = 439;
		this.EDLResetLock.Text = text23;
		this.EDLResetLock.Click += new System.EventHandler(EDLHandler);
		this.EDLMIReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLMIReset.ImageOptions.Image = AMFTool.Properties.Resources.reset_mcloud;
		this.EDLMIReset.Location = new System.Drawing.Point(209, 478);
		this.EDLMIReset.Name = name93;
		this.EDLMIReset.Size = new System.Drawing.Size(101, 28);
		this.EDLMIReset.TabIndex = 438;
		this.EDLMIReset.Text = text24;
		this.EDLMIReset.Click += new System.EventHandler(EDLHandler);
		this.EDLMIDisable.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLMIDisable.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name94);
		this.EDLMIDisable.Location = new System.Drawing.Point(209, 512);
		this.EDLMIDisable.Name = name95;
		this.EDLMIDisable.Size = new System.Drawing.Size(101, 28);
		this.EDLMIDisable.TabIndex = 437;
		this.EDLMIDisable.Text = text25;
		this.EDLMIDisable.Click += new System.EventHandler(EDLHandler);
		this.EDLEFSRestore.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLEFSRestore.ImageOptions.Image = AMFTool.Properties.Resources.WRITE;
		this.EDLEFSRestore.Location = new System.Drawing.Point(113, 512);
		this.EDLEFSRestore.Name = name96;
		this.EDLEFSRestore.Size = new System.Drawing.Size(90, 28);
		this.EDLEFSRestore.TabIndex = 436;
		this.EDLEFSRestore.Text = text26;
		this.EDLEFSRestore.Click += new System.EventHandler(EDLHandler);
		this.EDLEFSBackup.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLEFSBackup.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.EDLEFSBackup.Location = new System.Drawing.Point(16, 478);
		this.EDLEFSBackup.Name = name97;
		this.EDLEFSBackup.Size = new System.Drawing.Size(90, 28);
		this.EDLEFSBackup.TabIndex = 435;
		this.EDLEFSBackup.Text = text27;
		this.EDLEFSBackup.Click += new System.EventHandler(EDLHandler);
		this.EDLEFSErase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLEFSErase.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.EDLEFSErase.Location = new System.Drawing.Point(16, 512);
		this.EDLEFSErase.Name = name98;
		this.EDLEFSErase.Size = new System.Drawing.Size(90, 28);
		this.EDLEFSErase.TabIndex = 434;
		this.EDLEFSErase.Text = text28;
		this.EDLEFSErase.Click += new System.EventHandler(EDLHandler);
		this.EDLLoadLoader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLLoadLoader.Location = new System.Drawing.Point(713, 373);
		this.EDLLoadLoader.Name = name99;
		this.EDLLoadLoader.Size = new System.Drawing.Size(47, 20);
		this.EDLLoadLoader.TabIndex = 433;
		this.EDLLoadLoader.Text = text29;
		this.EDLLoadLoader.Click += new System.EventHandler(EDLLoad);
		this.EDLClearLoader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLClearLoader.Location = new System.Drawing.Point(660, 373);
		this.EDLClearLoader.Name = name100;
		this.EDLClearLoader.Size = new System.Drawing.Size(47, 20);
		this.EDLClearLoader.TabIndex = 432;
		this.EDLClearLoader.Text = text30;
		this.EDLClearLoader.Click += new System.EventHandler(EDLClear);
		this.LEDLLoader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LEDLLoader.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LEDLLoader.Appearance.Options.UseFont = true;
		this.LEDLLoader.Location = new System.Drawing.Point(15, 375);
		this.LEDLLoader.Name = name101;
		this.LEDLLoader.Size = new System.Drawing.Size(49, 15);
		this.LEDLLoader.TabIndex = 431;
		this.LEDLLoader.Text = text31;
		this.EDLLoader.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.EDLLoader.Location = new System.Drawing.Point(83, 374);
		this.EDLLoader.Name = name102;
		this.EDLLoader.Size = new System.Drawing.Size(571, 20);
		this.EDLLoader.TabIndex = 430;
		this.EDLStorageMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLStorageMode.EditValue = text32;
		this.EDLStorageMode.Location = new System.Drawing.Point(620, 441);
		this.EDLStorageMode.Name = name103;
		this.EDLStorageMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.EDLStorageMode.Properties.Items.AddRange(new object[2] { text32, text33 });
		this.EDLStorageMode.Size = new System.Drawing.Size(87, 20);
		this.EDLStorageMode.TabIndex = 429;
		this.EDLStorageMode.SelectedIndexChanged += new System.EventHandler(EDLStorageMode_SelectedIndexChanged);
		this.EDLLoadFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLLoadFirmware.Location = new System.Drawing.Point(713, 441);
		this.EDLLoadFirmware.Name = name104;
		this.EDLLoadFirmware.Size = new System.Drawing.Size(47, 20);
		this.EDLLoadFirmware.TabIndex = 428;
		this.EDLLoadFirmware.Text = text29;
		this.EDLLoadFirmware.Click += new System.EventHandler(EDLLoad);
		this.EDLFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.EDLFirmware.Location = new System.Drawing.Point(82, 442);
		this.EDLFirmware.Name = name105;
		this.EDLFirmware.Size = new System.Drawing.Size(531, 20);
		this.EDLFirmware.TabIndex = 427;
		this.LEDLFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LEDLFirmware.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LEDLFirmware.Appearance.Options.UseFont = true;
		this.LEDLFirmware.Location = new System.Drawing.Point(15, 444);
		this.LEDLFirmware.Name = name106;
		this.LEDLFirmware.Size = new System.Drawing.Size(52, 15);
		this.LEDLFirmware.TabIndex = 426;
		this.LEDLFirmware.Text = text34;
		this.EDLLoadPatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLLoadPatch.Location = new System.Drawing.Point(713, 407);
		this.EDLLoadPatch.Name = name107;
		this.EDLLoadPatch.Size = new System.Drawing.Size(47, 20);
		this.EDLLoadPatch.TabIndex = 425;
		this.EDLLoadPatch.Text = text29;
		this.EDLLoadPatch.Click += new System.EventHandler(EDLLoad);
		this.EDLClearPatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.EDLClearPatch.Location = new System.Drawing.Point(660, 407);
		this.EDLClearPatch.Name = name108;
		this.EDLClearPatch.Size = new System.Drawing.Size(47, 20);
		this.EDLClearPatch.TabIndex = 424;
		this.EDLClearPatch.Text = text30;
		this.EDLClearPatch.Click += new System.EventHandler(EDLClear);
		this.LEDLPatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LEDLPatch.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LEDLPatch.Appearance.Options.UseFont = true;
		this.LEDLPatch.Location = new System.Drawing.Point(15, 410);
		this.LEDLPatch.Name = name109;
		this.LEDLPatch.Size = new System.Drawing.Size(31, 15);
		this.LEDLPatch.TabIndex = 423;
		this.LEDLPatch.Text = text35;
		this.EDLPatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.EDLPatch.Location = new System.Drawing.Point(83, 407);
		this.EDLPatch.Name = name110;
		this.EDLPatch.Size = new System.Drawing.Size(571, 20);
		this.EDLPatch.TabIndex = 422;
		this.CEDLServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.CEDLServer.EditValue = true;
		this.CEDLServer.Location = new System.Drawing.Point(714, 338);
		this.CEDLServer.Name = name111;
		this.CEDLServer.Properties.AllowFocused = false;
		this.CEDLServer.Properties.Caption = caption3;
		this.CEDLServer.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CEDLServer.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CEDLServer.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CEDLServer.Size = new System.Drawing.Size(62, 22);
		this.CEDLServer.TabIndex = 421;
		this.CEDLServer.CheckedChanged += new System.EventHandler(CEDLServer_CheckedChanged);
		this.EDLModel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.EDLModel.Location = new System.Drawing.Point(412, 339);
		this.EDLModel.Name = name112;
		this.EDLModel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.EDLModel.Size = new System.Drawing.Size(295, 20);
		this.EDLModel.TabIndex = 420;
		this.EDLBrand.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EDLBrand.EditValue = text;
		this.EDLBrand.Location = new System.Drawing.Point(83, 339);
		this.EDLBrand.Name = name113;
		this.EDLBrand.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.EDLBrand.Size = new System.Drawing.Size(284, 20);
		this.EDLBrand.TabIndex = 419;
		this.EDLBrand.SelectedIndexChanged += new System.EventHandler(EDLBrand_SelectedIndexChanged);
		this.LEDLBrand.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LEDLBrand.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LEDLBrand.Appearance.Options.UseFont = true;
		this.LEDLBrand.Location = new System.Drawing.Point(15, 340);
		this.LEDLBrand.Name = name114;
		this.LEDLBrand.Size = new System.Drawing.Size(33, 15);
		this.LEDLBrand.TabIndex = 417;
		this.LEDLBrand.Text = text36;
		this.LEDLModel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.LEDLModel.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LEDLModel.Appearance.Options.UseFont = true;
		this.LEDLModel.Location = new System.Drawing.Point(373, 340);
		this.LEDLModel.Name = name115;
		this.LEDLModel.Size = new System.Drawing.Size(33, 15);
		this.LEDLModel.TabIndex = 418;
		this.LEDLModel.Text = text37;
		this.TabEDLNetwork.Controls.Add(this.groupControl2);
		this.TabEDLNetwork.Name = name116;
		this.TabEDLNetwork.Size = new System.Drawing.Size(811, 526);
		this.TabEDLNetwork.Text = text38;
		this.groupControl2.Controls.Add(this.QCN_Editor);
		this.groupControl2.Controls.Add(this.DiagEnableXiaomi);
		this.groupControl2.Controls.Add(this.DiagEnable);
		this.groupControl2.Controls.Add(this.DiagEnableRoot);
		this.groupControl2.Controls.Add(this.DiagCriticalWrite);
		this.groupControl2.Controls.Add(this.DiagCriticalRead);
		this.groupControl2.Controls.Add(this.DiagQCNWrite);
		this.groupControl2.Controls.Add(this.DiagQCNRead);
		this.groupControl2.Controls.Add(this.DiagWriteIMEI);
		this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupControl2.Location = new System.Drawing.Point(0, 0);
		this.groupControl2.Name = name117;
		this.groupControl2.Size = new System.Drawing.Size(811, 526);
		this.groupControl2.TabIndex = 0;
		this.groupControl2.Text = text39;
		this.QCN_Editor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.QCN_Editor.Enabled = false;
		this.QCN_Editor.Location = new System.Drawing.Point(10, 148);
		this.QCN_Editor.Name = name118;
		this.QCN_Editor.Size = new System.Drawing.Size(791, 26);
		this.QCN_Editor.TabIndex = 16;
		this.QCN_Editor.Text = text40;
		this.QCN_Editor.Click += new System.EventHandler(DiagHandler);
		this.DiagEnableXiaomi.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagEnableXiaomi.Location = new System.Drawing.Point(10, 281);
		this.DiagEnableXiaomi.Name = name119;
		this.DiagEnableXiaomi.Size = new System.Drawing.Size(791, 26);
		this.DiagEnableXiaomi.TabIndex = 15;
		this.DiagEnableXiaomi.Text = text41;
		this.DiagEnableXiaomi.Click += new System.EventHandler(DiagHandler);
		this.DiagEnable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagEnable.Location = new System.Drawing.Point(10, 249);
		this.DiagEnable.Name = name120;
		this.DiagEnable.Size = new System.Drawing.Size(791, 26);
		this.DiagEnable.TabIndex = 14;
		this.DiagEnable.Text = text42;
		this.DiagEnable.Click += new System.EventHandler(DiagHandler);
		this.DiagEnableRoot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagEnableRoot.Location = new System.Drawing.Point(10, 313);
		this.DiagEnableRoot.Name = name121;
		this.DiagEnableRoot.Size = new System.Drawing.Size(791, 26);
		this.DiagEnableRoot.TabIndex = 13;
		this.DiagEnableRoot.Text = text43;
		this.DiagEnableRoot.Click += new System.EventHandler(DiagHandler);
		this.DiagCriticalWrite.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagCriticalWrite.Location = new System.Drawing.Point(10, 217);
		this.DiagCriticalWrite.Name = name122;
		this.DiagCriticalWrite.Size = new System.Drawing.Size(791, 26);
		this.DiagCriticalWrite.TabIndex = 12;
		this.DiagCriticalWrite.Text = text44;
		this.DiagCriticalWrite.Click += new System.EventHandler(DiagHandler);
		this.DiagCriticalRead.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagCriticalRead.Location = new System.Drawing.Point(10, 185);
		this.DiagCriticalRead.Name = name123;
		this.DiagCriticalRead.Size = new System.Drawing.Size(791, 26);
		this.DiagCriticalRead.TabIndex = 11;
		this.DiagCriticalRead.Text = text45;
		this.DiagCriticalRead.Click += new System.EventHandler(DiagHandler);
		this.DiagQCNWrite.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagQCNWrite.Location = new System.Drawing.Point(10, 116);
		this.DiagQCNWrite.Name = name124;
		this.DiagQCNWrite.Size = new System.Drawing.Size(791, 26);
		this.DiagQCNWrite.TabIndex = 10;
		this.DiagQCNWrite.Text = text46;
		this.DiagQCNWrite.Click += new System.EventHandler(DiagHandler);
		this.DiagQCNRead.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagQCNRead.Location = new System.Drawing.Point(10, 84);
		this.DiagQCNRead.Name = name125;
		this.DiagQCNRead.Size = new System.Drawing.Size(791, 26);
		this.DiagQCNRead.TabIndex = 9;
		this.DiagQCNRead.Text = text47;
		this.DiagQCNRead.Click += new System.EventHandler(DiagHandler);
		this.DiagWriteIMEI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DiagWriteIMEI.Location = new System.Drawing.Point(10, 52);
		this.DiagWriteIMEI.Name = name126;
		this.DiagWriteIMEI.Size = new System.Drawing.Size(791, 26);
		this.DiagWriteIMEI.TabIndex = 8;
		this.DiagWriteIMEI.Text = text48;
		this.DiagWriteIMEI.Click += new System.EventHandler(DiagHandler);
		this.Unlock.Controls.Add(this.BrandUEDL);
		this.Unlock.Controls.Add(this.PBUEDL);
		this.Unlock.Controls.Add(this.SearchMEDL);
		this.Unlock.Controls.Add(this.LMEDL);
		this.Unlock.Controls.Add(this.panel4);
		this.Unlock.Name = text49;
		this.Unlock.Size = new System.Drawing.Size(813, 557);
		this.Unlock.Text = text49;
		this.BrandUEDL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.BrandUEDL.EditValue = editValue;
		this.BrandUEDL.Location = new System.Drawing.Point(3, 16);
		this.BrandUEDL.Name = name127;
		this.BrandUEDL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.BrandUEDL.Properties.Items.AddRange(new object[12]
		{
			text50, text51, text52, text53, text54, text55, text56, text57, text58, text59,
			text60, text61
		});
		this.BrandUEDL.Size = new System.Drawing.Size(316, 20);
		this.BrandUEDL.TabIndex = 7;
		this.BrandUEDL.SelectedIndexChanged += new System.EventHandler(BrandUEDL_SelectedIndexChanged);
		this.PBUEDL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.PBUEDL.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.PBUEDL.Appearance.Options.UseBackColor = true;
		this.PBUEDL.Location = new System.Drawing.Point(325, 18);
		this.PBUEDL.Name = name128;
		this.PBUEDL.Size = new System.Drawing.Size(484, 536);
		this.PBUEDL.TabIndex = 9;
		this.SearchMEDL.Client = this.LMEDL;
		this.SearchMEDL.Location = new System.Drawing.Point(3, 44);
		this.SearchMEDL.Name = name129;
		this.SearchMEDL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
		{
			new DevExpress.XtraEditors.Repository.ClearButton(),
			new DevExpress.XtraEditors.Repository.SearchButton()
		});
		this.SearchMEDL.Properties.Client = this.LMEDL;
		this.SearchMEDL.Properties.FindDelay = 100;
		this.SearchMEDL.Size = new System.Drawing.Size(316, 20);
		this.SearchMEDL.TabIndex = 0;
		this.SearchMEDL.SelectedIndexChanged += new System.EventHandler(SearchMEDL_EditValueChanged);
		this.LMEDL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LMEDL.ItemHeight = 52;
		this.LMEDL.Location = new System.Drawing.Point(3, 73);
		this.LMEDL.LookAndFeel.SkinName = skinName;
		this.LMEDL.LookAndFeel.UseDefaultLookAndFeel = false;
		this.LMEDL.Name = name130;
		this.LMEDL.Size = new System.Drawing.Size(316, 482);
		this.LMEDL.TabIndex = 0;
		this.LMEDL.SelectedIndexChanged += new System.EventHandler(LMEDL_SelectedIndexChanged);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = name131;
		this.panel4.Size = new System.Drawing.Size(813, 557);
		this.panel4.TabIndex = 0;
		this.TabMTK.Controls.Add(this.TabSubMTK);
		this.TabMTK.Name = name132;
		this.TabMTK.Size = new System.Drawing.Size(813, 557);
		this.TabMTK.Text = text62;
		this.TabSubMTK.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.TabSubMTK.Appearance.Options.UseBackColor = true;
		this.TabSubMTK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabSubMTK.Location = new System.Drawing.Point(0, 0);
		this.TabSubMTK.LookAndFeel.SkinName = skinName;
		this.TabSubMTK.LookAndFeel.UseDefaultLookAndFeel = false;
		this.TabSubMTK.Name = name133;
		this.TabSubMTK.SelectedTabPage = this.TabMTKFlashUnlock;
		this.TabSubMTK.Size = new System.Drawing.Size(813, 557);
		this.TabSubMTK.TabIndex = 1;
		this.TabSubMTK.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.TabMTKFlashUnlock });
		this.TabMTKFlashUnlock.Controls.Add(this.InfoFW);
		this.TabMTKFlashUnlock.Controls.Add(this.simpleButton16);
		this.TabMTKFlashUnlock.Controls.Add(this.RebuildUserDATA);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKWiteemi);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKReadDUMP);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKReadFlash);
		this.TabMTKFlashUnlock.Controls.Add(this.ShowALLMTK);
		this.TabMTKFlashUnlock.Controls.Add(this.CarbonaraExploit);
		this.TabMTKFlashUnlock.Controls.Add(this.DaCustum);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKLoadDA);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKLoadEMI);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKClearDA);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKClearEMI);
		this.TabMTKFlashUnlock.Controls.Add(this.CMTKPart);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKResetFrpNew);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKRemoveDemo);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKSafeFormat);
		this.TabMTKFlashUnlock.Controls.Add(this.CMTKServer);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKModel);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKBrand);
		this.TabMTKFlashUnlock.Controls.Add(this.LMTKBrand);
		this.TabMTKFlashUnlock.Controls.Add(this.LMTKModel);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKReadback);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKPatchVB);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKOrangestate);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKDMVerity);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKLBL);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKUBL);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKResetFrp);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKResetLock);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKSecPlugin);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKRPMBRestore);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKRPMBBackup);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKRPMBErase);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKNVRestore);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKNVBackup);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKNVErase);
		this.TabMTKFlashUnlock.Controls.Add(this.CMTKDA);
		this.TabMTKFlashUnlock.Controls.Add(this.CMTKEMI);
		this.TabMTKFlashUnlock.Controls.Add(this.LMTKDA);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKDA);
		this.TabMTKFlashUnlock.Controls.Add(this.LMTKEMI);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKEMI);
		this.TabMTKFlashUnlock.Controls.Add(this.LMTKFirmware);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKRepartition);
		this.TabMTKFlashUnlock.Controls.Add(this.Highspeed);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKFlash);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKReboot);
		this.TabMTKFlashUnlock.Controls.Add(this.MTKReadGPT);
		this.TabMTKFlashUnlock.Controls.Add(this.panel5);
		this.TabMTKFlashUnlock.Name = name134;
		this.TabMTKFlashUnlock.Size = new System.Drawing.Size(811, 526);
		this.TabMTKFlashUnlock.Text = text63;
		this.InfoFW.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.InfoFW.EditValue = true;
		this.InfoFW.Location = new System.Drawing.Point(87, 234);
		this.InfoFW.Name = name135;
		this.InfoFW.Properties.AllowFocused = false;
		this.InfoFW.Properties.Caption = caption2;
		this.InfoFW.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.InfoFW.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.InfoFW.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.InfoFW.Size = new System.Drawing.Size(94, 22);
		this.InfoFW.TabIndex = 480;
		this.InfoFW.CheckedChanged += new System.EventHandler(InfoFW_CheckedChanged);
		this.simpleButton16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.simpleButton16.Enabled = false;
		this.simpleButton16.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name136);
		this.simpleButton16.Location = new System.Drawing.Point(386, 394);
		this.simpleButton16.Name = name137;
		this.simpleButton16.Size = new System.Drawing.Size(100, 28);
		this.simpleButton16.TabIndex = 478;
		this.simpleButton16.Text = text64;
		this.RebuildUserDATA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.RebuildUserDATA.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name138);
		this.RebuildUserDATA.Location = new System.Drawing.Point(696, 394);
		this.RebuildUserDATA.Name = name139;
		this.RebuildUserDATA.Size = new System.Drawing.Size(108, 28);
		this.RebuildUserDATA.TabIndex = 475;
		this.RebuildUserDATA.Text = text65;
		this.RebuildUserDATA.Click += new System.EventHandler(MTKHandler);
		this.MTKWiteemi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKWiteemi.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name140);
		this.MTKWiteemi.Location = new System.Drawing.Point(491, 394);
		this.MTKWiteemi.Name = name141;
		this.MTKWiteemi.Size = new System.Drawing.Size(90, 28);
		this.MTKWiteemi.TabIndex = 474;
		this.MTKWiteemi.Text = text66;
		this.MTKWiteemi.Click += new System.EventHandler(MTKHandler);
		this.MTKReadDUMP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKReadDUMP.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name142);
		this.MTKReadDUMP.Location = new System.Drawing.Point(696, 426);
		this.MTKReadDUMP.Name = name143;
		this.MTKReadDUMP.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
		this.MTKReadDUMP.Size = new System.Drawing.Size(108, 28);
		this.MTKReadDUMP.TabIndex = 473;
		this.MTKReadDUMP.Text = text67;
		this.MTKReadDUMP.Click += new System.EventHandler(MTKHandler);
		this.MTKReadFlash.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKReadFlash.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name144);
		this.MTKReadFlash.Location = new System.Drawing.Point(696, 459);
		this.MTKReadFlash.Name = name145;
		this.MTKReadFlash.Size = new System.Drawing.Size(108, 28);
		this.MTKReadFlash.TabIndex = 472;
		this.MTKReadFlash.Text = text68;
		this.MTKReadFlash.Click += new System.EventHandler(MTKHandler);
		this.ShowALLMTK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ShowALLMTK.Location = new System.Drawing.Point(7, 234);
		this.ShowALLMTK.Name = name146;
		this.ShowALLMTK.Properties.AllowFocused = false;
		this.ShowALLMTK.Properties.Caption = caption;
		this.ShowALLMTK.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.ShowALLMTK.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.ShowALLMTK.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.ShowALLMTK.Size = new System.Drawing.Size(74, 22);
		this.ShowALLMTK.TabIndex = 471;
		this.ShowALLMTK.CheckedChanged += new System.EventHandler(ShowALLMTK_CheckedChanged);
		this.CarbonaraExploit.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.CarbonaraExploit.Location = new System.Drawing.Point(359, 234);
		this.CarbonaraExploit.Name = name147;
		this.CarbonaraExploit.Properties.AllowFocused = false;
		this.CarbonaraExploit.Properties.Caption = caption4;
		this.CarbonaraExploit.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CarbonaraExploit.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CarbonaraExploit.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CarbonaraExploit.Size = new System.Drawing.Size(113, 22);
		this.CarbonaraExploit.TabIndex = 470;
		this.CarbonaraExploit.CheckedChanged += new System.EventHandler(CarbonaraExploit_CheckedChanged);
		this.DaCustum.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.DaCustum.Location = new System.Drawing.Point(461, 301);
		this.DaCustum.Name = name148;
		this.DaCustum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.DaCustum.Size = new System.Drawing.Size(248, 20);
		this.DaCustum.TabIndex = 469;
		this.DaCustum.SelectedIndexChanged += new System.EventHandler(DaCustum_SelectedIndexChanged);
		this.MTKLoadDA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKLoadDA.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKLoadDA.FlatAppearance.BorderSize = 0;
		this.MTKLoadDA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.MTKLoadDA.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.MTKLoadDA.Location = new System.Drawing.Point(687, 338);
		this.MTKLoadDA.Name = name149;
		this.MTKLoadDA.Size = new System.Drawing.Size(17, 13);
		this.MTKLoadDA.TabIndex = 466;
		this.MTKLoadDA.UseVisualStyleBackColor = false;
		this.MTKLoadDA.Click += new System.EventHandler(MTKLoad);
		this.MTKLoadEMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKLoadEMI.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKLoadEMI.Enabled = false;
		this.MTKLoadEMI.FlatAppearance.BorderSize = 0;
		this.MTKLoadEMI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.MTKLoadEMI.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.MTKLoadEMI.Location = new System.Drawing.Point(433, 304);
		this.MTKLoadEMI.Name = name150;
		this.MTKLoadEMI.Size = new System.Drawing.Size(17, 13);
		this.MTKLoadEMI.TabIndex = 465;
		this.MTKLoadEMI.UseVisualStyleBackColor = false;
		this.MTKLoadEMI.Click += new System.EventHandler(MTKLoad);
		this.MTKClearDA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKClearDA.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKClearDA.FlatAppearance.BorderSize = 0;
		this.MTKClearDA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.MTKClearDA.Image = AMFTool.Properties.Resources.icons8_x_13px;
		this.MTKClearDA.Location = new System.Drawing.Point(110, 338);
		this.MTKClearDA.Name = name151;
		this.MTKClearDA.Size = new System.Drawing.Size(13, 13);
		this.MTKClearDA.TabIndex = 464;
		this.MTKClearDA.UseVisualStyleBackColor = false;
		this.MTKClearDA.Click += new System.EventHandler(MTKClear);
		this.MTKClearEMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKClearEMI.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKClearEMI.FlatAppearance.BorderSize = 0;
		this.MTKClearEMI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.MTKClearEMI.Image = AMFTool.Properties.Resources.icons8_x_13px;
		this.MTKClearEMI.Location = new System.Drawing.Point(109, 304);
		this.MTKClearEMI.Name = name152;
		this.MTKClearEMI.Size = new System.Drawing.Size(17, 13);
		this.MTKClearEMI.TabIndex = 463;
		this.MTKClearEMI.UseVisualStyleBackColor = false;
		this.MTKClearEMI.Click += new System.EventHandler(MTKClear);
		this.CMTKPart.AutoSize = true;
		this.CMTKPart.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CMTKPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CMTKPart.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CMTKPart.Location = new System.Drawing.Point(7, 4);
		this.CMTKPart.Name = name153;
		this.CMTKPart.Size = new System.Drawing.Size(12, 11);
		this.CMTKPart.TabIndex = 462;
		this.CMTKPart.UseVisualStyleBackColor = false;
		this.CMTKPart.CheckedChanged += new System.EventHandler(CMTKPart_CheckedChanged);
		this.MTKResetFrpNew.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKResetFrpNew.Enabled = false;
		this.MTKResetFrpNew.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name154);
		this.MTKResetFrpNew.Location = new System.Drawing.Point(94, 394);
		this.MTKResetFrpNew.Name = name155;
		this.MTKResetFrpNew.Size = new System.Drawing.Size(98, 28);
		this.MTKResetFrpNew.TabIndex = 460;
		this.MTKResetFrpNew.Text = text69;
		this.MTKRemoveDemo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKRemoveDemo.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name156);
		this.MTKRemoveDemo.Location = new System.Drawing.Point(491, 425);
		this.MTKRemoveDemo.Name = name157;
		this.MTKRemoveDemo.Size = new System.Drawing.Size(90, 28);
		this.MTKRemoveDemo.TabIndex = 459;
		this.MTKRemoveDemo.Text = text70;
		this.MTKRemoveDemo.Click += new System.EventHandler(MTKHandler);
		this.MTKSafeFormat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKSafeFormat.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name158);
		this.MTKSafeFormat.Location = new System.Drawing.Point(385, 491);
		this.MTKSafeFormat.Name = name159;
		this.MTKSafeFormat.Size = new System.Drawing.Size(100, 28);
		this.MTKSafeFormat.TabIndex = 458;
		this.MTKSafeFormat.Text = text71;
		this.MTKSafeFormat.Click += new System.EventHandler(MTKHandler);
		this.CMTKServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.CMTKServer.Location = new System.Drawing.Point(744, 268);
		this.CMTKServer.Name = name160;
		this.CMTKServer.Properties.AllowFocused = false;
		this.CMTKServer.Properties.Caption = caption3;
		this.CMTKServer.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CMTKServer.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CMTKServer.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CMTKServer.Size = new System.Drawing.Size(62, 22);
		this.CMTKServer.TabIndex = 457;
		this.CMTKServer.CheckedChanged += new System.EventHandler(CMTKServer_CheckedChanged);
		this.MTKModel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKModel.Location = new System.Drawing.Point(436, 269);
		this.MTKModel.Name = name161;
		this.MTKModel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.MTKModel.Size = new System.Drawing.Size(302, 20);
		this.MTKModel.TabIndex = 456;
		this.MTKModel.SelectedIndexChanged += new System.EventHandler(MTKModel_SelectedIndexChanged);
		this.MTKBrand.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKBrand.EditValue = text;
		this.MTKBrand.Location = new System.Drawing.Point(65, 269);
		this.MTKBrand.Name = name162;
		this.MTKBrand.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.MTKBrand.Size = new System.Drawing.Size(306, 20);
		this.MTKBrand.TabIndex = 455;
		this.MTKBrand.SelectedIndexChanged += new System.EventHandler(MTKBrand_SelectedIndexChanged);
		this.LMTKBrand.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LMTKBrand.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LMTKBrand.Appearance.Options.UseFont = true;
		this.LMTKBrand.Location = new System.Drawing.Point(17, 272);
		this.LMTKBrand.Name = name163;
		this.LMTKBrand.Size = new System.Drawing.Size(33, 15);
		this.LMTKBrand.TabIndex = 453;
		this.LMTKBrand.Text = text36;
		this.LMTKModel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.LMTKModel.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LMTKModel.Appearance.Options.UseFont = true;
		this.LMTKModel.Location = new System.Drawing.Point(387, 271);
		this.LMTKModel.Name = name164;
		this.LMTKModel.Size = new System.Drawing.Size(33, 15);
		this.LMTKModel.TabIndex = 454;
		this.LMTKModel.Text = text37;
		this.MTKReadback.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKReadback.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name165);
		this.MTKReadback.Location = new System.Drawing.Point(696, 491);
		this.MTKReadback.Name = name166;
		this.MTKReadback.Size = new System.Drawing.Size(110, 28);
		this.MTKReadback.TabIndex = 452;
		this.MTKReadback.Text = text72;
		this.MTKReadback.Click += new System.EventHandler(MTKHandler);
		this.MTKPatchVB.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKPatchVB.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name167);
		this.MTKPatchVB.Location = new System.Drawing.Point(584, 460);
		this.MTKPatchVB.Name = name168;
		this.MTKPatchVB.Size = new System.Drawing.Size(106, 28);
		this.MTKPatchVB.TabIndex = 449;
		this.MTKPatchVB.Text = text73;
		this.MTKPatchVB.Click += new System.EventHandler(MTKHandler);
		this.MTKOrangestate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKOrangestate.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name169);
		this.MTKOrangestate.Location = new System.Drawing.Point(584, 426);
		this.MTKOrangestate.Name = name170;
		this.MTKOrangestate.Size = new System.Drawing.Size(106, 28);
		this.MTKOrangestate.TabIndex = 448;
		this.MTKOrangestate.Text = text74;
		this.MTKOrangestate.Click += new System.EventHandler(MTKHandler);
		this.MTKDMVerity.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKDMVerity.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name171);
		this.MTKDMVerity.Location = new System.Drawing.Point(584, 491);
		this.MTKDMVerity.Name = name172;
		this.MTKDMVerity.Size = new System.Drawing.Size(106, 28);
		this.MTKDMVerity.TabIndex = 447;
		this.MTKDMVerity.Text = text75;
		this.MTKDMVerity.Click += new System.EventHandler(MTKHandler);
		this.MTKLBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKLBL.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name173);
		this.MTKLBL.Location = new System.Drawing.Point(492, 491);
		this.MTKLBL.Name = name174;
		this.MTKLBL.Size = new System.Drawing.Size(89, 28);
		this.MTKLBL.TabIndex = 445;
		this.MTKLBL.Text = text76;
		this.MTKLBL.Click += new System.EventHandler(MTKHandler);
		this.MTKUBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKUBL.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name175);
		this.MTKUBL.Location = new System.Drawing.Point(492, 457);
		this.MTKUBL.Name = name176;
		this.MTKUBL.Size = new System.Drawing.Size(89, 28);
		this.MTKUBL.TabIndex = 444;
		this.MTKUBL.Text = text77;
		this.MTKUBL.Click += new System.EventHandler(MTKHandler);
		this.MTKResetFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKResetFrp.ImageOptions.Image = AMFTool.Properties.Resources.FRP;
		this.MTKResetFrp.Location = new System.Drawing.Point(387, 459);
		this.MTKResetFrp.Name = name177;
		this.MTKResetFrp.Size = new System.Drawing.Size(99, 28);
		this.MTKResetFrp.TabIndex = 442;
		this.MTKResetFrp.Text = text22;
		this.MTKResetFrp.Click += new System.EventHandler(MTKHandler);
		this.MTKResetLock.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKResetLock.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.MTKResetLock.Location = new System.Drawing.Point(387, 426);
		this.MTKResetLock.Name = name178;
		this.MTKResetLock.Size = new System.Drawing.Size(100, 28);
		this.MTKResetLock.TabIndex = 441;
		this.MTKResetLock.Text = text23;
		this.MTKResetLock.Click += new System.EventHandler(MTKHandler);
		this.MTKSecPlugin.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKSecPlugin.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name179);
		this.MTKSecPlugin.Location = new System.Drawing.Point(584, 394);
		this.MTKSecPlugin.Name = name180;
		this.MTKSecPlugin.Size = new System.Drawing.Size(106, 28);
		this.MTKSecPlugin.TabIndex = 439;
		this.MTKSecPlugin.Text = text78;
		this.MTKSecPlugin.Click += new System.EventHandler(MTKHandler);
		this.MTKRPMBRestore.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKRPMBRestore.ImageOptions.Image = AMFTool.Properties.Resources.WRITE;
		this.MTKRPMBRestore.Location = new System.Drawing.Point(94, 491);
		this.MTKRPMBRestore.Name = name181;
		this.MTKRPMBRestore.Size = new System.Drawing.Size(97, 28);
		this.MTKRPMBRestore.TabIndex = 434;
		this.MTKRPMBRestore.Text = text79;
		this.MTKRPMBRestore.Click += new System.EventHandler(MTKHandler);
		this.MTKRPMBBackup.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKRPMBBackup.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.MTKRPMBBackup.Location = new System.Drawing.Point(94, 458);
		this.MTKRPMBBackup.Name = name182;
		this.MTKRPMBBackup.Size = new System.Drawing.Size(98, 28);
		this.MTKRPMBBackup.TabIndex = 433;
		this.MTKRPMBBackup.Text = text80;
		this.MTKRPMBBackup.Click += new System.EventHandler(MTKHandler);
		this.MTKRPMBErase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKRPMBErase.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.MTKRPMBErase.Location = new System.Drawing.Point(94, 426);
		this.MTKRPMBErase.Name = name183;
		this.MTKRPMBErase.Size = new System.Drawing.Size(98, 28);
		this.MTKRPMBErase.TabIndex = 432;
		this.MTKRPMBErase.Text = text81;
		this.MTKRPMBErase.Click += new System.EventHandler(MTKHandler);
		this.MTKNVRestore.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKNVRestore.ImageOptions.Image = AMFTool.Properties.Resources.WRITE;
		this.MTKNVRestore.Location = new System.Drawing.Point(3, 491);
		this.MTKNVRestore.Name = name184;
		this.MTKNVRestore.Size = new System.Drawing.Size(87, 28);
		this.MTKNVRestore.TabIndex = 431;
		this.MTKNVRestore.Text = text82;
		this.MTKNVRestore.Click += new System.EventHandler(MTKHandler);
		this.MTKNVBackup.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKNVBackup.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.MTKNVBackup.Location = new System.Drawing.Point(2, 458);
		this.MTKNVBackup.Name = name185;
		this.MTKNVBackup.Size = new System.Drawing.Size(88, 28);
		this.MTKNVBackup.TabIndex = 430;
		this.MTKNVBackup.Text = text83;
		this.MTKNVBackup.Click += new System.EventHandler(MTKHandler);
		this.MTKNVErase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKNVErase.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.MTKNVErase.Location = new System.Drawing.Point(3, 426);
		this.MTKNVErase.Name = name186;
		this.MTKNVErase.Size = new System.Drawing.Size(87, 28);
		this.MTKNVErase.TabIndex = 429;
		this.MTKNVErase.Text = text84;
		this.MTKNVErase.Click += new System.EventHandler(MTKHandler);
		this.CMTKDA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.CMTKDA.Location = new System.Drawing.Point(715, 335);
		this.CMTKDA.Name = name187;
		this.CMTKDA.Properties.AllowFocused = false;
		this.CMTKDA.Properties.Caption = caption5;
		this.CMTKDA.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CMTKDA.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CMTKDA.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CMTKDA.Size = new System.Drawing.Size(89, 22);
		this.CMTKDA.TabIndex = 428;
		this.CMTKDA.CheckedChanged += new System.EventHandler(CMTKDA_CheckedChanged);
		this.CMTKEMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.CMTKEMI.Location = new System.Drawing.Point(715, 301);
		this.CMTKEMI.Name = name188;
		this.CMTKEMI.Properties.AllowFocused = false;
		this.CMTKEMI.Properties.Caption = caption6;
		this.CMTKEMI.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CMTKEMI.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CMTKEMI.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CMTKEMI.Size = new System.Drawing.Size(89, 22);
		this.CMTKEMI.TabIndex = 427;
		this.CMTKEMI.CheckedChanged += new System.EventHandler(CMTKEMI_CheckedChanged);
		this.LMTKDA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LMTKDA.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LMTKDA.Appearance.Options.UseFont = true;
		this.LMTKDA.Location = new System.Drawing.Point(14, 337);
		this.LMTKDA.Name = name189;
		this.LMTKDA.Size = new System.Drawing.Size(84, 15);
		this.LMTKDA.TabIndex = 420;
		this.LMTKDA.Text = text85;
		this.MTKDA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKDA.Location = new System.Drawing.Point(104, 335);
		this.MTKDA.Name = name190;
		this.MTKDA.Properties.ContextImageOptions.Image = AMFTool.Properties.Resources.icons8_x_13px;
		this.MTKDA.Size = new System.Drawing.Size(605, 20);
		this.MTKDA.TabIndex = 419;
		this.LMTKEMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LMTKEMI.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.LMTKEMI.Appearance.Options.UseFont = true;
		this.LMTKEMI.Location = new System.Drawing.Point(15, 303);
		this.LMTKEMI.Name = name191;
		this.LMTKEMI.Size = new System.Drawing.Size(83, 15);
		this.LMTKEMI.TabIndex = 418;
		this.LMTKEMI.Text = text86;
		this.MTKEMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKEMI.Location = new System.Drawing.Point(106, 301);
		this.MTKEMI.Name = name192;
		this.MTKEMI.Properties.ContextImageOptions.Image = AMFTool.Properties.Resources.icons8_x_13px;
		this.MTKEMI.Size = new System.Drawing.Size(349, 20);
		this.MTKEMI.TabIndex = 417;
		this.LMTKFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LMTKFirmware.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LMTKFirmware.Appearance.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.LMTKFirmware.Appearance.Options.UseFont = true;
		this.LMTKFirmware.Appearance.Options.UseForeColor = true;
		this.LMTKFirmware.Location = new System.Drawing.Point(17, 369);
		this.LMTKFirmware.Name = name193;
		this.LMTKFirmware.Size = new System.Drawing.Size(54, 15);
		this.LMTKFirmware.TabIndex = 415;
		this.LMTKFirmware.Text = text34;
		this.MTKRepartition.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKRepartition.Location = new System.Drawing.Point(183, 234);
		this.MTKRepartition.Name = name194;
		this.MTKRepartition.Properties.AllowFocused = false;
		this.MTKRepartition.Properties.Caption = caption7;
		this.MTKRepartition.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.MTKRepartition.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.MTKRepartition.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.MTKRepartition.Size = new System.Drawing.Size(82, 22);
		this.MTKRepartition.TabIndex = 413;
		this.MTKRepartition.CheckedChanged += new System.EventHandler(MTKRepartition_CheckedChanged);
		this.Highspeed.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Highspeed.EditValue = true;
		this.Highspeed.Location = new System.Drawing.Point(271, 234);
		this.Highspeed.Name = name195;
		this.Highspeed.Properties.AllowFocused = false;
		this.Highspeed.Properties.Caption = caption8;
		this.Highspeed.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.Highspeed.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.Highspeed.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.Highspeed.Size = new System.Drawing.Size(86, 22);
		this.Highspeed.TabIndex = 411;
		this.Highspeed.CheckedChanged += new System.EventHandler(Highspeed_CheckedChanged);
		this.MTKFlash.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKFlash.ImageOptions.Image = AMFTool.Properties.Resources.flash;
		this.MTKFlash.Location = new System.Drawing.Point(619, 228);
		this.MTKFlash.Name = name196;
		this.MTKFlash.Size = new System.Drawing.Size(90, 28);
		this.MTKFlash.TabIndex = 410;
		this.MTKFlash.Text = text18;
		this.MTKFlash.Click += new System.EventHandler(MTKHandler);
		this.MTKReboot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKReboot.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name197);
		this.MTKReboot.Location = new System.Drawing.Point(523, 228);
		this.MTKReboot.Name = name198;
		this.MTKReboot.Size = new System.Drawing.Size(90, 28);
		this.MTKReboot.TabIndex = 409;
		this.MTKReboot.Text = text19;
		this.MTKReboot.Click += new System.EventHandler(MTKHandler);
		this.MTKReadGPT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKReadGPT.ImageOptions.Image = AMFTool.Properties.Resources.GPT_;
		this.MTKReadGPT.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
		this.MTKReadGPT.Location = new System.Drawing.Point(714, 228);
		this.MTKReadGPT.Name = name199;
		this.MTKReadGPT.Size = new System.Drawing.Size(90, 28);
		this.MTKReadGPT.TabIndex = 408;
		this.MTKReadGPT.Text = text20;
		this.MTKReadGPT.Click += new System.EventHandler(MTKHandler);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.panel5.Controls.Add(this.LayoutStore);
		this.panel5.Controls.Add(this.simpleButton17);
		this.panel5.Controls.Add(this.simpleButton14);
		this.panel5.Controls.Add(this.simpleButton15);
		this.panel5.Controls.Add(this.LabelControl10);
		this.panel5.Controls.Add(this.SuperMap);
		this.panel5.Controls.Add(this.labelControl6);
		this.panel5.Controls.Add(this.MTKFlashMode);
		this.panel5.Controls.Add(this.MTKLoadFirmware);
		this.panel5.Controls.Add(this.MTKFirmware);
		this.panel5.Controls.Add(this.MTKDataGrid);
		this.panel5.Controls.Add(this.MTKResetMICloud);
		this.panel5.Controls.Add(this.MTKDisableMiCloud);
		this.panel5.Controls.Add(this.MTKHuaweiID);
		this.panel5.Controls.Add(this.MTKIMEIRepair);
		this.panel5.Controls.Add(this.MTKPayjoy);
		this.panel5.Controls.Add(this.MTKOPPOID);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = name200;
		this.panel5.Size = new System.Drawing.Size(811, 526);
		this.panel5.TabIndex = 482;
		this.LayoutStore.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LayoutStore.EditValue = text87;
		this.LayoutStore.Location = new System.Drawing.Point(730, 368);
		this.LayoutStore.MenuManager = this.barManager1;
		this.LayoutStore.Name = name201;
		this.LayoutStore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.LayoutStore.Properties.Items.AddRange(new object[2] { text87, text33 });
		this.LayoutStore.Size = new System.Drawing.Size(72, 20);
		this.LayoutStore.TabIndex = 486;
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[14]
		{
			this.MTKWrite, this.MTKRead, this.MTKFormat, this.WriteCheck, this.FormatCheck, this.ReadPartition, this.EDLFormat, this.EDLWrite, this.EDLRead, this.FBWrite,
			this.FBFormat, this.SPDWrite, this.SPDFormat, this.SPDRead
		});
		this.barManager1.MaxItemId = 46;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Size = new System.Drawing.Size(1272, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 646);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Size = new System.Drawing.Size(1272, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 646);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(1272, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Size = new System.Drawing.Size(0, 646);
		this.MTKWrite.Caption = caption9;
		this.MTKWrite.Id = 32;
		this.MTKWrite.ImageOptions.Image = AMFTool.Properties.Resources.write2;
		this.MTKWrite.Name = name202;
		this.MTKWrite.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.MTKRead.Caption = caption10;
		this.MTKRead.Id = 33;
		this.MTKRead.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.MTKRead.Name = name203;
		this.MTKRead.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.MTKFormat.Caption = caption11;
		this.MTKFormat.Id = 34;
		this.MTKFormat.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.MTKFormat.Name = name204;
		this.MTKFormat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.WriteCheck.Caption = caption12;
		this.WriteCheck.Id = 35;
		this.WriteCheck.ImageOptions.Image = AMFTool.Properties.Resources.write2;
		this.WriteCheck.Name = name205;
		this.WriteCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.FormatCheck.Caption = caption13;
		this.FormatCheck.Id = 36;
		this.FormatCheck.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.FormatCheck.Name = name206;
		this.FormatCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.ReadPartition.Caption = caption14;
		this.ReadPartition.Id = 37;
		this.ReadPartition.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.ReadPartition.Name = name207;
		this.ReadPartition.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MTKOptions);
		this.EDLFormat.Caption = caption11;
		this.EDLFormat.Id = 38;
		this.EDLFormat.Name = name208;
		this.EDLFormat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(EDLOption);
		this.EDLWrite.Caption = caption9;
		this.EDLWrite.Id = 39;
		this.EDLWrite.Name = name209;
		this.EDLWrite.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(EDLOption);
		this.EDLRead.Caption = caption10;
		this.EDLRead.Id = 40;
		this.EDLRead.Name = name210;
		this.EDLRead.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(EDLOption);
		this.FBWrite.Caption = caption9;
		this.FBWrite.Id = 41;
		this.FBWrite.Name = name211;
		this.FBWrite.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FastbootOption);
		this.FBFormat.Caption = caption11;
		this.FBFormat.Id = 42;
		this.FBFormat.Name = name212;
		this.FBFormat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FastbootOption);
		this.SPDWrite.Caption = caption9;
		this.SPDWrite.Id = 43;
		this.SPDWrite.Name = name213;
		this.SPDWrite.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SPDOption);
		this.SPDFormat.Caption = caption11;
		this.SPDFormat.Id = 44;
		this.SPDFormat.Name = name214;
		this.SPDFormat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SPDOption);
		this.SPDRead.Caption = caption10;
		this.SPDRead.Id = 45;
		this.SPDRead.Name = name215;
		this.SPDRead.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SPDOption);
		this.simpleButton17.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.simpleButton17.Enabled = false;
		this.simpleButton17.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name216);
		this.simpleButton17.Location = new System.Drawing.Point(3, 394);
		this.simpleButton17.Name = name217;
		this.simpleButton17.Size = new System.Drawing.Size(87, 28);
		this.simpleButton17.TabIndex = 479;
		this.simpleButton17.Text = text88;
		this.simpleButton14.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.simpleButton14.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name218);
		this.simpleButton14.Location = new System.Drawing.Point(293, 394);
		this.simpleButton14.Name = name219;
		this.simpleButton14.Size = new System.Drawing.Size(91, 28);
		this.simpleButton14.TabIndex = 476;
		this.simpleButton14.Text = text89;
		this.simpleButton15.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.simpleButton15.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name220);
		this.simpleButton15.Location = new System.Drawing.Point(197, 394);
		this.simpleButton15.Name = name221;
		this.simpleButton15.Size = new System.Drawing.Size(90, 28);
		this.simpleButton15.TabIndex = 477;
		this.simpleButton15.Text = text90;
		this.LabelControl10.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LabelControl10.Location = new System.Drawing.Point(688, 370);
		this.LabelControl10.Name = name222;
		this.LabelControl10.Size = new System.Drawing.Size(33, 13);
		this.LabelControl10.TabIndex = 485;
		this.LabelControl10.Text = text91;
		this.SuperMap.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.SuperMap.EditValue = text92;
		this.SuperMap.Location = new System.Drawing.Point(594, 368);
		this.SuperMap.MenuManager = this.barManager1;
		this.SuperMap.Name = name223;
		this.SuperMap.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.SuperMap.Properties.Items.AddRange(new object[2] { text92, text93 });
		this.SuperMap.Size = new System.Drawing.Size(88, 20);
		this.SuperMap.TabIndex = 484;
		this.labelControl6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.labelControl6.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.labelControl6.Appearance.Options.UseFont = true;
		this.labelControl6.Location = new System.Drawing.Point(525, 369);
		this.labelControl6.Name = name224;
		this.labelControl6.Size = new System.Drawing.Size(59, 15);
		this.labelControl6.TabIndex = 483;
		this.labelControl6.Text = text94;
		this.MTKFlashMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKFlashMode.EditValue = text95;
		this.MTKFlashMode.Location = new System.Drawing.Point(87, 366);
		this.MTKFlashMode.Name = name225;
		this.MTKFlashMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.MTKFlashMode.Properties.Items.AddRange(new object[3] { text95, text96, text97 });
		this.MTKFlashMode.Size = new System.Drawing.Size(104, 20);
		this.MTKFlashMode.TabIndex = 426;
		this.MTKFlashMode.SelectedIndexChanged += new System.EventHandler(MTKFlashMode_SelectedIndexChanged);
		this.MTKLoadFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.MTKLoadFirmware.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKLoadFirmware.FlatAppearance.BorderSize = 0;
		this.MTKLoadFirmware.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.MTKLoadFirmware.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.MTKLoadFirmware.Location = new System.Drawing.Point(497, 369);
		this.MTKLoadFirmware.Name = name226;
		this.MTKLoadFirmware.Size = new System.Drawing.Size(17, 13);
		this.MTKLoadFirmware.TabIndex = 467;
		this.MTKLoadFirmware.UseVisualStyleBackColor = false;
		this.MTKLoadFirmware.Click += new System.EventHandler(MTKLoad);
		this.MTKFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKFirmware.Location = new System.Drawing.Point(198, 366);
		this.MTKFirmware.Name = name227;
		this.MTKFirmware.Size = new System.Drawing.Size(322, 20);
		this.MTKFirmware.TabIndex = 416;
		this.MTKDataGrid.AllowDrop = true;
		this.MTKDataGrid.AllowUserToAddRows = false;
		this.MTKDataGrid.AllowUserToDeleteRows = false;
		this.MTKDataGrid.AllowUserToResizeColumns = false;
		this.MTKDataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle13.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.DimGray;
		this.MTKDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
		this.MTKDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTKDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.MTKDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
		this.MTKDataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle14.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.MTKDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
		this.MTKDataGrid.ColumnHeadersHeight = 17;
		this.MTKDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.MTKDataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn8, this.dataGridViewTextBoxColumn36, this.dataGridViewTextBoxColumn37, this.Column7, this.dataGridViewTextBoxColumn38, this.dataGridViewTextBoxColumn40, this.dataGridViewTextBoxColumn69);
		dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle15.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.MTKDataGrid.DefaultCellStyle = dataGridViewCellStyle15;
		this.MTKDataGrid.EnableHeadersVisualStyles = false;
		this.MTKDataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.MTKDataGrid.Location = new System.Drawing.Point(2, 1);
		this.MTKDataGrid.Name = name228;
		this.MTKDataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle16.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle16.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.MTKDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
		this.MTKDataGrid.RowHeadersVisible = false;
		this.MTKDataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle17.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.MTKDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle17;
		this.MTKDataGrid.RowTemplate.Height = 20;
		this.MTKDataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.MTKDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.MTKDataGrid.Size = new System.Drawing.Size(807, 221);
		this.MTKDataGrid.StandardTab = true;
		this.MTKDataGrid.TabIndex = 461;
		this.MTKDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(MTKDataGrid_CellContentDoubleClick);
		this.MTKDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(MTKDataGrid_CellDoubleClick);
		this.MTKDataGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(MTKDataGrid_CellMouseClick);
		this.MTKDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(MTKDataGrid_CellMouseDown_1);
		dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle18.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle18.NullValue = false;
		dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn8.DefaultCellStyle = dataGridViewCellStyle18;
		this.dataGridViewCheckBoxColumn8.Frozen = true;
		this.dataGridViewCheckBoxColumn8.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn8.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn8.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn8.Name = name229;
		this.dataGridViewCheckBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn8.Width = 75;
		dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle19.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle19.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn36.DefaultCellStyle = dataGridViewCellStyle19;
		this.dataGridViewTextBoxColumn36.Frozen = true;
		this.dataGridViewTextBoxColumn36.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn36.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn36.Name = name230;
		this.dataGridViewTextBoxColumn36.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn36.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn36.Width = 150;
		this.dataGridViewTextBoxColumn37.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle20.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle20.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn37.DefaultCellStyle = dataGridViewCellStyle20;
		this.dataGridViewTextBoxColumn37.HeaderText = headerText16;
		this.dataGridViewTextBoxColumn37.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn37.Name = name231;
		this.dataGridViewTextBoxColumn37.ReadOnly = true;
		this.dataGridViewTextBoxColumn37.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn37.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle21.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle21.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.Column7.DefaultCellStyle = dataGridViewCellStyle21;
		this.Column7.HeaderText = headerText12;
		this.Column7.MinimumWidth = 4;
		this.Column7.Name = name232;
		this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Column7.Width = 80;
		dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle22.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle22.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn38.DefaultCellStyle = dataGridViewCellStyle22;
		this.dataGridViewTextBoxColumn38.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn38.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn38.Name = name233;
		this.dataGridViewTextBoxColumn38.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn38.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn38.Width = 85;
		dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle23.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle23.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle23.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn40.DefaultCellStyle = dataGridViewCellStyle23;
		this.dataGridViewTextBoxColumn40.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn40.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn40.Name = name234;
		this.dataGridViewTextBoxColumn40.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn40.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn40.Width = 85;
		dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle24.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle24.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn69.DefaultCellStyle = dataGridViewCellStyle24;
		this.dataGridViewTextBoxColumn69.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn69.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn69.Name = name235;
		this.dataGridViewTextBoxColumn69.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn69.Visible = false;
		this.MTKResetMICloud.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKResetMICloud.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name236);
		this.MTKResetMICloud.Location = new System.Drawing.Point(198, 458);
		this.MTKResetMICloud.Name = name237;
		this.MTKResetMICloud.Size = new System.Drawing.Size(89, 28);
		this.MTKResetMICloud.TabIndex = 436;
		this.MTKResetMICloud.Text = text98;
		this.MTKResetMICloud.Click += new System.EventHandler(MTKHandler);
		this.MTKDisableMiCloud.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKDisableMiCloud.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name238);
		this.MTKDisableMiCloud.Location = new System.Drawing.Point(197, 426);
		this.MTKDisableMiCloud.Name = name239;
		this.MTKDisableMiCloud.Size = new System.Drawing.Size(90, 28);
		this.MTKDisableMiCloud.TabIndex = 435;
		this.MTKDisableMiCloud.Text = text99;
		this.MTKDisableMiCloud.Click += new System.EventHandler(MTKHandler);
		this.MTKHuaweiID.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKHuaweiID.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name240);
		this.MTKHuaweiID.Location = new System.Drawing.Point(196, 491);
		this.MTKHuaweiID.Name = name241;
		this.MTKHuaweiID.Size = new System.Drawing.Size(91, 28);
		this.MTKHuaweiID.TabIndex = 438;
		this.MTKHuaweiID.Text = text100;
		this.MTKHuaweiID.Click += new System.EventHandler(MTKHandler);
		this.MTKIMEIRepair.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKIMEIRepair.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name242);
		this.MTKIMEIRepair.Location = new System.Drawing.Point(293, 426);
		this.MTKIMEIRepair.Name = name243;
		this.MTKIMEIRepair.Size = new System.Drawing.Size(91, 28);
		this.MTKIMEIRepair.TabIndex = 443;
		this.MTKIMEIRepair.Text = text101;
		this.MTKIMEIRepair.Click += new System.EventHandler(MTKHandler);
		this.MTKPayjoy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKPayjoy.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name244);
		this.MTKPayjoy.Location = new System.Drawing.Point(293, 458);
		this.MTKPayjoy.Name = name245;
		this.MTKPayjoy.Size = new System.Drawing.Size(91, 28);
		this.MTKPayjoy.TabIndex = 440;
		this.MTKPayjoy.Text = text102;
		this.MTKPayjoy.Click += new System.EventHandler(MTKHandler);
		this.MTKOPPOID.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.MTKOPPOID.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name246);
		this.MTKOPPOID.Location = new System.Drawing.Point(293, 491);
		this.MTKOPPOID.Name = name247;
		this.MTKOPPOID.Size = new System.Drawing.Size(90, 28);
		this.MTKOPPOID.TabIndex = 437;
		this.MTKOPPOID.Text = text17;
		this.MTKOPPOID.Click += new System.EventHandler(MTKHandler);
		this.TabSpd.Controls.Add(this.TabSubSpreadtrum);
		this.TabSpd.Name = name248;
		this.TabSpd.Size = new System.Drawing.Size(813, 557);
		this.TabSpd.Text = text4;
		this.TabSubSpreadtrum.Appearance.BackColor = System.Drawing.Color.White;
		this.TabSubSpreadtrum.Appearance.Options.UseBackColor = true;
		this.TabSubSpreadtrum.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabSubSpreadtrum.Location = new System.Drawing.Point(0, 0);
		this.TabSubSpreadtrum.LookAndFeel.SkinName = skinName;
		this.TabSubSpreadtrum.LookAndFeel.UseDefaultLookAndFeel = false;
		this.TabSubSpreadtrum.Name = name249;
		this.TabSubSpreadtrum.SelectedTabPage = this.TabSpdFlash;
		this.TabSubSpreadtrum.Size = new System.Drawing.Size(813, 557);
		this.TabSubSpreadtrum.TabIndex = 0;
		this.TabSubSpreadtrum.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[2] { this.TabSpdFlash, this.SPDATdiag });
		this.TabSpdFlash.Controls.Add(this.CSPDPart);
		this.TabSpdFlash.Controls.Add(this.SPDDataGrid);
		this.TabSpdFlash.Controls.Add(this.checkEdit2);
		this.TabSpdFlash.Controls.Add(this.Skippacth);
		this.TabSpdFlash.Controls.Add(this.labelControl3);
		this.TabSpdFlash.Controls.Add(this.CModelSPD);
		this.TabSpdFlash.Controls.Add(this.SPDResetFrp);
		this.TabSpdFlash.Controls.Add(this.SPDResetLock);
		this.TabSpdFlash.Controls.Add(this.LSPDCPU);
		this.TabSpdFlash.Controls.Add(this.CBrandSPD);
		this.TabSpdFlash.Controls.Add(this.SPDReboot);
		this.TabSpdFlash.Controls.Add(this.SPDReadGPT);
		this.TabSpdFlash.Name = name250;
		this.TabSpdFlash.Size = new System.Drawing.Size(811, 526);
		this.TabSpdFlash.Text = text18;
		this.CSPDPart.AutoSize = true;
		this.CSPDPart.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CSPDPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CSPDPart.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CSPDPart.Location = new System.Drawing.Point(7, 7);
		this.CSPDPart.Name = name251;
		this.CSPDPart.Size = new System.Drawing.Size(12, 11);
		this.CSPDPart.TabIndex = 508;
		this.CSPDPart.UseVisualStyleBackColor = false;
		this.CSPDPart.CheckedChanged += new System.EventHandler(CSPDPart_CheckedChanged);
		this.SPDDataGrid.AllowDrop = true;
		this.SPDDataGrid.AllowUserToAddRows = false;
		this.SPDDataGrid.AllowUserToDeleteRows = false;
		this.SPDDataGrid.AllowUserToResizeColumns = false;
		this.SPDDataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle25.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle25.SelectionForeColor = System.Drawing.Color.DimGray;
		this.SPDDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle25;
		this.SPDDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SPDDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.SPDDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.SPDDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
		this.SPDDataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle26.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle26.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle26.SelectionForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.SPDDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
		this.SPDDataGrid.ColumnHeadersHeight = 17;
		this.SPDDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.SPDDataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn12, this.dataGridViewTextBoxColumn35, this.dataGridViewTextBoxColumn62, this.dataGridViewTextBoxColumn63, this.dataGridViewTextBoxColumn64, this.dataGridViewTextBoxColumn65, this.dataGridViewTextBoxColumn66);
		dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle27.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle27.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle27.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.SPDDataGrid.DefaultCellStyle = dataGridViewCellStyle27;
		this.SPDDataGrid.EnableHeadersVisualStyles = false;
		this.SPDDataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.SPDDataGrid.Location = new System.Drawing.Point(3, 3);
		this.SPDDataGrid.Name = name252;
		this.SPDDataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle28.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle28.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle28.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle28.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.SPDDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle28;
		this.SPDDataGrid.RowHeadersVisible = false;
		this.SPDDataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle29.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle29.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle29.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.SPDDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle29;
		this.SPDDataGrid.RowTemplate.Height = 20;
		this.SPDDataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.SPDDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.SPDDataGrid.Size = new System.Drawing.Size(803, 386);
		this.SPDDataGrid.StandardTab = true;
		this.SPDDataGrid.TabIndex = 507;
		this.SPDDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(SPDDataGrid_CellContentClick);
		this.SPDDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(SPDDataGrid_CellMouseDown);
		dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle30.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle30.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle30.NullValue = false;
		dataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle30.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn12.DefaultCellStyle = dataGridViewCellStyle30;
		this.dataGridViewCheckBoxColumn12.Frozen = true;
		this.dataGridViewCheckBoxColumn12.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn12.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn12.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn12.Name = name253;
		this.dataGridViewCheckBoxColumn12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn12.Width = 75;
		dataGridViewCellStyle31.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle31.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle31.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle31.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle31.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn35.DefaultCellStyle = dataGridViewCellStyle31;
		this.dataGridViewTextBoxColumn35.Frozen = true;
		this.dataGridViewTextBoxColumn35.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn35.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn35.Name = name254;
		this.dataGridViewTextBoxColumn35.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn35.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn35.Width = 150;
		this.dataGridViewTextBoxColumn62.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle32.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle32.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle32.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle32.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle32.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn62.DefaultCellStyle = dataGridViewCellStyle32;
		this.dataGridViewTextBoxColumn62.HeaderText = headerText16;
		this.dataGridViewTextBoxColumn62.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn62.Name = name255;
		this.dataGridViewTextBoxColumn62.ReadOnly = true;
		this.dataGridViewTextBoxColumn62.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn62.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle33.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle33.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle33.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn63.DefaultCellStyle = dataGridViewCellStyle33;
		this.dataGridViewTextBoxColumn63.HeaderText = headerText12;
		this.dataGridViewTextBoxColumn63.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn63.Name = name256;
		this.dataGridViewTextBoxColumn63.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn63.Width = 80;
		dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle34.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle34.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle34.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn64.DefaultCellStyle = dataGridViewCellStyle34;
		this.dataGridViewTextBoxColumn64.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn64.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn64.Name = name257;
		this.dataGridViewTextBoxColumn64.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn64.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn64.Width = 85;
		dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle35.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle35.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle35.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn65.DefaultCellStyle = dataGridViewCellStyle35;
		this.dataGridViewTextBoxColumn65.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn65.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn65.Name = name258;
		this.dataGridViewTextBoxColumn65.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn65.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn65.Width = 85;
		dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle36.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle36.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle36.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn66.DefaultCellStyle = dataGridViewCellStyle36;
		this.dataGridViewTextBoxColumn66.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn66.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn66.Name = name259;
		this.dataGridViewTextBoxColumn66.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn66.Visible = false;
		this.checkEdit2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.checkEdit2.EditValue = true;
		this.checkEdit2.Location = new System.Drawing.Point(5, 397);
		this.checkEdit2.Name = name260;
		this.checkEdit2.Properties.AllowFocused = false;
		this.checkEdit2.Properties.Caption = caption15;
		this.checkEdit2.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.checkEdit2.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.checkEdit2.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.checkEdit2.Size = new System.Drawing.Size(94, 22);
		this.checkEdit2.TabIndex = 506;
		this.Skippacth.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Skippacth.Location = new System.Drawing.Point(99, 397);
		this.Skippacth.Name = name261;
		this.Skippacth.Properties.AllowFocused = false;
		this.Skippacth.Properties.Caption = caption16;
		this.Skippacth.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.Skippacth.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.Skippacth.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.Skippacth.Size = new System.Drawing.Size(78, 22);
		this.Skippacth.TabIndex = 505;
		this.Skippacth.CheckedChanged += new System.EventHandler(Skippacth_CheckedChanged);
		this.labelControl3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.labelControl3.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl3.Appearance.Options.UseFont = true;
		this.labelControl3.Location = new System.Drawing.Point(285, 432);
		this.labelControl3.Name = name262;
		this.labelControl3.Size = new System.Drawing.Size(33, 15);
		this.labelControl3.TabIndex = 504;
		this.labelControl3.Text = text36;
		this.CModelSPD.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CModelSPD.EditValue = text;
		this.CModelSPD.Location = new System.Drawing.Point(329, 430);
		this.CModelSPD.Name = name263;
		this.CModelSPD.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.CModelSPD.Properties.Items.AddRange(new object[10] { text103, text104, text105, text106, text107, text108, text109, text110, text111, text112 });
		this.CModelSPD.Size = new System.Drawing.Size(473, 20);
		this.CModelSPD.TabIndex = 503;
		this.CModelSPD.SelectedIndexChanged += new System.EventHandler(CModelSPD_SelectedIndexChanged);
		this.SPDResetFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SPDResetFrp.Location = new System.Drawing.Point(141, 472);
		this.SPDResetFrp.Name = name264;
		this.SPDResetFrp.Size = new System.Drawing.Size(78, 28);
		this.SPDResetFrp.TabIndex = 501;
		this.SPDResetFrp.Text = text22;
		this.SPDResetFrp.Click += new System.EventHandler(SpreadtrumHandler);
		this.SPDResetLock.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SPDResetLock.Location = new System.Drawing.Point(30, 472);
		this.SPDResetLock.Name = name265;
		this.SPDResetLock.Size = new System.Drawing.Size(78, 28);
		this.SPDResetLock.TabIndex = 499;
		this.SPDResetLock.Text = text113;
		this.SPDResetLock.Click += new System.EventHandler(SpreadtrumHandler);
		this.LSPDCPU.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LSPDCPU.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LSPDCPU.Appearance.Options.UseFont = true;
		this.LSPDCPU.Location = new System.Drawing.Point(8, 431);
		this.LSPDCPU.Name = name266;
		this.LSPDCPU.Size = new System.Drawing.Size(33, 15);
		this.LSPDCPU.TabIndex = 498;
		this.LSPDCPU.Text = text36;
		this.CBrandSPD.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.CBrandSPD.EditValue = editValue2;
		this.CBrandSPD.Location = new System.Drawing.Point(47, 429);
		this.CBrandSPD.Name = name267;
		this.CBrandSPD.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.CBrandSPD.Properties.Items.AddRange(new object[11]
		{
			text114, text56, text115, text57, text116, text60, text117, text59, text118, text119,
			text120
		});
		this.CBrandSPD.Size = new System.Drawing.Size(227, 20);
		this.CBrandSPD.TabIndex = 497;
		this.CBrandSPD.SelectedIndexChanged += new System.EventHandler(CBrandSPD_SelectedIndexChanged);
		this.SPDReboot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.SPDReboot.Location = new System.Drawing.Point(643, 396);
		this.SPDReboot.Name = name268;
		this.SPDReboot.Size = new System.Drawing.Size(78, 28);
		this.SPDReboot.TabIndex = 494;
		this.SPDReboot.Text = text19;
		this.SPDReboot.Click += new System.EventHandler(SpreadtrumHandler);
		this.SPDReadGPT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.SPDReadGPT.Location = new System.Drawing.Point(724, 396);
		this.SPDReadGPT.Name = name269;
		this.SPDReadGPT.Size = new System.Drawing.Size(78, 28);
		this.SPDReadGPT.TabIndex = 493;
		this.SPDReadGPT.Text = text20;
		this.SPDReadGPT.Click += new System.EventHandler(SpreadtrumHandler);
		this.SPDATdiag.Controls.Add(this.groupControl3);
		this.SPDATdiag.Name = name270;
		this.SPDATdiag.Size = new System.Drawing.Size(811, 526);
		this.SPDATdiag.Text = text121;
		this.groupControl3.Controls.Add(this.FactoryReset2);
		this.groupControl3.Controls.Add(this.FactoryReset);
		this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupControl3.Location = new System.Drawing.Point(0, 0);
		this.groupControl3.Name = name271;
		this.groupControl3.Size = new System.Drawing.Size(811, 526);
		this.groupControl3.TabIndex = 2;
		this.groupControl3.Text = text122;
		this.FactoryReset2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FactoryReset2.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.FactoryReset2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.FactoryReset2.Location = new System.Drawing.Point(16, 72);
		this.FactoryReset2.Name = name272;
		this.FactoryReset2.Size = new System.Drawing.Size(770, 28);
		this.FactoryReset2.TabIndex = 425;
		this.FactoryReset2.Text = text123;
		this.FactoryReset2.Click += new System.EventHandler(SpreadtrumATDiag);
		this.FactoryReset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FactoryReset.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
		this.FactoryReset.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.FactoryReset.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.FactoryReset.Location = new System.Drawing.Point(16, 38);
		this.FactoryReset.Name = name273;
		this.FactoryReset.Size = new System.Drawing.Size(770, 28);
		this.FactoryReset.TabIndex = 424;
		this.FactoryReset.Text = text124;
		this.FactoryReset.Click += new System.EventHandler(SpreadtrumATDiag);
		this.TabHisi.Controls.Add(this.HISIBLUnlock);
		this.TabHisi.Controls.Add(this.HISIResetFrp);
		this.TabHisi.Controls.Add(this.HISIResetlock);
		this.TabHisi.Controls.Add(this.HISIResetID);
		this.TabHisi.Controls.Add(this.CCPUHisi);
		this.TabHisi.Controls.Add(this.LHisiCPU);
		this.TabHisi.Name = name274;
		this.TabHisi.Size = new System.Drawing.Size(813, 557);
		this.TabHisi.Text = text125;
		this.HISIBLUnlock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HISIBLUnlock.Location = new System.Drawing.Point(27, 186);
		this.HISIBLUnlock.Name = name275;
		this.HISIBLUnlock.Size = new System.Drawing.Size(749, 28);
		this.HISIBLUnlock.TabIndex = 470;
		this.HISIBLUnlock.Text = text77;
		this.HISIBLUnlock.Click += new System.EventHandler(HisiHandler);
		this.HISIResetFrp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HISIResetFrp.Location = new System.Drawing.Point(27, 118);
		this.HISIResetFrp.Name = name276;
		this.HISIResetFrp.Size = new System.Drawing.Size(749, 28);
		this.HISIResetFrp.TabIndex = 466;
		this.HISIResetFrp.Text = text22;
		this.HISIResetFrp.Click += new System.EventHandler(HisiHandler);
		this.HISIResetlock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HISIResetlock.Location = new System.Drawing.Point(27, 152);
		this.HISIResetlock.Name = name277;
		this.HISIResetlock.Size = new System.Drawing.Size(749, 28);
		this.HISIResetlock.TabIndex = 464;
		this.HISIResetlock.Text = text113;
		this.HISIResetlock.Click += new System.EventHandler(HisiHandler);
		this.HISIResetID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HISIResetID.Location = new System.Drawing.Point(27, 84);
		this.HISIResetID.Name = name278;
		this.HISIResetID.Size = new System.Drawing.Size(749, 28);
		this.HISIResetID.TabIndex = 463;
		this.HISIResetID.Text = text100;
		this.HISIResetID.Click += new System.EventHandler(HisiHandler);
		this.CCPUHisi.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CCPUHisi.EditValue = editValue3;
		this.CCPUHisi.Location = new System.Drawing.Point(105, 43);
		this.CCPUHisi.Name = name279;
		this.CCPUHisi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.CCPUHisi.Properties.Items.AddRange(new object[12]
		{
			text126, text127, text128, text129, text130, text131, text132, text133, text134, text135,
			text136, text137
		});
		this.CCPUHisi.Size = new System.Drawing.Size(169, 20);
		this.CCPUHisi.TabIndex = 459;
		this.CCPUHisi.SelectedIndexChanged += new System.EventHandler(CMTKEMI_CheckedChanged);
		this.LHisiCPU.Appearance.Font = new System.Drawing.Font(familyName4, field_19, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LHisiCPU.Appearance.Options.UseFont = true;
		this.LHisiCPU.Location = new System.Drawing.Point(29, 45);
		this.LHisiCPU.Name = name280;
		this.LHisiCPU.Size = new System.Drawing.Size(54, 15);
		this.LHisiCPU.TabIndex = 458;
		this.LHisiCPU.Text = text138;
		this.TabMeta.Controls.Add(this.xtraTabControl2);
		this.TabMeta.Name = name281;
		this.TabMeta.Size = new System.Drawing.Size(813, 557);
		this.TabMeta.Text = text139;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.LookAndFeel.SkinName = skinName;
		this.xtraTabControl2.LookAndFeel.UseDefaultLookAndFeel = false;
		this.xtraTabControl2.Name = name282;
		this.xtraTabControl2.SelectedTabPage = this.Transsion;
		this.xtraTabControl2.Size = new System.Drawing.Size(813, 557);
		this.xtraTabControl2.TabIndex = 465;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[2] { this.Transsion, this.Andproid });
		this.Transsion.Controls.Add(this.METAWriteIMEI);
		this.Transsion.Controls.Add(this.METASecplugin);
		this.Transsion.Controls.Add(this.METAReboot);
		this.Transsion.Controls.Add(this.CheckMeta);
		this.Transsion.Controls.Add(this.METAReset);
		this.Transsion.Controls.Add(this.METAEnableADB);
		this.Transsion.Controls.Add(this.METADisableADB);
		this.Transsion.Controls.Add(this.METAPayjoy);
		this.Transsion.Controls.Add(this.METAFrp);
		this.Transsion.Controls.Add(this.METAReadGPT);
		this.Transsion.Controls.Add(this.METADataGrid);
		this.Transsion.Name = name283;
		this.Transsion.Size = new System.Drawing.Size(811, 526);
		this.Transsion.Text = text140;
		this.METAWriteIMEI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METAWriteIMEI.Location = new System.Drawing.Point(263, 350);
		this.METAWriteIMEI.Name = name284;
		this.METAWriteIMEI.Size = new System.Drawing.Size(84, 28);
		this.METAWriteIMEI.TabIndex = 441;
		this.METAWriteIMEI.Text = text141;
		this.METAWriteIMEI.Click += new System.EventHandler(METAHandler);
		this.METASecplugin.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METASecplugin.Location = new System.Drawing.Point(83, 384);
		this.METASecplugin.Name = name285;
		this.METASecplugin.Size = new System.Drawing.Size(84, 28);
		this.METASecplugin.TabIndex = 437;
		this.METASecplugin.Text = text78;
		this.METASecplugin.Click += new System.EventHandler(METAHandler);
		this.METAReboot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.METAReboot.Location = new System.Drawing.Point(649, 350);
		this.METAReboot.Name = name286;
		this.METAReboot.Size = new System.Drawing.Size(75, 28);
		this.METAReboot.TabIndex = 432;
		this.METAReboot.Text = text19;
		this.METAReboot.Click += new System.EventHandler(METAHandler);
		this.CheckMeta.AutoSize = true;
		this.CheckMeta.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CheckMeta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CheckMeta.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CheckMeta.Location = new System.Drawing.Point(10, 7);
		this.CheckMeta.Name = name287;
		this.CheckMeta.Size = new System.Drawing.Size(12, 11);
		this.CheckMeta.TabIndex = 464;
		this.CheckMeta.UseVisualStyleBackColor = false;
		this.CheckMeta.CheckedChanged += new System.EventHandler(CheckMeta_CheckedChanged);
		this.METAReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METAReset.Location = new System.Drawing.Point(2, 384);
		this.METAReset.Name = name288;
		this.METAReset.Size = new System.Drawing.Size(75, 28);
		this.METAReset.TabIndex = 433;
		this.METAReset.Text = text113;
		this.METAReset.Click += new System.EventHandler(METAHandler);
		this.METAEnableADB.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METAEnableADB.Location = new System.Drawing.Point(173, 350);
		this.METAEnableADB.Name = name289;
		this.METAEnableADB.Size = new System.Drawing.Size(84, 28);
		this.METAEnableADB.TabIndex = 439;
		this.METAEnableADB.Text = text142;
		this.METAEnableADB.Click += new System.EventHandler(METAHandler);
		this.METADisableADB.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METADisableADB.Location = new System.Drawing.Point(173, 384);
		this.METADisableADB.Name = name290;
		this.METADisableADB.Size = new System.Drawing.Size(84, 28);
		this.METADisableADB.TabIndex = 440;
		this.METADisableADB.Text = text143;
		this.METADisableADB.Click += new System.EventHandler(METAHandler);
		this.METAPayjoy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METAPayjoy.Location = new System.Drawing.Point(83, 350);
		this.METAPayjoy.Name = name291;
		this.METAPayjoy.Size = new System.Drawing.Size(84, 28);
		this.METAPayjoy.TabIndex = 435;
		this.METAPayjoy.Text = text102;
		this.METAPayjoy.Click += new System.EventHandler(METAHandler);
		this.METAFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.METAFrp.Location = new System.Drawing.Point(2, 350);
		this.METAFrp.Name = name292;
		this.METAFrp.Size = new System.Drawing.Size(75, 28);
		this.METAFrp.TabIndex = 434;
		this.METAFrp.Text = text22;
		this.METAFrp.Click += new System.EventHandler(METAHandler);
		this.METAReadGPT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.METAReadGPT.Location = new System.Drawing.Point(730, 350);
		this.METAReadGPT.Name = name293;
		this.METAReadGPT.Size = new System.Drawing.Size(75, 28);
		this.METAReadGPT.TabIndex = 431;
		this.METAReadGPT.Text = text20;
		this.METAReadGPT.Click += new System.EventHandler(METAHandler);
		this.METADataGrid.AllowDrop = true;
		this.METADataGrid.AllowUserToAddRows = false;
		this.METADataGrid.AllowUserToDeleteRows = false;
		this.METADataGrid.AllowUserToResizeColumns = false;
		this.METADataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle37.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle37.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle37.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle37.SelectionForeColor = System.Drawing.Color.DimGray;
		this.METADataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle37;
		this.METADataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.METADataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.METADataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.METADataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
		this.METADataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle38.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle38.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle38.SelectionForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.METADataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle38;
		this.METADataGrid.ColumnHeadersHeight = 17;
		this.METADataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.METADataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn10, this.dataGridViewTextBoxColumn49, this.dataGridViewTextBoxColumn50, this.dataGridViewTextBoxColumn51, this.dataGridViewTextBoxColumn52, this.dataGridViewTextBoxColumn53, this.dataGridViewTextBoxColumn54);
		dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle39.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle39.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle39.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle39.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		dataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.METADataGrid.DefaultCellStyle = dataGridViewCellStyle39;
		this.METADataGrid.EnableHeadersVisualStyles = false;
		this.METADataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.METADataGrid.Location = new System.Drawing.Point(3, 3);
		this.METADataGrid.Name = name294;
		this.METADataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle40.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle40.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle40.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle40.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.METADataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle40;
		this.METADataGrid.RowHeadersVisible = false;
		this.METADataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle41.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle41.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle41.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle41.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.METADataGrid.RowsDefaultCellStyle = dataGridViewCellStyle41;
		this.METADataGrid.RowTemplate.Height = 20;
		this.METADataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.METADataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.METADataGrid.Size = new System.Drawing.Size(802, 341);
		this.METADataGrid.StandardTab = true;
		this.METADataGrid.TabIndex = 463;
		dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle42.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle42.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle42.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle42.NullValue = false;
		dataGridViewCellStyle42.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle42.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn10.DefaultCellStyle = dataGridViewCellStyle42;
		this.dataGridViewCheckBoxColumn10.Frozen = true;
		this.dataGridViewCheckBoxColumn10.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn10.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn10.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn10.Name = name295;
		this.dataGridViewCheckBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn10.Width = 75;
		dataGridViewCellStyle43.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle43.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle43.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle43.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle43.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn49.DefaultCellStyle = dataGridViewCellStyle43;
		this.dataGridViewTextBoxColumn49.Frozen = true;
		this.dataGridViewTextBoxColumn49.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn49.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn49.Name = name296;
		this.dataGridViewTextBoxColumn49.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn49.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn49.Width = 150;
		this.dataGridViewTextBoxColumn50.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle44.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle44.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle44.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle44.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle44.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn50.DefaultCellStyle = dataGridViewCellStyle44;
		this.dataGridViewTextBoxColumn50.HeaderText = headerText16;
		this.dataGridViewTextBoxColumn50.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn50.Name = name297;
		this.dataGridViewTextBoxColumn50.ReadOnly = true;
		this.dataGridViewTextBoxColumn50.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn50.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle45.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle45.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle45.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle45.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle45.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn51.DefaultCellStyle = dataGridViewCellStyle45;
		this.dataGridViewTextBoxColumn51.HeaderText = headerText12;
		this.dataGridViewTextBoxColumn51.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn51.Name = name298;
		this.dataGridViewTextBoxColumn51.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn51.Width = 80;
		dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle46.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle46.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle46.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle46.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle46.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn52.DefaultCellStyle = dataGridViewCellStyle46;
		this.dataGridViewTextBoxColumn52.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn52.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn52.Name = name299;
		this.dataGridViewTextBoxColumn52.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn52.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn52.Width = 85;
		dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle47.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle47.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle47.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle47.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle47.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn53.DefaultCellStyle = dataGridViewCellStyle47;
		this.dataGridViewTextBoxColumn53.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn53.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn53.Name = name300;
		this.dataGridViewTextBoxColumn53.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn53.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn53.Width = 85;
		dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle48.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle48.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle48.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn54.DefaultCellStyle = dataGridViewCellStyle48;
		this.dataGridViewTextBoxColumn54.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn54.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn54.Name = name301;
		this.dataGridViewTextBoxColumn54.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn54.Visible = false;
		this.Andproid.Controls.Add(this.groupControl4);
		this.Andproid.Name = name302;
		this.Andproid.Size = new System.Drawing.Size(811, 526);
		this.Andproid.Text = text144;
		this.groupControl4.Controls.Add(this.simpleButton2);
		this.groupControl4.Controls.Add(this.SafeFormat);
		this.groupControl4.Controls.Add(this.metaFactoryReset);
		this.groupControl4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupControl4.Location = new System.Drawing.Point(0, 0);
		this.groupControl4.Name = name303;
		this.groupControl4.Size = new System.Drawing.Size(811, 526);
		this.groupControl4.TabIndex = 2;
		this.groupControl4.Text = text145;
		this.simpleButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.simpleButton2.Location = new System.Drawing.Point(26, 116);
		this.simpleButton2.Name = name304;
		this.simpleButton2.Size = new System.Drawing.Size(770, 28);
		this.simpleButton2.TabIndex = 426;
		this.simpleButton2.Text = text146;
		this.SafeFormat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SafeFormat.Location = new System.Drawing.Point(24, 82);
		this.SafeFormat.Name = name305;
		this.SafeFormat.Size = new System.Drawing.Size(770, 28);
		this.SafeFormat.TabIndex = 425;
		this.SafeFormat.Text = text71;
		this.SafeFormat.Click += new System.EventHandler(METAHandler);
		this.metaFactoryReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.metaFactoryReset.Location = new System.Drawing.Point(24, 48);
		this.metaFactoryReset.Name = name306;
		this.metaFactoryReset.Size = new System.Drawing.Size(770, 28);
		this.metaFactoryReset.TabIndex = 424;
		this.metaFactoryReset.Text = text23;
		this.metaFactoryReset.Click += new System.EventHandler(METAHandler);
		this.TabSamsung.Controls.Add(this.TabSubSamsung);
		this.TabSamsung.Name = name307;
		this.TabSamsung.Size = new System.Drawing.Size(813, 557);
		this.TabSamsung.Text = text57;
		this.TabSubSamsung.Appearance.BackColor = System.Drawing.Color.White;
		this.TabSubSamsung.Appearance.Options.UseBackColor = true;
		this.TabSubSamsung.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabSubSamsung.Location = new System.Drawing.Point(0, 0);
		this.TabSubSamsung.LookAndFeel.SkinName = skinName;
		this.TabSubSamsung.LookAndFeel.UseDefaultLookAndFeel = false;
		this.TabSubSamsung.Name = name308;
		this.TabSubSamsung.SelectedTabPage = this.TabSamMTP;
		this.TabSubSamsung.Size = new System.Drawing.Size(813, 557);
		this.TabSubSamsung.TabIndex = 474;
		this.TabSubSamsung.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[3] { this.TabSamMTP, this.TabSamsOdin, this.xtraTabPage2 });
		this.TabSamMTP.Controls.Add(this.GADBMode);
		this.TabSamMTP.Controls.Add(this.GMTPMode);
		this.TabSamMTP.Name = name309;
		this.TabSamMTP.Size = new System.Drawing.Size(811, 526);
		this.TabSamMTP.Text = text147;
		this.GADBMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.GADBMode.Controls.Add(this.FRPRemove16);
		this.GADBMode.Controls.Add(this.ADBSamFrp);
		this.GADBMode.Controls.Add(this.ADBSamId);
		this.GADBMode.Location = new System.Drawing.Point(32, 283);
		this.GADBMode.Name = name310;
		this.GADBMode.Size = new System.Drawing.Size(747, 244);
		this.GADBMode.TabIndex = 1;
		this.GADBMode.Text = text148;
		this.FRPRemove16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FRPRemove16.Location = new System.Drawing.Point(21, 125);
		this.FRPRemove16.Name = name311;
		this.FRPRemove16.Size = new System.Drawing.Size(706, 28);
		this.FRPRemove16.TabIndex = 426;
		this.FRPRemove16.Text = text149;
		this.FRPRemove16.Click += new System.EventHandler(ADBHandler);
		this.ADBSamFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ADBSamFrp.Location = new System.Drawing.Point(19, 91);
		this.ADBSamFrp.Name = name312;
		this.ADBSamFrp.Size = new System.Drawing.Size(706, 28);
		this.ADBSamFrp.TabIndex = 425;
		this.ADBSamFrp.Text = text150;
		this.ADBSamFrp.Click += new System.EventHandler(ADBHandler);
		this.ADBSamId.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ADBSamId.Location = new System.Drawing.Point(19, 57);
		this.ADBSamId.Name = name313;
		this.ADBSamId.Size = new System.Drawing.Size(706, 28);
		this.ADBSamId.TabIndex = 424;
		this.ADBSamId.Text = text151;
		this.ADBSamId.Click += new System.EventHandler(ADBHandler);
		this.GMTPMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.GMTPMode.Controls.Add(this.MTPRebootOdin);
		this.GMTPMode.Controls.Add(this.MTPSN);
		this.GMTPMode.Controls.Add(this.MTPReset);
		this.GMTPMode.Controls.Add(this.MTPFrp16);
		this.GMTPMode.Controls.Add(this.MTPFrp);
		this.GMTPMode.Controls.Add(this.MTPInfo);
		this.GMTPMode.Location = new System.Drawing.Point(32, 17);
		this.GMTPMode.Name = name314;
		this.GMTPMode.Size = new System.Drawing.Size(747, 260);
		this.GMTPMode.TabIndex = 0;
		this.GMTPMode.Text = text152;
		this.MTPRebootOdin.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPRebootOdin.Location = new System.Drawing.Point(19, 209);
		this.MTPRebootOdin.Name = name315;
		this.MTPRebootOdin.Size = new System.Drawing.Size(706, 28);
		this.MTPRebootOdin.TabIndex = 428;
		this.MTPRebootOdin.Text = text153;
		this.MTPRebootOdin.Click += new System.EventHandler(MTPHandler);
		this.MTPSN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPSN.Location = new System.Drawing.Point(19, 175);
		this.MTPSN.Name = name316;
		this.MTPSN.Size = new System.Drawing.Size(706, 28);
		this.MTPSN.TabIndex = 427;
		this.MTPSN.Text = text154;
		this.MTPSN.Click += new System.EventHandler(MTPHandler);
		this.MTPReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPReset.Location = new System.Drawing.Point(19, 141);
		this.MTPReset.Name = name317;
		this.MTPReset.Size = new System.Drawing.Size(706, 28);
		this.MTPReset.TabIndex = 426;
		this.MTPReset.Text = text23;
		this.MTPReset.Click += new System.EventHandler(MTPHandler);
		this.MTPFrp16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPFrp16.Location = new System.Drawing.Point(19, 107);
		this.MTPFrp16.Name = name318;
		this.MTPFrp16.Size = new System.Drawing.Size(706, 28);
		this.MTPFrp16.TabIndex = 425;
		this.MTPFrp16.Text = text155;
		this.MTPFrp16.Click += new System.EventHandler(MTPHandler);
		this.MTPFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPFrp.Location = new System.Drawing.Point(19, 73);
		this.MTPFrp.Name = name319;
		this.MTPFrp.Size = new System.Drawing.Size(706, 28);
		this.MTPFrp.TabIndex = 424;
		this.MTPFrp.Text = text156;
		this.MTPFrp.Click += new System.EventHandler(MTPHandler);
		this.MTPInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.MTPInfo.Location = new System.Drawing.Point(19, 39);
		this.MTPInfo.Name = name320;
		this.MTPInfo.Size = new System.Drawing.Size(706, 28);
		this.MTPInfo.TabIndex = 423;
		this.MTPInfo.Text = text157;
		this.MTPInfo.Click += new System.EventHandler(MTPHandler);
		this.TabSamsOdin.Controls.Add(this.groupControl1);
		this.TabSamsOdin.Name = name321;
		this.TabSamsOdin.Size = new System.Drawing.Size(811, 526);
		this.TabSamsOdin.Text = text158;
		this.groupControl1.Controls.Add(this.Reset2024);
		this.groupControl1.Controls.Add(this.OdinInfo);
		this.groupControl1.Controls.Add(this.OdinReboot);
		this.groupControl1.Controls.Add(this.OdinSoftbrick);
		this.groupControl1.Controls.Add(this.OdinFrp);
		this.groupControl1.Controls.Add(this.OdinReset);
		this.groupControl1.Controls.Add(this.OdinWritePit);
		this.groupControl1.Controls.Add(this.OdinReadPit);
		this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupControl1.Location = new System.Drawing.Point(0, 0);
		this.groupControl1.Name = name322;
		this.groupControl1.Size = new System.Drawing.Size(811, 526);
		this.groupControl1.TabIndex = 498;
		this.groupControl1.Text = text159;
		this.Reset2024.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.Reset2024.Location = new System.Drawing.Point(19, 202);
		this.Reset2024.Name = name323;
		this.Reset2024.Size = new System.Drawing.Size(770, 28);
		this.Reset2024.TabIndex = 424;
		this.Reset2024.Text = text160;
		this.Reset2024.Click += new System.EventHandler(OdinHandler);
		this.OdinInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinInfo.Location = new System.Drawing.Point(19, 65);
		this.OdinInfo.Name = name324;
		this.OdinInfo.Size = new System.Drawing.Size(770, 28);
		this.OdinInfo.TabIndex = 423;
		this.OdinInfo.Text = text157;
		this.OdinInfo.Click += new System.EventHandler(OdinHandler);
		this.OdinReboot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinReboot.Location = new System.Drawing.Point(19, 304);
		this.OdinReboot.Name = name325;
		this.OdinReboot.Size = new System.Drawing.Size(770, 28);
		this.OdinReboot.TabIndex = 422;
		this.OdinReboot.Text = text19;
		this.OdinReboot.Click += new System.EventHandler(OdinHandler);
		this.OdinSoftbrick.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinSoftbrick.Location = new System.Drawing.Point(19, 270);
		this.OdinSoftbrick.Name = name326;
		this.OdinSoftbrick.Size = new System.Drawing.Size(770, 28);
		this.OdinSoftbrick.TabIndex = 421;
		this.OdinSoftbrick.Text = text161;
		this.OdinSoftbrick.Click += new System.EventHandler(OdinHandler);
		this.OdinFrp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinFrp.Location = new System.Drawing.Point(19, 236);
		this.OdinFrp.Name = name327;
		this.OdinFrp.Size = new System.Drawing.Size(770, 28);
		this.OdinFrp.TabIndex = 420;
		this.OdinFrp.Text = text162;
		this.OdinFrp.Click += new System.EventHandler(OdinHandler);
		this.OdinReset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinReset.Location = new System.Drawing.Point(19, 167);
		this.OdinReset.Name = name328;
		this.OdinReset.Size = new System.Drawing.Size(770, 28);
		this.OdinReset.TabIndex = 419;
		this.OdinReset.Text = text113;
		this.OdinReset.Click += new System.EventHandler(OdinHandler);
		this.OdinWritePit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinWritePit.Location = new System.Drawing.Point(19, 133);
		this.OdinWritePit.Name = name329;
		this.OdinWritePit.Size = new System.Drawing.Size(770, 28);
		this.OdinWritePit.TabIndex = 418;
		this.OdinWritePit.Text = text163;
		this.OdinWritePit.Click += new System.EventHandler(OdinHandler);
		this.OdinReadPit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinReadPit.Location = new System.Drawing.Point(19, 99);
		this.OdinReadPit.Name = name330;
		this.OdinReadPit.Size = new System.Drawing.Size(770, 28);
		this.OdinReadPit.TabIndex = 417;
		this.OdinReadPit.Text = text164;
		this.OdinReadPit.Click += new System.EventHandler(OdinHandler);
		this.xtraTabPage2.Controls.Add(this.CheckOdin);
		this.xtraTabPage2.Controls.Add(this.OdinGridView);
		this.xtraTabPage2.Controls.Add(this.label6);
		this.xtraTabPage2.Controls.Add(this.label5);
		this.xtraTabPage2.Controls.Add(this.label4);
		this.xtraTabPage2.Controls.Add(this.label3);
		this.xtraTabPage2.Controls.Add(this.label2);
		this.xtraTabPage2.Controls.Add(this.ClearOdinFirmware);
		this.xtraTabPage2.Controls.Add(this.COdinEraseEFS);
		this.xtraTabPage2.Controls.Add(this.COdinRepartition);
		this.xtraTabPage2.Controls.Add(this.OdinFlash);
		this.xtraTabPage2.Controls.Add(this.LoadHome);
		this.xtraTabPage2.Controls.Add(this.LoadCSC);
		this.xtraTabPage2.Controls.Add(this.LoadCP);
		this.xtraTabPage2.Controls.Add(this.LoadAP);
		this.xtraTabPage2.Controls.Add(this.LoadBL);
		this.xtraTabPage2.Controls.Add(this.txtHOME);
		this.xtraTabPage2.Controls.Add(this.txtCSC);
		this.xtraTabPage2.Controls.Add(this.txtCP);
		this.xtraTabPage2.Controls.Add(this.txtAP);
		this.xtraTabPage2.Controls.Add(this.txtBL);
		this.xtraTabPage2.Name = name331;
		this.xtraTabPage2.Size = new System.Drawing.Size(811, 526);
		this.xtraTabPage2.Text = text165;
		this.CheckOdin.AutoSize = true;
		this.CheckOdin.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CheckOdin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CheckOdin.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CheckOdin.Location = new System.Drawing.Point(9, 14);
		this.CheckOdin.Name = name332;
		this.CheckOdin.Size = new System.Drawing.Size(12, 11);
		this.CheckOdin.TabIndex = 536;
		this.CheckOdin.UseVisualStyleBackColor = false;
		this.CheckOdin.CheckedChanged += new System.EventHandler(CheckOdin_CheckedChanged);
		this.OdinGridView.AllowDrop = true;
		this.OdinGridView.AllowUserToAddRows = false;
		this.OdinGridView.AllowUserToDeleteRows = false;
		this.OdinGridView.AllowUserToResizeColumns = false;
		this.OdinGridView.AllowUserToResizeRows = false;
		dataGridViewCellStyle49.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle49.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle49.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle49.SelectionForeColor = System.Drawing.Color.DimGray;
		this.OdinGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle49;
		this.OdinGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinGridView.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.OdinGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.OdinGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
		this.OdinGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle50.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle50.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle50.SelectionForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.OdinGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle50;
		this.OdinGridView.ColumnHeadersHeight = 17;
		this.OdinGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.OdinGridView.Columns.AddRange(this.dataGridViewCheckBoxColumn9, this.dataGridViewTextBoxColumn43, this.dataGridViewTextBoxColumn44, this.dataGridViewTextBoxColumn45, this.dataGridViewTextBoxColumn46, this.dataGridViewTextBoxColumn47, this.dataGridViewTextBoxColumn48, this.Column10);
		dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle51.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle51.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle51.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle51.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle51.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		dataGridViewCellStyle51.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.OdinGridView.DefaultCellStyle = dataGridViewCellStyle51;
		this.OdinGridView.EnableHeadersVisualStyles = false;
		this.OdinGridView.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.OdinGridView.Location = new System.Drawing.Point(2, 11);
		this.OdinGridView.Name = name333;
		this.OdinGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle52.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle52.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle52.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle52.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle52.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle52.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.OdinGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle52;
		this.OdinGridView.RowHeadersVisible = false;
		this.OdinGridView.RowHeadersWidth = 20;
		dataGridViewCellStyle53.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle53.ForeColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle53.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle53.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.OdinGridView.RowsDefaultCellStyle = dataGridViewCellStyle53;
		this.OdinGridView.RowTemplate.Height = 20;
		this.OdinGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.OdinGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.OdinGridView.Size = new System.Drawing.Size(808, 196);
		this.OdinGridView.StandardTab = true;
		this.OdinGridView.TabIndex = 535;
		dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle54.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle54.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle54.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle54.NullValue = false;
		dataGridViewCellStyle54.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle54.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn9.DefaultCellStyle = dataGridViewCellStyle54;
		this.dataGridViewCheckBoxColumn9.Frozen = true;
		this.dataGridViewCheckBoxColumn9.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn9.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn9.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn9.Name = name334;
		this.dataGridViewCheckBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn9.Width = 75;
		dataGridViewCellStyle55.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle55.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle55.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle55.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle55.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn43.DefaultCellStyle = dataGridViewCellStyle55;
		this.dataGridViewTextBoxColumn43.Frozen = true;
		this.dataGridViewTextBoxColumn43.HeaderText = headerText9;
		this.dataGridViewTextBoxColumn43.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn43.Name = name335;
		this.dataGridViewTextBoxColumn43.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn43.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn43.Width = 150;
		this.dataGridViewTextBoxColumn44.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle56.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle56.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle56.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle56.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle56.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn44.DefaultCellStyle = dataGridViewCellStyle56;
		this.dataGridViewTextBoxColumn44.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn44.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn44.Name = name336;
		this.dataGridViewTextBoxColumn44.ReadOnly = true;
		this.dataGridViewTextBoxColumn44.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn44.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle57.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle57.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle57.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle57.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle57.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn45.DefaultCellStyle = dataGridViewCellStyle57;
		this.dataGridViewTextBoxColumn45.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn45.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn45.Name = name337;
		this.dataGridViewTextBoxColumn45.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn45.Width = 80;
		dataGridViewCellStyle58.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle58.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle58.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle58.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle58.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle58.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn46.DefaultCellStyle = dataGridViewCellStyle58;
		this.dataGridViewTextBoxColumn46.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn46.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn46.Name = name338;
		this.dataGridViewTextBoxColumn46.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn46.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn46.Width = 85;
		dataGridViewCellStyle59.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle59.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle59.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle59.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle59.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle59.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn47.DefaultCellStyle = dataGridViewCellStyle59;
		this.dataGridViewTextBoxColumn47.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn47.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn47.Name = name339;
		this.dataGridViewTextBoxColumn47.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn47.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn47.Width = 85;
		dataGridViewCellStyle60.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle60.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle60.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle60.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle60.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn48.DefaultCellStyle = dataGridViewCellStyle60;
		this.dataGridViewTextBoxColumn48.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn48.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn48.Name = name340;
		this.dataGridViewTextBoxColumn48.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn48.Visible = false;
		this.Column10.HeaderText = headerText8;
		this.Column10.Name = name341;
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(14, 440);
		this.label6.Name = name342;
		this.label6.Size = new System.Drawing.Size(55, 13);
		this.label6.TabIndex = 534;
		this.label6.Text = text166;
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(23, 405);
		this.label5.Name = name343;
		this.label5.Size = new System.Drawing.Size(46, 13);
		this.label5.TabIndex = 533;
		this.label5.Text = text167;
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(30, 371);
		this.label4.Name = name344;
		this.label4.Size = new System.Drawing.Size(39, 13);
		this.label4.TabIndex = 532;
		this.label4.Text = text168;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(30, 337);
		this.label3.Name = name345;
		this.label3.Size = new System.Drawing.Size(39, 13);
		this.label3.TabIndex = 531;
		this.label3.Text = text169;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(32, 301);
		this.label2.Name = name346;
		this.label2.Size = new System.Drawing.Size(37, 13);
		this.label2.TabIndex = 530;
		this.label2.Text = text170;
		this.ClearOdinFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.ClearOdinFirmware.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.ClearOdinFirmware.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ClearOdinFirmware.Location = new System.Drawing.Point(699, 479);
		this.ClearOdinFirmware.Name = name347;
		this.ClearOdinFirmware.Size = new System.Drawing.Size(93, 28);
		this.ClearOdinFirmware.TabIndex = 529;
		this.ClearOdinFirmware.Text = text30;
		this.ClearOdinFirmware.Click += new System.EventHandler(OdinClearfirmware);
		this.COdinEraseEFS.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.COdinEraseEFS.Location = new System.Drawing.Point(118, 266);
		this.COdinEraseEFS.Name = name348;
		this.COdinEraseEFS.Properties.AllowFocused = false;
		this.COdinEraseEFS.Properties.Caption = caption17;
		this.COdinEraseEFS.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.COdinEraseEFS.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.COdinEraseEFS.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.COdinEraseEFS.Size = new System.Drawing.Size(79, 22);
		this.COdinEraseEFS.TabIndex = 528;
		this.COdinEraseEFS.CheckStateChanged += new System.EventHandler(COdinEraseEFS_CheckedChanged);
		this.COdinRepartition.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.COdinRepartition.Location = new System.Drawing.Point(10, 266);
		this.COdinRepartition.Name = name349;
		this.COdinRepartition.Properties.AllowFocused = false;
		this.COdinRepartition.Properties.Caption = caption18;
		this.COdinRepartition.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.COdinRepartition.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.COdinRepartition.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.COdinRepartition.Size = new System.Drawing.Size(95, 22);
		this.COdinRepartition.TabIndex = 527;
		this.COdinRepartition.CheckStateChanged += new System.EventHandler(COdinRepartition_CheckedChanged);
		this.OdinFlash.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OdinFlash.ImageOptions.Image = AMFTool.Properties.Resources.flash;
		this.OdinFlash.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.OdinFlash.Location = new System.Drawing.Point(17, 479);
		this.OdinFlash.Name = name350;
		this.OdinFlash.Size = new System.Drawing.Size(676, 28);
		this.OdinFlash.TabIndex = 526;
		this.OdinFlash.Text = text171;
		this.OdinFlash.Click += new System.EventHandler(OdinHandler);
		this.LoadHome.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LoadHome.ImageOptions.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.LoadHome.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.LoadHome.Location = new System.Drawing.Point(729, 435);
		this.LoadHome.Name = name351;
		this.LoadHome.Size = new System.Drawing.Size(62, 20);
		this.LoadHome.TabIndex = 525;
		this.LoadHome.Text = text29;
		this.LoadHome.Click += new System.EventHandler(OdinLoad);
		this.LoadCSC.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LoadCSC.ImageOptions.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.LoadCSC.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.LoadCSC.Location = new System.Drawing.Point(729, 401);
		this.LoadCSC.Name = name352;
		this.LoadCSC.Size = new System.Drawing.Size(62, 20);
		this.LoadCSC.TabIndex = 524;
		this.LoadCSC.Text = text29;
		this.LoadCSC.Click += new System.EventHandler(OdinLoad);
		this.LoadCP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LoadCP.ImageOptions.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.LoadCP.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.LoadCP.Location = new System.Drawing.Point(729, 367);
		this.LoadCP.Name = name353;
		this.LoadCP.Size = new System.Drawing.Size(62, 20);
		this.LoadCP.TabIndex = 523;
		this.LoadCP.Text = text29;
		this.LoadCP.Click += new System.EventHandler(OdinLoad);
		this.LoadAP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LoadAP.ImageOptions.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.LoadAP.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.LoadAP.Location = new System.Drawing.Point(729, 332);
		this.LoadAP.Name = name354;
		this.LoadAP.Size = new System.Drawing.Size(62, 20);
		this.LoadAP.TabIndex = 522;
		this.LoadAP.Text = text29;
		this.LoadAP.Click += new System.EventHandler(OdinLoad);
		this.LoadBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.LoadBL.ImageOptions.Image = AMFTool.Properties.Resources.icons8_opened_folder_17px;
		this.LoadBL.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.LoadBL.Location = new System.Drawing.Point(729, 299);
		this.LoadBL.Name = name355;
		this.LoadBL.Size = new System.Drawing.Size(62, 20);
		this.LoadBL.TabIndex = 521;
		this.LoadBL.Text = text29;
		this.LoadBL.Click += new System.EventHandler(OdinLoad);
		this.txtHOME.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtHOME.Location = new System.Drawing.Point(75, 436);
		this.txtHOME.Name = name356;
		this.txtHOME.Properties.NullText = nullText;
		this.txtHOME.Size = new System.Drawing.Size(648, 20);
		this.txtHOME.TabIndex = 520;
		this.txtCSC.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCSC.EditValue = text172;
		this.txtCSC.Location = new System.Drawing.Point(75, 402);
		this.txtCSC.Name = name357;
		this.txtCSC.Properties.NullText = text173;
		this.txtCSC.Size = new System.Drawing.Size(648, 20);
		this.txtCSC.TabIndex = 519;
		this.txtCP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCP.EditValue = text173;
		this.txtCP.Location = new System.Drawing.Point(75, 368);
		this.txtCP.Name = name358;
		this.txtCP.Properties.NullText = text172;
		this.txtCP.Size = new System.Drawing.Size(648, 20);
		this.txtCP.TabIndex = 518;
		this.txtAP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAP.Location = new System.Drawing.Point(75, 334);
		this.txtAP.Name = name359;
		this.txtAP.Properties.NullText = nullText2;
		this.txtAP.Size = new System.Drawing.Size(648, 20);
		this.txtAP.TabIndex = 517;
		this.txtBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBL.Location = new System.Drawing.Point(75, 300);
		this.txtBL.Name = name360;
		this.txtBL.Properties.NullText = nullText3;
		this.txtBL.Size = new System.Drawing.Size(648, 20);
		this.txtBL.TabIndex = 516;
		this.TabAdbFastboot.Controls.Add(this.xtraTabControl1);
		this.TabAdbFastboot.Name = name361;
		this.TabAdbFastboot.Size = new System.Drawing.Size(813, 557);
		this.TabAdbFastboot.Text = text174;
		this.xtraTabControl1.Appearance.BackColor = System.Drawing.Color.White;
		this.xtraTabControl1.Appearance.Options.UseBackColor = true;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.LookAndFeel.SkinName = skinName;
		this.xtraTabControl1.LookAndFeel.UseDefaultLookAndFeel = false;
		this.xtraTabControl1.Name = name362;
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage4;
		this.xtraTabControl1.Size = new System.Drawing.Size(813, 557);
		this.xtraTabControl1.TabIndex = 0;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[3] { this.xtraTabPage3, this.xtraTabPage4, this.xtraTabPage1 });
		this.xtraTabPage4.Controls.Add(this.FBinfofir);
		this.xtraTabPage4.Controls.Add(this.CFastbootPart);
		this.xtraTabPage4.Controls.Add(this.FastbootDataGrid);
		this.xtraTabPage4.Controls.Add(this.FBUnlockBL);
		this.xtraTabPage4.Controls.Add(this.FBRebootRecovery);
		this.xtraTabPage4.Controls.Add(this.FBRebootEDL);
		this.xtraTabPage4.Controls.Add(this.FBReadGPT);
		this.xtraTabPage4.Controls.Add(this.FBSlotA);
		this.xtraTabPage4.Controls.Add(this.FBEFSErase);
		this.xtraTabPage4.Controls.Add(this.FBReboot);
		this.xtraTabPage4.Controls.Add(this.FBResetFrp);
		this.xtraTabPage4.Controls.Add(this.FastbootClearFirmware);
		this.xtraTabPage4.Controls.Add(this.labelControl1);
		this.xtraTabPage4.Controls.Add(this.FBFlash);
		this.xtraTabPage4.Controls.Add(this.FastbootLoadFirmware);
		this.xtraTabPage4.Controls.Add(this.FastbootFirmware);
		this.xtraTabPage4.Name = name363;
		this.xtraTabPage4.Size = new System.Drawing.Size(811, 526);
		this.xtraTabPage4.Text = text175;
		this.FBinfofir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBinfofir.EditValue = true;
		this.FBinfofir.Location = new System.Drawing.Point(7, 384);
		this.FBinfofir.Name = name364;
		this.FBinfofir.Properties.AllowFocused = false;
		this.FBinfofir.Properties.Caption = caption2;
		this.FBinfofir.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.FBinfofir.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.FBinfofir.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.FBinfofir.Size = new System.Drawing.Size(94, 22);
		this.FBinfofir.TabIndex = 481;
		this.FBinfofir.CheckedChanged += new System.EventHandler(FBinfofir_CheckedChanged);
		this.CFastbootPart.AutoSize = true;
		this.CFastbootPart.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.CFastbootPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CFastbootPart.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CFastbootPart.Location = new System.Drawing.Point(4, 3);
		this.CFastbootPart.Name = name365;
		this.CFastbootPart.Size = new System.Drawing.Size(12, 11);
		this.CFastbootPart.TabIndex = 464;
		this.CFastbootPart.UseVisualStyleBackColor = false;
		this.CFastbootPart.CheckedChanged += new System.EventHandler(CPartFastboot_CheckedChanged);
		this.FastbootDataGrid.AllowDrop = true;
		this.FastbootDataGrid.AllowUserToAddRows = false;
		this.FastbootDataGrid.AllowUserToDeleteRows = false;
		this.FastbootDataGrid.AllowUserToResizeColumns = false;
		this.FastbootDataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle61.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle61.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle61.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle61.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.FastbootDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle61;
		this.FastbootDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FastbootDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.FastbootDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.FastbootDataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle62.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle62.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle62.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle62.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle62.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle62.SelectionForeColor = System.Drawing.Color.Gray;
		dataGridViewCellStyle62.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.FastbootDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle62;
		this.FastbootDataGrid.ColumnHeadersHeight = 17;
		this.FastbootDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.FastbootDataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn7, this.dataGridViewTextBoxColumn28, this.dataGridViewTextBoxColumn29, this.dataGridViewTextBoxColumn30, this.dataGridViewTextBoxColumn31, this.dataGridViewTextBoxColumn33);
		dataGridViewCellStyle63.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle63.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle63.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle63.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle63.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle63.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle63.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.FastbootDataGrid.DefaultCellStyle = dataGridViewCellStyle63;
		this.FastbootDataGrid.EnableHeadersVisualStyles = false;
		this.FastbootDataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.FastbootDataGrid.Location = new System.Drawing.Point(2, 1);
		this.FastbootDataGrid.Name = name366;
		this.FastbootDataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle64.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle64.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle64.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle64.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle64.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle64.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle64.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.FastbootDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle64;
		this.FastbootDataGrid.RowHeadersVisible = false;
		this.FastbootDataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle65.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle65.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle65.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle65.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.FastbootDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle65;
		this.FastbootDataGrid.RowTemplate.Height = 20;
		this.FastbootDataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FastbootDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.FastbootDataGrid.Size = new System.Drawing.Size(808, 373);
		this.FastbootDataGrid.StandardTab = true;
		this.FastbootDataGrid.TabIndex = 463;
		this.FastbootDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(FastbootDataGrid_CellContentClick);
		this.FastbootDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(FastbootDataGrid_CellDoubleClick);
		this.FastbootDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(FastbootDataGrid_CellMouseDown);
		dataGridViewCellStyle66.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle66.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle66.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle66.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle66.NullValue = false;
		dataGridViewCellStyle66.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle66.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn7.DefaultCellStyle = dataGridViewCellStyle66;
		this.dataGridViewCheckBoxColumn7.Frozen = true;
		this.dataGridViewCheckBoxColumn7.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn7.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn7.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn7.Name = name367;
		this.dataGridViewCheckBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn7.Width = 75;
		dataGridViewCellStyle67.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle67.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle67.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle67.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle67.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn28.DefaultCellStyle = dataGridViewCellStyle67;
		this.dataGridViewTextBoxColumn28.Frozen = true;
		this.dataGridViewTextBoxColumn28.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn28.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn28.Name = name368;
		this.dataGridViewTextBoxColumn28.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn28.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn28.Width = 120;
		dataGridViewCellStyle68.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle68.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle68.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle68.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle68.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn29.DefaultCellStyle = dataGridViewCellStyle68;
		this.dataGridViewTextBoxColumn29.HeaderText = headerText11;
		this.dataGridViewTextBoxColumn29.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn29.Name = name369;
		this.dataGridViewTextBoxColumn29.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn29.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn29.Width = 150;
		dataGridViewCellStyle69.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle69.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle69.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle69.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle69.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn30.DefaultCellStyle = dataGridViewCellStyle69;
		this.dataGridViewTextBoxColumn30.HeaderText = headerText12;
		this.dataGridViewTextBoxColumn30.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn30.Name = name370;
		this.dataGridViewTextBoxColumn30.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn30.Width = 80;
		this.dataGridViewTextBoxColumn31.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle70.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle70.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle70.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle70.ForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle70.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle70.SelectionForeColor = System.Drawing.Color.FromArgb(72, 70, 68);
		this.dataGridViewTextBoxColumn31.DefaultCellStyle = dataGridViewCellStyle70;
		this.dataGridViewTextBoxColumn31.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn31.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn31.Name = name371;
		this.dataGridViewTextBoxColumn31.ReadOnly = true;
		this.dataGridViewTextBoxColumn31.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn31.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle71.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle71.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle71.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle71.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle71.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn33.DefaultCellStyle = dataGridViewCellStyle71;
		this.dataGridViewTextBoxColumn33.HeaderText = headerText13;
		this.dataGridViewTextBoxColumn33.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn33.Name = name372;
		this.dataGridViewTextBoxColumn33.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn33.Visible = false;
		this.FBUnlockBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBUnlockBL.Location = new System.Drawing.Point(500, 482);
		this.FBUnlockBL.Name = name373;
		this.FBUnlockBL.Size = new System.Drawing.Size(99, 28);
		this.FBUnlockBL.TabIndex = 462;
		this.FBUnlockBL.Text = text176;
		this.FBUnlockBL.Click += new System.EventHandler(FastbootHandler);
		this.FBRebootRecovery.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBRebootRecovery.Location = new System.Drawing.Point(395, 482);
		this.FBRebootRecovery.Name = name374;
		this.FBRebootRecovery.Size = new System.Drawing.Size(99, 28);
		this.FBRebootRecovery.TabIndex = 461;
		this.FBRebootRecovery.Text = text177;
		this.FBRebootRecovery.Click += new System.EventHandler(FastbootHandler);
		this.FBRebootEDL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBRebootEDL.Location = new System.Drawing.Point(299, 482);
		this.FBRebootEDL.Name = name375;
		this.FBRebootEDL.Size = new System.Drawing.Size(90, 28);
		this.FBRebootEDL.TabIndex = 460;
		this.FBRebootEDL.Text = text178;
		this.FBRebootEDL.Click += new System.EventHandler(FastbootHandler);
		this.FBReadGPT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.FBReadGPT.Location = new System.Drawing.Point(705, 381);
		this.FBReadGPT.Name = name376;
		this.FBReadGPT.Size = new System.Drawing.Size(90, 28);
		this.FBReadGPT.TabIndex = 459;
		this.FBReadGPT.Text = text20;
		this.FBReadGPT.Click += new System.EventHandler(FastbootHandler);
		this.FBSlotA.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBSlotA.Location = new System.Drawing.Point(203, 482);
		this.FBSlotA.Name = name377;
		this.FBSlotA.Size = new System.Drawing.Size(90, 28);
		this.FBSlotA.TabIndex = 457;
		this.FBSlotA.Text = text16;
		this.FBSlotA.Click += new System.EventHandler(FastbootHandler);
		this.FBEFSErase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBEFSErase.Location = new System.Drawing.Point(11, 482);
		this.FBEFSErase.Name = name378;
		this.FBEFSErase.Size = new System.Drawing.Size(90, 28);
		this.FBEFSErase.TabIndex = 456;
		this.FBEFSErase.Text = text28;
		this.FBEFSErase.Click += new System.EventHandler(FastbootHandler);
		this.FBReboot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.FBReboot.Location = new System.Drawing.Point(513, 381);
		this.FBReboot.Name = name379;
		this.FBReboot.Size = new System.Drawing.Size(90, 28);
		this.FBReboot.TabIndex = 455;
		this.FBReboot.Text = text19;
		this.FBReboot.Click += new System.EventHandler(FastbootHandler);
		this.FBResetFrp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FBResetFrp.Location = new System.Drawing.Point(107, 482);
		this.FBResetFrp.Name = name380;
		this.FBResetFrp.Size = new System.Drawing.Size(90, 28);
		this.FBResetFrp.TabIndex = 454;
		this.FBResetFrp.Text = text22;
		this.FBResetFrp.Click += new System.EventHandler(FastbootHandler);
		this.FastbootClearFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.FastbootClearFirmware.Location = new System.Drawing.Point(685, 432);
		this.FastbootClearFirmware.Name = name381;
		this.FastbootClearFirmware.Size = new System.Drawing.Size(52, 20);
		this.FastbootClearFirmware.TabIndex = 453;
		this.FastbootClearFirmware.Text = text30;
		this.FastbootClearFirmware.Click += new System.EventHandler(FastbootClear);
		this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.labelControl1.Appearance.Font = new System.Drawing.Font(familyName4, field_19);
		this.labelControl1.Appearance.Options.UseFont = true;
		this.labelControl1.Location = new System.Drawing.Point(18, 434);
		this.labelControl1.Name = name382;
		this.labelControl1.Size = new System.Drawing.Size(52, 15);
		this.labelControl1.TabIndex = 452;
		this.labelControl1.Text = text34;
		this.FBFlash.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.FBFlash.Location = new System.Drawing.Point(609, 381);
		this.FBFlash.Name = name383;
		this.FBFlash.Size = new System.Drawing.Size(90, 28);
		this.FBFlash.TabIndex = 451;
		this.FBFlash.Text = text171;
		this.FBFlash.Click += new System.EventHandler(FastbootHandler);
		this.FastbootLoadFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.FastbootLoadFirmware.Location = new System.Drawing.Point(747, 432);
		this.FastbootLoadFirmware.Name = name384;
		this.FastbootLoadFirmware.Size = new System.Drawing.Size(48, 20);
		this.FastbootLoadFirmware.TabIndex = 450;
		this.FastbootLoadFirmware.Text = text29;
		this.FastbootLoadFirmware.Click += new System.EventHandler(FastbootLoad);
		this.FastbootFirmware.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FastbootFirmware.Location = new System.Drawing.Point(82, 432);
		this.FastbootFirmware.Name = name385;
		this.FastbootFirmware.Size = new System.Drawing.Size(597, 20);
		this.FastbootFirmware.TabIndex = 449;
		this.xtraTabPage3.Controls.Add(this.CAndroidPart);
		this.xtraTabPage3.Controls.Add(this.AndroidDataGrid);
		this.xtraTabPage3.Controls.Add(this.txtSearch);
		this.xtraTabPage3.Controls.Add(this.SamsungAccount);
		this.xtraTabPage3.Controls.Add(this.DisableKnox);
		this.xtraTabPage3.Controls.Add(this.ADBDisableHU);
		this.xtraTabPage3.Controls.Add(this.RemoveDemo);
		this.xtraTabPage3.Controls.Add(this.OppoSwith);
		this.xtraTabPage3.Controls.Add(this.RemoveMDM);
		this.xtraTabPage3.Controls.Add(this.Disable_OSOnePlus);
		this.xtraTabPage3.Controls.Add(this.DisableOSTecno);
		this.xtraTabPage3.Controls.Add(this.DisableOSItel);
		this.xtraTabPage3.Controls.Add(this.DisableDemo);
		this.xtraTabPage3.Controls.Add(this.DisabaleOSRealme);
		this.xtraTabPage3.Controls.Add(this.ResetHuaweiID);
		this.xtraTabPage3.Controls.Add(this.RemovePlugin);
		this.xtraTabPage3.Controls.Add(this.rebootdevice);
		this.xtraTabPage3.Controls.Add(this.FixAssistantWIFI);
		this.xtraTabPage3.Controls.Add(this.Remove_Payjoy);
		this.xtraTabPage3.Controls.Add(this.Reset_lock);
		this.xtraTabPage3.Controls.Add(this.EnableDiagRoot);
		this.xtraTabPage3.Controls.Add(this.EnableDiagOperation);
		this.xtraTabPage3.Controls.Add(this.DisablePengecualian);
		this.xtraTabPage3.Controls.Add(this.Disable_OS_Sam);
		this.xtraTabPage3.Controls.Add(this.Disable_OSMI);
		this.xtraTabPage3.Controls.Add(this.Disaba_OSOppo);
		this.xtraTabPage3.Controls.Add(this.Disable_osvivo);
		this.xtraTabPage3.Controls.Add(this.EnablediagXiaomi);
		this.xtraTabPage3.Controls.Add(this.FixAssistant);
		this.xtraTabPage3.Controls.Add(this.DidableMicloud);
		this.xtraTabPage3.Controls.Add(this.SamsungID);
		this.xtraTabPage3.Controls.Add(this.EraseFRP16);
		this.xtraTabPage3.Controls.Add(this.EraseFRP);
		this.xtraTabPage3.Controls.Add(this.ADBEnableApk);
		this.xtraTabPage3.Controls.Add(this.ADBInstallApk);
		this.xtraTabPage3.Controls.Add(this.ADBUninstallApk);
		this.xtraTabPage3.Controls.Add(this.ADBDisableApk);
		this.xtraTabPage3.Controls.Add(this.ADBLoadApk);
		this.xtraTabPage3.Name = name386;
		this.xtraTabPage3.Size = new System.Drawing.Size(811, 526);
		this.xtraTabPage3.Text = text148;
		this.CAndroidPart.AutoSize = true;
		this.CAndroidPart.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CAndroidPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CAndroidPart.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CAndroidPart.Location = new System.Drawing.Point(5, 4);
		this.CAndroidPart.Name = name387;
		this.CAndroidPart.Size = new System.Drawing.Size(12, 11);
		this.CAndroidPart.TabIndex = 464;
		this.CAndroidPart.UseVisualStyleBackColor = false;
		this.CAndroidPart.CheckedChanged += new System.EventHandler(CAndroidPart_CheckedChanged_1);
		this.AndroidDataGrid.AllowDrop = true;
		this.AndroidDataGrid.AllowUserToAddRows = false;
		this.AndroidDataGrid.AllowUserToDeleteRows = false;
		this.AndroidDataGrid.AllowUserToResizeColumns = false;
		this.AndroidDataGrid.AllowUserToResizeRows = false;
		dataGridViewCellStyle72.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle72.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle72.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle72.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.AndroidDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle72;
		this.AndroidDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.AndroidDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.AndroidDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.AndroidDataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle73.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle73.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle73.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle73.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle73.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle73.SelectionForeColor = System.Drawing.Color.Gray;
		dataGridViewCellStyle73.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.AndroidDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle73;
		this.AndroidDataGrid.ColumnHeadersHeight = 17;
		this.AndroidDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.AndroidDataGrid.Columns.AddRange(this.dataGridViewCheckBoxColumn13, this.dataGridViewTextBoxColumn32, this.dataGridViewTextBoxColumn34);
		dataGridViewCellStyle74.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle74.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle74.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle74.ForeColor = System.Drawing.Color.FromArgb(36, 36, 36);
		dataGridViewCellStyle74.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle74.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle74.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.AndroidDataGrid.DefaultCellStyle = dataGridViewCellStyle74;
		this.AndroidDataGrid.EnableHeadersVisualStyles = false;
		this.AndroidDataGrid.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.AndroidDataGrid.Location = new System.Drawing.Point(2, 1);
		this.AndroidDataGrid.Name = name388;
		this.AndroidDataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle75.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle75.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle75.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle75.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle75.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle75.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle75.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.AndroidDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle75;
		this.AndroidDataGrid.RowHeadersVisible = false;
		this.AndroidDataGrid.RowHeadersWidth = 20;
		dataGridViewCellStyle76.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle76.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle76.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle76.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.AndroidDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle76;
		this.AndroidDataGrid.RowTemplate.Height = 20;
		this.AndroidDataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.AndroidDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.AndroidDataGrid.Size = new System.Drawing.Size(808, 326);
		this.AndroidDataGrid.StandardTab = true;
		this.AndroidDataGrid.TabIndex = 463;
		dataGridViewCellStyle77.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle77.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle77.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle77.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle77.NullValue = false;
		dataGridViewCellStyle77.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle77.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn13.DefaultCellStyle = dataGridViewCellStyle77;
		this.dataGridViewCheckBoxColumn13.Frozen = true;
		this.dataGridViewCheckBoxColumn13.HeaderText = headerText14;
		this.dataGridViewCheckBoxColumn13.IndeterminateValue = text;
		this.dataGridViewCheckBoxColumn13.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn13.Name = name389;
		this.dataGridViewCheckBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn13.Width = 75;
		dataGridViewCellStyle78.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle78.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle78.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle78.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle78.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn32.DefaultCellStyle = dataGridViewCellStyle78;
		this.dataGridViewTextBoxColumn32.Frozen = true;
		this.dataGridViewTextBoxColumn32.HeaderText = headerText10;
		this.dataGridViewTextBoxColumn32.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn32.Name = name390;
		this.dataGridViewTextBoxColumn32.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn32.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn32.Width = 200;
		this.dataGridViewTextBoxColumn34.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle79.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle79.Font = new System.Drawing.Font(familyName3, field_19);
		dataGridViewCellStyle79.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle79.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle79.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn34.DefaultCellStyle = dataGridViewCellStyle79;
		this.dataGridViewTextBoxColumn34.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn34.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn34.Name = name391;
		this.dataGridViewTextBoxColumn34.ReadOnly = true;
		this.dataGridViewTextBoxColumn34.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn34.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSearch.EditValue = text;
		this.txtSearch.Location = new System.Drawing.Point(2, 330);
		this.txtSearch.Name = name392;
		this.txtSearch.Properties.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Far;
		this.txtSearch.Properties.ContextImageOptions.Image = (System.Drawing.Image)resources.GetObject(name393);
		this.txtSearch.Size = new System.Drawing.Size(808, 22);
		this.txtSearch.TabIndex = 453;
		this.txtSearch.ToolTip = toolTip;
		this.txtSearch.EditValueChanged += new System.EventHandler(txtSearch_EditValueChanged);
		this.SamsungAccount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SamsungAccount.Location = new System.Drawing.Point(156, 495);
		this.SamsungAccount.Name = name394;
		this.SamsungAccount.Size = new System.Drawing.Size(148, 28);
		this.SamsungAccount.TabIndex = 452;
		this.SamsungAccount.Text = text179;
		this.SamsungAccount.Click += new System.EventHandler(ADBHandler);
		this.DisableKnox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DisableKnox.Location = new System.Drawing.Point(2, 495);
		this.DisableKnox.Name = name395;
		this.DisableKnox.Size = new System.Drawing.Size(148, 28);
		this.DisableKnox.TabIndex = 451;
		this.DisableKnox.Text = text180;
		this.DisableKnox.Click += new System.EventHandler(ADBHandler);
		this.ADBDisableHU.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ADBDisableHU.Location = new System.Drawing.Point(576, 461);
		this.ADBDisableHU.Name = name396;
		this.ADBDisableHU.Size = new System.Drawing.Size(90, 28);
		this.ADBDisableHU.TabIndex = 450;
		this.ADBDisableHU.Text = text181;
		this.ADBDisableHU.Click += new System.EventHandler(ADBHandler);
		this.RemoveDemo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.RemoveDemo.Location = new System.Drawing.Point(483, 461);
		this.RemoveDemo.Name = name397;
		this.RemoveDemo.Size = new System.Drawing.Size(90, 28);
		this.RemoveDemo.TabIndex = 449;
		this.RemoveDemo.Text = text182;
		this.RemoveDemo.Click += new System.EventHandler(ADBHandler);
		this.OppoSwith.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.OppoSwith.Location = new System.Drawing.Point(310, 495);
		this.OppoSwith.Name = name398;
		this.OppoSwith.Size = new System.Drawing.Size(148, 28);
		this.OppoSwith.TabIndex = 448;
		this.OppoSwith.Text = text183;
		this.OppoSwith.Click += new System.EventHandler(ADBHandler);
		this.RemoveMDM.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.RemoveMDM.Location = new System.Drawing.Point(464, 495);
		this.RemoveMDM.Name = name399;
		this.RemoveMDM.Size = new System.Drawing.Size(148, 28);
		this.RemoveMDM.TabIndex = 447;
		this.RemoveMDM.Text = text184;
		this.RemoveMDM.Click += new System.EventHandler(ADBHandler);
		this.Disable_OSOnePlus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Disable_OSOnePlus.Location = new System.Drawing.Point(195, 428);
		this.Disable_OSOnePlus.Name = name400;
		this.Disable_OSOnePlus.Size = new System.Drawing.Size(90, 28);
		this.Disable_OSOnePlus.TabIndex = 446;
		this.Disable_OSOnePlus.Text = text185;
		this.Disable_OSOnePlus.Click += new System.EventHandler(ADBHandler);
		this.DisableOSTecno.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DisableOSTecno.Location = new System.Drawing.Point(576, 394);
		this.DisableOSTecno.Name = name401;
		this.DisableOSTecno.Size = new System.Drawing.Size(90, 28);
		this.DisableOSTecno.TabIndex = 445;
		this.DisableOSTecno.Text = text186;
		this.DisableOSTecno.Click += new System.EventHandler(ADBHandler);
		this.DisableOSItel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DisableOSItel.Location = new System.Drawing.Point(483, 427);
		this.DisableOSItel.Name = name402;
		this.DisableOSItel.Size = new System.Drawing.Size(90, 28);
		this.DisableOSItel.TabIndex = 444;
		this.DisableOSItel.Text = text187;
		this.DisableOSItel.Click += new System.EventHandler(ADBHandler);
		this.DisableDemo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DisableDemo.Location = new System.Drawing.Point(483, 394);
		this.DisableDemo.Name = name403;
		this.DisableDemo.Size = new System.Drawing.Size(90, 28);
		this.DisableDemo.TabIndex = 443;
		this.DisableDemo.Text = text188;
		this.DisableDemo.Click += new System.EventHandler(ADBHandler);
		this.DisabaleOSRealme.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DisabaleOSRealme.Location = new System.Drawing.Point(387, 428);
		this.DisabaleOSRealme.Name = name404;
		this.DisabaleOSRealme.Size = new System.Drawing.Size(90, 28);
		this.DisabaleOSRealme.TabIndex = 442;
		this.DisabaleOSRealme.Text = text189;
		this.DisabaleOSRealme.Click += new System.EventHandler(ADBHandler);
		this.ResetHuaweiID.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ResetHuaweiID.Location = new System.Drawing.Point(618, 495);
		this.ResetHuaweiID.Name = name405;
		this.ResetHuaweiID.Size = new System.Drawing.Size(190, 28);
		this.ResetHuaweiID.TabIndex = 441;
		this.ResetHuaweiID.Text = text190;
		this.ResetHuaweiID.Click += new System.EventHandler(ADBHandler);
		this.RemovePlugin.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.RemovePlugin.Location = new System.Drawing.Point(670, 462);
		this.RemovePlugin.Name = name406;
		this.RemovePlugin.Size = new System.Drawing.Size(138, 28);
		this.RemovePlugin.TabIndex = 440;
		this.RemovePlugin.Text = text191;
		this.RemovePlugin.Click += new System.EventHandler(ADBHandler);
		this.rebootdevice.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.rebootdevice.ImageOptions.Image = AMFTool.Properties.Resources.REboot;
		this.rebootdevice.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.rebootdevice.Location = new System.Drawing.Point(631, 360);
		this.rebootdevice.Name = name407;
		this.rebootdevice.Size = new System.Drawing.Size(81, 28);
		this.rebootdevice.TabIndex = 439;
		this.rebootdevice.Text = text19;
		this.rebootdevice.Click += new System.EventHandler(ADBHandler);
		this.FixAssistantWIFI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FixAssistantWIFI.Location = new System.Drawing.Point(670, 428);
		this.FixAssistantWIFI.Name = name408;
		this.FixAssistantWIFI.Size = new System.Drawing.Size(138, 28);
		this.FixAssistantWIFI.TabIndex = 437;
		this.FixAssistantWIFI.Text = text192;
		this.FixAssistantWIFI.Click += new System.EventHandler(ADBHandler);
		this.Remove_Payjoy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Remove_Payjoy.Location = new System.Drawing.Point(99, 462);
		this.Remove_Payjoy.Name = name409;
		this.Remove_Payjoy.Size = new System.Drawing.Size(90, 28);
		this.Remove_Payjoy.TabIndex = 436;
		this.Remove_Payjoy.Text = text193;
		this.Remove_Payjoy.Click += new System.EventHandler(ADBHandler);
		this.Reset_lock.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Reset_lock.Location = new System.Drawing.Point(3, 394);
		this.Reset_lock.Name = name410;
		this.Reset_lock.Size = new System.Drawing.Size(90, 28);
		this.Reset_lock.TabIndex = 435;
		this.Reset_lock.Text = text113;
		this.Reset_lock.Click += new System.EventHandler(ADBHandler);
		this.EnableDiagRoot.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EnableDiagRoot.Location = new System.Drawing.Point(291, 428);
		this.EnableDiagRoot.Name = name411;
		this.EnableDiagRoot.Size = new System.Drawing.Size(90, 28);
		this.EnableDiagRoot.TabIndex = 434;
		this.EnableDiagRoot.Text = text194;
		this.EnableDiagRoot.Click += new System.EventHandler(ADBHandler);
		this.EnableDiagOperation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EnableDiagOperation.Location = new System.Drawing.Point(291, 461);
		this.EnableDiagOperation.Name = name412;
		this.EnableDiagOperation.Size = new System.Drawing.Size(90, 28);
		this.EnableDiagOperation.TabIndex = 433;
		this.EnableDiagOperation.Text = text195;
		this.EnableDiagOperation.Click += new System.EventHandler(ADBHandler);
		this.DisablePengecualian.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DisablePengecualian.Location = new System.Drawing.Point(670, 394);
		this.DisablePengecualian.Name = name413;
		this.DisablePengecualian.Size = new System.Drawing.Size(138, 28);
		this.DisablePengecualian.TabIndex = 432;
		this.DisablePengecualian.Text = text196;
		this.DisablePengecualian.Click += new System.EventHandler(ADBHandler);
		this.Disable_OS_Sam.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Disable_OS_Sam.Location = new System.Drawing.Point(99, 428);
		this.Disable_OS_Sam.Name = name414;
		this.Disable_OS_Sam.Size = new System.Drawing.Size(90, 28);
		this.Disable_OS_Sam.TabIndex = 431;
		this.Disable_OS_Sam.Text = text197;
		this.Disable_OS_Sam.Click += new System.EventHandler(ADBHandler);
		this.Disable_OSMI.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Disable_OSMI.Location = new System.Drawing.Point(576, 427);
		this.Disable_OSMI.Name = name415;
		this.Disable_OSMI.Size = new System.Drawing.Size(90, 28);
		this.Disable_OSMI.TabIndex = 430;
		this.Disable_OSMI.Text = text198;
		this.Disable_OSMI.Click += new System.EventHandler(ADBHandler);
		this.Disaba_OSOppo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Disaba_OSOppo.Location = new System.Drawing.Point(195, 394);
		this.Disaba_OSOppo.Name = name416;
		this.Disaba_OSOppo.Size = new System.Drawing.Size(90, 28);
		this.Disaba_OSOppo.TabIndex = 429;
		this.Disaba_OSOppo.Text = text199;
		this.Disaba_OSOppo.Click += new System.EventHandler(ADBHandler);
		this.Disable_osvivo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.Disable_osvivo.Location = new System.Drawing.Point(387, 394);
		this.Disable_osvivo.Name = name417;
		this.Disable_osvivo.Size = new System.Drawing.Size(90, 28);
		this.Disable_osvivo.TabIndex = 428;
		this.Disable_osvivo.Text = text200;
		this.Disable_osvivo.Click += new System.EventHandler(ADBHandler);
		this.EnablediagXiaomi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EnablediagXiaomi.Location = new System.Drawing.Point(291, 394);
		this.EnablediagXiaomi.Name = name418;
		this.EnablediagXiaomi.Size = new System.Drawing.Size(90, 28);
		this.EnablediagXiaomi.TabIndex = 427;
		this.EnablediagXiaomi.Text = text201;
		this.EnablediagXiaomi.Click += new System.EventHandler(ADBHandler);
		this.FixAssistant.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.FixAssistant.Location = new System.Drawing.Point(387, 462);
		this.FixAssistant.Name = name419;
		this.FixAssistant.Size = new System.Drawing.Size(90, 28);
		this.FixAssistant.TabIndex = 426;
		this.FixAssistant.Text = text202;
		this.FixAssistant.Click += new System.EventHandler(ADBHandler);
		this.DidableMicloud.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DidableMicloud.Location = new System.Drawing.Point(195, 462);
		this.DidableMicloud.Name = name420;
		this.DidableMicloud.Size = new System.Drawing.Size(90, 28);
		this.DidableMicloud.TabIndex = 425;
		this.DidableMicloud.Text = text203;
		this.DidableMicloud.Click += new System.EventHandler(ADBHandler);
		this.SamsungID.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SamsungID.Location = new System.Drawing.Point(99, 394);
		this.SamsungID.Name = name421;
		this.SamsungID.Size = new System.Drawing.Size(90, 28);
		this.SamsungID.TabIndex = 424;
		this.SamsungID.Text = text204;
		this.SamsungID.Click += new System.EventHandler(ADBHandler);
		this.EraseFRP16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EraseFRP16.Location = new System.Drawing.Point(3, 462);
		this.EraseFRP16.Name = name422;
		this.EraseFRP16.Size = new System.Drawing.Size(90, 28);
		this.EraseFRP16.TabIndex = 423;
		this.EraseFRP16.Text = text205;
		this.EraseFRP16.Click += new System.EventHandler(ADBHandler);
		this.EraseFRP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.EraseFRP.Location = new System.Drawing.Point(3, 428);
		this.EraseFRP.Name = name423;
		this.EraseFRP.Size = new System.Drawing.Size(90, 28);
		this.EraseFRP.TabIndex = 422;
		this.EraseFRP.Text = text206;
		this.EraseFRP.Click += new System.EventHandler(ADBHandler);
		this.ADBEnableApk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ADBEnableApk.ImageOptions.Image = AMFTool.Properties.Resources.WRITE;
		this.ADBEnableApk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ADBEnableApk.Location = new System.Drawing.Point(141, 360);
		this.ADBEnableApk.Name = name424;
		this.ADBEnableApk.Size = new System.Drawing.Size(135, 28);
		this.ADBEnableApk.TabIndex = 421;
		this.ADBEnableApk.Text = text207;
		this.ADBEnableApk.Click += new System.EventHandler(ADBHandler);
		this.ADBInstallApk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ADBInstallApk.ImageOptions.Image = AMFTool.Properties.Resources.backup;
		this.ADBInstallApk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ADBInstallApk.Location = new System.Drawing.Point(429, 360);
		this.ADBInstallApk.Name = name425;
		this.ADBInstallApk.Size = new System.Drawing.Size(135, 28);
		this.ADBInstallApk.TabIndex = 420;
		this.ADBInstallApk.Text = text208;
		this.ADBInstallApk.Click += new System.EventHandler(ADBHandler);
		this.ADBUninstallApk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ADBUninstallApk.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.ADBUninstallApk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ADBUninstallApk.Location = new System.Drawing.Point(282, 360);
		this.ADBUninstallApk.Name = name426;
		this.ADBUninstallApk.Size = new System.Drawing.Size(135, 28);
		this.ADBUninstallApk.TabIndex = 419;
		this.ADBUninstallApk.Text = text209;
		this.ADBUninstallApk.Click += new System.EventHandler(ADBHandler);
		this.ADBDisableApk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ADBDisableApk.ImageOptions.Image = AMFTool.Properties.Resources.DATAERASE;
		this.ADBDisableApk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ADBDisableApk.Location = new System.Drawing.Point(2, 360);
		this.ADBDisableApk.Name = name427;
		this.ADBDisableApk.Size = new System.Drawing.Size(137, 28);
		this.ADBDisableApk.TabIndex = 418;
		this.ADBDisableApk.Text = text210;
		this.ADBDisableApk.Click += new System.EventHandler(ADBHandler);
		this.ADBLoadApk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.ADBLoadApk.ImageOptions.Image = AMFTool.Properties.Resources.GPT_;
		this.ADBLoadApk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.ADBLoadApk.Location = new System.Drawing.Point(718, 360);
		this.ADBLoadApk.Name = name428;
		this.ADBLoadApk.Size = new System.Drawing.Size(90, 28);
		this.ADBLoadApk.TabIndex = 417;
		this.ADBLoadApk.Text = text211;
		this.ADBLoadApk.Click += new System.EventHandler(ADBHandler);
		this.xtraTabPage1.Controls.Add(this.SDresetLock);
		this.xtraTabPage1.Controls.Add(this.SDdisabaleMI);
		this.xtraTabPage1.Controls.Add(this.SDresetMI);
		this.xtraTabPage1.Controls.Add(this.ResetFRPMIasis);
		this.xtraTabPage1.Name = name429;
		this.xtraTabPage1.Size = new System.Drawing.Size(811, 526);
		this.xtraTabPage1.Text = text212;
		this.SDresetLock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SDresetLock.ImageOptions.Image = AMFTool.Properties.Resources.FACTORY;
		this.SDresetLock.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
		this.SDresetLock.Location = new System.Drawing.Point(38, 11);
		this.SDresetLock.Name = name430;
		this.SDresetLock.Size = new System.Drawing.Size(706, 28);
		this.SDresetLock.TabIndex = 430;
		this.SDresetLock.Text = text23;
		this.SDresetLock.Click += new System.EventHandler(SideloadHandler);
		this.SDdisabaleMI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SDdisabaleMI.ImageOptions.Image = AMFTool.Properties.Resources.disable_micloud;
		this.SDdisabaleMI.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
		this.SDdisabaleMI.Location = new System.Drawing.Point(38, 114);
		this.SDdisabaleMI.Name = name431;
		this.SDdisabaleMI.Size = new System.Drawing.Size(706, 28);
		this.SDdisabaleMI.TabIndex = 429;
		this.SDdisabaleMI.Text = text213;
		this.SDdisabaleMI.Click += new System.EventHandler(SideloadHandler);
		this.SDresetMI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SDresetMI.ImageOptions.Image = AMFTool.Properties.Resources.reset_mcloud;
		this.SDresetMI.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
		this.SDresetMI.Location = new System.Drawing.Point(38, 79);
		this.SDresetMI.Name = name432;
		this.SDresetMI.Size = new System.Drawing.Size(706, 28);
		this.SDresetMI.TabIndex = 428;
		this.SDresetMI.Text = text214;
		this.SDresetMI.Click += new System.EventHandler(SideloadHandler);
		this.ResetFRPMIasis.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ResetFRPMIasis.ImageOptions.Image = AMFTool.Properties.Resources.ERASE;
		this.ResetFRPMIasis.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
		this.ResetFRPMIasis.Location = new System.Drawing.Point(38, 45);
		this.ResetFRPMIasis.Name = name433;
		this.ResetFRPMIasis.Size = new System.Drawing.Size(706, 28);
		this.ResetFRPMIasis.TabIndex = 427;
		this.ResetFRPMIasis.Text = text206;
		this.ResetFRPMIasis.Click += new System.EventHandler(SideloadHandler);
		this.TabNVEditor.Controls.Add(this.NVRAMEdit);
		this.TabNVEditor.Controls.Add(this.NVDataEdit);
		this.TabNVEditor.Name = name434;
		this.TabNVEditor.Size = new System.Drawing.Size(813, 557);
		this.TabNVEditor.Text = text215;
		this.NVRAMEdit.Location = new System.Drawing.Point(85, 72);
		this.NVRAMEdit.Name = name435;
		this.NVRAMEdit.Size = new System.Drawing.Size(663, 23);
		this.NVRAMEdit.TabIndex = 1;
		this.NVRAMEdit.Text = text216;
		this.NVRAMEdit.Click += new System.EventHandler(ExtraHandler);
		this.NVDataEdit.Location = new System.Drawing.Point(85, 43);
		this.NVDataEdit.Name = name436;
		this.NVDataEdit.Size = new System.Drawing.Size(663, 23);
		this.NVDataEdit.TabIndex = 0;
		this.NVDataEdit.Text = text217;
		this.NVDataEdit.Click += new System.EventHandler(ExtraHandler);
		this.dataGridViewCheckBoxColumn11.FillWeight = field_11;
		this.dataGridViewCheckBoxColumn11.HeaderText = text;
		this.dataGridViewCheckBoxColumn11.Name = name437;
		this.dataGridViewTextBoxColumn55.FillWeight = field_12;
		this.dataGridViewTextBoxColumn55.HeaderText = headerText9;
		this.dataGridViewTextBoxColumn55.Name = name438;
		this.dataGridViewTextBoxColumn56.FillWeight = field_13;
		this.dataGridViewTextBoxColumn56.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn56.Name = name439;
		this.dataGridViewTextBoxColumn57.FillWeight = field_13;
		this.dataGridViewTextBoxColumn57.HeaderText = headerText4;
		this.dataGridViewTextBoxColumn57.Name = name440;
		this.dataGridViewTextBoxColumn58.FillWeight = field_13;
		this.dataGridViewTextBoxColumn58.HeaderText = headerText5;
		this.dataGridViewTextBoxColumn58.Name = name441;
		this.dataGridViewTextBoxColumn59.FillWeight = field_14;
		this.dataGridViewTextBoxColumn59.HeaderText = headerText6;
		this.dataGridViewTextBoxColumn59.Name = name442;
		this.dataGridViewTextBoxColumn60.FillWeight = field_15;
		this.dataGridViewTextBoxColumn60.HeaderText = headerText7;
		this.dataGridViewTextBoxColumn60.Name = name443;
		this.dataGridViewTextBoxColumn61.FillWeight = field_8;
		this.dataGridViewTextBoxColumn61.HeaderText = headerText8;
		this.dataGridViewTextBoxColumn61.Name = name444;
		this.dataGridViewTextBoxColumn61.Visible = false;
		this.LSpeed.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.LSpeed.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.LSpeed.Appearance.Font = new System.Drawing.Font(familyName, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LSpeed.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.LSpeed.Appearance.Options.UseBackColor = true;
		this.LSpeed.Appearance.Options.UseFont = true;
		this.LSpeed.Appearance.Options.UseForeColor = true;
		this.LSpeed.Location = new System.Drawing.Point(8, 625);
		this.LSpeed.Name = name445;
		this.LSpeed.Size = new System.Drawing.Size(258, 14);
		this.LSpeed.TabIndex = 9;
		this.LSpeed.Text = text218;
		this.ComboPort.EditValue = editValue4;
		this.ComboPort.Location = new System.Drawing.Point(134, 17);
		this.ComboPort.Name = name446;
		this.ComboPort.Properties.Appearance.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ComboPort.Properties.Appearance.Options.UseFont = true;
		this.ComboPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.ComboPort.Properties.ContextImageOptions.Image = (System.Drawing.Image)resources.GetObject(name447);
		this.ComboPort.Size = new System.Drawing.Size(294, 20);
		this.ComboPort.TabIndex = 10;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.panel1.Controls.Add(this.expred);
		this.panel1.Controls.Add(this.Expired);
		this.panel1.Controls.Add(this.labelControl5);
		this.panel1.Controls.Add(this.PanelEMAIL);
		this.panel1.Controls.Add(this.PBar);
		this.panel1.Controls.Add(this.panelControl2);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = name448;
		this.panel1.Size = new System.Drawing.Size(1272, 646);
		this.panel1.TabIndex = 7;
		this.expred.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.expred.Location = new System.Drawing.Point(1140, 626);
		this.expred.Name = name449;
		this.expred.Size = new System.Drawing.Size(43, 13);
		this.expred.TabIndex = 485;
		this.expred.Text = text219;
		this.Expired.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.Expired.Location = new System.Drawing.Point(1189, 626);
		this.Expired.Name = name450;
		this.Expired.Size = new System.Drawing.Size(12, 13);
		this.Expired.TabIndex = 484;
		this.Expired.Text = text220;
		this.labelControl5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.labelControl5.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.labelControl5.Appearance.Font = new System.Drawing.Font(familyName, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.labelControl5.Appearance.Options.UseBackColor = true;
		this.labelControl5.Appearance.Options.UseFont = true;
		this.labelControl5.Appearance.Options.UseForeColor = true;
		this.labelControl5.Location = new System.Drawing.Point(866, 625);
		this.labelControl5.Name = name451;
		this.labelControl5.Size = new System.Drawing.Size(35, 14);
		this.labelControl5.TabIndex = 483;
		this.labelControl5.Text = text221;
		this.PanelEMAIL.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.PanelEMAIL.Appearance.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.PanelEMAIL.Appearance.Font = new System.Drawing.Font(familyName, field_19, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PanelEMAIL.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.PanelEMAIL.Appearance.Options.UseBackColor = true;
		this.PanelEMAIL.Appearance.Options.UseFont = true;
		this.PanelEMAIL.Appearance.Options.UseForeColor = true;
		this.PanelEMAIL.Location = new System.Drawing.Point(907, 625);
		this.PanelEMAIL.Name = name452;
		this.PanelEMAIL.Size = new System.Drawing.Size(12, 14);
		this.PanelEMAIL.TabIndex = 482;
		this.PanelEMAIL.Text = text220;
		this.panelControl2.Controls.Add(this.panel3);
		this.panelControl2.Location = new System.Drawing.Point(6, 3);
		this.panelControl2.Name = name453;
		this.panelControl2.Size = new System.Drawing.Size(445, 63);
		this.panelControl2.TabIndex = 4;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.panel3.Controls.Add(this.device);
		this.panel3.Controls.Add(this.simpleButton4);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.ComboPort);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(2, 2);
		this.panel3.Name = name454;
		this.panel3.Size = new System.Drawing.Size(441, 59);
		this.panel3.TabIndex = 0;
		this.device.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.device.FlatAppearance.BorderSize = 0;
		this.device.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.device.Image = (System.Drawing.Image)resources.GetObject(name455);
		this.device.Location = new System.Drawing.Point(65, 18);
		this.device.Name = name456;
		this.device.Size = new System.Drawing.Size(22, 18);
		this.device.TabIndex = 467;
		this.device.UseVisualStyleBackColor = false;
		this.device.Click += new System.EventHandler(device_Click);
		this.simpleButton4.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
		this.simpleButton4.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name457);
		this.simpleButton4.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.simpleButton4.Location = new System.Drawing.Point(15, 12);
		this.simpleButton4.Name = name458;
		this.simpleButton4.Size = new System.Drawing.Size(37, 32);
		this.simpleButton4.TabIndex = 11;
		this.simpleButton4.Click += new System.EventHandler(simpleButton4_Click);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.label1.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label1.Location = new System.Drawing.Point(93, 21);
		this.label1.Name = name459;
		this.label1.Size = new System.Drawing.Size(35, 13);
		this.label1.TabIndex = 4;
		this.label1.Text = text222;
		this.panel2.Controls.Add(this.TabMenu);
		this.panel2.Controls.Add(this.LSpeed);
		this.panel2.Controls.Add(this.STOP);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = name460;
		this.panel2.Size = new System.Drawing.Size(1272, 646);
		this.panel2.TabIndex = 4;
		this.Timer_Sidebar_Menu.Tick += new System.EventHandler(Timer_Sidebar_Menu_Tick);
		this.bar1.BarName = text223;
		this.bar1.DockCol = 0;
		this.bar1.DockRow = 0;
		this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
		this.bar1.Text = text223;
		this.PopMTK.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[6]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.MTKWrite),
			new DevExpress.XtraBars.LinkPersistInfo(this.MTKRead),
			new DevExpress.XtraBars.LinkPersistInfo(this.MTKFormat),
			new DevExpress.XtraBars.LinkPersistInfo(this.WriteCheck, true),
			new DevExpress.XtraBars.LinkPersistInfo(this.ReadPartition),
			new DevExpress.XtraBars.LinkPersistInfo(this.FormatCheck)
		});
		this.PopMTK.Manager = this.barManager1;
		this.PopMTK.Name = name461;
		this.PopQc.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[3]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.EDLFormat),
			new DevExpress.XtraBars.LinkPersistInfo(this.EDLWrite),
			new DevExpress.XtraBars.LinkPersistInfo(this.EDLRead)
		});
		this.PopQc.Manager = this.barManager1;
		this.PopQc.Name = name462;
		this.PopFB.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.FBWrite),
			new DevExpress.XtraBars.LinkPersistInfo(this.FBFormat)
		});
		this.PopFB.Manager = this.barManager1;
		this.PopFB.Name = name463;
		this.PopSPD.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[3]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.SPDWrite),
			new DevExpress.XtraBars.LinkPersistInfo(this.SPDFormat),
			new DevExpress.XtraBars.LinkPersistInfo(this.SPDRead)
		});
		this.PopSPD.Manager = this.barManager1;
		this.PopSPD.Name = name464;
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_20, field_21);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1272, 646);
		base.Controls.Add(this.PLog);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
		((DevExpress.Utils.ImageOptions)this.method_03()).Image = AMFTool.Properties.Resources.logoamftool;
		base.Name = name465;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text224;
		base.Shown += new System.EventHandler(Main_Shown);
		((System.ComponentModel.ISupportInitialize)this.PLog).EndInit();
		this.PLog.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.SideBar).EndInit();
		this.SideBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PBar.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.TabMenu).EndInit();
		this.TabMenu.ResumeLayout(false);
		this.TabEDL.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TabSubEDL).EndInit();
		this.TabSubEDL.ResumeLayout(false);
		this.TabEDLFlashing.ResumeLayout(false);
		this.TabEDLFlashing.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ShowALLqc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLinfofirmware.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLDataGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLLoader.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLStorageMode.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLFirmware.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLPatch.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CEDLServer.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLModel.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.EDLBrand.Properties).EndInit();
		this.TabEDLNetwork.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.groupControl2).EndInit();
		this.groupControl2.ResumeLayout(false);
		this.Unlock.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.BrandUEDL.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PBUEDL).EndInit();
		((System.ComponentModel.ISupportInitialize)this.SearchMEDL.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.LMEDL).EndInit();
		this.TabMTK.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TabSubMTK).EndInit();
		this.TabSubMTK.ResumeLayout(false);
		this.TabMTKFlashUnlock.ResumeLayout(false);
		this.TabMTKFlashUnlock.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.InfoFW.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ShowALLMTK.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CarbonaraExploit.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.DaCustum.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKServer.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKModel.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKBrand.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKDA.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CMTKEMI.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKDA.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKEMI.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKRepartition.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Highspeed.Properties).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.LayoutStore.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.SuperMap.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKFlashMode.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKFirmware.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MTKDataGrid).EndInit();
		this.TabSpd.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TabSubSpreadtrum).EndInit();
		this.TabSubSpreadtrum.ResumeLayout(false);
		this.TabSpdFlash.ResumeLayout(false);
		this.TabSpdFlash.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.SPDDataGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.checkEdit2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Skippacth.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CModelSPD.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CBrandSPD.Properties).EndInit();
		this.SPDATdiag.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.groupControl3).EndInit();
		this.groupControl3.ResumeLayout(false);
		this.TabHisi.ResumeLayout(false);
		this.TabHisi.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CCPUHisi.Properties).EndInit();
		this.TabMeta.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.Transsion.ResumeLayout(false);
		this.Transsion.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.METADataGrid).EndInit();
		this.Andproid.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.groupControl4).EndInit();
		this.groupControl4.ResumeLayout(false);
		this.TabSamsung.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TabSubSamsung).EndInit();
		this.TabSubSamsung.ResumeLayout(false);
		this.TabSamMTP.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GADBMode).EndInit();
		this.GADBMode.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GMTPMode).EndInit();
		this.GMTPMode.ResumeLayout(false);
		this.TabSamsOdin.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.groupControl1).EndInit();
		this.groupControl1.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.OdinGridView).EndInit();
		((System.ComponentModel.ISupportInitialize)this.COdinEraseEFS.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.COdinRepartition.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtHOME.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCSC.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCP.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtAP.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtBL.Properties).EndInit();
		this.TabAdbFastboot.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage4.ResumeLayout(false);
		this.xtraTabPage4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.FBinfofir.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.FastbootDataGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.FastbootFirmware.Properties).EndInit();
		this.xtraTabPage3.ResumeLayout(false);
		this.xtraTabPage3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.AndroidDataGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtSearch.Properties).EndInit();
		this.xtraTabPage1.ResumeLayout(false);
		this.TabNVEditor.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ComboPort.Properties).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
		this.panelControl2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.behaviorManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PopMTK).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PopQc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PopFB).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PopSPD).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public IntPtr method_02()
	{
		return base.Handle;
	}

	public object method_03()
	{
		return base.IconOptions;
	}
}
