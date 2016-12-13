using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 

public class UIEditMap : MonoBehaviour {
	public GameObject CreateBtnsObj_;
	public GameObject DrawBtnsObj_;
	public GameObject PointTextPosition_;
	public UISectionGiftGold UISectionGold_;
	public Button PrepareSaveBtn_;
	public Button SaveBtn_;
	public Text TextHeight_;
	public Text TextWidth_;
	 
	public ColorBoard ColorBoard_;
	protected GameObject _source_frade; 
	public List<FradeText> _frade_texts = new List<FradeText>();
	void Awake()
	{
		_source_frade = Resources.Load<GameObject>("prefab/point_out_txt");
		int color_index = 0;
		Dictionary<int, Color> colors = global_instance.Instance.getGroupColors ();
		foreach(KeyValuePair<int, Color> entry_pair in colors)
		{
			ColorBoard_.SetButtonGroupColor(color_index,entry_pair.Key, entry_pair.Value);
			ColorBoard_.SetCount(color_index, 0);
			color_index++;
		}
		ColorBoard_.SetText(10, "目标");
		ColorBoard_.SetText(11, "删除");

	}

	public void ShowCreateBtns(bool b)
	{
		if (b) 
		{
			InitCreateObjs ();
		}

		CreateBtnsObj_.SetActive (b);
	}

	public void ShowEditBtns(bool b)
	{
		DrawBtnsObj_.SetActive (b);
		if (b) 
		{
			if (global_instance.Instance._player.isadmin () == true) 
			{
				PrepareSaveBtn_.gameObject.SetActive (false);
				SaveBtn_.gameObject.SetActive (true);
			}
			else 
			{
				PrepareSaveBtn_.gameObject.SetActive (false);
				SaveBtn_.gameObject.SetActive (true);
			}

		}

	}

	public void setColorButtonText(int group, string text)
	{
		ColorBoard_.SetText (group, text);
	}

	public void InitCreateObjs()
	{
		int min_width = (int)map_def.map_def_min_width;
		int min_height = (int)map_def.map_def_min_height;
		TextWidth_.text = min_width.ToString();
		TextHeight_.text = min_height.ToString();
	}

	public int GetWidth()
	{
		return int.Parse (TextWidth_.text);
	}

	public int GetHeight()
	{
		return int.Parse (TextHeight_.text);
	}

	public void set_point_text(string txt)
	{
		FradeText frab = GameObject.Instantiate(_source_frade).GetComponent<FradeText>();
		frab.gameObject.transform.position = PointTextPosition_.transform.position;
		frab.setText(txt);
		frab.setParent(this);
		frab.gameObject.transform.parent = this.gameObject.transform;
		_frade_texts.Add(frab);
	}

	// Use this for initialization
	void Start () {
	
	}

	public int get_scrollbar_value(GameObject obj, int min, int max)
	{
		Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
		float value = bar_temp.value;
		float temp_distance = (max - min) * value;
		float target_ = temp_distance + min;
		int target = max - (int)target_ + min;        
		return target;
	}
	public void on_scrollbar_width_change(GameObject obj)
	{
		int number_width = get_scrollbar_value(obj, (int)map_def.map_def_min_width, (int)map_def.map_def_max_width);
		TextWidth_.text = number_width.ToString();              
	}

	public void on_scrollbar_height_change(GameObject obj)
	{
		int number_height = get_scrollbar_value(obj, (int)map_def.map_def_min_height, (int)map_def.map_def_max_height);
		TextHeight_.text = number_height.ToString();
	}


	public void DestroyFrade(FradeText entry)
	{
		_frade_texts.Remove(entry);
		DestroyObject(entry.gameObject);
	}

	public void ClearFradeText()
	{
		foreach(FradeText entry in _frade_texts)
		{
			DestroyObject(entry.gameObject);
		}
		_frade_texts.Clear();
	}

	public void OnSaveButtonClick()
	{
		global_instance.Instance._ngui_edit_manager.on_save_btn_click ();
	}

	public void OnPrepareSaveClick()
	{

	}

	public void OnBackClick()
	{
		global_instance.Instance._ngui_edit_manager.BackToMainPanel ();
	}

	public void OnCreateClick()
	{
		global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
