using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlFileManagement : result
    {
        public List<FileManagement_list> FileManagement_list { get; set; }
        public List<GetFolderSummary> GetFolderSummary { get; set; }
        public List<FolderList> Folder_list { get; set; }
        public List<documentuploadlist_list> DocumentUploadlist_list { get; set; }
        public string docupload_name { get; set; }
        public string parent_gid { get; set; }
        public string docupload_type { get; set; }
        public string docuploadparent_gid { get; set; }
        public string folder_name { get; set; }
        public string fileupload_name { get; set; }

    }
    public class FolderList : result
    {
        public string docupload_gid { get; set; }
        public string docuploadparent_gid { get; set; }
        public string docupload_name { get; set; }
        public string document_name { get; set; }
        public string docupload_type { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string fileupload_name { get; set; }
        public string file_path { get; set; }
        public string azure_path { get; set; }
        
    }
    public class FileManagement_list : result
    {
        public string file_gid { get; set; }
        public string document_name { get; set; }
        public string document_path { get; set; }
        public string file_type { get; set; }
        public string folder { get; set; }
        public string folder_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
       

    }
    public class GetFolderSummary : result
    {

        public string file_gid { get; set; }
        public string document_name { get; set; }
        public string file_type { get; set; }
        public string folder { get; set; }
        public string folder_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

    }
    public class documentuploadlist_list : result
    {
        public string docupload_gid { get; set; }
        public string docupload_name { get; set; }
        public string document_name { get; set; }
        public string docupload_type { get; set; }
        public string docuploadparent_gid { get; set; }
        public string document_path { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string fileupload_name { get; set; }
        public string file_path { get; set; }
        public string vertical_name { get; set; }
        public string status_log { get; set; }
        public string documenttype_name { get; set; }
        public string checklist_name { get; set; }
        public string azure_path { get; set; }




    }
}