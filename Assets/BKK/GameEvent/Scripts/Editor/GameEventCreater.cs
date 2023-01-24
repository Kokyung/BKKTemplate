using UnityEditor;
using UnityEngine;
using System.IO;

namespace BKK.GameEventArchitecture.Editor
{
    public class GameEventCreater
    {
        [MenuItem("BKK/게임 이벤트/게임 이벤트 생성기", false, 50)]
        private static void OpenCreateWindow()
        {
            GameEventCreaterWindow window = EditorWindow.GetWindow<GameEventCreaterWindow>(false, "게임 이벤트 생성기", true);
            window.titleContent = new GUIContent("게임 이벤트 생성기");
        }

        public static void CreateAll(string type, string path)
        {
            if (!path.EndsWith("/")) path += "/";
            
            CreateGameEvent(type, path + "GameEvent");
            CreateGameEventListener(type, path + "GameEventListener");

            AssetDatabase.Refresh();
        }

        private static void CreateGameEvent(string type, string folderPath,
            string menuName = "BKK/Game Event Architecture", bool refresh = false)
        {
            var filePath = $"{folderPath}/{type}GameEvent.cs";
            var existScripts = AssetDatabase.FindAssets($"{type}GameEvent t:script");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (existScripts.Length == 0)
            {
                using var outfile = new StreamWriter(filePath);
                outfile.WriteLine("// 게임 이벤트 생성 메뉴에 의해 생성되었습니다.");
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace BKK.GameEventArchitecture");
                outfile.WriteLine("{");
                outfile.WriteLine(
                    $"    [CreateAssetMenu(menuName = \"{menuName}/{type} Game Event\", fileName = \"New {type} Game Event\", order = 100)]");
                outfile.WriteLine($"    public class {type}GameEvent : GameEvent<{type}>");
                outfile.WriteLine("    {");
                outfile.WriteLine("    }");
                outfile.WriteLine("}");
            }
            else
            {
                var paths = "\n";

                foreach (var script in existScripts)
                {
                    var path = AssetDatabase.GUIDToAssetPath(script);
                    if (path.Contains("Listener")) continue;
                    paths += $"{path}\n";
                }

                Debug.Log($"{type} 게임 이벤트 리스너 파일이 이미 존재합니다.: {paths}");
                return;
            }

            if (refresh) AssetDatabase.Refresh();

            Debug.Log($"{type} 게임 이벤트 클래스 생성: {folderPath}");
        }

        private static void CreateGameEventListener(string type, string folderPath, bool refresh = false)
        {
            var filePath = $"{folderPath}/{type}GameEventListener.cs";
            var existScripts = AssetDatabase.FindAssets($"{type}GameEventListener t:script");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (existScripts.Length == 0)
            {
                using var outfile = new StreamWriter(filePath);
                outfile.WriteLine("// 게임 이벤트 생성 메뉴에 의해 생성되었습니다.");
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using UnityEngine.Events;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace BKK.GameEventArchitecture");
                outfile.WriteLine("{");
                outfile.WriteLine(
                    $"    public class {type}GameEventListener : GameEventListener<{type}, {type}GameEvent, UnityEvent<{type}>>");
                outfile.WriteLine("    {");
                outfile.WriteLine("");
                outfile.WriteLine("    }");
                outfile.WriteLine("}");
            }
            else
            {
                var paths = "\n";

                foreach (var script in existScripts)
                {
                    paths += $"{AssetDatabase.GUIDToAssetPath(script)}\n";
                }

                Debug.Log($"{type} 게임 이벤트 파일이 이미 존재합니다.: {paths}");
                return;
            }

            if (refresh) AssetDatabase.Refresh();

            Debug.Log($"{type} 게임 이벤트 리스너 클래스 생성: {folderPath}");
        }
    }
}
