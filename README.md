# how-to-do-indefinite-swiping-in-.net-maui-listview

This example demonstrates how to perform indefinite swiping in .Net Maui ListView
## XAML 

<ListView:SfListView Grid.Row="1"
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
                <Label Text="&#xe71C;"
                        FontFamily='{OnPlatform Android=ListViewFontIcons.ttf#,UWP=ListViewFontIcons.ttf#ListViewFontIcons,MacCatalyst=ListViewFontIcons,iOS=ListViewFontIcons}'
                        TextColor="#FFFFFF"
                        HorizontalOptions="Center"
                        FontSize="22"
                        FontAttributes="Bold"
                        VerticalOptions="Center">
                </Label>

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
                <Label Text="&#xe716;"
                        FontFamily='{OnPlatform Android=ListViewFontIcons.ttf#,UWP=ListViewFontIcons.ttf#ListViewFontIcons,MacCatalyst=ListViewFontIcons,iOS=ListViewFontIcons}'
                        TextColor="#FFFFFF"
                        HorizontalOptions="Center"
                        FontSize="26"
                        VerticalOptions="Center">
                </Label>
            </Grid>
        </DataTemplate>
    </ListView:SfListView.EndSwipeTemplate>
    <ListView:SfListView.ItemTemplate>
        ---
    </ListView:SfListView.ItemTemplate>
</ListView:SfListView>

## C#
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
## Requirements to run the demo

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer).

## Troubleshooting

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.