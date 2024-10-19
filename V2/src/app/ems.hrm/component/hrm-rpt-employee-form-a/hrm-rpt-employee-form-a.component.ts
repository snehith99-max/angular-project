import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ExcelService } from 'src/app/Service/excel.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
  employee_photo: string;
  employee_sign: string;
}

@Component({
  selector: 'app-hrm-rpt-employee-form-a',
  templateUrl: './hrm-rpt-employee-form-a.component.html',
  styleUrls: ['./hrm-rpt-employee-form-a.component.scss']
})
export class HrmRptEmployeeFormAComponent {
  invalidFileFormat: boolean = false;
  invalidFileFormat1: boolean = false;
  employeereportformalist: any[] = [];
  responsedata: any;
  reactiveFormadd!: FormGroup;
  file!: File;
  file1!: File;
  employee!: IEmployee;
  user_gid:any;

  constructor(private excelService: ExcelService, public NgxSpinnerService: NgxSpinnerService, private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.employee = {} as IEmployee;
  }
  ngOnInit(): void {

    var url = 'HrmRptEmployeeFormA/GetEmployeeFormA';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.employeereportformalist = this.responsedata.employee_listform;
      setTimeout(() => {
        $('#employeereportformalist').DataTable();
      }, 1);


    });
    this.reactiveFormadd = new FormGroup({
      employee_photo: new FormControl(this.employee.employee_photo, [Validators.required]),
      employee_sign: new FormControl(this.employee.employee_sign, [Validators.required])
    });
  }

  get employee_photo() {
    return this.reactiveFormadd.get('employee_photo')!;
  }
  get employee_sign() {
    return this.reactiveFormadd.get('employee_sign')!;
  }

  openModalphoto(user_gid:any) {
    this.user_gid = user_gid
   }

  onChange4(event: any) {
    this.file = event.target.files[0];
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
    
    if (this.file && validImageTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.reactiveFormadd.get('employee_photo')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveFormadd.get('employee_photo')?.reset();
      event.target.value = ''; // Clear the file input field
    }

  }

  onChange5(event: any) {
    this.file1 = event.target.files[0];
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
    
    if (this.file1 && validImageTypes.includes(this.file1.type)) {
      this.invalidFileFormat1 = false;
      this.reactiveFormadd.get('employee_photo')?.setValue(this.file1);
    } else {
      this.invalidFileFormat1 = true;
      this.reactiveFormadd.get('employee_photo')?.reset();
      event.target.value = ''; // Clear the file input field
    }
  }

  onadd() {
      if (this.file != null && this.file != undefined) {
        let formData = new FormData();
        this.employee = this.reactiveFormadd.value;

        const getFileExtension = (mimeType: any) => mimeType.split('/').pop();
        const employeePhotoFile = new File([this.file], 'EmployeePhoto', { type: this.file.type });
        const employeeSignFile = new File([this.file1], 'EmployeeSign', { type: this.file1.type });

        const file_name = (this.file.type);

        formData.append("Employee_photo", employeePhotoFile, "EmployeePhoto."+  getFileExtension(this.file.type));
        formData.append("Employee_photo", employeeSignFile, "EmployeeSign." +  getFileExtension(this.file.type));
        formData.append("user_gid", this.user_gid);
        
        var api = 'HrmRptEmployeeFormA/GetEmployeeImage'
        this.service.postfile(api, formData).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.reactiveFormadd.reset();
            this.ToastrService.success(result.message)

          }
        });
        
      }
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }

  onclose(){
    this.reactiveFormadd.reset();
    this.invalidFileFormat = false;
    this.invalidFileFormat1 = false;
  }

  exportformAExcel(): void {

    this.NgxSpinnerService.show();
    var url = "HrmRptEmployeeFormA/ExportFormAExcel";
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.employeereportformalist = this.responsedata.employee_listform;


      if (result.status == true) {
        this.service.filedownload1(result);
        this.ngOnInit()
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.NgxSpinnerService.hide();
    });
    (error: any) => {
      console.error('Error:', error);
      this.NgxSpinnerService.hide();
    }
  }
}
