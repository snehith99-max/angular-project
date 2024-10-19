import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-hrm-mst-editshifttype',
  templateUrl: './hrm-mst-editshifttype.component.html',
})

export class HrmMstEditshifttypeComponent {
  reactiveFormedit!: FormGroup;
  weekdays_list: any[] = [];
  responsedata: any;
  shiftedit_list: any[] =[];
  shiftlogin_list:any[]=[];
  shiftlogout_list:any[]=[];
  shifttype_gid: any;
  logouttime:any;
  logintime:any;
  Ot_cutoff:any;
  shiftedit_list1: any[] =[];

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private route: Router,
    public NgxSpinnerService: NgxSpinnerService,
    private router: ActivatedRoute,
    private fb: FormBuilder
  ) {

    this.reactiveFormedit = new FormGroup({
      shifttype_gid: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      shift_name: new FormControl(''),
      email_list: new FormControl(''),
      grace_time: new FormControl(''),
      login_scheduler: new FormControl(''),
      entrycutoff_time: new FormControl(''),
      overnight_flag: new FormControl(''),
      inovernight_flag: new FormControl(''),
      outovernight_flag: new FormControl(''),
      logout_schedular: new FormControl(''),
      existcutoff_time: new FormControl(''),
      logout_overnight_flag: new FormControl(''),
      logout_inovernight_flag: new FormControl(''),
      logout_outovernight_flag: new FormControl(''),
      logintime: new FormControl(''),
      logouttime: new FormControl(''),
      Ot_cutoff: new FormControl(''),
      weekdays_list: this.formBuilder.array([]),    
    });    
  }  
  ngOnInit(): void {
    const shifttype_gid = this.router.snapshot.paramMap.get('shifttype_gid');
    this.shifttype_gid = shifttype_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.shifttype_gid, secretKey).toString(enc.Utf8);
    var url = 'ShiftType/GetWeekdaysummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.weekdays_list = this.responsedata.weekday_list;
    //   this.shiftedit_list.forEach((item) => {
    //     item.logintime = ('');
    //     item.logouttime = ('');
    //     item.Ot_cutoff = ('');
    //   });
    });
  
    this.GetEditShiftType(deencryptedParam)
    this.GetEditlogintime(deencryptedParam)
  }
  // get weekdaysArray(): FormArray {
  //   return this.reactiveFormedit.get('weekdays_list') as FormArray;
  // }
  
  GetEditShiftType(shifttype_gid: any) {
    var url = 'ShiftType/GetEditShiftType'    
    let param = {
      shifttype_gid: shifttype_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.shiftedit_list = result.GetEditShiftType;     
      for (let i=0;i<this.shiftedit_list.length;i++) {
        this.reactiveFormedit.addControl(`logintime_${i}`, new FormControl(this.shiftedit_list[i].logintime));
        this.reactiveFormedit.addControl(`logouttime_${i}`, new FormControl(this.shiftedit_list[i].logouttime));
        this.reactiveFormedit.addControl(`Ot_cutoff_${i}`, new FormControl(this.shiftedit_list[i].Ot_cutoff));
      }
      console.log(this.shiftedit_list);            
      this.reactiveFormedit.get("shifttype_gid")?.setValue(this.shiftedit_list[0].shifttype_gid);
      this.reactiveFormedit.get("shift_name")?.setValue(this.shiftedit_list[0].shift_name);
      this.reactiveFormedit.get("email_list")?.setValue(this.shiftedit_list[0].email_list);
      this.reactiveFormedit.get("grace_time")?.setValue(this.shiftedit_list[0].grace_time);
     this.reactiveFormedit.get("logintime")?.setValue(this.shiftedit_list[0].logintime);
     this.reactiveFormedit.get("logouttime")?.setValue(this.shiftedit_list[0].logouttime);
     this.reactiveFormedit.get("Ot_cutoff")?.setValue(this.shiftedit_list[0].Ot_cutoff);      
      this.reactiveFormedit.get("login_scheduler")?.setValue(this.shiftedit_list[0].login_scheduler);      
      this.reactiveFormedit.get("entrycutoff_time")?.setValue(this.shiftedit_list[0].entrycutoff_time);      
      this.reactiveFormedit.get("overnight_flag")?.setValue(this.shiftedit_list[0].overnight_flag);
      this.reactiveFormedit.get("inovernight_flag")?.setValue(this.shiftedit_list[0].inovernight_flag);
      this.reactiveFormedit.get("outovernight_flag")?.setValue(this.shiftedit_list[0].outovernight_flag);
      this.reactiveFormedit.get("logout_schedular")?.setValue(this.shiftedit_list[0].logout_schedular);
      this.reactiveFormedit.get("existcutoff_time")?.setValue(this.shiftedit_list[0].existcutoff_time);
      this.reactiveFormedit.get("logout_overnight_flag")?.setValue(this.shiftedit_list[0].logout_overnight_flag);
      this.reactiveFormedit.get("logout_inovernight_flag")?.setValue(this.shiftedit_list[0].logout_inovernight_flag);
      this.reactiveFormedit.get("logout_outovernight_flag")?.setValue(this.shiftedit_list[0].logout_outovernight_flag);
    });
  }
  GetEditlogintime(shifttype_gid:any){
    var url = 'ShiftType/GetEditlogintime';
    let param ={
      shifttype_gid:shifttype_gid
    }
      this.service.getparams(url, param).subscribe((result: any) => {
        $('#shiftlogin_list').DataTable().destroy();
        this.shiftlogin_list = result.GetEditlogin; 
        $('#shiftlogout_list').DataTable().destroy();
        this.shiftlogout_list = result.GetEditlogout; 
             
        this.reactiveFormedit.get("login_scheduler")?.setValue(this.shiftlogin_list[0].execute_in);
        this.reactiveFormedit.get("entrycutoff_time")?.setValue(this.shiftlogin_list[0].cutoff_time);
        this.reactiveFormedit.get("overnight_flag")?.setValue(this.shiftlogin_list[0].overnight_flag);
        this.reactiveFormedit.get("inovernight_flag")?.setValue(this.shiftlogin_list[0].In_overnightflag);
       this.reactiveFormedit.get("outovernight_flag")?.setValue(this.shiftlogin_list[0].Out_overnightflag);

       this.reactiveFormedit.get("logout_schedular")?.setValue(this.shiftlogout_list[0].execute_out);
        this.reactiveFormedit.get("existcutoff_time")?.setValue(this.shiftlogout_list[0].cutoff_time);
        this.reactiveFormedit.get("logout_overnight_flag")?.setValue(this.shiftlogout_list[0].overnight_flag);
        this.reactiveFormedit.get("logout_inovernight_flag")?.setValue(this.shiftlogout_list[0].In_overnightflag);
       this.reactiveFormedit.get("logout_outovernight_flag")?.setValue(this.shiftlogout_list[0].Out_overnightflag);
  
      });
      setTimeout(() => {
        $('#shiftlogin_list').DataTable();
      }, );
    }
    // GetEditlogout(shifttype_gid:any){
    // var url = 'ShiftType/GetEditlogout'
    // let param={
    //   shifttype_gid:shifttype_gid
    // }
    //   this.service.getparams(url, param).subscribe((result: any) => {
    //     $('#shiftlogout_list').DataTable().destroy();
    //     this.shiftlogout_list = result.GetEditlogout; 
        
    //     this.reactiveFormedit.get("logout_schedular")?.setValue(this.shiftlogout_list[0].execute_out);
    //     this.reactiveFormedit.get("existcutoff_time")?.setValue(this.shiftlogout_list[0].cutoff_time);
    //     this.reactiveFormedit.get("logout_overnight_flag")?.setValue(this.shiftlogout_list[0].overnight_flag);
    //     this.reactiveFormedit.get("logout_inovernight_flag")?.setValue(this.shiftlogout_list[0].In_overnightflag);
    //    this.reactiveFormedit.get("logout_outovernight_flag")?.setValue(this.shiftlogout_list[0].Out_overnightflag);
  
    //   });
    //   setTimeout(() => {
    //     $('#shiftlogout_list').DataTable();
    //   }, );
    // }
  submit(){
    var params = {
      shifttype_gid: this.reactiveFormedit.value.shifttype_gid,
      shift_name: this.reactiveFormedit.value.shift_name,
      email_list: this.reactiveFormedit.value.email_list,
      grace_time: this.reactiveFormedit.value.grace_time,

      weekday_list: this.shiftedit_list,
      logintime: this.reactiveFormedit.value.logintime,
      logouttime: this.reactiveFormedit.value.logouttime,
      Ot_cutoff: this.reactiveFormedit.value.Ot_cutoff,

      login_scheduler: this.reactiveFormedit.value.login_scheduler,
      entrycutoff_time: this.reactiveFormedit.value.entrycutoff_time,
      overnight_flag: this.reactiveFormedit.value.overnight_flag,
      inovernight_flag: this.reactiveFormedit.value.inovernight_flag,
      outovernight_flag: this.reactiveFormedit.value.outovernight_flag,
      
      logout_schedular: this.reactiveFormedit.value.logout_schedular,
      existcutoff_time: this.reactiveFormedit.value.existcutoff_time,
      logout_overnight_flag: this.reactiveFormedit.value.logout_overnight_flag,
      logout_inovernight_flag: this.reactiveFormedit.value.logout_inovernight_flag,
      logout_outovernight_flag: this.reactiveFormedit.value.logout_outovernight_flag,      
    }
    var url = 'ShiftType/Shifteditsubmit'
    this.NgxSpinnerService.show();
    this.service.post(url,params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmMstShiftTypeSummary']);
      }
    }); 
    
  }
}