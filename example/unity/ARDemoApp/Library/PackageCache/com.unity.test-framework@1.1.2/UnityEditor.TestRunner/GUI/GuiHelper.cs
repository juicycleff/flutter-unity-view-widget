using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.GUI
{
    internal class GuiHelper : IGuiHelper
    {
        public GuiHelper(IMonoCecilHelper monoCecilHelper, IAssetsDatabaseHelper assetsDatabaseHelper)
        {
            MonoCecilHelper = monoCecilHelper;
            AssetsDatabaseHelper = assetsDatabaseHelper;
        }

        protected IMonoCecilHelper MonoCecilHelper { get; private set; }
        public IAssetsDatabaseHelper AssetsDatabaseHelper { get; private set; }

        public void OpenScriptInExternalEditor(Type type, MethodInfo method)
        {
            var fileOpenInfo = GetFileOpenInfo(type, method);

            if (string.IsNullOrEmpty(fileOpenInfo.FilePath))
            {
                Debug.LogWarning("Failed to open test method source code in external editor. Inconsistent filename and yield return operator in target method.");

                return;
            }

            if (fileOpenInfo.LineNumber == 1)
            {
                Debug.LogWarning("Failed to get a line number for unity test method. So please find it in opened file in external editor.");
            }

            AssetsDatabaseHelper.OpenAssetInItsDefaultExternalEditor(fileOpenInfo.FilePath, fileOpenInfo.LineNumber);
        }

        public IFileOpenInfo GetFileOpenInfo(Type type, MethodInfo method)
        {
            const string fileExtension = ".cs";

            var fileOpenInfo = MonoCecilHelper.TryGetCecilFileOpenInfo(type, method);
            if (string.IsNullOrEmpty(fileOpenInfo.FilePath))
            {
                var dirPath = Paths.UnifyDirectorySeparator(Application.dataPath);
                var allCsFiles = Directory.GetFiles(dirPath, string.Format("*{0}", fileExtension), SearchOption.AllDirectories)
                    .Select(Paths.UnifyDirectorySeparator);

                var fileName = allCsFiles.FirstOrDefault(x =>
                    x.Split(Path.DirectorySeparatorChar).Last().Equals(string.Concat(type.Name, fileExtension)));

                fileOpenInfo.FilePath = fileName ?? string.Empty;
            }

            fileOpenInfo.FilePath = FilePathToAssetsRelativeAndUnified(fileOpenInfo.FilePath);

            return fileOpenInfo;
        }

        public string FilePathToAssetsRelativeAndUnified(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            filePath = Paths.UnifyDirectorySeparator(filePath);
            var length = Paths.UnifyDirectorySeparator(Application.dataPath).Length - "Assets".Length;

            return filePath.Substring(length);
        }

        public bool OpenScriptInExternalEditor(string stacktrace)
        {
            if (string.IsNullOrEmpty(stacktrace))
                return false;

            var regex = new Regex("in (?<path>.*):{1}(?<line>[0-9]+)");

            var matchingLines = stacktrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Where(x => regex.IsMatch(x)).ToList();
            if (!matchingLines.Any())
                return false;

            var fileOpenInfo = matchingLines
                .Select(x => regex.Match(x))
                .Select(x =>
                    new FileOpenInfo
                    {
                        FilePath = x.Groups["path"].Value,
                        LineNumber = int.Parse(x.Groups["line"].Value)
                    })
                .First(openInfo => !string.IsNullOrEmpty(openInfo.FilePath) && File.Exists(openInfo.FilePath));

            var filePath = FilePathToAssetsRelativeAndUnified(fileOpenInfo.FilePath);
            AssetsDatabaseHelper.OpenAssetInItsDefaultExternalEditor(filePath, fileOpenInfo.LineNumber);

            return true;
        }
    }
}
