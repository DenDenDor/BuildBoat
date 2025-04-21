#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportTagsAndLayers : ScriptableObject
{
    //   [MenuItem("Tools/Export Tags and Layers")]
    public static void Export()
    {
        // Получаем путь к папке, где находится этот скрипт
        string scriptPath = GetScriptPath<ExportTagsAndLayers>();
        string exportPath = Path.Combine(scriptPath, "tags_and_layers.json");

        TagsAndLayersData data = new TagsAndLayersData
        {
            tags = UnityEditorInternal.InternalEditorUtility.tags,
            layers = UnityEditorInternal.InternalEditorUtility.layers
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(exportPath, json);
        Debug.Log("Tags and Layers exported to: " + exportPath);
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
