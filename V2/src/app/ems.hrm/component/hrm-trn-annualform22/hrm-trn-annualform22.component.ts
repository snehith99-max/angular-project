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
import { an } from '@fullcalendar/core/internal-common';

interface IAnnualForm {

// Company Master
  companyregistrtion_number: string;  
  company_name: string;  
  company_address: string;  
  occupier_name: string;  
  occupier_address: string;  
  company_phone: string;  
  natureof_industry: string;  

  // Product Master
    
  product_name: string;  
  capacity: string;  
  quantity: string;  
  product_value: string;  

  // Leave Wage
    
  mentotalnoofemp: string;  
  menearnedleave: string;  
  mengrantedleave: string;  
  mendischarged: string;
  mennoofempwages: string;  
  womentotalnoofemp: string;
  womenearnedleave: string; 
  womengrantedleave: string;  
  womendischarged: string;
  woemennoofempwages: string;
  adolescentstotalnoofemp: string;  
  adloscentsearnedleave: string;
  adloscentsgrantedleave: string;  
  adloescentsdischarged: string;
  adolescentsnoofempwages: string;  

  // Employeement

  menrollstartdate: string;  
  menrollenddate: string;  
  menfactoryworked: string;  
  menworkedyearnormal: string;
  menworkedyearot: string;  
  menworkedyeartotal: string;
  menworkperweek: string;  
  mentotalamount: string;
  womenrollstartdate: string;
  womenrollenddate: string;  
  womenfactoryworked: string;
  womenworkedyearnormal: string;  
  womenworkedyearot: string;
  womenworkedyeartotal: string;  
  womenworkperweek: string;  
  womentotalamount: string;  
  adloscentrollstartdate: string;  
  adloscentrollenddate: string;
  adloscentfactoryworked: string;  
  adloscentworkedyearnormal: string;
  adloscentworkedyearot: string;  
  adloscentworkedyeartotal: string;
  adloscentworkperweek: string;
  adloscenttotalamount: string;  
  adloscentmenrollstartdate: string;
  adloscentmenrollenddate: string;  
  adloscentmenfactoryworked: string;
  adloscentmenworkedyearnormal: string;  
  adloscentmenworkedyearot: string;
  adloscentmenworkedyeartotal: string;  
  adloscentmenworkperweek: string;
  adloscentwomenworkedyeartotal: string;  
  adloscentwomenrollstartdate: string;
  adloscentwomenrollenddate: string;  
  adloscentwomenfactoryworked: string;
  adloscentwomenworkedyearnormal: string;  
  adloscentwomenworkedyearot: string;
  adloscentwomenworkyeartotal: string;  
  adloscentwomenworkperweek: string;
  adloscentwomentotalamount: string;  
  bonus_amountfield: string;
  bonus_amountmen: string;  
  bonus_amountwomen: string;
  bonus_amountadomen: string; 
  bonus_amountadowomen: string; 

    
 // Concession
    
  empbonus_number: string;  
  bonus_declared: string;  
  bonus_amount: string;  
  bonus_date: string;
  exgratia_amount: string;  
  exgratia_date: string;
  incentive_amount: string;  
  incentive_date: string;
 
}

@Component({
  selector: 'app-hrm-trn-annualform22',
  templateUrl: './hrm-trn-annualform22.component.html',
  styleUrls: ['./hrm-trn-annualform22.component.scss']
})
export class HrmTrnAnnualform22Component {

  annualform!: IAnnualForm;

  reactivemasterForm!: FormGroup;
  reactiveworkForm!: FormGroup;
  reactiveleavewageForm!: FormGroup;
  reactiveemploymentForm!: FormGroup;
  reactivedeductionForm!: FormGroup;
  reactiveconcessionsForm!: FormGroup;
  addproductForm!:FormGroup;
  addcontractForm!:FormGroup;
  product_list: any[]=[];
  producttitle_list: any[]=[];
  contract_list:any[]=[];
  concessionedit_list: any;
  leavewageedit_list: any;
  employeemanagementedit_list: any;
  selectedEmployeeManagement: any;
  selectedConcession: any;
  selectedLeaveWage: any;

  responsedata: any;
  companyedit_list: any;
  selectedCompany: any;

  formnameyear: any;
  form_name:any;
  form_gid:any;
  processed_year:any;
  parameterValue: any;
  
  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    
       ) 
    {
    this.annualform = {} as IAnnualForm;
 
    this.reactivemasterForm = new FormGroup({

      companyregistrtion_number: new FormControl(this.annualform.companyregistrtion_number, []),
      company_name: new FormControl(this.annualform.company_name, []),
      company_address: new FormControl(this.annualform.company_address, []),
      occupier_name: new FormControl(this.annualform.occupier_name, []),
      occupier_address: new FormControl(this.annualform.occupier_address, []),
      company_phone: new FormControl(this.annualform.company_phone, []),
      natureof_industry: new FormControl(this.annualform.natureof_industry, []),
      form_name: new FormControl(''),

        });
 
 
  }

  ngOnInit(): void {

   
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options); 

    const formnameyear = this.router.snapshot.paramMap.get('formnameyear');
    this.formnameyear = formnameyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.formnameyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
   
    const [form_name, form_gid, processed_year] = deencryptedParam.split('+');
    this.form_name = form_name;
    this.form_gid = form_gid;
    this.processed_year = processed_year;

    
    this.reactiveworkForm = new FormGroup({

      
    });
   
    this.reactiveleavewageForm = new FormGroup({
      mentotalnoofemp: new FormControl(this.annualform.mentotalnoofemp, []),
      menearnedleave: new FormControl(this.annualform.menearnedleave, []),
      mengrantedleave: new FormControl(this.annualform.mengrantedleave, []),
      mendischarged: new FormControl(this.annualform.mendischarged, []),
      mennoofempwages: new FormControl(this.annualform.mennoofempwages, []),
      womentotalnoofemp: new FormControl(this.annualform.womentotalnoofemp, []),
      womenearnedleave: new FormControl(this.annualform.womenearnedleave, []),
      womengrantedleave: new FormControl(this.annualform.womengrantedleave, []),
      womendischarged: new FormControl(this.annualform.womendischarged, []),
      woemennoofempwages: new FormControl(this.annualform.woemennoofempwages, []),
      adolescentstotalnoofemp: new FormControl(this.annualform.adolescentstotalnoofemp, []),
      adloscentsearnedleave: new FormControl(this.annualform.adloscentsearnedleave, []),
      adloscentsgrantedleave: new FormControl(this.annualform.adloscentsgrantedleave, []),
      adloescentsdischarged: new FormControl(this.annualform.adloescentsdischarged, []),
      adolescentsnoofempwages: new FormControl(this.annualform.adolescentsnoofempwages, []),
    });
    this.reactiveemploymentForm = new FormGroup({

      menrollstartdate: new FormControl(''),
      menrollenddate: new FormControl(''),
      menfactoryworked: new FormControl(''),
      menworkedyearnormal: new FormControl(''),
      menworkedyearot: new FormControl(''),
      menworkedyeartotal: new FormControl(''),
      menworkperweek: new FormControl(''),
      mentotalamount: new FormControl(''),
      womenrollstartdate: new FormControl(''),

      womenrollenddate: new FormControl(''),
      womenfactoryworked: new FormControl(''),
      womenworkedyearnormal: new FormControl(''),
      womenworkedyearot: new FormControl(''),
      womenworkedyeartotal: new FormControl(''),

      womenworkperweek: new FormControl(''),
      womentotalamount: new FormControl(''),
      adloscentrollstartdate: new FormControl(''),
      adloscentrollenddate: new FormControl(''),
      adloscentfactoryworked: new FormControl(''),

      adloscentworkedyearnormal: new FormControl(''),
      adloscentworkedyearot: new FormControl(''),
      adloscentworkedyeartotal: new FormControl(''),
      adloscentworkperweek: new FormControl(''),
      adloscenttotalamount: new FormControl(''),

      adloscentmenrollstartdate: new FormControl(''),
      adloscentmenrollenddate: new FormControl(''),
      adloscentmenfactoryworked: new FormControl(''),
      adloscentmenworkedyearnormal: new FormControl(''),
      adloscentmenworkedyearot: new FormControl(''),

      adloscentmenworkedyeartotal: new FormControl(''),
      adloscentmenworkperweek: new FormControl(''),
      adloscentwomenworkedyeartotal: new FormControl(''),
      adloscentwomenrollstartdate: new FormControl(''),
      adloscentwomenrollenddate: new FormControl(''),

      adloscentwomenfactoryworked: new FormControl(''),
      adloscentwomenworkedyearnormal: new FormControl(''),
      adloscentwomenworkedyearot: new FormControl(''),
      adloscentwomenworkyeartotal: new FormControl(''),
      adloscentwomenworkperweek: new FormControl(''),

      adloscentwomentotalamount: new FormControl(''),

      bonus_amountfield: new FormControl(''),
      bonus_amountmen: new FormControl(''),
      bonus_amountwomen: new FormControl(''),
      bonus_amountadomen: new FormControl(''),

      bonus_amountadowomen: new FormControl(''),
      

    });
    this.reactivedeductionForm = new FormGroup({

          });
    this.reactiveconcessionsForm = new FormGroup({
      empbonus_number: new FormControl(this.annualform.empbonus_number, []),
      bonus_declared: new FormControl(this.annualform.bonus_declared, []),
      bonus_amount: new FormControl(this.annualform.bonus_amount, []),
      bonus_date: new FormControl(this.annualform.bonus_date, []),
      exgratia_amount: new FormControl(this.annualform.exgratia_amount, []),
      exgratia_date: new FormControl(this.annualform.exgratia_date, []),
      incentive_amount: new FormControl(this.annualform.incentive_amount, []),
      incentive_date: new FormControl(this.annualform.incentive_date, []),
      form_gid: new FormControl(''),
      
    });
    this.addproductForm = new FormGroup({

      product_name: new FormControl(this.annualform.product_name, [Validators.required,]),
      capacity: new FormControl(this.annualform.capacity, [Validators.required,]),
      quantity: new FormControl(this.annualform.quantity, [Validators.required,]),
      product_value: new FormControl(this.annualform.product_value, [Validators.required,]),
      form_gid: new FormControl(''),
      processed_year: new FormControl(''),
          });
    
          this.addcontractForm = new FormGroup({

      
    });
    
    this.GetEditCompany(form_name, processed_year);
    this.GetProductSummary(form_name, processed_year);
    this.GetEditConcession(form_gid, processed_year);
    this.GetEditLeaveWage(form_gid, processed_year);
    this.GetEditEmployeeManagement(form_gid, processed_year);

  }

  GetEditConcession(form_gid: any,processed_year: any) {

    var url = 'HrmForm22/GetEditConcession'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
    this.concessionedit_list = result.GetEditConcession;
      console.log(this.concessionedit_list)
      this.selectedConcession = this.concessionedit_list[0].form_gid;
      this.reactiveconcessionsForm.get("empbonus_number")?.setValue(this.concessionedit_list[0].numberofemployeeseligible_bonus);
      this.reactiveconcessionsForm.get("bonus_declared")?.setValue(this.concessionedit_list[0].bonus_percentage);
      this.reactiveconcessionsForm.get("bonus_amount")?.setValue(this.concessionedit_list[0].amountof_bonus);
      this.reactiveconcessionsForm.get("exgratia_amount")?.setValue(this.concessionedit_list[0].amountof_exgratia);
      this.reactiveconcessionsForm.get("incentive_amount")?.setValue(this.concessionedit_list[0].amountof_incentive);
      this.reactiveconcessionsForm.get("bonus_date")?.setValue(this.concessionedit_list[0].paymentof_bonus);
      this.reactiveconcessionsForm.get("exgratia_date")?.setValue(this.concessionedit_list[0].paymentof_exgratia);
      this.reactiveconcessionsForm.get("incentive_date")?.setValue(this.concessionedit_list[0].paymentof_incentive);
      this.reactiveconcessionsForm.get("form_gid")?.setValue(this.concessionedit_list[0].form_gid);
    
     
    });
  }

  GetEditLeaveWage(form_gid: any,processed_year: any) {

    var url = 'HrmForm22/GetEditLeaveWage'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
    this.leavewageedit_list = result.GetEditLeaveWage;
      console.log(this.concessionedit_list)
      this.selectedLeaveWage = this.leavewageedit_list[0].form_gid;
      this.reactiveleavewageForm.get("mentotalnoofemp")?.setValue(this.leavewageedit_list[0].total_employeemen);
      this.reactiveleavewageForm.get("menearnedleave")?.setValue(this.leavewageedit_list[0].employee_eligiblemen);
      this.reactiveleavewageForm.get("mengrantedleave")?.setValue(this.leavewageedit_list[0].no_ofemployeeavailedmen);
      this.reactiveleavewageForm.get("mendischarged")?.setValue(this.leavewageedit_list[0].no_ofemployeedischargemen);
      this.reactiveleavewageForm.get("mennoofempwages")?.setValue(this.leavewageedit_list[0].employee_lieuearnmen);
      this.reactiveleavewageForm.get("womentotalnoofemp")?.setValue(this.leavewageedit_list[0].total_employeewomen);
      this.reactiveleavewageForm.get("womenearnedleave")?.setValue(this.leavewageedit_list[0].employee_eligiblewomen);
      this.reactiveleavewageForm.get("womengrantedleave")?.setValue(this.leavewageedit_list[0].no_ofemployeeavailedwomen);
      this.reactiveleavewageForm.get("womendischarged")?.setValue(this.leavewageedit_list[0].no_employeedischargewomen);
      this.reactiveleavewageForm.get("woemennoofempwages")?.setValue(this.leavewageedit_list[0].employee_lieuearnwomen);
      this.reactiveleavewageForm.get("adolescentstotalnoofemp")?.setValue(this.leavewageedit_list[0].total_employeeado);
      this.reactiveleavewageForm.get("adloscentsearnedleave")?.setValue(this.leavewageedit_list[0].employee_eligibleado);
      this.reactiveleavewageForm.get("adloscentsgrantedleave")?.setValue(this.leavewageedit_list[0].no_ofemployeeavailedado);
      this.reactiveleavewageForm.get("adloescentsdischarged")?.setValue(this.leavewageedit_list[0].no_employeedischargeado);
      this.reactiveleavewageForm.get("adolescentsnoofempwages")?.setValue(this.leavewageedit_list[0].employee_lieuearnado);
      this.reactiveleavewageForm.get("form_gid")?.setValue(this.leavewageedit_list[0].form_gid);
    
     
    });
  }




  GetProductSummary(form_name: any,processed_year: any) {
    var url = 'HrmForm22/GetProductSummary'
    let param = {
      form_name: form_name,
      processed_year: processed_year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.producttitle_list = this.responsedata.GetProduct;
      setTimeout(() => {
        $('#product').DataTable();
      }, );


    });
  }

  openModaldelete(parameter: string){
    this.parameterValue = parameter
  }

  ondelete(){
    console.log(this.parameterValue);
    var url = 'HrmForm22/DeleteProduct'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      

    });
  }

  get product_name() {
    return this.addproductForm.get('product_name')!;
  }
  get capacity() {
    return this.addproductForm.get('capacity')!;
  }
  get quantity() {
    return this.addproductForm.get('quantity')!;
  }
  get product_value() {
    return this.addproductForm.get('product_value')!;
  }
 
  GetEditCompany(form_name: any,processed_year: any) {

    var url = 'HrmForm22/GetEditCompany'
    let param = {
      form_name: form_name,
      processed_year: processed_year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.companyedit_list = result.GetEditCompany;
      console.log(this.companyedit_list)
      debugger;
     
      this.reactivemasterForm.get("companyregistrtion_number")?.setValue(this.companyedit_list[0].companyregistrtion_number);
      this.reactivemasterForm.get("company_name")?.setValue(this.companyedit_list[0].company_name);
      this.reactivemasterForm.get("company_address")?.setValue(this.companyedit_list[0].company_address);
      this.reactivemasterForm.get("occupier_name")?.setValue(this.companyedit_list[0].occupier_name);
      this.reactivemasterForm.get("occupier_address")?.setValue(this.companyedit_list[0].occupier_address);
      this.reactivemasterForm.get("company_phone")?.setValue(this.companyedit_list[0].company_phone);
      this.reactivemasterForm.get("natureof_industry")?.setValue(this.companyedit_list[0].natureof_industry);
     
    
     
    });
  }

  onformback(){}

  ondataback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }

  onproductadd(){}
  oncontractadd(){}
  onclose(){}

  mastersubmit(): void {

    this.reactivemasterForm.get("form_name")?.setValue(this.form_name);

    debugger;
    let param= {
      companyregistrtion_number: this.reactivemasterForm.value.companyregistrtion_number,
      company_name: this.reactivemasterForm.value.company_name,
      company_address: this.reactivemasterForm.value.company_address,
      occupier_name: this.reactivemasterForm.value.occupier_name,
      occupier_address: this.reactivemasterForm.value.occupier_address,
      company_phone: this.reactivemasterForm.value.company_phone,
      natureof_industry: this.reactivemasterForm.value.natureof_industry,
     
       
       
    }

    var url = 'HrmForm22/UpdateCompanyDetails'
    this.service.postparams(url, param).pipe().subscribe((result: { status: boolean; message: string | undefined; }) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning('Error Occured in Company Details')
      }
      else {
        this.ToastrService.success('Company Details Updated Successfully')
      }
     
    });

  }
  onproductsubmit(){
    debugger;
    if (this.addproductForm.value.product_name != null && this.addproductForm.value.product_name != '')
    {

          this.addproductForm.value;
          var url = 'HrmForm22/PostProduct'
          this.service.postparams(url, this.addproductForm.value).subscribe((result: any) => {

            if (result.status == false) {
              this.ToastrService.warning(result.message)
             
            }
            else 
            {
              this.addproductForm.get("product_name")?.setValue(null);
              this.addproductForm.get("capacity")?.setValue(null);
              this.addproductForm.get("quantity")?.setValue(null);
              this.addproductForm.get("product_value")?.setValue(null);
              this.addproductForm.get("form_gid")?.setValue(null);
              this.addproductForm.get("processed_year")?.setValue(null);

              this.ToastrService.success(result.message)
              

            }

          });

        }
        this.addproductForm.reset();
  }
  leavewagesubmit(){

    this.reactiveleavewageForm.get("form_gid")?.setValue(this.form_gid);

    if (this.reactiveleavewageForm.value.mentotalnoofemp != null && this.reactiveleavewageForm.value.mentotalnoofemp != '') {
      this.reactiveleavewageForm.value;
      var url = 'HrmForm22/LeaveWagesubmit'
      this.service.post(url, this.reactiveleavewageForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }



  }

  onleavewageback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }

  Concessionssubmit(){
    this.reactiveconcessionsForm.get("form_gid")?.setValue(this.form_gid);

    if (this.reactiveconcessionsForm.value.empbonus_number != null && this.reactiveconcessionsForm.value.empbonus_number != '') {
      this.reactiveconcessionsForm.value;
      var url = 'HrmForm22/Concessionssubmit'
      this.service.post(url, this.reactiveconcessionsForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }



  }

  oncontractsubmit(){}
  deductionsubmit(){}


  GetEditEmployeeManagement(form_gid: any,processed_year: any) {

    var url = 'HrmForm22/GetEditEmployeeManagement'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
    this.employeemanagementedit_list = result.GetEditEmployeeManagement;
      console.log(this.concessionedit_list)
      this.selectedEmployeeManagement = this.employeemanagementedit_list[0].form_gid;
      this.reactiveemploymentForm.get("menrollstartdate")?.setValue(this.employeemanagementedit_list[0].total_employeemen);
      this.reactiveemploymentForm.get("menrollenddate")?.setValue(this.employeemanagementedit_list[0].employee_eligiblemen);
      this.reactiveemploymentForm.get("menfactoryworked")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedmen);
      this.reactiveemploymentForm.get("menworkedyearnormal")?.setValue(this.employeemanagementedit_list[0].no_ofemployeedischargemen);
      this.reactiveemploymentForm.get("menworkedyearot")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnmen);
      this.reactiveemploymentForm.get("menworkedyeartotal")?.setValue(this.employeemanagementedit_list[0].total_employeewomen);
      this.reactiveemploymentForm.get("menworkperweek")?.setValue(this.employeemanagementedit_list[0].employee_eligiblewomen);
      this.reactiveemploymentForm.get("mentotalamount")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedwomen);
      this.reactiveemploymentForm.get("womenrollstartdate")?.setValue(this.employeemanagementedit_list[0].no_employeedischargewomen);
      this.reactiveemploymentForm.get("womenrollenddate")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnwomen);
      this.reactiveemploymentForm.get("womenfactoryworked")?.setValue(this.employeemanagementedit_list[0].total_employeeado);
      this.reactiveemploymentForm.get("womenworkedyearnormal")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("womenworkedyearot")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("womenworkedyeartotal")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("womenworkperweek")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("womentotalamount")?.setValue(this.employeemanagementedit_list[0].form_gid);

      this.reactiveemploymentForm.get("adloscentrollstartdate")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("adloscentrollenddate")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("adloscentfactoryworked")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("adloscentworkedyearnormal")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("adloscentworkedyearot")?.setValue(this.employeemanagementedit_list[0].form_gid);
    
      this.reactiveemploymentForm.get("adloscentworkedyeartotal")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("adloscentworkperweek")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("adloscenttotalamount")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("adloscentmenrollstartdate")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("adloscentmenrollenddate")?.setValue(this.employeemanagementedit_list[0].form_gid);

      this.reactiveemploymentForm.get("adloscentmenfactoryworked")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("adloscentmenworkedyearnormal")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("adloscentmenworkedyearot")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("adloscentmenworkedyeartotal")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("adloscentmenworkperweek")?.setValue(this.employeemanagementedit_list[0].form_gid);

      this.reactiveemploymentForm.get("adloscentwomenworkedyeartotal")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("adloscentwomenrollstartdate")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("adloscentwomenrollenddate")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("adloscentwomenfactoryworked")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("adloscentwomenworkedyearnormal")?.setValue(this.employeemanagementedit_list[0].form_gid);

      this.reactiveemploymentForm.get("adloscentwomenworkedyearot")?.setValue(this.employeemanagementedit_list[0].employee_eligibleado);
      this.reactiveemploymentForm.get("adloscentwomenworkyeartotal")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("adloscentwomenworkperweek")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("adloscentwomentotalamount")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("bonus_amountfield")?.setValue(this.employeemanagementedit_list[0].form_gid);

      this.reactiveemploymentForm.get("bonus_amountmen")?.setValue(this.employeemanagementedit_list[0].no_ofemployeeavailedado);
      this.reactiveemploymentForm.get("bonus_amountwomen")?.setValue(this.employeemanagementedit_list[0].no_employeedischargeado);
      this.reactiveemploymentForm.get("bonus_amountadomen")?.setValue(this.employeemanagementedit_list[0].employee_lieuearnado);
      this.reactiveemploymentForm.get("bonus_amountadowomen")?.setValue(this.employeemanagementedit_list[0].form_gid);
    
     
    });
  }




  onemployeesubmit(){
    this.reactiveemploymentForm.get("form_gid")?.setValue(this.form_gid);

    if (this.reactiveemploymentForm.value.menrollstartdate != null && this.reactiveemploymentForm.value.menrollstartdate != '') {
      this.reactiveemploymentForm.value;
      var url = 'HrmForm22/EmployeeManagementsubmit'
      this.service.post(url, this.reactiveemploymentForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }


  }


  onback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }
  workerssubmit(){}



}
