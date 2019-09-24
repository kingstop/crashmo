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
    public Image DrawImage_;
    public RawImage DrawRawImage_; 
	 
	public ColorBoard ColorBoard_;
	protected GameObject _source_frade; 
	public List<FradeText> _frade_texts = new List<FradeText>();
    private MapData _mapinfo;
    private List<int> _grid_size_config = new List<int>();
    bool _mouse_down;


    public Vector2 CurrMousePosition(Transform thisTrans)
    {
        Vector2 vecMouse;
        RectTransform parentRectTrans = thisTrans.GetComponent<RectTransform>();
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, Input.mousePosition, Camera.current, out vecMouse))
        {
            int i = 0;
            i++;
        }
        
        return vecMouse;
    }

    void Update()
    {
        if(Input.anyKey)
        {

            if(DrawRawImage_.texture != null)
            {
                Vector2 vec =CurrMousePosition(DrawRawImage_.transform);
                float diff_x = DrawRawImage_.GetComponent<RectTransform>().rect.width / 2;
                float diff_y = DrawRawImage_.GetComponent<RectTransform>().rect.height / 2;
                if (Mathf.Abs(vec.x) <= diff_x && Mathf.Abs(vec.y) <= diff_y)
                {
                    int x_image = (int)(vec.x + diff_x) / _mapinfo.grid_size_;
                    int y_image = (int)(vec.y + diff_y) / _mapinfo.grid_size_;
                    _mapinfo.DrawGridTexture((Texture2D)DrawRawImage_.texture, x_image, y_image, global_instance.Instance._current_group);
                }
                //float x = Input.mousePosition.x;
                //float y = Input.mousePosition.y;
                ////if(x > DrawRawImage_.transform.position.x)
                //Debug.Log(Input.mousePosition);
                ////Debug.Log(DrawRawImage_.transform.position);

                //float texture_begin_x = DrawRawImage_.transform.position.x;// - (DrawRawImage_.GetComponent<RectTransform>().rect.width / 2);
                //float texture_begin_y = DrawRawImage_.transform.position.y;// - (DrawRawImage_.GetComponent<RectTransform>().rect.height / 2);
                //Debug.Log("texture[" + texture_begin_x + "," + texture_begin_y + "]");

            }

        }
    }

    void OnMouseDown()
    {
        Debug.Log(Input.mousePosition);
        _mouse_down = true;

    }

    void OnMouseUp()
    {
        _mouse_down = false;
    }

    void Awake()
	{
		_source_frade = Resources.Load<GameObject>("prefab/point_out_txt");
        ResetDrawBtns();
        _grid_size_config.Add(50);
        _grid_size_config.Add(45);
        _grid_size_config.Add(40);
        _grid_size_config.Add(35);
        _grid_size_config.Add(30);
        _grid_size_config.Add(25);
        _grid_size_config.Add(20);
        _grid_size_config.Add(10);
    }

    public void setMapInfo(MapData mapinfo)
    {
        _mapinfo = mapinfo;
        int grid_size = 0;
        foreach(int grid_config_size in _grid_size_config)
        {
            if(grid_config_size * _mapinfo.width_ > 660)
            {
                continue;
            }

            if(grid_config_size * _mapinfo.height_ > 660)
            {
                continue;
            }
            grid_size = grid_config_size;
            break;
        }
        if(grid_size != 0)
        {
            Material material_temp = new Material(DrawImage_.material);
            mapinfo.grid_size_ = grid_size;
            //DrawRawImage_.rectTransform.
            DrawRawImage_.texture = _mapinfo.CreateTexture(false);
            RectTransform rect = DrawRawImage_.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(DrawRawImage_.texture.width, DrawRawImage_.texture.height);
        }
    }

    public void ResetDrawBtns()
    {
        int color_index = 0;
        Dictionary<int, Color> colors = global_instance.Instance.getGroupColors();
        foreach (KeyValuePair<int, Color> entry_pair in colors)
        {
            ColorBoard_.SetButtonGroupColor(color_index, entry_pair.Key, entry_pair.Value);
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
			if (global_instance.Instance._player.isadmin () == false) 
			{
				PrepareSaveBtn_.gameObject.SetActive (true);
				SaveBtn_.gameObject.SetActive (false);
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
		global_instance.Instance.SetMapData(_mapinfo);

		global_instance.Instance._ngui_edit_manager.on_save_btn_click ();
	}

	public void OnPrepareSaveClick()
	{
        global_instance.Instance.SetMapData(_mapinfo);
        global_instance.Instance._ngui_edit_manager.update_game_type (game_type.game);
	}

	public void OnBackClick()
	{
		global_instance.Instance._ngui_edit_manager.BackToMainPanel ();
        ResetDrawBtns();
    }

	public void OnCreateClick()
	{
		global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
	}


}
