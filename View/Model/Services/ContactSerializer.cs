using System.IO;
using System.Text.Json;

namespace View.Model.Services
{
    public class ContactSerializer
    {
        private readonly string _filePath;

        public void SaveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact) + "Контакт не может быть пустым");
            }

            string json = JsonSerializer.Serialize(contact);
            File.WriteAllText(_filePath, json);
        }

        public Contact LoadContact()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            string json = File.ReadAllText(_filePath);
            Contact contact = JsonSerializer.Deserialize<Contact>(json);
            return contact;
        }

        public ContactSerializer()
        {
            _filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Contacts",
                "contacts.json"
            );

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }
    }
}
