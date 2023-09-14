using UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    /*[SerializeField]
    private*/public Button FirstSelected;

    protected virtual void OnEnable()
    {
        SetFirstSelected(FirstSelected);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}