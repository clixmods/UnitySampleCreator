using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace SampleCreator.Editor
{
    public class PackageSelectorWindow : EditorWindow
    {
        private List<PackageInfo> packages = new List<PackageInfo>();
        private int _selectedPackageIndex = -1;
        private ListRequest _listRequest;
        private Vector2 scrollPosition;
        private SampleCreationInfo _sampleCreationInfo;
        
        public static void ShowWindow(SampleCreationInfo sampleCreationInfo)
        {
            PackageSelectorWindow window = GetWindow<PackageSelectorWindow>("Package Selector");
            window.Show();
            window._sampleCreationInfo = sampleCreationInfo;
            window.RefreshPackageList();
        }

        private void RefreshPackageList()
        {
            _listRequest = Client.List(true);
            EditorApplication.update += Progress;
        }

        private void Progress()
        {
            if (_listRequest.IsCompleted)
            {
                if (_listRequest.Status == StatusCode.Success)
                {
                    packages = _listRequest.Result.ToList();
                }
                else if (_listRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError("Failed to list packages: " + _listRequest.Error.message);
                }

                EditorApplication.update -= Progress;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Select a Package", EditorStyles.boldLabel);

            if (packages.Count > 0)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                for (int i = 0; i < packages.Count; i++)
                {
                    if (GUILayout.Button(packages[i].name, EditorStyles.toolbarButton))
                    {
                        _selectedPackageIndex = i;
                    }
                }

                GUILayout.EndScrollView();

                if (_selectedPackageIndex >= 0)
                {
                    GUILayout.Space(10);

                    GUILayout.Label($"Selected Package: {packages[_selectedPackageIndex].name}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Create Samples for this Package"))
                    {
                        CreateSamplesForSelectedPackage();
                    }
                }
            }
            else
            {
                GUILayout.Label("Loading packages...");
            }
        }

        private void CreateSamplesForSelectedPackage()
        {
            string folderPath = GetSelectedFolderPath();

            if (!string.IsNullOrEmpty(folderPath))
            {
                // Implémentation de la logique pour créer des samples dans le package sélectionné
                Debug.Log($"Creating samples for the selected package: {packages[_selectedPackageIndex].name}");
                
                SampleHelper.AddSampleToPackage(packages[_selectedPackageIndex], _sampleCreationInfo);
                
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
