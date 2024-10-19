import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
export class IAdovacacy {

  leadtoapi: any[] = [];
  customer_gid: string = "";
}
@Component({
  selector: 'app-crm-trn-advocacymanagement',
  templateUrl: './crm-trn-advocacymanagement.component.html',
  styleUrls: ['./crm-trn-advocacymanagement.component.scss']
})


export class CrmTrnAdvocacymanagementComponent {
  reactiveForm!: FormGroup;
  GetLeaddropdown_list: any[] = [];
  leadbank_gid: any;
  leadbank_details: any = null;
  source: any = null;
  mobile: any = null;
  leaddetails_list: any[] = [];
  GetCustomerdropdown_list: any[] = [];
  customer_details: any = null;
  show: boolean = false;
  customer_source: any = null;
  customer_gid: any = null;
  checkleadbank_gid: any;
  CurObj: IAdovacacy = new IAdovacacy();
  leadtoapi: any[] = [];
  GetAdovacacysummary_list: any[] = [];
  GetAdvocacyDetails_list: any[] = [];
  leadbank_namepopup: any;
  reactiveFormEdit!: FormGroup;
  sourceedit: any = null;
  leadbank_detailsedit: any = null;
  leadbank_gidedit: any = null;
  checkleadbank_gidedit: any = null;
  parameter1: any;
  parameter2: any;
  parameter3: any;
  popupdelete: boolean = false;
  popupdeletemain: boolean = false;
  showOptionsDivId: any;

  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

    this.reactiveForm = new FormGroup({
      customer_name: new FormControl(),
      leadbank_name: new FormControl(null),
    });
    this.reactiveFormEdit = new FormGroup({
      leadbank_nameedit: new FormControl(null),
      advocacy_leadbankgidedit: new FormControl(null),
    });
    this.GetAdovacacysummary();
  }
  get customer_name() {
    return this.reactiveForm.get('customer_name')!;
  }
  get leadbank_name() {
    return this.reactiveForm.get('leadbank_name')!;
  }
  get leadbank_nameedit() {
    return this.reactiveFormEdit.get('leadbank_nameedit')!;
  }
  lead_details() {
    if (this.reactiveForm.value.leadbank_name !== null || this.reactiveForm.value.leadbank_name == '' || this.reactiveForm.value.leadbank_name == undefined) {
      this.leadbank_gid = this.reactiveForm.value.leadbank_name;
    }
    else {
      this.source = null;
    }
    const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.leadbank_gid);
    if (selectedLead) {
      const detailsArray = [
        selectedLead.leadbankbranch_name,
        selectedLead.leadbankcontact_name,
        selectedLead.address1,
        selectedLead.address2,
        selectedLead.city,
        selectedLead.state,
        selectedLead.pincode
      ];
      this.leadbank_details = detailsArray.filter(detail => detail).join('\n');
      this.source = selectedLead.source_name;
    }
  }
  onsubmit() {
    if (this.reactiveForm.value.leadbank_name !== null && this.reactiveForm.value.customer_name !== null) {
      this.NgxSpinnerService.show();
      this.CurObj.customer_gid = this.reactiveForm.value.customer_name;
      this.CurObj.leadtoapi = this.leaddetails_list;
      var url = 'AdovacacyManagement/PostAdovacacy';
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message);
          this.reactiveForm.reset();
          this.customer_source = null;
          this.customer_details = null;
          this.source = null;
          this.leadbank_details = null;
          this.GetAdovacacysummary();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message);
          this.reactiveForm.reset();
          this.customer_source = null;
          this.customer_details = null;
          this.source = null;
          this.leadbank_details = null;
          this.GetAdovacacysummary();
        }

        this.NgxSpinnerService.hide();
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.NgxSpinnerService.hide();
  }
  leaddetailsummary() {

    this.checkleadbank_gid = this.reactiveForm.value.leadbank_name;
    if (this.checkleadbank_gid !== null && this.checkleadbank_gid !== '') {
      const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.checkleadbank_gid);
      if (selectedLead && !this.leaddetails_list.some(item => item.leadbank_gid === this.checkleadbank_gid)) {
        this.leaddetails_list.push(selectedLead);
        this.show = true;
      }
    }
    else {
      this.show = false;
    }

  }
  toggleOptions(leadbank_gid: any) {
    if (this.showOptionsDivId === leadbank_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = leadbank_gid;
    }
  }
  GetDropdowns() {
    var api = 'AdovacacyManagement/GetLeaddropdownforadvocacy';
    this.service.get(api).subscribe((result: any) => {
      this.GetLeaddropdown_list = result.GetLeaddropdownadvocacy_list;
      //console.log('lnflkwek', this.GetLeaddropdown_list)
    });
    var api = 'AdovacacyManagement/GetCustomerdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.GetCustomerdropdown_list = result.GetCustomerdropdown_list;
    });
  }
  DeleteGentralDocumentClick(index: any) {
    if (index >= 0 && index < this.leaddetails_list.length) {
      this.leaddetails_list.splice(index, 1);
    }
  }
  customerdetails() {
    if (this.reactiveForm.value.customer_name !== null || this.reactiveForm.value.customer_name == '' || this.reactiveForm.value.customer_name == undefined) {
      this.customer_gid = this.reactiveForm.value.customer_name;
    } else {
      this.customer_gid = null;
    }
    const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.customer_gid);
    //console.log('mewlml',selectedLead)
    if (selectedLead) {
      const detailsArray = [
        selectedLead.leadbankbranch_name,
        selectedLead.leadbankcontact_name,
        selectedLead.address1,
        selectedLead.address2,
        selectedLead.city,
        selectedLead.state,
        selectedLead.pincode
      ];
      this.customer_details = detailsArray.filter(detail => detail).join('\n');
      this.customer_source = selectedLead.source_name;
    }
  }

  GetAdovacacysummary() {
    this.NgxSpinnerService.show();
    var api = 'AdovacacyManagement/GetAdovacacysummary';
    this.service.get(api).subscribe((result: any) => {
      $('#GetAdovacacysummary_lists').DataTable().destroy();
      this.NgxSpinnerService.hide();
      this.GetAdovacacysummary_list = result.GetAdovacacysummary_list;
      setTimeout(() => {
        $('#GetAdovacacysummary_lists').DataTable();
      }, 1);
    });
  }

  Ondetails(advocacy_leadbankgid: any, leadbank_name: any) {
    let param = {
      leadbank_gid: advocacy_leadbankgid
    }
    var api = 'AdovacacyManagement/GetAdvocacyDetails';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.GetAdvocacyDetails_list = result.GetAdvocacyDetails_list;
      this.leadbank_namepopup = leadbank_name;
      this.reactiveFormEdit.get('advocacy_leadbankgidedit')?.setValue(param.leadbank_gid);

    });
  }

  Detailleadsummary() {
    this.checkleadbank_gidedit = this.reactiveFormEdit.value.leadbank_nameedit;
    if (this.checkleadbank_gidedit !== null && this.checkleadbank_gidedit !== '' && this.checkleadbank_gidedit !== undefined) {
      const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.checkleadbank_gidedit);
      this.GetAdvocacyDetails_list.push(selectedLead);
      //console.log('edniewmd;l', this.GetAdvocacyDetails_list)
    }
  }
  lead_detailsedit() {

    if (this.reactiveFormEdit.value.leadbank_nameedit !== null || this.reactiveFormEdit.value.leadbank_nameedit == '' || this.reactiveFormEdit.value.leadbank_nameedit == undefined) {
      this.leadbank_gidedit = this.reactiveFormEdit.value.leadbank_nameedit;
    }
    else {
      this.sourceedit = null;
    }
    const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.leadbank_gidedit);
    if (selectedLead) {
      const detailsArray = [
        selectedLead.leadbankbranch_name,
        selectedLead.leadbankcontact_name,
        selectedLead.address1,
        selectedLead.address2,
        selectedLead.city,
        selectedLead.state,
        selectedLead.pincode
      ];
      this.leadbank_detailsedit = detailsArray.filter(detail => detail).join('\n');
      this.sourceedit = selectedLead.source_name;
    }
  }
  ondelete(parameter1: any, parameter2: any, parameter3: any) {
    if (parameter3 !== null && parameter1 == null && parameter2 == null) {
      if (parameter3 >= 0 && parameter3 < this.GetAdvocacyDetails_list.length) {
        this.GetAdvocacyDetails_list.splice(parameter3, 1);
      }
    }
    else {
      let paramdelete = {
        adovacacy_leadbankgid: parameter1,
        reference_leadbankgid: parameter2,
      }
      var url = 'AdovacacyManagement/DeleteAdvocacy'
      this.service.getparams(url, paramdelete).subscribe((result: any) => {
        if (result.status == true) {
          this.Ondetails(paramdelete.adovacacy_leadbankgid, this.leadbank_namepopup);
        }
      });
    }
  }
  onsubmitedit() {

    if (this.reactiveFormEdit.value.leadbank_nameedit !== null) {
      this.NgxSpinnerService.show();
      this.CurObj.customer_gid = this.reactiveFormEdit.value.advocacy_leadbankgidedit;
      this.CurObj.leadtoapi = this.GetAdvocacyDetails_list;
      var url = 'AdovacacyManagement/PostAdovacacy';
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message);
          this.reactiveFormEdit.reset();
          this.sourceedit = null;
          this.leadbank_detailsedit = null;
          this.GetAdovacacysummary();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message);
          this.reactiveFormEdit.reset();
          this.sourceedit = null;
          this.leadbank_detailsedit = null;
          this.GetAdovacacysummary();
        }
      });
      this.NgxSpinnerService.hide();
    }
    else {
      this.ToastrService.warning('No new records added !! ');
    }
  }

  oneditclose() {
    this.reactiveFormEdit.reset();
    this.sourceedit = null;
    this.leadbank_detailsedit = null;
  }
  onaddclose() {
    this.reactiveForm.reset();
    this.customer_source = null;
    this.customer_details = null;
    this.source = null;
    this.leadbank_details = null;
    this.leaddetails_list.splice(0, this.leaddetails_list.length);
    this.show = false;
  }

}
