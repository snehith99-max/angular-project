import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Table } from 'primeng/table';

export class IContactList {
  contacts_list: any[] = [];
  project_id: string = "";
  sendtext: string = "";
}
export interface Customer {

  whatsapp_gid?: any;
  id?: any | null;
  wkey?: any | null;
  value?: any | null;
  displayName?: any | null;
  first_letter?: any | null;
  firstName?: any | null;
  lastName?: any | null;
  gender?: any | null;
  created_date?: any | null;
  created_by?: any | null;
  read_flag?: any | null;
  last_seen?: any | null;
  customer_type?: any | null;
  source_name?: any | null;
  lead_status?: any | null;
  sendcampaign_flag?: any | null;

}
interface ILeadbank{
  region_name: string;
  source_name: string;
  customer_type: string;

}

@Component({
  selector: 'app-crm-mst-wabulkmessage',
  templateUrl: './crm-mst-wabulkmessage.component.html',
  styleUrls: ['./crm-mst-wabulkmessage.component.scss']
})
export class CrmMstWabulkmessageComponent implements OnInit {
  responsedata: any;
  contacts_list: any[] = [];
  file!: File;
  selection = new SelectionModel<IContactList>(true, []);
  reactiveForm!: FormGroup;
  CurObj: IContactList = new IContactList();
  reactiveMessageForm!: FormGroup;
  pick: Array<any> = [];
  cboTemplate: any;
  sendtext:any;
  templateList: any;
  bulkMessageForm: FormGroup | any;
  bulkMessageForm1: FormGroup | any;
  templateview_list: any;
  template_body: any;
  p_name: any;
  footer: any;
  media_url: any;
  project_id:any;
  version_id:any;
  contacts_list1: any[] = [];
  fileInputs: any;
   //////////////demo////////////////////
   customer!: Customer;
   selectedCustomer: Customer[] = [];
   source_list: any[] = [];
   regionnamelist: any[] = [];
   customertype_list: any[] = [];
   leadbank!: ILeadbank;
   Contactlist: any[] = [];
  constructor(private formBuilder: FormBuilder,  private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {
    this.leadbank = {} as ILeadbank;
    this.reactiveForm = new FormGroup({

    });
    this.bulkMessageForm = new FormGroup({
      cboTemplate: new FormControl(null, Validators.required),
      id: new FormControl(),
      region_name: new FormControl(this.leadbank.region_name, [   
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      source_name: new FormControl(this.leadbank.source_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      customer_type: new FormControl(this.leadbank.customer_type, [
        
      ]),
    });
    this.bulkMessageForm1 = new FormGroup({
      id: new FormControl(),
    });
  }
  ngOnInit(): void {
    const project_id = this.route.snapshot.paramMap.get('project_id');
    const version_id = this.route.snapshot.paramMap.get('version_id');
    this.project_id = project_id;
    this.version_id = version_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.project_id, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.version_id, secretKey).toString(enc.Utf8);

    this.project_id = deencryptedParam;
    this.version_id = deencryptedParam1;
    this.getContactList();
    this.getContactList1();
    this.gettemplateList();
    this.marketingteam();
    
    var api1 = 'Leadbank/Getsourcedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = result.source_list;
      //console.log(this.source_list)
    });

    var api2 = 'Leadbank/Getregiondropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.regionnamelist = result.regionname_list;
    });

    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });
  }
  getContactList() {
    this.NgxSpinnerService.show();
    let params = {
      project_id: this.project_id
    }
    var url = 'Whatsapp/GetCampaignContactsent'
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#whatsnamelist').DataTable().destroy();
      this.contacts_list = result.whatscontactlist;
      setTimeout(() => {
        $('#whatsnamelist').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }
  getContactList1() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetContact'
    this.service.get(url).subscribe((result: any) => {
      $('#whatsnamelist1').DataTable().destroy();
      this.Contactlist = result.whatscontactlist;
      setTimeout(() => {
        $('#whatsnamelist1').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.contacts_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.contacts_list.forEach((row: IContactList) => this.selection.select(row));
  }
  onsend(): void {
    this.CurObj.contacts_list =  this.selectedCustomer
    this.CurObj.project_id = this.project_id
    if (this.CurObj.project_id == undefined) {
      this.ToastrService.warning("Kindly select a template to send")
      return;
    }
    if (this.CurObj.contacts_list.length == 0) {
      this.ToastrService.warning("Kindly select atleast 1 contact")
      return;
    }
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/bulkMessageSend'
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
      }
      this.NgxSpinnerService.hide();
      this.selectedCustomer = [];
    });
  }
 
  gettemplateList() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetMessageTemplatesummary'
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#template_list').DataTable().destroy();
      this.templateList = result.whatsappMessagetemplatelist;
      setTimeout(() => {
        $('#template_list').DataTable();
      }, 1);
    });
  }
  redirecttolist() {
    this.router.navigate(['/crm/CrmSmmWhatsappcampaign']);

  }
  marketingteam() {
   
    let params = {
      project_id: this.project_id
    }
    var url = 'Whatsapp/GetTemplatepreview'
    this.service.getparams(url, params).subscribe((result: any) => {
      
      this.p_name = result.Gettemplateview_list[0].p_name;
      this.template_body = result.Gettemplateview_list[0].template_body;
      this.footer = result.Gettemplateview_list[0].footer;
      this.media_url = result.Gettemplateview_list[0].media_url;
    });
  }
  onGlobalFilterChange(event: Event, dt: Table): void {
    const inputValue = (event.target as HTMLInputElement).value;
    dt.filterGlobal(inputValue, 'contains');
  }
  GetPaymentSummary(){
    const selectedRegion = this.bulkMessageForm.value.region_name || 'null'
    const selectedSource = this.bulkMessageForm.value.source_name || 'null'
    const selectedCustomertype = this.bulkMessageForm.value.customer_type || 'null';
    for (const control of Object.keys(this.bulkMessageForm.controls)) {
      this.bulkMessageForm.controls[control].markAsTouched();
    }
    const params = {
      region_name: selectedRegion,
      source_name: selectedSource,
      customer_type: selectedCustomertype,

    };
    this.NgxSpinnerService.show();
    const url = 'Whatsapp/GetContactist';

    this.service.getparams(url, params).subscribe((result) => {
      this.responsedata = result;
      this.Contactlist = this.responsedata.whatscontactlist;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Contactlist').DataTable();
      },1);
    });
  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Whatsapp Contacts Template";
    link.href = "assets/media/Excels/Whatsapp_contact_list_template/whatsapp_contactlist.xlsx";
    link.click();
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  importexcel() {
    debugger
     this.NgxSpinnerService.show();
     let formData = new FormData();
     if (this.file != null && this.file != undefined) {
       window.scrollTo({
         top: 0, // Code is used for scroll top after event done
       });
       formData.append("file", this.file, this.file.name);
       formData.append("project_id", this.project_id);

       var api = 'Whatsapp/whatsappcontactsImport'
       this.service.postfile(api, formData).subscribe((result: any) => {
         this.responsedata = result;
         if (result.status == false) {
           this.NgxSpinnerService.hide();
           this.ToastrService.warning(result.message)
         }
         else {
 
           this.NgxSpinnerService.hide();
           // window.location.reload();
           this.fileInputs= null;
           this.ToastrService.success(result.message)
 
         }
        
       });
     }
   }
   canceluploadexcel(){
    this.fileInputs= null;
  }
  onclose(){
    this.fileInputs= null;
    }
}
