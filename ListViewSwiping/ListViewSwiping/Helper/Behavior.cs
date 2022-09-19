using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListViewSwiping.ListViewSwipingViewModel;

namespace ListViewSwiping
{
    public class ListViewSwipingBehavior : Behavior<ContentPage>
    {
        private Syncfusion.Maui.ListView.SfListView ListView;
        private ListViewSwipingViewModel ViewModel;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            ViewModel = new ListViewSwipingViewModel();
            bindable.BindingContext = ViewModel;
            ListView = bindable.FindByName<Syncfusion.Maui.ListView.SfListView>("listView");
            ListView.PropertyChanged += ListView_PropertyChanged;
            ListView.SwipeStarting += ListView_SwipeStarted;
            ListView.SwipeEnded += ListView_SwipeEnded;
            (bindable.BindingContext as ListViewSwipingViewModel).ResetSwipeView += ListViewSwipingBehavior_ResetSwipeView;
            ListView.ItemTapped += ListView_ItemTapped;
            base.OnAttachedTo(bindable);
        }

        private void ListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
        {
            (e.DataItem as ListViewInboxInfo).IsOpened = true;
        }

        private void ListViewSwipingBehavior_ResetSwipeView(object sender, ResetEventArgs e)
        {
            ListView!.ResetSwipeItem();
        }

        private void ListView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" && ListView.Orientation == ItemsLayoutOrientation.Vertical && ListView.SwipeOffset != ListView.Width)
                ListView.SwipeOffset = ListView.Width;
            else if (e.PropertyName == "Height" && ListView.Orientation == ItemsLayoutOrientation.Horizontal && ListView.SwipeOffset != ListView.Height)
                ListView.SwipeOffset = ListView.Height;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            (bindable.BindingContext as ListViewSwipingViewModel).ResetSwipeView -= ListViewSwipingBehavior_ResetSwipeView;
            ListView.PropertyChanged -= ListView_PropertyChanged;
            ListView.SwipeStarting -= ListView_SwipeStarted;
            ListView.SwipeEnded -= ListView_SwipeEnded;
            ListView.ItemTapped -= ListView_ItemTapped;
            ListView = null;
            ViewModel = null;
            base.OnDetachingFrom(bindable);
        }
        private void ListView_SwipeEnded(object sender, Syncfusion.Maui.ListView.SwipeEndedEventArgs e)
        {
            ViewModel.listViewItemIndex = e.Index;

            if (e.Direction == SwipeDirection.Right)
                e.Offset = 80;
            else if (e.Direction == SwipeDirection.Left)
                e.Offset = 120;
        }

        private void ListView_SwipeStarted(object sender, Syncfusion.Maui.ListView.SwipeStartingEventArgs e)
        {
            ViewModel.listViewItemIndex = -1;
        }
    }
}
