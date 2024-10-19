import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

interface IAppraisal360 {
  experience: string;
  softskills: string;
  contribution: string;
  grade_no: string;
  recommended_type: string;
}

@Component({
  selector: 'app-hrm-trn-appraisal360',
  templateUrl: './hrm-trn-appraisal360.component.html',
  styleUrls: ['./hrm-trn-appraisal360.component.scss']
})
export class HrmTrnAppraisal360Component {
  appraisaladd360!: IAppraisal360;
  parameterValue: any;
  appraisaldtl_list: any[] = [];
  reactiveForm!: FormGroup;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  showInput5: boolean = false;
  showInput6: boolean = false;
  
  responsedata: any;
  username_list: any;
  emp_firstname: any;
  emp_lastname: any;
  emp_name: any;
  emp_dob: any;
  emp_department: any;
  emp_designation: any;
  emp_branch: any;
  emp_mobile: any;
  employee_emailid: any;

  employee: any;
  user_gid: any;
  review_details: boolean = false;
  work_exp: boolean = false;
  skills_type: boolean = false;
  contribution_type: boolean = false;
  grade_type: boolean = false;
  recommended_for: boolean = false;
  ViewReviewSummary_list:any [] = [];
  recommended: any;
  grade: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, public NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    
    this.appraisaladd360 = {} as IAppraisal360;
  }

  ngOnInit(): void {
    
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options); 

    const user_gid =this.route.snapshot.paramMap.get('user_gid');
    this.employee = user_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.getViewReviewSummary(deencryptedParam);
    this.GetAppraisalDetailSummary(deencryptedParam);
    this.UserList(deencryptedParam);
    this.user_gid=deencryptedParam;

    this.reactiveForm = new FormGroup({
      review_details: new FormControl(''),
      Reviewed_by: new FormControl(''),
      work_exp: new FormControl(''),
      experience: new FormControl(this.appraisaladd360.experience, []),
      skills_type: new FormControl(''),
      softskills: new FormControl(this.appraisaladd360.softskills, []),
      contribution_type: new FormControl(''),
      contribution: new FormControl(this.appraisaladd360.contribution, []),
      grade_type: new FormControl(''),
      grade_no: new FormControl(this.appraisaladd360.grade_no, []),
      recommended_for: new FormControl(''),
      recommended_type: new FormControl(this.appraisaladd360.recommended_type, []),
      employee_gid: new FormControl(''),
    });
   
  }

  toggletextbox5(){
    if (this.review_details) {
      this.review_details = true;
    }
    else{
      this.review_details = false;
    }
   }
   toggletextbox6(){
    if (this.work_exp) {
      this.work_exp = true;
    }
    else{
      this.work_exp = false;
    }
   }
   toggletextbox7(){
    if (this.skills_type) {
      this.skills_type = true;
    }
    else{
      this.skills_type = false;
    }
   }
   toggletextbox8(){
    if (this.contribution_type) {
      this.contribution_type = true;
    }
    else{
      this.contribution_type = false;
    }
   }
   toggletextbox9(){
    if (this.grade_type) {
      this.grade_type = true;
    }
    else{
      this.grade_type = false;
    }
   }
   toggletextbox10(){
    if (this.recommended_for) {
      this.recommended_for = true;
    }
    else{
      this.recommended_for = false;
    }
   }
  
  

  GetAppraisalDetailSummary(user_gid: any) {
    var url = 'HrmTrnAppraisalManagement/GetAppraisalDetailSummary'
    let param = {
      user_gid : user_gid 
    }
    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.appraisaldtl_list = this.responsedata.appraisaldtllist;
      setTimeout(() => {
        $('#appraisaldtl_list').DataTable();
      }, );


    });
  }

  getViewReviewSummary(user_gid: any) {
    var url='HrmTrnAppraisalManagement/getViewReviewSummary'
    let param = {
      user_gid : user_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.ViewReviewSummary_list = result.ViewReviewSummarylist;   
    });
}

  showTextBox1(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput1 = target.value === 'REVIEW_DETAILS' ;
   }

   showTextBox2(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput2 = target.value === 'EXPERIENCE' ;
   }

   showTextBox3(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput3 = target.value === 'SKILLS' ;
   }

   showTextBox4(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput4 = target.value === 'CONTRIBUTION' ;
   }

   showTextBox5(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput5 = target.value === 'GRADE' ;
   }

   showTextBox6(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput6 = target.value === 'RECOMMENDED' ;
   }

  UserList(user_gid: any) {
   
    var url = 'HrmTrnAppraisalManagement/UserList'
    let param = {
      user_gid : user_gid 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      this.responsedata = result;
      this.username_list = this.responsedata.usernamelist;
      this.emp_firstname = this.username_list[0].user_name;
      this.emp_lastname = this.username_list[0].user_lastname;
      this.emp_name = this.username_list[0].user_firstname;
      this.emp_dob = this.username_list[0].dob;
      this.emp_department = this.username_list[0].department_name;
      this.emp_designation = this.username_list[0].designation_name;
      this.emp_branch = this.username_list[0].branch_name;
      this.emp_mobile = this.username_list[0].employee_mobileno;
      this.employee_emailid = this.username_list[0].employee_emailid;
      
      
     
    });
  }

  onsubmit(){

    var url = 'HrmTrnAppraisalManagement/PostAppraisalDtl'
   
    var params={
      employee_gid:this.username_list[0].employee_gid,

      Reviewed_by: "Reviewed_by",
      experience:this.reactiveForm.value.experience,
      softskills:this.reactiveForm.value.softskills,
      contribution:this.reactiveForm.value.contribution,
      grade_no:this.reactiveForm.value.grade_no,
      recommended_type:this.reactiveForm.value.recommended_type,
    }
  
    this.service.post(url, params).subscribe((result: any) => {
      
      if (result.status == false) {
        this.ToastrService.warning('Error While Adding Appraisal Details')
       
      }
      else {
       
        this.ToastrService.success('Appraisal Details Added Successfully')
       
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
    });
   
  }

  back(){
 this.router.navigate(['/hrm/HrmTrnAppraisalmanagement'])
  }

 ////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter

}

ondelete() {
 
  console.log(this.parameterValue);
  var url = 'HrmTrnAppraisalManagement/DeleteAppraisalDetail'
  this.service.getid(url, this.parameterValue).subscribe((result: any) => {
   
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.ToastrService.success(result.message)
    }
    setTimeout(function() {
      window.location.reload();
  }, 2000); // 2000 milliseconds = 2 seconds

  });

}

}
