import { Component,OnInit } from '@angular/core';
import { FormBuilder,FormControl,FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-hrm-mst-role-add',
  templateUrl: './hrm-mst-role-add.component.html',
  styleUrls: ['./hrm-mst-role-add.component.scss']
})

export class HrmMstRoleAddComponent {
  roleForm : FormGroup | any;
  countryList = [
    { MartialStatus: 'Married', },
    { MartialStatus: 'UnMarried', },
    { MartialStatus: 'Single',},
  ];
  reportingtoList: any;
  txtrolecode: any;
  txtroletitle: any;
  txtreportingto: any;
  txtprobationperiod: any;
  txtjobdescription: any;
  txtroleresponsible: any;
  employeereportingto_list: any;
  Code_Generation: any;
  rolecode_auto: any;
  showInputField: boolean | undefined;
  warningMessage: string = '';
  constructor(private SocketService: SocketService,private fb: FormBuilder,public router:Router,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService){
    this.roleForm = new FormGroup ({
      
      Role_name :  new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      Probation_Period:  new FormControl(null,[Validators.required,Validators.pattern(/^\S.*$/)]),
      // Job_Description:  new FormControl(null,[Validators.required,
      // ]),
      Job_Description: new FormControl(null,[Validators.required]),
      Reporting_to: new FormControl(''),
      Role_Responsible:new FormControl(),   
      Code_Generation: new FormControl('Y'),
      rolecode_auto: new FormControl(''),
      rolecode_manual: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
  })
  
}
  
  get JobDescription(){
    return this.roleForm.get('Job_Description')
  }

  get Role_name(){
    return this.roleForm.get('Role_name')
  }

  get rolecode_manual(){
    return this.roleForm.get('rolecode_manual')
  }

  get ProbationPeriod(){
    return this.roleForm.get('Probation_Period')
  }

  ngOnInit(): void {
    var url = 'ManageRole/PopRoleReportingToAdd';
    this.SocketService.get(url).subscribe((result: any) => {
    this.reportingtoList = result.rolereporting_to;
      
    });
  }
  
  AddSubmit() {
  this.roleForm.value;
  this.NgxSpinnerService.show();
    var url = 'ManageRole/RoleAdd';
    this.SocketService.post(url, this.roleForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.router.navigate(['/hrm/HrmMstRoleSummary']);
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
        this.router.navigate(['/hrm/HrmMstRoleSummary']);
      }
 
  });

}
  
 backbutton(){
    this.router.navigate(['/hrm/HrmMstRoleSummary']);
  }
  onadd(){}
  
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N'; 

  }
  
}
