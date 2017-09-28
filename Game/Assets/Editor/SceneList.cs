using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class BuildSettingSceneFile : AssetPostprocessor
{
    private const string BUILD_DIRECTORY_PATH = "Assets/Scenes/BuildScenes";
    private const string BUILD_DIRECTORY_FIRST_PATH = "Assets/Scenes/BuildScenes/First";
    static bool ExistsDrectryInAssets(List<string[]> assetsList,List<string> targetDirectoryNameList)
    {
        return assetsList
              .Any(assets => assets
              .Select(asset => System.IO.Path.GetDirectoryName(asset))
              .Intersect(targetDirectoryNameList)
              .Count() > 0);
    }


    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,string[] movedFromAssetPaths)
    {
        List<string[]> assetsList = new List<string[]>() {
            importedAssets,deletedAssets,movedAssets,movedFromAssetPaths
        };
        List<string> targetDirectoryNameList = new List<string>()
        {
          BUILD_DIRECTORY_PATH,BUILD_DIRECTORY_FIRST_PATH,
        };

        //変更ファイルが指定したディレクトリ内ならビルド情報を更新する
        if (ExistsDrectryInAssets(assetsList, targetDirectoryNameList))
        {
            UpdateScenesInBuild();
            SceneList.CreateList();
        }
    }

    //対象のファイルに格納されているものがあるかを検索します。
    //true : 空の場合 false : 中身が存在する
    private static bool EmptyChack(string path,string directory)
    {
        string[] directories = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);
        if(directories.Length + files.Length == 0)
        {
            return true;
        }
        return false;
    }

    [MenuItem("Tools/Update/Scenes In Build")]
    private static void UpdateScenesInBuild()
    {

        //sceneファイルの全取得
        List<string> pathList = new List<string>();
        string firstScenePath = "";

        foreach(var guid in AssetDatabase.FindAssets("t:Scene"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            //指定ディレクトリ以外のシーンはスルー処理
            if (!path.Contains(BUILD_DIRECTORY_PATH))
            {
                continue;
            }

            //シーン名が同名ならエラー表示
            if (pathList.Contains(sceneName))
            {
                Debug.LogError(sceneName + "というシーン名が重複しています。");

            }
            //親ディレクトリがFirstなら最初のシーンに設定
            else if(System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path)) == "First")
            {
                Debug.Log(System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path)));
                //二つ以上入っているならエラー
                if (!string.IsNullOrEmpty(firstScenePath))
                {
                    Debug.LogError("Firstディレクトリに複数シーンが設定されています。Firstディレクトリには一つのシーンしか設定できません。");
                }
                firstScenePath = path;
            }
            //パスをリストに追加する
            else
            {
                pathList.Add(path);
            }
        }

        //追加するシーンのリストの作成追加
        List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>();
        Debug.Log(string.IsNullOrEmpty(firstScenePath));
        if (!string.IsNullOrEmpty(firstScenePath))
        {
            Debug.Log("PushBuildSceneTo" + firstScenePath);
            sceneList.Add(new EditorBuildSettingsScene(firstScenePath, true));
        }
        foreach(string path in pathList)
        {
            Debug.Log("AddScene");
            sceneList.Add(new EditorBuildSettingsScene(path, true));
        }
        EditorBuildSettings.scenes = sceneList.ToArray();
    }
}

public static class SceneList {
 
    //無効な文字列を管理する固定配列
    private static readonly string[] INVALUD_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    private const string ITEM_NAME = "Tools/Create/SceneNameList";
    private const string PATH = "Assets/SceneName.cs";

    private static readonly string FILENAME = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)


    /*
     *管理クラスの作成 
     * 
     */
     [MenuItem(ITEM_NAME)]
     public static void CreateList()
    {
        if (!CanCreate())
        {
            return;
        }
        CreateScript();

       // EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");

    }


    public static void CreateScript()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// シーン名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        builder.Append("\t").AppendLine(@"public enum SceneNameList {");

        int num = 0;
        foreach (var n in EditorBuildSettings.scenes
            .Select(c => Path.GetFileNameWithoutExtension(c.path))
            .Distinct()
            .Select(c => new { var = RemoveInvalidChars(c), val = c}))
        {
            if (EditorBuildSettings.scenes[num++].enabled)
            {
                builder.Append("\t").AppendFormat(@"{0},", n.var, n.val).AppendLine();
            }
        }
        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

    }


    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }
}
