using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private SORecipe _recipeSO;

    public void SetupSlot(SORecipe recipeSO)
    {
        _recipeSO = recipeSO;
        _image.sprite = recipeSO.Icon; 
    }

    // Called by clicking on inventory slot
	public void OnClickRecipe()
    {
        if (_recipeSO != null)
        {
            _recipeSO.OnClickRecipe();
        }
    }
}