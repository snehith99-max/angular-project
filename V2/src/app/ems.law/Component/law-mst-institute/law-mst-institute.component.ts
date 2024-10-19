import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Iinstitute {
  institute_location: string;
  user_code:string;
  created_by:string;
  created_date: string;
  institute_gid:string;
  Institute_status:string;
  institute_code:string;
  institute_name :string;
  

  
}
@Component({
  selector: 'app-law-mst-institute',
  templateUrl: './law-mst-institute.component.html',
  styleUrls: ['./law-mst-institute.component.scss']
})

export class LawMstInstituteComponent  implements OnInit {

  InstituteList: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit:FormGroup | any;
  confirmPasswordTouched = false;
  reactiveFormReset: FormGroup | any;
  Institute!: Iinstitute;
  parameterValue:any
  parameterValue1:any;
  parameterValueReset: any;
  responsedata: any;
  txtinstitute_location:any;
  txtinstitute_name:any;
  txtpassword:any;
  txtinstitute_code:any;
  isdata:any;
  isref:any;
  institutecode:any;
  institutename:any;
  reset_list: any[] = [];
  showPassword: boolean = false;
  showConfrimPassword: boolean = false;
  password:any;
  confirmpassword:any;
  activeform:any;
  rbo_status:any;
  current_status: any;
  Institutioninactivelog_data:any;
  institutegid: any;


  constructor(private NgxSpinnerService: NgxSpinnerService, private route: Router, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.Institute = {} as Iinstitute;

  }

  ngOnInit(): void {
   this.GetInstituteSummary();
    this.reactiveForm = new FormGroup({
    institute_location: new FormControl(''),
    institute_code: new FormControl(''),
    institute_name: new FormControl(''),
    institute_gid: new FormControl(''),
    confirmpassword: new FormControl({ value: '', disabled: false }),
  });
 
  this.reactiveFormReset = new FormGroup({

    institutecode: new FormControl({ value: null, disabled: true }),    
    institutename: new FormControl({ value: null, disabled: true }),
    password: new FormControl(null,
      [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
      ]),
    institute_gid: new FormControl(''),
    confirmpassword: new FormControl(null, Validators.required),
  });
  this.activeform = new FormGroup({
    rbo_status: new FormControl(''),
    txtremarks: new FormControl(
      '', [Validators.required,Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]),
    institute_name: new FormControl(''),
    institute_gid: new FormControl(''),
  });

  }
  
  GetInstituteSummary() {
    debugger
    this.NgxSpinnerService.show();
    var url= 'LawMstInstitute/GetInstitutesummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.institute_List);
      if(result.institute_List != null){
        $('#InstituteSummary').DataTable().destroy();
        this.InstituteList = result.institute_List; 
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#InstituteSummary').DataTable();
        }, 1); 
      }
      else{
        this.InstituteList = result.institute_List; 
        setTimeout(()=>{   
          $('#InstituteSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#InstituteSummary').DataTable().destroy();
      } 
    });
   }
  get institute_location() {
    return this.reactiveForm.get('institute_location')!;
  }
  get campaign_description() {
    return this.reactiveForm.get('campaign_description')!;
  }

  get user_password() {
    return this.reactiveForm.get('user_password')!;
  }
  userpassword(password: any) {
    this.reactiveForm.get("confirmpassword")?.setValue(password.value);
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }

editInstitute(parameter: string){
  debugger
      const secretKey = 'storyboarderp';
      const param = (parameter);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.route.navigate(['/legal/LglMstInstituteedit', encryptedParam])

}
ViewInstitute(parameter: string){
  debugger
      const secretKey = 'storyboarderp';
      const param = (parameter);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.route.navigate(['/legal/LglMstInstituteview', encryptedParam])
}
openModalReset(parameter: string) {
  debugger
  this.parameterValueReset = parameter;
  this.reactiveFormReset.get("institute_gid")?.setValue(this.parameterValueReset.institute_gid);
  this.reactiveFormReset.get("institutecode")?.setValue(this.parameterValueReset.institute_code);
  this.reactiveFormReset.get("institutename")?.setValue(this.parameterValueReset.institute_name);
}
onupdatereset(){
  debugger
  for (const control of Object.keys(this.reactiveFormReset.controls)) {
    this.reactiveFormReset.controls[control].markAsTouched();
  }
  var url = 'LawMstInstitute/Postinstituteresetpassword'

  this.SocketService.post(url, this.reactiveFormReset.value).pipe().subscribe((result: any) => {
    this.responsedata = result;
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.ToastrService.success('Password updated Successfully')
      this.GetInstituteSummary();
      window.location.reload();
    }
  });
  this.reactiveFormReset.reset();
  
}
togglePasswordVisibility(): void {
  this.showPassword = !this.showPassword;
}

toggleConfrimPasswordVisibility(): void {
  this.showConfrimPassword = !this.showConfrimPassword;
}
passwordsMatch(): boolean {
  const password = this.reactiveFormReset.get('password').value;
  const confirmPassword = this.reactiveFormReset.get('confirmpassword').value;
  return password === confirmPassword;
}
activeInstitute(parameter: string){
  debugger
  this.NgxSpinnerService.show();
  this.parameterValue = parameter
  this.institutegid= this.parameterValue.institute_gid
  this.institutename = this.parameterValue.institute_name;
  this.rbo_status = this.parameterValue.Institute_status;
  this.current_status = this.rbo_status;
  var params = {
    institute_gid: this.parameterValue.institute_gid
  }
  var url = 'LawMstInstitute/InstitutionInactiveHistory'
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    this.Institutioninactivelog_data = result.Institutioninactivelog_data;
    this.NgxSpinnerService.hide();
  });
}
update_status(){
debugger
  if(this.current_status == this.activeform.value.rbo_status){
    if (this.activeform.rbo_status === 'InActive')
    this.ToastrService.warning("Institution is already Inactive.");
    if (this.activeform.rbo_status === 'Active')
    this.ToastrService.warning("Institution is already active.");
  }
  else{
    this.NgxSpinnerService.show();
    var params = {
      institute_gid: this.institutegid,
      institute_name:this.institutename,
      remarks: this.activeform.value.txtremarks,
      Status: this.activeform.value.rbo_status
    }
    var url = 'LawMstInstitute/GetinstituteInactive';
    this.SocketService.post(url, params).subscribe((result: any) => {
  
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.info(result.message)
        this.NgxSpinnerService.hide();
      }
      this.ngOnInit()
    })
  }
}
}
 
 


