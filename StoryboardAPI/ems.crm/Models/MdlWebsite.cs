﻿using System;
using System.Collections.Generic;

namespace ems.crm.Models
{
    public class MdlWebsite :results
    {
        public string found_chats { get; set; }
        public mdlChatSummary[] chats_summary { get; set; }
        public List<listof_chat> listof_chat { get; set; }
        public List<user_details> user_details { get; set; }
        public listofcustomer[] customers { get; set; }
        public string chat_id { get; set; }
        public List<chat_analytics> chat_analytics { get; set; }
        public List<chat_analytics1> chat_analytics1 { get; set; }
        public List<weekwise_report> weekwise_report { get; set; }
        public string thread_id { get; set; }

    }
   
    public class mdlChatSummary
    {
        public string id { get; set; }
        public Last_Event_Per_Type last_event_per_type { get; set; }
        public User[] users { get; set; }
        public Last_Thread_Summary last_thread_summary { get; set; }
        public Properties4 properties { get; set; }
        public Access1 access { get; set; }
        public bool is_followed { get; set; }
    }

    public class Last_Event_Per_Type
    {
        public Files file { get; set; }
        public Filled_Form filled_form { get; set; }
        public Message message { get; set; }
        public System_Message system_message { get; set; }
    }

    public class Files
    {
        public string thread_id { get; set; }
        public DateTime thread_created_at { get; set; }
        public Event _event { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string visibility { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public string author_id { get; set; }
        public string custom_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string thumbnail2x_url { get; set; }
        public string content_type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
    }

    public class Properties
    {
        public Source source { get; set; }
    }

    public class Source
    {
        public string client_id { get; set; }
    }

    public class Filled_Form
    {
        public string thread_id { get; set; }
        public DateTime thread_created_at { get; set; }
        public FormEvent @event { get; set; }
    }

    public class FormEvent
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string visibility { get; set; }
        public string type { get; set; }
        public Properties1 properties { get; set; }
        public string author_id { get; set; }
        public string custom_id { get; set; }
        public string form_id { get; set; }
        public string form_type { get; set; }
        public Field[] fields { get; set; }
    }

    public class Properties1
    {
        public Lc2 lc2 { get; set; }
        public Source1 source { get; set; }
    }

    public class Lc2
    {
        public string form_type { get; set; }
    }

    public class Source1
    {
        public string client_id { get; set; }
    }

    public class Field
    {
        public string id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string answer { get; set; }
    }

    public class Message
    {
        public string thread_id { get; set; }
        public DateTime thread_created_at { get; set; }
        public Event2 _event { get; set; }
    }

    public class Event2
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string visibility { get; set; }
        public string type { get; set; }
        public Properties2 properties { get; set; }
        public string text { get; set; }
        public string author_id { get; set; }
        public string custom_id { get; set; }
    }

    public class Properties2
    {
        public Source2 source { get; set; }
    }

    public class Source2
    {
        public string client_id { get; set; }
    }

    public class System_Message
    {
        public string thread_id { get; set; }
        public DateTime thread_created_at { get; set; }
        public Event3 _event { get; set; }
    }

    public class Event3
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string visibility { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string system_message_type { get; set; }
        public Text_Vars text_vars { get; set; }
    }

    public class Text_Vars
    {
        public string duration { get; set; }
    }

    public class Last_Thread_Summary
    {
        public string id { get; set; }
        public string[] user_ids { get; set; }
        public Properties3 properties { get; set; }
        public bool active { get; set; }
        public Access access { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Properties3
    {
        public Routing routing { get; set; }
        public Source3 source { get; set; }
    }

    public class Routing
    {
        public bool continuous { get; set; }
        public string group_status_at_start { get; set; }
        public bool idle { get; set; }
        public bool pinned { get; set; }
        public string start_url { get; set; }
        public bool unassigned { get; set; }
    }

    public class Source3
    {
        public string client_id { get; set; }
        public string customer_client_id { get; set; }
    }

    public class Access
    {
        public int[] group_ids { get; set; }
    }

    public class Properties4
    {
        public Routing1 routing { get; set; }
        public Source4 source { get; set; }
    }

    public class Routing1
    {
        public bool continuous { get; set; }
        public bool email_follow_up { get; set; }
        public bool pinned { get; set; }
        public bool was_pinned { get; set; }
    }

    public class Source4
    {
        public string client_id { get; set; }
        public string customer_client_id { get; set; }
    }

    public class Access1
    {
        public int[] group_ids { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime events_seen_up_to { get; set; }
        public string type { get; set; }
        public bool present { get; set; }
        public DateTime created_at { get; set; }
        public Last_Visit last_visit { get; set; }
        public Statistics statistics { get; set; }
        public DateTime agent_last_event_created_at { get; set; }
        public DateTime customer_last_event_created_at { get; set; }
        public bool email_verified { get; set; }
        public string avatar { get; set; }
        public string visibility { get; set; }
    }

    public class Last_Visit
    {
        public int id { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string ip { get; set; }
        public string user_agent { get; set; }
        public Geolocation geolocation { get; set; }
        public Last_Pages[] last_pages { get; set; }
    }

    public class Geolocation
    {
        public string country { get; set; }
        public string country_code { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string timezone { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Last_Pages
    {
        public DateTime opened_at { get; set; }
        public string url { get; set; }
        public string title { get; set; }
    }

    public class Statistics
    {
        public int chats_count { get; set; }
        public int threads_count { get; set; }
        public int visits_count { get; set; }
        public int page_views_count { get; set; }
        public int greetings_shown_count { get; set; }
        public int greetings_accepted_count { get; set; }
    }
    public class listof_chat: results
    {
        public string inlinechat_gid { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_mail { get; set; }
        public string first_letter { get; set; }
        public string chatted_at { get; set; }
        public string chat_id { get; set; }
        public string thread_id { get; set; }
        public string chatmessage_gid { get; set; }
        public string chatted_time { get; set; }
      
      
    }
    public class user_details
    {
        public string inlinechat_gid { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_mail { get; set; }
        public string first_letter { get; set; }
        public string chatted_at { get; set; }
        public string location { get; set; }
        public string user_type { get; set; }
        public string page_openedat { get; set; }
        public string created_date { get; set; }
        public string page_views_count { get; set; }
        public string visits_count { get; set; }
        public string threads_count { get; set; }
        public string page_url { get; set; }
        public string title { get; set; }
        public string ip_address { get; set; }
        public string user_agent { get; set; }

    }


    public class listofcustomer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
        public Last_Visit last_visit { get; set; }
        public Statistics statistics { get; set; }
        public DateTime agent_last_event_created_at { get; set; }
        public bool email_verified { get; set; }
    }



    ////////////////////////////////////////////////////


    public class mdlinlinechat:results
    {
        public string inlinechat_gid { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_mail { get; set; }
        public string first_letter { get; set; }
        public List<GetViewchatsummary> GetViewchatsummary { get; set; }
        public string id { get; set; }
        public User[] users { get; set; }
        public Thread1 thread { get; set; }
        public Properties5 properties { get; set; }
        public Access1 access { get; set; }
        public bool is_followed { get; set; }
        public List<listof_threads> listof_threads { get; set; }


    }

    public class Thread1
    {
        public string id { get; set; }
        public bool active { get; set; }
        public string[] user_ids { get; set; }
        public Properties properties { get; set; }
        public Access2 access { get; set; }
        public string next_thread_id { get; set; }
        public DateTime created_at { get; set; }
        public Event1[] events { get; set; }
    }

    public class Properties5
    {
        public Routing2 routing { get; set; }
        public Source5 source { get; set; }
    }

    public class Routing2
    {
        public bool continuous { get; set; }
        public string group_status_at_start { get; set; }
        public bool idle { get; set; }
        public bool pinned { get; set; }
        public string referrer { get; set; }
        public string start_url { get; set; }
        public bool unassigned { get; set; }
    }

    public class Source5
    {
        public string client_id { get; set; }
        public string customer_client_id { get; set; }
    }

    public class Access2
    {
        public int[] group_ids { get; set; }
    }

    public class Event1
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string visibility { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string alternative_text { get; set; }

        public Properties6 properties { get; set; }
        public string author_id { get; set; }
        public string custom_id { get; set; }
        public string form_id { get; set; }
        public string form_type { get; set; }
        public Field1[] fields { get; set; }
        public string text { get; set; }
        public string system_message_type { get; set; }
        public Text_Var text_vars { get; set; }
    }

    public class Properties6
    {
        public Lc3 lc2 { get; set; }
        public Source1 source { get; set; }
    }

    public class Lc3
    {
        public string form_type { get; set; }
        public bool welcome_message { get; set; }
    }



    public class Text_Var
    {
        public string customer { get; set; }
    }

    public class Field1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string answer { get; set; }
    }
    //public class mdlinlinechat
    //{
    //    public string inlinechat_gid { get; set; }
    //    public string user_id { get; set; }
    //    public string user_name { get; set; }
    //    public string user_mail { get; set; }
    //    public string first_letter { get; set; }

    //    public List<GetViewchatsummary> GetViewchatsummary { get; set; }

    //}
    public class GetViewchatsummary
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_mail { get; set; }
        public string chat_id { get; set; }
        public string thread_id { get; set; }
        public string next_thread_id { get; set; }
        public string created_date { get; set; }
        public string event_id { get; set; }
        public string event_type { get; set; }
        public string chatted_at { get; set; }
        public string message { get; set; }
        public string author_id { get; set; }
        public string agent_id { get; set; }
        public string agent_name { get; set; }
        public string agent_mail { get; set; }
        public string ext { get; set; }
        public string image_name { get; set; }
        public string leadbank_gid { get; set; }


    }

    public class Rootobject2:results
    {
        public string next_page_id { get; set; }
        public Thread[] threads { get; set; }
        public int found_threads { get; set; }
        public List<listof_threads> listof_threads { get; set; }

    }
    public class listof_threads
    {
        public string chat_id { get; set; }
        public string thread_id { get; set; }

    }

    public class Thread
    {
        public string id { get; set; }
        public bool active { get; set; }
        public string[] user_ids { get; set; }
        public Properties properties { get; set; }
        public Access access { get; set; }
        public string previous_thread_id { get; set; }
        public DateTime created_at { get; set; }
        public Event[] events { get; set; }
        public string next_thread_id { get; set; }

        internal static void Sleep(int v)
        {
            throw new NotImplementedException();
        }
    }

    public class messagesend:results
    {

        public string inlinechat_gid { get; set; }
        public string chat_id { get; set; }
        public string thread_id { get; set; }
        public string sendtext { get; set; }
        public string image_url { get; set; }


    }
    public class addleadvalues
    {

        public string customertype_edit { get; set; }
        public string phone_edit { get; set; }
        public string displayName_edit { get; set; }
        public string inline_id { get; set; }



    }
 
    public class inlinechatconfiguration
    {
        public string id { get; set; }
        public string access_token { get; set; }

    }
    public class chat_analytics : result
    {
        public string total_users { get; set; }
        public string country { get; set; }
        public string city { get; set; }
    }
    public class chat_analytics1 : result
    {
        public string user_name { get; set; }
        public string user_mail { get; set; }
        public string created_date { get; set; }
        public string total_user { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string visits_count { get; set; }
        public string page_views_count { get; set; }
        public string page_title { get; set; }

    }

    public class weekwise_report : result
    {

        public string week_date { get; set; }
        public string week_users { get; set; }
        public string month_date { get; set; }
        public string month_users { get; set; }
        public string year_users { get; set; }
        public string years { get; set; }
    }
}




