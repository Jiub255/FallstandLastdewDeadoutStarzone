using UnityEngine;
using UnityEngine.UI;

public class PainInjuryUI : MonoBehaviour
{
	[SerializeField]
	private Image _painFillbar;
	[SerializeField]
	private Image _injuryFillbar;

    private void OnEnable()
    {
        PlayerPain.OnPainChanged += UpdatePainBar;
        PlayerInjury.OnInjuryChanged += UpdateInjuryBar;
    }

    private void OnDisable()
    {
        PlayerPain.OnPainChanged -= UpdatePainBar;
        PlayerInjury.OnInjuryChanged -= UpdateInjuryBar;
    }

    private void UpdatePainBar(int pain)
    {
		_painFillbar.fillAmount = (float)pain / 100f;
    }

	private void UpdateInjuryBar(int injury)
    {
		_injuryFillbar.fillAmount = (float)injury / 100f;
    }
}