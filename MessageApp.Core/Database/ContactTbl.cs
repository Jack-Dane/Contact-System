using MessageApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ContactSystem.Core.Database
{
    public class ContactTbl : DatabaseConnection, IContactTbl
    {
        // CRUD
        public void DeleteContact(IContact contact)
        {
            int? contactId = contact.Id;

            SqlConnection conn = OpenConnection();

            string sql = @"DELETE FROM tblContact WHERE ContactId = @contactId";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@contactId", contactId);
            command.ExecuteNonQuery();

            command.Dispose();
            CloseConnection(conn);
        }

        public int SaveContact(IContact contact)
        {
            SqlConnection conn = OpenConnection();

            string sql = @"INSERT INTO tblContact (FirstName, LastName, Address, Town, PostCode, Mobile, Phone, Email)"
                            + " VALUES(@firstName, @lastName, @address, @town, @postCode, @mobile, @phone, @email);" +
                            "SELECT SCOPE_IDENTITY()";
            SqlCommand command = SelectUpdateCommand(contact, sql, conn, QueryType.Insert);
            int id = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            CloseConnection(conn);

            return id;
        }

        public void UpdateContact(IContact contact)
        {
            SqlConnection conn = OpenConnection();

            string sql = @"UPDATE tblContact"
                        + " SET FirstName = @firstName, LastName = @lastName, Address = @address, Town = @town, PostCode = @postCode, " +
                        "Mobile = @mobile, Phone = @phone, Email = @email" +
                        " WHERE ContactId = @contactId";
            SqlCommand command = SelectUpdateCommand(contact, sql, conn, QueryType.Update);

            command.ExecuteNonQuery();
            command.Dispose();
            CloseConnection(conn);
        }

        public List<IContact> GetAllContacts()
        {
            List<IContact> contacts = new List<IContact>();

            SqlConnection conn = OpenConnection();
            string sql = "Select * From tblContact";
            SqlCommand command = new SqlCommand(sql, conn);

            int id;
            string fullName;
            string address;
            string town;
            string postCode;
            string mobile;
            string phone;
            string email;
            Contact contact;
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                id = (int)dataReader["ContactId"];
                fullName = dataReader["FirstName"].ToString() + " " + dataReader["LastName"].ToString();
                address = dataReader["Address"].ToString();
                town = dataReader["Town"].ToString();
                postCode = dataReader["PostCode"].ToString();
                mobile = dataReader["Mobile"].ToString();
                phone = dataReader["Phone"].ToString();
                email = dataReader["Email"].ToString();

                contact = new Contact(this, id)
                {
                    Id = id,
                    FullName = fullName,
                    Address = address,
                    Town = town,
                    PostCode = postCode,
                    Mobile = mobile,
                    Phone = phone,
                    Email = email
                };

                contacts.Add(contact);
            }

            dataReader.Close();
            command.Dispose();
            CloseConnection(conn);

            return contacts;
        }

        private SqlCommand SelectUpdateCommand(IContact contact, string sqlQuery, SqlConnection conn, QueryType queryType)
        {
            int? id = contact.Id;
            List<string> fullName = new List<string>(contact.FullName.Split(Convert.ToChar(" ")));
            string firstName = fullName[0];
            string lastName = fullName.Count > 1 ? string.Join(" ", fullName.GetRange(1, fullName.Count - 1)): "";
            string address = contact.Address;
            string town = contact.Town;
            string postCode = contact.PostCode;
            string mobile = contact.Mobile;
            string phone = contact.Phone;
            string email = contact.Email;

            SqlCommand command = new SqlCommand(sqlQuery, conn);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@town", town);
            command.Parameters.AddWithValue("@postCode", postCode);
            command.Parameters.AddWithValue("@mobile", mobile);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@email", email);

            if (queryType == QueryType.Update && id.HasValue)
            {
                command.Parameters.AddWithValue("@contactId", id);
            }

            return command;
        }

        // Query types used in SelectUpdateCommand function
        enum QueryType
        {
            Insert,
            Update
        }
    }
}
