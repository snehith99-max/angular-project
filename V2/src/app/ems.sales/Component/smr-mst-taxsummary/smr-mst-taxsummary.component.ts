import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
interface ITax {
  tax_gid: string;
  tax_code: string;
  tax_segment: string;
  percentage: string;
  parameter: string;
  taxsegmentedit: string;
  tax_prefix: string;
  editpercentage: string;
  edittax_prefix:string;
  
}
@Component({
  selector: 'app-smr-mst-taxsummary',
  templateUrl: './smr-mst-taxsummary.component.html',
  styleUrls: ['./smr-mst-taxsummary.component.scss']
})
export class SmrMstTaxsummaryComponent {
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameter: any;
  parameterValue: any;
  parameterValue1: any;
  showOptionsDivId: any; 
  smrtax_list: any[] = [];
  taxsegment_list:any[]=[];
  tax!: ITax;
  getData: any;
  GetAssignedProduct: any;
  Unassigned_products : any;
  Assigned_products : any
  total_products : any;
  product_counts : any[]=[];
  
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router,public service: SocketService ,public NgxSpinnerService:NgxSpinnerService,) {
    this.tax = {} as ITax;
    
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetTaxSummary();
    this.gettaxsegmentdropdown();

    debugger
    var url = 'SmrMstTaxSummary/GetProductCounts';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.product_counts = result.assignedProductCount_list;

      // if (result && result.assignedProductCount_list && result.assignedProductCount_list.length > 0) {
      //   this.product_counts = result.assignedProductCount_list;
      //   const counts = this.product_counts[0];
      //   this.Assigned_products = counts?.countproduct_assigned ?? 0;
      //   this.Unassigned_products = counts?.countproduct_unassigned ?? 0;
      //   this.total_products = counts?.countproduct ?? 0;
      // } else {
       
      //   this.Assigned_products = 0;
      //   this.Unassigned_products = 0;
      //   this.total_products = 0;
      // }
       
    });
    
    // Form values for Add popup/////
    this.reactiveForm = new FormGroup({

      tax_segment: new FormControl(this.tax.tax_segment, [
        Validators.required,

      ]),
      tax_prefix: new FormControl(this.tax.tax_prefix, [
        Validators.required,

      ]),
      percentage: new FormControl(this.tax.percentage, [
        Validators.required
      ]),
    //  split_flag: new FormControl('Y'),
      active_flag: new FormControl('Y',[Validators.required])
     

    });
    // Form values for Edit popup/////
    this.reactiveFormEdit = new FormGroup({

      taxsegmentedit: new FormControl(this.tax.taxsegmentedit, [
        Validators.required ,
        
      ]),

      editpercentage: new FormControl(this.tax.editpercentage, [
        Validators.required,
       
      ]),
      edittax_prefix: new FormControl(this.tax.edittax_prefix, [
        Validators.required,
       
      ]),

      tax_gid: new FormControl(''),
     // splitedit_flag: new FormControl(''),
      activeedit_flag: new FormControl('')

    


    });

  }

  GetTilesDetails() {

  
  }
  gettaxsegmentdropdown(){
    var url = "SmrMstTaxSummary/GetTaxSegmentdropdown"
    this.service.get(url).subscribe((result:any)=>{
      this.taxsegment_list = result.TaxSegmentdtl_list;
    });
  }
  containsNonAlphabeticCharacters(value: string): boolean {
    debugger
    return /[A-Za-z]/.test(value);
  }
  containsSpaces(value: string): boolean {
    debugger
    return /\s/.test(value);
  }
  containsOnlyNumbers(value: string): boolean {
    debugger
    return /[0-9.]+/.test(value);
  }

  onMapProd(tax_gid: any,taxsegment_gid:any)
  {
  
  debugger
    const secretKey = 'storyboarderp';
    const param = (tax_gid);
    const param1 = (taxsegment_gid);
   
    const tax_gid1 = AES.encrypt(param,secretKey).toString();
    const taxsegment_gid2 = AES.encrypt(param1,secretKey).toString();
    this.route.navigate(['/smr/SmrMstMapProduct2Tax', tax_gid1,taxsegment_gid2]) 
    
  } 

 



  getAssignedProduct(tax_gid: any) {
    debugger
    var param = {
      tax_gid: tax_gid,
    }
    var url = 'SmrMstTaxSummary/GetAssignedProduct';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetAssignedProduct = result.assignedProductCount[0].count;
    });
  }
  // openunassignproduct(tax_gid1:string){
  //   debugger
  //   const key = 'storyboard';
  //   const param = tax_gid1;
  //   const tax_gid = AES.encrypt(param,key).toString();
  //   this.route.navigate(['/smr/SmrMstAssigntax2product', tax_gid]);
  // }
  
  openunassignproduct(tax_gid: any,taxsegment_gid:any)
  {
  
  debugger
    const secretKey = 'storyboarderp';
    const param = (tax_gid);
    const param1 = (taxsegment_gid);
   
    const tax_gid1 = AES.encrypt(param,secretKey).toString();
    const taxsegment_gid2 = AES.encrypt(param1,secretKey).toString();
    this.route.navigate(['/smr/SmrMstUnassigntax2product', tax_gid1,taxsegment_gid2]) 
    
  } 


  //// Summary Grid//////
  GetTaxSummary() {
    var url = 'SmrMstTaxSummary/GetTaxSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#smrtax_list').DataTable().destroy();
      this.responsedata = result;
      this.smrtax_list = this.responsedata.smrtax_list;
      
      setTimeout(() => {
        $('#smrtax_list').DataTable();
      }, 1);


    })
  }
  ////////////Add popup validtion////////
  get tax_segment() {
    return this.reactiveForm.get('tax_segment')!;
    
  }
  get percentage() {
    return this.reactiveForm.get('percentage')!;
  }
  get tax_prefix() {
    return this.reactiveForm.get('tax_prefix')!;
  }
  


  ////////////Add popup////////

  onsubmit() {
   
    if (this.reactiveForm.value.tax_segment != null && this.reactiveForm.value.tax_prefix != null && this.reactiveForm.value.percentage != '') {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'SmrMstTaxSummary/PostTax'
      this.NgxSpinnerService.show();
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          this.GetTaxSummary();
          this.reactiveForm.reset();
        }
        else {
        
          this.ToastrService.success(result.message)

          
          this.NgxSpinnerService.hide();
          this.reactiveForm.reset();
          this.GetTaxSummary();

        }
        

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }
  
  ////////////Edit popup////////
  openModaledit(parameter: string) {
    debugger
    this.parameterValue1 = parameter

    this.reactiveFormEdit.get("taxsegmentedit")?.setValue(this.parameterValue1.tax_name);
    this.reactiveFormEdit.get("edittax_prefix")?.setValue(this.parameterValue1.tax_prefix);
    this.reactiveFormEdit.get("editpercentage")?.setValue(this.parameterValue1.percentage);
    this.reactiveFormEdit.get("activeedit_flag")?.setValue(this.parameterValue1.active_flag);
    this.reactiveFormEdit.get("tax_gid")?.setValue(this.parameterValue1.tax_gid);
  }
  handleRadioButtonChange(value: string) {
    console.log('Selected value:', value);
    if (value === 'Y') {
      this.reactiveFormEdit.get('activeedit_flag')?.setValue('Y');
    } else if (value === 'N') {
      this.reactiveFormEdit.get('activeedit_flag')?.setValue('N');
    }
  }

////////////Edit popup validation////////

  get taxsegmentedit() {
    return this.reactiveFormEdit.get('taxsegmentedit')!
  }
  get editpercentage() {
    return this.reactiveFormEdit.get('editpercentage')!;
  }
  get edittax_prefix() {
    return this.reactiveFormEdit.get('edittax_prefix')!;
  }
  
  get  tax_gid() {
    return this.reactiveFormEdit.get('tax_gid')!;

    
  }
  
  ////////////Update popup////////
  public onupdate(): void {
    if (this.reactiveFormEdit.value.taxsegmentedit != null && this.reactiveFormEdit.value.edittax_prefix != null && this.reactiveFormEdit.value.editpercentage != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      var url = 'SmrMstTaxSummary/UpdatedTaxSummary'
      this.NgxSpinnerService.show();

      this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          this.GetTaxSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.reactiveForm.reset();
          
        }
        this.GetTaxSummary();
       
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  //// SPLIT TAX ////

  opensplit(parameter: String)
  {

  }

  ////////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter

}

  ondelete() {
    var url = 'SmrMstTaxSummary/deleteTaxSummary'
    let param = {
      tax_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
      this.GetTaxSummary();
    
  
  
    });
  }
  onclose() {
    this.reactiveForm.reset();

  }
  
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}
