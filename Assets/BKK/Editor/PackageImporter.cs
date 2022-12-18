using System.Linq;
using UnityEngine;
using UnityEditor;
using Debug = BKK.Debugging.Debug;


public class PackageImporter : AssetPostprocessor
{
    private static SerializedObject tagManager;
    private static SerializedProperty sortingLayers;
    private static SerializedProperty layers;
    private static SerializedProperty tags;
    
    private const int maxTags = 10000;
    private const int maxLayers = 31;

    private const string packageName = "PackageNameExample";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        var inPackages = importedAssets.Any(path => path.Contains(packageName));
        
        var outPackages = deletedAssets.Any(path => path.Contains(packageName));
        
        if (inPackages)
        {
            
        }

        if (outPackages)
        {
            
        }
    }

    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        if (EditorPrefs.GetBool($"PackageImporter_{PlayerSettings.productName}", false)) return;
        
        Setup();
    }

    [MenuItem("BKK/프로젝트 세팅 초기화")]
    public static void SetupMenu()
    {
        var question = EditorUtility.DisplayDialog("경고!!!!", "프로젝트 세팅이 기본 세팅으로 초기화 됩니다. 초기화 하시겠습니까?", "예", "아니오");

        if (question)
        {
            Setup();
        }
    }

    public static void Setup()
    {
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.gcIncremental = true;
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.companyName = "BKKProduction";
        
        //sortingLayers ??= tagManager.FindProperty("m_SortingLayers");
        
        CreateLayer(3, "Post Processing");
        CreateLayer(6, "Player");
        
        CreateTag("Enemy");

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetArchitecture(BuildTargetGroup.Standalone, (int) Architecture.ARM64);
#elif UNITY_ANDROID
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetArchitecture(BuildTargetGroup.Android, (int) Architecture.ARM64);
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
#elif UNITY_IOS
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int) Architecture.ARM64);
#endif
        
        EditorPrefs.SetBool($"PackageImporter_{PlayerSettings.productName}", true);
    }

    public static bool CreateTag(int index, string name)
    {
        tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        tags = tagManager.FindProperty("tags");
        if (TagExist(name))
        {
            Debug.Log($"태그 {name}가 이미 존재합니다.");
            return false;
        }

        if (index == maxTags)
        {
            Debug.Log("더이상 태그를 추가할 수 없습니다.");
            return false;
        }

        tags.InsertArrayElementAtIndex(index);
        tags.GetArrayElementAtIndex(index).stringValue = name;
        tagManager.ApplyModifiedProperties();
        
        return true;
    }
    
    public static bool CreateTag(string name)
    {
        tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        tags = tagManager.FindProperty("tags");
        
        return CreateTag(tags.arraySize, name);
    }
    
    private static bool TagExist(string name)
    {
        tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        tags = tagManager.FindProperty("tags");
        
        for (var i = 0; i < tags.arraySize; i++)
        {
            if(tags.GetArrayElementAtIndex(i).stringValue == name) return true;
        }

        return false;
    }

    public static bool CreateLayer(int index, string name)
    {
        tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        layers = tagManager.FindProperty("layers");

        var layer = layers.GetArrayElementAtIndex(index);

        if (layer.stringValue == string.Empty)
        {
            layers.GetArrayElementAtIndex(index).stringValue = name;
            tagManager.ApplyModifiedProperties();
            return true;
        }

        if (index == maxLayers)
        {
            Debug.Log("레이어를 더이상 추가할 수 없습니다.");
        }

        return false;
    }

    public static bool CreateLayer(string layerName)
    {
        if (!LayerExists(layerName))
        {
            // Start at layer 9th index -> 8 (zero based) => first 8 reserved for unity / greyed out
            for (var i = 8; i < maxLayers; i++)
            {
                CreateLayer(i, layerName);
            }
        }
        else
        {
            Debug.Log($"레이어 {layerName}가 이미 존재합니다.");
        }
        return false;
    }
    
    public static bool LayerExists(string layerName)
    {
        tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        layers = tagManager.FindProperty("layers");
        return PropertyExists(layers, 0, maxLayers, layerName);
    }
    
    private static bool PropertyExists(SerializedProperty property, int start, int end, string value)
    {
        for (int i = start; i < end; i++)
        {
            SerializedProperty t = property.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
}

public enum Architecture
{
    None = 0,
    ARM64 = 1,
    Universal = 2
}
