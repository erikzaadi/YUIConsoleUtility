using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YUICompressor
{
    class Program
    {
        #region Properties

        private const string MinimizedExtension = "min";
        private const string JavaScriptExtension = "js";
        private const string CSSExtension = "css";
        private const string ReplaceArg = "-replace";
        private const string IgnoreArg = "-ignore:";
        private const string HelpArgSimple = "?";
        private const string HelpArg = "help";

        private static char[] IgnoreSplitter = new char[] { ',' };
        private static string AppName
        {
            get { return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }
        private static int BuildNumber
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; }
        }
        private static int VersionNumber
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major; }
        }

        #endregion

        static void Main(string[] args)
        {
            if (args == null ||
                args.Length == 0 ||
                    (
                        args.Length == 1 &&
                        (
                        string.Compare(args[0], HelpArg, true) == 0 ||
                        string.Compare(args[0], HelpArgSimple, true) == 0
                        )
                    )
                )
            {
                ShowHelp();
                return;
            }
            DateTime started = DateTime.Now;
            bool replace = false;
            List<string> ignoreLists = new List<string>();
            foreach (string arg in args)
            {
                if (string.Compare(arg, ReplaceArg, true) == 0)
                    replace = true;
                if (arg.StartsWith(IgnoreArg, StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] ignores = arg.Replace(IgnoreArg, "").Split(IgnoreSplitter, StringSplitOptions.RemoveEmptyEntries);
                    if (ignores != null && ignores.Length > 0)
                    {
                        foreach (string igore in ignores)
                        {
                            ignoreLists.Add(StripQuoutes(igore));
                        }
                    }
                }
            }

            string dir = StripQuoutes(args[0]);

            if (!Directory.Exists(dir))
            {
                ShowNoDirError(dir);
                return;
            }


            string[] jsFiles = Directory.GetFiles(dir,
                string.Format("*.{0}", JavaScriptExtension),
                SearchOption.AllDirectories);
            string[] cssFiles = Directory.GetFiles(dir, 
                string.Format("*.{0}", CSSExtension), 
                SearchOption.AllDirectories);

            if ((jsFiles == null || jsFiles.Length == 0) &&
                (cssFiles == null || cssFiles.Length == 0))
            {
                ShowNoFilesToCompressError(dir);
                return;
            }

            int writtenJS = 0;
            int writtenCSS = 0;
            
            if (jsFiles != null)
                writtenJS = CompressFiles(jsFiles, 
                    JavaScriptExtension, 
                    ignoreLists, 
                    replace);
            if (cssFiles != null)
                writtenCSS = CompressFiles(cssFiles, 
                    CSSExtension, 
                    ignoreLists, 
                    replace);
            
            string totalTime = "";
            TimeSpan t = DateTime.Now.Subtract(started);
            totalTime = string.Format("{0} seconds", t.Seconds);

            ShowSummary(dir, 
                writtenJS, 
                writtenCSS, 
                totalTime, 
                ignoreLists.Count);
        }

        #region Console "UI" Helpers
    
        private static void ShowNoFilesToCompressError(string inDir)
        {
            WriteIntro();
            Console.WriteLine("The directory '{0}' has no {1} or {2} files",
                inDir,
                JavaScriptExtension,
                CSSExtension);
            WriteUseHelp();
        }

        private static void ShowNoDirError(string inDir)
        {
            WriteIntro();
            Console.WriteLine("The directory '{0}' was not found", inDir);
            WriteUseHelp();
        }

        private static void ShowHelp()
        {
            WriteIntro();
            Console.WriteLine("Usage : ");
            Console.WriteLine("{0} \"path with spaces\"", AppName);
            Console.WriteLine("Arguments :");
            Console.WriteLine(ReplaceArg);
            Console.WriteLine("     {0} is optional, and will", ReplaceArg);
            Console.WriteLine("     replace all {0} and {1}",
                JavaScriptExtension,
                CSSExtension);
            Console.WriteLine("     with the minified version.", CSSExtension);
            Console.WriteLine("     By default, the compressor will");
            Console.WriteLine("     create a new file called : ");
            Console.WriteLine("         filename.{0}.{1}|{2}",
                MinimizedExtension,
                CSSExtension,
                JavaScriptExtension);
            Console.WriteLine("");
            Console.WriteLine(IgnoreArg);
            Console.WriteLine("     {0}file1{1}\"file with spaces\"",
                IgnoreArg,
                IgnoreSplitter[0]);
            Console.WriteLine("     {0} is also optional, ", IgnoreArg);
            Console.WriteLine("     followed by a '{0}' separated list", IgnoreSplitter[0]);
            Console.WriteLine("");
            Console.WriteLine("");
        }

        private static void WriteIntro()
        {
            Console.WriteLine("***************************************");
            Console.WriteLine("YUI Compressor Utility Version : {0} Build : {1}",
                VersionNumber,
                BuildNumber);
            Console.WriteLine("***************************************");
            Console.WriteLine("");
        }

        private static void WriteUseHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("Use {0} {1} or {2} for help",
                AppName,
                HelpArg,
                HelpArgSimple);
            Console.WriteLine("");
            Console.WriteLine("");
        }

        private static void ShowSummary(string dir, 
            int jsFilesWritten, 
            int cssFilesWritten, 
            string seconds, 
            int ignoreLength)
        {
            WriteIntro();
            Console.WriteLine("Compressed the directory '{0}'", dir);
            Console.WriteLine("JavaScript files compressed : {0}", jsFilesWritten);
            Console.WriteLine("Stylesheet files compressed : {0}", cssFilesWritten);
            Console.WriteLine("Ignored files : {0}", ignoreLength);
            Console.WriteLine("");
            Console.WriteLine("Total time : {0}", seconds);
            Console.WriteLine("");
        }

        #endregion Console "UI" Helpers

        #region Helpers

        private static string GetReplacedPath(string inFile, string inExtension)
        {
            return string.Format("{0}.{1}.{2}",
                Path.Combine(Path.GetDirectoryName(inFile), Path.GetFileNameWithoutExtension(inFile)),
                MinimizedExtension,
                inExtension);
        }

        private static string StripQuoutes(string inString)
        {
            if (inString.StartsWith("\""))
                inString = inString.Remove(0, 1);
            if (inString.EndsWith("\""))
                inString = inString.Remove(inString.Length - 1, 1);
            return inString;
        }

        private static int CompressFiles(string[] inFiles, 
            string inExtension, 
            List<string> ignoreList, 
            bool inReplace)
        {
            int written = 0;
            foreach (string file in inFiles)
            {
                if (file.EndsWith(string.Format("{0}.{1}",
                    MinimizedExtension,
                    inExtension), StringComparison.InvariantCultureIgnoreCase))
                    continue;
                if (ignoreList.FindIndex(new Predicate<string>(
                    delegate(string ignore)
                    {
                        return Path.GetFileName(file).IndexOf(ignore, StringComparison.InvariantCultureIgnoreCase) != -1;
                    })
                    ) != -1)
                    continue;
                string fileContent = File.ReadAllText(file);
                if (string.IsNullOrEmpty(fileContent))
                    continue;
                if (inExtension == JavaScriptExtension)
                    fileContent = Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(fileContent);
                else
                    fileContent = Yahoo.Yui.Compressor.CssCompressor.Compress(fileContent);
                string path = inReplace ? file : GetReplacedPath(file, inExtension);
                File.WriteAllText(path, fileContent);
                written++;
            }
            return written;
        }

        #endregion Helpers
    }
}
