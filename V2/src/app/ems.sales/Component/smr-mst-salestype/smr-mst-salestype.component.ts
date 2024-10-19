import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-smr-mst-salestype',
  templateUrl: './smr-mst-salestype.component.html',
  styleUrls: ['./smr-mst-salestype.component.scss']
})
export class SmrMstSalestypeComponent {
  responsedata: any;
  Salestype_list: any[] = [];
  salesaddForm: FormGroup | any;
  salesEditForm: FormGroup | any;
  parameterValue1: any;
  parameterValue: any;
  salestypeedit_code:any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }
  ngOnInit(): void {
    this.Getsalestypesummary();
    this.salesaddForm = new FormGroup({
      salestype_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      salestype_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
    });
    this.salesEditForm = new FormGroup({
      salestypeedit_code: new FormControl(''),
      salestypeedit_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      salestype_gid: new FormControl(''),
    });
  }
  Getsalestypesummary(){
    var url = 'SmrMstSalesType/GetsalesType'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     $('#Salestype_list').DataTable().destroy();
      this.responsedata = result;
      this.Salestype_list = this.responsedata.GetSalesType_List;
      setTimeout(() => {
        $('#Salestype_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()
  
    });

  }
  get salestype_code() {
    return this.salesaddForm.get('salestype_code')!;
  }
  get salestype_name() {
    return this.salesaddForm.get('salestype_name')!;
  }
  get salestypeedit_name() {
    return this.salesEditForm.get('salestypeedit_name')!;
  }
  Submit_Salestype() {
    debugger
    if ( this.salesaddForm.value.salestype_name != '') {

      for (const control of Object.keys(this.salesaddForm.controls)) {
        this.salesaddForm.controls[control].markAsTouched();
      }
      this.salesaddForm.value;
      var url='SmrMstSalesType/PostSmrSalesType'
      this.NgxSpinnerService.show()
      this.service.post(url,this.salesaddForm.value).subscribe((result:any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.Getsalestypesummary();
        }
        else{
          this.salesaddForm.get("salestype_code")?.setValue(null);
          this.salesaddForm.get("salestype_name")?.setValue(null);
          this.ToastrService.success(result.message) 
          this.salesaddForm.reset();
          this.Getsalestypesummary();
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
    this.salestypeedit_code =this.parameterValue1.salestype_code;
    this.salesEditForm.get("salestypeedit_name")?.setValue(this.parameterValue1.salestype_name);
    this.salesEditForm.get("salestype_gid")?.setValue(this.parameterValue1.salestype_gid);
   
  }

  Update_Salestype(){

    debugger
    if ( this.salesEditForm.value.salestypeedit_name != '') {

      for (const control of Object.keys(this.salesEditForm.controls)) {
        this.salesEditForm.controls[control].markAsTouched();
      }
        let params= {
          salestype_gid : this.salesEditForm.value.salestype_gid ,
          salestype_name : this.salesEditForm.value.salestypeedit_name ,
          salestype_code : this.salesEditForm.value.salestypeedit_code 
        }
      var url='SmrMstSalesType/UpdateSmrSalesType'
      this.NgxSpinnerService.show()
      this.service.postparams(url,params).subscribe((result:any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.Getsalestypesummary();
        }
        else{
          this.salesEditForm.get("salestypeedit_code")?.setValue(null);
          this.salesEditForm.get("salestypeedit_name")?.setValue(null);
          this.ToastrService.success(result.message) 
          this.salesEditForm.reset();
          this.Getsalestypesummary();
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
    var url = 'SmrMstSalesType/GetDeleteSalestype'
    let param = {
      salestype_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
      this.Getsalestypesummary();
     
      this.NgxSpinnerService.show();
  
    });
  }
}
