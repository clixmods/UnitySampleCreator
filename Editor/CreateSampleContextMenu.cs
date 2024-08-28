using UnityEditor;
using UnityEngine;

namespace SampleCreator.Editor
{
    public static class CreateSampleContextMenu
    {
        [MenuItem("Assets/Create Samples for this folder...", true)]
        private static bool IsFolderSelected()
        {
            // Get the current selected folder in Assets window
            string folderPath = GetSelectedFolderPath();
            return !string.IsNullOrEmpty(folderPath);
        }
        
        [MenuItem("Assets/Create Samples for this folder...", false, 20)]
        private static void CreateSamples()
        {
            string folderPath = GetSelectedFolderPath();

            if (!string.IsNullOrEmpty(folderPath))
            {
                PackageSelectorWindow.ShowWindow(new SampleCreationInfo()
                {
                    DisplayName = "Sample",
                    Description = "Sample description",
                    Path = folderPath
                });
                
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning("No folder selected. Please select a folder in the Project view.");
            }
        }
        
        private static string GetSelectedFolderPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                if (System.IO.Directory.Exists(path))
                {
                    return path;
                }
                else
                {
                    path = System.IO.Path.GetDirectoryName(path);
                    if (System.IO.Directory.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return null;
        }
    }
}