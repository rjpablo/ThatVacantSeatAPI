﻿namespace Dribbly.Service.Enums
{
    public enum UserActivityTypeEnum
    {
        #region Account-Related (1-40)

        CreateAccount = 1,
        DeactivateAccount = 2,
        ReactivateAccount = 3,
        DeleteAccount = 4,
        MakeAccountPublic = 5,
        MakeAccountPrivate = 6,
        SetPrimaryPhoto = 7,
        AddAccountPhoto = 8,
        DeleteAccountPhoto = 9,
        SetAccountPrimaryPhoto = 10,
        AddAccountVideo = 11,
        DeleteAccountVideo = 12,

        #endregion

        #region Courts-Related (41-60)

        AddCourt = 41,
        UpdatCourt = 42,
        DeleteCourt = 43,
        FollowCourt = 44,
        UnfollowCourt = 45,
        AddCourtPhotos = 46,
        DeleteCourtPhotos = 47,
        AddCourtVideos = 48,
        DeleteCourtVideos = 49,
        SetCourtPrimaryPhoto = 50,

        #endregion

        #region Game-Related (61-80)

        AddGame = 61,
        UpdateGame = 62,

        #endregion

        #region Posts-Related (81-100)

        AddPost = 81,
        UpdatePost = 82,
        DeletePost = 83,

        #endregion

        #region Player-Related (101-120)

        FollowPlayer = 101,
        UnfollowPlayer = 102,
        RatePlayer = 103,

        #endregion

        #region Contacts
        RequestVerificationCode = 121,
        VerifyContact = 122,
        #endregion
    }
}