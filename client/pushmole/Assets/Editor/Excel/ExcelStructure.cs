using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;



public class ExcelStructure
{
	public string mExcelName;

	public string mSheetName;

	public DataTable mHeader;

	public DataTable mBody;


	public ExcelStructure ()
	{
	}

	public ExcelStructure (DataTable header, DataTable body)
	{
		this.mHeader = header;
		this.mBody = body;
	}

	public int GetHeaderRowCount()
	{
		return (int)EExcelHeaderInfo.Max;
	}

	public void Log ()
	{
		Debug.Log ("Header");
		DataTableUtility.Log (this.mHeader);

		Debug.Log ("Body");
		DataTableUtility.Log (this.mBody);
	}
}



//		前3行作为Excel头部
public enum EExcelHeaderInfo
{
	FieldName = 0,
	FieldType,
	FieldDescription,
	Max,
}
