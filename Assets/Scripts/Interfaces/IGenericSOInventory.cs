using System;
using System.Collections.Generic;

// Just to grab generic SOInventory<T>s. 
public interface IGenericSOInventory
{
	public event Action OnInventoryChanged;

	public List<ItemAmount<SOItem>> ItemAmounts { get; protected set; }
}