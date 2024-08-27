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
        
        
        private int _selectedPackageIndex;
        private int _previousSelectedPackageIndex = -1;
        
        private List<SampleElement> sampleElements = new List<SampleElement>();
        private Vector2 scrollPosition;
        
        private bool _isUpdatingPackages;

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
            if (_isUpdatingPackages)
            {
                // Display a loading spinner
                EditorGUILayout.LabelField("Loading packages...");

                return;
            }
            
            GUILayout.Label("Create Samples", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Generate List of Packages"))
            {
                await GenerateListPackagesInstalled();
            }

            GUILayout.Space(10);

            // Display the dropdown with the list of packages to choose from
            _selectedPackageIndex = EditorGUILayout.Popup("Select Package", _selectedPackageIndex,
                packages.Select(p => p.name).ToArray());
            
            if(_selectedPackageIndex != _previousSelectedPackageIndex)
            {
                _previousSelectedPackageIndex = _selectedPackageIndex;
                UpdateSamplesList();
            }

            PackageInfo selectedPackage = packages[_selectedPackageIndex];

            if (selectedPackage != null)
            {
                // Draw information about the selected package
                if (packages.Count > 0)
                {
                    GUILayout.Label("Package Name: " + packages[_selectedPackageIndex].name);
                    GUILayout.Label("Version: " + packages[_selectedPackageIndex].version);
                    GUILayout.Label("Description: " + packages[_selectedPackageIndex].description);
                }

                GUILayout.Space(10);

                // Editable list of sample elements
                GUILayout.Label("List of elements to edit samples:", EditorStyles.boldLabel);
                GUILayout.Space(5);
                
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
                    sampleElements[i].Description =
                        EditorGUILayout.TextField("Description", sampleElements[i].Description);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Folder", GUILayout.Width(50));
                    if (GUILayout.Button(string.IsNullOrEmpty(sampleElements[i].Folder)
                            ? "Select Folder"
                            : sampleElements[i].Folder))
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
            }


            // Display a button to create samples for the selected package
            if (GUILayout.Button("Create Samples"))
            {
                // Create samples for the selected package
                foreach (SampleElement sampleElement in sampleElements)
                {
                    Sampledd sample = new Sampledd()
                    {
                        DisplayName = sampleElement.Name,
                        Description = sampleElement.Description,
                        Path = sampleElement.Folder
                    };

                    SampleHelper.AddSampleToPackage(selectedPackage, sample);
                }
            }

            GUILayout.Space(10);
        }

        private void UpdateSamplesList()
        {
            Sample[] samples = SampleHelper.GetSamplesFromPackage(packages[_selectedPackageIndex]);
            
            if (samples != null && sampleElements.Count == 0)
            {
                sampleElements = samples.Select(s => new SampleElement()
                {
                    Name = s.displayName,
                    Description = s.description,
                    Folder = s.resolvedPath
                }).ToList();
            }
            // Clear the list of sample elements if no samples are found
            else
            {
                sampleElements.Clear();
            }
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
                _isUpdatingPackages = true;
                await Task.Delay(100);
            }

            foreach (PackageInfo package in packagesRequest.Result.ToArray())
            {
                packages.Add(package);
            }
            
            _isUpdatingPackages = false;
        }
    }
}