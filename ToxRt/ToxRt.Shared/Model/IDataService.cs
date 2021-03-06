﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToxRt.Model
{
    public interface IDataService
    {
        //geters
        Task<List<Message>> GetMessagesByFriendId(int friendId);
        Task<List<Friend>> GetCurrentProfileFriends(int profileId);
        Profile GetProfileByProfileId(int profileId);
        Profile GetDefaultProfile();
        void InsertNewProfile(Profile profile);
        void UpadteProfile(Profile profile);
        Task<List<DHT_Node>> LoadAllDhtNodes();
        void ReceiveFriendRequest(FriendRequest request);
        Task<List<FriendRequest>> GetAllFriendRequest();
        void RemoveFriendRequest(string friendRequestId);
        void RemoveAllFriendRequest();
        bool FriendRequestExists(string friendRequestId);   //Yu could also use ToxID as a Unique Id To Delete arequest
        FriendRequest GetFriendRequestById(int friendRequestId);
        void AddFriend(Friend friend);
        void RemoveFriend(int friendId);
        void RemoveAllFriends();
        Task<List<Friend>> GetAllFriends();

    }
}
