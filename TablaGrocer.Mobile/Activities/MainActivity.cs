using Android.Content;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.DatePicker;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Microsoft.EntityFrameworkCore;
using TablaGrocerMobile.CustomAdapters;
using TablaGrocerMobile.Fragments;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore;
using TableGrocer.EFCore.Models;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace TablaGrocerMobile;

[Activity(Label = "@string/app_name")]
public class MainActivity : AppCompatActivity, IOnClickListener<GroceryRun>
{
    private FloatingActionButton? _fab;
    private RecyclerView? _recyclerView;
    private List<GroceryRun> _groceryRuns;
    private GroceryRunAdapter _groceryRunAdapter;
    private MaterialDatePicker _datePicker;
    private LayoutInflater _inflater;


    protected override void OnCreate(Bundle? savedInstanceState)
    {
        
       

        base.OnCreate(savedInstanceState);
        
        using (var ctx = new AppDbContext())
        {
            _groceryRuns = ctx.GroceryRuns.ToList();
        }
        
        _groceryRunAdapter = new GroceryRunAdapter(this, _groceryRuns,this );
        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);


        
        
        _recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
        
        _recyclerView?.SetAdapter(_groceryRunAdapter);
        _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
        _recyclerView.Click += (sender, args) =>
        {
            HandleItemClick(0);
        };
        _fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
        _fab.Click += (sender, e) => {
            ShowGroceryRunDialog();
        };
        
        _inflater = LayoutInflater.From(this);

        // Setup DatePicker once
        _datePicker = MaterialDatePicker.Builder.DatePicker()
            .SetTitleText("Select Date")
            .Build();

    }
    public void OnItemClick(GroceryRun item, int position)
    {
        var intent = new Intent(this, typeof(GroceryRunActivity));
        int id = (int)_groceryRuns[position].Id;
        intent.PutExtra("grocery_run_id", id);
        StartActivity(intent);
    }

    public void OnEditClick(GroceryRun item, int position)
    {
        ShowEditGroceryRunDialog(position);
    }

    public void OnDeleteClick(GroceryRun item, int position)
    {
        using (var ctx = new AppDbContext())
        {
            var res = ctx.GroceryRuns.Find(_groceryRuns[position].Id);
            ctx.Remove(res);
            ctx.SaveChanges();
            _groceryRuns.Remove(_groceryRuns[position]);
            _groceryRunAdapter.NotifyItemRemoved(position);
            
            Toast.MakeText(this, "Item Deleted", ToastLength.Long).Show();

        }
    }
    private void HandleItemClick(int position)
    {
    
        Intent intent;
        intent = new Intent(this, typeof(GroceryRunActivity));
        StartActivity(intent);
        
    }

    private void ShowEditGroceryRunDialog(int position)
    {
        var ctx = new AppDbContext();
        
        View dialogView = _inflater.Inflate(Resource.Layout.fragment_my_dialog, null);

        var editTextName = dialogView.FindViewById<EditText>(Resource.Id.editTextName);
        var editTextDate = dialogView.FindViewById<EditText>(Resource.Id.editTextDate);
        var editTextPlace = dialogView.FindViewById<EditText>(Resource.Id.editTextPlace);

        
        var res = ctx.GroceryRuns.Find(_groceryRuns[position].Id);
        var id = res!.Id;
        editTextName!.Text = res.Name;
        editTextDate!.Text = res.Date;
        editTextPlace!.Text = res.PlaceOfPurchase;
        
        
        
        editTextDate.Click += (s, args) =>
        {
            _datePicker.Show(this.SupportFragmentManager, "MATERIAL_DATE_PICKER");
        };

        _datePicker.AddOnPositiveButtonClickListener(new OnPositiveButtonClickListener(editTextDate));
        
        var dialog = new MaterialAlertDialogBuilder(this)
            .SetTitle("Grocery Run Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", (s, args) =>
            {
                string? name = editTextName.Text;
                string? date = editTextDate.Text;
                string? place = editTextPlace.Text;
                res.Name = name;
                res.Date = date;
                res.PlaceOfPurchase = place;
                try
                {
                    _groceryRuns[position] = res;
                    ctx.GroceryRuns.Update(res);
                    ctx.SaveChanges();
                    ctx.Dispose();
                    _groceryRunAdapter.NotifyItemChanged(position);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long)!.Show();

                }
                
            })
            .SetNegativeButton("Cancel", (s, args) => { })
            .Create();

        dialog.Show();
        
        
    }
    private void ShowGroceryRunDialog()
    {
        View dialogView = _inflater!.Inflate(Resource.Layout.fragment_my_dialog, null);

        var editTextName = dialogView.FindViewById<EditText>(Resource.Id.editTextName);
        var editTextDate = dialogView.FindViewById<EditText>(Resource.Id.editTextDate);
        var editTextPlace = dialogView.FindViewById<EditText>(Resource.Id.editTextPlace);
        
        editTextDate!.Click += (s, args) =>
        {
            _datePicker.Show(this.SupportFragmentManager, "MATERIAL_DATE_PICKER");
        };
        
        _datePicker.AddOnPositiveButtonClickListener(new OnPositiveButtonClickListener(editTextDate));
        
        var dialog = new MaterialAlertDialogBuilder(this)
            .SetTitle("Grocery Run Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", (s, args) =>
            {
                string name = editTextName.Text;
                string date = editTextDate.Text;
                string place = editTextPlace.Text;
                GroceryRun temp = new GroceryRun
                {
                    Name = name,
                    Date = date,
                    PlaceOfPurchase = place
                };
                using (var ctx = new AppDbContext())
                {
                    ctx.GroceryRuns.Add(temp);
                    ctx.SaveChanges();
                }
                _groceryRunAdapter.AddItem(temp);
                Toast.MakeText(this, "Item Added", ToastLength.Long)!.Show();
            })
            .SetNegativeButton("Cancel", (s, args) => { })
            .Create();

        dialog.Show();
    }

   
}

class OnPositiveButtonClickListener : Java.Lang.Object,IMaterialPickerOnPositiveButtonClickListener
{
    private EditText _editTextDate;

    public OnPositiveButtonClickListener(EditText editTextDate)
    {
        _editTextDate = editTextDate;
    }

    public void OnPositiveButtonClick(Java.Lang.Object selection)
    {
        var date = DateTimeOffset.FromUnixTimeMilliseconds((long)selection);
        _editTextDate.Text = date.ToString("MM/dd/yyyy");
    }
}