using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PageControl<T> : Singleton<T> where T : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    public virtual void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
