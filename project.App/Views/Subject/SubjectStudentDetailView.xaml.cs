using project.App.ViewModels.Subject;
using project.BL.Models;

namespace project.App.Views.Subject;

public partial class SubjectStudentDetailView
{
    public SubjectStudentDetailView(SubjectStudentDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        //     BindingContext = viewModel;
        //     VerticalStackLayout mainLayout = [];
        //
        //     SearchBar searchBar = new() { Placeholder = "Filter students", WidthRequest = 600 };
        //     searchBar.SetBinding(SearchBar.TextProperty, "FilterPreferences.SearchedTerm");
        //     mainLayout.Children.Add(searchBar);
        //
        //     // Create the label
        //     mainLayout.Children.Add(new Label
        //     {
        //         Text = "List of Activities", FontSize = 20, HorizontalOptions = LayoutOptions.Center
        //     });
        //
        //     // Create the sort buttons grid
        //
        //     mainLayout.Children.Add(Generator.BuildGrid(new[]
        //         {
        //             Generator.SortButton("Begin Time", nameof(viewModel.SortByBeginTimeCommand)),
        //             Generator.SortButton("End TIme", nameof(viewModel.SortByEndTimeCommand)),
        //             Generator.SortButton("Area", nameof(viewModel.SortByAreaCommand)),
        //             Generator.SortButton("Type", nameof(viewModel.SortByTypeCommand)),
        //             Generator.SortButton("Registered Students", nameof(viewModel.SortByRegisteredStudentsCommand)),
        //             Generator.SortButton("Points", nameof(viewModel.SortByPointsCommand)),
        //             Generator.SortButton("Is Registered", nameof(viewModel.SortByIsRegisteredCommand))
        //         }
        //     ));
        //
        //     // Create the student list
        //     ListView listView = new() { ItemTemplate = new DataTemplate(typeof(StudentViewCell)) };
        //     listView.SetBinding(ListView.ItemsSourceProperty, nameof(viewModel.Activities));
        //     mainLayout.Children.Add(listView);
        //
        //     Content = new ScrollView { Content = mainLayout };
        // }
        //
        // public class StudentViewCell : ViewCell
        // {
        //     public StudentViewCell()
        //     {
        //         Label[] elements =
        //         [
        //             Generator.ListLabel(nameof(ActivityListModel.BeginTime)),
        //             Generator.ListLabel(nameof(ActivityListModel.EndTime)),
        //             Generator.ListLabel(nameof(ActivityListModel.Area)),
        //             Generator.ListLabel(nameof(ActivityListModel.Type)),
        //             Generator.ListLabel(nameof(ActivityListModel.RegisteredStudents)),
        //             Generator.ListLabel(nameof(ActivityListModel.Points))
        //         ];
        //         Grid grid = Generator.BuildGrid(elements);
        //         CheckBox checkBox = new();
        //         checkBox.SetBinding(CheckBox.IsCheckedProperty, nameof(ActivityListModel.IsRegistered));
        //         checkBox.SetBinding(VisualElement.IsEnabledProperty, "StudentView");
        //         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //         Grid.SetColumn(checkBox, elements.Length);
        //         grid.Children.Add(checkBox);
        //
        //         checkBox.HorizontalOptions = LayoutOptions.CenterAndExpand;
        //
        //         TapGestureRecognizer tapGestureRecognizer = new();
        //         tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "GoToDetailCommand");
        //         tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Id");
        //
        //         grid.GestureRecognizers.Add(tapGestureRecognizer);
        //
        //         View = grid;
        //     }
    }
}