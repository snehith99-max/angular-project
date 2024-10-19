import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

interface IWhatsappcampaign {
  created_date:string;
  direction:string;
  status:string;
  identifiervalue:string;
  displayName:string;
  CompanyName:string;
  wvalue:string;
  formatDate:string;
  

  
  }
@Component({
  selector: 'app-crm-smm-whatsapplog',
  templateUrl: './crm-smm-whatsapplog.component.html',
  styleUrls: ['./crm-smm-whatsapplog.component.scss']
})
export class CrmSmmWhatsapplogComponent {
  whatsappCampaign: any;
  responsedata: any;
  
  contactcount_list:any;
  file!: File;
  image_path: any;
  formatDate:any;
  template_list: any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormadd!: FormGroup;
  reactiveMessageForm!:FormGroup;
  project_id:any;
  Whatsappcampaign!: IWhatsappcampaign;
  parameterValue1: any;
  //reactiveFormEdit: any;
  parameterValue: any;
  count_list: any;
  log: any;
  p_name:any;
  reason:any;
 
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.Whatsappcampaign = {} as IWhatsappcampaign;
    
    function formatDate(isoDate: string): string {
      const options: Intl.DateTimeFormatOptions = {
        day: 'numeric',
        month: 'long',
        year: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric'
      };
    
      const date = new Date(isoDate);
      return date.toLocaleString('en-US', options);
    }
    
}
ngOnInit(): void {
  const project_id = this.router.snapshot.paramMap.get('project_id');
  this.project_id = project_id;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.project_id, secretKey).toString(enc.Utf8);
  this.project_id = deencryptedParam;
  this.Getmessagestatus();
  this.Getlog();

  this.reactiveForm = new FormGroup({
    created_date: new FormControl(this.Whatsappcampaign.created_date, [
      Validators.required,
    ]),
    direction: new FormControl(this.Whatsappcampaign.direction, [
    ]),
    status: new FormControl(this.Whatsappcampaign.status, [
    ]),
    identifiervalue: new FormControl(this.Whatsappcampaign.identifiervalue, [
    ]),
    displayName: new FormControl(this.Whatsappcampaign.displayName, [
    ]),
    wvalue: new FormControl(this.Whatsappcampaign.wvalue, [
    ]),
    
    
  });

}

get created_date() {
  return this.reactiveForm.get('created_date')!;
}
get wvalue() {
  return this.reactiveForm.get('wvalue')!;
}
get displayName() {
  return this.reactiveForm.get('displayName')!;
}
get identifiervalue() {
  return this.reactiveForm.get('identifiervalue')!;
}
get status() {
  return this.reactiveForm.get('status')!;
}
get direction() {
  return this.reactiveForm.get('direction')!;
}
onChange1(event: any) {
  this.file = event.target.files[0];
}



Getmessagestatus() {
  var url = 'Whatsapp/Getmessagestatus'
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
  });
}
  //// Summary Grid//////
  Getlog(){
    this.NgxSpinnerService.show();
    let param = {
      project_id: this.project_id,
    }
  var api = 'Whatsapp/Getlog'
  this.service.getparams(api,param).subscribe((result: any) => {
    $('#log').DataTable().destroy();
    this.responsedata = result;
    this.NgxSpinnerService.hide();
    setTimeout(() => {
      $('#log').DataTable();
    }, 1);
    this.log = this.responsedata.log;
    this.p_name = this.log[0].p_name;
   


  });
}
popmodal(parameter: string) {
  this.reason = parameter
 
} 

redirecttolist(){
  this.route.navigate(['/crm/CrmSmmWhatsappcampaign']);

}

}
