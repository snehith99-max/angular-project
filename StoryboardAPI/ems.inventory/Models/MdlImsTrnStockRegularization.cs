using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnStockRegularization:result
    {
        public List<stockdetails_list> stockdetails_list {  get; set; }
        public List<ContractSummary_List> ContractSummary_List {  get; set; }
    }
    public class stockdetails_list : result
    {
        public string stock_gid {  get; set; }
        public string branch_gid {  get; set; }
        public string product_gid {  get; set; }
        public string uom_gid {  get; set; }
        public string product_code {  get; set; }
        public string product_name {  get; set; }
        public string productgroup_name {  get; set; }
        public string productuom_name {  get; set; }
        public string adjustment_qty {  get; set; }
        public string branch_name {  get; set; }
        public string branch_prefix {  get; set; }
        public string product_type {  get; set; }
    }
    public class merge_list:result
    {
        public List<ContractSummary_List> ContractSummary_List { get; set; }
    }
    public class ContractSummary_List : result
    {
        public string branch_gid { get; set; }
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string adjustment_qty { get; set; }
        public string stock_gid { get; set; }
    }
}