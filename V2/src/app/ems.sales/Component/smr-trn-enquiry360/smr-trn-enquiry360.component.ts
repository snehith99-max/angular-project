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
  selector: 'app-smr-trn-enquiry360',
  templateUrl: './smr-trn-enquiry360.component.html',
  styleUrls: ['./smr-trn-enquiry360.component.scss']
})
export class SmrTrnEnquiry360Component {

  private unsubscribe: Subscription[] = [];
  file!: File;

  PostAll: any;
  combinedFormData: FormGroup | any;
  productform: FormGroup;
  ProductEdit: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  customer_list: any;
  products_list: any;
  products: any[] = [];
  branch_list: any[] = [];
  assign_list: any[] = [];
  enquiry_list: any[] = [];
  response_data: any;
  mdlcus: any;
  mdlEnq: any;
  mdlBranch: any;
  mdlEmployee: any;
  GetproductsCode: any;
  mdlproduct: any;
  uom_list: any;
  customeren!: Customeren
  POproductlist: any;
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
  txtEmailid: any;
  txtAddress: any;
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  lspage1: any;
  lspage: any;
  deencryptedParam: any;
  customer_gid: any;
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

    });

  }

  ngOnInit(): void {
    debugger
    this.POproductsummary();

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    const secretKey = 'storyboarderp';
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    this.deencryptedParam = customer_gid;
    this.customer_gid = AES.decrypt(this.deencryptedParam, secretKey).toString(enc.Utf8);
    this.GetCustomerDetails(this.customer_gid);

    var api = 'SmrTrnCustomerEnquiry/GetProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list1 = this.responsedata.GetProducts;

    });

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
    const appointment_gid = this.route.snapshot.paramMap.get('appointment_gid');
    this.appointment_gid = appointment_gid;
    const deencryptedParam3 = AES.decrypt(this.appointment_gid, secretKey).toString(enc.Utf8);
    this.appointment_gid = deencryptedParam3;
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
    return dd + '-' + mm + '-' + yyyy;
  }

  get enquiry_date() {
    return this.combinedFormData.get('enquiry_date')!;
  }
  get branch_name() {
    return this.combinedFormData.get('branch_name')!;
  }
  get customer_name() {
    return this.combinedFormData.get('customer_name')!;
  }
  get user_firstname() {
    return this.combinedFormData.get('user_firstname')!;
  }

  get product_name() {
    return this.productform.get('product_name')!;
  }

  get qty_requested() {
    return this.productform.get('qty_requested')!;

  }
  get potential_value() {
    return this.productform.get('potential_value')!;
  }

  GetCustomerDetails(customer_gid: any) {
    debugger
    let param = {
      customer_gid: customer_gid
    }
    var url = 'SmrTrnCustomerEnquiry/GetOnChangeCustomerName360';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.combinedFormData.get("customer_gid")?.setValue(result.GetCustomer[0].customer_gid);
      this.combinedFormData.get("customer_name")?.setValue(result.GetCustomer[0].customer_name);
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

  // GetOnChangeCustomerName() {
  //   let customer_gid = this.combinedFormData.value.customer_name.customer_gid;
  //   let param = {
  //     customer_gid: customer_gid
  //   }
  //   var url = 'SmrTrnCustomerEnquiry/GetOnChangeCustomerName360';
  //   this.NgxSpinnerService.show()
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     //this.cusraiseform.get("customercontact_gid")?.setValue(result.GetCustomer[0].customercontact_gid  );
  //     this.combinedFormData.get("customerbranch_name")?.setValue(result.GetCustomer[0].customerbranch_name);
  //     this.combinedFormData.get("contact_email")?.setValue(result.GetCustomer[0].contact_email);
  //     this.combinedFormData.get("customercontact_name")?.setValue(result.GetCustomer[0].customercontact_name);
  //     this.combinedFormData.get("contact_address")?.setValue(result.GetCustomer[0].contact_address);
  //     this.combinedFormData.get("contact_number")?.setValue(result.GetCustomer[0].contact_number);
  //     //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
  //     //this.cusraiseform.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
  //     this.NgxSpinnerService.hide()
  //   });


  // }
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
    let product_gid = this.productform.value.product_name.product_gid;
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
    var params = {
      productgroup_name: this.productform.value.productgroup_name,
      customerproduct_code: this.productform.value.customerproduct_code,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name.product_name,
      productuom_name: this.productform.value.productuom_name,
      qty_requested: this.productform.value.qty_requested,
      potential_value: this.productform.value.potential_value,
      product_requireddate: this.productform.value.product_requireddate,
      product_gid: this.productform.value.product_name.product_gid,
      productgroup_gid: this.productform.value.productgroup_gid,
      productuom_gid: this.productform.value.productuom_gid,
    }
    if (this.product_name.value != null && this.product_name.value != ""
      && this.qty_requested.value != null && this.qty_requested.value != ""
      && this.potential_value.value != null && this.potential_value.value != "") {
      var api = 'SmrTrnCustomerEnquiry/PostOnAdds'
      this.NgxSpinnerService.show()
      this.service.post(api, params).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning('Kindly Fill Mandatory Fields!!')
          this.POproductsummary();
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
    else {
      this.ToastrService.warning('Kindly Fill Mandatory Fields!!')
    }
  }
  POproductsummary() {
    var api = 'SmrTrnCustomerEnquiry/GetProductsSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.POproductlist = this.responsedata.productsummarys_list;
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
      this.productform.get("product_gid")?.setValue(this.directeditenquiry_list[0].product_gid);
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
    var params = {
      tmpsalesenquiry_gid: this.productform.value.tmpsalesenquiry_gid,
      productgroup_name: this.productform.value.productgroup_name,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name.product_name == undefined ? this.productform.value.product_name : this.productform.value.product_name.product_name,
      product_gid: this.productform.value.product_name.product_gid == undefined ? this.productform.value.product_gid : this.productform.value.product_name.product_gid,
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
    debugger
    var params = {
      branch_name: this.combinedFormData.value.branch_name,
      enquiry_date: this.combinedFormData.value.enquiry_date,
      enquiry_gid: this.combinedFormData.value.enquiry_gid,
      customer_name: this.combinedFormData.value.customer_name,
      customercontact_name: this.combinedFormData.value.customercontact_name,
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
      customer_gid: this.combinedFormData.value.customer_gid
    }
    if (this.enquiry_date.value != null && this.enquiry_date.value != ""
      && this.branch_name.value != null && this.branch_name.value != ""
      && this.customer_name.value != null && this.customer_name.value != ""
      && this.user_firstname.value != null && this.user_firstname.value != "") {
      var url = 'SmrTrnCustomerEnquiry/PostCustomerEnquiry'
      this.NgxSpinnerService.show()

      this.service.postparams(url, params).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();

        }
        else {
          this.ToastrService.success('Enquiry Has Been Successfully Raised!')
          this.Leadstage_update();
          window.history.back();
          this.NgxSpinnerService.hide();

        }

      });
    }
    else {
      this.ToastrService.warning('Kindly Fill Mandatory Fields!')
    }
  }
  Leadstage_update() {

    //this.reactiveForm2.get('appointment_gid')?.setValue(this.appointment_gid1);
    if (this.appointment_gid != null) {
      //this.reactiveForm2.get('call_response')?.setValue(sales_stages);
      let param = {
        call_response: "Evaluation",
        appointment_gid: this.appointment_gid,
      }
      var url = 'Leadbank360/PostLeadStage';
      this.service.post(url, param).pipe().subscribe((result: any) => {

      });
    }
  }
  onclose() {
    window.history.back();
  }


}

