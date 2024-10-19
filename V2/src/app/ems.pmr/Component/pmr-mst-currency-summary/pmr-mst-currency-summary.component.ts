import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface ICurrency {
  country_name: string;
  currencyexchange_gid: string;
  exchange_rate:string;
  currency_code:string;
  currency_codeedit:string;
  exchange_rateedit:string;
  country_nameedit:string;
  selectedEntity:any;

}
@Component({
  selector: 'app-pmr-mst-currency-summary',
  templateUrl: './pmr-mst-currency-summary.component.html',
  styleUrls: ['./pmr-mst-currency-summary.component.scss'],
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
export class PmrMstCurrencySummaryComponent {
  responsedata: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  parameterValue1: any;
  parameterValue: any;
  currency_list: any[] = [];
  country_list: any[] = [];
  previous_list: any[] = [];
  selectedCountry2: any;

  showOptionsDivId: any; 
  currency!: ICurrency;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
    this.currency = {} as ICurrency;

  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
   this.GetCurrencySummary();
   this.reactiveForm = new FormGroup({

     country_name: new FormControl(this.currency.country_name, [
       Validators.required,

     ]),
    currency_code: new FormControl(this.currency.currency_code, [
      Validators.required,

    ]),
    exchange_rate: new FormControl(this.currency.exchange_rate, [
      Validators.required,

    ]),
   
    
    currencyexchange_gid: new FormControl(''),
    //currency_code: new FormControl(''),



  });
  this.reactiveFormEdit = new FormGroup({

    currency_codeedit: new FormControl(this.currency.currency_codeedit, [
      Validators.required,
    ]),
    exchange_rateedit: new FormControl(this.currency.exchange_rateedit, [
      Validators.required,
    ]),
    country_nameedit: new FormControl(this.currency.country_nameedit, [
      Validators.required,
    ]),

    currencyexchange_gid : new FormControl(''),


  });
  
 
  var api1='PmrMstCurrency/GetPmrCountryDtl'
  this.service.get(api1).subscribe((result:any)=>{
    this.country_list = result.GetPmrCountryDtl;
  
  });
  document.addEventListener('click', (event: MouseEvent) => {
    if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
      this.showOptionsDivId = null;
    }
  });

  }
  GetCurrencySummary(){
    var url = 'PmrMstCurrency/GetPmrCurrencySummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#currency_list').DataTable().destroy();
      this.responsedata = result;
      this.currency_list = this.responsedata.currency_list;
      setTimeout(() => {
        $('#currency_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()
  
  
    });
  }
  get currency_code(){
    return this.reactiveForm.get('currency_code')!;
  }
  get country_name(){
    return this.reactiveForm.get('country_name')!;
  }
  get exchange_rate() {
    return this.reactiveForm.get('exchange_rate')!;
  }
  get currency_codeedit() {
    return this.reactiveFormEdit.get('currency_codeedit')!;
  }
  get exchange_rateedit() {
    return this.reactiveFormEdit.get('exchange_rateedit')!;
  }

  public onsubmit(): void {
    debugger
    if (this.reactiveForm.value.country_name != null && this.reactiveForm.value.exchange_rate != '') {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url='PmrMstCurrency/PostPmrCurrency'
    
            this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {

              if(result.status ==false){
                this.ToastrService.warning(result.message)
                this.reactiveForm.reset();
                
              }
              else{
                

                this.ToastrService.success(result.message)
                
                
                this.reactiveForm.reset();
                this.GetCurrencySummary();
               
              }
              
            });
            
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  myModalhistory(currencyexchange_gid: any)
   {
    var url='SmrMstCurrency/GetPreviousRate'
    let params={
      currencyexchange_gid: currencyexchange_gid
    }
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
       this.previous_list = this.responsedata.salescurrency_list;
    });
  

   }

  openModaledit(parameter: string) {
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("country_nameedit")?.setValue(this.parameterValue1.country_name);
    this.reactiveFormEdit.get("currencyexchange_gid")?.setValue(this.parameterValue1.currencyexchange_gid);
    this.reactiveFormEdit.get("currency_codeedit")?.setValue(this.parameterValue1.currency_code);
    this.reactiveFormEdit.get("exchange_rateedit")?.setValue(this.parameterValue1.exchange_rate);


   
  }
    public onupdate(): void {
    if (this.reactiveFormEdit.value.country_nameedit != null && this.reactiveFormEdit.value.currency_codeedit != null && this.reactiveFormEdit.value.exchange_rateedit != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      //console.log(this.reactiveFormEdit.value)
      var url1 = 'PmrMstCurrency/PmrCurrencyUpdate'
      this.NgxSpinnerService.show()

      this.service.post(url1,this.reactiveFormEdit.value).pipe().subscribe((result:any) =>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide()
        
        }
        else{
          this.ToastrService.success(result.message)
          this.GetCurrencySummary();
          this.NgxSpinnerService.hide()
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
    console.log(this.parameterValue);
    var url = 'PmrMstCurrency/PmrCurrencySummaryDelete'
    this.NgxSpinnerService.show()
    let param = {
      currencyexchange_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
      this.GetCurrencySummary();
      this.NgxSpinnerService.hide()
    
  
  
    });
  }
  onclose(){
    this.reactiveForm.reset();
  }
  
 
}
