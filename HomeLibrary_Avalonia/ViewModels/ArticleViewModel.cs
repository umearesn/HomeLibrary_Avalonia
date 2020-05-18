using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Services;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class ArticleViewModel : ReactiveObject
    {
        private LocalLibraryViewModel masterLibrary = null;

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

        private bool canBeAdded;
        public bool CanBeAdded
        {
            get => canBeAdded;
            set => this.RaiseAndSetIfChanged(ref canBeAdded, value);
        }

        public ReactiveCommand<Unit, Task> AddToTheLibrary { get; }

        public ReactiveCommand<Unit, Task> DeleteFromTheLibrary { get; }

        public ReactiveCommand<Unit, Unit> OpenFile { get; }

        public ArticleViewModel(ArticleObject article, LocalLibraryViewModel parentLibrary = null)
        {
            Article = article;
            PdfPath = article.PdfPath;
            masterLibrary = parentLibrary;
            if (parentLibrary != null || !Directory.Exists(SettingsService.GetDirectoryInfo()))
            {
                CanBeAdded = false;
            }
            else
            {
                Task.Run(async () =>
                {
                    CanBeAdded = !(await DatabaseService.CheckIfPresentedAsync(Article));
                });
            }
            if (article.Fulltext != null)
            {
                IsFullTextPresented = true;
            }

            AddToTheLibrary = ReactiveCommand.Create(async () =>
            {
                var responseMessage
                    = await SearchService.GetFulltextArticle(Article.Id);

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    ArticleObject article = responseMessage.Item2.Data[0];
                    using(var sw = new StreamWriter("LOOKHERE.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now} -- clicked {Article.Title}, {Article.DownloadUrl}");
                    }
                    string loadingLink = article.DownloadUrl;

                    if (loadingLink != null)
                    {
                        article.PdfPath = GetName(article, IsCoreLink(loadingLink));
                        PdfPath = article.PdfPath;
                        using (var sw = new StreamWriter("LOOKHERE.txt", true))
                        {
                            sw.WriteLine($"{DateTime.Now} -- clicked {Article.Title}, {PdfPath}");
                        }
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

                    using (var sw = new StreamWriter("LOOKHERE.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now} -- before put.");
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
                CanBeAdded = false;
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
                catch (Exception ex)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter("log.txt", true))
                        {
                            sw.WriteLine($"{DateTime.Now}: Deletion failed - {ex.Message}.");
                        }
                    }
                    catch { }
                }
                await masterLibrary.RemoveAndUpdateList(this);
            });

            OpenFile = ReactiveCommand.Create(() =>
            {
                try
                {
                    string path = Path.GetFullPath(pdfPath);
                    if (File.Exists(path))
                    {
                        Process.Start("explorer.exe", @$"{path}");
                    }
                    else
                    {
                        PdfPath = "File is not found!";
                    }
                }
                catch
                {
                    PdfPath = "OpeningFailed!";
                }
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
                    await SearchService.LoadPdf(PdfPath, loadingLink);
                }
                catch (Exception ex)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter("log.txt", true))
                        {
                            sw.WriteLine($"{DateTime.Now}: Loading PDF failed - {ex.Message}.");
                        }
                    }
                    catch { }
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
                authors = $"-{article.Authors[0]}";
            }
            authors.Trim();
            authors = Regex.Replace(authors, @"\t|\n|\r", "");

            string title = Regex.Replace(article.Title, @"\t|\n|\r", "");

            if (title.Length > 50)
            {
                title = title.Substring(0, 50);
                int i = 49;
                while (title[i] != ' ')
                {
                    i--;
                }
                title = title.Substring(0, i + 1);
            }

            string result = $"{title}-{authors}-{DateTime.Now.Ticks}";
            if (isCoreLink)
            {
                result = $"{result}.pdf";
            }
            else
            {
                result = $"{result}.txt";
            }
            result = RemoveProhibitedSymbolsWindows(result);
            return $"{directory}\\{result}";
        }

        private string RemoveProhibitedSymbolsWindows(string name)
        {
            char[] prohibited = { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '+', '$', '_', '{', '}' };
            string result = name;
            foreach (var item in prohibited)
            {
                result = result.Replace(item.ToString(), string.Empty);
            }
            result = result.Replace(',', '-');
            return result;
        }

    }
}
