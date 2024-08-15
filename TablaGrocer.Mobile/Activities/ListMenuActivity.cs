using Android;
using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Navigation;
using TablaGrocerMobile.Activities;

namespace TablaGrocerMobile;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class ListMenuActivity : AppCompatActivity
{
    private DrawerLayout _drawerLayout;
    private NavigationView _navigationView;
    private Button? _groceryRunButton;
    private Button? _templatesMenuButton;
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
        _templatesMenuButton.Click += (sender, args) =>
        {
            Intent intent; 
            intent = new Intent(this, typeof(TemplatesActivity));
            StartActivity(intent);
        };

    }
    
    private void initializeViews() {
        var toolbar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);
        SetSupportActionBar(toolbar);
        _groceryRunButton = FindViewById<Button>(Resource.Id.textButton3);
        _templatesMenuButton = FindViewById<Button>(Resource.Id.templates_menu_button);
    }
    private void handleItemClick() {
        Intent intent; 
        intent = new Intent(this, typeof(MainActivity));
        StartActivity(intent);
    }
}

