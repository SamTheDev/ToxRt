﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ToxRt.Model
{
    public class Profile : Friend
    {
        #region Fields
        private int _loggin = 0;
        private int _audioNotifications = 0;
        private int _closeToTray = 0;
        private ObservableCollection<Friend> _friends;
        private String _profileTheme = "Mytheme"; //tmp
        private String _profileLanguage = "English"; //tmp
        private int _isDefault = 0;
        private String _profileName;
        #endregion
        #region Properties              
        public String ProfileName
        {
            get
            {
                return _profileName;
            }

            set
            {
                if (_profileName == value)
                {
                    return;
                }

                _profileName = value;
                OnPropertyChanged();
            }
        }
        public int Loggin
        {
            get
            {
                return _loggin;
            }

            set
            {
                if (_loggin == value)
                {
                    return;
                }

                _loggin = value;
                OnPropertyChanged();
            }
        }
        public int AudioNotifications
        {
            get
            {
                return _audioNotifications;
            }

            set
            {
                if (_audioNotifications == value)
                {
                    return;
                }

                _audioNotifications = value;
                OnPropertyChanged();
            }
        }
        public int CloseToTray
        {
            get
            {
                return _closeToTray;
            }

            set
            {
                if (_closeToTray == value)
                {
                    return;
                }

                _closeToTray = value;
                OnPropertyChanged();
            }
        }
        public String ProfileTheme
        {
            get
            {
                return _profileTheme;
            }

            set
            {
                if (_profileTheme == value)
                {
                    return;
                }

                _profileTheme = value;
                OnPropertyChanged();
            }
        }
        public String ProfileLanguage
        {
            get
            {
                return _profileLanguage;
            }

            set
            {
                if (_profileLanguage == value)
                {
                    return;
                }

                _profileLanguage = value;
                OnPropertyChanged();
            }
        }        
        public ObservableCollection<Friend> Friends
        {
            get
            {
                return _friends;
            }

            set
            {
                if (_friends == value)
                {
                    return;
                }

                _friends = value;
                OnPropertyChanged();
            }
        }        
        public int IsDefault
        {
            get
            {
                return _isDefault;
            }

            set
            {
                if (_isDefault == value)
                {
                    return;
                }

                _isDefault = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
