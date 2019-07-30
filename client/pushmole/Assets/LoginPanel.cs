using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _login_obj;
    public GameObject _register_obj;
    public Text _account;
    public Text _password;

    void Awake()
    {
        //Resources.Load(path, typeof(Sprite)) as Sprite;
        Image login_image = _login_obj.GetComponent<Image>();        
        if (login_image)
        {
            string login_path = "ui_texture" + global_instance.Instance.GetLanguagePath() + "login";
            login_image.sprite = Resources.Load(login_path, typeof(Sprite)) as Sprite;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoginClick ()
    {
        string account = _account.text;
        string password = _account.text;
        global_instance.Instance._client_session.login(account, password);
    }

    void OnRegisterClick()
    {

    }
}
