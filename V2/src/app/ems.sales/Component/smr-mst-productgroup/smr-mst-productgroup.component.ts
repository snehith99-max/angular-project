import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';




interface product
{
  productgroup_gid: string;
  productgroup_code: string;
  productgroup_name: string;
  productgroupedit_code: string;
  productgroupedit_name: string;
  tax1_gid: string;
  tax2_gid: string;
  tax3_gid: string;
  tax1_name: string;
  tax2_name: string;
  tax3_name: string;
 


}

@Component({
  selector: 'app-smr-mst-productgroup',
  templateUrl: './smr-mst-productgroup.component.html',
  styleUrls: ['./smr-mst-productgroup.component.scss'],
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
export class SmrMstProductgroupComponent {

  salesproductForm: FormGroup | any;
  salesproductFormEdit: FormGroup | any;
  salesproductFormTax: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  salesproductgroup_list: any[] = [];
  tax1_list: any []=[];
  tax2_list: any []=[];
  tax3_list: any []=[];
  tax1_name:any;
  parameterValue1: any;

  showOptionsDivId: any; 
  product!: product;

  addtax: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private router: ActivatedRoute , ) {
    this.product = {} as product;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSalesProductGroupSummary();
    this.salesproductForm = new FormGroup({



      // productgroup_code: new FormControl(this.product.productgroup_code, [

      //   Validators.required,



      // ]),

      productgroup_name: new FormControl(this.product.productgroup_name, [

        Validators.required,



      ])

    });

    this.salesproductFormEdit = new FormGroup({



      productgroupedit_code: new FormControl(this.product.productgroupedit_code, [

        Validators.required,

      ]),


      productgroupedit_name: new FormControl(this.product.productgroupedit_name, [

        Validators.required,

      ]),
      productgroup_gid: new FormControl(''),
    });

    this.salesproductFormTax = new FormGroup({

      productgroup_gid: new FormControl(''),

      productgroup_code: new FormControl(''),

      productgroup_name: new FormControl(''),

     tax1_gid: new FormControl(''),

     tax2_gid: new FormControl(''),

     tax3_gid: new FormControl(''),

     tax1_name: new FormControl('',[Validators.required]),

     tax2_name: new FormControl(''),

     tax3_name: new FormControl(''),

  });


  
   
    var url= 'SmrMstProductGroup/GetTaxDtl'

  this.service.get(url).subscribe((result: any) =>{

    this.responsedata = result;

    this.tax1_list = this.responsedata.GetTaxDtl;

  });

  var url= 'SmrMstProductGroup/GetTax2Dtl'

  this.service.get(url).subscribe((result: any) =>{

    this.responsedata = result;

    this.tax2_list = this.responsedata.GetTax2Dtl;

  });

  var url= 'SmrMstProductGroup/GetTax3Dtl'

  this.service.get(url).subscribe((result: any) =>{

    this.responsedata = result;

    this.tax3_list = this.responsedata.GetTax3Dtl;

  });
   
  }
    
  
  
   //// Summary Grid//////
   GetSalesProductGroupSummary(){
   var url = 'SmrMstProductGroup/GetSalesProductGroupSummary'
   this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
    $('#salesproductgroup_list').DataTable().destroy();
     this.responsedata = result;
     this.salesproductgroup_list = this.responsedata.salesproductgroup_list;
     setTimeout(() => {
       $('#salesproductgroup_list').DataTable()
     }, 1); 
     this.NgxSpinnerService.hide()
 
   });
 
}
 ////////////Add popup validtion////////
  // get productgroupcontrol_code() {
  //   return this.salesproductForm.get('productgroup_code')!;
  // }

get productgroupcontrol_name() {
  return this.salesproductForm.get('productgroup_name')!;
}
////////////Edit popup validtion////////
get productgroupedit_code() {
  return this.salesproductFormEdit.get('productgroupedit_code')!;
}

get productgroupedit_name() {
return this.salesproductFormEdit.get('productgroupedit_name')!;
}


////////////Add popup////////
   onsubmit() {
    if (this.salesproductForm.value.productgroup_name != '' && this.salesproductForm.value.productgroup_name !=null) {

      for (const control of Object.keys(this.salesproductForm.controls)) {
        this.salesproductForm.controls[control].markAsTouched();
      }
      this.salesproductForm.value;
      var url='SmrMstProductGroup/PostSalesProductGroup'
      this.NgxSpinnerService.show()
      this.service.post(url,this.salesproductForm.value).subscribe((result:any) => {

        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide()
          this.GetSalesProductGroupSummary();
        }
        else{
          //this.salesproductForm.get("productgroup_code")?.setValue(null);
          this.salesproductForm.get("productgroup_name")?.setValue(null);
          
          this.ToastrService.success(result.message) 
          this.salesproductForm.reset();
         
          this.GetSalesProductGroupSummary();
          this.NgxSpinnerService.hide();
          
          
         
        }
      
              
            });
            
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  //////////Edit popup////////
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.salesproductFormEdit.get("productgroupedit_code")?.setValue(this.parameterValue1.productgroup_code);
    this.salesproductFormEdit.get("productgroupedit_name")?.setValue(this.parameterValue1.productgroup_name);
    this.salesproductFormEdit.get("productgroup_gid")?.setValue(this.parameterValue1.productgroup_gid);
   
  }
  ////////////Update popup////////
   onupdate() {
    
    if (this.salesproductFormEdit.value.productgroupedit_code != null && this.salesproductFormEdit.value.productgroupedit_name != '') {
      for (const control of Object.keys(this.salesproductFormEdit.controls)) {
        this.salesproductFormEdit.controls[control].markAsTouched();
      }
      this.salesproductFormEdit.value;

      
      var url = 'SmrMstProductGroup/GetUpdatedSalesProductgroup'
      this.NgxSpinnerService.show()

      this.service.post(url,this.salesproductFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetSalesProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
        else{
          this.ToastrService.success(result.message)
          this.GetSalesProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      this.NgxSpinnerService.hide()
    }
  }
  ////////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  ondelete() {
    
    var url = 'SmrMstProductGroup/GetDeleteSalesProductSummary'
    let param = {
      productgroup_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message);
        this.GetSalesProductGroupSummary();
      }
      else{
        
        this.ToastrService.success(result.message);
        this.GetSalesProductGroupSummary();
        
      }
      
      this.GetSalesProductGroupSummary();
     
  
    });
  }
  onclose() {
    this.salesproductForm.reset();

  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }



 //////// Add Tax ///////

 openModaladd(parameter: string){

  this.parameterValue1 = parameter

  this.salesproductFormTax.get("productgroup_code")?.setValue(this.parameterValue1.productgroup_code);

  this.salesproductFormTax.get("productgroup_name")?.setValue(this.parameterValue1.productgroup_name);

  this.salesproductFormTax.get("productgroup_gid")?.setValue(this.parameterValue1.productgroup_gid);

  this.salesproductFormTax.get("tax1_name")?.setValue(this.parameterValue1.tax1_name);

  this.salesproductFormTax.get("tax2_name")?.setValue(this.parameterValue1.tax2_name);

  this.salesproductFormTax.get("tax3_name")?.setValue(this.parameterValue1.tax3_name);

  this.salesproductFormTax.get("tax1_gid")?.setValue(this.parameterValue1.tax1_gid);

  this.salesproductFormTax.get("tax2_gid")?.setValue(this.parameterValue1.tax2_gid);

  this.salesproductFormTax.get("tax3_gid")?.setValue(this.parameterValue1.tax3_gid);

}

update(){

  this.salesproductFormTax.value;  
  if( this.tax1_name.value != null){
    var url = 'SmrMstProductGroup/GetUpdatedSalesTax'
    this.service.post(url,this.salesproductFormTax.value).pipe().subscribe((result:any)=>{

      this.responsedata=result;

      if(result.status ==false){

        this.ToastrService.warning(result.message)
        this.GetSalesProductGroupSummary();

      }
      else{

        this.ToastrService.success(result.message)
        this.GetSalesProductGroupSummary();

      }
    

  });
}
else{
  this.ToastrService.warning("Kindly Fill All Mandatory Fields !")
}



}





}


  

