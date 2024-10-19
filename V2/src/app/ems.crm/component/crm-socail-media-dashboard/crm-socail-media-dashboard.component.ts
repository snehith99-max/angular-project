import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,  ValidationErrors,
  AbstractControl,
  ValidatorFn } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-crm-socail-media-dashboard',
  templateUrl: './crm-socail-media-dashboard.component.html',
  styleUrls: ['./crm-socail-media-dashboard.component.scss']
})
export class CrmSocailMediaDashboardComponent {
  shopifyproductcount: any;
shopifycustomercount: any;
shopifyordercount: any;
product_count: any;
customer_count: any;
 order_count:any;
 shopifystorename:any;
 store_name:any;
 contactcount_list: any;
contact_count: any;
messagecount_list: any;
message_count: any;
messageincoming_list: any;
incoming_count:any;
messageoutgoing_list:any;
 sent_count:any;

emailstatus_list: any;
deliverytotal_count: any;
opentotal_count:any;
clicktotal_count:any;
 sentmailcount_list:any;
 mail_sent:any;
 customertotalcount_list:any;
  customer_assigncount: any;
  customerassignedcount_list: any;
  unassign_count:any;
  customerunassignedcount_list:any;
  shopifyproduct_counts:any;
 shopifyproducts_list:any[]=[];
 response_data:any;


  
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
   
  }
  ngOnInit(): void {
   
    var url1 = 'SocialMedia/GetShopifyProductCount'
  this.service.get(url1).subscribe((result,) => {

    this.shopifyproductcount = result;
    this.product_count = this.shopifyproductcount.count;
  

  });

  var url66 = 'SocialMedia/GetShopifyProductCounts'
  this.service.get(url66).subscribe((result,) => {

    this.response_data = result;
    this.shopifyproduct_counts =  Number(this.response_data.shopifyproducts_list[0].shopifyproduct_count); 
  });
  // var url2 = 'SocialMedia/GetShopifyOrderCount'
  // this.service.get(url2).subscribe((result,) => {

  //   this.shopifyordercount = result;
  //   this.order_count = this.shopifyordercount.count;
  
   

  // });
  // var url3 = 'SocialMedia/GetShopifyCustomerCount'
  // this.service.get(url3).subscribe((result,) => {

  //   this.shopifycustomercount = result;
  //   this.customer_count = this.shopifycustomercount.count;


  // });

  var url4 = 'SocialMedia/GetShopifyStoreName'
  this.service.get(url4).subscribe((result,) => {

    this.shopifystorename = result;
    this.store_name = this.shopifystorename.shop.name;
  
   

  });
  var url5 = 'SocialMedia/GetContactCount'
  this.service.get(url5).subscribe((result,) => {

    this.contactcount_list = result;
    this.contact_count = this.contactcount_list.contactcount_list[0].contact_count1;
  
   

  });
  var url6 = 'SocialMedia/GetMessageCount'
  this.service.get(url6).subscribe((result,) => {

    this.messagecount_list = result;
    this.message_count = this.messagecount_list.messagecount_list[0].message_count;
  
    //  console.log('customer',this.messagecount_list)

  });
  var url7 = 'SocialMedia/GetMessageIncomingCount'
  this.service.get(url7).subscribe((result,) => {

    this.messageincoming_list = result;
    this.incoming_count = this.messageincoming_list.messageincoming_list[0].incoming_count;
  
    // console.log('customer',this.incoming_count)

  });
  var url8 = 'SocialMedia/GetMessageOutgoingCount'
  this.service.get(url8).subscribe((result,) => {

    this.messageoutgoing_list = result;
    this.sent_count = this.messageoutgoing_list.messageoutgoing_list[0].sent_count;
  
    // console.log('customer',this.sent_count)

  });
  var url9 = 'SocialMedia/GetEmailStatusCount'
  this.service.get(url9).subscribe((result,) => {

    this.emailstatus_list = result;
    this.deliverytotal_count = this.emailstatus_list.emailstatus_list[0].deliverytotal_count;
    this.opentotal_count = this.emailstatus_list.emailstatus_list[0].opentotal_count;
    this.clicktotal_count = this.emailstatus_list.emailstatus_list[0].clicktotal_count;
    
    //  console.log('customer',this.sent_count)
    

  });
  var url10 = 'SocialMedia/GetSentCount'
  this.service.get(url10).subscribe((result,) => {

    this.sentmailcount_list = result;
    this.mail_sent = this.sentmailcount_list.sentmailcount_list[0].mail_sent;
  
    // console.log('customer',this.mail_sent)

  });
  var url11 = 'ShopifyCustomer/GetCustomerAssignedCount'
  this.service.get(url11).subscribe((result,) => {

    this.customerassignedcount_list = result;
    this.customer_assigncount = this.customerassignedcount_list.customerassignedcount_list[0].customer_assigncount;
  

  });
  var url12 = 'ShopifyCustomer/GetCustomerUnassignedCount'
  this.service.get(url12).subscribe((result,) => {

    this.customerunassignedcount_list = result;
    this.unassign_count = this.customerunassignedcount_list.customerunassignedcount_list[0].unassign_count;
  

  });
  var url13 = 'ShopifyCustomer/GetCustomerTotalCount'
  this.service.get(url13).subscribe((result,) => {

    this.customertotalcount_list = result;
    this.customer_count = this.customertotalcount_list.customertotalcount_list[0].customer_totalcount;
  

  });
 
}
}
