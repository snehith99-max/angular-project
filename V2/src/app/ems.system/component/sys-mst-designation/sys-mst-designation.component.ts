import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

interface IDesignation {
  designation_gid: string;
  designation_code: string;
  designation_name: string;

}

@Component({
  selector: 'app-sys-mst-designation',
  templateUrl: './sys-mst-designation.component.html',
  styleUrls: ['./sys-mst-designation.component.scss'],
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

export class SysMstDesignationComponent {
  invalidFileFormat: boolean = false;
  reactiveFormimport!: FormGroup;
  reactiveForm!: FormGroup;
  //reactiveForm!: FormGroup;
  showOptionsDivId: any;
  reactiveFormEdit!: FormGroup;
  reactiveFormEdit1!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  showInputField: boolean | undefined;
  Code_Generation: any;
  Designation_list: any[] = []
  Designation_erorrlog: any[] = []
  reactiveFormadd: FormGroup;
  Designation!: IDesignation;
  file!: File;
  constructor(public service: SocketService, private route: Router, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
    this.Designation = {} as IDesignation;
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

    this.DesignationSummary();

    //Form values for Add popup//
    this.reactiveForm = new FormGroup({
      designation_gid: new FormControl(''),
      designation_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      designation_status: new FormControl(''),
      Code_Generation: new FormControl('Y'),
      // designation_code_auto: new FormControl(''),
      designation_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    });

    this.reactiveFormimport = new FormGroup({
      designation_import: new FormControl(''),
    });

    this.reactiveFormEdit1 = new FormGroup({
      designation_gid: new FormControl(''),
      designation_code: new FormControl(''),
      designation_name: new FormControl(''),
      designation_status: new FormControl(''),
    });

    //Form values for Edit popup//
    this.reactiveFormEdit = new FormGroup({
      designation_gid: new FormControl(''),
      designation_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      designation_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      designation_status: new FormControl(''),
    });
  }

  DesignationSummary() {
    var url = 'SysMstDesignation/GetDesignationtSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#Designation_list').DataTable().destroy();
      this.responsedata = result;
      this.Designation_list = this.responsedata.Designation_list;
      //console.log(this.branch_list)
      setTimeout(() => {
        $('#Designation_list').DataTable();
      }, 1);
    });
  }

  //Add popup validtion//
  get adddesignation_code() {
    return this.reactiveForm.get('designation_code')!;
  }
  get adddesignation_name() {
    return this.reactiveForm.get('designation_name')!;
  }

  //Edit popup validtion//
  get designation_code() {
    return this.reactiveFormEdit.get('designation_code')!;
  }
  get designation_name() {
    return this.reactiveFormEdit.get('designation_name')!;
  }
  get designation_gid() {
    return this.reactiveFormEdit.get('designation_gid')!;
  }

  //Edit1 popup validtion//
  designation_status() {
    return this.reactiveFormEdit1.get('designation_status')!;
  }

  //Add popup//
  public onsubmit(): void {

    debugger
    if (this.reactiveForm.value.designation_name != null && this.reactiveForm.value.designation_name != '') {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }

      this.reactiveForm.value;
      var url1 = 'SysMstDesignation/PostDesignationAdd'
      this.NgxSpinnerService.show();
      this.service.post(url1, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          this.reactiveForm.reset();
          this.DesignationSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.reactiveForm.reset();
          this.DesignationSummary();
        }
      });
      //  setTimeout(function () {
      //    window.location.reload();
      //  }, 2000);
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  //Edit popup//
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("designation_gid")?.setValue(this.parameterValue1.designation_gid);
    this.reactiveFormEdit.get("designation_code")?.setValue(this.parameterValue1.designation_code);
    this.reactiveFormEdit.get("designation_name")?.setValue(this.parameterValue1.designation_name);
  }

  public onupdate(): void {
    if (this.reactiveFormEdit.value.designation_name != null && this.reactiveFormEdit.value.designation_name != '') {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      console.log(this.reactiveFormEdit.value)
      var url = 'SysMstDesignation/PostUpdateDesignation'
      this.NgxSpinnerService.show();
      this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == true) {
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.DesignationSummary();
        }
        else {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();

          this.DesignationSummary();
        }
      });
      // setTimeout(function () {
      //   window.location.reload();
      // }, 2000);
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
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
    var url = 'SysMstDesignation/GetDesignationActive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
 

      }
      else {
        this.ToastrService.success(result.message)

      }  
      this.DesignationSummary();
    });
  }

  openModalinactive(parameter: string) {
    this.parameterValue = parameter;
  }

  onInactivate() {
debugger
      console.log(this.parameterValue);
    var url = 'SysMstDesignation/GetDesignationInactive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)

      }
      this.DesignationSummary();

    });
  }





  openModalactiveted(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit1.get("designation_gid")?.setValue(this.parameterValue1.designation_gid);
    this.reactiveFormEdit1.get("designation_status")?.setValue(this.parameterValue1.designation_status);
  }

  public onsubmit1(): void {
    if (this.reactiveFormEdit1.value.designation_status != null && this.reactiveFormEdit1.value.designation_status != '') {
      for (const control of Object.keys(this.reactiveFormEdit1.controls)) {
        this.reactiveFormEdit1.controls[control].markAsTouched();
      }
      this.reactiveFormEdit1.value;

      console.log(this.reactiveFormEdit1.value)
      var url4 = 'SysMstDesignation/PostDesignationStatus'

      this.service.postparams(url4, this.reactiveFormEdit1.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning("Error While Updating Designation Status")
          this.DesignationSummary();

        }
        else {
          this.ToastrService.success(" Designation Status Updated Successfully")
          this.DesignationSummary();

        }
      });
      // setTimeout(function () {
      //   window.location.reload();
      // }, 2000);
    }

  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Designation Import";
    window.location.href = "https://" + environment.host + "/Templates/DesignationImport.xlsx";
    link.click();
  }
  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0,
      });
      formData.append("file", this.file, this.file.name);
      var api = 'SysMstDesignation/PostDesignationImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.DesignationSummary();
          this.NgxSpinnerService.hide();

        }
        else {
          this.ToastrService.success(result.message)
          this.DesignationSummary();
          this.NgxSpinnerService.hide();

        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Upload Excel !!')
    }
  }

  onclose() {
    this.reactiveForm.reset();
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
      this.reactiveFormimport.get('designation_import')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveFormimport.get('designation_import')?.reset();
      event.target.value = ''; // Clear the file input field
    }
  }

  onclose1() {
    this.reactiveFormimport.reset();
    this.invalidFileFormat = false;
  }

  geterorrlog() {
    var url = 'SysMstDesignation/GetDesignationtErrorSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Designation_erorrlog = this.responsedata.Designation_list;

    });
  }
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N';

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

