using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<string, string, RectTransform> OnEnterTooltipArea;
    public static event Action OnExitTooltipArea;

    [SerializeField]
    private string _title;
    [SerializeField]
    private string _description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnterTooltipArea?.Invoke(_title, _description, GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitTooltipArea?.Invoke();
    }
}