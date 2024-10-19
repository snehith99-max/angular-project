import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { PmrTrnPurchaseRequisitionComponent } from 'src/app/ems.pmr/Component/pmr-trn-purchase-requisition/pmr-trn-purchase-requisition.component';

interface Customeren {
  enquiry_gid: string,
  customer_name : string,
  customercontact_gid : string,
  product_gid : string,
  customer_gid:string,
  enquiry_date : string,
  branch_name : string,
  enquiry_referencenumber : string,
  contact_number : string,
  customercontact_name : string,
  contact_email : string,
  enquiry_remarks : string,
  contact_address : string,
  customer_requirement : string,
  landmark : string,
  closure_date : string,
  product_name : string,
  product_code : string,
  productuom_name : string,
  productgroup_name : string,
  customerproduct_code : string,
  qty_requested : string,
  potential_value : string,
  product_requireddate : string,
  customerbranch_name: string,
  user_firstname: string,
  product_requireddateremarks: string,
  display_field: string,
  customer_rating: string,
  branch_gid: string,
  employee_gid: string;
}

@Component({
  selector: 'app-smr-trn-customerenquiryedit',
  templateUrl: './smr-trn-customerenquiryedit.component.html',
  styleUrls: ['./smr-trn-customerenquiryedit.component.scss']
})
export class SmrTrnCustomerenquiryeditComponent {
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  private unsubscribe: Subscription[] = [];
  file!:File;

  PostAll: any;
  cuseditform: FormGroup | any;
  productform: FormGroup;
  ProductEdit: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  customer_list: any;
  products_list: any;
  products: any[] = [];
  branch_list: any[] = [];
  assign_list: any[]=[];
  editenquiry_list: any[]=[];
  enquiryprod_list: any [] = [];
  response_data :any;
  mdlsales:any;
  mdlcus:any;
  mdlEnq:any;
  mdlBranch:any;
  mdlEmployee:any;
  mdlproduct:any;
  enqui_gid:any;
  GetCustomerDet:any; 
  uom_list:any;
  customeren!: Customeren
  POproductlist: any;
  productname_list : any[] = [];
  productgrp_list : any[] = [];
  products_list1: any;
  products_unit: any;
  parameterValue1: any;
  txtProductGroup:any;
  txtProductCode:any;
  txtProductUnit:any;
  enquiry: any;
  MdlRate:any;
  tmpsalesenquiry_gid: any;
  enquirysummary_list: any;
  enquiry_gid: any;
  productsummary_list:any[]=[];
  filteredSOProductList: any[] = [];
  SOProductList: any[] = [];
  productsearch: any;
  productcodesearch: any;
  productcodesearch1:any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,
    private service: SocketService,private ToastrService: ToastrService, public NgxSpinnerService : NgxSpinnerService) {
    
    this.productform = new FormGroup({
      product_gid : new FormControl(''),
      product_name : new FormControl(''),
      product_code : new FormControl(''),
      tmpsalesenquiry_gid: new FormControl(''),
      productuom_name : new FormControl(''),
      productgroup_name : new FormControl(''),
      customerproduct_code : new FormControl(''),
      qty_requested : new FormControl(''),
      potential_value : new FormControl(''),
      product_requireddate : new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      display_field: new FormControl('')


    });

    this.cuseditform = new FormGroup({
      enquiry_gid: new FormControl(''),
      customer_name : new FormControl(''),
      customercontact_gid : new FormControl(''),
      contact_person: new FormControl(''),
      assign_to : new FormControl(''),
      product_gid : new FormControl(''),
      customer_gid:new FormControl(''),
      customer_branch: new FormControl(''),
      enquiry_date : new FormControl(''),
      branch_name : new FormControl(''),
      branch_gid: new FormControl(''),
      enquiry_referencenumber : new FormControl(''),
      contact_number : new FormControl(''),
      customercontact_name : new FormControl(''),
      contact_email : new FormControl(''),
      enquiry_remarks : new FormControl(''),
      contact_address : new FormControl(''),
      customer_requirement : new FormControl(''),
      landmark : new FormControl(''),
      closure_date : new FormControl(''),
      product_name : new FormControl(''),
      product_code : new FormControl(''),
      productuom_name : new FormControl(''),
      productgroup_name : new FormControl(''),
      customerproduct_code : new FormControl(''),
      qty_requested : new FormControl(''),
      potential_value : new FormControl(''),
      product_requireddate : new FormControl(''),
      customerbranch_name: new FormControl(''),
      user_firstname: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      display_field: new FormControl(''),
      customer_rating: new FormControl(''),
      employee_gid: new FormControl('')
    });
     
  } 
  
  

  ngOnInit(): void {
    this.productSearch();
    this.Productsummary();
      const options: Options = {
        dateFormat: 'd-m-Y' ,    
      };
      flatpickr('.date-picker', options);
    const enquiry_gid =this.route.snapshot.paramMap.get('enquiry_gid');
    this.enquiry= enquiry_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.enquiry,secretKey).toString(enc.Utf8);
    this.enqui_gid=deencryptedParam;
    this.GetViewcustomerSummary(deencryptedParam);
  

  

    var api = 'SmrTrnCustomerEnquiry/GetProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list1 = this.responsedata.GetProducts;

    });

    var url = 'SmrTrnCustomerEnquiry/GetBranchDet'
this.service.get(url).subscribe((result:any)=>{
  this.branch_list = result.GetBranchDet;
 });

 var url = 'SmrTrnCustomerEnquiry/GetEmployeePerson'
 this.service.get(url).subscribe((result: any) => {
   this.assign_list = result.GetEmployeePerson;
 });

  
}
productSearch() {
  debugger
  //let customer_gid = this.combinedFormData.get("customer_name")?.value;
  // var params = {
  //   producttype_gid: this.productform.value.producttype_name,
  //   product_name: this.productform.value.product_name,
  //   customer_gid: customer_gid.customer_gid
  // };
  var api = 'SmrTrnSalesorder/GetProductsearchSummarySales';
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.SOProductList = this.responsedata.GetProductsearchs;
    this.filteredSOProductList = this.SOProductList;
    //this.GetTaxSegmentList = this.responsedata.GetTaxSegmentListorder;
  });
}
OnProductCode(event: any) {
  const product_code = this.SOProductList.find(product => product.product_code === event.product_code);
  if (product_code) {
    this.productform.patchValue({
      product_name: product_code.product_gid,
      productgroup_name: product_code.productgroup_name
    });
  }
  this.GetOnChangeProductsName();
}
onclearproductcode() {
  this.productsearch = null;
  this.productcodesearch1 = null;
}
onclearproduct() {
  // this.productform.get("product_code").setValue('');
  // this.productform.get("productgroup_name").setValue('');
  this.productcodesearch = null;
}
onProductSelect(event: any): void {
  debugger

  const product_name = this.SOProductList.find(product => product.product_gid === event.product_gid);
  if (product_name) {
    this.productform.patchValue({
      product_code: product_name.product_code,
      productgroup_name: product_name.productgroup_name
    });
  }
  this.GetOnChangeProductsName();
}
GetViewcustomerSummary(enquiry_gid: any) {
  debugger
  var url='SmrCustomerEnquiryEdit/GetEditCustomerEnquirySummary'
  this.NgxSpinnerService.show()
  let param = {
    enquiry_gid : enquiry_gid 
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
    this.editenquiry_list = result.editenquiry_list;
    this.cuseditform.get("enquiry_gid")?.setValue(this.editenquiry_list[0].enquiry_gid); 
    this.cuseditform.get("enquiry_date")?.setValue(this.editenquiry_list[0].enquiry_date); 
    this.cuseditform.get("branch_name")?.setValue(this.editenquiry_list[0].branch_name); 
    this.cuseditform.get("branch_gid")?.setValue(this.editenquiry_list[0].branch_gid); 
    this.cuseditform.get("enquiry_referencenumber")?.setValue(this.editenquiry_list[0].enquiry_referencenumber); 
    this.cuseditform.get("customer_name")?.setValue(this.editenquiry_list[0].customer_name);  
    this.cuseditform.get("customer_branch")?.setValue(this.editenquiry_list[0].customer_branch);  
    this.cuseditform.get("contact_number")?.setValue(this.editenquiry_list[0].contact_number); 
    this.cuseditform.get("contact_person")?.setValue(this.editenquiry_list[0].contact_person); 
    this.cuseditform.get("contact_email")?.setValue(this.editenquiry_list[0].contact_email); 
    this.cuseditform.get("enquiry_remarks")?.setValue(this.editenquiry_list[0].enquiry_remarks); 
    this.cuseditform.get("contact_address")?.setValue(this.editenquiry_list[0].contact_address); 
    this.cuseditform.get("customer_requirement")?.setValue(this.editenquiry_list[0].customer_requirement); 
    this.cuseditform.get("landmark")?.setValue(this.editenquiry_list[0].landmark); 
    this.cuseditform.get("closure_date")?.setValue(this.editenquiry_list[0].closure_date);
    this.cuseditform.get("assign_to")?.setValue(this.editenquiry_list[0].assign_to);
    this.MdlRate = this.editenquiry_list[0].customer_rating;
    this.cuseditform.get("display_field")?.setValue(this.editenquiry_list[0].display_field);
    this.cuseditform.get("contact_details")?.setValue(this.editenquiry_list[0].contact_details);
    this.cuseditform.get("product_name")?.setValue(this.editenquiry_list[0].product_name);
    this.cuseditform.get("productgroup_name")?.setValue(this.editenquiry_list[0].productgroup_name);
    this.cuseditform.get("qty_requested")?.setValue(this.editenquiry_list[0].qty_requested);
    this.cuseditform.get("potential_value")?.setValue(this.editenquiry_list[0].potential_value);
    this.responsedata=result.editproductsummary_list;
    this.Productsummary();
    this.NgxSpinnerService.hide()
  });

  
}
get product_code() {
  return this.productform.get('product_code')!;
}
get enquiry_date ()
{
  return this.cuseditform.get('enquiry_date')!;
}
get customer_name ()
{
  return this.cuseditform.get('customer_name')!;
}
get user_firstname ()
{
  return this.cuseditform.get('user_firstname')!;
}
get product_name ()
{
  return this.productform.get('product_name')!;
}
get qty_requested ()
{
  return this.productform.get('qty_requested')!;
}

OnClearProduct()
{
  this.txtProductGroup='';
  this.txtProductCode='';
  this.txtProductUnit='';
}

  GetOnChangeCustomerName(){
    debugger
    let customercontact_gid = this.cuseditform.value.customer_name;
    let param ={
      customercontact_gid :customercontact_gid
    }
    var url = 'SmrTrnCustomerEnquiry/GetOnEditCustomerName';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomer;
      //this.cusraiseform.get("customercontact_gid")?.setValue(result.GetCustomer[0].customercontact_gid  );
      this.cuseditform.get("customerbranch_name")?.setValue(result.GetCustomer[0].customerbranch_name);
      this.cuseditform.get("contact_email")?.setValue(result.GetCustomer[0].contact_email);
      this.cuseditform.get("customer_person")?.setValue(result.GetCustomer[0].customercontact_name);
      this.cuseditform.get("contact_address")?.setValue(result.GetCustomer[0].contact_address);
      this.cuseditform.get("contact_number")?.setValue(result.GetCustomer[0].contact_number);
 
    });

  }
  
  GetOnChangeProductsName() {
    debugger;
    let product_gid = this.productform.value.product_name;
    let param = {
      product_gid: product_gid
    }
    var url = 'SmrTrnCustomerEnquiry/GetOnChangeProductsName';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("product_code")?.setValue(result.GetProductsName[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.GetProductsName[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.GetProductsName[0].productgroup_name);
      this.productform.value.productgroup_gid = result.GetProductsName[0].productgroup_gid
      this.NgxSpinnerService.hide()

    });

  }
  

  
  productSubmit(){
    const product = this.SOProductList.find(item => item.product_gid === this.productform.value.product_name);
       
    var params = {
      enquiry_gid:this.enqui_gid,
      productgroup_name: this.productform.value.productgroup_name,
      customerproduct_code: this.productform.value.customerproduct_code,
      product_code: this.productform.value.product_code,
      product_name: product.product_name,
      productuom_name: this.productform.value.productuom_name,
      qty_requested: this.productform.value.qty_requested,
      potential_value: this.productform.value.potential_value,
      product_requireddate: this.productform.value.product_requireddate,
      product_gid: this.productform.value.product_name.product_gid,
      productgroup_gid: this.productform.value.productgroup_gid,
      productuom_gid: this.productform.value.productuom_gid,
     
}
    console.log(params)
    var api = 'SmrCustomerEnquiryEdit/PostProductEnquiryEdit'
    this.NgxSpinnerService.show()
    this.service.post(api, params).subscribe((result: any) => {
     this.Productsummary();
    this.productform.reset();
    this.NgxSpinnerService.hide()
    },
    );
  }
  Productsummary() {
    debugger
    const enquiry_gid =this.route.snapshot.paramMap.get('enquiry_gid');
    this.enquiry= enquiry_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.enquiry,secretKey).toString(enc.Utf8);
    this.enqui_gid=deencryptedParam;
   
    var params = {
      enquiry_gid:this.enqui_gid,
    }
    var url = 'SmrCustomerEnquiryEdit/EditCustomerProductSummary'
  
    this.service.getparams(url,params).subscribe((result: any) => {
      
      this.responsedata = result;
      this.enquiryprod_list = this.responsedata.editproductsummary_list;
      
    });

  }

  openModaldelete(parameter: string){
    this.parameterValue = parameter
    
}

ondelete(){

  var url = 'SmrCustomerEnquiryEdit/DeleteProductSummary'
  this.NgxSpinnerService.show()
  let param = {
    tmpsalesenquiry_gid : this.parameterValue,
    }
  this.service.getparams(url,param).subscribe((result: any) => {
   this.response_data=result;
    if(result.status == false){
      this.ToastrService.warning(result.message)
      this.Productsummary();
      this.NgxSpinnerService.hide()
    }
    else{
      
      this.ToastrService.success(result.message)
      this.Productsummary();
      this.NgxSpinnerService.hide()
    }
  });
}


onSubmit() {
  debugger
  var params = {
    branch_name:this.cuseditform.value.branch_name,
    branch_gid: this.cuseditform.value.branch_gid,
    enquiry_date:this.cuseditform.value.enquiry_date,
    enquiry_gid:this.enqui_gid,
    customer_rating: this.cuseditform.value.customer_rating,
    customer_name:this.cuseditform.value.customer_name,
    customer_gid:this.cuseditform.value.customer_gid,
    customercontact_name: this.cuseditform.value.customercontact_name,
    contact_email: this.cuseditform.value.contact_email,
    contact_person: this.cuseditform.value.contact_person,
    enquiry_referencenumber:this.cuseditform.value.enquiry_referencenumber,
    contact_number: this.cuseditform.value.contact_number,
    enquiry_remarks: this.cuseditform.value.enquiry_remarks,
    contact_address: this.cuseditform.value.contact_address,
    customer_requirement: this.cuseditform.value.customer_requirement,
    customerbranch_gid: this.cuseditform.value.customerbranch_gid,
    landmark : this.cuseditform.value.landmark,
    closure_date: this.cuseditform.value.closure_date,
    assign_to: this.cuseditform.value.assign_to,
    customerbranch_name: this.cuseditform.value.customerbranch_name,
    campaign_gid:this.cuseditform.value.campaign_gid,
    contact_details:this.cuseditform.value.contact_details,
    potorder_value:this.cuseditform.value.potorder_value,

  }
  
  var url='SmrCustomerEnquiryEdit/PostCustomerEnquiryEdit'
  this.NgxSpinnerService.show()
  if (this.cuseditform.value.enquiry_date != null && this.cuseditform.value.branch_name != '',
  this.cuseditform.value.customer_name != '') {
    
  this.service.postparams(url, params).subscribe((result: any) => {
    this.response_data=result;
    if(result.status == false){
      this.ToastrService.warning(result.message)
      this.Productsummary();
      this.NgxSpinnerService.hide()
    }
    else{
      
      this.ToastrService.success(result.message)
      this.router.navigate(['/smr/SmrTrnCustomerenquirySummary']);
      this.NgxSpinnerService.hide()
    }
   
  });
}
      
}



onclose(){
  
  this.router.navigate(['/smr/SmrTrnCustomerenquirySummary']);
}

onedit(tmpsalesenquiry_gid: any) {
  var url = 'SmrCustomerEnquiryEdit/EditProductSummary'
  let param = {
    tmpsalesenquiry_gid: tmpsalesenquiry_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.productsummary_list = result.editproductsummary_list;
    this.productform.get("tmpsalesenquiry_gid")?.setValue(this.productsummary_list[0].tmpsalesenquiry_gid);
    this.productform.get("qty_requested")?.setValue(this.productsummary_list[0].qty_requested);
    this.productform.get("product_requireddate")?.setValue(this.productsummary_list[0].product_requireddate);
    this.productform.get("productgroup_name")?.setValue(this.productsummary_list[0].productgroup_name);
    this.productform.get("product_code")?.setValue(this.productsummary_list[0].product_code);
    this.productform.get("product_name")?.setValue(this.productsummary_list[0].product_name);
    this.productform.get("productuom_name")?.setValue(this.productsummary_list[0].productuom_name);
    this.productcodesearch=this.productsummary_list[0].product_gid;
    this.productform.get("potential_value")?.setValue(this.productsummary_list[0].potential_value);
  });
  this.showUpdateButton = true;
  this.showAddButton = false;
}
onupdateprod(){
  debugger
  const product = this.SOProductList.find(item => item.product_gid === this.productform.value.product_name);
      //  console.log(product.product_name)
      //  console.log(this.productform.value)
  var params = {
    tmpsalesenquiry_gid: this.productform.value.tmpsalesenquiry_gid,
    qty_requested: this.productform.value.qty_requested,
    product_name: product.product_name,
    product_gid: this.productform.value.product_name,
    productuom_name: this.productform.value.productuom_name,
    potential_value: this.productform.value.potential_value,
    product_code: this.productform.value.product_code,
    productgroup_name: this.productform.value.productgroup_name,
    product_requireddate: this.productform.value.product_requireddate,
   
  }
  var url = 'SmrCustomerEnquiryEdit/PostEnquiryUpdateProduct'
  this.service.postparams(url, params).pipe().subscribe((result: any) => {
    this.responsedata = result;
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.ToastrService.success(result.message)
      this.productform.reset();

    }
    this.Productsummary();
  });
  this.showAddButton = true;
  this.showUpdateButton = false;
}

}