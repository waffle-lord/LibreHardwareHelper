﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibreHardware_Helper.Model.HardwareData;

public class PropertyNotifierBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected TRet RaiseAndSetIfChanged<TRet>(ref TRet backingField, TRet newValue,
        [CallerMemberName] string propertyName = null)
    {
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));

        if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            return newValue;

        backingField = newValue;
        RaisePropertyChanged(propertyName);
        return newValue;
    }

    protected virtual void RaisePropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}