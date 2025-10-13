using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Fangtang
{
    public class ProjectUtil : Editor
    {
//        [MenuItem("Fangtang/Project/Delete all name unformed .mat files")]
//        private static void DeleteAllUnunformateFile()
//        {
//            if (EditorUtility.DisplayDialog("Delete unformed .mat File",
//                "Are you sure to delete all unformed .mat File in current project?", "OK", "Cancel"))
//            {
//                DoDeleteAllUnformateMATFile();
//            }
//        }
//
//        [MenuItem("Fangtang/Project/Search name unformed .mat files")]
//        private static void SearchAllUnunformateFile()
//        {
//            DoSearchAllUnformateMATFile();
//        }

        [MenuItem("SoyBean/Project/Delete all empty folder and orig files")]
        private static void DeleteAllEmptyFolderAndOrigFile()
        {
            if (EditorUtility.DisplayDialog("Delete empty folder and orgFile",
                "Are you sure to delete all empty folder and orgFile in current project?", "OK", "Cancel"))
            {
                DoDeleteAllOrig();
                DeleteCurrentDirectoryFolder(Application.dataPath + "/");
                AssetDatabase.Refresh();
            }
        }

        private static List<string> DoSearchAllUnformateMATFile()
        {
            List<string> result = new List<string>();
            string[] filePaths = Directory.GetFiles(Application.dataPath + "/Artworks/", "*.mat", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                string[] splitStings = fileName.Split('_');
                if (splitStings[splitStings.Length - 1].Length != 1)
                {
                    result.Add(filePath);
                    Debug.Log(filePath);
                }
            }
            return result;
        }

        private static void DoDeleteAllUnformateMATFile()
        {
            List<string> filePaths = DoSearchAllUnformateMATFile();

            foreach (var filePath in filePaths)
            {
                FileUtil.DeleteFileOrDirectory(filePath);
                FileUtil.DeleteFileOrDirectory(filePath + ".meta");
            }
            AssetDatabase.Refresh();
        }

        private static void DeleteCurrentDirectoryFolder(string paths)
        {
            string[] directoriesPaths = Directory.GetDirectories(paths);
            for (int i = 0; i < directoriesPaths.Length; i++)
            {
                DeleteCurrentDirectoryFolder(directoriesPaths[i]);
            }

            string[] filePaths = Directory.GetFiles(paths);
            bool isEmpty = true;

            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = Path.GetFileName(filePaths[i]);

                if (fileName != ".DS_Store" &&
                    fileName != "Thumbs.db" &&
                    fileName != "Desktop.ini")
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty)
            {
                FileUtil.DeleteFileOrDirectory(paths);
                FileUtil.DeleteFileOrDirectory(paths + ".meta");
                Debug.Log("Deleted folder " + paths);
            }
        }

        [MenuItem("SoyBean/Project/Delete All orig Files")]
        private static void DeleteAllOrigFiles()
        {
            if (EditorUtility.DisplayDialog("Delete orig files",
                "Are you sure to delete all orig files in current project?", "OK", "Cancel"))
            {
                DoDeleteAllOrig();
            }
        }

        private static void DoDeleteAllOrig()
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath + "/", "*.orig", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                FileUtil.DeleteFileOrDirectory(filePath);
                FileUtil.DeleteFileOrDirectory(filePath + ".meta");
                Debug.Log("Deleted file " + filePath);
            }
            AssetDatabase.Refresh();

        }
    }
}
