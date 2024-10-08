﻿
using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using TablaGrocerMobile.CustomAdapters;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.Fragments;

public class TemplateListFragment : AndroidX.Fragment.App.Fragment, IOnClickListener<Template>
{
    private RecyclerView? _recyclerView;
    private List<Template> _templates;
    private TemplatesListAdapter _templatesListAdapter;
    private FloatingActionButton? _fab;
    private LayoutInflater _inflater;
    private Context _context;
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        using (var ctx = new AppDbContext())
        {
            _templates = ctx.Templates.ToList();
        }
        // Inflate the layout for this fragment
        _context = container.Context;
        _inflater = inflater;
        View view = _inflater.Inflate(Resource.Layout.fragment_templates_list, container, false);
        _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view_templates);
        _templatesListAdapter = new TemplatesListAdapter(view.Context, _templates, this);
        _recyclerView?.SetAdapter(_templatesListAdapter);
        _recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
        _fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab_templates);
        _fab.Click += AddNewTemplate;

        return view;
    }

    public void AddNewTemplate(object? sender, EventArgs args)
    {
        View dialogView = _inflater.Inflate(Resource.Layout.template_info_dialog, null);

        var editTextName = dialogView.FindViewById<EditText>(Resource.Id.editTemplateName);
        var dialog = new MaterialAlertDialogBuilder(Context)
            .SetTitle("Template Details")!
            .SetView(dialogView)!
            .SetPositiveButton("OK", (s, args) =>
            {
                Template temp = new Template
                {
                    TemplateName =  editTextName.Text,
                };
                //TODO : this doesn't work, TemplateName will always be null
                using (var ctx = new AppDbContext())
                {
                    ctx.Templates.Add(temp);
                    ctx.SaveChanges();
                    _templatesListAdapter.AddItem(temp);
                }
                Toast.MakeText(_context, editTextName.Text ?? "Value is Null", ToastLength.Long)!.Show();
            })
            .SetNegativeButton("Cancel", (s, args) => { })
            .Create();

        dialog.Show();
    }
    
    public void ViewTemplate(int position)
    {
        // Handle button click to switch to FragmentTwo
        AndroidX.Fragment.App.FragmentTransaction? transaction = Activity.SupportFragmentManager.BeginTransaction();
        transaction.SetCustomAnimations(
            Resource.Animation.slide_in, Resource.Animation.fade_out, Resource.Animation.fade_in,
            Resource.Animation.slide_out
        );
        transaction.Replace(Resource.Id.fragment_container, new TemplateListItemFragment());
        transaction.AddToBackStack(null);
        transaction.Commit();
    }

    public void OnItemClick(Template item, int position)
    {
        throw new NotImplementedException();
    }

    public void OnEditClick(Template item, int position)
    {
        throw new NotImplementedException();
    }

    public void OnDeleteClick(Template item, int position)
    {
        throw new NotImplementedException();
    }
}