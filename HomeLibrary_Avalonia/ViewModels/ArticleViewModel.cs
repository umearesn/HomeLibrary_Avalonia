using DynamicData;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Services;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class ArticleViewModel : ReactiveObject
    {
        private LocalLibraryViewModel _masterLibrary = null;

        public ArticleObject Article { get; }

        private bool isFulltextPresented = false;
        public bool IsFullTextPresented
        {
            get => isFulltextPresented;
            set => this.RaiseAndSetIfChanged(ref isFulltextPresented, value);
        }

        private string pdfPath = "PDF is not available.";
        public string PdfPath
        {
            get => pdfPath;
            set => this.RaiseAndSetIfChanged(ref pdfPath, value);
        }

        private bool notAdded;
        public bool NotAdded
        {
            get => notAdded;
            set => this.RaiseAndSetIfChanged(ref notAdded, value);
        }

        public ReactiveCommand<Unit, Task> AddToTheLibrary { get; }

        public ReactiveCommand<Unit, Task> DeleteFromTheLibrary { get; }

        public ReactiveCommand<Unit, Unit> OpenFile { get; }

        public ArticleViewModel(ArticleObject article, LocalLibraryViewModel masterLibrary = null)
        {
            Article = article;
            PdfPath = article.PdfPath;
            _masterLibrary = masterLibrary;
            if(masterLibrary != null)
            {
                NotAdded = false;
            }
            else
            {
                Task.Run(async () =>
                {
                    NotAdded = !(await DatabaseService.CheckIfPresentedAsync(Article));
                });
            }
            if (article.Fulltext != null)
            {
                using(var sw = new StreamWriter("FulltextDebug.txt", true))
                {
                    sw.WriteLine($"{DateTime.Now}: {Article.Title}");
                    sw.WriteLine($"{Article.Fulltext}");
                    sw.WriteLine();
                }
                IsFullTextPresented = true;
            }

            AddToTheLibrary = ReactiveCommand.Create(async () => 
            {
                var responseMessage
                    = await SearchService.GetFulltextArticle(Article.Id);

                using (StreamWriter sw = new StreamWriter("fulltextprinting.txt", true))
                {
                    sw.WriteLine($"{DateTime.Now} article {article.Title}.\nLink: {article.Fulltext}");
                }

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    ArticleObject article = responseMessage.Item2.Data[0];

                    string loadingLink = article.DownloadUrl;

                    using (StreamWriter sw = new StreamWriter("addingStarted.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now} article added. Link: {article.DownloadUrl}");
                    }

                    if (loadingLink != null)
                    {
                        article.PdfPath = GetName(article, IsCoreLink(loadingLink));
                        PdfPath = article.PdfPath;
                        try
                        {
                            LoadPdf(article);
                        }
                        catch
                        {
                            article.PdfPath = "Failed to save!";
                            PdfPath = article.PdfPath;
                        }
                    }

                    await ElasticRepository.PutArticleAsync(article);
                }
                else
                {
                    string loadingLink = Article.DownloadUrl;
                    if (loadingLink != null)
                    {
                        Article.PdfPath = GetName(Article, IsCoreLink(loadingLink));
                        PdfPath = Article.PdfPath;
                        try
                        {
                            LoadPdf(Article);
                        }
                        catch
                        {
                            Article.PdfPath = "Failed to save!";
                            PdfPath = Article.PdfPath;
                        }
                    }

                    await ElasticRepository.PutArticleAsync(Article);
                }
                NotAdded = false;
            });

            DeleteFromTheLibrary = ReactiveCommand.Create(async () =>
            {
                string filePath = Article.PdfPath;

                ElasticRepository.DeleteArticleId(Article.Id);

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch(Exception ex)
                {
                    using(StreamWriter sw = new StreamWriter("DeleteFailure.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now}: {ex.Message}.");
                    }
                }
                await _masterLibrary.RemoveAndUpdateList(this);
            });

            OpenFile = ReactiveCommand.Create(() => 
            {
                string path = Path.GetFullPath(pdfPath);
                Process.Start("explorer.exe", @$"{path}");
            });
        }

        private async void LoadPdf(ArticleObject article)
        {
            string loadingLink = article.DownloadUrl;
            
            if (loadingLink != null)
            {
                try
                {
                    article.PdfPath = GetName(Article, IsCoreLink(loadingLink));
                    PdfPath = article.PdfPath;
                    using (StreamWriter sw = new StreamWriter("startedLoading.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now}: started {article.PdfPath}.");
                        sw.WriteLine(loadingLink);
                        sw.WriteLine();
                    }
                    await SearchService.LoadPdf(PdfPath, loadingLink);
                    using (StreamWriter sw = new StreamWriter("endedLoading.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now}: finished.");
                        sw.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter("loading_debug_ArticleVM.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now}: {ex.Message}.");
                        sw.WriteLine();
                    }
                }
            }
        }

        private bool IsCoreLink(string link)
        {
            return link.Contains("download/pdf");
        }

        private string GetName(ArticleObject article, bool isCoreLink)
        {
            string directory = SettingsService.GetDirectoryInfo();
            string authors = string.Empty;
            if (article.Authors != null && article.Authors.Count > 0)
            {
                if (article.Authors.Count == 1)
                {
                    authors = article.Authors[0];
                }
                for (int i = 1; i < article.Authors.Count; i++)
                {
                    authors += $", {article.Authors[i]}";
                }
            }
            authors.Trim();
            string result = $"{article.Title}-{authors}-{DateTime.Now.Ticks}";
            if (isCoreLink)
            {
                result = $"{result}.pdf";
            }
            else
            {
                result = $"{result}.txt";
            }
            result = RemoveProhibitedSymbolsWindows(result);
            return $"{directory}/{result}";
        }

        private string RemoveProhibitedSymbolsWindows(string name)
        {
            char[] prohibited = {'\\', '/', ':', '*', '?', '"', '<', '>', '|', '+' };
            string result = name;
            foreach (var item in prohibited)
            {
                result = result.Replace(item, ' ');
            }
            result = result.Replace(',', '-');
            return result;
        }

    }
}
