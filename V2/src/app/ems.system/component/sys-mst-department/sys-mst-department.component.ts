import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

interface IEntity {
  department_name: string;
  department_code: string;
  department_gid: string;
  department_prefix: string;
  employee_gid: string;
  department_name_edit: string;
  department_code_edit: string;
  department_manager_edit: string;
  department_prefix_edit: string;
}

@Component({
  selector: 'app-sys-mst-department',
  templateUrl: './sys-mst-department.component.html',
  styleUrls: ['./sys-mst-department.component.scss'],
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

export class SysMstDepartmentComponent {
  invalidFileFormat: boolean = false;
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormimport!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  file!: File;
  department_list: any[] = [];
  Departmet_erorrlog: any[] = [];
  department!: IEntity;
  GetDepartmentAddDropdowns: any[] = [];
  reactiveFormadd: FormGroup;

  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.department = {} as IEntity;
    this.reactiveFormadd = new FormGroup({
      remarks: new FormControl('')  
    })
  }

  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetDepartmentSummary();
    // Form values for Add popup//
    this.reactiveForm = new FormGroup({
      department_name: new FormControl(this.department.department_name, [Validators.required, Validators.pattern(/^\S.*$/)]),
      department_code: new FormControl(this.department.department_code, [Validators.required, Validators.pattern(/^\S.*$/)]),
      department_prefix: new FormControl(''),
      employee_gid: new FormControl(''),
      department_gid: new FormControl(''),
    });

    this.reactiveFormimport = new FormGroup({
      department_import: new FormControl(''),
    });

    this.reactiveFormEdit = new FormGroup({
      department_name_edit: new FormControl(this.department.department_name_edit, [Validators.required, Validators.pattern(/^\S.*$/)]),
      department_code_edit: new FormControl(this.department.department_code_edit, [Validators.required,]),
      department_prefix_edit: new FormControl(this.department.department_prefix_edit, []),
      department_gid: new FormControl(''),
      employee_gid: new FormControl(''),
    });

    var url = 'Department/GetDepartmentAddDropdown';
    this.service.get(url).subscribe((result: any) => {
      this.GetDepartmentAddDropdowns = result.GetDepartmentAddDropdown;
    });
  }

  GetDepartmentSummary() {
    var url = 'Department/GetDepartmentSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#department_list').DataTable().destroy();
      this.responsedata = result;
      this.department_list = this.responsedata.department_list;
      setTimeout(() => {
        $('#department_list').DataTable();
      },);
    });
  }

  get department_name() {
    return this.reactiveForm.get('department_name')!;
  }

  get department_code() {
    return this.reactiveForm.get('department_code')!;
  }

  get department_prefix() {
    return this.reactiveForm.get('department_prefix')!;
  }

  get department_name_edit() {
    return this.reactiveFormEdit.get('department_name_edit')!;
  }

  get department_code_edit() {
    return this.reactiveFormEdit.get('department_code_edit')!;
  }

  get department_prefix_edit() {
    return this.reactiveFormEdit.get('department_prefix_edit')!;
  }

  public onsubmit(): void {
    
      if (this.reactiveForm.value.department_code != null && this.reactiveForm.value.department_code != '') {
        this.reactiveForm.value;
        var url = 'Department/PostDepartment'
        this.NgxSpinnerService.show();
        this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {
          if (result.status == false) {
            this.GetDepartmentSummary();
              this.ToastrService.warning(result.message)
              this.NgxSpinnerService.hide();
              this.reactiveForm.reset();
            
          }
          else {
            this.GetDepartmentSummary();
            this.reactiveForm.reset();
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide();

            
          }
        });
        // setTimeout(function () {
        //   window.location.reload();
        // }, 2000);
      }
      else {
        this.ToastrService.warning("Kindly fill the mandatory fields")
      }
    this.reactiveForm.reset();
    this.GetDepartmentSummary();
  }

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("department_name_edit")?.setValue(this.parameterValue1.department_name);
    this.reactiveFormEdit.get("department_code_edit")?.setValue(this.parameterValue1.department_code);
    this.reactiveFormEdit.get("department_prefix_edit")?.setValue(this.parameterValue1.department_prefix);
    this.reactiveFormEdit.get("department_gid")?.setValue(this.parameterValue1.department_gid);
  }
  ////////////Update popup////////
  public onupdate(): void {
    if (this.reactiveFormEdit.value.department_name_edit != null && this.reactiveFormEdit.value.department_name_edit != '') {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      var url = 'Department/getUpdatedDepartment'
      this.NgxSpinnerService.show();
      this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.NgxSpinnerService.hide();
          this.GetDepartmentSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.GetDepartmentSummary();
        }
      });
      // setTimeout(function () {
      //   window.location.reload();
      // }, 2000);
     
    }
  }
  toggleStatus(data: any) {
    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }
  openModalactive(parameter: string) {
    this.parameterValue = parameter;
  }

  onActivate() {
    console.log(this.parameterValue);
    var url = 'Department/GetDepartmentActive'
    this.NgxSpinnerService.show();
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }  
      this.GetDepartmentSummary();
    });
  }

openModalinactive(parameter: string) {
  this.parameterValue = parameter;
}

oninactive() {
  console.log(this.parameterValue);
  var url = 'Department/GetDepartmentInactive'
  this.NgxSpinnerService.show();
  this.service.getid(url, this.parameterValue).subscribe((result: any) => {

    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();

    }
    else {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
    }
    this.GetDepartmentSummary();

  });
}

  ////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url = 'Department/DeleteDepartment'
    this.NgxSpinnerService.show();
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      this.GetDepartmentSummary();
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  onclose() {
    this.reactiveForm.reset();
  }

  onclose1() {
    this.reactiveFormimport.reset();
    this.invalidFileFormat = false;
  }
 

  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Department Import";
    window.location.href = "https://" + environment.host + "/Templates/DepartmentImport.xlsx";
    link.click();
  }

  importexcel() {
    debugger;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0,
      });
      formData.append("file", this.file, this.file.name);
      var api = 'Department/PostDepartmentImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetDepartmentSummary();
          this.NgxSpinnerService.hide();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Upload Excel !!')
    }   
  }

  onChange1(event: any) {
    this.file = event.target.files[0];
    // Update the valid types to include Excel formats
const validExcelTypes = [
  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', // .xlsx
  'application/vnd.ms-excel' // .xls
];
    
    if (this.file && validExcelTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.reactiveFormimport.get('department_import')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveFormimport.get('department_import')?.reset();
      event.target.value = ''; // Clear the file input field
    }
  }
  geterorrlog() {
    var url = 'Department/GetDepartmentErrorSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Departmet_erorrlog = this.responsedata.department_list;
      setTimeout(() => {
        $('#department').DataTable();
      },);
    });
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  close(){
    this.reactiveFormadd.reset();
  }
}