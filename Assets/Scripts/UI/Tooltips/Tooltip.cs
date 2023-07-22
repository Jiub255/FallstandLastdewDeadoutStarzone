using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
	private TextMeshProUGUI _titleText;
    [SerializeField]
	private TextMeshProUGUI _descriptionText;
    [SerializeField]
    private LayoutElement _layoutElement;
    [SerializeField]
    private RectTransform _rectTransform;

    public void SetupTooltip(string title, string description, RectTransform rectTransform)
    {
        SetTitleText(title);
        SetDescriptionText(description);
        SetSize();
        SetPivot(rectTransform);
        SetTooltipPosition(rectTransform);
    }

    private void SetTitleText(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            _titleText.gameObject.SetActive(false);
        }
        else
        {
            _titleText.gameObject.SetActive(true);
            _titleText.text = title;
        }
    }

    private void SetDescriptionText(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            _descriptionText.gameObject.SetActive(false);
        }
        else
        {
            _descriptionText.gameObject.SetActive(true);
            _descriptionText.text = description;
        }
    }

    /// <summary>
    /// Control size of tooltip box. Turns off layout element if text shorter than preferred width. 
    /// </summary>
    private void SetSize()
    {
        _layoutElement.enabled = Mathf.Max(_titleText.preferredWidth, _descriptionText.preferredWidth) >= _layoutElement.preferredWidth;
    }

    /// <summary>
    /// Set pivot based on screen position of UI element, so tooltip always stays on screen. 
    /// </summary>
    /// <param name="rectTransform"></param>
    private void SetPivot(RectTransform rectTransform)
    {
        float pivotX = Mathf.Round(rectTransform.position.x / Screen.width);
        float pivotY = Mathf.Round(rectTransform.position.y / Screen.height);
        _rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    /// <summary>
    /// Move tooltip to top or bottom of UI element, depending on element's screen position. 
    /// </summary>
    /// <param name="rectTransform"></param>
    private void SetTooltipPosition(RectTransform rectTransform)
    {
        Vector3 position;
        if (rectTransform.position.y / Screen.height < 0.5f)
        {
            float y = rectTransform.position.y + (rectTransform.sizeDelta.y / 2f);
            position = new Vector3(rectTransform.position.x, y, rectTransform.position.z);
        }
        else
        {
            float y = rectTransform.position.y - (rectTransform.sizeDelta.y / 2f);
            position = new Vector3(rectTransform.position.x, y, rectTransform.position.z);
        }
        transform.position = position;
    }
}