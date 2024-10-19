import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface Iserver {
  server_name: string;
  hosting_details: string;
  token_number: string;
  server_ipaddress: string;
  cbopermanent_country: string;
  server_status:string;
}

@Component({
  selector: 'app-sbc-mst-serversummary',
  templateUrl: './sbc-mst-serversummary.component.html',
  styleUrls: ['./sbc-mst-serversummary.component.scss']
})
export class SbcMstServersummaryComponent {
  server_list:any;
  response_data :any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  server_gid: any;
  consumer_list:any[]=[];
  txthosting_details:any;
  txtserver_name:any;
  txttoken_number:any;
  txtserver_ipaddress:any;
  txtserver_status:any; 
  txtcountry_name:any;
  countryList: any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {} 
  ngOnInit(): void {
    this.GetServerSummary();
    this.reactiveForm = new FormGroup({
      
      server_name: new FormControl('', [Validators.required]),
      hosting_details: new FormControl('', [Validators.required]),
      token_number: new FormControl('', [Validators.required]),
      server_ipaddress: new FormControl('', [Validators.required]),
      cbopermanent_country: new FormControl('', [Validators.required]),
      server_status: new FormControl('', [Validators.required]),

    });
    this.reactiveFormEdit = new FormGroup({
      
      server_name: new FormControl('', [Validators.required]),
      hosting_details: new FormControl('', [Validators.required]),
      token_number: new FormControl('', [Validators.required]),
      server_ipaddress: new FormControl('', [Validators.required]),
      server_gid: new FormControl('', [Validators.required]),
      server_status: new FormControl('', [Validators.required]),
      cbopermanent_country: new FormControl('', [Validators.required]),


    });
    var url = 'SubTrnSubscrition/PopCountry';
    this.service.get(url).subscribe((result: any) => {
      this.countryList = result.country;
    });
  }
  GetServerSummary() {
   
    const api = 'SubTrnSubscrition/GetServerSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.server_list = this.response_data.server_list;
      setTimeout(() => {  
        $('#server_list').DataTable({
          order: [] // Disable initial sorting
        });
      }, 1);
 
    });
  }
// Add popup validtion //
 
get server_name() {
  return this.reactiveForm.get('server_name')!;
}
get hosting_details() {
  return this.reactiveForm.get('hosting_details')!;
}
get token_number() {
  return this.reactiveForm.get('token_number')!;
}
get server_ipaddress() {
  return this.reactiveForm.get('server_ipaddress')!;
}
get server_status() {
  return this.reactiveForm.get('server_status')!;
}
get cbopermanent_country() {
  return this.reactiveForm.get('cbopermanent_country')!;
}
 // Edit popup validtion //
 
get server_nameedit() {
  return this.reactiveFormEdit.get('server_name')!;
}
get hosting_detailsedit() {
  return this.reactiveFormEdit.get('hosting_details')!;
}
get token_numberedit() {
  return this.reactiveFormEdit.get('token_number')!;
}
get server_ipaddressedit() {
  return this.reactiveFormEdit.get('server_ipaddress')!;
}
// Add popup //
public onsubmit(): void {
    for (const control of Object.keys(this.reactiveForm.controls)) {
      this.reactiveForm.controls[control].markAsTouched();
    }
    this.reactiveForm.value;
    var url = 'SubTrnSubscrition/PostServer'
    this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        // this.GetEntitySummary();
        this.reactiveForm.reset();
      }
      else {
        this.reactiveForm.get("server_name")?.setValue(null);
        this.reactiveForm.get("hosting_details")?.setValue(null);
        this.reactiveForm.get("token_number")?.setValue(null);
        this.reactiveForm.get("server_ipaddress")?.setValue(null);
        this.reactiveForm.get("status")?.setValue(null);
        this.reactiveForm.get("cbopermanent_country")?.setValue(null);
        this.NgxSpinnerService.hide();

        this.ToastrService.success(result.message)
      this.reactiveForm.reset();
      this.NgxSpinnerService.hide();
      this.GetServerSummary();
    }
    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  
 this.reactiveForm.reset();
 
}
// Edit popup //
openModaledit(parameter: string) {
  this.parameterValue1 = parameter
  this.reactiveFormEdit.get("server_name")?.setValue(this.parameterValue1.server_name);

  this.reactiveFormEdit.get("hosting_details")?.setValue(this.parameterValue1.hosting_details);
  this.reactiveFormEdit.get("token_number")?.setValue(this.parameterValue1.token_number);
  this.reactiveFormEdit.get("server_ipaddress")?.setValue(this.parameterValue1.server_ipaddress);
  this.reactiveFormEdit.get("server_gid")?.setValue(this.parameterValue1.server_gid);
  this.reactiveFormEdit.get("server_status")?.setValue(this.parameterValue1.server_status);
  this.reactiveFormEdit.get("cbopermanent_country")?.setValue(this.parameterValue1.country_name);

}
isFormEdited(): boolean {
  return !Object.keys(this.reactiveFormEdit.controls).every(control => {
    return this.reactiveFormEdit.controls[control].pristine;
  });
}
 // Update popup //
 onupdate() {
    for (const control of Object.keys(this.reactiveFormEdit.controls)) {
      this.reactiveFormEdit.controls[control].markAsTouched();
    }
    this.reactiveFormEdit.value;
    
  var url = 'SubTrnSubscrition/UpdateServer'
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
oncloseadd(){
  this.reactiveForm.reset()

}
view(server_gid: any) {
  this.server_gid = server_gid;
  this.NgxSpinnerService.show();
  var param = {
    server_gid: server_gid,
  }
  var url = 'SubTrnSubscrition/GetServerView';
  this.service.getparams(url, param).subscribe((result: any) => {
  
    this.consumer_list = result.server_list;
    this.txtserver_name = result.server_name;
    this.txthosting_details = result.hosting_details;
    this.txttoken_number = result.token_number;
    this.txtserver_ipaddress = result.server_ipaddress;
    this.txtserver_status = result.server_status;
    this.txtcountry_name = result.country_name;

    this.NgxSpinnerService.hide();
  });
}

autoGrow(event: Event) {
  const textArea = event.target as HTMLTextAreaElement;
  textArea.style.height = 'auto';
  textArea.style.height = textArea.scrollHeight + 'px';
}

}
