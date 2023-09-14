using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPopupMenu : Menu
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI _displayText;
    [SerializeField]
    private Button _confirmButton;
    [SerializeField]
    private Button _cancelButton;

    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        gameObject.SetActive(true);

        // Set the display text
        this._displayText.text = displayText;

        // Remove any existing listeners just to make sure there aren't any previous ones hanging around
        // Note: this only removes listeners added through code
        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        // Assign the onClick listeners
        _confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });
        _cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }

    private void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}