using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
    public class NamespaceSynchronizer : EditorWindow
    {
        private string _rootNamespace = "Project";
        private DefaultAsset _selectedFolder;

        [MenuItem("Tools/Sync Namespaces In Folder")]
        public static void ShowWindow()
        {
            GetWindow<NamespaceSynchronizer>("Namespace Sync");
        }

        private void OnGUI()
        {
            GUILayout.Label("��������� namespace", EditorStyles.boldLabel);

            _rootNamespace = EditorGUILayout.TextField("Root Namespace", _rootNamespace);

            _selectedFolder = (DefaultAsset)EditorGUILayout.ObjectField("����� ��� �������������", _selectedFolder, typeof(DefaultAsset), false);

            if (GUILayout.Button("�������� namespace"))
            {
                if (_selectedFolder == null)
                {
                    EditorUtility.DisplayDialog("������", "�������� ����� ��� �������������.", "��");
                    return;
                }

                var folderPath = AssetDatabase.GetAssetPath(_selectedFolder);
                if (!Directory.Exists(folderPath))
                {
                    EditorUtility.DisplayDialog("������", "��������� ����� ����������.", "��");
                    return;
                }

                UpdateNamespaces(folderPath, _rootNamespace);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("������", "Namespace ���������.", "��");
            }
        }

        private void UpdateNamespaces(string folderPath, string rootNamespace)
        {
            foreach (var filePath in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath("Assets", Path.GetDirectoryName(filePath));
                var namespaceParts = relativePath.Split(Path.DirectorySeparatorChar);
                var newNamespace = string.Join(".", new[] { rootNamespace }.Concat(namespaceParts));

                var content = File.ReadAllText(filePath);

                if (Regex.IsMatch(content, @"namespace\s+[^\s{]+"))
                {
                    content = Regex.Replace(content, @"namespace\s+[^\s{]+", $"namespace {newNamespace}");
                }
                else
                {
                    var classMatch = Regex.Match(content, @"(public|internal)\s+(class|struct|interface)\s+");
                    if (classMatch.Success)
                    {
                        content = content.Insert(classMatch.Index, $"namespace {newNamespace}\n{{\n") + "\n}";
                    }
                }

                File.WriteAllText(filePath, content);
            }
        }
    }

}