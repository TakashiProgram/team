using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;


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

    private const string ITEM_NAME = "Tools/Create/SceneName";
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

        EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");

    }


    public static void CreateScript()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// シーン名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        //builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
      //  builder.AppendLine("{");
        builder.Append("\t").AppendLine(@"enum SceneListTest {");
        foreach (var n in EditorBuildSettings.scenes
            .Select(c => Path.GetFileNameWithoutExtension(c.path))
            .Distinct()
            .Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            
            builder.Append("\t").AppendFormat(@"{0},", n.var, n.val).AppendLine();
        }
        //builder.AppendLine("\t };");
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
