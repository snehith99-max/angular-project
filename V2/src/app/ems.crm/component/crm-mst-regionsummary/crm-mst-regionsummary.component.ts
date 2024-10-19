import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IRegion {
  region_name: string;
  region_code: string;
  region_gid: string;
  city_name:string;
  region_name_edit: string;
  region_code_edit: string;
  city_name_edit:string
  // city_name_edit:string;
}

@Component({
  selector: 'app-crm-mst-regionsummary',
  templateUrl: './crm-mst-regionsummary.component.html',
  styleUrls: ['./crm-mst-regionsummary.component.scss'],
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
export class CrmMstRegionsummaryComponent {
  isReadOnly = true;
  private unsubscribe: Subscription [] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  showOptionsDivId: any;
  parameterValue1: any;
  region_gid: any;
  region_list: any[] = [];
  region!: IRegion;
  status: any;
  
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
    this.region = {} as IRegion;
  }
  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    // Form values for Add popup/////
    this.GetRegionSummary();
    this.reactiveForm = new FormGroup({

      region_name: new FormControl(this.region.region_name, [
        Validators.required,
          Validators.pattern(/^(\S+\s*)*(?!\s).*$/),


       ]),

      city_name: new FormControl(this.region.city_name, [
      Validators.required,
      Validators.pattern(/^(\S+\s*)*(?!\s).*$/),


    ]),



     

    });
    this.reactiveFormEdit = new FormGroup({

      region_name_edit: new FormControl(this .region.region_name_edit, [

        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/),
        //Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces
  
      ]),
  
     region_code_edit: new FormControl(this.region.region_code_edit, [
       Validators.required,
       Validators.pattern('[A-Za-z0-9]+')

     ]),
     city_name_edit: new FormControl(this.region.city_name_edit, [

      Validators.required,
      Validators.pattern(/^(\S+\s*)*(?!\s).*$/),
      //Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces

    ]),  
     region_gid: new FormControl(''),

   });

    
  }
  GetRegionSummary(){
    this.NgxSpinnerService.show();
    var api = 'Region/GetRegionSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#region_list').DataTable().destroy();
    this.responsedata = result;
    this.region_list = this.responsedata.region_lists1;
    this.NgxSpinnerService.hide();
    
    //console.log(this.entity_list)
    setTimeout(() => {
    $('#region_list').DataTable();
      }, 1);
    });
    }


    toggleOptions(region_gid: any) {
      if (this.showOptionsDivId === region_gid) {
        this.showOptionsDivId = null;
      } else {
        this.showOptionsDivId = region_gid;
      }
    }
 
  
get region_name() {
  return this.reactiveForm.get('region_name')!;
}
get region_code() {
  return this.reactiveForm.get('region_code')!;
}
get city_name() {
  return this.reactiveForm.get('city_name')!;
}

get region_name_edit() {
  return this.reactiveFormEdit.get('region_name_edit')!;
}
get region_code_edit() {
  return this.reactiveFormEdit.get('region_code_edit')!;
}
get city_name_edit() {
  return this.reactiveFormEdit.get('city_name_edit')!;
}

public onsubmit(): void {
  if (this.reactiveForm.value.region_name != null ) {

    for (const control of Object.keys(this.reactiveForm.controls)) {
      this.reactiveForm.controls[control].markAsTouched();
    }
    this.reactiveForm.value;
    this.NgxSpinnerService.show();
    var url='Region/PostRegion'
          this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {

            if(result.status ==false){
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done
    
              });
              this.reactiveForm.get("region_name")?.setValue(null);
              this.reactiveForm.get("region_code")?.setValue(null);
              this.reactiveForm.get("city_name")?.setValue(null);
              this.ToastrService.warning(result.message)
              this.GetRegionSummary();
              this.reactiveForm.reset();

            }
            else{
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done
    
              });
              this.reactiveForm.get("region_name")?.setValue(null);
              this.reactiveForm.get("region_code")?.setValue(null);
              this.reactiveForm.get("city_name")?.setValue(null);
              this.ToastrService.success(result.message)
              this.GetRegionSummary();
              this.reactiveForm.reset();

            }
            this.NgxSpinnerService.hide();
            this.GetRegionSummary();
            this.reactiveForm.reset();

          }); 
          
  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
  
}
openModaledit(parameter: string) {
  this.parameterValue1 = parameter
  this.reactiveFormEdit.get("region_name_edit")?.setValue(this.parameterValue1.region_name);
  this.reactiveFormEdit.get("region_code_edit")?.setValue(this.parameterValue1.region_code);
  this.reactiveFormEdit.get("city_name_edit")?.setValue(this.parameterValue1.city_name);
  this.reactiveFormEdit.get("region_gid")?.setValue(this.parameterValue1.region_gid);
  
}
////////Update popup////////
public onupdate(): void {
  if (this.reactiveFormEdit.value.region_name_edit != null && this.reactiveFormEdit.value.region_code_edit != null) {
    for (const control of Object.keys(this.reactiveFormEdit.controls)) {
      this.reactiveFormEdit.controls[control].markAsTouched();
    }
    this.reactiveFormEdit.value;
    this.NgxSpinnerService.show();
    var url = 'Region/UpdateRegion'

    this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetRegionSummary();
        this.reactiveFormEdit.reset();

      }
      else{
        this.ToastrService.success(result.message)
        this.GetRegionSummary();
        this.reactiveFormEdit.reset();

      }
      this.NgxSpinnerService.show();
      this.GetRegionSummary();
      
  }); 

  }
  else {
    
    window.scrollTo({

      top: 0, // Code is used for scroll top after event done

    });
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
}
openModaldelete(parameter: string) {
  this.parameterValue = parameter

}
ondelete() {
  console.log(this.parameterValue);
  this.NgxSpinnerService.show();
  var url = 'Region/DeleteRegion'
  let param = {
    region_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if(result.status ==false){
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning(result.message)
      this.reactiveFormEdit.reset();

    }
    else{
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.success(result.message)
      this.GetRegionSummary();
      this.reactiveFormEdit.reset();

    }
    this.NgxSpinnerService.hide();
    this.GetRegionSummary();
    this.reactiveFormEdit.reset();

  });
}
statusvalue( value: any, region_gid: String){
  this.status = value
  this.region_gid = region_gid
  

}
onstatusupdate(region_gid : String , status_flag : String){
  this.NgxSpinnerService.show();
  let params = {
    region_gid: region_gid,
    status_flag: status_flag
    
   }
   var url4 = 'Region/regionstatusupdate'
   this.service.post(url4, params).subscribe((result: any) => {

      if ( result.status == false) {
       this.ToastrService.warning(result.message)
      }
      else {
       this.ToastrService.success(result.message)
      
        }
        this.NgxSpinnerService.hide();
        this.GetRegionSummary();
       
    });
}
  


onclose() {
  this.reactiveForm.reset();
  this.reactiveFormEdit.reset();

}

}