import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface IProductgroup {
  productgroup_gid: string;
  productgroup_code: string;
  productgroup_name: string;
  productgroup_codeedit: string;
  productgroup_nameedit: string;
}
@Component({
  selector: 'app-crm-mst-productgroupsummary',
  templateUrl: './crm-mst-productgroupsummary.component.html',
  styleUrls: ['./crm-mst-productgroupsummary.component.scss'],
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
export class CrmMstProductgroupsummaryComponent {
  isReadOnly = true;

  responsedata: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  parameterValue1: any;
  parameterValue: any;
  productgroup_list: any[] = [];
  productgroup!: IProductgroup;

  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.productgroup = {} as IProductgroup;

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetProductgroupSummary();
    this.reactiveForm = new FormGroup({

      // productgroup_code: new FormControl(this.productgroup.productgroup_code, [
      //   Validators.required,
      //   Validators.pattern('[A-Za-z0-9]+')


      // ]),
      productgroup_name: new FormControl(this .productgroup.productgroup_name, [

        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/),
  
        // Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces
  
      ]),


    });
    this.reactiveFormEdit = new FormGroup({

      productgroup_codeedit: new FormControl(this.productgroup.productgroup_codeedit, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      productgroup_nameedit: new FormControl(this.productgroup.productgroup_nameedit, [

        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/),
        // Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces
  
      ]),
      productgroup_gid: new FormControl(''),



    });

  }
  toggleOptions(productgroup_gid: any) {
    if (this.showOptionsDivId === productgroup_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = productgroup_gid;
    }
  }
  GetProductgroupSummary() {
    var url = 'ProductGroup/GetProductgroupSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.responsedata = result;
      this.productgroup_list = this.responsedata.productgroup_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#productgroup_list').DataTable();
      }, 1);


    });


  }
  get productgroup_code() {
    return this.reactiveForm.get('productgroup_code')!;
  }
  get productgroup_name() {
    return this.reactiveForm.get('productgroup_name')!;
  }
  get productgroup_codeedit() {
    return this.reactiveFormEdit.get('productgroup_codeedit')!
    ;
  }
  get productgroup_nameedit() {
    return this.reactiveFormEdit.get('productgroup_nameedit')!;
  
  }
  public onsubmit(): void {
    if (this.reactiveForm.value.productgroup_name != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'ProductGroup/PostProductgroup'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          // this.reactiveForm.get("productgroup_code")?.setValue(null);
          this.reactiveForm.get("productgroup_name")?.setValue(null);
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetProductgroupSummary();
          this.reactiveForm.reset();

        }
        else {

          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();



          this.GetProductgroupSummary();
          this.reactiveForm.reset();
 
           


        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("productgroup_codeedit")?.setValue(this.parameterValue1.productgroup_code);
    this.reactiveFormEdit.get("productgroup_nameedit")?.setValue(this.parameterValue1.productgroup_name);
    this.reactiveFormEdit.get("productgroup_gid")?.setValue(this.parameterValue1.productgroup_gid);

  }
  public onupdate(): void {
    if (this.reactiveFormEdit.value.productgroup_codeedit != null && this.reactiveFormEdit.value.productgroup_nameedit != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      //console.log(this.reactiveFormEdit.value)
      var url = 'ProductGroup/UpdatedProductgroup'

      this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetProductgroupSummary();
          this.reactiveFormEdit.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          

          
          this.GetProductgroupSummary();
          this.reactiveFormEdit.reset();

          
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
    var url = 'ProductGroup/deleteProductgroupSummary'
    let param = {
      productgroup_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        this.reactiveFormEdit.reset();

      }
      this.GetProductgroupSummary();



    });
  }
  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormEdit.reset();

    

  }

}
