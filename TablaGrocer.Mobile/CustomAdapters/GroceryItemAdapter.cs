using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using TablaGrocerMobile.Interface;
using TablaGrocerMobile.Wrappers;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.CustomAdapters;

public class GroceryItemAdapter : RecyclerView.Adapter
{
    private readonly Context _context;
    private readonly List<GroceryItem> _items;
    private IOnClickListener<GroceryItem> _listener;
    private IOnGroceryRunCheckedChangeListener<GroceryItem> _checkedChangeListener;

    private class GroceryItemViewHolder : RecyclerView.ViewHolder
    {

        public TextView GroceryItemTextView { get; private set;}
        public TextView ItemQuantityTextView { get;private set; }
        public TextView ItemPriceTextView { get; set; }
        public CheckBox CheckBox { get; private set;}
        public ImageView EditGroceryItemImageView { get; }
        public ImageView DeleteGroceryItemImageView { get; }
        public IOnGroceryRunCheckedChangeListener<GroceryItem> checkedListener { get; set; }
        public View itemView;
        public GroceryItemViewHolder(View itemView,IOnClickListener<GroceryItem> listener,IOnGroceryRunCheckedChangeListener<GroceryItem> checkedListener) : base(itemView)
        {
            GroceryItemTextView = itemView.FindViewById<TextView>(Resource.Id.groceryItem);
            ItemQuantityTextView = itemView.FindViewById<TextView>(Resource.Id.itemQuantity);
            ItemPriceTextView = itemView.FindViewById<TextView>(Resource.Id.itemPrice);
            CheckBox = itemView.FindViewById<CheckBox>(Resource.Id.checkBox);
            EditGroceryItemImageView = itemView.FindViewById<ImageView>(Resource.Id.grocery_item_edit);
            DeleteGroceryItemImageView = itemView.FindViewById<ImageView>(Resource.Id.grocery_item_delete);
            this.itemView = itemView;

            
          
            
            EditGroceryItemImageView.Click += (sender, e) => 
                listener.OnEditClick(null, AdapterPosition);
            
            DeleteGroceryItemImageView.Click += (sender, e) => 
                listener.OnDeleteClick(null, AdapterPosition);
        }

        public void RegisterCheckedListener(IOnGroceryRunCheckedChangeListener<GroceryItem> checkedListener)
        {
            CheckBox.CheckedChange  +=  async (sender, args) =>
               await checkedListener.OnGroceryRunCheckedChanged(null, AdapterPosition, args.IsChecked);
        }

        public void UnregisterCheckedListener(IOnGroceryRunCheckedChangeListener<GroceryItem> checkedListener)
        {
            CheckBox.CheckedChange  -=  async (sender, args) =>
                await checkedListener.OnGroceryRunCheckedChanged(null, AdapterPosition, args.IsChecked);
        }
    }
    public GroceryItemAdapter(Context context, List<GroceryItem> items,IOnClickListener<GroceryItem> listener,IOnGroceryRunCheckedChangeListener<GroceryItem> checkedListener)
    {
        _context = context;
        _items = items;
        _listener = listener;
        _checkedChangeListener = checkedListener;
    }
    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        GroceryItemViewHolder? vh = holder as GroceryItemViewHolder;
        
        var item = _items[position];
        vh.itemView.Tag = new GroceryItemWrapper(item);
        vh.GroceryItemTextView.Text = item.ItemName;
        vh.ItemQuantityTextView.Text = item.Quantity;
        vh.ItemPriceTextView.Text = item.Price.ToString() ?? "0.00";
        vh.UnregisterCheckedListener(_checkedChangeListener);
        vh.CheckBox.Checked = item.IsDone ?? false ;
        vh.RegisterCheckedListener(_checkedChangeListener);
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.grocery_item_row_item, parent, false);
        return new GroceryItemViewHolder(itemView,_listener,_checkedChangeListener);
    }
    public void AddItem(GroceryItem item)
    {
        _items.Add(item);
        NotifyItemInserted(_items.Count - 1);
    }

    public override int ItemCount => _items.Count;
}