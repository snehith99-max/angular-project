﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlTelegram : result
    {
        public List<telegramlist> telegramlist { get; set; }
        public List<message_list> message_list { get; set; }
        public List<telegramsummary_list> telegramsummary_list { get; set; }

        public List<telegramrepost_list> telegramrepost_list { get; set; }
        public string image_count { get; set; }
        public string video_count { get; set; }
        public string text_count { get; set; }
        public string total_count { get; set; }


    }
    public class telegramsummary_list : result
    {
        public string id { get; set; }
        public string image_id { get; set; }

        public string upload_path { get; set; }
        public string message_content { get; set; }
        public string upload_type { get; set; }
        public string telegram_caption { get; set; }

        public string created_date { get; set; }


    }
  
    public class message_list : result
        {
            public string telegram_caption { get; set; }
        }

        public class telegramlist:result
        {
            public bool ok { get; set; }
            public Result1 result { get; set; }
           public string id { get; set; }
        public string bot_name { get; set; }
        public string telegram_gid { get; set; }

        public string title { get; set; }
        public string invite_link { get; set; }
        public string username { get; set; }

    }

        public class Result1
        {
            public long id { get; set; }
            public string title { get; set; }
            public string username { get; set; }
            public string type { get; set; }
            public string[] active_usernames { get; set; }
            public string invite_link { get; set; }
            public Permissions permissions { get; set; }
            public bool join_to_send_messages { get; set; }
        }

        public class Permissions
        {
            public bool can_send_messages { get; set; }
            public bool can_send_media_messages { get; set; }
            public bool can_send_audios { get; set; }
            public bool can_send_documents { get; set; }
            public bool can_send_photos { get; set; }
            public bool can_send_videos { get; set; }
            public bool can_send_video_notes { get; set; }
            public bool can_send_voice_notes { get; set; }
            public bool can_send_polls { get; set; }
            public bool can_send_other_messages { get; set; }
            public bool can_add_web_page_previews { get; set; }
            public bool can_change_info { get; set; }
            public bool can_invite_users { get; set; }
            public bool can_pin_messages { get; set; }
            public bool can_manage_topics { get; set; }
        }
    public class telegramrepost_list : result
    {
        public string telegram_caption { get; set; }
    }
    public class telegramconfiguration
    {
        public string bot_id { get; set; }
        public string chat_id { get; set; }

    }
}