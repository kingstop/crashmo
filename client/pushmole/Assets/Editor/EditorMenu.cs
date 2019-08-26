using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

//		http://www.ituring.com.cn/article/273978

public class EditorMenu
{
	
	[MenuItem ("扩展工具/1 调用内部指令测试")]
	static void ExecuteUnityBuiltInMenu ()
	{
		EditorApplication.ExecuteMenuItem ("Edit/Play");
	}



	[MenuItem ("扩展工具/2 生成角色动画")]
	static void CreateAnimatorController ()
	{
		EditorUtility.DisplayProgressBar ("动画生成器", "动画生成...", 0.5f);

		AnimationEditor.CreateAnimatorController ();

		EditorUtility.ClearProgressBar ();
		EditorUtility.DisplayDialog ("动画生成器", "动画生成成功", "确定");
	}



	[MenuItem ("扩展工具/3 读取数据")]
	static void UpdateData ()
	{
		
		string content = System.IO.File.ReadAllText (string.Format ("{0}/Design/Json/monster.json", Application.dataPath));
		MonsterList config = JsonUtility.FromJson<MonsterList> (content);

		foreach (MonsterInfo monster in config.List)
		{
			DebugFormat.Log ("config:", monster.ID, monster.Name, monster.ATK, monster.HP);
		}
	}



	[MenuItem ("扩展工具/4 Excel 转换成 Json")]
	static void ReadData ()
	{
		var excelFiles = Directory.GetFiles (PathUtility.ExcelPath, "*", SearchOption.AllDirectories)
			.Where (name => name.EndsWith (".xls") || name.EndsWith ("xlsx"));

		foreach (string path in excelFiles)
		{
			ExcelStructure table = SimpleExcel.Read (path);
			string jsonPath = string.Format ("{0}/{1}.json", PathUtility.JsonPath, Path.GetFileNameWithoutExtension (path));
			SimpleFile.Write (jsonPath, DataTableUtility.DataTableToJson (table.mBody, "List"));
		}

		AssetDatabase.Refresh ();
	}



	[MenuItem ("扩展工具/5 读取工程资源")]
	static void ConfigResource ()
	{
		ResourceEditor.VisitAllResources (Application.dataPath);

		ResourceManager.Instance.Init ();
		ResourceManager.Instance.Load<GameObject> ("FlightObject", EResourceType.Prefab);
		ResourceManager.Instance.Release ();

		AssetDatabase.Refresh ();
	}



	[MenuItem ("扩展工具/6 资源打包/1 Build With Name")]
	static void BuildAssetBundleWithName ()
	{
		AssetBundleEditor.BuildWithName ();
		AssetDatabase.Refresh ();
	}



	[MenuItem ("扩展工具/6 资源打包/2 Build All")]
	static void BuildAssetBundleAll ()
	{
		EditorUtility.DisplayProgressBar ("提示", "资源打包中...", 0.5f);
		AssetBundleEditor.BuildAll ();
		AssetDatabase.Refresh ();
		EditorUtility.ClearProgressBar ();
		EditorUtility.DisplayDialog ("打包","打包成功","确定");
	}



	[MenuItem ("扩展工具/ UI冗余图片扫描")]
	static void ScanUnusedTexture ()
	{
		FindUnUnUsedUITexture.Scan ();
	}

}
