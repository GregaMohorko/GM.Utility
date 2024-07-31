/*
MIT License

Copyright (c) 2020 Gregor Mohorko

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
Created: 2020-07-21
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility.Patterns.UndoRedo
{
	/// <summary>
	/// A undo/redo action.
	/// <para>Used in undo/redo pattern, where for every action there must exist an invertable action.</para>
	/// </summary>
	public class UndoRedoAction
	{
		/// <summary>
		/// The description of this undo/redo action.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Gets the action of this undo/redo action.
		/// </summary>
		public Action Action { get; private set; }
		/// <summary>
		/// Gets the inverted action of this undo/redo action.
		/// </summary>
		public Action ActionInverted { get; private set; }

		/// <summary>
		/// Creates a new instance of <see cref="UndoRedoAction"/>.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="action">The action.</param>
		/// <param name="actionInverted">The inverted action.</param>
		public UndoRedoAction(string description, Action action, Action actionInverted)
		{
			Description = description;
			Action = action ?? throw new ArgumentNullException(nameof(action));
			ActionInverted = actionInverted ?? throw new ArgumentNullException(nameof(actionInverted));
		}

		/// <summary>
		/// Returns the inverted undo/redo action, where the <see cref="Action"/> and <see cref="ActionInverted"/> are swapped.
		/// </summary>
		public UndoRedoAction GetInvertedUndoRedoAction()
		{
			return new UndoRedoAction(Description, ActionInverted, Action);
		}
	}
}
