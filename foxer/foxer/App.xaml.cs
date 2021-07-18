using foxer.Core.Interfaces;
using foxer.Pages;
using foxer.Render;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace foxer
{
    public partial class App : Application, INavigator
    {
        private readonly Dictionary<Type, Lazy<Page>> _pagesCache = new Dictionary<Type, Lazy<Page>>();
        private readonly Stack<Page> _navigationStack = new Stack<Page>();

        public App(INativeViewFactory nativeViewFactory)
        {
            InitializeComponent();

            _pagesCache.Add(typeof(MainPage), new Lazy<Page>(() => new MainPage(this)));
            _pagesCache.Add(typeof(PageGame), new Lazy<Page>(() => new PageGame(this, nativeViewFactory)));

            ShowMainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void ShowMainPage()
        {
            ShowPage<MainPage>();
        }

        public void ShowGamePage()
        {
            ShowPage<PageGame>();
        }

        public bool CanGoBack()
        {
            return _navigationStack.Count > 0;
        }

        public void Back()
        {
            MainPage = _navigationStack.Pop();
        }

        private void ShowPage<T>() where T : Page
        {
            if(!_pagesCache.TryGetValue(typeof(T), out var page))
            {
                throw new Exception("Попытка перехода на неизвестную страницу");
            }

            MainPage = page.Value;
            if(_navigationStack.Count != 0 
                &&  _navigationStack.Peek() != MainPage)
            {
                _navigationStack.Push(MainPage);
            }
        }
    }
}
