namespace PulseMates.Infrastructure.Optimization
{
    using dotless.Core;
    using dotless.Core.Abstractions;
    using dotless.Core.Importers;
    using dotless.Core.Input;
    using dotless.Core.Loggers;
    using dotless.Core.Parser;
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Optimization;

    public class DotlessTransform : IBundleTransform
    {
        #region IBundleTransform Members

        public void Process(BundleContext context, BundleResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();

            var lessParser = new Parser();
            ILessEngine lessEngine = CreateLessEngine(lessParser);

            var content = new StringBuilder(response.Content.Length);

            foreach (FileInfo file in response.Files)
            {
                SetCurrentFilePath(lessParser, file.FullName);
                string source = File.ReadAllText(file.FullName);
                content.Append(lessEngine.TransformToCss(source, file.FullName));
                content.AppendLine();

                AddFileDependencies(lessParser);
            }

            response.ContentType = "text/css";
            response.Content = content.ToString();
        }

        #endregion

        /// <summary>
        /// Creates an instance of LESS engine.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private ILessEngine CreateLessEngine(Parser lessParser)
        {
            var logger = new AspNetTraceLogger(LogLevel.Debug, new Http());
            return new LessEngine(lessParser, logger, true, false);
        }

        /// <summary>
        /// Adds imported files to the collection of files on which the current response is dependent.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private void AddFileDependencies(Parser lessParser)
        {
            IPathResolver pathResolver = GetPathResolver(lessParser);

            foreach (string importedFilePath in lessParser.Importer.Imports)
            {
                string fullPath = pathResolver.GetFullPath(importedFilePath);
                HttpContext.Current.Response.AddFileDependency(fullPath);
            }

            lessParser.Importer.Imports.Clear();
        }

        /// <summary>
        /// Returns an <see cref="IPathResolver"/> instance used by the specified LESS lessParser.
        /// </summary>
        /// <param name="lessParser">The LESS prser.</param>
        private IPathResolver GetPathResolver(Parser lessParser)
        {
            var importer = lessParser.Importer as Importer;
            if (importer != null)
            {
                var fileReader = importer.FileReader as FileReader;
                if (fileReader != null)
                {
                    return fileReader.PathResolver;
                }
            }

            return null;
        }

        /// <summary>
        /// Informs the LESS parser about the path to the currently processed file. 
        /// This is done by using custom <see cref="IPathResolver"/> implementation.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <param name="currentFilePath">The path to the currently processed file.</param>
        private void SetCurrentFilePath(Parser lessParser, string currentFilePath)
        {
            var importer = lessParser.Importer as Importer;
            if (importer != null)
            {
                var fileReader = importer.FileReader as FileReader;

                if (fileReader == null)
                {
                    importer.FileReader = fileReader = new FileReader();
                }

                var pathResolver = fileReader.PathResolver as ImportedFilePathResolver;

                if (pathResolver != null)
                {
                    pathResolver.CurrentFilePath = currentFilePath;
                }
                else
                {
                    fileReader.PathResolver = new ImportedFilePathResolver(currentFilePath);
                }
            }
            else
            {
                throw new InvalidOperationException("Unexpected importer type on dotless parser");
            }
        }

        class ImportedFilePathResolver : IPathResolver
        {
            private string _currentFileDirectory;
            private string _currentFilePath;

            public ImportedFilePathResolver(string currentFilePath)
            {
                CurrentFilePath = currentFilePath;
            }

            /// <summary>
            /// Gets or sets the path to the currently processed file.
            /// </summary>
            public string CurrentFilePath
            {
                get { return _currentFilePath; }
                set
                {
                    _currentFilePath = value;
                    _currentFileDirectory = Path.GetDirectoryName(value);
                }
            }

            /// <summary>
            /// Returns the absolute path for the specified improted file path.
            /// </summary>
            /// <param name="filePath">The imported file path.</param>
            public string GetFullPath(string filePath)
            {
                filePath = filePath.Replace('\\', '/').Trim();

                if (filePath.StartsWith("~"))
                {
                    filePath = VirtualPathUtility.ToAbsolute(filePath);
                }

                if (filePath.StartsWith("/"))
                {
                    filePath = HostingEnvironment.MapPath(filePath);
                }
                else if (!Path.IsPathRooted(filePath))
                {
                    filePath = Path.Combine(_currentFileDirectory, filePath);
                }

                return filePath;
            }
        }
    }
}