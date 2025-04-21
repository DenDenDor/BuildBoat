#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportTagsAndLayers : ScriptableObject
{
    [MenuItem("Tools/Import Tags and Layers")]
    public static void Import()
    {
        // Получаем путь к папке, где находится этот скрипт
        string scriptPath = GetScriptPath<ImportTagsAndLayers>();
        string importPath = Path.Combine(scriptPath, "tags_and_layers.json");

        if (File.Exists(importPath))
        {
            string json = File.ReadAllText(importPath);
            TagsAndLayersData data = JsonUtility.FromJson<TagsAndLayersData>(json);

            // Импорт тегов
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Очистка существующих тегов (опционально)
            tagsProp.ClearArray();
            tagManager.ApplyModifiedProperties();

            // Добавление новых тегов
            foreach (var tag in data.tags)
            {
                bool tagExists = false;
                for (int i = 0; i < tagsProp.arraySize; i++)
                {
                    SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                    if (t.stringValue == tag)
                    {
                        tagExists = true;
                        break;
                    }
                }

                if (!tagExists)
                {
                    tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                    SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
                    newTag.stringValue = tag;
                }
            }

            // Импорт слоев
            SerializedProperty layersProp = tagManager.FindProperty("layers");

            // Очистка существующих слоев (опционально)
            for (int i = 0; i < layersProp.arraySize; i++)
            {
                if (i >= 8) // Не трогаем встроенные слои (первые 8 слоев зарезервированы Unity)
                {
                    SerializedProperty layer = layersProp.GetArrayElementAtIndex(i);
                    layer.stringValue = "";
                }
            }

            // Добавление новых слоев
            foreach (var layer in data.layers)
            {
                bool layerExists = false;
                for (int i = 0; i < layersProp.arraySize; i++)
                {
                    SerializedProperty l = layersProp.GetArrayElementAtIndex(i);
                    if (l.stringValue == layer)
                    {
                        layerExists = true;
                        break;
                    }
                }

                if (!layerExists)
                {
                    for (int i = 8; i < layersProp.arraySize; i++) // Начинаем с 8, чтобы не перезаписать встроенные слои
                    {
                        SerializedProperty l = layersProp.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(l.stringValue))
                        {
                            l.stringValue = layer;
                            break;
                        }
                    }
                }
            }

            tagManager.ApplyModifiedProperties();
            Debug.Log("Tags and Layers imported from: " + importPath);
        }
        else
        {
            Debug.LogError("File not found: " + importPath);
        }
    }

    private static string GetScriptPath<T>() where T : ScriptableObject
    {
        // Создаем временный экземпляр ScriptableObject для получения пути
        T instance = ScriptableObject.CreateInstance<T>();
        MonoScript script = MonoScript.FromScriptableObject(instance);
        string scriptPath = AssetDatabase.GetAssetPath(script);
        ScriptableObject.DestroyImmediate(instance); // Удаляем временный объект
        return Path.GetDirectoryName(scriptPath);
    }

    [System.Serializable]
    public class TagsAndLayersData
    {
        public string[] tags;
        public string[] layers;
    }
}
#endif
