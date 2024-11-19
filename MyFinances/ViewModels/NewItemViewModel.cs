﻿using MyFinances.Core.Dtos;
using MyFinances.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyFinances.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private decimal _value;
        private DateTime _createdDate;
        private LookupItem _selectedCategory;
        private IEnumerable<LookupItem> _categories = new List<LookupItem>
        {
             new LookupItem { Id = 1, Name = "Ogólna"}
        };

        public NewItemViewModel()
        {
            CreatedDate = DateTime.Now;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Name)
                && !String.IsNullOrWhiteSpace(Description)
                && SelectedCategory != null;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public LookupItem SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public IEnumerable<LookupItem> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
         
        private async void OnSave()
        {
            var operation = new OperationDto()
            {
                Name = Name,
                Description = Description,
                Value = Value,
                Date = DateTime.Now,
                CategoryId = SelectedCategory.Id,
            };
            
            var response = await OperationService.AddAsync(operation);

            if (!response.IsSuccess)
            {
                await ShowErrorAlert(response);
            }
            

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
