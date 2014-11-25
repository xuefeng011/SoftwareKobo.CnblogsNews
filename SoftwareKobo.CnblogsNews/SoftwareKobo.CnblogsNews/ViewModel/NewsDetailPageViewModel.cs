﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using SoftwareKobo.CnblogsNews.Model;
using SoftwareKobo.CnblogsNews.Service;
using System;
using Windows.UI.Xaml;
using News = SoftwareKobo.CnblogsAPI.Model.News;
using NewsService = SoftwareKobo.CnblogsAPI.Service.NewsService;

namespace SoftwareKobo.CnblogsNews.ViewModel
{
    public class NewsDetailPageViewModel : ViewModelBase
    {
        private bool _isLoading;
        private UIElement _newsDetail;
        private string _title;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public UIElement NewsDetail
        {
            get
            {
                return _newsDetail;
            }
            set
            {
                _newsDetail = value;
                RaisePropertyChanged(() => NewsDetail);
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public async void Render(News news)
        {
            if (NetworkService.IsNetworkAvailable() == false)
            {
                await new DialogService().ShowMessageBox("请检查网络连接。", "错误");
                return;
            }
            this.IsLoading = true;
            Exception exception = null;
            try
            {
                var newsDetail = await NewsService.DetailAsync(news.Id);
                var html = newsDetail.Content;
                var document = NewsDetailService.ParseHtmlToDocument(html);

                this.Title = newsDetail.Title;
                this.NewsDetail = NewsDetailService.RenderNewsDetail(document);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                await new DialogService().ShowError(exception, "错误", "关闭", null);
            }
            this.IsLoading = false;
        }
    }
}