using project.App.ViewModels;

namespace project.App.Views;

public class Generator
{
    public static Grid BuildGrid<T>(T[] elements) where T : View
    {
        Grid grid = new() { HorizontalOptions = LayoutOptions.Fill, ColumnSpacing = 20 };

        for (int i = 0; i < elements.Length; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(elements[i], i);
            grid.Children.Add(elements[i]);
        }

        return grid;
    }

    public static Button SortButton(string buttonText, string commandProperty)
    {
        Button button = new() { Text = buttonText };
        button.SetBinding(Button.CommandProperty, commandProperty);
        return button;
    }

    public static Label ListLabel(string text)
    {
        Label label = new();
        label.SetBinding(Label.TextProperty, text);
        return label;
    }
}