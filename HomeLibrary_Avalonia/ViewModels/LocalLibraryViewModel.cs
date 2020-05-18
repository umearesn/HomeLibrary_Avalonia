using DynamicData;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Services;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class LocalLibraryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "/local_library";

        public IScreen HostScreen { get; }

        private const int _amount = 10;

        private string currentTitle;
        private string currentAuthors;
        private string currentFulltext;

        public LocalLibraryViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            var articlesPartsBinding = localArticlesSource
                .Connect()
                .Bind(out localArticles)
                .DisposeMany()
                .Subscribe();

            if (ElasticRepository.ConnectRepository() != "OK")
            {
                Status = "Connection failed! Check settings and refresh connection!";
                IsStatusEnabled = true;
            }
            else
            {
                LoadDefaultArticlesAsync();
            }

            GoNext = ReactiveCommand.Create(async () =>
            {
                int startIndex = CurrentPage * _amount;
                var response = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, startIndex, _amount + 1);
                DisplayLoaded(response, CurrentPage + 1, _amount);
            });

            GoPrev = ReactiveCommand.Create(async () =>
            {
                int startIndex = Math.Max(0, (CurrentPage - 2) * _amount);
                var response = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, startIndex, _amount + 1);
                DisplayLoaded(response, CurrentPage - 1, _amount);
            });

            Search = ReactiveCommand.Create(async () =>
            {
                currentTitle = TitleField;
                currentAuthors = AuthorsField;
                currentFulltext = FulltextField;

                var response = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, 0, _amount + 1);

                DisplayLoaded(response, 1, _amount);
            });
        }

        private async Task LoadDefaultArticlesAsync()
        {
            currentTitle = TitleField;
            currentAuthors = AuthorsField;
            currentFulltext = FulltextField;
            var response = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, 0, _amount + 1);
            DisplayLoaded(response, 1, _amount);
            IsNavigationVisible = true;
        }

        private void DisplayLoaded(List<ArticleObject> responseMessage, int currentPage, int amountToDisplay)
        {
            if (responseMessage != null && responseMessage.Count > 0)
            {
                localArticlesSource.Clear();
                for (int i = 0; i < Math.Min(_amount, responseMessage.Count); i++)
                {
                    localArticlesSource.Add(new ArticleViewModel(responseMessage[i], this));
                }
                CurrentPage = currentPage;
                if (CurrentPage == 1)
                {
                    IsNavigationBackEnabled = false;
                }
                else
                {
                    IsNavigationBackEnabled = true;
                }
                if (responseMessage.Count < 11)
                {
                    IsNavigationForwardEnabled = false;
                }
                else
                {
                    IsNavigationForwardEnabled = true;
                }
            }
            else
            {
                Status = $"Failed to find any data!";
                IsStatusEnabled = true;
            }
        }

        private bool isNavigationVisible = false;
        public bool IsNavigationVisible
        {
            get => isNavigationVisible;
            set => this.RaiseAndSetIfChanged(ref isNavigationVisible, value);
        }

        private bool isNavigationForwardEnabled = false;
        public bool IsNavigationForwardEnabled
        {
            get => isNavigationForwardEnabled;
            set => this.RaiseAndSetIfChanged(ref isNavigationForwardEnabled, value);
        }

        private bool isNavigationBackEnabled = false;
        public bool IsNavigationBackEnabled
        {
            get => isNavigationBackEnabled;
            set => this.RaiseAndSetIfChanged(ref isNavigationBackEnabled, value);
        }

        private int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set => this.RaiseAndSetIfChanged(ref currentPage, value);
        }

        // Results
        public ReadOnlyObservableCollection<ArticleViewModel> localArticles;

        public SourceList<ArticleViewModel> localArticlesSource
            = new SourceList<ArticleViewModel>();

        public ReactiveCommand<Unit, Task> Search { get; }

        public ReactiveCommand<Unit, Task> GoNext { get; }

        public ReactiveCommand<Unit, Task> GoPrev { get; }

        private string status = null;
        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        // Should status be active
        private bool isStatusEnabled = false;
        public bool IsStatusEnabled
        {
            get => isStatusEnabled;
            set => this.RaiseAndSetIfChanged(ref isStatusEnabled, value);
        }

        public async Task RemoveAndUpdateList(ArticleViewModel articleVM)
        {
            if (localArticlesSource.Count == 1)
            {
                localArticlesSource.Remove(articleVM);
                int startIndex = (CurrentPage - 2) * 10;
                List<ArticleObject> response 
                    = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, startIndex, _amount + 1);
                DisplayLoaded(response, CurrentPage - 1, _amount);
                IsNavigationForwardEnabled = false;

            }
            else
            {
                int startIndex = CurrentPage * 10;
                var response
                    = await DatabaseService.SearchAsync(currentTitle, currentAuthors, currentFulltext, startIndex, 2);
                if (response != null && response.Count > 0)
                {
                    localArticlesSource.Remove(articleVM);
                    localArticlesSource.Add(new ArticleViewModel(response[0], this));
                    if (response.Count == 1)
                    {
                        IsNavigationForwardEnabled = false;
                    }
                    else
                    {
                        IsNavigationForwardEnabled = true;
                    }
                }
                else
                {
                    localArticlesSource.Remove(articleVM);
                    IsNavigationForwardEnabled = false;
                }
            }
        }

        public string TitleField { get; set; }

        public string AuthorsField { get; set; }

        public string FulltextField { get; set; }

    }
}
