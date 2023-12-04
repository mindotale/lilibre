using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Desktop.Data;
using Desktop.Models;

namespace Desktop.ViewModels
{
    public class AuthorViewModel
    {
        private ICommand _saveCommand;
        private ICommand _resetCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private readonly IAuthorRepository _repository;
        private readonly Author _authorEntity;
        public AuthorRecord AuthorRecord { get; set; }

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => ResetData(), null);

                return _resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveData(), null);

                return _saveCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => EditData((int)param), null);

                return _editCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => DeleteAuthor((int)param), null);

                return _deleteCommand;
            }
        }

        public AuthorViewModel()
        {
            _authorEntity = new Author();
            _repository = new InMemoryAuthorRepository();
            AuthorRecord = new AuthorRecord();
            GetAll();
        }

        public void ResetData()
        {
            AuthorRecord.Name = string.Empty;
            AuthorRecord.Id = 0;
            AuthorRecord.Description = string.Empty;
            AuthorRecord.BirthYear = 0;
        }

        public void DeleteAuthor(int id)
        {
            if (MessageBox.Show("Confirm delete of this record?", "Author", MessageBoxButton.YesNo)
                != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _repository.Remove(id);
                MessageBox.Show("Record successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while saving. " + ex.InnerException);
            }
            finally
            {
                GetAll();
            }
        }

        public void SaveData()
        {
            if (AuthorRecord == null)
            {
                return;
            }

            _authorEntity.Name = AuthorRecord.Name;
                _authorEntity.BirthYear = AuthorRecord.BirthYear;
                _authorEntity.Description = AuthorRecord.Description;

                try
                {
                    if (AuthorRecord.Id <= 0)
                    {
                        _repository.Add(_authorEntity);
                        MessageBox.Show("New record successfully saved.");
                    }
                    else
                    {
                        _authorEntity.Id = AuthorRecord.Id;
                        _repository.Update(_authorEntity);
                        MessageBox.Show("Record successfully updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                    ResetData();
                }
        }

        public void EditData(int id)
        {
            var model = _repository.GetById(id);
            AuthorRecord.Id = model.Id;
            AuthorRecord.Name = model.Name;
            AuthorRecord.Description = model.Description;
            AuthorRecord.BirthYear = model.BirthYear;
        }

        public void GetAll()
        {
            AuthorRecord.AuthorRecords = new ObservableCollection<AuthorRecord>();
            foreach (var data in _repository.GetAll())
            {
                var authorRecord = new AuthorRecord()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    BirthYear = data.BirthYear
                };
                AuthorRecord.AuthorRecords.Add(authorRecord);
            }
        }
    }
}