using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationText : MonoBehaviour
{
    // Start is called before the first frame update
    UIAnimationComponent _animationComponent = null;
    public Text text_;
    
    private void Awake()
    {
        _animationComponent = GetComponent<UIAnimationComponent>();
        if(_animationComponent != null)
        {
            _animationComponent.setOK(OnAnimationEnd);
        }
        text_.gameObject.SetActive(false);
    }
    void Start()
    {
        text_.gameObject.SetActive(false);
    }

    public void setText(string txt)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimationEnd()
    {
        text_.gameObject.SetActive(true);
    }
}
