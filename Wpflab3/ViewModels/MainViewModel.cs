using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Wpflab3.Models;

namespace Wpflab3.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private ObservableCollection<RegistrationProgram> _programs;
        private ObservableCollection<RegistrationProgram> _filteredPrograms;
        private string _selectedRegularity;

        public ObservableCollection<RegistrationProgram> Programs
        {
            get => _programs;
            set
            {
                _programs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RegistrationProgram> FilteredPrograms
        {
            get => _filteredPrograms;
            set
            {
                _filteredPrograms = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRegularity
        {
            get => _selectedRegularity;
            set
            {
                    _selectedRegularity = value;
                    OnPropertyChanged();
                    ExecuteFilter(value);
            }
        }

        internal RelayCommand FilterCommand { get; }

        public MainViewModel(string jwtToken)
        {
            Initialize(jwtToken);
            FilterCommand = new RelayCommand(ExecuteFilter);
        }

        private async void Initialize(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
            using var response = await _httpClient.GetAsync("http://localhost:5000/api/RegistrationProgram");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var registrationPrograms = await response.Content.ReadFromJsonAsync<List<RegistrationProgram>>();
            Programs = new ObservableCollection<RegistrationProgram>(registrationPrograms);
            FilteredPrograms = new ObservableCollection<RegistrationProgram>(Programs);
        }

        private void ExecuteFilter(object parameter)
        {
            if (parameter is string regularityStr && int.TryParse(regularityStr, out var regularity))
            {
                FilteredPrograms = new ObservableCollection<RegistrationProgram>(Programs.Where(p => p.Regularity == regularity).OrderBy(p => p.Price)
                );
            }
            else
            {
                FilteredPrograms = new ObservableCollection<RegistrationProgram>(Programs);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}