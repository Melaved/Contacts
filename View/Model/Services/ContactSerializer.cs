using System.IO;
using System.Text.Json;

namespace View.Model.Services
{
    /// <summary>
    /// Предоставляет методы для сериализации и десериализации списка контактов в формате JSON.
    /// </summary>
    public static class ContactSerializer
    {
        /// <summary>
        /// Путь к файлу, в который сохраняются контакты.
        /// </summary>
        private static readonly string FilePath = "contacts.json";

        /// <summary>
        /// Сохраняет список контактов в файл в формате JSON.
        /// </summary>
        /// <param name="contacts">Список контактов для сохранения.</param>
        public static void SaveContacts(IEnumerable<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Загружает список контактов из файла JSON.
        /// </summary>
        /// <returns>
        /// Возвращает список контактов, если файл существует и успешно десериализован.
        /// В противном случае возвращает пустой список.</returns>
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