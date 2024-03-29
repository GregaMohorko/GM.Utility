﻿/*
MIT License

Copyright (c) 2023 Gregor Mohorko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Project: GM.Utility.Test
Created: 2018-12-13
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace GM.Utility;

/// <summary>
/// Utilities for comma separated values.
/// <para>Tab and semicolon separated values are also supported.</para>
/// <para>These methods are useful when some values contain the comma and are surrounded with double quotes. This class respects this, otherwise a simple <see cref="string.Split(char[])"/> would be enough.</para>
/// </summary>
public static class CsvUtility
{
	/// <summary>
	/// Represents the delimiter between data segments in a line.
	/// </summary>
	public enum LineSeparator
	{
		/// <summary>
		/// Unknown separator.
		/// </summary>
		Unknown,
		/// <summary>
		/// Comma (,).
		/// </summary>
		Comma,
		/// <summary>
		/// Tab (\t).
		/// </summary>
		Tab,
		/// <summary>
		/// Semicolon (;).
		/// </summary>
		Semicolon
	}

	/// <summary>
	/// An array containing "\r\n" and "\n".
	/// </summary>
	public static readonly string[] NEWLINES = { "\r\n", "\n" };

	private static readonly ReadOnlyDictionary<LineSeparator, Regex> separatorToRegex;

	static CsvUtility()
	{
		// CSV line parsing: From "jgr4" in http://www.kimgentes.com/worshiptech-web-tools-page/2008/10/14/regex-pattern-for-parsing-csv-files-with-embedded-commas-dou.html
		separatorToRegex = new Dictionary<LineSeparator, Regex>
		{
			{ LineSeparator.Unknown, null },
			{ LineSeparator.Comma, new Regex(@"\s?((?<x>(?=[,]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^,]+)),?", RegexOptions.ExplicitCapture | RegexOptions.Compiled) },
			{ LineSeparator.Tab, new Regex(@"[\r\n\f\v ]*?((?<x>(?=[\t]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^\t]+))\t?", RegexOptions.ExplicitCapture | RegexOptions.Compiled) },
			{ LineSeparator.Semicolon, new Regex(@"\s?((?<x>(?=[;]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^;]+));?", RegexOptions.ExplicitCapture | RegexOptions.Compiled) }
		}.AsReadOnly();
	}

	/// <summary>
	/// Parses the provided text.
	/// <para>The line separator is determined by the max median of occurences in the entire text.</para>
	/// </summary>
	/// <param name="text">The text to parse. Can be multi-line.</param>
	public static IEnumerable<string[]> Parse(string text)
	{
		return Parse(text, LineSeparator.Unknown);
	}

	/// <summary>
	/// Parses the provided text of comma separated values.
	/// </summary>
	/// <param name="text">The text to parse. Can be multi-line.</param>
	public static IEnumerable<string[]> ParseCommaSeparated(string text)
	{
		return Parse(text, LineSeparator.Comma);
	}

	/// <summary>
	/// Parses the provided text of tab separated values.
	/// </summary>
	/// <param name="text">The text to parse. Can be multi-line.</param>
	public static IEnumerable<string[]> ParseTabSeparated(string text)
	{
		return Parse(text, LineSeparator.Tab);
	}

	/// <summary>
	/// Parses the provided text of semicolon separated values.
	/// </summary>
	/// <param name="text">The text to parse. Can be multi-line.</param>
	public static IEnumerable<string[]> ParseSemicolonSeparated(string text)
	{
		return Parse(text, LineSeparator.Semicolon);
	}

	/// <summary>
	/// Parses the provided text with the specified line separator.
	/// <para>If the line separator is unknown, it is determined by the max median of occurences in the entire text.</para>
	/// </summary>
	/// <param name="text">The text to parse. Can be multi-line.</param>
	/// <param name="lineSeparator">The type of delimiter between data segments in a line.</param>
	public static IEnumerable<string[]> Parse(string text, LineSeparator lineSeparator)
	{
		string[] lines = text.Split(NEWLINES, StringSplitOptions.None);
		return Parse(lines, lineSeparator);
	}

	/// <summary>
	/// Parses the provided lines of text.
	/// <para>The line separator is determined by the max median of occurences in all of the lines.</para>
	/// </summary>
	/// <param name="lines">Lines of text to parse.</param>
	public static IEnumerable<string[]> Parse(IEnumerable<string> lines)
	{
		return ParseUnknown(lines);
	}

	/// <summary>
	/// Parses the providede lines of comma separated values.
	/// </summary>
	/// <param name="lines">Lines of comma separated values to parse.</param>
	public static IEnumerable<string[]> ParseCommaSeparated(IEnumerable<string> lines)
	{
		return ParseKnown(lines, LineSeparator.Comma);
	}

	/// <summary>
	/// Parses the providede lines of tab separated values.
	/// </summary>
	/// <param name="lines">Lines of tab separated values to parse.</param>
	public static IEnumerable<string[]> ParseTabSeparated(IEnumerable<string> lines)
	{
		return ParseKnown(lines, LineSeparator.Tab);
	}

	/// <summary>
	/// Parses the providede lines of semicolon separated values.
	/// </summary>
	/// <param name="lines">Lines of semicolon separated values to parse.</param>
	public static IEnumerable<string[]> ParseSemicolonSeparated(IEnumerable<string> lines)
	{
		return ParseKnown(lines, LineSeparator.Semicolon);
	}

	/// <summary>
	/// Parses the provided lines of text with the specified line separator.
	/// <para>If the line separator is unknown, it is determined by the max median of occurences in all of the lines.</para>
	/// </summary>
	/// <param name="lines">Lines of text to parse.</param>
	/// <param name="lineSeparator">The type of delimiter between data segments in a line.</param>
	public static IEnumerable<string[]> Parse(IEnumerable<string> lines, LineSeparator lineSeparator)
	{
		if(lineSeparator == LineSeparator.Unknown) {
			return ParseUnknown(lines);
		}
		return ParseKnown(lines, lineSeparator);
	}

	/// <summary>
	/// Parses the provided line.
	/// <para>The line separator is determined by the max count.</para>
	/// </summary>
	/// <param name="line">The line of text to parse.</param>
	public static string[] ParseLine(string line)
	{
		return ParseLineUnknown(line);
	}

	/// <summary>
	/// Parses the provided line of comma separated values.
	/// </summary>
	/// <param name="line">The line of text to parse.</param>
	public static string[] ParseLineCommaSeparated(string line)
	{
		return ParseLineKnown(line, LineSeparator.Comma);
	}

	/// <summary>
	/// Parses the provided line of tab separated values.
	/// </summary>
	/// <param name="line">The line of text to parse.</param>
	public static string[] ParseLineTabSeparated(string line)
	{
		return ParseLineKnown(line, LineSeparator.Tab);
	}

	/// <summary>
	/// Parses the provided line of semicolon separated values.
	/// </summary>
	/// <param name="line">The line of text to parse.</param>
	public static string[] ParseLineSemicolonSeparated(string line)
	{
		return ParseLineKnown(line, LineSeparator.Semicolon);
	}

	/// <summary>
	/// Parses the provided line separated with the specified line separator.
	/// <para>If the line separator is unknown, it is set to the one with the max count.</para>
	/// </summary>
	/// <param name="line">The line of text to parse.</param>
	/// <param name="lineSeparator">The type of delimiter between data segments in a line.</param>
	public static string[] ParseLine(string line, LineSeparator lineSeparator)
	{
		if(lineSeparator == LineSeparator.Unknown) {
			return ParseLineUnknown(line);
		}
		return ParseLineKnown(line, lineSeparator);
	}

	private static IEnumerable<string[]> ParseKnown(IEnumerable<string> lines, LineSeparator lineSeparator)
	{
		Regex regex = separatorToRegex[lineSeparator];
		foreach(string line in lines) {
			yield return ParseLine(line, regex);
		}
	}

	private static List<string[]> ParseUnknown(IEnumerable<string> lines)
	{
		// try to guess by checking the max median of occurences of all supported separators in all lines
		List<string[]> resultByComma = ParseKnown(lines, LineSeparator.Comma).ToList();
		List<string[]> resultByTab = ParseKnown(lines, LineSeparator.Tab).ToList();
		List<string[]> resultBySemicolon = ParseKnown(lines, LineSeparator.Semicolon).ToList();

		double median(List<string[]> values)
		{
			return Math.Round(StatisticUtility.FindMedianValue(values.Select(s => s.Length)));
		}
		int range(List<string[]> values)
		{
			return StatisticUtility.CalculateRange(values.Select(s => s.Length));
		}

		return Util.SelectThoseWithMax(median,
			resultByComma,
			resultByTab,
			resultBySemicolon)
			.FirstMin(range);
	}

	private static string[] ParseLineKnown(string line, LineSeparator lineSeparator)
	{
		Regex regex = separatorToRegex[lineSeparator];
		return ParseLine(line, regex);
	}

	private static string[] ParseLineUnknown(string line)
	{
		// try to guess by checking the max count of each supported separator
		string[] resultByComma = ParseLine(line, separatorToRegex[LineSeparator.Comma]);
		string[] resultByTab = ParseLine(line, separatorToRegex[LineSeparator.Tab]);
		string[] resultBySemicolon = ParseLine(line, separatorToRegex[LineSeparator.Semicolon]);
		return Util.SelectOneWithMax(r => r.Length, resultByComma, resultByTab, resultBySemicolon);
	}

	private static string[] ParseLine(string line, Regex regex)
	{
		var matches = regex.Matches(line);

		string[] values = (from Match m in matches
						   select m.Groups["x"].Value
							   .Trim()
							   .Replace("\"\"", "\"")
							).ToArray();

		return values;
	}

	/// <summary>
	/// Returns the provided text data surrounded with double quotes (").
	/// </summary>
	/// <param name="data">The text data.</param>
	/// <param name="onlyIfEmptyOrContainsCommaOrWhitespace">If true, double quotes are added only if data is null/empty or it contains either comma or whitespace.</param>
	public static string SurroundWithDoubleQuotes(string data, bool onlyIfEmptyOrContainsCommaOrWhitespace = false)
	{
		if(!onlyIfEmptyOrContainsCommaOrWhitespace || string.IsNullOrEmpty(data) || (data.Contains(',') || data.ContainsWhitespace())) {
			return $"\"{data}\"";
		}
		return data;
	}

	/// <summary>
	/// Returns the provided text data surrounded with double quotes (") if any of the specified conditions are met.
	/// </summary>
	/// <param name="data">The text data.</param>
	/// <param name="ifEmpty">If true, double quotes are added when data is null/empty.</param>
	/// <param name="ifContainsWhitespace">If true, double quotes are added when data contains any whitespace.</param>
	/// <param name="ifContainsAnyOfTheseCharacters">Double quotes are added then data contains any of these characters (case-sensitive).</param>
	public static string SurroundWithDoubleQuotes(string data, bool ifEmpty, bool ifContainsWhitespace, params char[] ifContainsAnyOfTheseCharacters)
	{
		bool surround = false;
		if(string.IsNullOrEmpty(data)) {
			surround = ifEmpty;
		} else if(data.ContainsWhitespace() && ifContainsWhitespace) {
			surround = true;
		} else {
			foreach(char c in data) {
				if(ifContainsAnyOfTheseCharacters.Contains(c)) {
					surround = true;
					break;
				}
			}
		}
		if(surround) {
			return $"\"{data}\"";
		}
		return data;
	}

	/// <summary>
	/// Parses the provided csv line with the specified custom delimiter with the selected string split options.
	/// <para>Results are trimmed.</para>
	/// </summary>
	/// <param name="csvLine">The csv line.</param>
	/// <param name="comma">The custom delimiter.</param>
	/// <param name="stringSplitOptions"></param>
	/// <exception cref="ArgumentException">Thrown for the csv line when the double quotes do not close properly.</exception>
	public static List<string> ParseLineWithCustomComma(string csvLine, string comma, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
	{
		var commaSeparatedParts = csvLine
			.Split(comma.ToArray(), stringSplitOptions)
			.ToList();

		for(int i = 0; i < commaSeparatedParts.Count; ++i) {
			string part = commaSeparatedParts[i];
			if(part.Length > 0
				&& part[0] == '\"'
				&& part.Last() != '\"'
				) {
				// this is the start of a "..." section where it should not have been splitted
				part = part.Substring(1);
				while(true) {
					if(i + 1 == commaSeparatedParts.Count) {
						throw new ArgumentException($"A csv line starts with double quote but doesn't end anywhere. Problematic CSV line: {csvLine}", nameof(csvLine));
					}
					string nextPart = commaSeparatedParts[i + 1];
					part += comma + nextPart;
					commaSeparatedParts.RemoveAt(i + 1);
					if(nextPart.Length > 0
						&& nextPart.Last() == '\"'
						) {
						// this is the end of "..." section
						commaSeparatedParts[1] = part.Substring(0, part.Length - 1);
						break;
					}
				}
			}
		}

		return commaSeparatedParts
			.Select(part => part.Trim())
			.ToList();
	}
}
