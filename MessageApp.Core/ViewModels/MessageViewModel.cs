using MessageApp.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data;
using ContactSystem.Core.Database;
using System.Collections.Generic;

namespace MessageApp.Core.ViewModels
{
    public class MessageViewModel : MvxViewModel
    {
        //button commands used in view
        public IMvxCommand CreateContactCommand { get; set; }
        public IMvxCommand DeleteContactCommand { get; set; }
        public IMvxCommand SaveContactCommand { get; set; }

        //variables
        private List<IContact> _allContacts;
        private ObservableCollection<IContact> _contacts;
        private IContact _selectedItem;
        private string _searchQuery = "";
        private IContact _newContact;

        //database connections
        private IContactTbl _contactConnection;

        public MessageViewModel(IContactTbl contactConnection)
        {
            _contactConnection = contactConnection;

            // get a list of all contacts from database and convert into observable collection
            List<IContact> contactList = _contactConnection.GetAllContacts();
            _allContacts = contactList;
            _contacts = new ObservableCollection<IContact>(contactList);

            CreateContactCommand = new MvxCommand(AddContact);
            DeleteContactCommand = new MvxCommand(DeleteContact);
            SaveContactCommand = new MvxCommand(SaveContact);

            // setting defaul values
            if (_contacts.Count > 0)
            {
                SelectedItem = _contacts[0];
            }

            _newContact = new Contact(_contactConnection, null);
        }

        public IContact SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                // if the user hasn't selected a contact, use the fielded in items
                if (_selectedItem is null)
                {
                    _selectedItem = _newContact;
                }
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public ObservableCollection<IContact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                RaisePropertyChanged(() => Contacts);
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;

                ObservableCollection<IContact> tempContact = new ObservableCollection<IContact>();
                
                foreach (IContact contact in _allContacts)
                {
                    if (contact.FullName.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        tempContact.Add(contact);
                    }
                }

                Contacts = tempContact;

                RaisePropertyChanged(() => SearchQuery);
            }
        }

        public void SaveContact()
        {
            // Get the Contact object and save
            IContact saveContact = _selectedItem;

            saveContact.Save();

            if (!Contacts.Contains(saveContact))
            {
                AddContact(saveContact);
                SelectedItem = saveContact;
            }
        }

        public void AddContact()
        {
            Contact newContact = new Contact(_contactConnection, null);

            AddContact(newContact);
            SelectedItem = newContact;
        }

        public void DeleteContact()
        {
            //get contact that needs to be deleted and removed from list
            IContact deleteContact = _selectedItem;
            RemoveContact(deleteContact);
            deleteContact.Delete();

            //change the select list value
            if (_contacts.Count > 0)
            {
                SelectedItem = _contacts[0];
            }
        }

        private void RemoveContact(IContact contact)
        {
            Contacts.Remove(contact);
            _allContacts.Remove(contact);
        }

        private void AddContact(IContact contact)
        {
            Contacts.Add(contact);
            _allContacts.Add(contact);
        }
    }
}
