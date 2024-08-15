using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.DatePicker;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using TablaGrocerMobile.CustomAdapters;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile;

[Activity]
public class GroceryRunActivity : AppCompatActivity,IOnClickListener<GroceryItem>,IOnGroceryRunCheckedChangeListener<GroceryItem>
{
    private RecyclerView _recyclerView;
    private List<GroceryItem> _groceryItems;
    private GroceryItemAdapter _groceryItemAdapter;
    private FloatingActionButton _fab;
    private EditText _groceryEditText;
    private EditText _groceryQuantityEditText;
    private EditText _groceryPriceEditText;
    private LayoutInflater _inflater;
    private AndroidX.AppCompat.Widget.Toolbar? _toolbar;
    private int _groceryRunId;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_grocery_run);
        _toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar?>(Resource.Id.toolbar);
        _recyclerView = FindViewById<RecyclerView>(Resource.Id.grocery_item_recycler_view);
        _fab = FindViewById<FloatingActionButton>(Resource.Id.grocery_item_fab);
        _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
        _groceryRunId = Intent.GetIntExtra("grocery_run_id",0);

        using (var ctx = new AppDbContext())
        {
          _groceryItems =  ctx.GroceryItems.Where(e => e.GroceryRunId == _groceryRunId).ToList();
        }
        
        
        // Initialize the adapter with the list of items
        _groceryItemAdapter = new GroceryItemAdapter(this, _groceryItems,this,this);

        // Set the adapter for the RecyclerView
        _recyclerView.SetAdapter(_groceryItemAdapter);
        _fab.Click += (sender, args) =>
        {
            ShowDialog();
        };
        _inflater = LayoutInflater.From(this);
        SetSupportActionBar(_toolbar);
    }
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.drawer_menu, menu);  // Inflate your menu resource
        return base.OnCreateOptionsMenu(menu);
    }
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.nav_home:
                // Handle action_search click
                return true;

            case Resource.Id.nav_settings:
                // Handle action_settings click
                return true;

            default:
                return base.OnOptionsItemSelected(item);
        }
    }
    public void ShowDialog()
    {
        View dialogView = _inflater.Inflate(Resource.Layout.grocery_item_dialog, null); 
        _groceryEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryEditText);
        _groceryQuantityEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryItemQuantityEditText);
        
        
        
        var dialog = new MaterialAlertDialogBuilder(this)
            .SetTitle("Grocery Item Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", async (s, args) =>
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
                    await ctx.GroceryItems.AddAsync(temp);
                    await ctx.SaveChangesAsync();
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
        return;
    }
    public async Task OnGroceryRunCheckedChanged(GroceryItem item,int position, bool isChecked)
    {
        // Update the IsCompleted property
        item = _groceryItems[position];
        item.IsDone = isChecked;
        // Update the database
        await using (var ctx = new AppDbContext())
        {
            ctx.GroceryItems.Update(item);
            await ctx.SaveChangesAsync();
        }

        // Find the index of the item in the list
       
            // Update the item in the list
            _groceryItems[position] = item;
            // Notify the adapter that the item has changed
            var task = new Task(() =>
            {
                _groceryItemAdapter.NotifyItemChanged(position);

            });

            await task;
    }
    public void OnEditClick(GroceryItem item, int position)
    {        
        var ctx = new AppDbContext();
        View dialogView = _inflater.Inflate(Resource.Layout.grocery_item_edit_dialog, null); 
        _groceryEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryEditText);
        _groceryQuantityEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryItemQuantityEditText);
        _groceryPriceEditText = dialogView.FindViewById<EditText>(Resource.Id.GroceryItemPriceEditText);

        var res = ctx.GroceryItems.Find(_groceryItems[position].Id);
        var id = res!.Id;
        _groceryEditText.Text = res.ItemName;
        _groceryQuantityEditText.Text = res.Quantity;
        _groceryPriceEditText.Text = res.Price.ToString();
        var dialog = new MaterialAlertDialogBuilder(this)
            .SetTitle("Grocery Item Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", (s, args) =>
            {
                
                if (string.IsNullOrEmpty(_groceryEditText.Text))
                {
                    Toast.MakeText(this, "Item Name cannot be empty", ToastLength.Long)!.Show();
                    return;
                }
        
                if (string.IsNullOrEmpty(_groceryQuantityEditText.Text))
                {
                    Toast.MakeText(this, "Please enter a valid quantity", ToastLength.Long)!.Show();
                    return;
                }
                
                
                res.ItemName = _groceryEditText.Text.Trim();
                res.Quantity = _groceryQuantityEditText.Text.Trim();
                res.Price = Decimal.Parse(_groceryPriceEditText.Text ?? "0.00");
                try
                {
                    _groceryItems[position] = res;
                    ctx.GroceryItems.Update(res);
                    ctx.SaveChanges();
                    ctx.Dispose();
                    _groceryItemAdapter.NotifyItemChanged(position);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long)!.Show();

                }
                Toast.MakeText(this, "Item Added", ToastLength.Long)!.Show();
            })
            .SetNegativeButton("Cancel", (s, args) => { })
            .Create();

        dialog.Show();
    }

    public void OnDeleteClick(GroceryItem item, int position)
    {
        using (var ctx = new AppDbContext())
        {
            var res = ctx.GroceryItems.Find(_groceryItems[position].Id);
            ctx.Remove(res);
            ctx.SaveChanges();
            _groceryItems.Remove(_groceryItems[position]);
            _groceryItemAdapter.NotifyItemRemoved(position);
            
            Toast.MakeText(this, "Item Deleted", ToastLength.Long).Show();

        }
    }

    
}