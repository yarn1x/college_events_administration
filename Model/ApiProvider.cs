using college_events_desktop.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace college_events_desktop.Model.ApiProvider
{
    public class ApiClient
    {
        private readonly HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7280/college/admin/")
        };

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


        /// <summary>
        /// GET Запрос к API
        /// </summary>
        /// <returns>
        /// Список мероприятий. Новое мероприятие добавляется в начало списка
        /// </returns>
        internal async Task<Stack<Event>> GetAllEventsAsync() => await GetAsync<Stack<Event>>("events");

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
