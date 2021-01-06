namespace MessageApp.Core.Models
{
    public interface IContact
    {
        int? Id { get; set; }
        string FullName { get; set; }
        string Address { get; set; }
        string Town { get; set; }
        string PostCode { get; set; }
        string Mobile { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        void Save();
        void Delete();
    }
}
