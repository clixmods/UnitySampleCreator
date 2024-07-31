using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace SampleCreator.Editor
{
    public class SampleCreatorWindow : EditorWindow
    {
        public List<PackageInfo> packages = new();
        
        private int _selectedPackage;
        
        private List<SampleElement> sampleElements = new List<SampleElement>();
        
        
        private class SampleElement
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Folder { get; set; }
        }
        
        [MenuItem("MENUITEM/MENUITEMCOMMAND")]
        private static void ShowWindow()
        {
            SampleCreatorWindow window = GetWindow<SampleCreatorWindow>();
            window.titleContent = new GUIContent("Sample Creator");
            window.Show();
        }

        private async void OnGUI()
        {
            GUILayout.Label("Create Samples", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            if (GUILayout.Button("Generate List of Packages"))
            {
                await GenerateListInstalledPackages();
            }
            
            GUILayout.Space(10);
            
            // Display the a dropdown with the list of packages to choose from
            _selectedPackage = EditorGUILayout.Popup("Select Package", _selectedPackage, packages.Select(p => p.name).ToArray());
            
            // Draw information about the selected package
            GUILayout.Label("Package Name: " + packages[_selectedPackage]);
            GUILayout.Label("Version: " + packages[_selectedPackage].version);
            GUILayout.Label("Description: " + packages[_selectedPackage].description);
            
            GUILayout.Space(10);
            
            // Editable list of sample elements
            GUILayout.Label("List of elements to edit samples:", EditorStyles.boldLabel);

            if (GUILayout.Button("Add New Sample Element"))
            {
                sampleElements.Add(new SampleElement());
            }

            for (int i = 0; i < sampleElements.Count; i++)
            {
                GUILayout.BeginHorizontal();
                sampleElements[i].Name = EditorGUILayout.TextField("Name", sampleElements[i].Name);
                sampleElements[i].Description = EditorGUILayout.TextField("Description", sampleElements[i].Description);
                
                GUILayout.Label("Folder");
                if (GUILayout.Button(string.IsNullOrEmpty(sampleElements[i].Folder) ? "Select Folder" : sampleElements[i].Folder))
                {
                    string selectedFolder = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    if (!string.IsNullOrEmpty(selectedFolder))
                    {
                        sampleElements[i].Folder = selectedFolder;
                    }
                }

                if (GUILayout.Button("Remove"))
                {
                    sampleElements.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }
            
            
            // Display a button to create samples for the selected package  
            if (GUILayout.Button("Create Samples"))
            {
                // Create samples for the selected package
            }
            
            GUILayout.Space(10);
            
        }

        private async Task GenerateListInstalledPackages()
        {
            // Get packages from the package manager
            ListRequest packages = UnityEditor.PackageManager.Client.List(true);
            
            // Clear the list of package names
            this.packages.Clear();
            
            // Wait for the packages to be fetched
            while (!packages.IsCompleted)
            {
               
            }
            
            foreach (PackageInfo package in packages.Result.ToArray())
            {
                this.packages.Add(package);
            }
            
        }
    }
}