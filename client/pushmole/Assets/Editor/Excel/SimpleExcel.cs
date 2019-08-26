using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Reflection;


//		http://www.mamicode.com/info-detail-1507524.html
//		http://blog.csdn.net/ma_jiang/article/details/53557889
//		https://www.cnblogs.com/sylone/p/6094707.html


public class SimpleExcel : MonoBehaviour
{

	public static ExcelStructure Read (string filePath)
	{
		ExcelStructure excelStructure = new ExcelStructure ();
		DataTable header = new  DataTable ();
		DataTable body = new DataTable ();

		using (ExcelPackage package = new ExcelPackage (new FileStream (filePath, FileMode.Open)))
		{
			for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
			{
				ExcelWorksheet sheet = package.Workbook.Worksheets [i];

				for (int r = sheet.Dimension.Start.Row; r <= sheet.Dimension.End.Row; r++)
				{
					DataRow row = null;
					if (r <= excelStructure.GetHeaderRowCount ())
					{
						row = header.NewRow ();
					}
					else
					{
						row = body.NewRow ();
					}

					for (int c = sheet.Dimension.Start.Column; c <= sheet.Dimension.End.Column; c++)
					{
						object o = sheet.GetValue (r, c);

						if (r - 1 == (int)EExcelHeaderInfo.FieldName)
						{
							header.Columns.Add (o.ToString ()/*, typeof(string)*/);
							body.Columns.Add (header.Columns [c - 1].ColumnName);
						}

						row [header.Columns [c - 1].ColumnName] = o;
					}

					if (r <= excelStructure.GetHeaderRowCount ())
					{
						header.Rows.Add (row);
					}
					else
					{
						body.Rows.Add (row);
					}
				}
			}
		}

		excelStructure.mHeader = header;
		excelStructure.mBody = body;
		excelStructure.Log ();

		return excelStructure;
	}

	public static void Write (string filePath, ExcelStructure excelStructure)
	{
		SimpleFile.CheckDirectory (filePath);

		string fileName = Path.GetFileNameWithoutExtension (filePath);
		string sheetName = fileName;

		FileStream fs = new FileStream (filePath, FileMode.Create);

		using (ExcelPackage package = new ExcelPackage (fs))
		{
			ExcelWorksheet sheet = package.Workbook.Worksheets.Add (sheetName);

			int row = 1;
			int column = 1;

			if (excelStructure != null)
			{
				if (excelStructure.mHeader != null)
				{
					foreach (DataRow dr in excelStructure.mHeader.Rows)
					{
						column = 1;

						foreach (DataColumn dc in excelStructure.mHeader.Columns)
						{
							sheet.SetValue (row, column, excelStructure.mHeader.Rows [row - 1] [column - 1]);
							column++;
						}

						row++;
					}
				}

				if (excelStructure.mBody != null)
				{
					foreach (DataRow dr in excelStructure.mBody.Rows)
					{
						column = 1;

						foreach (DataColumn dc in excelStructure.mBody.Columns)
						{
							sheet.SetValue (row, column, excelStructure.mBody.Rows [row - 1 - excelStructure.GetHeaderRowCount ()] [column - 1]);
							column++;
						}

						row++;
					}
				}
			}

			package.Save ();
		}

		fs.Close ();
	}



	public static void Write (string filePath, DataTable dataTable)
	{
		SimpleFile.CheckDirectory (filePath);

		string fileName = Path.GetFileNameWithoutExtension (filePath);
		string sheetName = fileName;

		FileStream fs = new FileStream (filePath, FileMode.Create);

		using (ExcelPackage package = new ExcelPackage (fs))
		{
			ExcelWorksheet sheet = package.Workbook.Worksheets.Add (sheetName);

			for (int row = 1; row <= dataTable.Rows.Count; row++)
			{
				for (int column = 1; column <= dataTable.Columns.Count; column++)
				{
					DebugFormat.LogWarning ("111", row, column, dataTable.Rows [row - 1] [column - 1]);
					sheet.SetValue (row, column, dataTable.Rows [row - 1] [column - 1]);
				}
			}

			package.Save ();
		}

		fs.Close ();
	}




}
