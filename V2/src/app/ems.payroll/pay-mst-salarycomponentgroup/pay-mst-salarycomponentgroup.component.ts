import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface IComponenetGroup {
  componentgroup_gid: string;
  componentgroup_code: string;
  componentgroup_name: string;
  componentgroup_code_manual: string;
  componentgroup_code_auto:string;
  display_name: string;
  statutory: string;
  group_belongsto:string;
  


}
@Component({
  selector: 'app-pay-mst-salarycomponentgroup',
  templateUrl: './pay-mst-salarycomponentgroup.component.html',
  styleUrls: ['./pay-mst-salarycomponentgroup.component.scss'],
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
export class PayMstSalarycomponentgroupComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormadd!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  Componentgroup_list: any[] = []
  componentgroupmaster!: IComponenetGroup;
  Code_Generation: any;
  showInputField: boolean | undefined;
  


  

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService :NgxSpinnerService) {
    this.componentgroupmaster = {} as IComponenetGroup;
  }

  ngOnInit(): void {
    this.ComponentgroupSummary();
    this.reactiveFormadd = new FormGroup({
      componentgroup_gid: new FormControl(''),
      componentgroup_code_auto: new FormControl(this.componentgroupmaster.componentgroup_code_auto),
      componentgroup_code_manual: new FormControl(this.componentgroupmaster.componentgroup_code_manual,[Validators.pattern(/^\S.*$/)]),
      componentgroup_name: new FormControl(this.componentgroupmaster.componentgroup_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      statutory: new FormControl('Y'),
      group_belongsto: new FormControl('Basic'),
      Code_Generation: new FormControl('Y',[Validators.required]),
    });
    this.reactiveFormadd.get('Code_Generation')!.valueChanges.subscribe(value => {
      const componentgroup_code_manual = this.reactiveFormadd.get('componentgroup_code_manual');
      if (value === 'N') {
        componentgroup_code_manual!.setValidators([Validators.required, Validators.pattern(/^\S.*$/)]);
      } else {
        componentgroup_code_manual!.setValidators([Validators.pattern(/^\S.*$/)]);
      }
      componentgroup_code_manual!.updateValueAndValidity();
    });

    this.reactiveFormEdit = new FormGroup({
      componentgroup_gid: new FormControl(''),
      componentgroup_code: new FormControl(''),
      componentgroup_name: new FormControl(this.componentgroupmaster.componentgroup_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      group_belongsto: new FormControl(this.componentgroupmaster.group_belongsto),
      statutory: new FormControl(this.componentgroupmaster.statutory),
    });
  }

  ComponentgroupSummary() {
    var url = 'PayMstComponentgroup/GetComponentgroupSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Componentgroup_list = this.responsedata.Componentgroup_list;

      setTimeout(() => {
        $('#Componentgroup_list').DataTable();
      }, 1);
    });  
  }

  get componentgroup_gid() {
    return this.reactiveFormadd.get('componentgroup_gid')!;
  }
  get componentgroup_code_manual() {
    return this.reactiveFormadd.get('componentgroup_code_manual')!;
  }
  get componentgroup_name() {
    return this.reactiveFormadd.get('componentgroup_name')!;
  }
  get display_name() {
    return this.reactiveFormadd.get('display_name')!;
  }
  get statutory() {
    return this.reactiveFormadd.get('statutory')!;
  }
  get group_belongsto(){
    return this.reactiveFormadd.get('group_belongsto')!;
  }
  onRadioChange(value: string): void {
    debugger;
    this.reactiveFormadd.get('statutory')?.setValue(value);
  }
  onRadioChange1(value: string): void {
    debugger;
    this.reactiveFormadd.get('group_belongsto')?.setValue(value);
  }
  get componentgroup_name1() {
    return this.reactiveFormEdit.get('componentgroup_name')!;
  }

  // get componentgroup_codeedit() {
  //   return this.reactiveFormEdit.get('componentgroup_codeedit')!;
  // }
  // get componentgroup_nameedit() {
  //   return this.reactiveFormEdit.get('componentgroup_nameedit')!;
  // }
  // get display_nameedit() {
  //   return this.reactiveFormEdit.get('display_nameedit')!;
  // }
  // get statutory_edit() {
  //   return this.reactiveFormEdit.get('statutory_edit')!;
  // }
  // get group_belongstoedit() {
  //   return this.reactiveFormEdit.get('group_belongstoedit')!;
  // }
  

  openModaledit(parameter: string) {
    debugger;
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("componentgroup_gid")?.setValue(this.parameterValue1.componentgroup_gid);
    this.reactiveFormEdit.get("componentgroup_code")?.setValue(this.parameterValue1.componentgroup_code);
    this.reactiveFormEdit.get("componentgroup_name")?.setValue(this.parameterValue1.componentgroup_name);
    this.reactiveFormEdit.get("display_name")?.setValue(this.parameterValue1.display_name);
    this.reactiveFormEdit.get("statutory")?.setValue(this.parameterValue1.statutory);
    this.reactiveFormEdit.get("group_belongsto")?.setValue(this.parameterValue1.group_belongsto)

  }

  // public onsubmit(): void {
  //   if (this.reactiveFormadd.value.componentgroup_code != null && this.reactiveFormadd.value.componentgroup_code != '')
  //     if (this.reactiveFormadd.value.componentgroup_name != null && this.reactiveFormadd.value.componentgroup_name != '')
  //       if (this.reactiveFormadd.value.display_name != null && this.reactiveFormadd.value.display_name != '') {
  //         for (const control of Object.keys(this.reactiveFormadd.controls)) {
  //           this.reactiveFormadd.controls[control].markAsTouched();
  //         }
  //         debugger;
  //         this.reactiveFormadd.value;
  //         var url1 = 'PayMstComponentgroup/Postcomponentgroup'

  //         this.service.postparams(url1, this.reactiveFormadd.value).subscribe((result: any) => {
  //           debugger;
  //           if (result.status == false) {
  //             this.ToastrService.warning(result.message)
  //             this.ComponentgroupSummary();
  //           }
  //           else {
  //             this.reactiveFormadd.get("componentgroup_code")?.setValue(null);
  //             this.reactiveFormadd.get("componentgroup_name")?.setValue(null);
  //             this.reactiveFormadd.get("display_name")?.setValue(null);
  //             this.reactiveFormadd.get("statutory")?.setValue(null);
  //             this.ToastrService.success("Salary Component Group Added successfully")
  //             window.location.reload();
  //           }
  //         });
  //       }
  //       else {
  //         this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //       }
  // }
  onsubmit(){
    // console.log(this.reactiveFormadd.value)
    var url = 'PayMstComponentgroup/Postcomponentgroup';
    this.service.post(url, this.reactiveFormadd.value).subscribe((result:any) => {
      if(result.status == false){
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();
      this.ComponentgroupSummary();

      }
      else {
        this.reactiveFormadd.get("componentgroup_name")?.setValue(null);

        this.ToastrService.success(result.message); 
        this.NgxSpinnerService.hide();

        this.reactiveFormadd.reset();
        this.ComponentgroupSummary();
       }
    });
    setTimeout(function() {
      window.location.reload();
  }, 2000);
    
    }

  onclose(){
    this.reactiveFormadd.reset();
  }
  
  public onupdate(): void {
    debugger;
    if (this.reactiveFormEdit.value.componentgroup_name != null && this.reactiveFormEdit.value.componentgroup_name != '') {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;
      var url = 'PayMstComponentgroup/Updatecomponentgroup'
      this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.ComponentgroupSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.ComponentgroupSummary();
        }
      });
    }
  }
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N'; 
  }


  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }


ondelete(){
console.log(this.parameterValue);
    var url3 = 'PayMstComponentgroup/getDeleteComponent'

    let params = {

      componentgroup_gid : this.parameterValue
  
    }

    this.service.getparams(url3,params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        // this.ComponentgroupSummary();

      }
      else {
        this.ToastrService.success(result.message)
        // this.ComponentgroupSummary();
      }

    });
   
  }
}
