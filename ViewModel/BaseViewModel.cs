using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UndoRedo.Misc;

namespace UndoRedo.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs _emptyChangeArgs = new(string.Empty);
    private static readonly IDictionary<string, PropertyChangedEventArgs> _changedProperties = new Dictionary<string, PropertyChangedEventArgs>();

    public event PropertyChangedEventHandler? PropertyChanged;

    #region "Boilerplate stuff"
    protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
    {
        OnPropertyChanged(ExpressionHelper.Name(expression));
    }

    protected virtual void OnPropertyChanged()
    {
        OnPropertyChanged(null!);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null)
        {
            if (propertyName == null)
            {
                handler(this, _emptyChangeArgs);
            }
            else
            {
                PropertyChangedEventArgs args;
                if (!_changedProperties.TryGetValue(propertyName, out args!))
                {
                    args = new PropertyChangedEventArgs(propertyName);
                    _changedProperties.Add(propertyName, args);
                }

                handler(this, args);
            }
        }
    }

    protected virtual bool SetPropertyAndNotify<T>(ref T existingValue, T newValue, Expression<Func<T>> expression)
    {
        if (EqualityComparer<T>.Default.Equals(existingValue, newValue))
        {
            return false;
        }

        existingValue = newValue;
        OnPropertyChanged(expression);

        return true;
    }
    #endregion
}
