import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

  
@Component({
  selector: 'app-ims-trn-raise-deliveryorder',
  templateUrl: './ims-trn-raise-deliveryorder.component.html',
  styleUrls: ['./ims-trn-raise-deliveryorder.component.scss']
})
export class ImsTrnRaiseDeliveryorderComponent {
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '1130px',
    placeholder: 'Enter text here...',
    translate: 'no',

  };
  deliveryform: FormGroup | any;
  deliveryform1: FormGroup | any;
  template_content : FormGroup |any;
  productform: FormGroup | any;
  raisedeliveryorder_list: any;
  branch_list : any [] = [];
  customer_list: any [] = [];
  contact_list: any [] = [];
  sales_list: any [] = [];
  currency_list: any [] = [];
  product_list: any [] = [];
  productgroup_list: any [] = [];
  productname_list: any [] = [];
  tax_list: any [] = [];
  POproductlist: any [] = [];
  OutstandingQty_list: any [] = [];
  IssuedQty_list: any [] = [];
  modeofdispatch_list:any[]=[];
  totalamount: any;
  taxamount3: any;
  data:any;
  mdlTaxName3: any;
  invoice_gid:any;
  taxamount2: any;
  taxamount1: any;
  discountamount: any;
  discountpercentage: any;
  quantity: any;
  isSubmitDisabled:any;
  salesorder_gid:any;
  deliveryorder: any;
  raisedelivery_list :any
  response_data :any
  parameter: any;
  responsedata: any;
  producttotalamount:any;
  raisedeliveryorder_list1:any;
  qtyissued_list:any;
  total_amount:any;
  constructor(private formBuilder: FormBuilder, public ToastrService: ToastrService, public service: SocketService, public NgxSpinnerService: NgxSpinnerService, private route: Router,private router: ActivatedRoute) {
    this.deliveryform1 = new FormGroup ({
      product_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),
    producttotalamount:new FormControl(''),
    total_amount:new FormControl(''),
    product_name:new FormControl(''),
    uom_name:new FormControl(''),
    display_field:new FormControl(''),
    outstanding_qty:new FormControl(''),
    created_date:new FormControl(''),
    reference_gid:new FormControl(''),
    display:new FormControl(''),    
    stock_gid:new FormControl(''),
    stock_qty:new FormControl(''),
    })

    this.deliveryform = new FormGroup ({
      salesorder_gid: new FormControl(''),
      salesorder_date: new FormControl(''),
      branch_name:new FormControl(''),
      customer_details:new FormControl(''),
      customer_branch: new FormControl(''),
      branch_gid: new FormControl(''),
      so_referencenumber: new FormControl(''),
      customer_gid: new FormControl(''),
      customer_name: new FormControl(''),
      customer_address_so : new FormControl(''),
      customer_contact_gid: new FormControl(''),
      customercontact_names: new FormControl(''),
      customer_mobile: new FormControl(''),
      customer_email: new FormControl(''),
      customer_address: new FormControl(''),
      dc_no :  new FormControl('',Validators.required),
      customer_mode : new FormControl(''),
      tracker_id:new FormControl(''),
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      customerproduct_code: new FormControl(''),
      product_code: new FormControl(''),
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      display_field: new FormControl(''),
      template_content : new FormControl(''),
      qty_quoted: new FormControl(''),
      product_uom: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      remark : new FormControl(''),
      available_quantity : new FormControl(''),
      product_delivered : new FormControl(''),
      qty_issued : new FormControl(''),
      producttotalamount:new FormControl(''),
      total_amount:new FormControl(''),
      dc_note:new FormControl(''),
        });
  }

  ngOnInit(): void {


   
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
  this.deliveryform = new FormGroup ({
  salesorder_gid: new FormControl(''),
  salesorderdtl_gid:new FormControl(''),
  salesorder_date: new FormControl(''),
  customer_branch: new FormControl(''),
  branch_gid: new FormControl(''),
  branch_name:new FormControl(''),
  customer_details:new FormControl(''),
  so_referencenumber: new FormControl(''),
  customer_gid: new FormControl(''),
  customer_name: new FormControl(''),
  customer_address_so : new FormControl(''),
  customer_contact_gid: new FormControl(''),
  customercontact_names: new FormControl(''),
  customer_mobile: new FormControl(''),
  customer_email: new FormControl(''),
  customer_address: new FormControl(''),
  dc_no: new FormControl('',Validators.required),
  customer_mode : new FormControl(''),
  tracker_id:new FormControl(''),
  no_of_boxs:new FormControl(''),
  productgroup_gid: new FormControl(''),
  productgroup_name: new FormControl(''),
  customerproduct_code: new FormControl(''),
  product_code: new FormControl(''),
  product_gid: new FormControl(''),
  product_name: new FormControl(''),
  display_field: new FormControl(''),
  template_content : new FormControl(''),
  qty_quoted: new FormControl(''),
  product_uom: new FormControl(''),
  product_requireddate: new FormControl(''),
  product_requireddateremarks: new FormControl(''),
  remark : new FormControl(''),
  available_quantity : new FormControl(''),
  qty_issued : new FormControl(''),
  product_delivered : new FormControl(''),
  producttotalamount:new FormControl(''),
  total_amount:new FormControl(''),
  dc_note:new FormControl(''),
    });
    this.deliveryform1 = new FormGroup ({
      product_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),
    product_name:new FormControl(''),
    uom_name:new FormControl(''),
    display_field:new FormControl(''),
    outstanding_qty:new FormControl(''),
    created_date:new FormControl(''),
    reference_gid:new FormControl(''),
    display:new FormControl(''),    
    stock_gid:new FormControl(''),
    stock_qty:new FormControl(''),
    qty_issued : new FormControl(''),
    producttotalamount:new FormControl(''),
    total_amount:new FormControl(''),
    })
  this.deliveryorder = this.router.snapshot.paramMap.get('salesorder_gid');
     const secretKey = 'storyboarderp';
     const deencryptedParam = AES.decrypt(this.deliveryorder, secretKey).toString(enc.Utf8);
     this.GetRaiseDeliveryorderSummary(deencryptedParam);
     this.GetProductdelivery(deencryptedParam);
     var url = 'ImsTrnDeliveryorderSummary/modeofdispatchdropdown'
     this.service.get(url).subscribe((result:any)=>{
       this.modeofdispatch_list = result.modeofdispatch_list
     });
}
get tracker_id() {
  return this.deliveryform.get('tracker_id')!;
}
get customer_mode() {
  return this.deliveryform.get('customer_mode')!;
}
get no_of_boxs() {
  return this.deliveryform.get('no_of_boxs')!;
}
get dc_no() {
  return this.deliveryform.get('dc_no')!;
}
  GetRaiseDeliveryorderSummary(salesorder_gid: any) {
        var url = 'ImsTrnDeliveryorderSummary/GetRaiseDeliveryorderSummary'
        this.NgxSpinnerService.show()
        let param = {
          salesorder_gid: salesorder_gid
        }
        this.service.getparams(url, param).subscribe((result: any) => {
    debugger;
          this.raisedelivery_list = result.raisedelivery_list;
          // this.qtyissued_list = result.qtyissued_list;
          this.deliveryform.get("salesorder_gid")?.setValue(result.raisedelivery_list[0].salesorder_gid);
          this.deliveryform.get("salesorderdtl_gid")?.setValue(result.raisedelivery_list[0].salesorderdtl_gid);
          this.deliveryform.get("product_gid")?.setValue(result.raisedelivery_list[0].product_gid);
          this.deliveryform.get("salesorder_date")?.setValue(result.raisedelivery_list[0].salesorder_date);
          this.deliveryform.get("dc_no")?.setValue(result.raisedelivery_list[0].invoice_refno);
          this.deliveryform.get("customer_branch")?.setValue(result.raisedelivery_list[0].customer_branch);
          this.deliveryform.get("customer_gid")?.setValue(result.raisedelivery_list[0].customer_gid);
          this.deliveryform.get("customer_name")?.setValue(result.raisedelivery_list[0].customer_name);
          this.deliveryform.get("so_referencenumber")?.setValue(result.raisedelivery_list[0].so_referencenumber);
          this.deliveryform.get("customercontact_names")?.setValue(result.raisedelivery_list[0].customercontact_names);
          this.deliveryform.get("customer_mobile")?.setValue(result.raisedelivery_list[0].customer_mobile);
          this.deliveryform.get("customer_email")?.setValue(result.raisedelivery_list[0].customer_email);       
          this.deliveryform.get("customer_address")?.setValue(result.raisedelivery_list[0].customer_address_so);
          this.deliveryform.get("customer_address_so")?.setValue(result.raisedelivery_list[0].customer_address_so);
          this.deliveryform.get("productgroup_name")?.setValue(result.raisedelivery_list[0].productgroup_name);
          this.deliveryform.get("customerproduct_code")?.setValue(result.raisedelivery_list[0].customerproduct_code);
          this.deliveryform.get("product_code")?.setValue(result.raisedelivery_list[0].product_code);
          this.deliveryform.get("product_name")?.setValue(result.raisedelivery_list[0].product_name);
          this.deliveryform.get("display_field")?.setValue(result.raisedelivery_list[0].display_field);
          this.deliveryform.get("uom_name")?.setValue(result.raisedelivery_list[0].uom_name);
          this.deliveryform.get("available_quantity")?.setValue(result.raisedelivery_list[0].available_quantity);
          this.deliveryform.get("qty_quoted")?.setValue(result.raisedelivery_list[0].qty_quoted);
          this.deliveryform.get("product_delivered")?.setValue(result.raisedelivery_list[0].product_delivered);
          this.deliveryform.get("qty_issued")?.setValue(result.raisedelivery_list[0].qty_issued);
          this.deliveryform.get("branch_name")?.setValue(result.raisedelivery_list[0].branch_name);
          this.deliveryform.get("salesorderdtl_gid")?.setValue(result.raisedelivery_list[0].salesorderdtl_gid);
          const emailId = this.raisedelivery_list[0].customer_email;
          const contactTelephonenumber = this.raisedelivery_list[0].customer_mobile;
          const gst_no=this.raisedelivery_list[0].gst_number;
          const so_referencenumber1 = this.raisedelivery_list[0].so_referencenumber;
          const salesorder_date = this.raisedelivery_list[0].salesorder_date;
          const so_referencenumber=`${so_referencenumber1}/${salesorder_date}`;
          const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gst_no}`;
          this.deliveryform.get("customer_details")?.setValue(vendorDetails);
          this.NgxSpinnerService.hide()
        });
      }
      GetProductdelivery(salesorder_gid :any){
        debugger
        var api = 'ImsTrnDeliveryorderSummary/GetProductdelivery';
        let param = {
          salesorder_gid: salesorder_gid
        }
        this.service.getparams(api,param).subscribe((result:any) => {
          this.raisedeliveryorder_list1 = result.raisedelivery_list;
          setTimeout(()=>{  
            $('#raisedelivery_list').DataTable();
          }, 1);
        });
      }
      onadd(salesorder_gid:any, product_gid:any, salesorderdtl_gid:any) {
        debugger
        var api = 'ImsTrnDeliveryorderSummary/IssueFromStock';
        let param1 = {
          product_gid: product_gid,
          salesorder_gid:salesorder_gid,
          salesorderdtl_gid:salesorderdtl_gid 
        }
        this.service.getparams(api,param1).subscribe((result:any) => {
          this.IssuedQty_list = result.IssuedQty_list;
          this.deliveryform1.get('total_amount').valueChanges.subscribe((value: any) => {
            this.deliveryform1.get('producttotalamount').setValue(value);
          });
        });
        var api = 'ImsTrnDeliveryorderSummary/GetOutstandingQty';
        let param2 = {
          salesorder_gid: salesorder_gid,
          salesorderdtl_gid:salesorderdtl_gid
        }
        this.service.getparams(api,param2).subscribe((result:any) => {
          this.OutstandingQty_list = result.OutstandingQty_list;
        });
      }
OnSubmit(){
  debugger
  this.deliveryorder = this.router.snapshot.paramMap.get('salesorder_gid');
     const secretKey = 'storyboarderp';
     const deencryptedParam = AES.decrypt(this.deliveryorder, secretKey).toString(enc.Utf8);
     this.salesorder_gid = deencryptedParam
  console.log(this.IssuedQty_list)
  var params = {
    salesorder_gid : this.salesorder_gid,
    product_gid:this.IssuedQty_list[0].product_gid,
    stock_gid:this.IssuedQty_list[0].stock_gid,
    uom_gid:this.IssuedQty_list[0].uom_gid,
    branch_gid:this.IssuedQty_list[0].branch_gid,
    salesorderdtl_gid:this.OutstandingQty_list[0].salesorderdtl_gid,
    outstanding_qty:this.OutstandingQty_list[0].outstanding_qty,
    stock_qty:this.IssuedQty_list[0].stock_qty,
    producttotalamount:this.deliveryform1.value.producttotalamount,
    total_amount:this.deliveryform1.value.total_amount,
  }
      var url1='ImsTrnDeliveryorderSummary/PostSelectIssueQtySubmit'
      this.NgxSpinnerService.show()
      this.service.post(url1, params).subscribe((result: any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)    
          this.NgxSpinnerService.hide()
        }
        else{
          this.GetProductdelivery(this.salesorder_gid)
          this.NgxSpinnerService.hide()
        } 
      });
}

onSubmitDO(){
  debugger
    var params = {
      salesorder_date: this.deliveryform.value.salesorder_date,
      customer_name:this.deliveryform.value.customer_name,
      customer_branch:this.deliveryform.value.customer_branch,
      customercontact_names:this.deliveryform.value.customercontact_names,
      customer_mobile:this.deliveryform.value.customer_mobile,
      customer_email:this.deliveryform.value.customer_email,
      customer_address:this.deliveryform.value.customer_address,
      customer_address_so:this.deliveryform.value.customer_address_so,
      so_referencenumber:this.deliveryform.value.so_referencenumber,
      salesorderdtl_gid1:this.deliveryform.value.salesorderdtl_gid,
      customer_mode:this.deliveryform.value.customer_mode,
      customer_gid:this.deliveryform.value.customer_gid,
      dc_no:this.deliveryform.value.dc_no,
      tracker_id:this.deliveryform.value.tracker_id,
      template_content:this.deliveryform.value.template_content,
      no_of_boxs:this.deliveryform.value.no_of_boxs,
      dc_note:this.deliveryform.value.dc_note,
      qty_issued:this.raisedeliveryorder_list1[0].qty_issued,
      salesorderdtl_gid:this.raisedeliveryorder_list1[0].salesorderdtl_gid,
      uom_name:this.raisedelivery_list[0].uom_name,
      available_quantity:this.raisedeliveryorder_list1[0].available_quantity,
      product_gid:this.raisedeliveryorder_list1[0].product_gid,
      salesorder_gid:this.raisedeliveryorder_list1[0].salesorder_gid,
      stock_gid:this.raisedeliveryorder_list1[0].stock_gid,
    }
        var url='ImsTrnDeliveryorderSummary/PostDeliveryOrderSubmit'
        this.NgxSpinnerService.show()
   
        this.service.post(url, params).subscribe((result: any) => {
          if(result.status == false){
            this.ToastrService.warning(result.message)    
            this.NgxSpinnerService.hide()
          }
          else{
            this.ToastrService.success(result.message)
             this.route.navigate(['/ims/ImsTrnDeliveryorder']);
             this.NgxSpinnerService.hide()
          }      
        });
      
}
onclose(){
  this.deliveryform1.reset();
}
}





