﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using SharpTox.Core;
using ToxRt.Helpers;
using ToxRt.Model;
using ToxRt.NavigationService;

namespace ToxRt.ViewModel
{
    public class AddFriendViewModel : NavigableViewModelBase
    {
        #region Fields
        private FriendRequest _newFriendRequest;
        private ObservableCollection<FriendRequest> _listPendingRequests;
        private Tox _tox;
        #endregion
        #region Properties
        public FriendRequest NewFriendRequest
        {
            get
            {
                return _newFriendRequest;
            }

            set
            {
                if (_newFriendRequest == value)
                {
                    return;
                }

                _newFriendRequest = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<FriendRequest> ListPendingRequests
        {
            get
            {
                return _listPendingRequests;
            }

            set
            {
                if (_listPendingRequests == value)
                {
                    return;
                }

                _listPendingRequests = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region Commands
        private RelayCommand _sendRequestCommand;
        public RelayCommand SendRequestCommand
        {
            get
            {
                return _sendRequestCommand
                    ?? (_sendRequestCommand = new RelayCommand(
                    () =>
                    {
                        try
                        {
                            var friend = new Friend()
                            {
                                ToxId = NewFriendRequest.ToxId,
                                ProfileId = 1   //-->Need To Get The Current Profile Id
                            };
                            friend.FriendNumber = _tox.AddFriend(new ToxId(NewFriendRequest.ToxId), NewFriendRequest.RequestMessage);                            
                            friend.StatusMessage = "";
                            if (_tox.IsOnline(friend.FriendNumber))
                            {
                                friend.StatusMessage = getFriendStatusMessage(friend.FriendNumber);
                            }
                            else
                            {
                                var lastOnline = TimeZoneInfo.ConvertTime(_tox.GetLastOnline(friend.FriendNumber), TimeZoneInfo.Local);

                                if (lastOnline.Year == 1970)
                                {
                                    friend.StatusMessage = "Friend request sent";
                                }
                                else
                                    friend.StatusMessage = string.Format("Last seen: {0}", lastOnline.Date);
                            }
                            friend.ScreenName = getFriendName(friend.FriendNumber);
                            if (string.IsNullOrEmpty(friend.ScreenName))
                            {
                                friend.ScreenName = _tox.GetClientId(friend.FriendNumber).GetString();
                            }
                            
                            DataService.AddFriend(friend);  //loged to the local db
                            //We Must Notify The Main ViewModel Where the Friend List Is Located 
                            //In Order For It To add The New Friend Withe The Panding Sign
                             //The Less Sofisticated Solution is By Using the mvvmlight Messenger to communicate between ViewModels --> This Need To Updated Later 
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("RefreshFriends"));
                            NewFriendRequest = new FriendRequest();
                        }
                        catch (ToxAFException ex)
                        {
                            if (ex.Error != ToxAFError.SetNewNospam)
                                Debug.WriteLine("An error occurred Code [0]");

                            return;
                        }
                        catch
                        {
                            Debug.WriteLine("An error occurred, The ID you entered is not valid.");
                            return;
                        }

                    }));
            }            
        }
        private RelayCommand<int> _acceptRequestCommand;
        public RelayCommand<int> AcceptRequestCommand
        {
            get
            {
                return _acceptRequestCommand
                    ?? (_acceptRequestCommand = new RelayCommand<int>(
                    (requestId) =>
                    {
                        var request = DataService.GetFriendRequestById(requestId);
                        _tox.AddFriendNoRequest(new ToxKey(ToxKeyType.Public, request.ToxId));
                        DataService.RemoveFriendRequest(request.ToxId);


                        //Add Friend to the friend Table
                    }));
            }
        }
        private RelayCommand<int> _refuseRequestCommand;
        public RelayCommand<int> RefuseRequestCommand
        {
            get
            {
                return _refuseRequestCommand
                    ?? (_refuseRequestCommand = new RelayCommand<int>(async (requestId) =>
                    {
                        var request = DataService.GetFriendRequestById(requestId);
                        DataService.RemoveFriendRequest(request.ToxId);
                        ListPendingRequests = new ObservableCollection<FriendRequest>(await DataService.GetAllFriendRequest());



                    }));
            }
        }
        private RelayCommand _selectPandingRequestCommand;
        public RelayCommand SelectPandingRequestCommand
        {
            get
            {
                return _selectPandingRequestCommand
                    ?? (_selectPandingRequestCommand = new RelayCommand(
                    () =>
                    {

                    }));
            }
        }
        private RelayCommand _pastFromClipboardCommand;
        public RelayCommand PastFromClipboardCommand
        {
            get
            {
                return _pastFromClipboardCommand
                    ?? (_pastFromClipboardCommand = new RelayCommand( 
                    () =>
                    {                       
                        NewFriendRequest.ToxId = Clipboard.GetContent().GetTextAsync().ToString();
                    }));
            }
        }
        #endregion
        #region Ctors and Methods

        public AddFriendViewModel(INavigationService navigationService, IDataService dataService, IMessagesNavigationService innerNavigationService)
            : base(navigationService, dataService, innerNavigationService)
        {

        }
        private string getFriendStatusMessage(int friendnumber)
        {
            return _tox.GetStatusMessage(friendnumber).Replace("\n", "").Replace("\r", "");
        }
        private string getFriendName(int friendnumber)
        {
            return _tox.GetName(friendnumber).Replace("\n", "").Replace("\r", "");
        }
        private string getSelfStatusMessage()
        {
            return _tox.StatusMessage.Replace("\n", "").Replace("\r", "");
        }

        private string getSelfName()
        {
            return _tox.Name.Replace("\n", "").Replace("\r", "");
        }
        public override async void Activate(object parameter)
        {
            if (parameter is Tox)
            {
                _tox = (Tox)parameter;
            }
            NewFriendRequest = new FriendRequest();
            ListPendingRequests = new ObservableCollection<FriendRequest>(await DataService.GetAllFriendRequest());
        }
        #endregion
    }
}
