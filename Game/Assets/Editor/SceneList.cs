using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;


enum ExeOrder
{
    order_first,order_second,
}

///<summary>
///BuildSceneに追加されたSceneをビルドセッティングへ追加し、そのリストを作成するクラスです。
///</summary>
public class BuildSettingSceneFile : AssetPostprocessor
{
    //ビルドするSceneを格納するフォルダのパス
    private const string BUILD_DIRECTORY_PATH = "Assets/Scenes/BuildScenes";
    //ビルドするSceneを格納するフォルダの最初のシーンを格納するフォルダへのパス
    private const string BUILD_DIRECTORY_FIRST_PATH = "Assets/Scenes/BuildScenes/First";


    ///<summary>
    ///変更されたファイルが指定しているディレクトリ内のファイルであるかどうかを判定します
    //////</summary>
    static bool ExistsDrectryInAssets(List<string[]> _assetsList,List<string> _targetDirectoryNameList)
    {
        return _assetsList
              .Any(assets => assets
              .Select(asset => System.IO.Path.GetDirectoryName(asset))
              .Intersect(_targetDirectoryNameList)
              .Count() > 0);
    }

    ///<summary>
    ///ファイルの情報変更がある場合に実行され、情報変更があったファイルがBUILD_DIRECTORY_PATH・BUILD_DIRECTORY_FIRST_PATHに格納されている場合に
    ///BuildSettingのシーン追加情報・シーンリストを更新します。
    ///</summary>
    private static void OnPostprocessAllAssets(string[] _importedAssets, string[] _deletedAssets, string[] _movedAssets,string[] _movedFromAssetPaths)
    {
        //変更のあったアセットの名前リスト
        List<string[]> assetsList = new List<string[]>() {
            _importedAssets,_deletedAssets,_movedAssets,_movedFromAssetPaths
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



    ///<summary>
    ///BUILD_DIRECTORY_PATH・BUILD_DIRECTORY_FIRST_PATH内の情報を参照し、ビルド情報のシーンリストを更新、シーンリストであるSceneNameListを作成します。
    ///この関数はエディタ画面のToolsバーから手動実行することができます。
    ///※この関数はUnityEditorが開かれたときに自動実行されます。
    ///</summary>
    [MenuItem("Tools/Update/Scenes In Build"),DidReloadScripts((int)ExeOrder.order_first)]
    private static void UpdateScenesInBuild()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }
       
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

        if (!string.IsNullOrEmpty(firstScenePath))
        {
            Debug.Log("PushBuildSceneTo : " + firstScenePath);
            sceneList.Add(new EditorBuildSettingsScene(firstScenePath, true));
        }
        foreach(string path in pathList)
        {
            Debug.Log("PushBuildSceneTo : " + path);
            sceneList.Add(new EditorBuildSettingsScene(path, true));
        }
        EditorBuildSettings.scenes = sceneList.ToArray();
    }
}

///<summary>
///ビルドセッティング内のシーンリスト情報から、現在設定されているシーンを参照し
///列挙型リストを作成するクラスです。
///</summary>
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

    //エディタ画面での実行名
    private const string ITEM_NAME = "Tools/Create/SceneNameList";
    //作成するリストのファイルパス&名前
    private const string PATH = "Assets/SceneNameList.cs";

    private static readonly string FILENAME = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
                                                                                        //private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)


     ///<summary>
     ///現在のビルドセッティングのシーンリストの情報から
     /// 新たにシーンのリスト情報を作成します。
     /// ※この関数はUnityEditorが開かれたときに自動実行されます。 
     ///</summary>
    [MenuItem(ITEM_NAME),DidReloadScripts((int)ExeOrder.order_second)]
     public static void CreateList()
    {
        //スクリプトを作成できる状況にないorゲーム起動時は処理を終了する
        if (!CanCreate() || EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }
        CreateScript();
        Debug.Log(FILENAME + " Edit Success");
       // EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");

    }

    ///<summary>
    ///ビルドセッティングのシーンリストを参照してシーンをリスト化したスクリプトを作成します。
    ///</summary>
    public static void CreateScript()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// シーン名を定数で管理する");
        builder.AppendLine("/// </summary>");
        builder.Append("\t").AppendLine(@"public enum SceneNameList {");
        builder.Append("\t").AppendLine(@"None,").AppendLine();
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

    ///<summary>
    ///現在のエディタ情報をもとに現在CreateScriptが実行可能であるかを判定します。
    ///</summary>
    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    ///<summary>
    ///引数strの中から無効な文字を削除して返却します。
    ///</summary>
    public static string RemoveInvalidChars(string _str)
    {
        Array.ForEach(INVALUD_CHARS, c => _str = _str.Replace(c, string.Empty));
        return _str;
    }
}
