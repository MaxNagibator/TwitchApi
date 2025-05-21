namespace TwitchApi.Twitch
{
    [Flags]
    public enum TwitchScope : long
    {
        [TwitchScope("openid")]
        Openid = 1L << 0,

        [TwitchScope("channel:bot")]
        ChannelBot = 1L << 1,

        [TwitchScope("channel:manage:broadcast")]
        ChannelManageBroadcast = 1L << 2,

        [TwitchScope("channel:manage:extensions")]
        ChannelManageExtensions = 1L << 3,

        [TwitchScope("channel:read:goals")]
        ChannelReadGoals = 1L << 4,

        [TwitchScope("channel:read:guest_star")]
        ChannelReadGuestStar = 1L << 5,

        [TwitchScope("channel:manage:guest_star")]
        ChannelManageGuestStar = 1L << 6,

        [TwitchScope("channel:read:hype_train")]
        ChannelReadHypeTrain = 1L << 7,

        [TwitchScope("channel:manage:moderators")]
        ChannelManageModerators = 1L << 8,

        [TwitchScope("channel:read:polls")]
        ChannelReadPolls = 1L << 9,

        [TwitchScope("channel:manage:polls")]
        ChannelManagePolls = 1L << 10,

        [TwitchScope("channel:read:predictions")]
        ChannelReadPredictions = 1L << 11,

        [TwitchScope("channel:manage:predictions")]
        ChannelManagePredictions = 1L << 12,

        [TwitchScope("channel:manage:raids")]
        ChannelManageRaids = 1L << 13,

        [TwitchScope("channel:read:redemptions")]
        ChannelReadRedemptions = 1L << 14,

        [TwitchScope("channel:manage:redemptions")]
        ChannelManageRedemptions = 1L << 15,

        [TwitchScope("channel:manage:schedule")]
        ChannelManageSchedule = 1L << 16,

        [TwitchScope("channel:read:stream_key")]
        ChannelReadStreamKey = 1L << 17,

        [TwitchScope("channel:read:subscriptions")]
        ChannelReadSubscriptions = 1L << 18,

        [TwitchScope("channel:manage:videos")]
        ChannelManageVideos = 1L << 19,

        [TwitchScope("channel:read:vips")]
        ChannelReadVips = 1L << 20,

        [TwitchScope("channel:manage:vips")]
        ChannelManageVips = 1L << 21,

        [TwitchScope("channel:moderate")]
        ChannelModerate = 1L << 22,

        [TwitchScope("moderation:read")]
        ModerationRead = 1L << 23,

        [TwitchScope("moderator:manage:announcements")]
        ModeratorManageAnnouncements = 1L << 24,

        [TwitchScope("moderator:manage:automod")]
        ModeratorManageAutomod = 1L << 25,

        [TwitchScope("moderator:read:automod_settings")]
        ModeratorReadAutomodSettings = 1L << 26,

        [TwitchScope("moderator:manage:automod_settings")]
        ModeratorManageAutomodSettings = 1L << 27,

        [TwitchScope("moderator:read:banned_users")]
        ModeratorReadBannedUsers = 1L << 28,

        [TwitchScope("moderator:manage:banned_users")]
        ModeratorManageBannedUsers = 1L << 29,

        [TwitchScope("moderator:read:blocked_terms")]
        ModeratorReadBlockedTerms = 1L << 30,

        [TwitchScope("moderator:read:chat_messages")]
        ModeratorReadChatMessages = 1L << 31,

        [TwitchScope("moderator:manage:blocked_terms")]
        ModeratorManageBlockedTerms = 1L << 32,

        [TwitchScope("moderator:manage:chat_messages")]
        ModeratorManageChatMessages = 1L << 33,

        [TwitchScope("moderator:read:chat_settings")]
        ModeratorReadChatSettings = 1L << 34,

        [TwitchScope("moderator:manage:chat_settings")]
        ModeratorManageChatSettings = 1L << 35,

        [TwitchScope("moderator:read:chatters")]
        ModeratorReadChatters = 1L << 36,

        [TwitchScope("moderator:read:followers")]
        ModeratorReadFollowers = 1L << 37,

        [TwitchScope("moderator:read:guest_star")]
        ModeratorReadGuestStar = 1L << 38,

        [TwitchScope("moderator:manage:guest_star")]
        ModeratorManageGuestStar = 1L << 39,

        [TwitchScope("moderator:read:moderators")]
        ModeratorReadModerators = 1L << 40,

        [TwitchScope("moderator:read:shield_mode")]
        ModeratorReadShieldMode = 1L << 41,

        [TwitchScope("moderator:manage:shield_mode")]
        ModeratorManageShieldMode = 1L << 42,

        [TwitchScope("moderator:read:shoutouts")]
        ModeratorReadShoutouts = 1L << 43,

        [TwitchScope("moderator:manage:shoutouts")]
        ModeratorManageShoutouts = 1L << 44,

        [TwitchScope("moderator:read:suspicious_users")]
        ModeratorReadSuspiciousUsers = 1L << 45,

        [TwitchScope("moderator:read:unban_requests")]
        ModeratorReadUnbanRequests = 1L << 46,

        [TwitchScope("moderator:manage:unban_requests")]
        ModeratorManageUnbanRequests = 1L << 47,

        [TwitchScope("moderator:read:vips")]
        ModeratorReadVips = 1L << 48,

        [TwitchScope("moderator:read:warnings")]
        ModeratorReadWarnings = 1L << 49,

        [TwitchScope("moderator:manage:warnings")]
        ModeratorManageWarnings = 1L << 50,

        [TwitchScope("user:edit:broadcast")]
        UserEditBroadcast = 1L << 51,

        [TwitchScope("user:read:blocked_users")]
        UserReadBlockedUsers = 1L << 52,

        [TwitchScope("user:manage:blocked_users")]
        UserManageBlockedUsers = 1L << 53,

        [TwitchScope("user:read:broadcast")]
        UserReadBroadcast = 1L << 54,

        [TwitchScope("user:read:chat")]
        UserReadChat = 1L << 55,

        [TwitchScope("user:manage:chat_color")]
        UserManageChatColor = 1L << 56,

        [TwitchScope("user:read:email")]
        UserReadEmail = 1L << 57,

        [TwitchScope("user:read:follows")]
        UserReadFollows = 1L << 58,

        [TwitchScope("user:read:subscriptions")]
        UserReadSubscriptions = 1L << 59,

        [TwitchScope("user:read:whispers")]
        UserReadWhispers = 1L << 60,

        [TwitchScope("user:manage:whispers")]
        UserManageWhispers = 1L << 61,

        [TwitchScope("user:write:chat")]
        UserWriteChat = 1L << 62,
    }
}
