import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-pmr-mst-purchasetype',
  templateUrl: './pmr-mst-purchasetype.component.html',
  styleUrls: ['./pmr-mst-purchasetype.component.scss']
})
export class PmrMstPurchasetypeComponent {
  responsedata: any;
  purchasetype_list: any[] = [];
  purchaseaddForm: FormGroup | any;
  purchaseEditForm: FormGroup | any;
  parameterValue1: any;
  parameterValue: any;
  purchasetypeedit_code:any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }
  ngOnInit(): void {
    this.Getpurchasetypesummary();
    this.purchaseaddForm = new FormGroup({
      purchasetype_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      purchasetype_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
    });
    this.purchaseEditForm = new FormGroup({
      purchasetypeedit_code: new FormControl(''),
      purchasetypeedit_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      purchasetype_gid: new FormControl(''),
    });
  }
  Getpurchasetypesummary(){
    var url = 'PmrMstPurchaseType/GetpurchaseType'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     $('#purchasetype_list').DataTable().destroy();
      this.responsedata = result;
      this.purchasetype_list = this.responsedata.GetpurchaseType_List;
      setTimeout(() => {
        $('#purchasetype_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()
  
    });

  }
  get purchasetype_code() {
    return this.purchaseaddForm.get('purchasetype_code')!;
  }
  get purchasetype_name() {
    return this.purchaseaddForm.get('purchasetype_name')!;
  }
  get purchasetypeedit_name() {
    return this.purchaseEditForm.get('purchasetypeedit_name')!;
  }
  Submit_purchasetype() {
    debugger
    if ( this.purchaseaddForm.value.purchasetype_name != '') {

      for (const control of Object.keys(this.purchaseaddForm.controls)) {
        this.purchaseaddForm.controls[control].markAsTouched();
      }
      this.purchaseaddForm.value;
      var url='PmrMstPurchaseType/PostPmrpurchaseType'
      this.NgxSpinnerService.show()
      this.service.post(url,this.purchaseaddForm.value).subscribe((result:any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.Getpurchasetypesummary();
        }
        else{
          this.purchaseaddForm.get("purchasetype_code")?.setValue(null);
          this.purchaseaddForm.get("purchasetype_name")?.setValue(null);
          this.ToastrService.success(result.message) 
          this.purchaseaddForm.reset();
          this.Getpurchasetypesummary();
          this.NgxSpinnerService.hide();
        }
 });
            
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaledit(parameter: string) {
    debugger
    this.parameterValue1 = parameter
    this.purchasetypeedit_code =this.parameterValue1.purchasetype_code;
    this.purchaseEditForm.get("purchasetypeedit_name")?.setValue(this.parameterValue1.purchasetype_name);
    this.purchaseEditForm.get("purchasetype_gid")?.setValue(this.parameterValue1.purchasetype_gid);
   
  }

  Update_purchasetype(){

    debugger
    if ( this.purchaseEditForm.value.purchasetypeedit_name != '') {

      for (const control of Object.keys(this.purchaseEditForm.controls)) {
        this.purchaseEditForm.controls[control].markAsTouched();
      }
        let params= {
          purchasetype_gid : this.purchaseEditForm.value.purchasetype_gid ,
          purchasetype_name : this.purchaseEditForm.value.purchasetypeedit_name ,
          purchasetype_code : this.purchaseEditForm.value.purchasetypeedit_code 
        }
      var url='PmrMstPurchaseType/UpdatePmrpurchaseType'
      this.NgxSpinnerService.show()
      this.service.postparams(url,params).subscribe((result:any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.Getpurchasetypesummary();
        }
        else{
          this.purchaseEditForm.get("purchasetypeedit_code")?.setValue(null);
          this.purchaseEditForm.get("purchasetypeedit_name")?.setValue(null);
          this.ToastrService.success(result.message) 
          this.purchaseEditForm.reset();
          this.Getpurchasetypesummary();
          this.NgxSpinnerService.hide();
        }
      });
            
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  ondelete() {
    var url = 'PmrMstPurchaseType/GetDeletepurchasetype'
    let param = {
      purchasetype_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
      this.Getpurchasetypesummary();
     
      this.NgxSpinnerService.show();
  
    });
  }
}
