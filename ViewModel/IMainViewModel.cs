﻿using System.ComponentModel;
using UndoRedo.Model;

namespace UndoRedo.ViewModel;

public interface IMainViewModel
{
    public ICommandExtended AddCommand { get; }
    public ICommandExtended UndoCommand { get; }
    public ICommandExtended RedoCommand { get; }

    public DataRecord? Value { get; }
}
