using project.App.ViewModels.Student;
using project.BL.Models;

namespace project.App.Views.Student
{
    public partial class StudentListView
    {
        public StudentListView(StudentListViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
    //         //BindingContext = viewModel;
    //         VerticalStackLayout mainLayout = [];
    //
    //         // Create the top grid
    //         Grid topGrid = new()
    //         {
    //             HorizontalOptions = LayoutOptions.Fill,
    //             ColumnDefinitions =
    //             [
    //                 new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
    //                 new ColumnDefinition { Width = GridLength.Auto }
    //             ]
    //         };
    //
    //         int count = 0;
    //         SearchBar searchBar = new() { Placeholder = "Filter students", WidthRequest = 600 };
    //         searchBar.SetBinding(SearchBar.TextProperty, "FilterPreferences.SearchedTerm");
    //         Grid.SetColumn(searchBar, count++);
    //         topGrid.Children.Add(searchBar);
    //
    //         Button addButton = new() { Text = "Add" };
    //         addButton.SetBinding(Button.CommandProperty, "AddStudentCommand");
    //         Grid.SetColumn(addButton, count);
    //         topGrid.Children.Add(addButton);
    //
    //         mainLayout.Children.Add(topGrid);
    //
    //         // Create the label
    //         Label titleLabel = new()
    //         {
    //             Text = "List of Students", FontSize = 20, HorizontalOptions = LayoutOptions.Center
    //         };
    //         mainLayout.Children.Add(titleLabel);
    //
    //         // Create the sort buttons grid
    //         Grid sortGrid = new()
    //         {
    //             HorizontalOptions = LayoutOptions.Fill,
    //             ColumnSpacing = 20,
    //             ColumnDefinitions =
    //             [
    //                 new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
    //                 new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
    //             ]
    //         };
    //
    //         count = 0;
    //         Button sortBySurnameButton = new() { Text = nameof(StudentListModel.Surname) };
    //         sortBySurnameButton.SetBinding(Button.CommandProperty, nameof(viewModel.SortBySurnameCommand));
    //         Grid.SetColumn(sortBySurnameButton, count++);
    //
    //         Button sortByNameButton = new() { Text = nameof(StudentListModel.Name) };
    //         sortByNameButton.SetBinding(Button.CommandProperty, nameof(viewModel.SortByNameCommand));
    //         Grid.SetColumn(sortByNameButton, count);
    //
    //         sortGrid.Children.Add(sortBySurnameButton);
    //         sortGrid.Children.Add(sortByNameButton);
    //
    //         mainLayout.Children.Add(sortGrid);
    //
    //         // Create the student list
    //         ListView listView = new() { ItemTemplate = new DataTemplate(typeof(StudentViewCell)) };
    //         listView.SetBinding(ListView.ItemsSourceProperty, "Students");
    //         mainLayout.Children.Add(listView);
    //
    //         Content = new ScrollView { Content = mainLayout };
    //     }
    // }
    //
    // public class StudentViewCell : ViewCell
    // {
    //     public StudentViewCell()
    //     {
    //         Grid grid = new()
    //         {
    //             HorizontalOptions = LayoutOptions.Fill,
    //             ColumnSpacing = 40,
    //             ColumnDefinitions =
    //             [
    //                 new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
    //                 new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
    //             ]
    //         };
    //
    //         int count = 0;
    //         Label surnameLabel = new();
    //         surnameLabel.SetBinding(Label.TextProperty, "Surname");
    //         Grid.SetColumn(surnameLabel, count++);
    //
    //         Label nameLabel = new();
    //         nameLabel.SetBinding(Label.TextProperty, "Name");
    //         Grid.SetColumn(nameLabel, count);
    //
    //         grid.Children.Add(surnameLabel);
    //         grid.Children.Add(nameLabel);
    //
    //         TapGestureRecognizer tapGestureRecognizer = new();
    //         tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty,
    //             new Binding("BindingContext.GoToDetailCommand",
    //                 source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, typeof(ListView))));
    //         tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Id");
    //
    //         grid.GestureRecognizers.Add(tapGestureRecognizer);
    //
    //         View = grid;
        }
    }
}