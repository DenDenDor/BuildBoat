using UnityEngine;
using UnityEditor;
using System.IO;

public class PhysicsLayerTransferTool : EditorWindow
{
    // Путь к файлу PhysicsManager.asset
    private static string physicsManagerPath = "ProjectSettings/PhysicsManager.asset";

    // Путь к папке, где хранятся скрипты
    private static string scriptFolderPath;

    [MenuItem("Tools/Export Physics Layers")]
    public static void ExportPhysicsLayers()
    {
        // Получаем путь к папке, где находится этот скрипт
        scriptFolderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<PhysicsLayerTransferTool>())));

        // Формируем путь для экспорта
        string exportFilePath = Path.Combine(scriptFolderPath, "Exported_PhysicsManager.asset");

        // Копируем файл PhysicsManager.asset в папку со скриптами
        File.Copy(physicsManagerPath, exportFilePath, true);

        Debug.Log($"Physics layers exported to: {exportFilePath}");
    }

    [MenuItem("Tools/Import Physics Layers")]
    public static void ImportPhysicsLayers()
    {
        // Получаем путь к папке, где находится этот скрипт
        scriptFolderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<PhysicsLayerTransferTool>())));

        // Формируем путь для импорта
        string importFilePath = Path.Combine(scriptFolderPath, "Exported_PhysicsManager.asset");

        // Проверяем, существует ли файл для импорта
        if (File.Exists(importFilePath))
        {
            // Копируем файл обратно в ProjectSettings
            File.Copy(importFilePath, physicsManagerPath, true);

            Debug.Log($"Physics layers imported from: {importFilePath}");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError($"File not found: {importFilePath}");
        }
    }
}