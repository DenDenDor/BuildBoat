using UnityEngine;

public class AbstractPanelChildrenHandler : MonoBehaviour
{
    private Transform _childrenHolder;

    public Transform Initialize()
    {
        GameObject newParent = new GameObject("UI_ChildrenHolder", typeof(RectTransform));
        _childrenHolder = newParent.transform;
        _childrenHolder.SetParent(transform, false);
        
        RectTransform newParentRT = _childrenHolder.GetComponent<RectTransform>();
        newParentRT.anchorMin = new Vector2(0.5f, 0.5f);
        newParentRT.anchorMax = new Vector2(0.5f, 0.5f);
        newParentRT.pivot = new Vector2(0.5f, 0.5f);
        newParentRT.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        newParentRT.localPosition = Vector3.zero;
        newParentRT.localRotation = Quaternion.identity;
        newParentRT.localScale = Vector3.one;
        
        MoveChildrenToHolder();
        ReverseChildrenOrder();
        
        return _childrenHolder;
    }

    private void MoveChildrenToHolder()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child != _childrenHolder)
            {
                child.SetParent(_childrenHolder, true);
            }
        }
    }

    private void ReverseChildrenOrder()
    {
        int childCount = _childrenHolder.childCount;
        for (int i = 0; i < childCount / 2; i++)
        {
            int oppositeIndex = childCount - 1 - i;
            Transform childA = _childrenHolder.GetChild(i);
            Transform childB = _childrenHolder.GetChild(oppositeIndex);
            childA.SetSiblingIndex(oppositeIndex);
            childB.SetSiblingIndex(i);
        }
    }
}