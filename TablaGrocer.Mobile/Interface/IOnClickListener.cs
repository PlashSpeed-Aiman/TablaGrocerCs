using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.Interface;

public  interface IOnClickListener<T> 
{
    void OnItemClick(T item, int position);
    void OnEditClick(T item, int position);
    void OnDeleteClick(T item, int position);
}

public interface IOnGroceryRunCheckedChangeListener<T>
{
    Task OnGroceryRunCheckedChanged(T item, int position, bool isChecked);
}

