using System;
using System.IO;
using Newtonsoft.Json;

namespace View.Model.Services
{
    /// <summary>
    /// Статический класс для сериализации и десериализации контактов.
    /// </summary>
    public static class ContactSerializer
    {
        /// <summary>
        /// Статический конструктор для инициализации пути к файлу и создания каталога.
        /// </summary>
        static ContactSerializer()
        {
            FilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Contacts",
                "contacts.json"
            );

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        }

        /// <summary>
        /// Путь к файлу, в котором хранятся контакты.
        /// </summary>
        public static string FilePath { get; }

        /// <summary>
        /// Сохраняет контакт в файл.
        /// </summary>
        /// <param name="contact">Объект контакта для сохранения.</param>
        /// <exception cref="ArgumentNullException">Контакт не может быть пустым</exception>
        public static void SaveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact) + "Контакт не может быть пустым");
            }

            string json = JsonConvert.SerializeObject(contact, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Загружает контакт из файла.
        /// </summary>
        /// <returns>Объект <see cref="Contact"/>, если файл существует, иначе null.</returns>
        public static Contact LoadContact()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            string json = File.ReadAllText(FilePath);
            Contact contact = JsonConvert.DeserializeObject<Contact>(json);
            return contact;
        }
    }
}