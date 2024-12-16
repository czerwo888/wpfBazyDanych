using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfBazyDanych1._0;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
     private dbContext _context = new dbContext();
    
    public MainWindow()
    {
        InitializeComponent();

        _context.Movies.ToList().ForEach(film =>
            film.Category = _context.Categories.ToList().FirstOrDefault(category => category.Id == film.CategoryId));
        
        data.ItemsSource = _context.Movies.ToList();
        filmCategory.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
        filmCategory.SelectedIndex = 0;
        
        List<Category> categories = _context.Categories.ToList();
        categories.Insert(0, new Category { Id = 0, Name = "All"});
        categoryFilter.ItemsSource = categories;
        categoryFilter.SelectedIndex = 0;
    }

    private void addFilm_Click(object sender, RoutedEventArgs e)
    {
        if(filmName.Text == "" || filmYear.Text == "")
        {
            MessageBox.Show("Fill all fields");
            return;
        }
        var film = new Movie
        {
            Title = filmName.Text,
            Year = int.Parse(filmYear.Text),
            CategoryId = _context.Categories.ToList()
                .FirstOrDefault(c => c.Name == filmCategory.Text).Id
        };
        _context.Movies.Add(film);
        _context.SaveChanges();
        data.ItemsSource = _context.Movies.ToList();
    }

    private void CategoryFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cat = (Category) categoryFilter.SelectedItem;
        if(cat != null)
            if (cat.Id == 0)
                data.ItemsSource = _context.Movies.ToList();
            else
                data.ItemsSource = _context.Movies.ToList()
                    .Where(film => film.CategoryId == cat.Id).ToList();
    }

    private void DeleteFilm_OnClick(object sender, RoutedEventArgs e)
    {
        data.SelectedItems.Cast<Movie>().ToList().ForEach(film => _context.Movies.Remove(film));
        _context.SaveChanges();
        data.ItemsSource = _context.Movies.ToList();
        if (data.Items.Count == 0)
            MessageBox.Show("Select a film to delete");
    }

    private void EditFilm_OnClick(object sender, RoutedEventArgs e)
    {
        if (data.SelectedItem is Movie selectedFilm)
        {
            filmNameEdit.Text = selectedFilm.Title;
            filmYearEdit.Text = selectedFilm.Year.ToString();
            filmCategoryEdit.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
            filmCategoryEdit.SelectedItem = _context.Categories.FirstOrDefault(c => c.Id == selectedFilm.CategoryId)?.Name;
            editPanel.Visibility = Visibility.Visible;
        }
        else
        {
            editPanel.Visibility = Visibility.Hidden;
        }
    }

    private void EditFilmSave_OnClick(object sender, RoutedEventArgs e)
    {
        if (data.SelectedItem is Movie film)
        {
            film.Title = filmNameEdit.Text;
            film.Year = int.Parse(filmYearEdit.Text);
            film.CategoryId = _context.Categories.FirstOrDefault(c => c.Name == filmCategoryEdit.Text).Id;
            _context.SaveChanges();
            data.ItemsSource = _context.Movies.ToList();
            editPanel.Visibility = Visibility.Hidden;
        }
    }
}