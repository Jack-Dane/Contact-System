using System;
using System.Collections.Generic;
using System.Text;
using ContactSystem.Core.Database;

namespace MessageApp.Core.Models
{
    public class Contact : IContact
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        private IContactTbl _contactDatabase;

        public Contact(IContactTbl ContactDatabase, int? id)
        {
            _contactDatabase = ContactDatabase;
            Id = id;
        }

        public void Save()
        {
            // if no id, means that the record is newly created, so save
            // otherwise, update the already existing record
            if (Id.HasValue)
            {
                _contactDatabase.UpdateContact(this);
            }
            else
            {
                int id = _contactDatabase.SaveContact(this);
                Id = id;
            }
        }

        public void Delete()
        {
            // Delete the record, if the id has a value and thus exists in the database
            if (Id.HasValue)
            {
                _contactDatabase.DeleteContact(this);
            }
        }
    }
}
