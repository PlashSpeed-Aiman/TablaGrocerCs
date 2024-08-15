using Android.Views;

namespace TablaGrocerMobile.Fragments;

public class TemplateListItemFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Inflate the layout for this fragment
        View view = inflater.Inflate(Resource.Layout.fragment_templates_items_list, container, false);

        // Handle button click to switch to FragmentTwo
        Button button = view.FindViewById<Button>(Resource.Id.buttonSwitchToFragmentOne);
        button.Click += (sender, e) =>
        {
            AndroidX.Fragment.App.FragmentTransaction? transaction = Activity.SupportFragmentManager.BeginTransaction();
            transaction.SetCustomAnimations(
                Resource.Animation.slide_in, Resource.Animation.fade_out, Resource.Animation.fade_in,
                Resource.Animation.slide_out
            );
            transaction.Replace(Resource.Id.fragment_container, new TemplateListFragment());
            transaction.AddToBackStack(null);
            transaction.Commit();
        };

        return view;
    }
}