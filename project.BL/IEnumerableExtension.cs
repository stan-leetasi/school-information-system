﻿using System.Collections.ObjectModel;

namespace project.BL;

public static class EnumerableExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new(values);
}