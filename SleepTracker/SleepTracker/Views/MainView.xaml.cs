﻿using SleepTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SleepTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}