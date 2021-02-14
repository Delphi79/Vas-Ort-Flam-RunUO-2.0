/***********************************************
*
* This script was made by milt, AKA Pokey.
*
* Email: pylon2007@gmail.com
*
* AIM: TrueBornStunna
*
* Website: www.pokey.f13nd.net
*
* Version: 1.0.0
*
* Release Date: June 30, 2006
*
************************************************/
using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CSharp;
using System.Text;
using System.Threading;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using Server;
using Server.Commands;

namespace Server.Misc
{
	public class QuickRestart
	{
		private static Mobile m_Mobile;
		private static bool m_Restarting;

		private static bool m_Debug;
		private static bool m_Service;
		private static bool m_Profiling;
		private static bool m_HaltOnWarning;

		public static void Initialize()
		{
			CommandSystem.Register( "QuickRestart", AccessLevel.Administrator, new CommandEventHandler( QuickRestart_OnCommand ) );
		}

		public static void QuickRestart_OnCommand( CommandEventArgs e )
		{
			string m_Args = e.ArgString.ToLower();

			if( m_Args.IndexOf( "-debug" ) != -1 )
				m_Debug = true;
			if( m_Args.IndexOf( "-service" ) != -1 )
				m_Service = true;
			if( m_Args.IndexOf( "-profile" ) != -1 )
				m_Profiling = true;
			if( m_Args.IndexOf( "-haltonwarning" ) != -1 )
				m_HaltOnWarning = true;

			if ( m_Restarting || AutoRestart.Restarting)
			{
				e.Mobile.SendMessage( "The server is already restarting." );
				return;
			}

			e.Mobile.SendMessage( "Quick restart has been activated. Do NOT use [restart at this time." );

			m_Restarting = true;
			m_Mobile = e.Mobile;
			AutoRestart.Enabled = false;

			new Thread(new ThreadStart(CheckRestart)).Start();
		}

		public static void CheckRestart()
		{
			string[] files = ScriptCompiler.GetScripts( "*.cs" );

			if ( files.Length == 0 )
			{
				m_Mobile.SendMessage("No scripts found to compile!");
				return;
			}

			if(AlreadyCached(files))
			{
				m_Mobile.SendMessage("Scripts are already cached. Restarting...");
				DoRestart(new object[] { false });
				return;
			}

			using ( CSharpCodeProvider provider = new CSharpCodeProvider() )
			{
				string path = GetUnusedPath( "Scripts.CS" );

				CompilerParameters parms = new CompilerParameters( ScriptCompiler.GetReferenceAssemblies(), path, m_Debug );

				string defines = ScriptCompiler.GetDefines();

				if ( defines != null )
					parms.CompilerOptions = string.Format( "/D:{0}", defines );

				m_Mobile.SendMessage("Compiling C# scripts, please wait...");
				World.Broadcast(1154, true, "[ATTENTION]:The server is restarting shortly...");

				CompilerResults results = provider.CompileAssemblyFromFile( parms, files );

				if ( results.Errors.Count > 0 )
				{
					m_Mobile.SendMessage("There were errors in compiling the scripts. QuickRestart can NOT restart.");
					World.Broadcast(1154, true, "[ATTENTION]:Server restart has been aborted.");
					m_Restarting = false;
					return;
				}

				if ( Path.GetFileName( path ) == "Scripts.CS.dll.new" )
				{
					try
					{
						byte[] hashCode = GetHashCode( path, files, false );

						using ( FileStream fs = new FileStream( "Scripts/Output/Scripts.CS.hash.new", FileMode.Create, FileAccess.Write, FileShare.None ) )
						{
							using ( BinaryWriter bin = new BinaryWriter( fs ) )
							{
								bin.Write( hashCode, 0, hashCode.Length );
							}
						}
					}
					catch { }
				}

				m_Mobile.SendMessage("Compilation successful. Restarting in 15 seconds.");
				World.Broadcast(1154, true, "[ATTENTION]:The server will restart in 15 seconds.");
				Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerStateCallback(DoRestart), new object[] { true });
			}
		}

		public static void DoRestart(object state)
		{
			object[] states = (object[])state;
			bool normal = (bool)states[0];

			lock (World.Items.Values)
			{
				lock (World.Mobiles.Values)
				{
					AutoSave.Save();
				}
			}

			if(normal)
			{
				if( File.Exists("Launcher.exe") )
				{
					m_Mobile.SendMessage("Launcher found, restarting...");
					Process.Start("Launcher.exe", Arguments());
					Core.Process.Kill();
				}
			}

			Process.Start( Core.ExePath, Arguments() );
			Core.Process.Kill();
		}

		public static bool AlreadyCached(string [] files)
		{
			if ( File.Exists( "Scripts/Output/Scripts.CS.dll" ) )
			{
				if ( File.Exists( "Scripts/Output/Scripts.CS.hash" ) )
				{
					try
					{
						byte[] hashCode = GetHashCode( "Scripts/Output/Scripts.CS.dll", files, m_Debug );

						using ( FileStream fs = new FileStream( "Scripts/Output/Scripts.CS.hash", FileMode.Open, FileAccess.Read, FileShare.Read ) )
						{
							using ( BinaryReader bin = new BinaryReader( fs ) )
							{
								byte[] bytes = bin.ReadBytes( hashCode.Length );

								if ( bytes.Length == hashCode.Length )
								{
									bool valid = true;

									for ( int i = 0; i < bytes.Length; ++i )
									{
										if ( bytes[i] != hashCode[i] )
										{
											valid = false;
											break;
										}
									}

									if ( valid )
									{
										m_Mobile.SendMessage("The scripts are already cached. Restarting...");
										return true;
									}
								}
							}
						}
					}
					catch
					{
						m_Mobile.SendMessage("Read error. Continuing...");
					}
				}
			}

			return false;
		}

		public static string GetUnusedPath( string name )
		{
			string path = Path.Combine( Core.BaseDirectory, String.Format( "Scripts/Output/{0}.dll.new", name ) );

			for ( int i = 2; File.Exists( path ) && i <= 1000; ++i )
				path = Path.Combine( Core.BaseDirectory, String.Format( "Scripts/Output/{0}.{1}.dll.new", name, i ) );

			return path;
		}

		private static byte[] GetHashCode( string compiledFile, string[] scriptFiles, bool debug )
		{
			using ( MemoryStream ms = new MemoryStream() )
			{
				using ( BinaryWriter bin = new BinaryWriter( ms ) )
				{
					FileInfo fileInfo = new FileInfo( compiledFile );

					bin.Write( fileInfo.LastWriteTimeUtc.Ticks );

					foreach ( string scriptFile in scriptFiles )
					{
						fileInfo = new FileInfo( scriptFile );

						bin.Write( fileInfo.LastWriteTimeUtc.Ticks );
					}

					bin.Write( debug );

					ms.Position = 0;

					using ( System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create() )
					{
						return sha1.ComputeHash( ms );
					}
				}
			}
		}

		private static string Arguments()
		{
			StringBuilder sb = new StringBuilder();
 
			if( m_Debug )
				sb.Append( " -debug" );
			if( m_Service )
				sb.Append( " -service" );
			if( m_Profiling )
				sb.Append( " -profile" );
			if( m_HaltOnWarning )
				sb.Append( " -haltonwarning" );

			return sb.ToString();
		}
	}
}