using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace SampleCreator.Editor
{
    public class SampleCreatorWindow : EditorWindow
    {
        public List<PackageInfo> packages = new List<PackageInfo>();
        private int _selectedPackage;
        private List<SampleElement> sampleElements = new List<SampleElement>();
        private Vector2 scrollPosition;

        private class SampleElement
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Folder { get; set; }
        }

        [MenuItem("Sample Creator/Open Window")]
        private static void ShowWindow()
        {
            SampleCreatorWindow window = GetWindow<SampleCreatorWindow>();
            window.titleContent = new GUIContent("Sample Creator");
            window.Show();
        }

        private async void CreateGUI()
        {
            await GenerateListPackagesInstalled();
        }

        private async void OnGUI()
        {
            GUILayout.Label("Create Samples", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Generate List of Packages"))
            {
                await GenerateListPackagesInstalled();
            }

            GUILayout.Space(10);

            // Display the dropdown with the list of packages to choose from
            _selectedPackage = EditorGUILayout.Popup("Select Package", _selectedPackage, packages.Select(p => p.name).ToArray());

            // Draw information about the selected package
            if (packages.Count > 0)
            {
                GUILayout.Label("Package Name: " + packages[_selectedPackage].name);
                GUILayout.Label("Version: " + packages[_selectedPackage].version);
                GUILayout.Label("Description: " + packages[_selectedPackage].description);
            }

            GUILayout.Space(10);

            // Editable list of sample elements
            GUILayout.Label("List of elements to edit samples:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            // Initialize sample elements from samples in selected package
            Sample[] samples = SampleHelper.GetSamplesFromPackage(packages[_selectedPackage]);
            if (samples != null && sampleElements.Count == 0)
            {
                sampleElements = samples.Select(s => new SampleElement()
                {
                    Name = s.displayName,
                    Description = s.description,
                    Folder = s.resolvedPath
                }).ToList();
            }

            if (GUILayout.Button("Add New Sample Element"))
            {
                sampleElements.Add(new SampleElement());
            }

            GUILayout.Space(10);

            // Add scroll view
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < sampleElements.Count; i++)
            {
                GUILayout.BeginVertical("box");
                sampleElements[i].Name = EditorGUILayout.TextField("Name", sampleElements[i].Name);
                sampleElements[i].Description = EditorGUILayout.TextField("Description", sampleElements[i].Description);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Folder", GUILayout.Width(50));
                if (GUILayout.Button(string.IsNullOrEmpty(sampleElements[i].Folder) ? "Select Folder" : sampleElements[i].Folder))
                {
                    string selectedFolder = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    if (!string.IsNullOrEmpty(selectedFolder))
                    {
                        sampleElements[i].Folder = selectedFolder;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                if (GUILayout.Button("Remove"))
                {
                    sampleElements.RemoveAt(i);
                }
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();

            // Display a button to create samples for the selected package
            if (GUILayout.Button("Create Samples"))
            {
                // Create samples for the selected package
            }

            GUILayout.Space(10);
        }

        private async Task GenerateListPackagesInstalled()
        {
            // Get packages from the package manager
            ListRequest packagesRequest = UnityEditor.PackageManager.Client.List(true);

            // Clear the list of package names
            packages.Clear();

            // Wait for the packages to be fetched
            while (!packagesRequest.IsCompleted)
            {
                await Task.Delay(100);
            }

            foreach (PackageInfo package in packagesRequest.Result.ToArray())
            {
                packages.Add(package);
            }
        }
    }
}
