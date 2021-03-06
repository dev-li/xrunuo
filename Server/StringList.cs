﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Server
{
	public class StringList
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static StringList Localization { get; }

		static StringList()
		{
			Localization = new StringList();
		}

		public StringEntry[] Entries { get; }

		public Dictionary<int, string> Table { get; }

		public string Language { get; }

		public string this[int number]
		{
			get
			{
				if ( Table.ContainsKey( number ) )
					return Table[number];
				else
					return null;
			}
		}

		public StringList()
			: this( "enu" )
		{
		}

		public StringList( string language )
			: this( language, true )
		{
		}

		public StringList( string language, bool format )
		{
			Language = language;
			Table = new Dictionary<int, string>();

			string path = Core.FindDataFile( string.Format( "Cliloc.{0}", language ) );

			if ( path == null )
			{
				log.Warning( "Cliloc.{0} not found", language );
				Entries = new StringEntry[0];
				return;
			}

			log.Info( "Loading localization strings");

			List<StringEntry> list = new List<StringEntry>();

			using ( BinaryReader bin = new BinaryReader( new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
			{
				byte[] buffer = new byte[1024];

				bin.ReadInt32();
				bin.ReadInt16();

				while ( bin.BaseStream.Length != bin.BaseStream.Position )
				{
					int number = bin.ReadInt32();
					bin.ReadByte();
					int length = bin.ReadInt16();

					if ( length > buffer.Length )
						buffer = new byte[( length + 1023 ) & ~1023];

					bin.Read( buffer, 0, length );
					string text = Encoding.UTF8.GetString( buffer, 0, length );

					if ( format )
						text = FormatArguments( text );

					list.Add( new StringEntry( number, text ) );
					Table[number] = text;
				}
			}

			Entries = list.ToArray();
		}

		//C# argument support
		public static Regex FormatExpression = new Regex( @"~(\d)+_.*?~", RegexOptions.IgnoreCase );

		public static string MatchComparison( Match m )
		{
			return "{" + ( Utility.ToInt32( m.Groups[1].Value ) - 1 ) + "}";
		}

		public static string FormatArguments( string entry )
		{
			return FormatExpression.Replace( entry, new MatchEvaluator( MatchComparison ) );
		}

		//UO tabbed argument conversion
		public static string CombineArguments( string str, string args )
		{
			if ( string.IsNullOrEmpty( args ) )
				return str;
			else
				return CombineArguments( str, args.Split( new char[] { '\t' } ) );
		}

		public static string CombineArguments( string str, params object[] args )
		{
			return String.Format( str, args );
		}

		public static string CombineArguments( int number, string args )
		{
			return CombineArguments( number, args.Split( new char[] { '\t' } ) );
		}

		public static string CombineArguments( int number, params object[] args )
		{
			return String.Format( StringList.Localization[number], args );
		}
	}

	public class StringEntry
	{
		public int Number { get; private set; }
		public string Text { get; private set; }

		public StringEntry( int number, string text )
		{
			Number = number;
			Text = text;
		}
	}
}
