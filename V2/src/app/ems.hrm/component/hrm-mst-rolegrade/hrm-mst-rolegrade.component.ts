import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup,Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hrm-mst-rolegrade',
  templateUrl: './hrm-mst-rolegrade.component.html',
  styleUrls: ['./hrm-mst-rolegrade.component.scss'],
  
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
export class HrmMstRolegradeComponent {
  
 RoleGrade_data : []=[];
  RoleGrade: FormGroup | any;
  rolegrade_list:any[]=[];
  responsedata: any;
  Edit: FormGroup | any;
  parameterValue1: any;
  parameterValue: any;
  showInputField: boolean | undefined;
  Code_Generation: any;
  showOptionsDivId: any;
 constructor(private SocketService: SocketService,private NgxSpinnerService: NgxSpinnerService,private route:Router,private router:ActivatedRoute,public service: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder) {
  
  this.RoleGrade=new FormGroup({
    role_code_auto: new FormControl(''),
    role_code_manual: new FormControl('',[Validators.required]),
    RoleGradename: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
    gradelevel_gid: new FormControl(''),
    Code_Generation: new FormControl('Y'),
    rolegradecode:new FormControl('')


  })


 }


ngOnInit() {
  var api = 'RoleGrade/GetRoleGradeSummary';
  this.service.get(api).subscribe((result: any) => {
    $('#rolegrade_list').DataTable().destroy();
    this.responsedata = result;
    this.rolegrade_list = this.responsedata.RoleGradeLists;
    setTimeout(() => {
      $('#rolegrade_list').DataTable();
    }, 1);
  });
  
  
  
}

get RoleGradename() {
  return this.RoleGrade.get('RoleGradename')!;
}
  get role(){
  return this.RoleGrade.get('RoleGradename')
}
addRoleGrade(){
  // var params = {
  //   entity_name:this.addEntityFormData.txtEntity_name,
  //   entity_code:this.addEntityFormData.txtEntity_code,
  // }
  if (this.RoleGrade.value.RoleGradename != null && this.RoleGrade.value.RoleGradename !== '') {
    for (const control of Object.keys(this.RoleGrade.controls)) {
      this.RoleGrade.controls[control].markAsTouched();
    }
    this.RoleGrade.value;
    this.NgxSpinnerService.show();
  var url = 'RoleGrade/PostRoleGrade';
  this.SocketService.post(url, this.RoleGrade.value).subscribe((result:any) => {
    if(result.status == true){
      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
      this.RoleGrade.reset();
    }
    else {
          
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
      this.RoleGrade.reset();

       }
    this.ngOnInit();
  })

}
else {
  this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
}

}
openModaledit(parameter: string) {
  debugger;
  this.parameterValue1 = parameter
  this.RoleGrade.get("rolegradecode")?.setValue(this.parameterValue1.gradelevel_code);
  this.RoleGrade.get("RoleGradename")?.setValue(this.parameterValue1.gradelevel_name);
  this.RoleGrade.get("gradelevel_gid")?.setValue(this.parameterValue1.gradelevel_gid);  
}
updaterolegrade(){
  if (this.RoleGrade.value.RoleGradename != null && this.RoleGrade.value.RoleGradename !== '') {
    for (const control of Object.keys(this.RoleGrade.controls)) {
      this.RoleGrade.controls[control].markAsTouched();
    }
  this.NgxSpinnerService.show();
  var url = 'RoleGrade/getUpdatedRoleGrade';
  this.SocketService.post(url, this.RoleGrade.value).subscribe((result:any) => {
    if(result.status == true){
      this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message);
      this.RoleGrade.reset();
    }
    else {
          
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
      this.RoleGrade.reset();

      
    }
    this.ngOnInit();
  })

}
else {
  this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
}
    

}

closeadd(){
  this.RoleGrade.reset();
}

ondelete(){
  
  this.NgxSpinnerService.show();

  var url = 'RoleGrade/DeleteRoleGrade';

  let params = {

    gradelevel_gid : this.parameterValue

  }

  this.SocketService.getparams(url, params).subscribe((result:any) => {

    if(result.status == true){

      this.NgxSpinnerService.hide();

      this.ToastrService.success(result.message);

      this.ngOnInit();

    }

    else {

      this.ToastrService.warning(result.message);

      this.NgxSpinnerService.hide();

      this.ngOnInit();

    }  

  })

}

deletemodal(parameter: string) {
  this.parameterValue = parameter
}


close(){
  this.RoleGrade.reset();

}

toggleInputField() {
  this.showInputField = this.Code_Generation === 'N'; 

}
} 
