﻿using Avalonia.Markup.Xaml;
using DynamicData;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Services;
using Network;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Reactive;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{

    [DataContract]
    public class SearchViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string UrlPathSegment => "/search";

        public SearchViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            var searchPartBinding = searchResultSource
                .Connect()
                .Bind(out searchResult)
                .DisposeMany()
                .Subscribe();

            StartSearching = ReactiveCommand.Create(async () =>
            {

                Page = 1;
                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page, TitleField, AuthorsField);
                DisplayLoaded(responseMessage, Page);
            });


            GoNext = ReactiveCommand.Create(async () =>
            {
                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page + 1, TitleField, AuthorsField);

                DisplayLoaded(responseMessage, Page + 1);
            });

            GoPrev = ReactiveCommand.Create(async () =>
            {
                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page - 1, TitleField, AuthorsField);

                DisplayLoaded(responseMessage, Page - 1);
            });
        }

        private void DisplayLoaded((string, ResponseBody<ArticleObject>) responseMessage, int currentPage)
        {
            if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
            {
                searchResultSource.Clear();
                if (responseMessage.Item2.Data != null)
                {
                    Status = $"TotalHits:{responseMessage.Item2.TotalHits}.";
                    IsStatusEnabled = true;
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(new ArticleViewModel(item));
                    }
                    Page = currentPage;
                    if (Page == 1)
                    {
                        IsNavigationBackEnabled = false;
                    }
                    else
                    {
                        IsNavigationBackEnabled = true;
                    }
                    if (Page == ((responseMessage.Item2.TotalHits + 9) / 10))
                    {
                        IsNavigationForwardEnabled = false;
                    }
                    else
                    {
                        IsNavigationEnabled = true;
                    }
                }
                else
                {
                    Status = $"Failed to find any data!";
                    IsStatusEnabled = true;
                }
            }
            else
            {
                Status = $"Search failed: {responseMessage.Item1}";
                IsStatusEnabled = true;
            }
        }

        // Status info
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

        private int page = 1;
        public int Page
        {
            get => page;
            set => this.RaiseAndSetIfChanged(ref page, value);
        }

        private bool isNavigationEnabled = false;
        public bool IsNavigationEnabled
        {
            get => isNavigationEnabled;
            set => this.RaiseAndSetIfChanged(ref isNavigationEnabled, value);
        }

        private bool isNavigationForwardEnabled = true;
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

        // Results

        public ReadOnlyObservableCollection<ArticleViewModel> searchResult;

        private SourceList<ArticleViewModel> searchResultSource
            = new SourceList<ArticleViewModel>();

        // Navigation
        public ReactiveCommand<Unit, Task> GoNext { get; }
        public ReactiveCommand<Unit, Task> GoPrev { get; }
        //

        public string TitleField { get; set; }

        // 

        public string AuthorsField { get; set; }

        public ReactiveCommand<Unit, Task> StartSearching { get; }

        public ReactiveCommand<Unit, Task> ToTheLibrary { get; }
    }
}