using Android;
using Android.Content;
using AndroidX.AppCompat.App;

namespace TablaGrocerMobile;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class ListMenuActivity : AppCompatActivity
{
    private ListView _listView;
    private string[] items = new[] { "View or Create New Grocery Runs", "View or Create New Grocery Item" };
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_list);
        initializeViews();
        _listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, items);
        _listView.ItemClick += (sender, e) =>
        {
            handleItemClick(e.Position);
        };
    }
    
    private void initializeViews() {
        _listView = FindViewById<ListView>(Resource.Id.menuListView);
    }
    private void handleItemClick(int position) {
        Intent intent;
        switch (position) {
            case 0:
                intent = new Intent(this, typeof(MainActivity));
                break;
            /*case 1:
                intent = new Intent(this, Activity2.class);
                break;
            case 2:
                intent = new Intent(this, Activity3.class);
                break;*/
            default:
                return;
        }
        StartActivity(intent);
    }
}

