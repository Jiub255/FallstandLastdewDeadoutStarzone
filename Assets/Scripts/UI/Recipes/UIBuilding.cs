using System;
using System.Collections.Generic;

public class UIBuilding : UIRecipes
{
	public static event Func<List<SORecipe>, List<SORecipe>> OnGetHaveEnoughItemsRecipes;

    protected override void GetHaveEnoughItemsRecipes()
    {
        // Building manager listens, sends back all recipes that you have enough items to craft/build. 
        _haveEnoughItemsRecipes = OnGetHaveEnoughItemsRecipes(_metRequirementsRecipes);
    }
}