import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { AbstractControl} from '@angular/forms'; 


@Component({
  selector: 'app-hrm-mst-roledesignation',
  templateUrl: './hrm-mst-roledesignation.component.html', 
  styleUrls: ['./hrm-mst-roledesignation.component.scss'],
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

export class HrmMstRoledesignationComponent {
  RoleDesignation_data: [] = [];
  RoleDesignation: FormGroup | any;
  roledesignation_list: any[] = [];
  responsedata: any;
  Role_list: any[] = [];
  parameterValue1: any;
  parameterValue: any;
  Code_Generation: any;
  showInputField: boolean | undefined;
  // Designation_Name:any;
  showWarning: boolean = false;
 
  warningMessage: string = '';
  mdlRole_Name: any;

  showOptionsDivId: any;
  

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.RoleDesignation = new FormGroup({
      Role_Name: new FormControl('', [Validators.required]),     
      Designation_Name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      designation_gid: new FormControl(''),
      Designation_code_auto: new FormControl(''),
      Designation_code_manual: new FormControl('',[Validators.required]),   
      Code_Generation: new FormControl('Y'),
      Designation_Code:new FormControl(''),
    })
  }
  get Role_Name() {
    return this.RoleDesignation.get('Role_Name')!;
  }
  get Designation_Name() {
    return this.RoleDesignation.get('Designation_Name')!;
  }
 

  // get DesignationNameControl() {
  //   return this.RoleDesignation.get('Designation_Name');
  // }
  ngOnInit(): void {
    var api = 'RoleDesignation/GetRoleDesignationdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.Role_list = result.rolelists;
    });

    this.roledesignationsummary()
  }


  

  roledesignationsummary() {
    var api = 'RoleDesignation/GetRoleDesignationSummary';
    this.service.get(api).subscribe((result: any) => {
     $('#roledesignation_list').DataTable().destroy();
      this.roledesignation_list = result.RoleDesignationLists;
      setTimeout(() => {
        $('#roledesignation_list').DataTable();
      }, 1);
    });
  }
  addRoleDesignation() {
    if (this.RoleDesignation.value.Role_Name != null && this.RoleDesignation.value.Role_Name != '' &&
        this.RoleDesignation.value.Designation_Name != null && this.RoleDesignation.value.Designation_Name !== '') {
      for (const control of Object.keys(this.RoleDesignation.controls)) {
        this.RoleDesignation.controls[control].markAsTouched();
      }
      this.NgxSpinnerService.show();
    var url = 'RoleDesignation/PostRoleDesignation';
    this.SocketService.postparams(url, this.RoleDesignation.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide()
        this.RoleDesignation.reset();
        this.roledesignationsummary()
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide()
      }
    })
  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }  
  }
  openModaledit(parameter: string) {
    debugger;
    this.parameterValue1 = parameter
    this.RoleDesignation.get("Role_Name")?.setValue(this.parameterValue1.Role_Name);
    this.RoleDesignation.get("Designation_Code")?.setValue(this.parameterValue1.Designation_Code);
    this.RoleDesignation.get("Designation_Name")?.setValue(this.parameterValue1.Designation_Name);
    this.RoleDesignation.get("designation_gid")?.setValue(this.parameterValue1.designation_gid );
  }
  updateroledesignation(){
    if (this.RoleDesignation.value.Role_Name != null && this.RoleDesignation.value.Role_Name != '' &&
      this.RoleDesignation.value.Designation_Name != null && this.RoleDesignation.value.Designation_Name !== '') {
    for (const control of Object.keys(this.RoleDesignation.controls)) {
      this.RoleDesignation.controls[control].markAsTouched();
    }
    this.NgxSpinnerService.show();
    var url = 'RoleDesignation/getUpdatedRoleDesignation';
    this.SocketService.post(url, this.RoleDesignation.value).subscribe((result:any) => {
      if(result.status == true){
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide()
        this.roledesignationsummary();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide()
        this.roledesignationsummary();
      }
    })
  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }   
  
  }
  ondelete(){
  var url = 'RoleDesignation/DeleteRoleDesignation';
  let params = {
  designation_gid : this.parameterValue
  }
  this.SocketService.getparams(url, params).subscribe((result:any) => {
  if(result.status == true){
  this.ToastrService.success(result.message);
  this.roledesignationsummary();
  }
  else {
  this.ToastrService.warning(result.message);
  this.roledesignationsummary();
  }  
  })
  }
  deletemodal(parameter: string) {
    this.parameterValue = parameter
  }
  
  closeadd(){
    this.RoleDesignation.reset();
  }
  
  close() {
    this.RoleDesignation.reset();
  }
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N'; 
  }


  // checkForSpace(event: any) {
  //   const inputValue: string = event.target.value;
  //   if (inputValue && inputValue.trim() !== inputValue) {
  //     // If the input has leading spaces, set the warning message
  //     this.warningMessage = 'Space at the beginning is not allowed.';
  //   } else {
  //     this.warningMessage = '';
  //   }
  // }

}