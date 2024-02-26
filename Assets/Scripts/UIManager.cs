using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private Button btn;
    [SerializeField] private RawImage buttonImage;
    private int _itemId;
    private Sprite btnSprite;
    public int ItemId {
        set {
            _itemId = value;
        }
    }

    public Sprite BtnSprite {
        set {
        btnSprite = value;
        buttonImage.texture = btnSprite.texture;
        }
    }  


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectedFurnitureManager.Instance.OnEntered(gameObject)) {
            transform.DOScale(Vector3.one * 2, 0.2f);
            // transform.localScale = Vector3.one * 2;
        } else {
            transform.DOScale(Vector3.one, 0.2f);
            // transform.localScale = Vector3.one;
        }
    }

    void SelectObject() {
        DataHandler.Instance.SetFurniture(_itemId);
    }
}
