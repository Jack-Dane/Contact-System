using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageApp.Core.Models;

namespace ContactSystem.Core.Database
{
    public interface IContactTbl
    {
        // CRUD
        int SaveContact(IContact contact);
        void UpdateContact(IContact contact);
        void DeleteContact(IContact contact);
        List<IContact> GetAllContacts();
    }
}
