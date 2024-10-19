import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
@Component({
  selector: 'app-hrm-trn-employee360',
  templateUrl: './hrm-trn-employee360.component.html',
  styleUrls: ['./hrm-trn-employee360.component.scss']
})
export class HrmTrnEmployee360Component {
  RoleDesignation_data: [] = [];
  RoleDesignation: FormGroup | any;
  paymeny_details: any[] = [];
  responsedata: any;
  loan_details: any[] = [];
  attendance_details: any[] = [];
  statutory_details: any[] = [];
  work_experience: any[] = [];
  document_details: any[] = [];
  employee_information: any[] = [];
  general_details: any[] = [];
  acoount_details: any[] = [];
  address_details: any[] = [];
  parameterValue1: any;
  parameterValue: any;
  employeegid: any;
  user_name:any;
  user_code:any;
  branch_name:any;
  department_name:any;
  designation_name:any;
  employee_experience:any;
  employee_mobileno:any;
  employee_qualification:any;
  employee_state:any;
  employee_district:any;
  employee_po:any;
  employee_subdivision:any;employee_taluk:any;employee_village:any;pf_doj:any;stateinsure_no:any;pf_no:any;
  lic_no:any;quarter_no:any;nationality:any;religion:any;marital_status:any;mother_name:any;father_name:any;






  constructor( public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.RoleDesignation = new FormGroup({
      Role_Name: new FormControl('', [Validators.required]),
      Designation_Code: new FormControl('', [Validators.required]),
      Designation_Name: new FormControl('', [Validators.required]),
      designation_gid: new FormControl('')

    })
  }
  ngOnInit(): void {
    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');

    this.employeegid = employee_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.employeegid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    // this.GetEditEmployeeSummary(deencryptedParam)
    let param = {
      employee_gid: deencryptedParam
    }
    var api = 'HrmTrnEmployee360/Getemployeedatabinding';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.paymeny_details = result.Getemployeebinding;
      this.user_name = this.paymeny_details[0].user_name
      this.user_code = this.paymeny_details[0].user_code
      this.branch_name = this.paymeny_details[0].branch_name
      this.department_name = this.paymeny_details[0].department_name
      this.designation_name = this.paymeny_details[0].designation_name      
    });

    var api = 'HrmTrnEmployee360/Getemployeeinformation';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.employee_information = result.Getemployeeinformation;
      this.branch_name = this.paymeny_details[0].branch_name
      this.employee_qualification = this.employee_information[0].employee_qualification
      this.employee_experience = this.employee_information[0].employee_experience
      this.employee_mobileno = this.employee_information[0].employee_mobileno
      this.department_name = this.employee_information[0].department_name      
      this.designation_name = this.employee_information[0].designation_name 
      });
    
      var api = 'HrmTrnEmployee360/Getemployeegeneraldetails';
      this.service.getparams(api,param).subscribe((result: any) => {
        this.general_details = result.Getemployeegeneral;  
        this.father_name = this.general_details[0].father_name
        this.mother_name = this.general_details[0].mother_name
        this.marital_status = this.general_details[0].marital_status
        this.religion = this.general_details[0].religion      
        this.nationality = this.general_details[0].nationality 
        
      });
      
    var api = 'HrmTrnEmployee360/getemployeeaddress';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.address_details = result.getaddrees;
      this.employee_village = this.address_details[0].employee_village
      this.employee_taluk = this.address_details[0].employee_taluk
      this.employee_subdivision = this.address_details[0].employee_subdivision
      this.employee_district = this.address_details[0].employee_district      
      this.employee_state = this.address_details[0].employee_state 
    });
    
    var api = 'HrmTrnEmployee360/getemployeeaccount';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.acoount_details = result.getaccount;
      this.quarter_no = this.acoount_details[0].quarter_no
      this.lic_no = this.acoount_details[0].lic_no
      this.pf_no = this.acoount_details[0].pf_no
      this.stateinsure_no = this.acoount_details[0].stateinsure_no      
      this.pf_doj = this.acoount_details[0].pf_doj 
    });

    var api = 'HrmTrnEmployee360/Getpaymentdetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.paymeny_details = result.GetPaymentdetails;
      setTimeout(() => {
        $('#paymeny_details').DataTable();
      }, );
    });
    var api = 'HrmTrnEmployee360/Getloandetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.loan_details = result.Getloandetails;
      setTimeout(() => {
        $('#loan_details').DataTable();
      }, );
    });
    var api = 'HrmTrnEmployee360/Getattendancetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.attendance_details = result.Getattendancedetails;
      setTimeout(() => {
        $('#statutory_details').DataTable();
      }, );
    });
    var api = 'HrmTrnEmployee360/Getststutorydetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.statutory_details = result.Getstatutorydetails;
      setTimeout(() => {
        $('#work_experience').DataTable();
      }, );
    });
    var api = 'HrmTrnEmployee360/Getdocumentdetailsdetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.document_details = result.Getdocumentdetails;
      setTimeout(() => {
        $('#document_details').DataTable();
      }, );
    });
    var api = 'HrmTrnEmployee360/Getworkexperiencedetails';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.work_experience = result.Getworkexperienedetails;
      setTimeout(() => {
        $('#attendance_details').DataTable();
      }, );
    });
  }

  onback(){
    this.route.navigate(['/hrm/HrmTrnAdmincontrol'])
  }
 
}
