using AndroidX.AppCompat.App;
using TablaGrocerMobile.Fragments;

namespace TablaGrocerMobile.Activities;
[Activity(Label = "Templates")]
public class TemplatesActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_templates);

        if (savedInstanceState == null)
        {
            // Load the first fragment by default
            SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_container, new TemplateListFragment())
                .Commit();
        }
    }
}