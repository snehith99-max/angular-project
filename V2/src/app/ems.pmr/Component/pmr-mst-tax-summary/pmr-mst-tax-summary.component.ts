import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
interface ITax {
  taxedit_prefix: string;
  tax_gid: string;
  tax_code: string;
  tax_name: string;
  tax_prefix: string;
  percentage: string;
  parameter: string;
  taxedit_name: string;
  editpercentage: string;
  tax_segment: string;
  taxsegmentedit: string;
}
@Component({
  selector: 'app-pmr-mst-tax-summary',
  templateUrl: './pmr-mst-tax-summary.component.html',
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
export class PmrMstTaxSummaryComponent {
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameter: any;
  parameterValue: any;
  taxsegment_list: any;
  parameterValue1: any;
  pmrtax_list: any[] = [];
  showOptionsDivId: any; 

  tax!: ITax;
  getData: any;
  GetAssignedProduct: any;
  constructor(private formBuilder: FormBuilder,private route: Router,public NgxSpinnerService:NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.tax = {} as ITax;
  }
  
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
 
  ngOnInit(): void {
    this.GetTaxSummary();
    this.gettaxsegmentdropdown();
    // Form values for Add popup/////
    this.reactiveForm = new FormGroup({
      tax_prefix: new FormControl(this.tax.tax_prefix, [
        Validators.required,

      ]),
      tax_segment: new FormControl(this.tax.tax_segment, [
        Validators.required,

      ]),
      // tax_name: new FormControl(this.tax.tax_name, [
      //   Validators.required,

      // ]),
      percentage: new FormControl(this.tax.percentage, [
        Validators.required
      ])

     

    });
    // Form values for Edit popup/////
    this.reactiveFormEdit = new FormGroup({
      taxedit_prefix: new FormControl(this.tax.taxedit_prefix, [
        Validators.required ,
        
      ]),
      // taxedit_name: new FormControl(this.tax.taxedit_name, [
      //   Validators.required ,
        
      // ]),

      editpercentage: new FormControl(this.tax.editpercentage, [
        Validators.required,
       
      ]),
      taxsegmentedit: new FormControl(this.tax.taxsegmentedit, [
        Validators.required,
       
      ]),

      tax_gid: new FormControl(''),

    


    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  // onMapProd(params: any,tax_name:string)
  // {
  
  // debugger
  //   const secretKey = 'storyboarderp';
  //   const param = (params);
   
  //   const encryptedParam = AES.encrypt(param,secretKey).toString();
  //   this.route.navigate(['/pmr/PmrMstMapProduct2Tax',encryptedParam]) 
  // } 
  gettaxsegmentdropdown(){
    var url = "PmrMstTax/GetTaxSegmentdropdown"
    this.service.get(url).subscribe((result:any)=>{
      this.taxsegment_list = result.TaxSegmentdtl_list;
    });
  }
  
  onMapProd(tax_gid: any,taxsegment_gid:any)
  {
  
  debugger
    const secretKey = 'storyboarderp';
    const param = (tax_gid);
    const param1 = (taxsegment_gid);   
    const tax_gid1 = AES.encrypt(param,secretKey).toString();
    const taxsegment_gid2 = AES.encrypt(param1,secretKey).toString();
    this.route.navigate(['/pmr/PmrMstMapProduct2Tax', tax_gid1,taxsegment_gid2]) 
    
  } 
  openunassignproduct(tax_gid: any,taxsegment_gid:any)
  {
  
  debugger
    const secretKey = 'storyboarderp';
    const param = (tax_gid);
    const param1 = (taxsegment_gid);   
    const tax_gid1 = AES.encrypt(param,secretKey).toString();
    const taxsegment_gid2 = AES.encrypt(param1,secretKey).toString();
    this.route.navigate(['/pmr/PmrMstTaxUnMap2Product',tax_gid1,taxsegment_gid2]) 
    
  } 
  getAssignedProduct(tax_gid: any) {
    debugger
    var param = {
      tax_gid: tax_gid,
    }
    var url = 'PmrMstTax/GetAssignedProduct';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetAssignedProduct = result.assignedProductCount[0].count;
    });
  }
  //// Summary Grid//////
  GetTaxSummary() {
    var url = 'PmrMstTax/GetTaxSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#pmrtax_list').DataTable().destroy();
      this.responsedata = result;
      this.pmrtax_list = this.responsedata.pmrtax_list;
      setTimeout(() => {
        $('#pmrtax_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()


    })
  }

  ////////////Add popup validtion////////
  get tax_prefix() {
    return this.reactiveForm.get('tax_prefix')!;
    
  }
  // get tax_name() {
  //   return this.reactiveForm.get('tax_name')!;
    
  // }
  get percentage() {
    return this.reactiveForm.get('percentage')!;
  }
  get tax_segment() {
    return this.reactiveForm.get('tax_segment')!;
  }
  


  ////////////Add popup////////

  onsubmit() {
    debugger
    console.log(this.reactiveForm)
    if (this.reactiveForm.value.tax_prefix != null  && this.reactiveForm.value.percentage != '' && this.reactiveForm.value.tax_segment != '' ) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'PmrMstTax/PostTax'
      this.NgxSpinnerService.show()
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()
        }
        else {
          this.reactiveForm.get("tax_name")?.setValue(null);
          this.reactiveForm.get("tax_prefix")?.setValue(null);
          this.reactiveForm.get("percentage")?.setValue(null);
          this.ToastrService.success(result.message)

          

          this.reactiveForm.reset();
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()

        }
        

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }
  ////////////Edit popup////////
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter

    this.reactiveFormEdit.get("taxedit_name")?.setValue(this.parameterValue1.tax_name);
    this.reactiveFormEdit.get("taxedit_prefix")?.setValue(this.parameterValue1.tax_prefix);
    this.reactiveFormEdit.get("editpercentage")?.setValue(this.parameterValue1.percentage);
    this.reactiveFormEdit.get("taxsegmentedit")?.setValue(this.parameterValue1.taxsegment_name);
    this.reactiveFormEdit.get("tax_gid")?.setValue(this.parameterValue1.tax_gid);
  }
////////////Edit popup validation////////
get taxedit_prefix() {
  return this.reactiveFormEdit.get('taxedit_prefix')!
}
  get taxedit_name() {
    return this.reactiveFormEdit.get('taxedit_name')!
  }
  get editpercentage() {
    return this.reactiveFormEdit.get('editpercentage')!;
  }
  
  get  tax_gid() {
    return this.reactiveFormEdit.get('tax_gid')!;

    
  }
  get  taxsegmentedit() {
    return this.reactiveFormEdit.get('taxsegmentedit')!;

    
  }
  
  ////////////Update popup////////
  public onupdate(): void {
    debugger
    if (this.reactiveFormEdit.value.taxedit_prefix != null  && this.reactiveFormEdit.value.editpercentage != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      //console.log(this.reactiveFormEdit.value)
      var url = 'PmrMstTax/UpdatedTaxSummary'
      this.NgxSpinnerService.show()
      this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetTaxSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()
        }
        this.reactiveForm.reset();
       
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  ////////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter

}

  ondelete() {
    debugger;
    console.log(this.parameterValue);
    var url = 'PmrMstTax/deleteTaxSummary'
    this.NgxSpinnerService.show()
    let param = {
      tax_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
    //  window.location.reload();
      }
      
      this.GetTaxSummary();
      // window.location.reload();
    
  
  
    });
  }
  onclose() {
    this.reactiveForm.reset();

  }
  
 
}
