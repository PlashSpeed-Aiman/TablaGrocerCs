using Android;
using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Navigation;

namespace TablaGrocerMobile;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class ListMenuActivity : AppCompatActivity
{
    private DrawerLayout _drawerLayout;
    private NavigationView _navigationView;
    private Button? _groceryRunButton;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_list);
        initializeViews();
        _groceryRunButton.Click += (sender, args) =>
        {
            handleItemClick();
        };

    }
    
    private void initializeViews() {
        var toolbar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);
        SetSupportActionBar(toolbar);
        _groceryRunButton = FindViewById<Button>(Resource.Id.textButton3);

    }
    private void handleItemClick() {
        Intent intent; 
        intent = new Intent(this, typeof(MainActivity));
        StartActivity(intent);
    }
}

