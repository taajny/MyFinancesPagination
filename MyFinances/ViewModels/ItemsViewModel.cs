using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Models;
using MyFinances.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyFinances.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private static int _numberOfPage;
        private readonly int _itemsPerPage = 5;
        private int _numberOfOperations;

        private bool _isButtonPrevVisible;
        private bool _isButtonNextVisible;
        public ObservableCollection<OperationDto> Operations { get; }

        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DeleteItemCommand { get; }

        public Command NextPageCommand { get; }
        public Command PrevPageCommand { get; }
        public Command<OperationDto> ItemTapped { get; }

        public bool IsButtonPrevVisible
        {
            get => _isButtonPrevVisible;
            set
            {
                _isButtonPrevVisible = value;
                OnPropertyChanged(nameof(IsButtonPrevVisible));
            }
        }

        public bool IsButtonNextVisible
        {
            get => _isButtonNextVisible;
            set
            {
                _isButtonNextVisible = value;
                OnPropertyChanged(nameof(IsButtonNextVisible));
            }
        }
        public ItemsViewModel()
        {
   
            Title = "Operacje";
            _numberOfPage = 1;

            _isButtonPrevVisible = false;
            _isButtonNextVisible = true;
            
            Operations = new ObservableCollection<OperationDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OperationDto>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            DeleteItemCommand = new Command<OperationDto>(async (x) => await OnDeleteItem(x));

            NextPageCommand = new Command(OnNextPage);
            PrevPageCommand = new Command(OnPrevPage);
        }

        private async void OnPrevPage(object obj)
        {
            _numberOfPage -= 1;
            if (_numberOfPage == 1)
            {
                IsButtonPrevVisible = false;
                OnPropertyChanged(nameof(IsButtonPrevVisible));
            }

            if (_numberOfOperations > _itemsPerPage * _numberOfPage)
            {
                _isButtonNextVisible = true;
                OnPropertyChanged(nameof(IsButtonNextVisible));
            }
            else
            {
                _isButtonNextVisible = false;
                OnPropertyChanged(nameof(IsButtonNextVisible));
            }

            await ExecuteLoadItemsCommand();
        }

        private async void OnNextPage(object obj)
        {
            _numberOfPage += 1;
            _isButtonPrevVisible = true;
            OnPropertyChanged(nameof(IsButtonPrevVisible));
            
            if( _numberOfOperations > _itemsPerPage * _numberOfPage)
            {
                _isButtonNextVisible = true;
                OnPropertyChanged(nameof(IsButtonNextVisible));
            }
            else
            {
                _isButtonNextVisible = false;
                OnPropertyChanged(nameof(IsButtonNextVisible));
            }

            await ExecuteLoadItemsCommand();
        }

        private async Task OnDeleteItem(OperationDto operation)
        {
            if (operation == null)
            {
                return;
            }

            var dialog = await Shell.Current.DisplayAlert(
                "Usuwanie!",
                $"Czy na pewno chcesz usunąć operację: {operation.Name}?",
                "TaK",
                "Nie");

            if (!dialog)
            {
                return;
            }

            var response = await OperationService.DeleteAsync(operation.Id);

            if (!response.IsSuccess)
            {
                await ShowErrorAlert(response);
            }

            await ExecuteLoadItemsCommand(); 
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var response = await OperationService.GetAsync(_itemsPerPage, _numberOfPage);
                var response2 = await OperationService.GetAsync();
                
                if (!response.IsSuccess)
                {
                    await ShowErrorAlert(response);
                }

                Operations.Clear();
                
                foreach (var item in response2.Data)
                {
                    Operations.Add(item);
                }
                _numberOfOperations = Operations.Count;
                
                Operations.Clear();

                foreach (var item in response.Data)
                {
                    Operations.Add(item);
                }
            } 
            catch (Exception exception)
            {
                await Shell.Current.DisplayAlert(
                    "Wystąpił Błąd!",
                    exception.Message,
                    "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
            await ExecuteLoadItemsCommand();
        }

        async void OnItemSelected(OperationDto operation)
        {
            if (operation == null)
                return;

             // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={operation.Id}");
        }
    }
}