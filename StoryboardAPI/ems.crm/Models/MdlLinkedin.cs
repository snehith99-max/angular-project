using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ems.crm.Models
{
    public class MdlLinkedin : result
    {
        public List<list_accesstokens> list_accesstokens { get; set; }
        public List<accountsummary_list> accountsummary_list { get; set; }
        public List<postsummarylist> postsummarylist { get; set; }

    }
    public class list_accesstokens
    {
        public string account_id { get; set; }
        public string access_token { get; set; }
    }

    public class Mdlcompanydetails
    {
        public string localizedName { get; set; }
        public Website website { get; set; }
        public Foundedon foundedOn { get; set; }
        public Created created { get; set; }
        public Description description { get; set; }
        public string versionTag { get; set; }
        public string organizationStatus { get; set; }
        public Coverphotov2 coverPhotoV2 { get; set; }
        public string organizationType { get; set; }
        public string[] localizedSpecialties { get; set; }
        public Location2[] locations { get; set; }
        public string id { get; set; }
    }
    public class Website
    {
        public Localized localized { get; set; }
    }

    public class Localized
    {
        public string en_US { get; set; }
    }
    public class Foundedon
    {
        public int year { get; set; }
    }

    public class Created
    {
        public string actor { get; set; }
    }

    public class Description
    {
        public Localized1 localized { get; set; }
    }

    public class Localized1
    {
        public string en_US { get; set; }
    }
    public class Coverphotov2
    {
        public string cropped { get; set; }
        public string original { get; set; }
    }

    public class Name
    {
        public Localized2 localized { get; set; }
    }

    public class Localized2
    {
        public string en_US { get; set; }
    }
    public class Lastmodified
    {
        public string actor { get; set; }
    }

    public class Logov2
    {
        public string cropped { get; set; }
        public string original { get; set; }
    }

    public class Specialty
    {
        public string[] tags { get; set; }
    }
    public class Location2
    {
        public Description1 description { get; set; }
        public string locationType { get; set; }
        public Address1 address { get; set; }
        public string localizedDescription { get; set; }
        public string streetAddressFieldState { get; set; }
        public string geoLocation { get; set; }
    }

    public class Description1
    {
        public Localized3 localized { get; set; }
    }

    public class Localized3
    {
        public string en_US { get; set; }
    }

    public class Address1
    {
        public string geographicArea { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string line2 { get; set; }
        public string line1 { get; set; }
        public string postalCode { get; set; }
    }


    public class Mdlfollowers
    {
        public int firstDegreeSize { get; set; }
    }

    public class accountsummary_list
    {
        public string account_id { get; set; }
        public string company_name { get; set; }
        public string company_website { get; set; }
        public string founded_on { get; set; }
        public string followers_count { get; set; }
    }
    public class Mdlaccountview : result
    {
        public string account_id { get; set; }
        public string company_name { get; set; }
        public string company_website { get; set; }
        public string founded_on { get; set; }
        public string followers_count { get; set; }
        public string description { get; set; }
        public string organizationStatus { get; set; }
        public string organizationType { get; set; }
        public string localizedSpecialties { get; set; }
        public List<accountview_list> accountview_list { get; set; }
    }
    public class accountview_list
    {
        public string locationcompany_name { get; set; }
        public string location_type { get; set; }
        public string account_id { get; set; }
        public string geographicArea { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string line2 { get; set; }
        public string line1 { get; set; }
        public string postalCode { get; set; }
    }
    public class postvalues
    {
        public string account_id { get; set; }
        public string linkedin_text { get; set; }
        public string access_token { get; set; }
        public string author { get; set; }
        public string commentary { get; set; }
        public string visibility { get; set; }
        public string distribution { get; set; }
        public string feedDistribution { get; set; }
        public string targetEntities { get; set; }
        public string thirdPartyDistributionChannels { get; set; }
        public string lifecycleState { get; set; }
        public string isReshareDisabledByAuthor { get; set; }
    }
    public class Responses
    {
        public Value value { get; set; }
    }

    public class Value
    {
        public UploadMechanism uploadMechanism { get; set; }
        public string asset { get; set; }
    }

    public class UploadMechanism
    {
        [JsonProperty("com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest")]
        public MediaUploadHttpRequest MediaUploadHttpRequest { get; set; }
    }

    public class MediaUploadHttpRequest
    {
        public string uploadUrl { get; set; }
        public Headers headers { get; set; }
    }

    public class Headers
    {
        [JsonProperty("media-type-family")]
        public string MediaTypeFamily { get; set; }
    }
    public class pollpost
    {
        public string linkedin_text { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public string poll_duration { get; set; }
        public string account_id { get; set; }
        public string poll_question { get; set; }
    }


    public class mdlgetpost
    {
        public Paging21 paging { get; set; }
        public Element[] elements { get; set; }
    }

    public class Paging21
    {
        public int start { get; set; }
        public int count { get; set; }
        public Link[] links { get; set; }
        public int total { get; set; }
    }

    public class Link
    {
        public string type { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Element
    {
        public bool isReshareDisabledByAuthor { get; set; }
        public long createdAt { get; set; }
        public string lifecycleState { get; set; }
        public long lastModifiedAt { get; set; }
        public string visibility { get; set; }
        public long publishedAt { get; set; }
        public string author { get; set; }
        public string id { get; set; }
        public Distribution distribution { get; set; }
        public Content content { get; set; }
        public string commentary { get; set; }
        public Lifecyclestateinfo lifecycleStateInfo { get; set; }
    }

    public class Distribution
    {
        public string feedDistribution { get; set; }
        public object[] thirdPartyDistributionChannels { get; set; }
    }

    public class Content
    {
        public Media media { get; set; }
        public Multiimage multiImage { get; set; }
        public Article article { get; set; }
        public Poll poll { get; set; }

    }
    public class Poll
    {
        public Option1[] options { get; set; }
        public Settings settings { get; set; }
        public int uniqueVotersCount { get; set; }
        public string question { get; set; }
    }
    public class Option1
    {
        public string isVotedByViewer { get; set; }
        public string voteCount { get; set; }
        public string text { get; set; }
    }
    public class Settings
    {
        public string duration { get; set; }
        public string voteSelectionType { get; set; }
        public bool isVoterVisibleToAuthor { get; set; }
    }
    public class Media
    {
        public string altText { get; set; }
        public string id { get; set; }
    }

    public class Multiimage
    {
        public Image12[] images { get; set; }
    }

    public class Image12
    {
        public string altText { get; set; }
        public string id { get; set; }
    }

    public class Article
    {
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string source { get; set; }
        public string title { get; set; }
    }

    public class Lifecyclestateinfo
    {
        public bool isEditedByAuthor { get; set; }
    }
    public class downloadevent
    {
        public string downloadUrl { get; set; }
        public string id { get; set; }
        public long downloadUrlExpiresAt { get; set; }
        public string status { get; set; }
        public int duration { get; set; }
        public float aspectRatioWidth { get; set; }
        public string owner { get; set; }
        public string thumbnail { get; set; }
        public float aspectRatioHeight { get; set; }
        public string captions { get; set; }
        public List<postsummarylist> postsummarylist { get; set; }
        public string image_urls { get; set; }
    }
    public class postsummarylist
    {
        public string image_url { get; set; }
        public string post_type { get; set; }
        public string tableType { get; set; }
    }


    public class mediainfo
    {
        public string DownloadUrl { get; set; }
        public string Owner { get; set; }
        public string Id { get; set; }
        public long DownloadUrlExpiresAt { get; set; }
        public string Status { get; set; }
        public long Duration { get; set; }
        public double AspectRatioWidth { get; set; }
        public string Thumbnail { get; set; }
        public double AspectRatioHeight { get; set; }
    }

    public class ApiResponse
    {
        public Dictionary<string, mediainfo> Results { get; set; }
        public Dictionary<string, int> Statuses { get; set; }
        public Dictionary<string, object> Errors { get; set; }
    }
    public class Mdlpostsummary
    {
        public List<postsummarylistview> postsummarylistview { get; set; }
        public string lscompany_name { get; set; }
        public string lstotal_post { get; set; }
    }
    public class postsummarylistview
    {
        public string post_id { get; set; }
        public string createdAt { get; set; }
        public string visibility { get; set; }
        public string media_type { get; set; }
        public string imagedownload_url { get; set; }
        public string image_url { get; set; }
        public string caption { get; set; }
        public string dtlpost_id { get; set; }
        public string dtlcreatedAt { get; set; }
        public string dtlvisibility { get; set; }
        public string dtlmedia_type { get; set; }
        public string dtlimage_url { get; set; }
        public string dtlimagedownload_url { get; set; }
        public string dtlcaption { get; set; }


    }
}