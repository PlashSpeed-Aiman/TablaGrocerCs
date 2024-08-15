using Android.Views;

namespace TablaGrocerMobile.Fragments;

public class TemplateListItemFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Inflate the layout for this fragment
        View view = inflater.Inflate(Resource.Layout.fragment_template_items_list, container, false);

        // Handle button click to switch to FragmentTwo
       

        return view;
    }
}