using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YUICompressor
{
    class Program
    {
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


            string[] jsFiles = Directory.GetFiles(dir, string.Format("*.{0}", JavaScriptExtension), SearchOption.AllDirectories);
            string[] cssFiles = Directory.GetFiles(dir, string.Format("*.{0}", CSSExtension), SearchOption.AllDirectories);

            int writtenJS = 0;
            int writtenCSS = 0;

            if (
                (jsFiles == null || jsFiles.Length == 0) &&
                (cssFiles == null || cssFiles.Length == 0))
            {
                ShowNoFilesToCompressError(dir);
                return;
            }
            if (jsFiles != null)
            {
                foreach (string jsFile in jsFiles)
                {
                    if (jsFile.EndsWith(string.Format("{0}.{1}",
                        MinimizedExtension,
                        JavaScriptExtension), StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    if (ignoreLists.FindIndex(new Predicate<string>(
                        delegate(string ignore)
                        {
                            return Path.GetFileName(jsFile).IndexOf(ignore, StringComparison.InvariantCultureIgnoreCase) != -1;
                        })
                        ) != -1)
                        continue;
                    string jsContent = File.ReadAllText(jsFile);
                    if (string.IsNullOrEmpty(jsContent))
                        continue;
                    jsContent = Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(jsContent);
                    string path = replace ? jsFile : GetReplacedPath(jsFile, JavaScriptExtension);
                    File.WriteAllText(path, jsContent);
                    writtenJS++;
                }
            }
            if (cssFiles != null)
            {
                foreach (string cssFile in cssFiles)
                {
                    if (cssFile.EndsWith(string.Format("{0}.{1}",
                        MinimizedExtension,
                        CSSExtension), StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    if (ignoreLists.FindIndex(new Predicate<string>(
                        delegate(string ignore)
                        {
                            return Path.GetFileName(cssFile).IndexOf(ignore, StringComparison.InvariantCultureIgnoreCase) != -1;
                        })
                        ) != -1)
                        continue;
                    string cssContent = File.ReadAllText(cssFile);
                    if (string.IsNullOrEmpty(cssContent))
                        continue;
                    cssContent = Yahoo.Yui.Compressor.CssCompressor.Compress(cssContent);
                    string path = replace ? cssFile : GetReplacedPath(cssFile, CSSExtension);
                    File.WriteAllText(path, cssContent);
                    writtenCSS++;
                }
            }
            ShowSummary(dir, writtenJS, writtenCSS, started, ignoreLists.Count);

        }

        private static void ShowSummary(string dir, int jsFilesWritten, int cssFilesWritten, DateTime started, int ignoreLength)
        {
            string totalTime = "";
            TimeSpan t = DateTime.Now.Subtract(started);
            totalTime = string.Format("{0} seconds", t.Seconds);
            Console.WriteLine("***************************************");
            Console.WriteLine("YUI Compressor Version : {0} Build : {1}",
                VersionNumber,
                BuildNumber);
            Console.WriteLine("***************************************");
            Console.WriteLine("");
            Console.WriteLine("Compressed the directory '{0}'", dir);
            Console.WriteLine("JavaScript files compressed : {0}", jsFilesWritten);
            Console.WriteLine("Stylesheet files compressed : {0}", cssFilesWritten);
            Console.WriteLine("Ignored files : {0}", ignoreLength);
            Console.WriteLine("");
            Console.WriteLine("Total time : {0}", totalTime);
        }

        private static string GetReplacedPath(string inFile, string inExtension)
        {
            return string.Format("{0}.{1}.{2}",
                Path.Combine(Path.GetDirectoryName(inFile), Path.GetFileNameWithoutExtension(inFile)),
                MinimizedExtension,
                inExtension);
        }

        private static void ShowNoFilesToCompressError(string inDir)
        {
            Console.WriteLine("The directory '{0}' has no javascripts or css files", inDir);
        }

        private static void ShowNoDirError(string inDir)
        {
            Console.WriteLine("The directory '{0}' was not found", inDir);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("***************************************");
            Console.WriteLine("YUI Compressor Version : {0} Build : {1}",
                VersionNumber,
                BuildNumber);
            Console.WriteLine("***************************************");
            Console.WriteLine("");
            Console.WriteLine("Usage : ");
            Console.WriteLine("{0} \"path with spaces\"",AppName);
            Console.WriteLine("Arguments :");
            Console.WriteLine(ReplaceArg);
            Console.WriteLine("     {0} is optional, and will",                ReplaceArg);
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


        private static string StripQuoutes(string inString)
        {
            if (inString.StartsWith("\""))
                inString = inString.Remove(0, 1);
            if (inString.EndsWith("\""))
                inString = inString.Remove(inString.Length - 1, 1);
            return inString;
        }

        private const string MinimizedExtension = "min";
        private const string JavaScriptExtension = "js";
        private const string CSSExtension = "css";
        private const string ReplaceArg = "-replace";
        private const string IgnoreArg = "-ignore:";
        private static char[] IgnoreSplitter = new char[] { ',' };
        private const string HelpArgSimple = "?";
        private const string HelpArg = "help";

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
    }
}
