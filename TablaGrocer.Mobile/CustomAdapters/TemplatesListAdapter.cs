using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using TablaGrocerMobile.Interface;
using TableGrocer.EFCore.Models;

namespace TablaGrocerMobile.CustomAdapters;

public class TemplatesListAdapter : RecyclerView.Adapter
{
    private readonly Context context;
    private readonly List<Template> _items;
    private readonly IOnClickListener<Template> _listener;

    public class TemplatesViewHolder : RecyclerView.ViewHolder
    {
        public TextView ProductName { get; private set; }
        public ImageView EditImageView { get; private set; }
        public ImageView DeleteImageView { get; private set; }

        public TemplatesViewHolder(View itemView) : base(itemView)
        {
            ProductName = itemView.FindViewById<TextView>(Resource.Id.template_name);
        }
    }
    public TemplatesListAdapter(Context context, List<Template> items,IOnClickListener<Template> listener)
    {
        this.context = context;
        this._items = items;
        _listener = listener;
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        throw new NotImplementedException();
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.grocery_run_row_item, parent, false);
        return new TemplatesViewHolder(itemView);    }
    
    public void AddItem(Template item)
    {
        _items.Add(item);
        NotifyItemInserted(_items.Count - 1);
    }
    public override int ItemCount => _items.Count;
}