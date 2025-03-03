using System.IO;
using System.Text.Json;

namespace View.Model.Services
{
    public static class ContactSerializer
    {
        private static readonly string FilePath = "contacts.json";

        public static void SaveContacts(IEnumerable<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts);
            File.WriteAllText(FilePath, json);
        }

        public static List<Contact> LoadContacts()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Contact>>(json);
            }
            return new List<Contact>();
        }
    }
}