using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour {

    public GameObject[] InitPosObj_;

    public RawImage OfficilMapImage_;
    public RawImage CreateMapImage_;
    public RawImage NewMapImage_;
    public RawImage RankMapImage_;
    public RawImage MyMapImage_;


    protected List<Vector3> _ButtonPos = new List<Vector3>();

    void Awake()
    {
        string path_language = global_instance.Instance.GetLanguagePath();
        string texture_path = "ui_texture" + path_language + "officil_map";
        OfficilMapImage_.texture = Resources.Load<Texture2D>(texture_path);
        texture_path = "ui_texture" + path_language + "create_map";
        CreateMapImage_.texture = Resources.Load<Texture2D>(texture_path);
        texture_path = "ui_texture" + path_language + "new_map";
        NewMapImage_.texture = Resources.Load<Texture2D>(texture_path);
        texture_path = "ui_texture" + path_language + "rank_map";
        RankMapImage_.texture = Resources.Load<Texture2D>(texture_path);
        texture_path = "ui_texture" + path_language + "my_map";
        MyMapImage_.texture = Resources.Load<Texture2D>(texture_path);
        
        
        foreach (GameObject obj in InitPosObj_)
        {
            Vector3 vec = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y , obj.transform.localPosition.z);

            _ButtonPos.Add(vec);
        }
    }

	public void OnOfficilMapBtnClick ()
	{
		global_instance.Instance._ngui_edit_manager.ShowChapterPanel ();
	
	}

	public void OnCreateMapBtnClick()
	{
		global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
	}

	public void OnRankMapBtnClick()
	{

	}

	public void OnNewMapBtnClick()
	{
		
	}

	public void OnMyMapBtnClick()
	{
		global_instance.Instance._ngui_edit_manager.HidMainMenuPanel ();
		global_instance.Instance._ngui_edit_manager._map_panel.setPage (page_type.page_type_self_complete);
		global_instance.Instance._ngui_edit_manager.ShowMapPanel();

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
