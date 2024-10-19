import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-ims-trn-opendcaddselect-update',
  templateUrl: './ims-trn-opendcaddselect-update.component.html',
  styleUrls: ['./ims-trn-opendcaddselect-update.component.scss']
})
export class ImsTrnOpendcaddselectUpdateComponent {
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '100%',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  combinedFormData: FormGroup | any;
  opendcaddsel_list: any[] = [];
  salesorder_gid: any;
  opendc: any;
  opendcaddselprod_list: any[] = [];
  mdlTerms :any;
  terms_list: any[] = [];
  constructor(private http: HttpClient, private fb: FormBuilder,public NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.combinedFormData = new FormGroup({
      customer_address_so: new FormControl(''),
      shipping_to: new FormControl(''),
      dc_no: new FormControl('',Validators.required),
      despatch_mode: new FormControl(''),
      tracker_id: new FormControl(''),
      template_name: new FormControl(''),
      termsandconditions: new FormControl(''),
      customer_gid: new FormControl(''),
      directorder_refno: new FormControl(''),
      customer_name: new FormControl(''),
      customer_code: new FormControl(''),
      customer_contact_person: new FormControl(''),
      customer_email: new FormControl(''),
      customer_mobile: new FormControl(''),
      salesorder_gid: new FormControl(''),
      customer_details: new FormControl(''),
      directorder_date: new FormControl(this.getCurrentDate()),
      despatch_quantity: new FormControl('',[Validators.required,Validators.pattern('^[0-9]*$')])
    })
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    this.opendc = this.route.snapshot.paramMap.get('salesorder_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.opendc, secretKey).toString(enc.Utf8);
    this.GetOpenDcSummary(deencryptedParam);
    this.salesorder_gid = deencryptedParam;
    var url = 'SmrTrnQuotation/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsandConditions;
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  GetOpenDcSummary(salesorder_gid: any) {
    var api = 'ImsTrnOpenDcSummary/GetOpenDcUpdate';
    this.NgxSpinnerService.show()
    let params = {
      salesorder_gid: salesorder_gid,
    }
    this.service.getparams(api, params).subscribe((result: any) => {
      this.opendcaddsel_list = result.opendcaddsel_list;
      this.combinedFormData.get("directorder_refno")?.setValue(this.opendcaddsel_list[0].directorder_refno);
      this.combinedFormData.get("directorder_date")?.setValue(this.opendcaddsel_list[0].directorder_date);
      this.combinedFormData.get("customer_name")?.setValue(this.opendcaddsel_list[0].customer_name);
      this.combinedFormData.get("customer_code")?.setValue(this.opendcaddsel_list[0].customer_code);
      this.combinedFormData.get("customer_contact_person")?.setValue(this.opendcaddsel_list[0].customer_contact_person);
      this.combinedFormData.get("customer_email")?.setValue(this.opendcaddsel_list[0].customer_email);
      this.combinedFormData.get("customer_mobile")?.setValue(this.opendcaddsel_list[0].customer_mobile);
      this.combinedFormData.get("salesorder_gid")?.setValue(this.opendcaddsel_list[0].salesorder_gid);
      const customer_mobile = result.opendcaddsel_list[0].customer_mobile;
      const customer_email = result.opendcaddsel_list[0].customer_email;
      const customer_contactperson = result.opendcaddsel_list[0].customer_contact_person;
      const customerDetails = `${customer_contactperson}\n${customer_mobile}\n${customer_email}`;
      this.combinedFormData.get("customer_details")?.setValue(customerDetails);
      this.combinedFormData.get("customer_address_so")?.setValue(this.opendcaddsel_list[0].customer_address_so);
      this.combinedFormData.get("customer_gid")?.setValue(this.opendcaddsel_list[0].customer_gid);
      this.combinedFormData.get("shipping_to")?.setValue(this.opendcaddsel_list[0].customer_address_so);
    });
    var api = 'ImsTrnOpenDcSummary/GetOpenDcUpdateProd';
    let param1 = {
      salesorder_gid: salesorder_gid,
    }
    this.service.getparams(api, param1).subscribe((result: any) => {
      this.opendcaddselprod_list = result.opendcaddselprod_list;
      setTimeout(() => {
        $('#opendcaddselprod_list').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide()
  }
  get dc_no() {
    return this.combinedFormData.get('dc_no')!;
  };
  OnDelDcSubmit() {
    if (this.combinedFormData.value.despatch_quantity !== null && this.combinedFormData.value.despatch_quantity !== "" && this.combinedFormData.value.dc_no !==null) {
    var params = {
      directorder_refno: this.opendcaddsel_list[0].directorder_refno,
      directorder_date: this.opendcaddsel_list[0].directorder_date,
      customercontact_name: this.opendcaddsel_list[0].customercontact_name,
      customer_code: this.opendcaddsel_list[0].customer_code,
      stock_qty: this.opendcaddsel_list[0].stock_qty,
      customer_contact_person: this.opendcaddsel_list[0].customer_contact_person,
      customer_name: this.opendcaddsel_list[0].customer_name,
      customer_email: this.opendcaddsel_list[0].customer_email,
      customer_mobile: this.opendcaddsel_list[0].customer_mobile,
      salesorder_gid: this.opendcaddsel_list[0].salesorder_gid,
      customer_address_so: this.combinedFormData.value.customer_address_so,
      shipping_to: this.combinedFormData.value.shipping_to,
      dc_no: this.combinedFormData.value.dc_no,
      despatch_mode: this.combinedFormData.value.despatch_mode,
      tracker_id: this.combinedFormData.value.tracker_id,
      despatch_quantity: this.combinedFormData.value.despatch_quantity,
      termsandconditions: this.combinedFormData.value.termsandconditions,
      productgroup_name: this.opendcaddselprod_list[0].productgroup_name,
      product_name: this.opendcaddselprod_list[0].product_name,
      display_field: this.opendcaddselprod_list[0].display_field,
      uom_name: this.opendcaddselprod_list[0].uom_name,
      qty_quoted: this.opendcaddselprod_list[0].qty_quoted,
      available_quantity: this.opendcaddselprod_list[0].available_quantity,
    }
    var url = 'ImsTrnOpenDcSummary/PostOpenDcSubmit'
    this.NgxSpinnerService.show()
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.router.navigate(['/ims/ImsTrnOpendcsummary']);   
        this.NgxSpinnerService.hide()
      }
    });
  }
else {
  this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
}
return;
  }
  redirecttolist(){
    this.router.navigate(['/ims/ImsTrnOpendcAddselect']);
  }
  GetOnChangeTerms() {
    let template_gid = this.combinedFormData.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.combinedFormData.get("termsandconditions")?.setValue(result.terms_list[0].termsandconditions);
      this.combinedFormData.value.template_gid = result.terms_list[0].template_gid   
     });
  }
}
