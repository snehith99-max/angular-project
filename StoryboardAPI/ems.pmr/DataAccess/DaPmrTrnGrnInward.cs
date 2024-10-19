using ems.pmr.Models;
using ems.utilities.Functions;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;

using System;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;

namespace ems.pmr.DataAccess
{
    public class DaPmrTrnGrnInward
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string base_url, api_key = string.Empty;
        public void DaGetGrnInwardSummary(MdlPmrTrnGrnInward values)
        {
            try
            {

                msSQL = " select distinct g.branch_prefix,a.mintsoftasn_id,a.grn_gid,a.dispatch_mode,a.grn_gid as grnrefno,a.dc_no,g.branch_name, a.grn_status, a.vendor_gid, a.grn_flag, a.invoice_flag,f.costcenter_name,d.purchaseorder_gid,DATE_FORMAT(d.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,a.file_path,a.file_name, " +
                    " CASE WHEN d.poref_no IS NULL OR d.poref_no = '' THEN d.purchaseorder_gid ELSE d.poref_no END AS porefno," +
                    " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag " +
                    " else a.grn_flag end as 'overall_status',concat(c.vendor_code,'/',c.vendor_companyname) as vendor, " +
                    " c.vendor_code,concat(c.contactperson_name,'/',c.email_id,'/',c.contact_telephonenumber) as contact, " +
                    " DATE_FORMAT(a.grn_date,'%d-%m-%Y')  as grn_date, c.vendor_companyname,a.purchaseorder_gid,format(d.total_amount,2) as po_amount,DATE_FORMAT(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " case when group_concat(distinct e.purchaserequisition_referencenumber)=',' then '' " +
                    " when group_concat(distinct e.purchaserequisition_referencenumber) <> ',' then group_concat(distinct e.purchaserequisition_referencenumber) end  as refrence_no," +
                    "(select mintsoft_flag from adm_mst_tcompany limit 1) as mintsoft_flag " +
                    " from pmr_trn_tgrn a " +
                    " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid   " +
                    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                    " left join pmr_trn_tpurchaseorder d on d.purchaseorder_gid=a.purchaseorder_gid" +
                    " left join pmr_trn_tpurchaserequisition e on e.purchaserequisition_gid=d.purchaserequisition_gid" +
                    " left join pmr_mst_tcostcenter f on d.costcenter_gid=f.costcenter_gid " +
                    "  left join hrm_mst_tbranch g on a.branch_gid=g.branch_gid  " +
                    " where 0 = 0  group by a.grn_gid order by a.grn_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetGrnInward_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetGrnInward_lists
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            grnrefno = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            po_date = dt["purchaseorder_date"].ToString(),
                            refrence_no = dt["refrence_no"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            po_amount = dt["po_amount"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            invoice_flag = dt["grn_flag"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            contact = dt["contact"].ToString(),
                            vendor = dt["vendor"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            despatch_mode = dt["dispatch_mode"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            mintsoft_flag = dt["mintsoft_flag"].ToString(),
                            mintsoftasn_id = dt["mintsoftasn_id"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.GetGrnInward_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting GRN summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetEditGrnInward(string grn_gid, MdlPmrTrnGrnInward values)
        {
            try
            {

                msSQL = " select a.grn_gid,a.purchaseorder_gid,date_format(a.grn_date,'%d-%m-%Y') as grn_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date,a.vendor_contact_person, " +
                " a.checkeruser_gid,a.purchaseorder_list,a.grn_remarks,a.grn_reference,a.grn_receipt,a.grn_status, " +
                " date_format(a.dc_date,'%d-%m-%Y') as dc_date,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.invoice_refno,a.dc_no," +
                " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag " +
                " else a.grn_flag end as 'overall_status', a.reject_reason,a.deliverytracking_number,a.dispatch_mode,a.received_note,i.gst_number, " +
                " concat(b.user_firstname,' - ',d.department_name) as user_firstname,e.branch_name,a.no_of_boxs, " +
                " i.vendor_gid,i.vendor_companyname,CONCAT_WS('\n',COALESCE(e.address1, ''),COALESCE(e.city, ''),COALESCE(e.state, ''),COALESCE(e.postal_code, '')) as Shipping_address, " +
                " i.contact_telephonenumber,i.email_id,concat(f.address1, '\n',f.address2, '\n',f.city, '\n',f.postal_code) as address,concat(y.user_firstname,'  ',y.user_lastname) as user_checkername,a.priority," +
                " a.checkeruser_gid, a.approved_by, y.user_gid,concat(z.user_firstname,'  ',z.user_lastname) as user_approvedby,CASE WHEN a.priority = 'N' THEN 'Low' ELSE 'High' END AS priority_n " +
                " from pmr_trn_tgrn a " +
                " left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                " left join hrm_mst_temployee c on c.user_gid = b.user_gid  " +
                " left join adm_mst_tuser y on y.user_gid = a.checkeruser_gid " +
                "  left join adm_mst_tuser z on z.user_gid = a.approved_by " +
                " left join hrm_mst_tdepartment d on c.department_gid=d.department_gid " +
                " left join acp_mst_tvendor i on i.vendor_gid = a.vendor_gid " +
                " left join adm_mst_taddress f on i.address_gid=f.address_gid " +
                " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid  " +
                " where a.grn_gid = '" + grn_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditGrnInward_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditGrnInward_lists
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            expected_date = dt["expected_date"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            address = dt["address"].ToString(),
                            vendor_address = dt["Shipping_address"].ToString().Replace("/", " "),
                            purchaseorder_list = dt["purchaseorder_list"].ToString(),
                            reject_reason = dt["reject_reason"].ToString(),
                            grn_remarks = dt["grn_remarks"].ToString(),
                            dc_date = dt["dc_date"].ToString(),
                            grn_reference = dt["grn_reference"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            user_checkername = dt["user_checkername"].ToString(),
                            user_approvedby = dt["user_approvedby"].ToString(),
                            priority_n = dt["priority_n"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            deliverytracking_number = dt["deliverytracking_number"].ToString(),
                            dispatch_mode = dt["dispatch_mode"].ToString(),
                            no_of_boxs = dt["no_of_boxs"].ToString(),
                            received_note = dt["received_note"].ToString(),
                        });

                        values.GetEditGrnInward_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetEditGrnInwardproduct(string grn_gid, MdlPmrTrnGrnInward values)
        {
            try
            {

                msSQL = " SELECT distinct b.grndtl_gid, a.grn_gid, a.purchaseorder_gid, b.qty_rejected as qty_rejected, b.qc_remarks, b.product_remarks, " +
                        " b.qtyreceivedas as qty_received, b.qty_grnadjusted as qty_grnadjusted, " +
                        " date_format( e.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,b.product_remarks,a.priority,b.display_field,b.qty_delivered,  " +
                        " b.product_gid, b.product_code, b.product_name, d.productgroup_name,b.productuom_name, f.qty_ordered as qty_ordered, " +
                        " h.location_name,k.bin_number FROM pmr_trn_tgrn a " +
                        " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid " +
                        " left join pmr_mst_tproduct c on b.product_gid = c.product_gid " +
                        " left join pmr_mst_tproductgroup d on d.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_trn_tpurchaseorder e on a.purchaseorder_gid = e.purchaseorder_gid " +
                        " left join pmr_trn_tpurchaseorderdtl f on f.purchaseorderdtl_gid = b.purchaseorderdtl_gid " +
                        " left join pmr_mst_tproductuom g on b.uom_gid = g.productuom_gid " +
                        " left join ims_mst_tlocation h on b.location_gid=h.location_gid " +
                        " left join ims_mst_tbin k on b.bin_gid=k.bin_gid" +
                        " where a.grn_gid = '" + grn_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditGrnInwardproduct_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditGrnInwardproduct_lists
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            qty_received = dt["qty_received"].ToString(),
                            qty_grnadjusted = dt["qty_grnadjusted"].ToString(),
                            qty_rejected = dt["qty_rejected"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            bin_number = dt["bin_number"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            qc_remarks = dt["qc_remarks"].ToString(),
                            display_field = dt["display_field"].ToString(),

                        });

                        values.GetEditGrnInwardproduct_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating product in GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPurchaseOrderDetails(string purchaseorder_gid, MdlPmrTrnGrnInward values)
        {
            try
            {
                msSQL = " select a.purchaseorderdtl_gid, a.product_gid, a.purchaseorder_gid, c.productgroup_name, a.product_code, a.product_name, a.productuom_name, a.qty_ordered, " +
        " format(a.product_price,2) as product_price, format(a.discount_percentage,2) as discount_percentage , format(a.discount_amount,2) as discount_amount, " +
        " a.tax_name,e.netamount,e.tax_amount,e.discount_amount as addon_discount,e.insurance_charges,e.packing_charges,e.roundoff, format(a.tax_percentage,2) as tax_percentage, format(a.tax_amount,2) as tax_amount, " +
        " a.tax_name2, format(a.tax_percentage2,2) as tax_percentage2, format(a.tax_amount2,2) as tax_amount2, concat(a.tax_name,' - ', a.tax_amount, ' / ', a.tax_name2,' - ', a.tax_amount2) as tax, " +
        " format((((qty_ordered * a.product_price) - a.discount_amount) + a.tax_amount + a.tax_amount2),2) as product_total, " +
        " e.payment_days, e.delivery_days, format(e.total_amount,2) as total_amount, format((a.tax_amount) + (a.tax_amount2),2) as total_tax, e.discount_amount, e.addon_amount, e.freightcharges, e.buybackorscrap, format(e.total_amount,2) as grand_total, e.currency_code from pmr_trn_tpurchaseorderdtl a " +
        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
        " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
        " left join pmr_trn_tpurchaseorder e on a.purchaseorder_gid = e.purchaseorder_gid " +
        " where a.purchaseorder_gid = '" + purchaseorder_gid + "'" + " " +
        " group by a.product_gid, a.uom_gid, a.display_field_name order by b.product_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaseorder_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaseorder_list
                        {
                            purchaseorderdtl_gid = dt["purchaseorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax = dt["tax_amount"].ToString(),
                            product_total = dt["netamount"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            total_amount = dt["netamount"].ToString(),
                            total_tax = dt["total_tax"].ToString(),
                            total_discount_amount = dt["addon_discount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            freight_charges = dt["freightcharges"].ToString(),
                            //buybackorscrap = dt["buybackorscrap"].ToString(),
                            grand_total = dt["grand_total"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.Getpurchaseorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        //Minsoft Api code by snehith for create asn
        //public result DaCreateASN(string grn_gid, string goodsintypes_id, string supplier)
        //{
            
        //    result objresult = new result();
        //    ASNList1 objMdlMintsoftJSON = new ASNList();
        //    PMRASNSTOCK_list OBJMintsoftStock = new PMRASNSTOCK_list();
        //    try
        //    {
        //        msSQL = " select * from smr_trn_tminsoftconfig;";
        //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objOdbcDataReader.HasRows == true)
        //        {
        //            objOdbcDataReader.Read();
        //            base_url = objOdbcDataReader["base_url"].ToString();
        //            api_key = objOdbcDataReader["api_key"].ToString();
        //        }
        //        objOdbcDataReader.Close();

        //        msSQL = " select goodsintypes_name from crm_smm_tmintsoftasngoodsintypes where goodsintypes_id='" + goodsintypes_id + "'";
        //        string goodsintypes_name = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = " select supplier_id from acP_mst_tvendor where  vendor_companyname='" + supplier + "'";
        //        string supplier_id = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = "select a.expected_date, b.poref_no from pmr_trn_tgrn a  " +
        //            " left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = a.purchaseorder_gid " +
        //            " where grn_gid ='" + grn_gid + "' ";
        //        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objOdbcDataReader.HasRows)
        //        {
        //            string expecteddate = objOdbcDataReader["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
        //            objMdlMintsoftJSON.WarehouseId = 3;
        //            objMdlMintsoftJSON.Supplier = supplier;
        //            objMdlMintsoftJSON.POReference = objOdbcDataReader["poref_no"].ToString();
        //            objMdlMintsoftJSON.EstimatedDelivery = expecteddate;
        //            objMdlMintsoftJSON.GoodsInType = goodsintypes_name;
        //            objMdlMintsoftJSON.ProductSupplierId = int.Parse(supplier_id);
        //        }

        //        msSQL = "select b.customerproduct_code,a.qtyreceivedas,b.mintsoftproduct_id, d.expected_date" +
        //                " from pmr_trn_tgrndtl a " +
        //                " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
        //                " left join pmr_trn_tgrn d on a.grn_gid=d.grn_gid" +
        //                " where a.grn_gid = '" + grn_gid + "'";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        if (dt_datatable.Rows.Count > 0)
        //        {
        //            int i = 0;
        //            objMdlMintsoftJSON.Items = new AsnItem[dt_datatable.Rows.Count];

        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                objMdlMintsoftJSON.Items[i] = new AsnItem();
        //                objMdlMintsoftJSON.Items[i].ProductId = dt["mintsoftproduct_id"].ToString();
        //                objMdlMintsoftJSON.Items[i].SKU = dt["customerproduct_code"].ToString();
        //                objMdlMintsoftJSON.Items[i].Quantity = int.Parse(dt["qtyreceivedas"].ToString());
        //                objMdlMintsoftJSON.Items[i].ExpiryDate = dt["expected_date"].ToString() + DateTimeOffset.Now.ToString("THH:mm:ss.fffffffK");
                        

        //                //OBJMintsoftStock.ProductId = int.Parse(dt["mintsoftproduct_id"].ToString());
        //                //OBJMintsoftStock.PMRASN_list[i].SKU = dt["customerproduct_code"].ToString();
        //                //OBJMintsoftStock.PMRASN_list[i].Quantity = int.Parse(dt["qtyreceivedas"].ToString());
        //                //OBJMintsoftStock.PMRASN_list[i].WarehouseId = 3;
        //                i++;
        //            }
        //            dt_datatable.Dispose();
        //        }
        //        string json = JsonConvert.SerializeObject(objMdlMintsoftJSON);
               

        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //        var client = new RestClient(base_url);
        //        var request = new RestRequest("/api/ASN", Method.PUT);
        //        request.AddHeader("ms-apikey", api_key);
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddParameter("application/json", json, ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseData = JsonConvert.DeserializeObject<MintsoftASNResponse>(response.Content);
        //            msSQL = "update pmr_trn_tgrn set mintsoftasn_id = '" + responseData.ID + "' where grn_gid ='" + grn_gid + "'";
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            if (mnResult != 0)
        //            {

        //                string json1 = JsonConvert.SerializeObject(OBJMintsoftStock);
                       
        //                JObject jsonObject = JObject.Parse(json);

        //                JArray jsonArray = new JArray();

        //                jsonArray.Add(jsonObject);
                       
        //                string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //                var client2 = new RestClient(base_url);
        //                var request2 = new RestRequest("/api/Product/BulkOnHandStockUpdate", Method.POST);
        //                request.AddHeader("ms-apikey", api_key);
        //                request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
        //                IRestResponse response2 = client.Execute(request2);
        //                if (response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
        //                    if (objResult[0].Success)
        //                    {
        //                        var result = objResult[0].ID;
        //                    }
        //                }

        //                var client1 = new RestClient(base_url);
        //                var request1 = new RestRequest("/api/ASN/'" + responseData.ID + "'/Confirm", Method.GET);
        //                request.AddHeader("Accept", "application/json");
        //                request.AddHeader("ms-apikey", api_key);
        //                IRestResponse response1 = client.Execute(request1);
        //                if (response1.StatusCode == HttpStatusCode.OK)
        //                {                       
        //                    objresult.status = true;
        //                    objresult.message = "ASN Added Successfully, ASN ID:" + responseData.ID + "";
        //                }
        //                else
        //                {

        //                    string errorMessage = $"Failed to fetch products. Status code: {response1.StatusCode}, Reason: {response1.ErrorMessage}";
        //                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Purchase/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //                    objresult.status = false;
        //                    objresult.message = "Error While Updating ASN Status as AWAITINGDELIVERY ";
        //                }
        //            }
        //            else
        //            {
        //                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
        //               "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //                objresult.status = false;
        //                objresult.message = "Error While Creating ASN";
        //            }
        //        }
        //        else
        //        {
        //            string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ErrorMessage}";
        //            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Purchase/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //            objresult.status = false;
        //            objresult.message = "Error While Creating ASN";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objresult.message = "Exception occured while posting to Mintsoft!";
        //    }
        //    return objresult;
        //}
        //public class MintsoftASNResponse
        //{
        //    public string ID { get; set; }
        //    public string Success { get; set; }
        //    public string Message { get; set; }
        //    public string WarningMessage { get; set; }
        //    public string AllocatedFromReplen { get; set; }
        //}
        public void DaGetGoodInTypesMintSoft(MdlPmrTrnGrnInward values)
        {
            msSQL = " select goodsintypes_name, goodsintypes_id from crm_smm_tmintsoftasngoodsintypes";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var GetGoodInType = new List<GetGoodInType_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetGoodInType.Add(new GetGoodInType_list
                    {
                        goodsintypes_name = dt["goodsintypes_name"].ToString(),
                        goodsintypes_id = dt["goodsintypes_id"].ToString(),
                    });
                }
                values.GetGoodInType_list = GetGoodInType;
            }
        }


        public void DaGetGRNsixmonthschart(MdlPmrTrnGrnInward values)
        {


            msSQL = " select  DATE_FORMAT(grn_date, '%b-%Y')  as grn_date,substring(date_format(a.grn_date,'%M'),1,3)as month, "+
                    " a.grn_gid,year(a.grn_date) as year,count(a.grn_gid) as ordercount,date_format(grn_date, '%M/%Y') as month_wise "+
                    " from pmr_trn_tgrn a where a.grn_date > date_add(now(), interval - 6 month) "+
                    " and a.grn_date <= date(now()) group by date_format(a.grn_date, '%M') order by a.grn_date desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GRNlastsixmonths_list = new List<GRNlastsixmonths_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GRNlastsixmonths_list.Add(new GRNlastsixmonths_list
                    {
                        grn_date = (dt["grn_date"].ToString()),
                        months = (dt["month"].ToString()),
                        ordercount = (dt["ordercount"].ToString()),

                    });
                    values.GRNlastsixmonths_list = GRNlastsixmonths_list;
                }

            }

            msSQL = " select (select Count(a.grn_gid)from pmr_trn_tgrn a ) as grn_count,COUNT(a.purchaseorder_gid) AS approved_count FROM pmr_trn_tpurchaseorder a  "+
                    " LEFT JOIN pmr_trn_tpurchaseorderdtl s ON a.purchaseorder_gid = s.purchaseorder_gid  " +
                    " LEFT JOIN pmr_mst_tproduct g ON s.product_gid = g.product_gid " +
                    " LEFT JOIN pmr_mst_tproducttype i ON i.producttype_gid = g.producttype_gid " +
                    " where 0 = 0 And i.producttype_name != 'Services'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                values.ordertogrncount = objOdbcDataReader["grn_count"].ToString();
                values.ordercount = objOdbcDataReader["approved_count"].ToString();
            }

        }
    }
}
