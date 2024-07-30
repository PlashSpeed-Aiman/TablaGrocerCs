using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.CustomAdapters;

public class GroceryRunAdapter : RecyclerView.Adapter

{
    private readonly Context context;
    private readonly List<GroceryRun> _items;
    private readonly IOnClickListener<GroceryRun> _listener;

    private class GroceryRunViewHolder : RecyclerView.ViewHolder
    {

        public TextView NameTextView { get; }
        public TextView DateTextView { get; }
        public TextView PlaceTextView { get;}
        public ImageView EditGroceryRunImageView { get; }
        public ImageView DeleteGroceryRunImageView { get; }
        public GroceryRunViewHolder(View itemView,IOnClickListener<GroceryRun> listener) : base(itemView)
        {
            NameTextView = itemView.FindViewById<TextView>(Resource.Id.textViewName);
            DateTextView = itemView.FindViewById<TextView>(Resource.Id.textViewDate);
            PlaceTextView = itemView.FindViewById<TextView>(Resource.Id.textViewPlace);
            EditGroceryRunImageView = itemView.FindViewById<ImageView>(Resource.Id.edit_grocery_run);
            DeleteGroceryRunImageView = itemView.FindViewById<ImageView>(Resource.Id.delete_grocery_run);
            itemView.Click += (sender, e) => listener.OnItemClick((GroceryRun)(Object)itemView.Tag, AdapterPosition);
            EditGroceryRunImageView.Click += (sender, e) => listener.OnEditClick((GroceryRun)(Object)itemView.Tag, AdapterPosition);
            DeleteGroceryRunImageView.Click += (sender, e) => listener.OnDeleteClick((GroceryRun)(Object)itemView.Tag, AdapterPosition);
        }
    }
    public GroceryRunAdapter(Context context, List<GroceryRun> items,IOnClickListener<GroceryRun> listener)
    {
        this.context = context;
        this._items = items;
        _listener = listener;
    }


    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        GroceryRunViewHolder vh = holder as GroceryRunViewHolder;
        
        var item = _items[position];
        vh.NameTextView.Text = item.Name;
        vh.DateTextView.Text = item.Date;
        vh.PlaceTextView.Text = item.PlaceOfPurchase;
        /*vh.ItemView.Click += (sender, e) => _listener.OnItemClick(item, position);
        vh.EditGroceryRunImageView.Click += (sender, e) => _listener.OnEditClick(item, position);
        vh.DeleteGroceryRunImageView.Click += (sender, e) => _listener.OnDeleteClick(item, position);*/
        vh.ItemView.Tag = (Object)item as Java.Lang.Object;
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.grocery_run_row_item, parent, false);
        return new GroceryRunViewHolder(itemView,_listener);
    }
    public void AddItem(GroceryRun item)
    {
        _items.Add(item);
        NotifyItemInserted(_items.Count - 1);
    }
    public override int ItemCount => _items.Count;
}