import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ExcelService } from 'src/app/Service/excel.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'src/environments/environment';


interface IEmployee {
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  employee_gid: string;
  user_code: string;
  confirmusercode: string;
  active_flag: string;
  vendor_gid: string;
  product_desc: string;

}

@Component({
  selector: 'app-pmr-trn-vendorregister-summary',
  templateUrl: './pmr-trn-vendorregister-summary.component.html',
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class PmrTrnVendorregisterSummaryComponent {
  file!: File;
  reactiveFormReset!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormstatus!: FormGroup;
  reactiveFormImport: FormGroup | any;
  reactiveFormUpdateUserCode!: FormGroup;
  responsedata: any;
  reset_list: any[] = [];
  GetExcelLog_list: any[] = [];
  GetExcelLogDetails_list: any[] = [];
  vendor_list: any[] = [];
  parameterValuecode: any;
  parameterValueReset: any;
  showOptionsDivId: any;
  rows: any[] = [];
  employee!: IEmployee;
  usercode: any;
  user_firstname: any;
  branch: any;
  department: any;
  designation: any;
  parameterValue: any;
  parameterValue1: any;
  fileInputs: any;
  myModaledit: any;

  constructor(private modalService: NgbModal, public service: SocketService, private excelService: ExcelService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.employee = {} as IEmployee;
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
    this.reactiveFormReset = new FormGroup({

      password: new FormControl(this.employee.password, [
        Validators.required,
      ]),
      confirmpassword: new FormControl(''),
      employee_gid: new FormControl(''),

    });
    this.reactiveFormUpdateUserCode = new FormGroup({

      password: new FormControl(this.employee.password, [
        Validators.required,
      ]),
      confirmusercode: new FormControl(''),
      employee_gid: new FormControl(''),

    });
    this.reactiveFormstatus = new FormGroup({

      active_flag: new FormControl(this.employee.active_flag, [
        Validators.required,
      ]),
      product_desc: new FormControl(''),
      vendor_gid: new FormControl(''),

    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetVendorregisterSummary();

  }
  openModalinactive(parameter: string) {
    this.parameterValue = parameter
  }


  oninactive() {
    console.log(this.parameterValue);
    // var url3 = 'SmrTrnCustomerSummary/GetcustomerInactive'
    var url3 = 'PmrMstVendorRegister/UpdateVendorStatus';
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning('Error While Vendor Inactivated')
      }
      else {
        this.ToastrService.success('Vendor Inactivated Successfully')
      }
      window.location.reload();
    });
  }
  GetVendorregisterSummary() {
    debugger
    var api1 = 'PmrMstVendorRegister/GetVendorregisterSummary'
    this.NgxSpinnerService.show();
    this.service.get(api1).subscribe((result: any) => {
      $('#vendor_list').DataTable().destroy();
      this.responsedata = result;
      this.vendor_list = this.responsedata.Getvendor_lists;
      console.log(this.vendor_list)
      setTimeout(() => {
        $('#vendor_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      // this.reactiveFormstatus.get("active_flag")?.setValue(this.vendor_list[0].active_flag);
      // this.reactiveFormstatus.get("vendorregister_gid")?.setValue(this.vendor_list[0].vendorregister_gid);

    });
  }
  get password() {
    return this.reactiveFormReset.get('password')!;
  }
  get user_code() {
    return this.reactiveFormUpdateUserCode.get('user_code')!;
  }
  userpassword(password: any) {
    this.reactiveFormReset.get("confirmpassword")?.setValue(password.value);
  }
  updateusercode(user_code: any) {
    console.log(user_code.value)
    this.reactiveFormUpdateUserCode.get("confirmusercode")?.setValue(user_code.value);
  }
  openModalUpdateCode(parameter: string) {

  }
  openModalReset(parameter: string) {


  }

  onview(params: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnVendorregisterView', encryptedParam])
  }
  onedit(params: any) {
    if (params.supplier_id && params.supplier_id !== "") {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning("Vendor Push To Mintsoft Can't Edit")

    }
    else {
      const secretKey = 'storyboarderp';
      const param = (params.vendor_gid);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.route.navigate(['/pmr/PmrTrnVendorregisterEdit', encryptedParam])

    }
  }
  onaddinfo(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/pmr/PmrMstVendorAdditionalinformation', encryptedParam])
  }
  onclose() {
    this.reactiveFormstatus.reset();

  }
  oncloseupdatecode() {
    this.reactiveFormUpdateUserCode.reset();

  }
  onupdatereset() {
    //console.log(this.reactiveFormReset.value)

    if (this.reactiveFormReset.value.password != null && this.reactiveFormReset.value.password != '') {
      for (const control of Object.keys(this.reactiveFormReset.controls)) {
        this.reactiveFormReset.controls[control].markAsTouched();
      }



      var url = 'Vendorlist/Getresetpassword'

      this.service.post(url, this.reactiveFormReset.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)

        }
        else {
          this.ToastrService.success(result.message)
          this.GetVendorregisterSummary();
        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormReset.reset();
  }
  onupdateusercode() {
    if (this.reactiveFormUpdateUserCode.value.user_code != null && this.reactiveFormUpdateUserCode.value.user_code != '') {
      for (const control of Object.keys(this.reactiveFormUpdateUserCode.controls)) {
        this.reactiveFormUpdateUserCode.controls[control].markAsTouched();
      }



      var url = 'Vendorlist/Getupdateusercode'

      this.service.post(url, this.reactiveFormUpdateUserCode.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)

        }
        else {
          this.ToastrService.success(result.message)
          this.GetVendorregisterSummary();
        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormUpdateUserCode.reset();
  }

  openModaldelete(parameter: any) {
    this.parameterValue = parameter.vendor_gid;
    this.parameterValue1 = parameter.supplier_id;
  }
  ondelete() {
    if (this.parameterValue1 && this.parameterValue1 !== "") {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning("Vendor Push To Mintsoft Can't Edit")

    }
    else {
      //debugger
      //console.log(this.parameterValue);
      var url = 'PmrMstVendorRegister/VendorRegisterSummaryDelete'
      let param = {
        vendor_gid: this.parameterValue
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
        }
        this.GetVendorregisterSummary();
      });
    }
  }
  onattach(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/pmr/PmrMstVendorRegisterDocument', encryptedParam])
  }

  activestatus(parameter: any) {
    debugger
    this.parameterValue1 = parameter
    // // const secretKey = 'storyboarderp';
    // // const param = (params);
    // // const encryptedParam = AES.encrypt(param,secretKey).toString();
    // // this.route.navigate(['/pmr/PmrMstVendorRegisterDocument',encryptedParam])
    this.reactiveFormstatus.get("active_flag")?.setValue(this.parameterValue1.active_flag);
    this.reactiveFormstatus.get("vendor_gid")?.setValue(this.parameterValue1.vendor_gid);
  }

  validate() {
    debugger
    console.log(this.reactiveFormstatus.value)

    this.employee = this.reactiveFormstatus.value;
    // this.service.Profileupload(this.reactiveForm.value).subscribe(result => {  
    //   this.responsedata=result;
    // });   
    if (this.employee.active_flag != null && this.employee.product_desc != '') {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        formData.append("active_flag", this.employee.active_flag);
        formData.append("vendor_gid", this.employee.vendor_gid);
        formData.append("product_desc", this.employee.product_desc);
      }
      this.reactiveFormstatus.value;

      var url1 = 'PmrMstVendorRegister/UpdateVendorStatus'

      this.service.post(url1, this.reactiveFormstatus.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetVendorregisterSummary();
          this.modalService.dismissAll(this.myModaledit.nativeElement);
        }
        else {

          this.ToastrService.success(result.message)
          this.GetVendorregisterSummary();
          this.modalService.dismissAll(this.myModaledit.nativeElement);
        }
      });
      // }


    }

    else {
      this.ToastrService.warning('Kindly Fill The Mandatory Field !! ')
    }
    this.GetVendorregisterSummary();
  }

  vrndorexportExcel() {
    debugger
    // var api7 = 'PmrMstVendorRegister/GetVendorReportExport'
    // this.service.generateexcel(api7).subscribe((result: any) => {
    //   this.responsedata = result;
    //   var phyPath = this.responsedata.vendorexport_list[0].lspath1;
    //   var relPath = phyPath.split("src");
    //   var hosts = window.location.host;
    //   var prefix = location.protocol + "//";
    //   var str = prefix.concat(hosts, relPath[1]);
    //   var link = document.createElement("a");
    //   var name = this.responsedata.vendorexport_list[0].lsname2.split('.');
    //   link.download = name[0];
    //   link.href = str;
    //   link.click();
    //   this.ToastrService.success("Vendor Excel Exported Successfully")

    // });
    const VendorExcel = this.vendor_list.map(item => ({
      'Vendor Code': item.vendor_code || '',
      'Vendor Companyname': item.vendor_companyname || '',
      'Contact personName': item.contactperson_name || '',
      'Contact Telephonenumber': item.contact_telephonenumber || '',
      'Contact Email': item.email_address || '',
      'Billing Email': item.billingemail_address || '',
      'Tax Number': item.tax_number || '',
      TaxSegment: item.taxsegment_name || '',
      AverageLeadTime: item.averageleadtime || '',
      AddressI: item.address1 || '',
      AddressII: item.address2 || '',
      City: item.city || '',
      'Postal Code': item.postal_code || '',
      Country: item.countryname || '',
      Region: item.region || '',
      Currency: item.currencyname || '',
      'Payment Terms': item.paymentterms || '',
      'Credit Days': item.creditdays || '',
      Status: item.vendor_status || '',
    }));
    this.excelService.exportAsExcelFile(VendorExcel, 'Vendor_Excel');
  }


  downloadfileformat() {
    debugger;
    // let link = document.createElement("a");
    // link.download = "Purchase VendorExcel";
    //  window.location.href = "https://"+ environment.host + "/Templates/Purchase VendorExcel.xls";
    // link.click();

    let link = document.createElement("a");
    link.download = "Vendor Template";
    link.href = environment.URL_FILEPATH + "/Templates/Vendor Excel.xlsx";
    link.click();



  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }

  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      formData.append("file", this.file, this.file.name);
      var api = 'PmrMstVendorRegister/VendorImportExcel'
      this.NgxSpinnerService.show();

      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetVendorregisterSummary()
        }
        else {

          this.NgxSpinnerService.hide();
          // window.location.reload();
          this.fileInputs = null;
          this.ToastrService.success("Excel Uploaded Successfully")
          this.GetVendorregisterSummary();

        }


      });
    }
  }
  onback() {
    this.route.navigate(['/pmr/PmrMstVendorregister']);
  }
  Excellog() {
    var url = 'PmrMstVendorRegister/GetImportExcelLog';
    this.service.get(url).subscribe((result: any) => {
      this.GetExcelLog_list = result.GetExcelLog_list;
    });
  }
  ondetail(upload_gid: any) {
    let param = { upload_gid: upload_gid }
    var url = 'PmrMstVendorRegister/GetExcelLogDetails';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetExcelLogDetails_list = result.GetExcelLogDetails_list;
    });
  }
  exportExcel(upload_gid: any) {
    let param = { upload_gid: upload_gid }
    var url = 'PmrMstVendorRegister/GetExcelLogDetails';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetExcelLogDetails_list = result.GetExcelLogDetails_list;

      const VendorExcel = this.GetExcelLogDetails_list.map(item => ({
        'Vendor Code': item.vendor_code || '',
        'Vendor Companyname': item.vendor_companyname || '',
        'Contact personName': item.contactperson_name || '',
        'Contact Telephonenumber': item.contact_telephonenumber || '',
        'Contact Email': item.email_id || '',
        'Billing Email': item.billing_mail || '',
        'Tax Number': item.tax_no || '',
        TaxSegment: item.tax_segment || '',
        AverageLeadTime: item.avrge_lead_time || '',
        AddressI: item.address1 || '',
        AddressII: item.address2 || '',
        City: item.city || '',
        'Postal Code': item.postal_code || '',
        Country: item.country_name || '',
        Region: item.region || '',
        Currency: item.currency || '',
        'Payment Terms': item.payment_term || '',
        'Credit Days': item.credit_days || '',
      }));
      this.excelService.exportAsExcelFile(VendorExcel, 'Vendor_Excel');
    });
  }
}
