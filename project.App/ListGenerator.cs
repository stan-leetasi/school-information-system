using System.Reflection;

namespace project.App
{
    public abstract class ListGenerator
    {
        public static void SetListView<T>(ref ListView listView, IEnumerable<T> list)
        {
            listView.ItemsSource = list;
            listView.ItemTemplate = new DataTemplate(() =>
            {
                PropertyInfo[] properties =
                    typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                var grid = new Grid();
                for (int i = 0; i < properties.Length; i++)
                {
                    var label = new Label();
                    label.SetBinding(Label.TextProperty, properties[i].Name);
                    grid.Children.Add(label);
                    grid.SetColumn(label, i);
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }

                return new ViewCell { View = grid };
            });
        }
    }
}

/*

How to use

CodeBehind:
    ListGenerator.SetListView(ref subjectListView, Subjects);
XAML:
    <ListView x:Name="subjectListView"></ListView>

( also u can edit the Formatting after or inside the function )

*/