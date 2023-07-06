using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    // TODO - Set up recipe cost UI. Use icons and numbers instead of all text? 

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