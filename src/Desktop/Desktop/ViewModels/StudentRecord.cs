using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Desktop.ViewModels;

public class AuthorRecord : ViewModelBase
{
    private int _id;
    private string _name = null!;
    private string _description = null!;
    private int _birthYear;

    private ObservableCollection<AuthorRecord> _authorRecords = null!;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public int BirthYear
    {
        get => _birthYear;
        set
        {
            _birthYear = value;
            OnPropertyChanged(nameof(BirthYear));
        }
    }

    public ObservableCollection<AuthorRecord> AuthorRecords
    {
        get => _authorRecords;
        set
        {
            _authorRecords = value;
            OnPropertyChanged(nameof(AuthorRecords));
        }
    }

    private void AuthorModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(AuthorRecords));
    }
}
