using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace EFCodeFirst.WPF
{
    public partial class MainWindow : Window
    {
        private ApiClient _apiClient;
        private List<PublisherDto> _publishers;
        private string _currentEditingBookISBN;
        private int? _currentEditingPublisherID;

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            _publishers = new List<PublisherDto>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBooksAsync();
            await LoadPublishersAsync();
        }

        #region Books Operations

        private async Task LoadBooksAsync()
        {
            try
            {
                var books = await _apiClient.GetAllBooksAsync();
                BooksDataGrid.ItemsSource = books;
                StatusMessage.Text = $"Sikeresen betöltve {books.Count} könyv.";
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Hiba: {ex.Message}";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnRefreshBooks_Click(object sender, RoutedEventArgs e)
        {
            await LoadBooksAsync();
        }

        private void BtnNewBook_Click(object sender, RoutedEventArgs e)
        {
            ClearBookForm();
            _currentEditingBookISBN = null;
            ShowBookForm();
            FormTitle.Text = "Új könyv";
            TxtISBN.IsReadOnly = false;
        }

        private void BooksDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is BookDto book)
            {
                EditBook(book);
            }
        }

        private void EditBook(BookDto book)
        {
            _currentEditingBookISBN = book.ISBN;
            ShowBookForm();
            FormTitle.Text = "Könyv szerkesztése";
            
            TxtISBN.Text = book.ISBN;
            TxtISBN.IsReadOnly = true;
            TxtTitle.Text = book.Title;
            TxtAuthor.Text = book.Author;
            TxtLanguage.Text = book.Language;
            TxtPages.Text = book.Pages.ToString();
            TxtDescription.Text = book.Description;
            CmbPublisher.SelectedValue = book.PublisherID;
        }

        private async void BtnSaveBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtISBN.Text) || 
                    string.IsNullOrWhiteSpace(TxtTitle.Text) ||
                    CmbPublisher.SelectedValue == null)
                {
                    StatusMessage.Text = "Kérjük töltse ki az ISBN, cím és kiadó mezőket!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                if (!int.TryParse(TxtPages.Text, out int pages))
                {
                    StatusMessage.Text = "Az oldalak száma számnak kell lennie!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                bool success;

                if (_currentEditingBookISBN == null)
                {
                    // Create new book
                    var newBook = new CreateBookDto
                    {
                        ISBN = TxtISBN.Text,
                        Title = TxtTitle.Text,
                        Author = TxtAuthor.Text,
                        Language = TxtLanguage.Text,
                        Pages = pages,
                        Description = TxtDescription.Text,
                        PublisherID = (int)CmbPublisher.SelectedValue
                    };
                    success = await _apiClient.CreateBookAsync(newBook);
                }
                else
                {
                    // Update existing book
                    var updateBook = new UpdateBookDto
                    {
                        Title = TxtTitle.Text,
                        Author = TxtAuthor.Text,
                        Language = TxtLanguage.Text,
                        Pages = pages,
                        Description = TxtDescription.Text,
                        PublisherID = (int)CmbPublisher.SelectedValue
                    };
                    success = await _apiClient.UpdateBookAsync(_currentEditingBookISBN, updateBook);
                }

                if (success)
                {
                    StatusMessage.Text = "Könyv sikeresen mentve!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                    await LoadBooksAsync();
                    ClearBookForm();
                    HideBookForm();
                }
                else
                {
                    StatusMessage.Text = "Hiba történt a mentés során!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Hiba: {ex.Message}";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtISBN.Text))
            {
                StatusMessage.Text = "Kérjük válasszon egy könyvet a törléshez!";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (MessageBox.Show($"Biztosan törli a \"{TxtTitle.Text}\" könyvet?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _apiClient.DeleteBookAsync(TxtISBN.Text);
                    if (success)
                    {
                        StatusMessage.Text = "Könyv sikeresen törölve!";
                        StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                        await LoadBooksAsync();
                        ClearBookForm();
                        HideBookForm();
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage.Text = $"Hiba: {ex.Message}";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void BtnCancelBook_Click(object sender, RoutedEventArgs e)
        {
            ClearBookForm();
            HideBookForm();
        }

        private void ClearBookForm()
        {
            TxtISBN.Text = "";
            TxtTitle.Text = "";
            TxtAuthor.Text = "";
            TxtLanguage.Text = "";
            TxtPages.Text = "";
            TxtDescription.Text = "";
            CmbPublisher.SelectedIndex = -1;
            _currentEditingBookISBN = null;
        }

        #endregion

        #region Publishers Operations

        private async Task LoadPublishersAsync()
        {
            try
            {
                _publishers = await _apiClient.GetAllPublishersAsync();
                PublishersDataGrid.ItemsSource = _publishers;
                CmbPublisher.ItemsSource = _publishers;
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Hiba: {ex.Message}";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnRefreshPublishers_Click(object sender, RoutedEventArgs e)
        {
            await LoadPublishersAsync();
        }

        private void BtnNewPublisher_Click(object sender, RoutedEventArgs e)
        {
            ClearPublisherForm();
            _currentEditingPublisherID = null;
            ShowPublisherForm();
            FormTitle.Text = "Új kiadó";
        }

        private void PublishersDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PublishersDataGrid.SelectedItem is PublisherDto publisher)
            {
                EditPublisher(publisher);
            }
        }

        private void EditPublisher(PublisherDto publisher)
        {
            _currentEditingPublisherID = publisher.ID;
            ShowPublisherForm();
            FormTitle.Text = "Kiadó szerkesztése";

            TxtPublisherID.Text = publisher.ID.ToString();
            TxtPublisherName.Text = publisher.Name;
            TxtPublisherBooks.Text = publisher.BookCount.ToString();
        }

        private async void BtnSavePublisher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtPublisherName.Text))
                {
                    StatusMessage.Text = "Kérjük töltse ki a kiadó nevét!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                bool success;

                if (_currentEditingPublisherID == null)
                {
                    // Create new publisher
                    var newPublisher = new CreatePublisherDto
                    {
                        Name = TxtPublisherName.Text
                    };
                    success = await _apiClient.CreatePublisherAsync(newPublisher);
                }
                else
                {
                    // Update existing publisher
                    var updatePublisher = new UpdatePublisherDto
                    {
                        Name = TxtPublisherName.Text
                    };
                    success = await _apiClient.UpdatePublisherAsync((int)_currentEditingPublisherID, updatePublisher);
                }

                if (success)
                {
                    StatusMessage.Text = "Kiadó sikeresen mentve!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                    await LoadPublishersAsync();
                    ClearPublisherForm();
                    HidePublisherForm();
                }
                else
                {
                    StatusMessage.Text = "Hiba történt a mentés során!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Hiba: {ex.Message}";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnDeletePublisher_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentEditingPublisherID.HasValue)
            {
                StatusMessage.Text = "Kérjük válasszon egy kiadót a törléshez!";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (MessageBox.Show($"Biztosan törli a \"{TxtPublisherName.Text}\" kiadót?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _apiClient.DeletePublisherAsync((int)_currentEditingPublisherID);
                    if (success)
                    {
                        StatusMessage.Text = "Kiadó sikeresen törölve!";
                        StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                        await LoadPublishersAsync();
                        ClearPublisherForm();
                        HidePublisherForm();
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage.Text = $"Hiba: {ex.Message}";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void BtnCancelPublisher_Click(object sender, RoutedEventArgs e)
        {
            ClearPublisherForm();
            HidePublisherForm();
        }

        private void ClearPublisherForm()
        {
            TxtPublisherID.Text = "";
            TxtPublisherName.Text = "";
            TxtPublisherBooks.Text = "";
            _currentEditingPublisherID = null;
        }

        #endregion

        #region UI Helper Methods

        private void ShowBookForm()
        {
            BookFormPanel.Visibility = Visibility.Visible;
            PublisherFormPanel.Visibility = Visibility.Collapsed;
        }

        private void HideBookForm()
        {
            BookFormPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowPublisherForm()
        {
            BookFormPanel.Visibility = Visibility.Collapsed;
            PublisherFormPanel.Visibility = Visibility.Visible;
        }

        private void HidePublisherForm()
        {
            PublisherFormPanel.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
