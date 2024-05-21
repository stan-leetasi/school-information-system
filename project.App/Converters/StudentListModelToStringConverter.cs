using CommunityToolkit.Maui.Converters;
using project.BL.Models;
using System.Globalization;

namespace project.App.Converters;

public class StudentListModelToStringConverter : BaseConverterOneWay<StudentListModel, string>
{
    public override string ConvertFrom(StudentListModel value, CultureInfo? culture)
    {
        return $"{value.Name} {value.Surname}";
    }

    public override string DefaultConvertReturnValue { get; set; } = string.Empty;
}