import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

interface Customeren {
  enquiry_gid: string,
  customer_name: string,
  customercontact_gid: string,
  product_gid: string,
  customer_gid: string,
  enquiry_date: string,
  branch_name: string,
  enquiry_referencenumber: string,
  contact_number: string,
  customercontact_name: string,
  contact_email: string,
  enquiry_remarks: string,
  contact_address: string,
  customer_requirement: string,
  landmark: string,
  closure_date: string,
  product_name: string,
  product_code: string,
  productuom_name: string,
  productgroup_name: string,
  customerproduct_code: string,
  qty_requested: string,
  potential_value: string,
  product_requireddate: string,
  customerbranch_name: string,
  user_firstname: string,
  product_requireddateremarks: string,
  display_field: string,
  customer_rating: string,
  branch_gid: string,
  employee_gid: string;
}
@Component({
  selector: 'app-smr-trn-customerraiseenquiry',
  templateUrl: './smr-trn-customerraiseenquiry.component.html',
  styleUrls: ['./smr-trn-customerraiseenquiry.component.scss']
})
export class SmrTrnCustomerraiseenquiryComponent {

  productsearch: any;
  productcodesearch: any;
  private unsubscribe: Subscription[] = [];
  file!: File;
  filteredSOProductList: any[] = [];
  SOProductList: any[] = [];
  PostAll: any;
  combinedFormData: FormGroup | any;
  productform: FormGroup;
  ProductEdit: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  customer_list: any;
  Lead_list: any;
  products_list: any;
  products: any[] = [];
  branch_list: any[] = [];
  assign_list: any[] = [];
  enquiry_list: any[] = [];
  response_data: any;
  mdlcus: any;
  tomorrowDate: any;
  mdlEnq: any;
  mdlBranch: any;
  mdlEmployee: any;
  GetproductsCode: any;
  mdlproduct: any;
  uom_list: any;
  customeren!: Customeren
  POproductlist: any;
  todayDate: any;
  productname_list: any[] = [];
  productgrp_list: any[] = [];
  directeditenquiry_list: any[] = [];
  products_list1: any;
  products_unit: any;
  parameterValue1: any;
  leadbank_gid: any;
  enquiry: any;
  txtProductGroup: any;
  txtProductCode: any;
  txtUnit: any;
  txtCustomerBranch: any;
  txtContactPerson: any;
  txtContactNumber: any;
  productcodesearch1: any;
  txtEmailid: any;
  txtAddress: any;
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  decryt_leadbank_gid: any;
  customegid: any;
  decryt_appointment_gid: any;
  appointment_gid: any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router,
    private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {

    this.productform = new FormGroup({
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      productuom_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      customerproduct_code: new FormControl(''),
      qty_requested: new FormControl(''),
      potential_value: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      display_field: new FormControl(''),
      tmpsalesenquiry_gid: new FormControl('')


    });

    this.combinedFormData = new FormGroup({
      enquiry_gid: new FormControl(''),
      customer_name: new FormControl(''),
      customercontact_gid: new FormControl(''),
      product_gid: new FormControl(''),
      customer_gid: new FormControl(''),
      enquiry_date: new FormControl(this.getCurrentDate()),
      branch_name: new FormControl(''),
      branch_gid: new FormControl(''),
      enquiry_referencenumber: new FormControl(''),
      contact_number: new FormControl(''),
      customercontact_name: new FormControl(''),
      contact_email: new FormControl(''),
      enquiry_remarks: new FormControl(''),
      contact_address: new FormControl(''),
      customer_requirement: new FormControl(''),
      landmark: new FormControl(''),
      closure_date: new FormControl(this.getTomorrowDate()),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      productuom_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      customerproduct_code: new FormControl(''),
      qty_requested: new FormControl(''),
      potential_value: new FormControl(''),
      product_requireddate: new FormControl(''),
      customerbranch_name: new FormControl(''),
      user_firstname: new FormControl(''),
      product_requireddateremarks: new FormControl(this.getCurrentDate()),
      display_field: new FormControl(''),
      customer_rating: new FormControl(''),
      employee_gid: new FormControl(''),
      leadbank_name: new FormControl(''),
      leadbank_gid: new FormControl(''),


      leadbankbranch_name: new FormControl(''),
      leadbankcontact_name: new FormControl('')

    });

  }


  ngOnInit(): void {
    debugger
    const secretKey = 'storyboarderp';
    this.decryt_leadbank_gid = sessionStorage.getItem('CRM_LEADBANK_GID_ENQUIRY');
    this.decryt_appointment_gid = sessionStorage.getItem('CRM_APPOINTMENT_GID');
    
    if (this.decryt_leadbank_gid != null && this.decryt_appointment_gid != null) {
      this.leadbank_gid = AES.decrypt(this.decryt_leadbank_gid, secretKey).toString(enc.Utf8);
      this.appointment_gid = AES.decrypt(this.decryt_appointment_gid, secretKey).toString(enc.Utf8);
      if (this.leadbank_gid != null && this.appointment_gid != null) {
        this.GetOnChangeLeadName();
      }
    }
    else if (this.decryt_leadbank_gid != null ) {
      this.leadbank_gid = AES.decrypt(this.decryt_leadbank_gid, secretKey).toString(enc.Utf8);
      this.GetOnChangeLeadName();
      var api = 'SmrTrnCustomerEnquiry/GetLead';
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.Lead_list = this.responsedata.GetLeadname;
      });
    }
    else {
      sessionStorage.removeItem('CRM_APPOINTMENT_GID');
      sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
      var api = 'SmrTrnCustomerEnquiry/GetLead';
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.Lead_list = this.responsedata.GetLeadname;
      });
    }
    this.POproductsummary();
    this.productSearch();
    this.tomorrowDate = this.getTomorrowDate();

    const options: Options = {
      dateFormat: 'd-m-Y',

      minDate: this.tomorrowDate,

    };
    flatpickr('.date-picker', options);


    var api = 'SmrTrnCustomerEnquiry/GetProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list1 = this.responsedata.GetProducts;

    });


    // var api = 'SmrTrnCustomerEnquiry/GetCustomer';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.customer_list = this.responsedata.GetCustomername;


    // });


    var url = 'SmrTrnCustomerEnquiry/GetBranchDet'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDet;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);

    });

    var url = 'SmrTrnCustomerEnquiry/GetEmployeePerson'
    this.service.get(url).subscribe((result: any) => {
      this.assign_list = result.GetEmployeePerson;
    });

  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }


  getTomorrowDate(): string {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    const dd = String(tomorrow.getDate()).padStart(2, '0');
    const mm = String(tomorrow.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = tomorrow.getFullYear();

    // Set Flatpickr options
    return dd + '-' + mm + '-' + yyyy;
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
  get enquiry_date() {
    return this.combinedFormData.get('enquiry_date')!;
  }
  get customer_name() {
    return this.combinedFormData.get('customer_name')!;
  }
  get leadbank_name() {
    return this.combinedFormData.get('leadbank_name')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get qty_requested() {
    return this.productform.get('qty_requested')!;
  }
  onadd() { }


  GetOnChangeCustomerName() {
    let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
    let param = {
      customercontact_gid: customercontact_gid
    }
    var url = 'SmrTrnCustomerEnquiry/GetOnChangeCustomerName';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      //this.cusraiseform.get("customercontact_gid")?.setValue(result.GetCustomer[0].customercontact_gid  );
      this.combinedFormData.get("customerbranch_name")?.setValue(result.GetCustomer[0].customerbranch_name);
      this.combinedFormData.get("contact_email")?.setValue(result.GetCustomer[0].contact_email);
      this.combinedFormData.get("customercontact_name")?.setValue(result.GetCustomer[0].customercontact_name);
      this.combinedFormData.get("contact_address")?.setValue(result.GetCustomer[0].contact_address);
      this.combinedFormData.get("contact_number")?.setValue(result.GetCustomer[0].contact_number);
      //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
      //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
      this.NgxSpinnerService.hide()
    });


  }


  GetOnChangeLeadName() {
    debugger
    let param;
    if (this.decryt_leadbank_gid != null && this.decryt_appointment_gid != null) {
      let leadbank_gid = this.leadbank_gid;
      param = {
        leadbank_gid: leadbank_gid
      }
      var url = 'SmrTrnCustomerEnquiry/GetOnChangeLead';
      this.NgxSpinnerService.show()
      this.service.getparams(url, param).subscribe((result: any) => {
        //this.cusraiseform.get("customercontact_gid")?.setValue(result.GetCustomer[0].customercontact_gid  );
        this.combinedFormData.get("leadbankbranch_name")?.setValue(result.GetLead[0].leadbankbranch_name);
        this.combinedFormData.get("contact_email")?.setValue(result.GetLead[0].contact_email);
        this.combinedFormData.get("leadbankcontact_name")?.setValue(result.GetLead[0].leadbankcontact_name);
        this.combinedFormData.get("contact_address")?.setValue(result.GetLead[0].contact_address);
        this.combinedFormData.get("contact_number")?.setValue(result.GetLead[0].contact_number);
        this.combinedFormData.get("leadbank_name")?.setValue(result.GetLead[0].leadbank_name);
        this.combinedFormData.get("leadbank_gid")?.setValue(result.GetLead[0].leadbank_gid);
        //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
        //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
        this.NgxSpinnerService.hide();
      });
    }
    else if (this.decryt_leadbank_gid != null) {
      const key = 'storyboarderp';
      this.customegid = AES.decrypt(this.decryt_leadbank_gid, key).toString(enc.Utf8);
      param = {
        customer_gid: this.customegid
      }
      var url = 'SmrTrnSalesorder/GetOnChangeCustomer';
      this.service.getparams(url, param).subscribe((result :any) => {
        this.combinedFormData.get("leadbankbranch_name")?.setValue(result.GetCustomer[0].customer_name);
        this.combinedFormData.get("contact_email")?.setValue(result.GetCustomer[0].customer_email);
        this.combinedFormData.get("leadbankcontact_name")?.setValue(result.GetCustomer[0].customercontact_names);
        this.combinedFormData.get("contact_address")?.setValue(result.GetCustomer[0].address1);
        this.combinedFormData.get("contact_number")?.setValue(result.GetCustomer[0].customer_mobile);
        this.combinedFormData.get("leadbank_name")?.setValue(result.GetCustomer[0].customer_name);
        this.combinedFormData.get("leadbank_gid")?.setValue(result.GetCustomer[0].customer_gid);
      });
    }
    else {
      let leadbank_gid = this.combinedFormData.value.leadbank_name.leadbank_gid;
      param = {
        leadbank_gid: leadbank_gid
      }

      var url = 'SmrTrnCustomerEnquiry/GetOnChangeLead';
      this.NgxSpinnerService.show()
      this.service.getparams(url, param).subscribe((result: any) => {
        //this.cusraiseform.get("customercontact_gid")?.setValue(result.GetCustomer[0].customercontact_gid  );
        this.combinedFormData.get("leadbankbranch_name")?.setValue(result.GetLead[0].leadbankbranch_name);
        this.combinedFormData.get("contact_email")?.setValue(result.GetLead[0].contact_email);
        this.combinedFormData.get("leadbankcontact_name")?.setValue(result.GetLead[0].leadbankcontact_name);
        this.combinedFormData.get("contact_address")?.setValue(result.GetLead[0].contact_address);
        this.combinedFormData.get("contact_number")?.setValue(result.GetLead[0].contact_number);
        this.combinedFormData.get("leadbank_name")?.setValue(result.GetLead[0].leadbank_name);
        this.combinedFormData.get("leadbank_gid")?.setValue(result.GetLead[0].leadbank_gid);
        //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
        //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
        this.NgxSpinnerService.hide();
      });
    }
  }
  OnClearCustomerName() {
    this.txtCustomerBranch = '';
    this.txtContactPerson = '';
    this.txtContactNumber = '';
    this.txtEmailid = '';
    this.txtAddress = '';
  }


  OnClearProduct() {
    this.txtProductGroup = '';
    this.txtProductCode = '';
    this.txtUnit = '';

  }

  GetOnChangeProductsName() {
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
      //this.cusraiseform.value.productuom_gid = result.GetProductsName[0].productuom_gid
      this.NgxSpinnerService.hide()
    });

  }
  productSubmit() {
    //console.log(this.SOProductList)
    //console.log(this.productform.value.product_name)
    const product = this.SOProductList.find(item => item.product_gid === this.productform.value.product_name);
    // If product is found, return its product_name, otherwise return null or handle appropriately
    //console.log(product.product_name)
    var params = {
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
    debugger

    var api = 'SmrTrnCustomerEnquiry/PostOnAdds'
    this.NgxSpinnerService.show()
    this.service.post(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.POproductsummary();
        window.scrollTo({

          top: 0,

        });
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success('Product Added Successfully!!')
        this.productform.reset();
        this.POproductsummary();
        this.NgxSpinnerService.hide()
      }
    });


  }
  POproductsummary() {
    var api = 'SmrTrnCustomerEnquiry/GetProductsSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.POproductlist = this.responsedata.productsummarys_list;
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

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'SmrTrnCustomerEnquiry/GetDeleteEnquiryProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpsalesenquiry_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {

        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()

      }
      this.POproductsummary();
    });
  }
  editprod(tmpsalesenquiry_gid: any) {
    debugger
    var url = 'SmrTrnCustomerEnquiry/GetDirectEnquiryEditProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpsalesenquiry_gid: tmpsalesenquiry_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.directeditenquiry_list = result.directeditenquiry_list;
      this.productform.get("tmpsalesenquiry_gid")?.setValue(this.directeditenquiry_list[0].tmpsalesenquiry_gid);
      this.productform.get("product_name")?.setValue(this.directeditenquiry_list[0].product_name);
      this.productcodesearch = this.directeditenquiry_list[0].product_gid;
      this.productform.get("product_code")?.setValue(this.directeditenquiry_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(this.directeditenquiry_list[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(this.directeditenquiry_list[0].productgroup_name);
      this.productform.get("qty_requested")?.setValue(this.directeditenquiry_list[0].qty_requested);
      this.productform.get("potential_value")?.setValue(this.directeditenquiry_list[0].potential_value);
      this.productform.get("product_requireddate")?.setValue(this.directeditenquiry_list[0].product_requireddate);
      this.NgxSpinnerService.hide()
      this.showUpdateButton = true;
      this.showAddButton = false;
    });
    console.log(this.productform.value)
  }

  onupdate() {
    debugger
    const product = this.SOProductList.find(item => item.product_gid === this.productform.value.product_name);
    //  console.log(product.product_name)
    //  console.log(this.productform.value)
    var params = {
      tmpsalesenquiry_gid: this.productform.value.tmpsalesenquiry_gid,
      productgroup_name: this.productform.value.productgroup_name,
      product_code: this.productform.value.product_code,
      product_name: product.product_name,
      product_gid: this.productform.value.product_name,
      productuom_name: this.productform.value.productuom_name,
      qty_requested: this.productform.value.qty_requested,
      potential_value: this.productform.value.potential_value,
      product_requireddate: this.productform.value.product_requireddate,
      productgroup_gid: this.productform.value.productgroup_gid,
      productuom_gid: this.productform.value.productuom_gid,

    }
    var url = 'SmrTrnCustomerEnquiry/PostUpdateEnquiryProduct'
    this.NgxSpinnerService.show()
    this.service.post(url, params).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.POproductsummary();
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success(result.message)
        this.productform.reset();
        this.NgxSpinnerService.hide()

      }
      this.POproductsummary();
    });

    this.showAddButton = true;
    this.showUpdateButton = false;
  }

  onSubmit() {
    let params;
    if (this.decryt_leadbank_gid != null && this.decryt_appointment_gid != null) {
      params = {
        branch_name: this.combinedFormData.value.branch_name,
        enquiry_date: this.combinedFormData.value.enquiry_date,
        enquiry_gid: this.combinedFormData.value.enquiry_gid,
        leadbank_name: this.combinedFormData.value.leadbank_name,
        leadbankcontact_name: this.combinedFormData.value.leadbankcontact_name,
        contact_email: this.combinedFormData.value.contact_email,
        enquiry_referencenumber: this.combinedFormData.value.enquiry_referencenumber,
        contact_number: this.combinedFormData.value.contact_number,
        enquiry_remarks: this.combinedFormData.value.enquiry_remarks,
        contact_address: this.combinedFormData.value.contact_address,
        customer_requirement: this.combinedFormData.value.customer_requirement,
        landmark: this.combinedFormData.value.landmark,
        closure_date: this.combinedFormData.value.closure_date,
        user_firstname: this.combinedFormData.value.user_firstname,
        customer_rating: this.combinedFormData.value.customer_rating,
        customerbranch_name: this.combinedFormData.value.customerbranch_name,
        leadbank_gid: this.leadbank_gid,
        customer_gid: this.leadbank_gid,
      }
    }
    else {
      params = {
        branch_name: this.combinedFormData.value.branch_name,
        enquiry_date: this.combinedFormData.value.enquiry_date,
        enquiry_gid: this.combinedFormData.value.enquiry_gid,
        leadbank_name: this.combinedFormData.value.leadbank_name,
        leadbankcontact_name: this.combinedFormData.value.leadbankcontact_name,
        contact_email: this.combinedFormData.value.contact_email,
        enquiry_referencenumber: this.combinedFormData.value.enquiry_referencenumber,
        contact_number: this.combinedFormData.value.contact_number,
        enquiry_remarks: this.combinedFormData.value.enquiry_remarks,
        contact_address: this.combinedFormData.value.contact_address,
        customer_requirement: this.combinedFormData.value.customer_requirement,
        landmark: this.combinedFormData.value.landmark,
        closure_date: this.combinedFormData.value.closure_date,
        user_firstname: this.combinedFormData.value.user_firstname,
        customer_rating: this.combinedFormData.value.customer_rating,
        customerbranch_name: this.combinedFormData.value.customerbranch_name,
        leadbank_gid: this.combinedFormData.value.leadbank_gid,
      }
    }
    console.log('ehdbjwkendkew', params)
    if (this.enquiry_date.value != null && this.enquiry_date.value != ""
      && this.leadbank_name.value != null && this.leadbank_name.value != "") {
      var url = 'SmrTrnCustomerEnquiry/PostCustomerEnquiry'
      this.NgxSpinnerService.show()

      this.service.postparams(url, params).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide()

        }
        else {
          this.ToastrService.success('Enquiry Has Been Successfully Raised!')
          sessionStorage.removeItem('CRM_APPOINTMENT_GID');
          sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
          window.history.back();
          this.NgxSpinnerService.hide()
        }

      });
    }
    else {
      this.ToastrService.warning('Kindly Fill Mandatory Fields!')
    }
  }

  onclose() {

    window.history.back();
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
  }
  openModaledit() { }
}

