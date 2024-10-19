﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrRptAgeingreport
    {

        public List<thirtydays_list> thirtydays_list {  get; set; }
        public List<thirtytosixty_list> thirtytosixty_list {  get; set; }
        public List<sixtytoninty_list> sixtytoninty_list {  get; set; }
        public List<nintytoonetwenty_list> nintytoonetwenty_list {  get; set; }
        public List<onetwentytooneeighty_list> onetwentytooneeighty_list {  get; set; }
        public List<All_lists> All_lists {  get; set; }

        public double paid_amount30 { get; set; }
        public double invoice_amount30 { get; set; }
        public double outstanding_amount30 { get; set; }

        public double paid_amount60 { get; set; }
        public double invoice_amount60 { get; set; }
        public double outstanding_amount60 { get; set; }

        public double paid_amount90 { get; set; }
        public double invoice_amount90 { get; set; }
        public double outstanding_amount90 { get; set; }

        public double paid_amount120 { get; set; }
        public double invoice_amount120 { get; set; }
        public double outstanding_amount120 { get; set; }

        public double paid_amount180 { get; set; }
        public double invoice_amount180 { get; set; }
        public double outstanding_amount180 { get; set; }

        public double paid_amountall { get; set; }
        public double invoice_amountall { get; set; }
        public double outstanding_amountall { get; set; }

    }
    public class thirtydays_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
    public class thirtytosixty_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
    public class sixtytoninty_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
    public class nintytoonetwenty_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
    public class onetwentytooneeighty_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
    public class All_lists : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch { get; set; }
        public string company_details { get; set; }
        public string type { get; set; }
        public string invoice_amount { get; set; }
        public string paid_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string statuses { get; set; }


    }
}