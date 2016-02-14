using UnityEngine;
using System.Collections;
using UnityEngine.UI ;


//竖直布局的ScrollView


public class ScrollViewVerticalAdjust : ScrollViewAdjust 
{
	VerticalLayoutGroup layout ;	//布局	

	protected override void InitLayoutInfo ()
	{
		layout = GetComponentInChildren<VerticalLayoutGroup>() ; 
		layoutRect = layout.GetComponent<RectTransform>() ; 
	}

	protected override Vector2 GetRealSize()
	{
		float width = 0 ;
		float height = 0 ;
		float maxWidth = 0 ;
		
		int childCount = 0 ;
		foreach(Transform ts in layout.transform)
		{
			if(ts.gameObject.activeInHierarchy)
			{
				childCount++;
				
				Image image = ts.GetComponent<Image>() ; 
				if(image!=null)
				{
					height += image.preferredHeight ;
					maxWidth = image.preferredWidth>maxWidth?image.preferredWidth:maxWidth ;
				}
			}
		}
		
		width = maxWidth + layout.padding.left + layout.padding.right  ; 
		height = height + layout.padding.top + layout.padding.bottom + layout.spacing * (childCount - 1) ; 
		return new Vector2(width,height) ;	
	}
}
