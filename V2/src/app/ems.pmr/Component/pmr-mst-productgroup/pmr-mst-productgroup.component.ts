import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-pmr-mst-productgroup',
  templateUrl: './pmr-mst-productgroup.component.html',
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
export class PmrMstProductgroupComponent {
  productForm!: FormGroup;
  productFormEdit: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  productgroup_list: any[] = [];
  parameterValue1: any;
  showOptionsDivId: any; 
 

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
    this.product();
  }
  product()
  {
    this.productForm = new FormGroup({



      productgroup_code: new FormControl(''),

      productgroup_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),

    });
    this.productFormEdit = new FormGroup({



      productgroupedit_code: new FormControl([

        Validators.required,

      ]),


      productgroupedit_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      productgroup_gid: new FormControl(''),
    });
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
  ngOnInit(): void {
    this.GetProductGroupSummary();
    
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
   
  }
    
  
  
   //// Summary Grid//////
   GetProductGroupSummary(){
   var url = 'PmrMstProductGroup/GetProductGroupSummary'
   this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
    $('#productgroup_list').DataTable().destroy();
     this.responsedata = result;
     this.productgroup_list = this.responsedata.productgroup_list;
     setTimeout(() => {
       $('#productgroup_list').DataTable()
     }, 1);
     this.NgxSpinnerService.hide()
 
   });
 
}
 ////////////Add popup validtion////////
  get productgroupcontrol_code() {
    return this.productForm.get('productgroup_code')!;
  }

get productgroupcontrol_name() {
  return this.productForm.get('productgroup_name')!;
}
////////////Edit popup validtion////////
get productgroupedit_code() {
  return this.productFormEdit.get('productgroupedit_code')!;
}

get productgroupedit_name() {
return this.productFormEdit.get('productgroupedit_name')!;
}


////////////Add popup////////
  //  onsubmit() {
    

  //   debugger
  //   if ( this.productForm.value.productgroup_name != ' ') {

  //     for (const control of Object.keys(this.productForm.controls)) {
  //       this.productForm.controls[control].markAsTouched();
  //     }
  //     this.productForm.value;
  //     var url='PmrMstProductGroup/PostProductGroup'
   
  //     this.NgxSpinnerService.show()
  //     this.service.post(url,this.productForm.value).subscribe((result:any) => {

  //       if(result.status == false){
  //         this.ToastrService.warning(result.message)
  //         this.GetProductGroupSummary();
  //       }
  //       else{
  //         this.productForm.get("productgroup_code")?.setValue(null);
  //         this.productForm.get("productgroup_name")?.setValue(null);
          
  //         this.ToastrService.success(result.message) 
  //         this.productForm.reset();
         
  //         this.GetProductGroupSummary();
  //         this.NgxSpinnerService.hide();
          
  //         this.productForm.reset();
         
  //       }
      
              
  //           });
            
  //   }
  //   else {
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //     this.productForm.reset();
  //   }
    
  // }
  onsubmit() {
    debugger
    const productGroupName = this.productForm.value.productgroup_name?.trim();
  
    if (productGroupName !== '') {
      for (const control of Object.keys(this.productForm.controls)) {
        this.productForm.controls[control].markAsTouched();
      }
  
      var url = 'PmrMstProductGroup/PostProductGroup';
      this.NgxSpinnerService.show();
  
      this.service.post(url, this.productForm.value).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message);
          this.GetProductGroupSummary();
        } else {
          this.productForm.get("productgroup_code")?.setValue(null);
          this.productForm.get("productgroup_name")?.setValue(null);
          this.ToastrService.success(result.message);
          this.productForm.reset();
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide();
        }
      });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!');
      this.productForm.reset();
    }
  }
  
  //////////Edit popup////////
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.productFormEdit.get("productgroupedit_code")?.setValue(this.parameterValue1.productgroup_code);
    this.productFormEdit.get("productgroupedit_name")?.setValue(this.parameterValue1.productgroup_name);
    this.productFormEdit.get("productgroup_gid")?.setValue(this.parameterValue1.productgroup_gid);
   
  }
  ////////////Update popup////////

   onupdate() {

    debugger
    const productGroupName = this.productFormEdit.value.productgroupedit_name?.trim();
  
    if (productGroupName !== '') {
      for (const control of Object.keys(this.productFormEdit.controls)) {
        this.productFormEdit.controls[control].markAsTouched();
      }
    

    // if (this.productFormEdit.value.productgroupedit_code != null && this.productFormEdit.value.productGroupName != '') {
    //   for (const control of Object.keys(this.productFormEdit.controls)) {
    //     this.productFormEdit.controls[control].markAsTouched();
    //   }
    //   this.productFormEdit.value;
    

      
      var url = 'PmrMstProductGroup/GetUpdatedProductgroup'
      this.NgxSpinnerService.show()

      this.service.post(url,this.productFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
        else{
          this.ToastrService.success(result.message)
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
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
    
    var url = 'PmrMstProductGroup/GetDeleteProductSummary'
    let param = {
      productgroup_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        
        this.ToastrService.success(result.message)
        
      }
      
      this.GetProductGroupSummary();
     
      this.NgxSpinnerService.show();
  
    });
  }
  onclose() {
    this.productForm.reset();

  }
  
}

  
