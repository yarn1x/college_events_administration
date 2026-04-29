using college_events_desktop.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace college_events_desktop.Model
{
    public class ApiClient
    {
        public readonly HttpClient _client = new HttpClient()
        {
            //BaseAddress = new Uri("http://192.168.1.253:33679/college/admin/")
            BaseAddress = new Uri("https://localhost:7280/college/admin/")
        };

        private static string ComputeSha256Hash(string rawData)
        {
            // Создаем экземпляр SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем входную строку в массив байтов и вычисляем хэш
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Преобразуем байты хэша в строку в шестнадцатеричном формате
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // формат "x2" — двухсимвольное hex-представление
                }
                return builder.ToString();
            }
        }

        private async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET Api error: {ex}");
                return default;
            }
        }

        public async Task<AuthResponse> LoginAsync(string login, string password)
        {
            var loginData = new { login, passwordHash = password };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(json);
                return authResponse;
            }
            return null;
        }


        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <returns>
        /// Список мероприятий. Новое мероприятие добавляется в начало списка
        /// </returns>
        internal async Task<List<Event>> GetAllEventsAsync() => await GetAsync<List<Event>>("events");

        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <returns>
        /// Список всех категорий (направлений мероприятий).
        /// </returns>
        internal async Task<List<Category>> GetListOfCategories() => await GetAsync<List<Category>>("events/categories");

        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <param name="eventId">Уникальный идентификатор мероприятия</param>
        /// <returns>Список групп, которые закреплены к мероприятию</returns>
        internal async Task<List<EventGroups>> GetEventGroupsByEventId(int eventId) => await GetAsync<List<EventGroups>>($"events/{eventId}/groups");

        /// <summary> GET Запрос к API </summary>
        /// <returns> Список локаций, в которых проходят мероприятия. </returns>
        internal async Task<List<Place>> GetListOfPlaces() => await GetAsync<List<Place>>("events/places");

        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <returns>
        /// Список всех студенческих групп.
        /// </returns>
        internal async Task<List<Group>> GetListOfGroups() => await GetAsync<List<Group>>("groups");

        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <returns>
        /// Список всех организаторов.
        /// </returns>
        internal async Task<List<Organizer>> GetListOfOrganizers() => await GetAsync<List<Organizer>>("organizers");
    }
}
