using System;
using System.Collections.Generic;

public class UIBuilding : UIRecipes
{
	public static event Func<List<SORecipe>, List<SORecipe>> OnGetHaveEnoughItemsRecipes;

    protected override void GetHaveEnoughItemsRecipes()
    {
        // Building manager listens, sends back all recipes that you have enough items to craft/build. 
        // TODO - Why not just send it through PlayerInventoryManager like UICrafting and eliminate the need for 
        // these two separate child classes? Or actually still might need the child classes. Figure it out. 
        _haveEnoughItemsRecipes = OnGetHaveEnoughItemsRecipes(_metRequirementsRecipes);
    }
}