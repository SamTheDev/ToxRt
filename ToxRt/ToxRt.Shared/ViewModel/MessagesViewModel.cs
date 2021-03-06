﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using ToxRt.Helpers;
using ToxRt.Model;
using ToxRt.NavigationService;

namespace ToxRt.ViewModel
{
    public class MessagesViewModel : NavigableViewModelBase
    {
        #region Fields        

        #endregion
        #region Properties
        private ObservableCollection<Message> _listOnScreenMessages;
        private Friend _feriend;
        private String _textMessage;
        public ObservableCollection<Message> ListOnscreenMessages
        {
            get
            {
                return _listOnScreenMessages;
            }

            set
            {
                if (_listOnScreenMessages == value)
                {
                    return;
                }

                _listOnScreenMessages = value;
                RaisePropertyChanged();
            }
        }
        public Friend Friend
        {
            get
            {
                return _feriend;
            }

            set
            {
                if (_feriend == value)
                {
                    return;
                }

                _feriend = value;
                RaisePropertyChanged();
            }
        }
        public String TextMessage
        {
            get
            {
                return _textMessage;
            }

            set
            {                
                _textMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region Commands
        private RelayCommand _sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand
                    ?? (_sendCommand = new RelayCommand(
                    () =>
                    {
                        ListOnscreenMessages.Add(new Message()
                        {
                            MessageText = TextMessage,
                            Sender = Friend
                        });
                        TextMessage = "";
                    }
                ));
            }
        }
        #endregion
        #region Ctors and Methods

        public MessagesViewModel(INavigationService navigationService, IDataService dataService, IMessagesNavigationService innerNavigationService)
            : base(navigationService,dataService, innerNavigationService)
        {                       
            //For test purpus only
            ListOnscreenMessages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    MessageText = "Message num 1",
                    Sender =  new Friend(){
                    PicSource = "../Images/user.png",
                    RealName = "Joseph Walsh",
                    ScreenName = "Josheph"
                   }
                },
                 new Message()
                {
                    MessageText = "Message num 2",
                    Sender =  new Friend()
                {
                    PicSource = "../Images/user.png",
                    RealName = "Joseph Walsh",
                    ScreenName = "Josheph"
                }
                }
            };

        }

        #endregion

        public override void Activate(object parameter)
        {
            Friend = parameter as Friend;            
        }
        
    }
}
