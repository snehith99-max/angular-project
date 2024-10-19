import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

interface IAppraisalAdd {
  peer_name: string;
  manager_name: string;
  management_name: string;
}


@Component({
  selector: 'app-hrm-trn-appraisaladd',
  templateUrl: './hrm-trn-appraisaladd.component.html',
  styleUrls: ['./hrm-trn-appraisaladd.component.scss']
})
export class HrmTrnAppraisaladdComponent {
  appraisaladd!: IAppraisalAdd;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  self_review: boolean = false;
  
  peer_dropdown: any; 
  manager_review: boolean = false;
  manager_dropdown: any;
  management_review: boolean = false;
  management_dropdown: any;
  reactiveForm2!: FormGroup;
  peer_review: boolean = false;
  

 
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
  
 

  employee_list: any;
  peer_list: any;
  manager_list: any;
  management_list: any;
  user_gid: any;
  employee: any;
  self_name: boolean = false;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, public NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.appraisaladd = {} as IAppraisalAdd;
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
    this.UserList(deencryptedParam);
    this.user_gid=deencryptedParam;
  


    this.reactiveForm2 = new FormGroup({
      self_review: new FormControl(''),
      self_name: new FormControl(''),
      peer_review: new FormControl(''),
      peer_name: new FormControl(this.appraisaladd.peer_name, []),
      manager_review: new FormControl(''),
      manager_name: new FormControl(this.appraisaladd.manager_name, []),
      management_review: new FormControl(''),
      management_name: new FormControl(this.appraisaladd.management_name, []),
      employee_gid: new FormControl(''),
     
    });

    var url = 'HrmTrnAppraisalManagement/GetPeerDetail';
    this.service.get(url).subscribe((result: any) => {
      this.peer_list = result.GetPeer_Detail;
    
    });

    var url = 'HrmTrnAppraisalManagement/GetManagerDetail';
    this.service.get(url).subscribe((result: any) => {
      this.manager_list = result.GetManager_Detail;
    
    });

    var url = 'HrmTrnAppraisalManagement/GetManagementDetail';
    this.service.get(url).subscribe((result: any) => {
      this.management_list = result.GetManagement_Detail;
    
    });


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

toggletextbox1(){
  if (this.self_review) {
    this.self_review = true;
  }
  else{
    this.self_review = false;
  }
 }

 

   toggletextbox2(){
    if (this.peer_review) {
      this.peer_review = true;
    }
    else{
      this.peer_review = false;
    }
   }
   toggletextbox3(){
    if (this.manager_review) {
      this.manager_review = true;
    }
    else{
      this.manager_review = false;
    }
   }
   toggletextbox4(){
    if (this.management_review) {
      this.management_review = true;
    }
    else{
      this.management_review = false;
    }
   }
  showTextBox2(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput2 = target.value === 'PEER' ;
    if (this.showInput2) {
      const peer_review = document.getElementById('peer_review') as HTMLInputElement;
      const peer_name = document.getElementById('peer_name') as HTMLInputElement;
      peer_review.disabled = false; // Enable the checkbox
      peer_name.style.display = "PEER"; // Show the textbox
  } 
  else {
    const peer_review = document.getElementById('peer_review') as HTMLInputElement;
    const peer_name = document.getElementById('peer_name') as HTMLInputElement;
    peer_review.disabled = true; // Disable the checkbox
    peer_name.style.display = "PEER"; // Hide the textbox
  }
   }
   hideTextBox2(event: Event){
    const target = event.target as HTMLInputElement;
    this.showInput2 = target.value === 'PEER' ;
    const peer_review = document.getElementById('peer_review') as HTMLInputElement;
    const peer_name = document.getElementById('peer_name') as HTMLInputElement;
    peer_review.disabled = true; // Disable the checkbox
    peer_name.style.display = "PEER"; 
   }

  showTextBox3(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput3 = target.value === 'MANAGER' ;
   }

  showTextBox4(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput4 = target.value === 'MANAGEMENT' ;
   }

 

 


  back() {
    this.router.navigate(['/hrm/HrmTrnAppraisalmanagement'])
  }

  onsubmit(){
    
    var url = 'HrmTrnAppraisalManagement/PostReview'
    this.NgxSpinnerService.show();
   
    var params={
     
      emp_firstname:this.emp_firstname,
      emp_department:this.emp_department,
      emp_designation:this.emp_designation,
      emp_branch:this.emp_branch,
      emp_dob:this.emp_dob,
      emp_mobile:this.emp_mobile,
      employee_gid:this.username_list[0].employee_gid,

      self_name: "Self",
      peer_name:this.reactiveForm2.value.peer_name,
      manager_name:this.reactiveForm2.value.manager_name,
      management_name:this.reactiveForm2.value.management_name,
    
      
    }
  
    this.service.post(url, params).subscribe((result: any) => {
     
      
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      this.router.navigate(['/hrm/HrmTrnAppraisalmanagement']);
    });
  }

}
