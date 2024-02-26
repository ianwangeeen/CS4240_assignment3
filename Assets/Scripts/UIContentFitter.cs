using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContentFitter : MonoBehaviour
{
    // Start is called before the first frame update
    public void FitContent()
    {
        if (transform.childCount > 0)
        {
            HorizontalLayoutGroup horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            int childrenCount = transform.childCount - 1;
            float childWidth = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
            float width = horizontalLayoutGroup.spacing * childrenCount + childrenCount * childWidth + horizontalLayoutGroup.padding.left;

            // height of content is 280
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, 280); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
