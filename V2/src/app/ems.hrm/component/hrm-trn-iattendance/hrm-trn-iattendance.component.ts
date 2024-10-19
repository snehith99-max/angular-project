import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HeaderComponent } from '../../../layout/components/header/header.component';
import { Router,ActivatedRoute } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';



@Component({
  selector: 'app-hrm-trn-iattendance',
  templateUrl: './hrm-trn-iattendance.component.html',
  styleUrls: ['./hrm-trn-iattendance.component.scss']
})

export class HrmTrnIattendanceComponent {
  attendanceform: FormGroup | any;
  responsedata: any;
  Time: string;
   Date: any;
  punchiattandence: any;
  login_time: any;
  logout_time: any;
  punch_flag: any;
 

  ngOnInit() {
    setInterval(() => {
      this.Time = new Date().toString();
    }, 1000);
    this.punchinoutcall()
  }
  punchinoutcall(){
    var api = 'hrmTrnDashboard/Punchinlogin';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      debugger;
      this.punchiattandence = this.responsedata.punchinlogin;
      if(this.punchiattandence !=null){
        this.login_time = this.punchiattandence[0].login_time;
        this.logout_time = this.punchiattandence[0].logout_time;
        debugger;
        if(this.login_time == null && this.logout_time ==null){
          this.punch_flag='punchin';
        }
        else if(this.logout_time == null ||this.logout_time == ""){
          this.punch_flag='punchout';
        }
        else{
          this.punch_flag='nothing';
        }

      } 
      else{
        this.punch_flag='punchin';
      }
     

      
    });
  }

  constructor(private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService) {
    this.Time = new Date().toString();
    HeaderComponent.constructor(); {
      setInterval(() => {
        this.Time = new Date().toString();
      }, 1000);
    }
    {
      this.attendanceform = new FormGroup({
        location: new FormControl('', [Validators.required]),
      });
    }
  }

  punchin() {
   
    this.NgxSpinnerService.show();
    const url = 'Iattendance/PostSignIn';
    this.service.post(url, this.attendanceform.value).subscribe((result: any) => {
        if (result.status) {
            // Handle success
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.punchinoutcall();
        } else {
            // Handle failure
            this.ToastrService.warning(result.message);
        }
        // Hide spinner in both cases
        this.NgxSpinnerService.hide();
    });
  }  
  punchout() {
   
    this.NgxSpinnerService.show();
    const url = 'Iattendance/PostSignOut';
    this.service.post(url, this.attendanceform.value).subscribe((result: any) => {
        if (result.status) {
            // Handle success
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.punchinoutcall();
        } else {
            // Handle failure
            this.ToastrService.warning(result.message);
        }
        // Hide spinner in both cases
        this.NgxSpinnerService.hide();

    });
  } 
   //Add popup validtion//
 get location() {
  return this.attendanceform.get('location')!;
   }
 
}

function In() {
  throw new Error('Function not implemented.');
}