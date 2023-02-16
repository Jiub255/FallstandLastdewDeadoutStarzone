using System.Collections.Generic;

public interface ICraftable
{
	public List<CraftingItemAmount> CraftingCost { get; set; }

	public void Craft();
}