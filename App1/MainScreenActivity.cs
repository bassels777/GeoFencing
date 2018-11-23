using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoFencing
{
    [Activity(Label = "MainScreenActivity", MainLauncher = false)]
    public class MainScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.MainScreenLayout);
            var policiesView = FindViewById<ListView>(Resource.Id.policiesListView);
            var policiesList = new List<PolicyItem>
            {
                new PolicyItem{Name = "Boiler", Description = "When I'm 1 kilometer away, turn boiler on"},
                new PolicyItem{Name = "Lights", Description = "When I'm 100 meters away, turn lights on"}
            };

            var adapter = new ItemsAdapter(this, policiesList);
            policiesView.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.mainmenu, menu);
            return true;
        }

        private class PolicyItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private class ItemsAdapter : ArrayAdapter<PolicyItem>
        {
            public ItemsAdapter(Context context, IList<PolicyItem> objects)
                : base(context, 0,objects)
            {

            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = GetItem(position);
                if (convertView == null)
                {
                    convertView = LayoutInflater.From(Context).Inflate(Resource.Layout.PoliciesLayout, parent, false);
                }

                var policyNameView = convertView.FindViewById<TextView>(Resource.Id.policyName);
                var policyDescriptionView = convertView.FindViewById<TextView>(Resource.Id.policyDescription);
                policyNameView.Text = item.Name;
                policyDescriptionView.Text = item.Description;
                return convertView;

            }
        }
    }
}