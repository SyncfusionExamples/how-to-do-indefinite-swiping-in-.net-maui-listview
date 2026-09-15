# How to perform indefinite swiping in .NET MAUI ListView (SfListView)?
This example demonstrates how to perform indefinite swiping in .NET MAUI ListView (SfListView).

## Sample

```xaml
<ListView:SfListView 
                    x:Name="listView"
                    ItemsSource="{Binding InboxInfo}"
                    AllowSwiping="True"
                    SwipeThreshold="50"
                    SwipeOffset="100"
                    SelectionMode="Multiple"
                    SelectionGesture="LongPress"
                    ScrollBarVisibility="Always"
                    AutoFitMode="Height">

<ListView:SfListView.StartSwipeTemplate>
    <DataTemplate>
        <Grid BackgroundColor="#DD0000">
            <Grid.GestureRecognizers>
                <TapGestureRecognizer Command="{Binding Path=BindingContext.ArchiveCommand, Source={x:Reference listView}}"
                                        CommandParameter="{Binding .}" />
            </Grid.GestureRecognizers>
        </Grid>
    </DataTemplate>
</ListView:SfListView.StartSwipeTemplate>
<ListView:SfListView.EndSwipeTemplate>
    <DataTemplate>
        <Grid BackgroundColor="#DD0000"
                x:Name="listViewGrid">
            <Grid.GestureRecognizers>
                <TapGestureRecognizer  Command="{Binding Path=BindingContext.DeleteImageCommand, Source={x:Reference listView}}"
                                        CommandParameter="{Binding .}" />
            </Grid.GestureRecognizers>
        </Grid>
    </DataTemplate>
</ListView:SfListView.EndSwipeTemplate>
<ListView:SfListView.ItemTemplate>
    <DataTemplate>
        <code>
        . . .
        . . .
        <code>
    </DataTemplate>
</ListView:SfListView.ItemTemplate>

C#:
ListView.PropertyChanged += ListView_PropertyChanged;
ListView.SwipeStarting += ListView_SwipeStarted;
ListView.SwipeEnded += ListView_SwipeEnded;
(bindable.BindingContext as ListViewSwipingViewModel).ResetSwipeView += ListViewSwipingBehavior_ResetSwipeView;
ListView.ItemTapped += ListView_ItemTapped;

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

ViewModel.cs:

this.IsDeleted = false;
ListViewInboxInfoRepository inboxinfo = new ListViewInboxInfoRepository();
archivedMessages = new ObservableCollection<ListViewInboxInfo>();
inboxInfo = inboxinfo.GetInboxInfo();
deleteImageCommand = new Command(Delete);
undoCommand = new Command(UndoAction);
favoritesImageCommand = new Command(SetFavorites);
archiveCommand = new Command(Archive);

private async void Delete(object item)
{
    PopUpText = "Deleted";
    listViewItem = (ListViewInboxInfo)item;
    listViewItemIndex = inboxInfo!.IndexOf(listViewItem);
    inboxInfo!.Remove(listViewItem);
    this.IsDeleted = true;
    await Task.Delay(3000);
    this.IsDeleted = false;
}

private async void Archive(object item)
{
    PopUpText = "Archived";
    listViewItem = (ListViewInboxInfo)item;
    listViewItemIndex = inboxInfo!.IndexOf(listViewItem);
    inboxInfo!.Remove(listViewItem);
    archivedMessages!.Add(listViewItem);
    this.IsDeleted = true;
    await Task.Delay(3000);
    this.IsDeleted = false;
}

private void UndoAction()
{
    this.IsDeleted = false;
    if (listViewItem != null)
    {
        inboxInfo!.Insert(listViewItemIndex, listViewItem);
    }
    listViewItemIndex = 0;
    listViewItem = null;
}

private void SetFavorites(object item)
{
    var listViewItem = item as ListViewInboxInfo;
    if ((bool)listViewItem!.IsFavorite)
    {
        listViewItem.IsFavorite = false;
    }
    else
    {
        listViewItem.IsFavorite = true;
    }
    OnResetSwipe(new ResetEventArgs());
}
```
