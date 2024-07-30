using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.DatePicker;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using TablaGrocerMobile.CustomAdapters;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile;

[Activity(Label = "GroceryRunActivity")]
public class GroceryRunActivity : AppCompatActivity,IOnClickListener<GroceryItem>
{
    private RecyclerView _recyclerView;
    private List<GroceryItem> _groceryItems;
    private GroceryItemAdapter _groceryItemAdapter;
    private FloatingActionButton _fab;
    private EditText _groceryEditText;
    private EditText _groceryQuantityEditText;
    private LayoutInflater _inflater;
    private int _groceryRunId;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_grocery_run);
        _recyclerView = FindViewById<RecyclerView>(Resource.Id.grocery_item_recycler_view);
        _fab = FindViewById<FloatingActionButton>(Resource.Id.grocery_item_fab);
        _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
        _groceryRunId = Intent.GetIntExtra("grocery_run_id",0);

        using (var ctx = new AppDbContext())
        {
          _groceryItems =  ctx.GroceryItems.Where(e => e.GroceryRunId == _groceryRunId).ToList();
        }
        
        
        // Initialize the adapter with the list of items
        _groceryItemAdapter = new GroceryItemAdapter(this, _groceryItems,this);

        // Set the adapter for the RecyclerView
        _recyclerView.SetAdapter(_groceryItemAdapter);
        _fab.Click += (sender, args) =>
        {
            ShowDialog();
        };
        _inflater = LayoutInflater.From(this);
        

    }

    public void ShowDialog()
    {
        View dialogView = _inflater.Inflate(Resource.Layout.grocery_item_dialog, null); 
        _groceryEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryEditText);
        _groceryQuantityEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryItemQuantityEditText);
        var dialog = new MaterialAlertDialogBuilder(this)
            .SetTitle("Grocery Item Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", (s, args) =>
            {
                string itemName = _groceryEditText.Text.Trim();
                string quantityText = _groceryQuantityEditText.Text.Trim();
                if (string.IsNullOrEmpty(itemName))
                {
                    Toast.MakeText(this, "Item Name cannot be empty", ToastLength.Long)!.Show();
                    return;
                }
        
                if (string.IsNullOrEmpty(quantityText))
                {
                    Toast.MakeText(this, "Please enter a valid quantity", ToastLength.Long)!.Show();
                    return;
                }
                
                GroceryItem temp = new GroceryItem
                {
                    ItemName = itemName,
                    Quantity = quantityText,
                    Price = new decimal(0.00),
                    IsDone = false,
                    GroceryRunId = _groceryRunId
                };
                using (var ctx = new AppDbContext())
                {
                    ctx.GroceryItems.Add(temp);
                    ctx.SaveChanges();
                }
                _groceryItemAdapter.AddItem(temp);
                Toast.MakeText(this, "Item Added", ToastLength.Long)!.Show();
            })
            .SetNegativeButton("Cancel", (s, args) => { })
            .Create();

        dialog.Show();
    }

    public void OnItemClick(GroceryItem item, int position)
    {
        throw new NotImplementedException();
    }

    public void OnEditClick(GroceryItem item, int position)
    {
        throw new NotImplementedException();
    }

    public void OnDeleteClick(GroceryItem item, int position)
    {
        throw new NotImplementedException();
    }
}