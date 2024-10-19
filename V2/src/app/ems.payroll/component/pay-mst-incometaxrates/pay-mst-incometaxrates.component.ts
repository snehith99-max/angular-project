import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { R } from '@fullcalendar/core/internal-common';

interface IIncome {
 
  tax_slabs_fromold: string;
  tax_slabs_fromnew: string;
  tax_slabs_toold: string;
  tax_slabs_tonew: string;
  individuals: string;
  resident_senior_citizens: string;
  resident_super_senior_citizens: string;
  income_tax_rates: string;
  tax_slabs_fromoldedit: string;
  tax_slabs_tooldedit: string;
  tax_slabs_fromnewedit: string;
  tax_slabs_tonewedit: string;
  individuals_edit: string;
  resident_senior_citizensedit: string;
  resident_super_senior_citizensedit: string;
  income_tax_ratesedit: string;
  tax_regime_gid: string;
  
}

@Component({
  selector: 'app-pay-mst-incometaxrates',
  templateUrl: './pay-mst-incometaxrates.component.html',
  styleUrls: ['./pay-mst-incometaxrates.component.scss']
})
export class PayMstIncometaxratesComponent {
  tax_name: any;
   tax_name_new: any;
  tax_nameedit: any;
  // tax_name_newedit: any;
  taxperiod: any;
  responsedata: any;
  incomemaster_list: any[] = [];
  incomemasternew_list: any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  income!: IIncome;
  parameterValue1: any;

  constructor(public service: SocketService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
    this.income = {} as IIncome;
  }

  ngOnInit(): void {
    this.GetIncomeTaxMasterSummary();
    this.GetIncomeTaxMasterNew();

    this.reactiveForm = new FormGroup({
      tax_name: new FormControl(''),
      tax_name_new: new FormControl(''),
      tax_slabs_fromold: new FormControl(this.income.tax_slabs_fromold, [ Validators.required,]),
      tax_slabs_fromnew: new FormControl(this.income.tax_slabs_fromnew, [ Validators.required,]),
      tax_slabs_toold: new FormControl(this.income.tax_slabs_toold, [ Validators.required,]),
      tax_slabs_tonew: new FormControl(this.income.tax_slabs_tonew, [ Validators.required,]),
      individuals: new FormControl(this.income.individuals, [ Validators.required,]),
      resident_senior_citizens: new FormControl(this.income.resident_senior_citizens, [ Validators.required,]),
      resident_super_senior_citizens: new FormControl(this.income.resident_super_senior_citizens, [ Validators.required,]),
      income_tax_rates: new FormControl(this.income.income_tax_rates, [ Validators.required,]),
      remarks_old: new FormControl(''),
      remarks_new: new FormControl(''),
      tax_regime_gid: new FormControl(''),
      

    });

    this.reactiveFormEdit = new FormGroup({
      tax_nameedit: new FormControl(''),
      // tax_name_newedit: new FormControl(''),
      tax_slabs_fromoldedit: new FormControl(this.income.tax_slabs_fromoldedit, [ Validators.required,]),
      tax_slabs_tooldedit: new FormControl(this.income.tax_slabs_tooldedit, [ Validators.required,]),
      tax_slabs_fromnewedit: new FormControl(this.income.tax_slabs_fromnewedit, [ Validators.required,]),
      tax_slabs_tonewedit: new FormControl(this.income.tax_slabs_tonewedit, [ Validators.required,]),
      individuals_edit: new FormControl(this.income.individuals_edit, [ Validators.required,]),
      resident_senior_citizensedit: new FormControl(this.income.resident_senior_citizensedit, [ Validators.required,]),
      resident_super_senior_citizensedit: new FormControl(this.income.resident_super_senior_citizensedit, [ Validators.required,]),
      income_tax_ratesedit: new FormControl(this.income.income_tax_ratesedit, [ Validators.required,]),
      remarksold_edit: new FormControl(''),
      remarksnew_edit: new FormControl(''),
      tax_regime_gid: new FormControl(''),
      


    });

  }

  GetIncomeTaxMasterSummary() {
  var url = 'PayMstIncomeTax/GetIncomeTaxMasterSummary'
  this.service.get(url).subscribe((result: any) => {

    this.responsedata = result;
    this.incomemaster_list = this.responsedata.GetIncomeMaster_list;
    // setTimeout(() => {
    //   $('#incomemaster_list').DataTable();
    // }, 1);
  });
}

GetIncomeTaxMasterNew() {
  var url = 'PayMstIncomeTax/GetIncomeTaxMasterNew'
  this.service.get(url).subscribe((result: any) => {

    this.responsedata = result;
    this.incomemasternew_list = this.responsedata.GetIncomeMasterNew_list;
    // setTimeout(() => {
    //   $('#incomemasternew_list').DataTable();
    // }, 1);
  });
}



 get tax_slabs_fromold() {
    return this.reactiveForm.get('tax_slabs_fromold')!;
  }
  get tax_slabs_fromnew() {
    return this.reactiveForm.get('tax_slabs_fromnew')!;
  }
  get tax_slabs_toold() {
    return this.reactiveForm.get('tax_slabs_toold')!;
  }
  get tax_slabs_tonew() {
    return this.reactiveForm.get('tax_slabs_tonew')!;
  }
  get individuals() {
    return this.reactiveForm.get('individuals')!;
  }
  get resident_senior_citizens() {
    return this.reactiveForm.get('resident_senior_citizens')!;
  }
  get resident_super_senior_citizens() {
    return this.reactiveForm.get('resident_super_senior_citizens')!;
  }
  get income_tax_rates() {
    return this.reactiveForm.get('income_tax_rates')!;
  }

  
 
  get tax_slabs_fromoldedit() {
    return this.reactiveFormEdit.get('tax_slabs_fromoldedit')!;
  }
  get tax_slabs_tooldedit() {
    return this.reactiveFormEdit.get('tax_slabs_tooldedit')!;
  }
  get tax_slabs_fromnewedit() {
    return this.reactiveFormEdit.get('tax_slabs_fromnewedit')!;
  }
  get tax_slabs_tonewedit() {
    return this.reactiveFormEdit.get('tax_slabs_tonewedit')!;
  }
  get individuals_edit() {
    return this.reactiveFormEdit.get('individuals_edit')!;
  }
  get resident_senior_citizensedit() {
    return this.reactiveFormEdit.get('resident_senior_citizensedit')!;
  }
  get resident_super_senior_citizensedit() {
    return this.reactiveFormEdit.get('resident_super_senior_citizensedit')!;
  }
  get income_tax_ratesedit() {
    return this.reactiveFormEdit.get('income_tax_ratesedit')!;
  }
  

  public onsubmit(): void {
    debugger;

    if (this.reactiveForm.value.tax_name != null && this.reactiveForm.value.tax_name != '')
      {

          this.reactiveForm.value;
          var url = 'PayMstIncomeTax/PostIncomeTaxRates'
         
          this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {

            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetIncomeTaxMasterSummary(); 
              this.GetIncomeTaxMasterNew(); 
            }
            else {
              this.reactiveForm.get("tax_regime_gid")?.setValue(null);
              this.reactiveForm.get("tax_name")?.setValue(null);
              // this.reactiveForm.get("tax_name_new")?.setValue(null);
              this.reactiveForm.get("tax_slabs_fromold")?.setValue(null);
              this.reactiveForm.get("tax_slabs_fromnew")?.setValue(null);
              this.reactiveForm.get("tax_slabs_toold")?.setValue(null);
              this.reactiveForm.get("tax_slabs_tonew")?.setValue(null);
              this.reactiveForm.get("individuals")?.setValue(null);
              this.reactiveForm.get("resident_senior_citizens")?.setValue(null);
              this.reactiveForm.get("resident_super_senior_citizens")?.setValue(null);
              this.reactiveForm.get("income_tax_rates")?.setValue(null);
              this.reactiveForm.get("remarks_old")?.setValue(null);
              this.reactiveForm.get("remarks_new")?.setValue(null);

              this.ToastrService.success(result.message)
              this.GetIncomeTaxMasterSummary();
              this.GetIncomeTaxMasterNew();
             

            }

          });

        }
        else {
          this.ToastrService.warning('result.message')
        }
        this.reactiveForm.reset();
  }

  onedit(parameter: string){
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("tax_nameedit")?.setValue(this.parameterValue1.tax_name);
    // this.reactiveFormEdit.get("tax_name_newedit")?.setValue(this.parameterValue1.tax_name_new);
    this.reactiveFormEdit.get("tax_slabs_fromoldedit")?.setValue(this.parameterValue1.tax_slabs_fromold);
    this.reactiveFormEdit.get("tax_slabs_tooldedit")?.setValue(this.parameterValue1.tax_slabs_toold);
    this.reactiveFormEdit.get("tax_slabs_fromnewedit")?.setValue(this.parameterValue1.tax_slabs_fromnew);
    this.reactiveFormEdit.get("tax_slabs_tonewedit")?.setValue(this.parameterValue1.tax_slabs_tonew);
    this.reactiveFormEdit.get("individuals_edit")?.setValue(this.parameterValue1.individuals);
    this.reactiveFormEdit.get("resident_senior_citizensedit")?.setValue(this.parameterValue1.resident_senior_citizens);
    this.reactiveFormEdit.get("resident_super_senior_citizensedit")?.setValue(this.parameterValue1.resident_super_senior_citizens);
    this.reactiveFormEdit.get("income_tax_ratesedit")?.setValue(this.parameterValue1.income_tax_rates);
    this.reactiveFormEdit.get("remarksold_edit")?.setValue(this.parameterValue1.remarks_old);
    this.reactiveFormEdit.get("remarksnew_edit")?.setValue(this.parameterValue1.remarks_new);
    this.reactiveFormEdit.get("tax_regime_gid")?.setValue(this.parameterValue1.tax_regime_gid);
  }

  public onupdate(): void {
    debugger
    if (this.reactiveFormEdit.value.tax_nameedit != null && this.reactiveFormEdit.value.tax_nameedit != '')

     {
          for (const control of Object.keys(this.reactiveFormEdit.controls)) {
            this.reactiveFormEdit.controls[control].markAsTouched();
          }
         
          let param = {
            tax_regime_gid: this.reactiveFormEdit.value.tax_regime_gid,
            tax_nameedit: this.reactiveFormEdit.value.tax_nameedit,
            tax_name_newedit: this.reactiveFormEdit.value.tax_name_newedit,
            tax_slabs_fromoldedit: this.reactiveFormEdit.value.tax_slabs_fromoldedit,
            tax_slabs_tooldedit: this.reactiveFormEdit.value.tax_slabs_tooldedit,
            tax_slabs_fromnewedit: this.reactiveFormEdit.value.tax_slabs_fromnewedit,
            tax_slabs_tonewedit: this.reactiveFormEdit.value.tax_slabs_tonewedit,
            individuals_edit: this.reactiveFormEdit.value.individuals_edit,
            resident_senior_citizensedit: this.reactiveFormEdit.value.resident_senior_citizensedit,
            resident_super_senior_citizensedit: this.reactiveFormEdit.value.resident_super_senior_citizensedit,
            income_tax_ratesedit: this.reactiveFormEdit.value.income_tax_ratesedit,
            remarksold_edit: this.reactiveFormEdit.value.remarksold_edit,
            remarksnew_edit: this.reactiveFormEdit.value.remarksnew_edit,
          }
          
          this.reactiveFormEdit.value;
          var url = 'PayMstIncomeTax/getUpdatedIncomeTaxRates'
         

          this.service.postparams(url, param).pipe().subscribe(result => {
            this.responsedata = result;
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetIncomeTaxMasterSummary();
              this.GetIncomeTaxMasterNew();
            }
            else {
              this.ToastrService.success(result.message)
              this.GetIncomeTaxMasterSummary();
              this.GetIncomeTaxMasterNew();
            }

          });

     }
     this.reactiveForm.reset();
  }

  onclose(){

  }

  taxmethod(){
   
  }

  taxPeriod() {
    
  }

}
