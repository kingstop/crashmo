using UnityEngine;
using System.Collections;
using UnityEngine.UI ;	

//针对水平方向布局调整

public class ScrollViewHarizonAdjust : ScrollViewAdjust
{
	HorizontalLayoutGroup layout ;		

	protected override void InitLayoutInfo ()
	{
		layout = GetComponentInChildren<HorizontalLayoutGroup> (); 
		layoutRect = layout.GetComponent<RectTransform> (); 
	}

	protected override Vector2 GetRealSize()
	{
		float width = 0 ;
		float height = 0 ;
		float maxHeight = 0 ;

		int childCount = 0 ;
		foreach(Transform ts in layout.transform)
		{
			if(ts.gameObject.activeInHierarchy)
			{
				childCount++;

				Image image = ts.GetComponent<Image>(); 
				if(image!=null)
				{
					width += image.preferredWidth; 
					maxHeight = image.preferredHeight > maxHeight?image.preferredHeight:maxHeight;
				}
			}
		}

		width = width + layout.padding.left + layout.padding.right + layout.spacing * (childCount - 1) ; 
		height = maxHeight + layout.padding.top + layout.padding.bottom ; 
		return new Vector2(width,height) ;	
	}
}
