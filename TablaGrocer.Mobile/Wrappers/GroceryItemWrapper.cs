using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.Wrappers;

public class GroceryItemWrapper : Java.Lang.Object
{
    public GroceryItem GroceryItem { get; }

    public GroceryItemWrapper(GroceryItem groceryRun)
    {
        GroceryItem = groceryRun;
    }
}