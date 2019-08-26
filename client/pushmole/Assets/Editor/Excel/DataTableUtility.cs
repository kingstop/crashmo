using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Reflection;
using System.Text;

//		http://blog.csdn.net/shuizhaoshui/article/details/51425527

public class DataTableUtility
{



	//		运行时保存到配置表。
	//		InvalidProgramException: Invalid IL code in System.Data.DataTable:get_Columns (): IL_0000: ret
	//		解决：System.Data.dll 版本不对
	public static DataTable ListToDataTable<T> (List<T> list)
	{
		FieldInfo[] fieldInfos = typeof(T).GetFields ();

		Debug.LogWarning (fieldInfos.Length);

		DataTable dataTable = new DataTable ();

		foreach (FieldInfo fi in fieldInfos)
		{
			DebugFormat.LogWarning (fi.Name, fi.FieldType);
			dataTable.Columns.Add (fi.Name, fi.FieldType);
		}

		foreach (T entity in list)
		{
			DataRow row = dataTable.NewRow ();

			foreach (FieldInfo fi in fieldInfos)
			{
				row [fi.Name] = fi.GetValue (entity);
				DebugFormat.LogWarning ("propertys[i]", fi.Name, fi.GetValue (entity));
			}
			dataTable.Rows.Add (row);
		}

		return dataTable;
	}



	public static List<T> DataTableToList<T> (DataTable dt)
	{
		List<T> list = new List<T> ();
		FieldInfo[] fieldInfos = typeof(T).GetFields ();

		foreach (DataRow dr in dt.Rows)
		{
			T entity = System.Activator.CreateInstance<T> ();
			foreach (var fi in fieldInfos)
			{
				object value = System.Convert.ChangeType (dr [fi.Name], fi.FieldType);
				fi.SetValue (entity, value);
			}
			list.Add (entity);
		}

		Debug.LogError ("TODO:内存测试；测试变量声明【T entity;】在foreach内部与在foreach外部的区别");

		return list;
	}


	public static string DataTableToJson (DataTable table, string root = "mConfig")
	{
		StringBuilder json = new StringBuilder ();

		json.AppendLine ("{");

		if (table.Rows.Count > 0)
		{
			json.AppendLine (string.Format ("\t{0}:\t[", Quate (root)));

			for (int i = 0; i < table.Rows.Count; i++)
			{
				json.AppendLine ("\t\t{");

				for (int j = 0; j < table.Columns.Count; j++)
				{
					json.Append (string.Format ("\t\t\t{0}:{1}", Quate (table.Columns [j].ColumnName), Quate (table.Rows [i] [j].ToString ())));		//		末尾","

					if (j < table.Columns.Count - 1)
					{
						json.AppendLine (",");
					}
					else
					{
						json.Append (System.Environment.NewLine);
					}
				}

				json.Append ("\t\t}");

				if (i != table.Rows.Count - 1)
				{
					json.AppendLine (",");
				}
				else
				{
					json.Append (System.Environment.NewLine);
				}
			}
			json.AppendLine ("\t]");
		}

		json.AppendLine ("}");

		return json.ToString ();  
	}

	public const string mQuote = "\"" ;

	//		字符串用引号括起来
	public static string Quate (string str)
	{
		return string.Format ("{0}{1}{2}", mQuote, str, mQuote);
	}



	public static void Log (DataTable table)
	{
		for (int row = 0; row < table.Rows.Count; row++)
		{
			for (int column = 0; column < table.Columns.Count; column++)
			{
				DebugFormat.Log (row, column, table.Rows [row] [column]);
			}
		}
	}

}
