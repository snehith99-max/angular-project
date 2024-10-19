import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
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
  selector: 'app-crm-trn-tcustomerraiseenquiry',
  templateUrl: './crm-trn-tcustomerraiseenquiry.component.html',
  styleUrls: ['./crm-trn-tcustomerraiseenquiry.component.scss']
})
export class CrmTrnTcustomerraiseenquiryComponent {
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
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  enquiry_list: any[] = [];
  response_data: any;
  mdlcus: any;
  ECRMProductSummarylist: any[] = [];
  mdlEnq: any;
  mdlBranch: any;
  mdlEmployee: any;
  mdlproduct: any;
  uom_list: any;
  customeren!: Customeren
  POproductlist: any;
  productname_list: any[] = [];
  productgrp_list: any[] = [];
  products_list1: any;
  products_unit: any;
  parameterValue1: any;
  leadbank_gid: any;
  enquiry: any;
  lead2campaign_gid: any;
  leadbankcontact_gid: any;
  lspage: any;
  lspage1: any;
  txtProductGroup:any;
  txtProductCode: any;
  txtUnit:any;
  txtCustomerBranch:any;
  txtContactPerson:any;
  txtContactNumber:any;
  txtEmailid:any;
  txtAddress:any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, 
    private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {

    this.productform = new FormGroup({
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      productuom_name: new FormControl(''),
      tmpsalesenquiry_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      customerproduct_code: new FormControl(''),
      qty_requested: new FormControl(''),
      potential_value: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      display_field: new FormControl('')


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
      product_requireddate: new FormControl(this.getCurrentDate()),
      customerbranch_name: new FormControl(''),
      user_firstname: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      display_field: new FormControl(''),
      customer_rating: new FormControl(''),
      employee_gid: new FormControl('')
    });

  }


  ngOnInit(): void {
    this.POproductsummary();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    var api = 'SmrCustomerEnquiry360/GetProductECRM';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list1 = this.responsedata.GetProductECRM;

    });
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    const leadbankcontact_gid = this.route.snapshot.paramMap.get('leadbankcontact_gid');
    const lead2campaign_gid = this.route.snapshot.paramMap.get('lead2campaign_gid');
    const lspage = this.route.snapshot.paramMap.get('lspage');

    this.leadbank_gid = leadbank_gid;
    this.leadbankcontact_gid = leadbankcontact_gid;
    this.lead2campaign_gid = lead2campaign_gid;
    this.lspage = lspage;
    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.leadbankcontact_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.lead2campaign_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam3 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);

    this.lspage1 = deencryptedParam3;

    console.log("leadbank_gid =" + deencryptedParam);
    console.log("leadbankcontact_gid = " + deencryptedParam1);
    console.log("lead2campaign_gid = " + deencryptedParam2);
    console.log("lspage=" + deencryptedParam3);

    if (deencryptedParam != null) {
      this.leadbank_gid = (deencryptedParam);
    }
    var api = 'SmrCustomerEnquiry360/GetCustomerECRM';
    let params = {
      leadbank_gid: deencryptedParam,
    }
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.customer_list = this.responsedata.GetCustomerECRM;
    });

    var url = 'SmrCustomerEnquiry360/GetBranchECRM'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchECRM;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
    });

    var url = 'SmrCustomerEnquiry360/GetSalesPersonECRM'
    this.service.get(url).subscribe((result: any) => {
      this.assign_list = result.GetSalesPersonECRM;
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
  onadd() { }


  GetOnChangeCustomerName() {
    let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
    let param = {
      customercontact_gid: customercontact_gid
    }
    var url = 'SmrCustomerEnquiry360/GetOnChangeCustomerECRM';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {      
      this.combinedFormData.get("customerbranch_name")?.setValue(result.GetOnchangeCustomer_ECRM[0].customerbranch_name);
      this.combinedFormData.get("contact_email")?.setValue(result.GetOnchangeCustomer_ECRM[0].contact_email);
      this.combinedFormData.get("customercontact_name")?.setValue(result.GetOnchangeCustomer_ECRM[0].customercontact_name);
      this.combinedFormData.get("contact_address")?.setValue(result.GetOnchangeCustomer_ECRM[0].contact_address);
      this.combinedFormData.get("contact_number")?.setValue(result.GetOnchangeCustomer_ECRM[0].contact_number);      
      this.NgxSpinnerService.hide()
    });

  }
  GetOnChangeProductsName() {
    let product_gid = this.productform.value.product_name.product_gid;
    let param = {
      product_gid: product_gid
    }
    var url = 'SmrCustomerEnquiry360/GetOnChangeProductECRM';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("product_code")?.setValue(result.GetOnChangeProduct_ECRM[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.GetOnChangeProduct_ECRM[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.GetOnChangeProduct_ECRM[0].productgroup_name);
      this.productform.value.productgroup_gid = result.GetOnChangeProduct_ECRM[0].productgroup_gid
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
    var api = 'SmrCustomerEnquiry360/PostProductECRM'
    this.NgxSpinnerService.show()
    this.service.post(api, params).subscribe((result: any) => {
      this.response_data=result;
      if(result.status ==false){
        this.ToastrService.warning(result.message) 
        this.NgxSpinnerService.hide()      
        this.POproductsummary();
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
        this.productform.reset();
        this.POproductsummary();
        
      }
    },
    );
  }
  POproductsummary() {
    var api = 'SmrCustomerEnquiry360/GetProductSummaryECRM';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.POproductlist = this.responsedata.ECRMProductSummarylist;

    });
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'SmrCustomerEnquiry360/DeleteProductSummaryECRM'
    let param = {
      tmpsalesenquiry_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)

      }
      this.POproductsummary();


    });
  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter

  }

  onSubmit() {
    var params = {
      branch_name: this.combinedFormData.value.branch_name,
      branch_gid: this.combinedFormData.value.branch_name.branch_gid,
      enquiry_date: this.combinedFormData.value.enquiry_date,
      enquiry_gid: this.combinedFormData.value.enquiry_gid,
      customer_name: this.combinedFormData.value.customer_name.customer_name,
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
      customer_gid: this.combinedFormData.value.customer_name.customer_gid
    }

    var url = 'SmrCustomerEnquiry360/PostCustomerEnquiryECRM'
    this.NgxSpinnerService.show()
    if (this.combinedFormData.value.enquiry_date != null && this.combinedFormData.value.branch_name != '',
      this.combinedFormData.value.customer_name != null && this.combinedFormData.value.user_firstname != '') {
      for (const control of Object.keys(this.combinedFormData.controls)) {
        this.combinedFormData.controls[control].markAsTouched();
      }
      this.service.postparams(url, params).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide()
          this.ToastrService.warning(result.message);

        }
        else {
          this.ToastrService.success(result.message)
          const secretKey = 'storyboarderp';
          const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();

          if (this.lspage1 == 'MM-Total') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Upcoming') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Lapsed') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Longest') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-New') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Prospect') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Potential') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-mtd') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-ytd') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Customer') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'MM-Drop') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Today') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-New') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Prospect') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Potential') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Customer') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Drop') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-All') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else if (this.lspage1 == 'My-Upcoming') {
            this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
            this.NgxSpinnerService.hide()
          }
          else {
            this.router.navigate(['/smr/SmrTrnCustomerenquirySummary']);
            this.NgxSpinnerService.hide()
          }
        }
      });
    }
  }

  onback() {
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();

    if (this.lspage1 == 'MM-Total') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Upcoming') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Lapsed') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Longest') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-New') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Prospect') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Potential') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-mtd') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-ytd') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Customer') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Drop') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Today') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-New') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Prospect') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Potential') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Customer') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Drop') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-All') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Upcoming') {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else {
      this.router.navigate(['/smr/SmrTrnCustomerenquirySummary']);
    }
  }

  editprod(tmpsalesenquiry_gid: any) {
    var url = 'SmrCustomerEnquiry360/EditProductSummaryECRM'
    this.NgxSpinnerService.show()
    let param = {
      tmpsalesenquiry_gid: tmpsalesenquiry_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.ECRMProductSummarylist = result.ECRMProductSummarylist;
      this.productform.get("tmpsalesenquiry_gid")?.setValue(this.ECRMProductSummarylist[0].tmpsalesenquiry_gid);
      this.productform.get("product_name")?.setValue(this.ECRMProductSummarylist[0].product_name);
      this.productform.get("product_gid")?.setValue(this.ECRMProductSummarylist[0].product_gid);
      this.productform.get("product_code")?.setValue(this.ECRMProductSummarylist[0].product_code);
      this.productform.get("productuom_name")?.setValue(this.ECRMProductSummarylist[0].productuom_name); 
      this.productform.get("productgroup_name")?.setValue(this.ECRMProductSummarylist[0].productgroup_name);      
      this.productform.get("qty_requested")?.setValue(this.ECRMProductSummarylist[0].qty_requested);
      this.productform.get("potential_value")?.setValue(this.ECRMProductSummarylist[0].potential_value);
      this.productform.get("product_requireddate")?.setValue(this.ECRMProductSummarylist[0].product_requireddate); 
      this.showUpdateButton = true;
      this.showAddButton = false;   
      this.NgxSpinnerService.hide()
    });
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
    var url = 'SmrCustomerEnquiry360/UpdateEnquiryProductECRM'
    this.NgxSpinnerService.show()
    this.service.post(url,params).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        this.ToastrService.warning(result.message) 
        this.NgxSpinnerService.hide()      
        this.POproductsummary();
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
        this.productform.reset();
        
      }
      this.POproductsummary();
    });
    this.showAddButton = true;
    this.showUpdateButton = false;
  }
  OnClearProduct()
  {
    this.txtProductGroup='';
    this.txtProductCode='';
    this.txtUnit='';
 
  }
  OnClearCustomerName()
  {
    this.txtCustomerBranch='';
    this.txtContactPerson='';
    this.txtContactNumber='';
    this.txtEmailid='';
    this.txtAddress='';
    }

}
