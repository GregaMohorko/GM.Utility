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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GM.Utility.Patterns.UndoRedo
{
	/// <summary>
	/// Represents an Undo/Redo manager.
	/// <para>A classical approach to implement undo/redo is to allow changes on the model only through commands. And every command should be invertible. The user then executes an action, the application creates a command, executes it and puts an inverted command on the undo-stack. When the user clicks on undo, the application executes the top-most (inverse) command on the undo-stack, inverts it again (to get the original command again) and puts it on the redo-stack. That's it.</para>
	/// </summary>
	public class GMUndoRedo : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event for the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		public void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion INotifyPropertyChanged

		/// <summary>
		/// Gets or sets the maximum amount of undo actions. Default is 100.
		/// </summary>
		public int MaximumHistoryCount { get; set; } = 100;

		private readonly LinkedList<UndoRedoAction> undoStack;
		private readonly Stack<UndoRedoAction> redoStack;

		/// <summary>
		/// Creates a new instance of <see cref="GMUndoRedo"/>.
		/// </summary>
		public GMUndoRedo()
		{
			undoStack = new LinkedList<UndoRedoAction>();
			redoStack = new Stack<UndoRedoAction>();
		}

		/// <summary>
		/// Returns true if there is at least one action on the undo stack.
		/// </summary>
		public bool CanUndo => undoStack.Count > 0;
		/// <summary>
		/// Returns true if there is at least one action on the redo stack.
		/// </summary>
		public bool CanRedo => redoStack.Count > 0;
		/// <summary>
		/// Gets a list of descriptions of available undo actions.
		/// </summary>
		public List<string> AvailableUndoCommands => undoStack
			.Select(ura => ura.Description)
			.ToList();
		/// <summary>
		/// Gets a list of descriptions of available redo actions.
		/// </summary>
		public List<string> AvailableRedoCommands => redoStack
			.Select(ura => ura.Description)
			.ToList();

		/// <summary>
		/// Clears the undo/redo stacks.
		/// </summary>
		public void Clear()
		{
			undoStack.Clear();
			redoStack.Clear();
			RaisePropertyChanged(nameof(CanUndo));
			RaisePropertyChanged(nameof(CanRedo));
			RaisePropertyChanged(nameof(AvailableUndoCommands));
			RaisePropertyChanged(nameof(AvailableRedoCommands));
		}

		/// <summary>
		/// Adds the specified undo/redo actions to the undo stack as a single action.
		/// <para>If there are currently any actions on the redo stack, they will be removed.</para>
		/// </summary>
		/// <param name="description">The description of the action.</param>
		/// <param name="undoRedoActions">The undo/redo actions.</param>
		public void Add(string description, List<(Action undo, Action redo)> undoRedoActions)
		{
			if(undoRedoActions == null) {
				throw new ArgumentNullException(nameof(undoRedoActions));
			}
			if(undoRedoActions.Count == 0) {
				return;
			}
			List<Action> undoActions = undoRedoActions
				.Select(t => t.undo)
				.ToList();
			List<Action> redoActions = undoRedoActions
				.Select(t => t.redo)
				.ToList();

			void undo()
			{
				foreach(Action undoAction in undoActions) {
					undoAction();
				}
			}
			void redo()
			{
				foreach(Action redoAction in redoActions) {
					redoAction();
				}
			}
			Add(description, undo, redo);
		}

		/// <summary>
		/// Adds the specified undo/redo action to the undo stack.
		/// <para>If there are currently any actions on the redo stack, they will be removed.</para>
		/// </summary>
		/// <param name="description">The description of the action.</param>
		/// <param name="undo">The undo action.</param>
		/// <param name="redo">The redo action.</param>
		public void Add(string description, Action undo, Action redo)
		{
			var undoRedoAction = new UndoRedoAction(description, undo, redo);
			Add(undoRedoAction);
		}

		/// <summary>
		/// Adds the specified undo/redo action to the undo stack.
		/// <para>If there are currently any actions on the redo stack, they will be removed.</para>
		/// </summary>
		/// <param name="undoRedoAction">The undo action.</param>
		public void Add(UndoRedoAction undoRedoAction)
		{
			_ = undoStack.AddLast(undoRedoAction);
			RaisePropertyChanged(nameof(CanUndo));
			RaisePropertyChanged(nameof(AvailableUndoCommands));
			if(redoStack.Count > 0) {
				// when adding something new to undo, clear the current redo stack
				// this is to avoid multiple realities ...
				redoStack.Clear();
				RaisePropertyChanged(nameof(CanRedo));
				RaisePropertyChanged(nameof(AvailableRedoCommands));
			}
			while(undoStack.Count > MaximumHistoryCount) {
				undoStack.RemoveFirst();
			}
		}

		/// <summary>
		/// Executes an undo/redo action.
		/// </summary>
		/// <param name="undo">Determines whether to execute the undo or redo action.</param>
		public void UndoRedo(bool undo)
		{
			// check if there is anything to execute
			{
				ICollection executingStack = undo ? (ICollection)undoStack : redoStack;
				if(executingStack.Count == 0) {
					// how did this happen? hmm ...
					return;
				}
			}
			// take the topmost action from the executing stack
			UndoRedoAction actionToExecute;
			if(undo) {
				actionToExecute = undoStack.Last.Value;
				undoStack.RemoveLast();
			} else {
				actionToExecute = redoStack.Pop();
			}
			// execute the command
			actionToExecute.Action.Invoke();
			// push the inverted action to the opposite stack
			UndoRedoAction invertedAction = actionToExecute.GetInvertedUndoRedoAction();
			if(undo) {
				redoStack.Push(invertedAction);
			} else {
				_ = undoStack.AddLast(invertedAction);
			}

			RaisePropertyChanged(nameof(CanUndo));
			RaisePropertyChanged(nameof(CanRedo));
			RaisePropertyChanged(nameof(AvailableUndoCommands));
			RaisePropertyChanged(nameof(AvailableRedoCommands));
		}
	}
}
