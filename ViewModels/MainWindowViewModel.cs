using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace SwitchTest.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            var command = ReactiveCommand.Create(() =>
            {
                Counter++;
            });

            var obs2 = this.WhenAnyValue(vm => vm.SelectedItem)
                .Where(i => i != null)
                .Select(item =>
                {
                    return Observable.Merge(
                        item.WhenAnyValue(i => i.FirstName),
                        item.WhenAnyValue(i => i.LastName)
                        );
                })
                .Switch()
                .Select((_, _) => Unit.Default)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .InvokeCommand(command);

            Items = new List<Item>
            {
                new Item { FirstName = "First1", LastName = "Last1" },
                new Item { FirstName = "First2", LastName = "Last2" },
                new Item { FirstName = "First3", LastName = "Last3" },
            };

            SelectedItem = Items.FirstOrDefault();
        }


        private int _counter = 0;
        public int Counter
        {
            get => _counter;
            set => this.RaiseAndSetIfChanged(ref _counter, value);
        }

        public List<Item> Items { get; }


        private Item? _selectedItem;
        public Item? SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}

public class Item : ReactiveObject
{
    private string _firstName = "";
    public string FirstName
    {
        get => _firstName;
        set => this.RaiseAndSetIfChanged(ref _firstName, value);
    }

    private string _lastName = "";
    public string LastName
    {
        get => _lastName;
        set => this.RaiseAndSetIfChanged(ref _lastName, value);
    }
}