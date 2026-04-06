using college_events_desktop.DataModels;
using college_events_desktop.Model.ApiProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace college_events_desktop.Model
{
    public class DataService
    {
        internal readonly ApiClient apiClient;

        public Stack<Event> events { get; private set; }
        public List<Category> categories { get; private set; }
        public List<Organizer> organizers {  get; private set; }
        public List<Group> groups { get; private set; }
        public List<Place> places { get; private set; }

        public DataService(ApiClient apiClient)
        {
            this.apiClient = apiClient;
            events = new Stack<Event>();
            categories = new List<Category>();
            organizers = new List<Organizer>();
            groups = new List<Group>();
            places = new List<Place>();
        }

        public async Task LoadEventsAsync()
        {
            events = await apiClient.GetAllEventsAsync();
        }
        public async Task LoadCategoriesAsync()
        {
            categories = await apiClient.GetListOfCategories();
        }
        public async Task LoadOrganizerListAsync()
        {
            organizers = await apiClient.GetListOfOrganizers();
        }
        public async Task LoadGroupsListAsync()
        {
            groups = await apiClient.GetListOfGroups();
        }
        public async Task LoadPlacesListAsync()
        {
            places = await apiClient.GetListOfPlaces();
        }
    }
}
