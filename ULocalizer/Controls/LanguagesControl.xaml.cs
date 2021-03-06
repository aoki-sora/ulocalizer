﻿using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ULocalizer.Binding;
using ULocalizer.Classes;
using ULocalizer.ExtensionMethods;

namespace ULocalizer.Controls
{
    /// <summary>
    ///     Interaction logic for LanguagesControl.xaml
    /// </summary>
    public partial class LanguagesControl : INotifyPropertyChanged
    {
        public static readonly DependencyProperty DestinationSourceProperty = DependencyProperty.Register("DestinationSource", typeof (CObservableList<CCulture>), typeof (LanguagesControl), new PropertyMetadata(new CObservableList<CCulture>()));
        public static readonly DependencyProperty WorkingProjectProperty = DependencyProperty.Register("WorkingProject", typeof (CProject), typeof (LanguagesControl), new PropertyMetadata(Projects.NewProject));

        public LanguagesControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public CObservableList<CCulture> DestinationSource
        {
            get { return (CObservableList<CCulture>) GetValue(DestinationSourceProperty); }
            set
            {
                SetValue(DestinationSourceProperty, value);
                NotifyPropertyChanged();
            }
        }

        public CProject WorkingProject
        {
            get { return (CProject) GetValue(WorkingProjectProperty); }
            set
            {
                SetValue(WorkingProjectProperty, value);
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void AddLangBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((SourceLanguagePicker.SelectedLanguage != null) && (!WorkingProject.Languages.Contains(SourceLanguagePicker.SelectedLanguage)))
            {
                WorkingProject.Languages.Add(SourceLanguagePicker.SelectedLanguage);
                WorkingProject.Languages = WorkingProject.Languages.OrderBy(culture => culture.ISO).ToObservableList();
            }
        }

        private void DelLangBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationLanguagePicker.SelectedLanguage != null && DestinationLanguagePicker.Items.Count > 1 && DestinationLanguagePicker.SelectedLanguage.ISO != WorkingProject.SourceCulture.ISO)
            {
                WorkingProject.Languages.Remove(DestinationLanguagePicker.SelectedLanguage);
            }
        }

        private void AddAllLangBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ci in SourceLanguagePicker.Items)
            {
                if (!WorkingProject.Languages.Contains(ci))
                {
                    WorkingProject.Languages.Add(ci);
                }
            }
            WorkingProject.Languages = WorkingProject.Languages.OrderBy(culture => culture.ISO).ToObservableList();
        }

        private void RemoveAllLangBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ci in DestinationLanguagePicker.Items.ToList())
            {
                if ((ci.ISO != WorkingProject.SourceCulture.ISO) && (WorkingProject.Languages.Contains(ci)))
                {
                    WorkingProject.Languages.Remove(ci);
                }
            }
        }
    }
}