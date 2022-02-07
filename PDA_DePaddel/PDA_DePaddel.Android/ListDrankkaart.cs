using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PDA_DePaddel.Droid
{
    class ListDrankkaart : BaseAdapter<Models.Token>
    {
        private List<Models.Token> mItems;
        private Context mContext;
        public ListDrankkaart(Context context, List<Models.Token> items)
        {
            mItems = items;
            mContext = context;
        }
        public override int Count
        {
            get { return mItems.Count; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Models.Token this[int position]
        {
            get { return mItems[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Listview_drankkaart, null, false);

            }
            TextView txtprijs = row.FindViewById<TextView>(Resource.Id.txtprijs);
            txtprijs.Text = mItems[position].Price.ToString();

            TextView txtcomment = row.FindViewById<TextView>(Resource.Id.txtcomment);
            txtcomment.Text = mItems[position].Comment;

            return row;

        }
    }
}