using UnityEngine;

public class UITooltip : MonoBehaviour
{
	[SerializeField]
	private Tooltip _tooltip;

    private void OnEnable()
    {
        TooltipTrigger.OnEnterTooltipArea += Show;
        TooltipTrigger.OnExitTooltipArea += Hide;
    }

    private void OnDisable()
    {
        TooltipTrigger.OnEnterTooltipArea -= Show;
        TooltipTrigger.OnExitTooltipArea -= Hide;
    }

    private void Show(string title, string description, RectTransform rectTransform)
    {
        _tooltip.SetupTooltip(title, description, rectTransform);
        _tooltip.gameObject.SetActive(true);
    }

	private void Hide()
    {
        _tooltip.gameObject.SetActive(false);
    }
}