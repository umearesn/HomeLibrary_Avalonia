﻿using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class AuthorsAsString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> authorsList = value as List<string>;
            if (authorsList == null || authorsList.Count == 0)
                return "No data about authors.";

            string res = authorsList[0];
            int i = 1;
            while (i < authorsList.Count)
            {
                res += $", {authorsList[i]}";
                i++;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
          => throw new NotImplementedException();
    }

    public class FilePathAsSplittedString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = string.Empty;
            using (StreamWriter sw = new StreamWriter("splitter_debug.txt", true))
            {
                
                string path = value as string;
                if (path != null && path.Length > 0)
                {
                    string[] parts = path.Split('/');
                    sw.WriteLine("parts");
                    foreach (var item in parts)
                    {
                        sw.WriteLine(item);
                        //sw.WriteLine();
                    }
                    
                    sw.WriteLine();
                    List<string> segments = new List<string>();
                    int currentSegment = 0;
                    segments.Add(parts[0]);
                    for (int i = 1; i < parts.Length; i++)
                    {
                        while (segments[currentSegment].Length < 30 && i < parts.Length)
                        {
                            sw.WriteLine($"segment length: {segments[currentSegment].Length}");
                            segments[currentSegment] += $"/{parts[i]}";
                            i++;
                        }
                        if (i < parts.Length - 1)
                        {
                            segments[currentSegment] += Environment.NewLine;
                            sw.WriteLine($"{segments[currentSegment]}: {segments[currentSegment].Length}");
                            currentSegment++;
                            segments.Add("\t/");
                        }
                    }
                    foreach (var item in segments)
                    {
                        res += item;
                    }
                    sw.WriteLine(res);
                    sw.WriteLine();
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
          => throw new NotImplementedException();
    }

}
