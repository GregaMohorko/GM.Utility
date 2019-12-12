/*
MIT License

Copyright (c) 2019 Gregor Mohorko

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

Project: GM.Utility
Created: 2017-10-27
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for input and output of any kind.
	/// </summary>
	public static class IOUtility
	{
		/// <summary>
		/// Adds the <see cref="FileAttributes.ReadOnly"/> attribute to the file on the specified path.
		/// <para>Returns false if the specified file was already set as read-only.</para>
		/// </summary>
		/// <param name="path">The path to the file.</param>
		public static bool SetAsReadOnly(string path)
		{
			FileAttributes existing = File.GetAttributes(path);
			if((existing & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) {
				// already set as read-only
				return false;
			}
			FileAttributes newAttributes = existing | FileAttributes.ReadOnly;
			File.SetAttributes(path, newAttributes);
			return true;
		}

		/// <summary>
		/// Removes the <see cref="FileAttributes.ReadOnly"/> attribute from the file on the specified path.
		/// <para>Returns false if the specified file was already not set as read-only.</para>
		/// </summary>
		/// <param name="path">The path to the file.</param>
		public static bool SetAsNotReadOnly(string path)
		{
			FileAttributes existing = File.GetAttributes(path);
			if((existing & FileAttributes.ReadOnly) != FileAttributes.ReadOnly) {
				// already not set as read-only
				return false;
			}
			FileAttributes newAttributes = existing & ~FileAttributes.ReadOnly;
			File.SetAttributes(path, newAttributes);
			return true;
		}

		/// <summary>
		/// Renames the specified directory (adds the specified suffix to the end of the name).
		/// </summary>
		/// <param name="directory">The directory to add the suffix to.</param>
		/// <param name="suffix">The suffix to add to the directory.</param>
		public static void AddSuffix(DirectoryInfo directory, string suffix)
		{
			string newName = directory.Name + suffix;
			string absolutePath = Path.Combine(Path.GetDirectoryName(directory.FullName), newName);
			directory.MoveTo(absolutePath);
		}

		/// <summary>
		/// Renames the specified file (adds the specified suffix to the end of the name).
		/// </summary>
		/// <param name="file">The file to add the suffix to.</param>
		/// <param name="suffix">The suffix to add to the file.</param>
		public static void AddSuffix(FileInfo file, string suffix)
		{
			string newName = file.Name + suffix;
			string absolutePath = Path.Combine(Path.GetDirectoryName(file.FullName), newName);
			file.MoveTo(absolutePath);
		}

		/// <summary>
		/// Adds the specified suffix to all directories and files in the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories and files.</param>
		/// <param name="suffix">The suffix to add to all directories and files.</param>
		public static void AddSuffixToAll(DirectoryInfo directory, string suffix)
		{
			AddSuffixToAll(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Adds the specified suffix to all directories and files in the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories and files.</param>
		/// <param name="suffix">The suffix to add to all directories and files.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void AddSuffixToAll(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			AddSuffixToAll(directory, suffix, searchOption, null);
		}

		/// <summary>
		/// Adds the specified suffix to all directories and files in the specified directory, using the specified search option. If a directory matches any of the values in the provided ignore list, the suffix will not be added to it.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories and files.</param>
		/// <param name="suffix">The suffix to add to all directories and files.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="ignoredDirectories">A collection of directory names to ignore when adding suffix.</param>
		public static void AddSuffixToAll(DirectoryInfo directory, string suffix, SearchOption searchOption, IEnumerable<string> ignoredDirectories)
		{
			AddSuffixToAllFiles(directory, suffix, SearchOption.TopDirectoryOnly);
			AddSuffixToAllDirectories(directory, suffix, SearchOption.TopDirectoryOnly, ignoredDirectories);

			if(searchOption == SearchOption.AllDirectories) {
				DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
				foreach(DirectoryInfo subdirectory in allSubdirectories) {
					AddSuffixToAll(subdirectory, suffix, SearchOption.AllDirectories, ignoredDirectories);
				}
			}
		}

		/// <summary>
		/// Adds the specified suffix to all directories in the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories.</param>
		/// <param name="suffix">The suffix to add to all directories.</param>
		public static void AddSuffixToAllDirectories(DirectoryInfo directory, string suffix)
		{
			AddSuffixToAllDirectories(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Adds the specified suffix to all directories in the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories.</param>
		/// <param name="suffix">The suffix to add to all directories.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void AddSuffixToAllDirectories(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			AddSuffixToAllDirectories(directory, suffix, searchOption, null);
		}

		/// <summary>
		/// Adds the specified suffix to all directories in the specified directory, using the specified search option. If a directory matches any of the values in the provided ignore list, the suffix will not be added to it.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all directories.</param>
		/// <param name="suffix">The suffix to add to all directories.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="ignoredDirectories">A collection of directory names to ignore when adding suffix.</param>
		public static void AddSuffixToAllDirectories(DirectoryInfo directory, string suffix, SearchOption searchOption, IEnumerable<string> ignoredDirectories)
		{
			DirectoryInfo[] allDirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);

			foreach(DirectoryInfo subdirectory in allDirectories) {
				if(ignoredDirectories != null && ignoredDirectories.Contains(subdirectory.Name)) {
					continue;
				}

				if(searchOption == SearchOption.AllDirectories) {
					AddSuffixToAllDirectories(subdirectory, suffix, SearchOption.AllDirectories, ignoredDirectories);
				}
				AddSuffix(subdirectory, suffix);
			}
		}

		/// <summary>
		/// Adds the specified suffix to all files inside the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all files.</param>
		/// <param name="suffix">The suffix to add to all files.</param>
		public static void AddSuffixToAllFiles(DirectoryInfo directory, string suffix)
		{
			AddSuffixToAllFiles(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Adds the specified suffix to all files inside the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to add the suffix to all files.</param>
		/// <param name="suffix">The suffix to add to all files.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void AddSuffixToAllFiles(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			FileInfo[] allFiles = directory.GetFiles("*", searchOption);
			foreach(FileInfo file in allFiles) {
				AddSuffix(file, suffix);
			}
		}

		/// <summary>
		/// Copies the specified source directory to the specified location.
		/// </summary>
		/// <param name="sourceDirectory">The path of the directory to copy.</param>
		/// <param name="destinationDirectory">The path where to copy.</param>
		/// <param name="includeSubdirectories">Determines whether to copy subdirectories.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyDirectory(string sourceDirectory, string destinationDirectory, bool includeSubdirectories = true, bool overwrite = true)
		{
			var directory = new DirectoryInfo(sourceDirectory);
			CopyDirectory(directory, destinationDirectory, includeSubdirectories, overwrite);
		}

		/// <summary>
		/// Copies the specified source directory to the specified location.
		/// </summary>
		/// <param name="sourceDirectory">The directory to copy.</param>
		/// <param name="destinationDirectory">The path where to copy.</param>
		/// <param name="includeSubdirectories">Determines whether to copy subdirectories.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyDirectory(DirectoryInfo sourceDirectory, string destinationDirectory, bool includeSubdirectories = true, bool overwrite = true)
		{
			if(!sourceDirectory.Exists) {
				throw new DirectoryNotFoundException($"Source directory {sourceDirectory.FullName} does not exist or could not be found.");
			}

			if(!Directory.Exists(destinationDirectory)) {
				_ = Directory.CreateDirectory(destinationDirectory);
			}

			// get the files in the top directory and copy them to the new location
			FileInfo[] files = sourceDirectory.GetFiles("*", SearchOption.TopDirectoryOnly);
			foreach(FileInfo file in files) {
				string fileNewPath = Path.Combine(destinationDirectory, file.Name);
				_ = file.CopyTo(fileNewPath, overwrite);
			}

			if(includeSubdirectories) {
				// copy all subdirectories and their contents to the new location
				DirectoryInfo[] subdirectories = sourceDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
				foreach(DirectoryInfo subdirectory in subdirectories) {
					string subdirectoryNewPath = Path.Combine(destinationDirectory, subdirectory.Name);
					CopyDirectory(subdirectory, subdirectoryNewPath, true, overwrite);
				}
			}
		}

		/// <summary>
		/// Copies all files whose name starts with the specified prefix in the specified directory, including subdirectories.
		/// <para>If the destination directory doesn't exist, it is created.</para>
		/// </summary>
		/// <param name="sourceDirectory">The path of the source directory.</param>
		/// <param name="destinationDirectory">The path of the directory where to copy files.</param>
		/// <param name="prefix">The prefix that files must contain in order to be copied.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyAllFilesWithPrefix(string sourceDirectory, string destinationDirectory, string prefix, bool overwrite = true)
		{
			CopyAllFilesWithPrefix(sourceDirectory, destinationDirectory, prefix, SearchOption.AllDirectories, overwrite);
		}

		/// <summary>
		/// Copies all files whose name starts with the specified prefix in the specified directory, using the specified search option.
		/// <para>If the destination directory doesn't exist, it is created.</para>
		/// </summary>
		/// <param name="sourceDirectory">The path of the source directory.</param>
		/// <param name="destinationDirectory">The path of the directory where to copy files.</param>
		/// <param name="prefix">The prefix that files must contain in order to be copied.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyAllFilesWithPrefix(string sourceDirectory, string destinationDirectory, string prefix, SearchOption searchOption, bool overwrite = true)
		{
			DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDirectory);
			CopyAllFilesWithPrefix(sourceDirectoryInfo, destinationDirectory, prefix, searchOption, overwrite);
		}

		/// <summary>
		/// Copies all files whose name starts with the specified prefix in the specified directory, including subdirectories.
		/// <para>If the destination directory doesn't exist, it is created.</para>
		/// </summary>
		/// <param name="sourceDirectory">The source directory.</param>
		/// <param name="destinationDirectory">The path of the directory where to copy files.</param>
		/// <param name="prefix">The prefix that files must contain in order to be copied.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyAllFilesWithPrefix(DirectoryInfo sourceDirectory, string destinationDirectory, string prefix, bool overwrite = true)
		{
			CopyAllFilesWithPrefix(sourceDirectory, destinationDirectory, prefix, SearchOption.AllDirectories, overwrite);
		}

		/// <summary>
		/// Copies all files whose name starts with the specified prefix in the specified directory, using the specified search option.
		/// <para>If the destination directory doesn't exist, it is created.</para>
		/// </summary>
		/// <param name="sourceDirectory">The source directory.</param>
		/// <param name="destinationDirectory">The path of the directory where to copy files.</param>
		/// <param name="prefix">The prefix that files must contain in order to be copied.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false.</param>
		public static void CopyAllFilesWithPrefix(DirectoryInfo sourceDirectory, string destinationDirectory, string prefix, SearchOption searchOption, bool overwrite = true)
		{
			if(!Directory.Exists(destinationDirectory)) {
				_ = Directory.CreateDirectory(destinationDirectory);
			}

			FileInfo[] topFiles = sourceDirectory.GetFiles($"{prefix}*", SearchOption.TopDirectoryOnly);
			foreach(FileInfo topFile in topFiles) {
				string fileNewPath = Path.Combine(destinationDirectory, topFile.Name);
				_ = topFile.CopyTo(fileNewPath, overwrite);
			}

			if(searchOption == SearchOption.AllDirectories) {
				DirectoryInfo[] subdirectories = sourceDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
				foreach(DirectoryInfo subdirectory in subdirectories) {
					string subdirectoryNewPath = Path.Combine(destinationDirectory, subdirectory.Name);
					CopyAllFilesWithPrefix(subdirectory, subdirectoryNewPath, prefix, SearchOption.AllDirectories, overwrite);
				}
			}
		}

		/// <summary>
		/// Deletes all directories in the specified directory, whose name is not ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all directories.</param>
		/// <param name="suffix">The suffix that directories must not contain in order to be deleted.</param>
		public static void DeleteAllDirectoriesWithoutSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllDirectoriesWithoutSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all directories in the specified directory, whose name is not ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all directories.</param>
		/// <param name="suffix">The suffix that directories must not contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllDirectoriesWithoutSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			DeleteAllDirectoriesWithoutSuffix(directory, suffix, searchOption, null);
		}

		/// <summary>
		/// Deletes all directories in the specified directory, whose name is not ended by the specified suffix, using the specified search option. If a directory matches any of the values in the provided ignore list, it will not be deleted.
		/// </summary>
		/// <param name="directory">The directory in which to delete all directories.</param>
		/// <param name="suffix">The suffix that directories must not contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="ignoredDirectories">A collection of directory names to ignore.</param>
		public static void DeleteAllDirectoriesWithoutSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption, IEnumerable<string> ignoredDirectories)
		{
			DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
			for(int i = allSubdirectories.Length - 1; i >= 0; --i) {
				DirectoryInfo subdirectory = allSubdirectories[i];

				if(ignoredDirectories != null && ignoredDirectories.Contains(subdirectory.Name)) {
					continue;
				}

				if(!subdirectory.Name.EndsWith(suffix)) {
					allSubdirectories[i].Delete(true);
					continue;
				}

				if(searchOption == SearchOption.AllDirectories) {
					DeleteAllDirectoriesWithoutSuffix(subdirectory, suffix, SearchOption.AllDirectories, ignoredDirectories);
				}
			}
		}

		/// <summary>
		/// Deletes all directories in the specified directory, whose name is ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all directories.</param>
		/// <param name="suffix">The suffix that directories must contain in order to be deleted.</param>
		public static void DeleteAllDirectoriesWithSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllDirectoriesWithSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all directories in the specified directory, whose name is ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all directories.</param>
		/// <param name="suffix">The suffix that directories must contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllDirectoriesWithSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
			for(int i = allSubdirectories.Length - 1; i >= 0; i--) {
				DirectoryInfo subdirectory = allSubdirectories[i];

				if(subdirectory.Name.EndsWith(suffix)) {
					allSubdirectories[i].Delete(true);
					continue;
				}

				if(searchOption == SearchOption.AllDirectories) {
					DeleteAllDirectoriesWithSuffix(subdirectory, suffix, SearchOption.AllDirectories);
				}
			}
		}

		/// <summary>
		/// Deletes all files in the specified directory, whose name is not ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files.</param>
		/// <param name="suffix">The suffix that files must not contain in order to be deleted.</param>
		public static void DeleteAllFilesWithoutSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllFilesWithoutSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all files in the specified directory, whose name is not ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files.</param>
		/// <param name="suffix">The suffix that files must not contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllFilesWithoutSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			FileInfo[] allFiles = directory.GetFiles("*", searchOption);
			foreach(FileInfo file in allFiles) {
				if(!file.Name.EndsWith(suffix)) {
					file.Delete();
				}
			}
		}

		/// <summary>
		/// Deletes all files in the specified directory, whose name is ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files.</param>
		/// <param name="suffix">The suffix that files must contain in order to be deleted.</param>
		public static void DeleteAllFilesWithSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllFilesWithSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all files in the specified directory, whose name is ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files.</param>
		/// <param name="suffix">The suffix that files must contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllFilesWithSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			FileInfo[] allFiles = directory.GetFiles("*" + suffix, searchOption);
			foreach(FileInfo file in allFiles) {
				file.Delete();
			}
		}

		/// <summary>
		/// Deletes all files and directories in the specified directory, whose name is not ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files and directories.</param>
		/// <param name="suffix">The suffix that files and directories must not contain in order to be deleted.</param>
		public static void DeleteAllWithoutSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllWithoutSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all files and directories in the specified directory, whose name is not ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files and directories.</param>
		/// <param name="suffix">The suffix that files and directories must not contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllWithoutSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			DeleteAllWithoutSuffix(directory, suffix, searchOption, null);
		}

		/// <summary>
		/// Deletes all files and directories in the specified directory, whose name is not ended by the specified suffix, using the specified search option. If a directory matches any of the values in the provided ignore list, it will not be deleted.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files and directories.</param>
		/// <param name="suffix">The suffix that files and directories must not contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		/// <param name="ignoredDirectories">A collection of directory names to ignore.</param>
		public static void DeleteAllWithoutSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption, IEnumerable<string> ignoredDirectories)
		{
			DeleteAllFilesWithoutSuffix(directory, suffix, SearchOption.TopDirectoryOnly);
			DeleteAllDirectoriesWithoutSuffix(directory, suffix, SearchOption.TopDirectoryOnly, ignoredDirectories);

			if(searchOption == SearchOption.AllDirectories) {
				DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);

				foreach(DirectoryInfo subdirectory in allSubdirectories) {
					DeleteAllWithoutSuffix(subdirectory, suffix, SearchOption.AllDirectories, ignoredDirectories);
				}
			}
		}

		/// <summary>
		/// Deletes all files and directories in the specified directory, whose name is ended by the specified suffix, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files and directories.</param>
		/// <param name="suffix">The suffix that files and directories must contain in order to be deleted.</param>
		public static void DeleteAllWithSuffix(DirectoryInfo directory, string suffix)
		{
			DeleteAllWithSuffix(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Deletes all files and directories in the specified directory, whose name is ended by the specified suffix, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to delete all files and directories.</param>
		/// <param name="suffix">The suffix that files and directories must contain in order to be deleted.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void DeleteAllWithSuffix(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			DeleteAllFilesWithSuffix(directory, suffix, SearchOption.TopDirectoryOnly);
			DeleteAllDirectoriesWithSuffix(directory, suffix, SearchOption.TopDirectoryOnly);

			if(searchOption == SearchOption.AllDirectories) {
				DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);

				foreach(DirectoryInfo subdirectory in allSubdirectories) {
					DeleteAllWithSuffix(subdirectory, suffix, SearchOption.AllDirectories);
				}
			}
		}

		/// <summary>
		/// Renames the specified directory (removes the specified suffix from the end of the name, if present).
		/// </summary>
		/// <param name="directory">The directory to remove the suffix from.</param>
		/// <param name="suffix">The suffix to remove from the directory.</param>
		public static void RemoveSuffix(DirectoryInfo directory, string suffix)
		{
			if(!directory.Name.EndsWith(suffix)) {
				return;
			}

			string newName = directory.Name.Substring(0, directory.Name.Length - suffix.Length);
			string absolutePath = Path.Combine(Path.GetDirectoryName(directory.FullName), newName);
			directory.MoveTo(absolutePath);
		}

		/// <summary>
		/// Renames the specified file (removes the specified suffix from the end of the name, if present).
		/// </summary>
		/// <param name="file">The file to remove the suffix from.</param>
		/// <param name="suffix">The suffix to remove from the file.</param>
		public static void RemoveSuffix(FileInfo file, string suffix)
		{
			if(!file.Name.EndsWith(suffix)) {
				return;
			}

			string newName = file.Name.Substring(0, file.Name.Length - suffix.Length);
			string absolutePath = Path.Combine(Path.GetDirectoryName(file.FullName), newName);
			file.MoveTo(absolutePath);
		}

		/// <summary>
		/// Removes the specified suffix from all directories and files in the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories and files.</param>
		/// <param name="suffix">The suffix to remove from all directories and files.</param>
		public static void RemoveSuffixFromAll(DirectoryInfo directory, string suffix)
		{
			RemoveSuffixFromAll(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Removes the specified suffix from all directories and files in the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories and files.</param>
		/// <param name="suffix">The suffix to remove from all directories and files.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void RemoveSuffixFromAll(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			RemoveSuffixFromAllFiles(directory, suffix, SearchOption.TopDirectoryOnly);
			RemoveSuffixFromAllDirectories(directory, suffix, SearchOption.TopDirectoryOnly);

			if(searchOption == SearchOption.AllDirectories) {
				DirectoryInfo[] allSubdirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
				foreach(DirectoryInfo subdirectory in allSubdirectories) {
					RemoveSuffixFromAll(subdirectory, suffix, SearchOption.AllDirectories);
				}
			}
		}

		/// <summary>
		/// Removes the specified suffix from all directories in the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories.</param>
		/// <param name="suffix">The suffix to remove from all directories.</param>
		public static void RemoveSuffixFromAllDirectories(DirectoryInfo directory, string suffix)
		{
			RemoveSuffixFromAllDirectories(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Removes the specified suffix from all directories in the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories.</param>
		/// <param name="suffix">The suffix to remove from all directories.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void RemoveSuffixFromAllDirectories(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			DirectoryInfo[] allDirectories = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);

			foreach(DirectoryInfo subdirectory in allDirectories) {
				if(searchOption == SearchOption.AllDirectories) {
					RemoveSuffixFromAllDirectories(subdirectory, suffix, SearchOption.AllDirectories);
				}
				RemoveSuffix(subdirectory, suffix);
			}
		}

		/// <summary>
		/// Removes the specified suffix from all files inside the specified directory, including subdirectories.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories.</param>
		/// <param name="suffix">The suffix to remove from all directories.</param>
		public static void RemoveSuffixFromAllFiles(DirectoryInfo directory, string suffix)
		{
			RemoveSuffixFromAllFiles(directory, suffix, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Removes the specified suffix from all files inside the specified directory, using the specified search option.
		/// </summary>
		/// <param name="directory">The directory in which to remove the suffix from all directories.</param>
		/// <param name="suffix">The suffix to remove from all directories.</param>
		/// <param name="searchOption">Specifies whether to include only the current directory or all subdirectories.</param>
		public static void RemoveSuffixFromAllFiles(DirectoryInfo directory, string suffix, SearchOption searchOption)
		{
			FileInfo[] allFiles = directory.GetFiles("*" + suffix, searchOption);
			foreach(FileInfo file in allFiles) {
				RemoveSuffix(file, suffix);
			}
		}

		/// <summary>
		/// Searches for a file that matches the specified search pattern from the specified directory up.
		/// <para>
		/// If the file is not in the start directory, it will search in the parent directory. And then again in the parent directory. Until either the file is found or the start of the drive is reached and an exception is thrown.
		/// </para>
		/// </summary>
		/// <param name="startDirectory">The relative or absolute path to the directory in which to start searching. This string is not case-sensitive.</param>
		/// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
		/// <param name="throwExceptionIfMultipleFound">Determines whether or not to throw an exception if multiple files are found for the specified search pattern.</param>
		public static string SearchUp(string startDirectory, string searchPattern, bool throwExceptionIfMultipleFound = false)
		{
			string currentDirectory = startDirectory;
			while(true) {
				string[] files = Directory.GetFiles(currentDirectory, searchPattern, SearchOption.TopDirectoryOnly);

				if(files.Length > 0) {
					if(throwExceptionIfMultipleFound && files.Length > 1) {
						throw new Exception($"Multiple files were found for the search pattern '{searchPattern}'.");
					}
					return files[0];
				}

				// moves to the parent
				currentDirectory = Directory.GetParent(currentDirectory).FullName;
			}
		}
	}
}
