import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { ExcelService } from 'src/app/Service/excel.service';

interface Iconsumer {
  company_code: string;
  consumer_url: string;
  subscription_details: string;
  
}
@Component({
  selector: 'app-sbc-mst-consumersummary',
  templateUrl: './sbc-mst-consumersummary.component.html',
  styleUrls: ['./sbc-mst-consumersummary.component.scss']
})
export class SbcMstConsumersummaryComponent {

  consumer_list:any[]=[];
  response_data :any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormadd!: FormGroup;
  server_list:any;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  consumerstatusForm!:FormGroup;
  remainingChars: any | number = 125;
  mdlserver_name:any;
statusFormData = {
  txtconsumername: '',
  rbo_status: '',
  txtremarks: '',
};
consumerlog_data:any;
consumer_gid:any;
current_status:any;
  constructor(private excelService : ExcelService,private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private SocketService: SocketService,private ToastrService: ToastrService,public FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService) {} 
  ngOnInit(): void {
    this.statusForm();
    this.GetConsumerSummary();
    this.reactiveForm = new FormGroup({
    file: new FormControl(''),
    });
    var api = 'Scriptmanagement/Getserverdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.server_list = this.responsedata.serverlists;
    });
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    this.reactiveFormEdit = new FormGroup({
      
      company_code: new FormControl('', [Validators.required]),
      consumer_url: new FormControl('', [Validators.required]),
      server_name: new FormControl(''),
      server_gid: new FormControl(''),
      consumer_gid: new FormControl(''),
      from:new FormControl(''),
      to: new FormControl('')
    });
    this.reactiveFormadd = new FormGroup({
      consumer_url:new FormControl(''),
      company_name:new FormControl(''),
      server_name:new FormControl(''),
      from:new FormControl(''),
      to:new FormControl(''),
     


    });
  }
  statusForm(){
    this.consumerstatusForm =this.FormBuilder.group({
      rbo_status:['',Validators.required],
      txtremarks:[null, [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
    });
  }
  GetConsumerSummary() {
    const api = 'SubTrnSubscrition/GetConsumerSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.consumer_list = this.response_data.consumer_list;
      setTimeout(() => {  
        $('#consumer_list').DataTable({
          order: [] // Disable initial sorting
        });
      }, 1);
 
    });
  }
  
 // Edit popup validtion //
 
get company_code() {
  return this.reactiveFormEdit.get('company_code')!;
}
get consumer_url() {
  return this.reactiveFormEdit.get('consumer_url')!;
}
get subscription_details() {
  return this.reactiveFormEdit.get('subscription_details')!;
}


// Edit popup //
openModaledit(parameter: string) {
  debugger
  this.parameterValue1 = parameter
  this.reactiveFormEdit.get("company_code")?.setValue(this.parameterValue1.company_code);
  this.reactiveFormEdit.get("from")?.setValue(this.parameterValue1.start_date);
  this.reactiveFormEdit.get("to")?.setValue(this.parameterValue1.end_date);
  this.reactiveFormEdit.get("consumer_url")?.setValue(this.parameterValue1.consumer_url);
  this.reactiveFormEdit.get("server_name")?.setValue(this.parameterValue1.server_name);
  this.reactiveFormEdit.get("server_gid")?.setValue(this.parameterValue1.server_gid);
  this.reactiveFormEdit.get("consumer_gid")?.setValue(this.parameterValue1.consumer_gid);

}
isFormEdited(): boolean {
  return !Object.keys(this.reactiveFormEdit.controls).every(control => {
    return this.reactiveFormEdit.controls[control].pristine;
  });
}
 // Update popup //
 onupdate() {
  debugger
    for (const control of Object.keys(this.reactiveFormEdit.controls)) {
      this.reactiveFormEdit.controls[control].markAsTouched();
    }
    this.reactiveFormEdit.value;
    
  var url = 'SubTrnSubscrition/UpdateConsumer'
  this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
    this.responsedata = result;
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.ToastrService.success(result.message)
    }
  });
  setTimeout(function () {
    window.location.reload();
  }, 2000);
  
}

onclose(){
  this.reactiveFormEdit.reset()

}
//Status remarks character count
updateRemainingCharsadd() {
  this.remainingChars = 125 - this.consumerstatusForm.value.txtremarks.length;
}  
updateRemainingCharsAfterClear(previousLength: number) {
  this.remainingChars = 125; // Recalculate remainingChars after clearing the form
}
//status get function
status(consumer_gid: any)
{
  this.consumer_gid = consumer_gid
  this.consumerstatusForm.reset();
  var params = {
    consumer_gid: consumer_gid
}
const teamDescriptionLength = this.consumerstatusForm.value.txtremarks ? this.consumerstatusForm.value.txtremarks.length : 0; // Store the length of team_description
this.updateRemainingCharsAfterClear(teamDescriptionLength); // Recalculate remainingChars after clearing the form
this.NgxSpinnerService.show();
var url = 'SubTrnSubscrition/GetConsumerEdit';
this.service.getparams(url, params).subscribe((result:any) => {
this.statusFormData.txtconsumername = result.company_code;
this.statusFormData.rbo_status = result.active_status;
this.current_status = this.statusFormData.rbo_status;
this.NgxSpinnerService.hide();
}); 
  var url ='SubTrnSubscrition/InactiveConsumerHistory';
    this.service.getparams(url, params).subscribe((result:any) => {
    this.consumerlog_data = result.consumerhistory_list;
    this.NgxSpinnerService.hide();   
  });
}
//Status post funtion
Statusaudittype(){
  if(this.current_status == this.statusFormData.rbo_status){
    if (this.statusFormData.rbo_status === 'N')
    this.ToastrService.warning("Consumer Inactive.");
    if (this.statusFormData.rbo_status === 'Y')
    this.ToastrService.warning("Consumer is already active.");
  }
  else{
    this.NgxSpinnerService.show();
    var params = {
      consumer_gid : this.consumer_gid,
      remarks: this.consumerstatusForm.value.txtremarks,
      rbo_status:this.consumerstatusForm.value.rbo_status
  }
  var url = 'SubTrnSubscrition/InactiveConsumer';
  this.service.post(url, params).subscribe((result:any) => {
    this.NgxSpinnerService.hide();
      if(result.status == true){
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
          }
          else {
            this.ToastrService.info(result.message)
            this.NgxSpinnerService.hide();
          }  
          setTimeout(function () {
            window.location.reload();
          }, 2000);
      })
    }
}
onsubmit(){
  this.NgxSpinnerService.show();
  var url = 'SubTrnSubscrition/PostConsumer';
  this.SocketService.postparams(url, this.reactiveFormadd.value).subscribe((result: any) => {
    if (result.status == true) {
      this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message);
      this.reactiveFormadd.reset();
      this.NgxSpinnerService.hide();
      this.GetConsumerSummary();
    }
    else {
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
    }
  })
}
exportExcel():void{
  const consumersummary = this.consumer_list.map(item => ({
    ServerName: item.server_name || '', 
    CompanyName: item.company_code || '', 
    ConsumerURL: item.consumer_url || '', 
    Subscription: item.subscription_details || '', 
    StartDate: item.start_date || '', 
    EndDate: item.end_date || '', 
  }));
  this.excelService.exportAsExcelFile(consumersummary, 'Consumer');
}

}
