import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'; 
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES , enc} from 'crypto-js';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface IVendor {
  rtgs_code:string;
  pan_number:string;
  vendorregister_gid:string;
  tin_number: string;
  servicetax_number: string;
  cst_number: string;
  excise_details: string;
  bank_details: string;
  ifsc_code: string;
  

}

@Component({
  selector: 'app-pmr-mst-vendor-additionalinformation',
  templateUrl: './pmr-mst-vendor-additionalinformation.component.html',
})
export class PmrMstVendorAdditionalinformationComponent {
  file!:File;
  reactiveForm!: FormGroup;
  vendor!: IVendor;
  responsedata:any;
  vendorregisteredit_list:any;
  vendorregister_gid:any;
  ViewVendorregister_list:any;
  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService,private ToastrService: ToastrService,private route:Router,private router: ActivatedRoute,private NgxSpinnerService :NgxSpinnerService ) {
    this.vendor = {} as IVendor;
  }
  ngOnInit(): void {
    debugger
    const vendorregister_gid = this.router.snapshot.paramMap.get('vendorregister_gid');
    // console.log(termsconditions_gid)
    this.vendorregister_gid = vendorregister_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendorregister_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetVendorRegisterDetail(deencryptedParam)
   
    
   
    
  this.reactiveForm = new FormGroup({  
    rtgs_code: new FormControl(this.vendor.rtgs_code, [
      Validators.required,
      Validators.maxLength(22),
      Validators.minLength(10),
    
    
    ]),
    tin_number: new FormControl(this.vendor.tin_number, [
      Validators.required,
      Validators.maxLength(11),
      Validators.minLength(10),
    
    ]),
        pan_number: new FormControl(this.vendor.pan_number, [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
    
    ]),

    cst_number: new FormControl(this.vendor.cst_number, [
      Validators.required,
      Validators.maxLength(11),
      Validators.minLength(11),
    
    ]),

    servicetax_number: new FormControl(this.vendor.servicetax_number, [
      Validators.required,
      Validators.maxLength(15),
      Validators.minLength(15),
    
    ]),
    excise_details: new FormControl(this.vendor.excise_details, [
      Validators.required,
      Validators.maxLength(100),
      Validators.minLength(0),
    
    ]),
    bank_details: new FormControl(this.vendor.bank_details, [
      Validators.required,
      Validators.maxLength(100),
      Validators.minLength(0),
    
    ]),
    ifsc_code: new FormControl(this.vendor.ifsc_code, [
      Validators.required,
      Validators.maxLength(11),
      Validators.minLength(11),
    
    ]),
    vendorregister_gid: new FormControl(this.vendor.vendorregister_gid,[
    Validators.required,])
  });

}
GetVendorRegisterDetail(vendorregister_gid: any) {
  debugger
  var url1='PmrMstVendorRegister/GetVendorRegisterDetail'
  let param = {
    vendorregister_gid : vendorregister_gid 
  }
  this.service.getparams(url1, param).subscribe((result: any) => {
    // this.responsedata=result;
    this.vendorregisteredit_list = result.editvendorregistersummary_list;
    console.log(this.vendorregisteredit_list)
    // console.log(this.vendorregisteredit_list[0].vendorregister_gid)
    // this.selectedVendorcode = this.vendorregisteredit_list[0].vendorregister_gid;
    this.reactiveForm.get("vendorregister_gid")?.setValue(this.vendorregisteredit_list[0].vendorregister_gid);
    this.reactiveForm.get("rtgs_code")?.setValue(this.vendorregisteredit_list[0].rtgs_code);
    this.reactiveForm.get("pan_number")?.setValue(this.vendorregisteredit_list[0].pan_number);
    this.reactiveForm.get("tin_number")?.setValue(this.vendorregisteredit_list[0].tin_number);
    this.reactiveForm.get("servicetax_number")?.setValue(this.vendorregisteredit_list[0].servicetax_number);
    this.reactiveForm.get("cst_number")?.setValue(this.vendorregisteredit_list[0].cst_number);
    this.reactiveForm.get("excise_details")?.setValue(this.vendorregisteredit_list[0].excise_details);
    this.reactiveForm.get("bank_details")?.setValue(this.vendorregisteredit_list[0].bank_details);
    this.reactiveForm.get("ifsc_code")?.setValue(this.vendorregisteredit_list[0].ifsc_code);
  });
}
onChange2(event:any) {
  this.file =event.target.files[0];

  }
 

get rtgs_code() {
  return this.reactiveForm.get('rtgs_code')!;
}
get tin_number() {
  return this.reactiveForm.get('tin_number')!;
}
get pan_number() {
  return this.reactiveForm.get('pan_number')!;
}
get cst_number() {
  return this.reactiveForm.get('cst_number')!;
}
get servicetax_number() {
  return this.reactiveForm.get('servicetax_number')!;
}
get excise_details() {
  return this.reactiveForm.get('excise_details')!;
}
get bank_details() {
  return this.reactiveForm.get('bank_details')!;
}
get ifsc_code() {
  return this.reactiveForm.get('ifsc_code')!;
}

public validate(): void {console.log(this.reactiveForm.value)
  this.vendor = this.reactiveForm.value;
  if(   this.vendor.vendorregister_gid != null   ){
    let formData = new FormData();
    if(this.file !=null &&  this.file != undefined){
   formData.append("vendorregister_gid", this.vendor.rtgs_code);
   formData.append("rtgs_code", this.vendor.rtgs_code);
   formData.append(" pan_number", this.vendor. pan_number);
   formData.append("tin_number", this.vendor.tin_number);
   formData.append("servicetax_number", this.vendor.servicetax_number);
   formData.append("cst_number", this.vendor.cst_number);
   formData.append("excise_details", this.vendor.excise_details);
   formData.append("bank_details", this.vendor.bank_details);
   formData.append("ifsc_code", this.vendor.ifsc_code);
 
    var api='PmrMstVendorRegister/PostVendorRegisterAdditionalInformation'
    this.NgxSpinnerService.show();
      this.service.postfile(api,formData).subscribe((result:any) => {
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
        }
        else{
          this.route.navigate(['/pmr/PmrMstVendorregister']);
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
        }
      });
  
  }
  else{
    var api7='PmrMstVendorRegister/PostVendorRegisterAdditionalInformation'
    this.NgxSpinnerService.show();
      this.service.post(api7,this.vendor).subscribe((result:any) => {

        if(result.status ==false){
          this.ToastrService.warning(result.message)
        }
        else{
          this.route.navigate(['/pmr/PmrMstVendorregister']);
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
        }
        this.responsedata=result;
      });
  }
  }
  else{
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
  return;


}
}
