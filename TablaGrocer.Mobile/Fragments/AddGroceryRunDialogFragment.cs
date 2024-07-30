using Android.Views;

namespace TablaGrocerMobile.Fragments;

public class AddGroceryRunDialogFragment : DialogFragment
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View view = inflater.Inflate(Resource.Layout.fragment_my_dialog, container, false);

        // Get reference to the button in the dialog
      

        return view;
    }
}