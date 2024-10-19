import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
interface IStatutory {
  form_name: string;
  processed_year: string;
}

@Component({
  selector: 'app-hrm-trn-statutoryforms',
  templateUrl: './hrm-trn-statutoryforms.component.html',
  styleUrls: ['./hrm-trn-statutoryforms.component.scss']
})
export class HrmTrnStatutoryformsComponent {
  reactiveForm!: FormGroup;
  yearreturn_list: any[] = [];
  formList: any[] = [];
  responsedata: any;
  parameterValue: any;
  statutory!: IStatutory;
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
  ) {
    this.statutory = {} as IStatutory;
  }
  ngOnInit(): void {


  this.GetYearReturnSummary();
  this.reactiveForm = new FormGroup({
    form_name: new FormControl(this.statutory.form_name, [Validators.required, Validators.minLength(1), Validators.maxLength(250)]),
    processed_year: new FormControl(this.statutory.processed_year, [Validators.required,]),
  });

  var url = 'HrmForm22/Getformdropdown';
  this.service.get(url).subscribe((result: any) => {
    this.formList = result.Getformdropdown;
  });
  
}

onhalfyear(params:any,params1:any){
  debugger;
  const secretKey = 'storyboarderp';
  const param = (params+'+'+params1);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/hrm/HrmForm22',encryptedParam])  
}

onannualyear(params2:any,params3:any) {
  debugger;
  const secretKey = 'storyboarderp';
  const param = (params2+'+'+params3);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/hrm/annualform22',encryptedParam])  
}


GetYearReturnSummary() {
    var url = 'HrmForm22/GetYearReturnSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.yearreturn_list = this.responsedata.yearreturn_list;
      setTimeout(() => {
        $('#yearreturn').DataTable();
      }, );


    });
  }
   get form_name() {
     return this.reactiveForm.get('form_name')!;
   }
   get processed_year() {
     return this.reactiveForm.get('processed_year')!;
   }

  onsubmit(){
    if (this.reactiveForm.value.form_name != null && this.reactiveForm.value.form_name != '')
    {

          this.reactiveForm.value;
          var url = 'HrmForm22/PostForm'
          this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {

            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetYearReturnSummary();  
            }
            else 
            {
              this.reactiveForm.get("form_gid")?.setValue(null);
              this.reactiveForm.get("form_name")?.setValue(null);
              this.reactiveForm.get("processed_year")?.setValue(null);
              
              this.ToastrService.success(result.message)
              this.GetYearReturnSummary();
              

            }

          });

        }
        else {
          this.ToastrService.warning('result.message')
        }
        this.reactiveForm.reset();
  }

  onclose(){

  }
 
  openModaldelete(parameter: string){
    this.parameterValue = parameter
  }

  ondelete(){
    console.log(this.parameterValue);
    var url = 'HrmForm22/DeleteForm22'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.GetYearReturnSummary();

    });
  }

}
