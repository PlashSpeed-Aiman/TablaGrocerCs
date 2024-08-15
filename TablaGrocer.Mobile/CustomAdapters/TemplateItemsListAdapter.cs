using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace TablaGrocerMobile.CustomAdapters;

public class TemplateItemsListAdapter : ListAdapter
{
    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        throw new NotImplementedException();
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        throw new NotImplementedException();
    }

    public override int ItemCount { get; }

    public TemplateItemsListAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }

    public TemplateItemsListAdapter(AsyncDifferConfig config) : base(config)
    {
    }

    public TemplateItemsListAdapter(DiffUtil.ItemCallback diffCallback) : base(diffCallback)
    {
    }
}