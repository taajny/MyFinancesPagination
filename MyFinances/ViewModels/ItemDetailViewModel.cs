﻿using MyFinances.Core.Dtos;
using MyFinances.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyFinances.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private OperationDto _operation;

        private string itemId;
        public string Id { get; set; }

        public ItemDetailViewModel()
        {
            Title = "Podgląd operacji";
        }
        public OperationDto Operation
        {
            get => _operation;
            set => SetProperty(ref _operation, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(int.Parse(value));
            }
        }

        public async void LoadItemId(int itemId)
        { 
                var response = await OperationService.GetAsync(itemId);

                if (!response.IsSuccess)
                {
                    await ShowErrorAlert(response);
                }

                Operation = response.Data;
        }
    }
}
