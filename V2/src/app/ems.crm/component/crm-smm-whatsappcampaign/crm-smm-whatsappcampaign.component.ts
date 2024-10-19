import { Component } from '@angular/core'; import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { ExcelService } from 'src/app/Service/excel.service';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subject } from 'rxjs';
import * as XLSX from 'xlsx';
import { environment } from 'src/environments/environment.development';
import { get } from 'jquery';
import  jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';



interface IWhatsappcampaign {
  platform: string;
  localae: string;
  template_name: string;
  category: string;
  version_id: string;
  last_updated: string;
  updated_date: string;
  type: string;
  description: any;
  body: string;
  footer: string;
  project_id: string;
  value1: string;
  id: string;
  p_template_body: string;
  p_name: string;
  p_type: string;

  Total_messages: string;
  Sent_messages: string;
  Received_messages: string;
  //template_name: string;
  template_description: string;
  contact_count1: string;
  p_nameedit: string;

  templateedit_description: string;

}
@Component({
  selector: 'app-crm-smm-whatsappcampaign',
  templateUrl: './crm-smm-whatsappcampaign.component.html',
  styleUrls: ['./crm-smm-whatsappcampaign.component.scss'],
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
export class CrmSmmWhatsappcampaignComponent {
  isReadOnly = true;
  whatsappCampaign: any;
  responsedata: any;
  media_url:any
  contactcount_list: any;
  file!: File;
  image_path: any;
  campaignservice_list: any;
  template_list: any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormadd!: FormGroup;
  reactiveMessageForm!: FormGroup;
  channel_name: any;
  mobile_number: any;
  Whatsappcampaign!: IWhatsappcampaign;
  parameterValue1: any;
  contacts_list: any[] = [];
  parameterValue: any;
  parameterValue3: any;
  parameterValue4: any;
  parameterValue5: any;
  contact_count1: any[] = [];
  message_count: any[] = [];
  templateview_list: any;
  delivered_messages: any;
  footers: any;
  fileInputs: any;
  send_campaign:any;
  project_count: any;
  descriptions:any;
  Project_name:any;
  lsmtd: any;
  lsytd: any;
  leadbank_name: any;
  mobile: any;
  customer_type: any;
  isButtonTrue: boolean = true;
  isButtonFalse: boolean = false;
  file_type:any;
  spinnerEnabled = false;
  keys!: string[];
  dataSheet = new Subject();
  data: any[] = [];
  whatsapp_list: any;
  whatsappdtl_list: any[] = [];
  invalidlog_list: any[] =[];
  invalidDTLlog_list: any[] =[];
  showOptionsDivId: any;
  isExcelFile!: boolean;
  constructor(private formBuilder: FormBuilder,
    private excelService : ExcelService, private ToastrService: ToastrService,
    private route: Router, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {
    this.Whatsappcampaign = {} as IWhatsappcampaign;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getcampaign();
    // this.Getmessagestatus();
    this.GetWhatsappMessageCount();
    this.GetContactCount();
    this.GetTemplate();
    this.GetWhatsappSummarycampaign();
    this.getservicewindow();



    this.reactiveForm = new FormGroup({
      platform: new FormControl(this.Whatsappcampaign.platform, [
        Validators.required,
      ]),
      localae: new FormControl(this.Whatsappcampaign.localae, [
      ]),
      template_name: new FormControl(this.Whatsappcampaign.template_name, [
      ]),
      version_id: new FormControl(this.Whatsappcampaign.version_id, [
      ]),
      last_updated: new FormControl(this.Whatsappcampaign.last_updated, [
      ]),
      updated_date: new FormControl(this.Whatsappcampaign.updated_date, [
      ]),
      contact_count1: new FormControl(this.Whatsappcampaign.contact_count1, [
      ]),
      Total_messages: new FormControl(this.Whatsappcampaign.Total_messages, [
      ]),
      Sent_messages: new FormControl(this.Whatsappcampaign.Sent_messages, [
      ]),
      Received_messages: new FormControl(this.Whatsappcampaign.Received_messages, [
      ]),
      type: new FormControl(this.Whatsappcampaign.type, [
      ]),
      project_id: new FormControl(this.Whatsappcampaign.project_id, [
      ]),

      p_type: new FormControl(this.Whatsappcampaign.p_type, [
      ]),
      p_name: new FormControl(this.Whatsappcampaign.p_name, [
      ]),

      p_template_body: new FormControl(this.Whatsappcampaign.p_template_body, [
      ]),

      description: new FormControl(this.Whatsappcampaign.description, [
      ]),
      value1: new FormControl(this.Whatsappcampaign.value1, [
      ]),

      body: new FormControl(this.Whatsappcampaign.body, [
        Validators.required,
      ]),
      footer: new FormControl(this.Whatsappcampaign.footer, [
        Validators.required,
      ]),
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      id: new FormControl(this.Whatsappcampaign.id, [
      ]),
    });

  }

  get last_updated() {
    return this.reactiveForm.get('last_updated')!;
  }
  get Total_messages() {
    return this.reactiveForm.get('Total_messages')!;
  } get Sent_messages() {
    return this.reactiveForm.get('Sent_messages')!;
  }
  get Received_messages() {
    return this.reactiveForm.get('Received_messages')!;
  }
  get updated_date() {
    return this.reactiveForm.get('updated_date')!;
  }
  get version_id() {
    return this.reactiveForm.get('version_id')!;
  }
  get category() {
    return this.reactiveForm.get('category')!;
  }
  get value() {
    return this.reactiveForm.get('value1')!;
  }
  get category_change() {
    return this.reactiveForm.get('category_change')!;
  }
  get message_type() {
    return this.reactiveForm.get('message_type')!;
  }
  get project_type() {
    return this.reactiveForm.get('type')!;
  }
  get project_name() {
    return this.reactiveForm.get('name')!;
  }
  get project_description() {
    return this.reactiveForm.get('description')!;
  }
  get body() {
    return this.reactiveForm.get('body')!;
  }
  get footer() {
    return this.reactiveForm.get('footer')!;
  }
  get image() {
    return this.reactiveForm.get('image')!;
  }

  get template_description() {
    return this.reactiveForm.get('template_description')!;
  }
  get project_id() {
    return this.reactiveForm.get('project_id')!;
  }
  // get template_name() {
  //   return this.reactiveForm.get('template_name')!;
  // }

  
 
  getservicewindow() {
   
    var url = 'Whatsapp/Getservicewindowcontact'
    this.service.get(url).subscribe((result: any) => {
    });
  }
  //// Summary Grid//////
  Getcampaign() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/Getcampaign'
    this.service.get(url).subscribe((result: any) => {
      $('#whatsappCampaign').DataTable().destroy();
      this.responsedata = result;
      this.whatsappCampaign = this.responsedata.whatsappCampaign;
      this.project_count = this.responsedata.whatsappCampaign[0].project_count
      this.delivered_messages = this.responsedata.whatsappCampaign[0].delivered_messages
      this.lsmtd = this.responsedata.whatsappCampaign[0].lsmtd
      this.lsytd = this.responsedata.whatsappCampaign[0].lsytd
      this.send_campaign = this.responsedata.whatsappCampaign[0].send_campaign
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      setTimeout(() => {
        $('#whatsappCampaign').DataTable();
      }, 1);
    });
  }
  // Getmessagestatus() {
  //   var url = 'Whatsapp/Getmessagestatus'
  //   this.service.get(url).subscribe((result: any) => {
  //     if (result.status == false) {
  //       window.scrollTo({
  //         top: 0, 
  //       });
  //       this.ToastrService.warning(result.message)
  //     }
  //     else{
  //       this.responsedata = result;
  //     }
  //   });
  // }
  toggleOptions(project_id: any) {
    if (this.showOptionsDivId === project_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = project_id;
    }
  }

  onsend(params: any): void {
    const secretKey = 'storyboarderp';
    const project_id = AES.encrypt(params.project_id, secretKey).toString();
    const version_id = AES.encrypt(params.version_id, secretKey).toString();
    this.route.navigate(['/crm/CrmMstWabulkmessage', project_id, version_id])
  }

  GetTemplate() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetTemplate'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.NgxSpinnerService.hide();

    });
  }
  public whatsapplog(params: any): void {
    const secretKey = 'storyboarderp';
    const project_id = AES.encrypt(params.project_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmWhatsapplog', project_id])
  }
  GetContactCount() {
    var url = 'Whatsapp/GetContactCount'
    this.service.get(url).subscribe((result: any) => {
      $('#contactcount_list').DataTable().destroy();
      this.responsedata = result;
      this.contact_count1 = this.responsedata.contactcount_list;
      //console.log(this.source_list)

    });
  }
  GetWhatsappMessageCount() {
    var url = 'Whatsapp/GetWhatsappMessageCount'
    this.service.get(url).subscribe((result: any) => {
      $('#whatsappmessagescount').DataTable().destroy();
      this.responsedata = result;
      this.message_count = this.responsedata.whatsappmessagescount;
      //console.log(this.source_list)
      console.log("Received counts are" + this.message_count);
    });
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    console.log(this.parameterValue);
    var url = 'Whatsapp/DeleteCampaign'
    let param = {
      project_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
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

      this.Getcampaign();

    });

  }

  onclose() {
    this.fileInputs= null;

  }
  formatNumber(value: number): string {
    if (value >= 100000) {
      const num = value / 100000;
      if (num % 1 === 0) {
          return num.toFixed(0) + 'L';
      } else {
          return num.toFixed(1) + 'L';
      }
    } else if (value >= 1000) {
        const num = value / 1000;
        if (num % 1 === 0) {
            return num.toFixed(0) + 'k';
        } else {
            return num.toFixed(1) + 'k';
        }
    } else {
        return value.toString();
    }
}
  openModalview(project_id: any) {
    let params = {
      project_id: project_id
    }
    var url = 'Whatsapp/GetTemplatepreview'
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.templateview_list = this.responsedata.Gettemplateview_list;
      this.footers = this.responsedata.Gettemplateview_list[0].footer;
      this.media_url = result.Gettemplateview_list[0].media_url;
      this.file_type = result.Gettemplateview_list[0].file_type;



    });
  }

  GetWhatsappSummarycampaign() {
    var api = 'CampaignService/GetWhatsappSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignservice_list = this.responsedata.campaignservice_list;
      this.channel_name = this.responsedata.campaignservice_list[0].channel_name;
      this.mobile_number = this.responsedata.campaignservice_list[0].mobile_number;
    });
  }
  public toggleswitch(param: any): void {
    var api = 'Whatsapp/UpdateWhatsappCampaignStatus'
    this.service.post(api, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.Getcampaign();
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        this.Getcampaign();
      }
    });
  }
  popmodal(parameter: string,parameter1: string) {
    this.descriptions = parameter; // Access parameter directly
    this.Project_name = parameter1; // Access parameter directly

  }
 
   canceluploadexcel(){
    this.fileInputs= null;
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
  importexcel(evt:any) {
    debugger
    let header;
  const target: DataTransfer = <DataTransfer>(evt.target);
  this.isExcelFile = !! this.file.name.match(/(.xls|.xlsx)/); 
  
  if (this.isExcelFile) {
    this.spinnerEnabled = true;
    const reader: FileReader = new FileReader();
 
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
 
      /* grab first sheet */
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];
 
      /* save data */
      this.data = XLSX.utils.sheet_to_json(ws);
    };
 
    reader.readAsBinaryString(this.file);
 
    reader.onloadend = (e) => {
      if (this.data && this.data.length > 0) {
        if (this.isValidExcelData(this.data)) {
          this.spinnerEnabled = false;
          this.keys = Object.keys(this.data[0]);
          this.dataSheet.next(this.data);
          this.NgxSpinnerService.show();
          let formData = new FormData();
          if (this.file != null && this.file != undefined) {
            window.scrollTo({
              top: 0, // Code is used for scroll top after event done
            });
            formData.append("file", this.file, this.file.name);
            var api = 'Whatsapp/whatsappcontactsleadImport'
            this.service.postfile(api, formData).subscribe((result: any) => {
              this.responsedata = result;
              if (result.status == false) {
                this.NgxSpinnerService.hide();
                this.ToastrService.warning('Error While Occured Excel Upload')
              }
              else {
      
                this.NgxSpinnerService.hide();
                // window.location.reload();
                this.fileInputs= null;
                this.ToastrService.success("Leads are being added to contacts. Please wait until the process is complete.")
      
              }
             
            });
          }
        } else {
         // this.resetInputFile();
          alert("Some columns in the Excel data contain null values. Please provide valid data.");
        }
      } else {
       // this.resetInputFile();
       this.ToastrService.warning('No data found in the Excel file');
      }
    };
  } else {
    //this.resetInputFile();
    alert("Please select a valid Excel file.");
  }
}
 
isValidExcelData(data: any[]): boolean {
  // Check if data is empty
  if (!data || data.length === 0) {
    return false; // No data found, hence invalid
  }
 
  // Iterate over each row
  for (const row of data) {
    // Iterate over each column
    for (const key of Object.keys(row)) {
      // If any column is null or undefined, return false
      if (row[key] === null || row[key] === undefined) {
        return false;
      }
    }
  }
 
  return true; // All columns in all rows are not null, hence valid
    
   }

// ----------------------------------- start log details ----------------------------------------//

  getwhatspplist() {
    var api2 = 'Whatsapp/GetlogWhatsapplist';
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.whatsapp_list = this.responsedata.whatslog_list;
    });

  }
  ondetail(upload_gid: any) {

    let param = {
      document_gid: upload_gid
    }
    var api4 = 'Whatsapp/GetlogWhatsappdtllist';
    this.service.getparams(api4, param).subscribe((result: any) => {
      this.responsedata = result;
      this.whatsappdtl_list = this.responsedata.whatsdtllog_list;
    });
  }

  exportExcel(upload_gid :any): void {
    debugger
    let param = {
      document_gid: upload_gid
    }
    var api4 = 'Whatsapp/GetlogWhatsappdtllist';
    this.service.getparams(api4, param).subscribe((result: any) => {
      this.responsedata = result;
      this.whatsappdtl_list = this.responsedata.whatsdtllog_list;

      const WhatsApp_log = this.whatsappdtl_list.map(sitem => ({
        upload_gid : sitem.upload_gid || '',
        Name: sitem.leadbank_name || '',
        'Mobile Number': sitem.mobile || '',
        'Customer Type': sitem.customer_type || '',
      }));
      this.excelService.exportAsExcelFile(WhatsApp_log, 'WhatsApp_log', 'upload_gid');
    });

    
  }
// ========================================== invalid number log ===========================================//  
  getinvalidwhatspplist(){
    var api5 = 'Whatsapp/GetlogInvalidnumber';
    this.service.get(api5).subscribe((result: any) => {
      this.responsedata = result;
      this.invalidlog_list = this.responsedata.invalidlog_list;
    });
  }

  ondetail2(upload_gid: any) {

    let param = {
      document_gid: upload_gid
    }
    var api4 = 'Whatsapp/GetloginvalidDTLlist';
    this.service.getparams(api4, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invalidDTLlog_list = this.responsedata.invalidDTLlog_list;
    });
  }
  exportExcel2(upload_gid: any){
    let param = {
      document_gid: upload_gid
    }
    var api4 = 'Whatsapp/GetloginvalidDTLlist';
    this.service.getparams(api4, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invalidDTLlog_list = this.responsedata.invalidDTLlog_list;

      const WhatsApp_log = this.invalidDTLlog_list.map(sitem => ({
        upload_gid : sitem.upload_gid || '',
        Name: sitem.leadbank_name || '',
        'Mobile Number': sitem.mobile || '',
        'Customer Type': sitem.customer_type || '',
        'Remarks': sitem.remarks || '',
      }));
      this.excelService.exportAsExcelFile(WhatsApp_log, 'WhatsApp_log', 'upload_gid');
    });
  }
  
 // -----------------------------------end log details ----------------------------------------// 
}