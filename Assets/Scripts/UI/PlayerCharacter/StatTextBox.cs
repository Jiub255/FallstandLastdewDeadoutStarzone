using TMPro;
using UnityEngine;

public class StatTextBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _statText;

	public void SetupText(string text)
    {
        _statText.text = text;
    }
}