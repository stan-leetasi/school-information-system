
using System.Reflection;
using Xunit.Sdk;

namespace project.Common.Tests;

public static class SortAssert
{
    public static void IsSorted<T>(List<T> sortedList, string sortedByPropertyName, bool descending)
    {
        PropertyInfo? property = typeof(T).GetProperty(sortedByPropertyName);
        if (property == null) throw new ArgumentException($"{typeof(T).Name} does not have property {sortedByPropertyName}.");

        for (int i = 0; i < sortedList.Count - 1; i++)
        {
            object currentValue = property.GetValue(sortedList[i])!;
            object nextValue = property.GetValue(sortedList[i + 1])!;
            if (descending) Assert.True(Comparer<object>.Default.Compare(currentValue, nextValue) >= 0);
            else Assert.True(Comparer<object>.Default.Compare(currentValue, nextValue) <= 0);
        }
    }
}
