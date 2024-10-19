import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';

interface IRegion {
  region_name: string;
  region_code: string;
  region_gid: string;
  city_name:string;
  region_name_edit: string;
  region_code_edit: string;
  city_name_edit:string
}
@Component({
  selector: 'app-otl-mst-daytrackersummary',
  templateUrl: './otl-mst-daytrackersummary.component.html',
  styleUrls: ['./otl-mst-daytrackersummary.component.scss']
})
export class OtlMstDaytrackersummaryComponent {

  isReadOnly = true; 
  private unsubscribe: Subscription [] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormotp!: FormGroup;
  responsedata: any;
  campaigntitle:any;
  parameterValue: any;
  parameterValue1: any;
  outletname_list:any;
  otpverification_list:any;
  daytracker_list: any[] = [];
  region!: IRegion;
  showOptionsDivId: any;
  rows: any[] = [];
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,private router: Router, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
    this.region = {} as IRegion;
  }
  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    // Form values for Add popup/////
    this.GetRegionSummary();   
    this.reactiveFormEdit = new FormGroup({
      campaigntitle: new FormControl({ value: null, disabled: true }),    
      edit_reason : new FormControl(''),
      daytracker_gid: new FormControl(''),
   });
   this.reactiveFormotp = new FormGroup({
    daytracker_gid: new FormControl(''),
    tracker_gid: new FormControl(''),
    otp_input : new FormControl(null,[Validators.required, Validators.pattern("^(?!\s*$).+")]),
 });  
  }
  GetRegionSummary() {
    var url = 'DayTracker/GetdaytrackerSummary'
    this.service.get(url).subscribe((result: any) => {
      debugger
      $('#daytracker_list').DataTable().destroy();
      this.responsedata = result;
      this.daytracker_list = this.responsedata.daytrackersummary_list;
      setTimeout(() => {
        $('#daytracker_list').DataTable();
      }, 1);
    });
  }
  get edit_reason() {
    return this.reactiveFormEdit.get('edit_reason')!;
  }
  get campaign_title() {
    return this.reactiveFormEdit.get('campaign_title')!;
  }
  get daytracker_gid() {
    return this.reactiveFormEdit.get('daytracker_gid')!;
  }
  get otp_input() {
    return this.reactiveFormotp.get('otp_input')!;
  }
  get tracker_gid() {
    return this.reactiveFormotp.get('tracker_gid')!;
  }
  onadd() {
    this.router.navigate(['/outlet/otlmstdaytrackeradd'])
  }
  onedit(params:any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/otlmstdaytrackeredit',encryptedParam])
  }
  editrequest(parameter:any){
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("daytracker_gid")?.setValue(this.parameterValue1);
    var url = 'DayTracker/GetOutletname'
    this.service.get(url).subscribe((result: any) => {
      this.outletname_list = result.outletname_list
      this.campaigntitle=this.outletname_list[0].campaign_title
    });
  }
  Viewedittracker(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/otlmstdaytrackerview',encryptedParam])
  }
  onrequest(){
    debugger;
    var params = {
      campaign_title:this.reactiveFormEdit.getRawValue().campaigntitle,
      edit_reason: this.reactiveFormEdit.value.edit_reason,
      daytracker_gid: this.reactiveFormEdit.value.daytracker_gid,
    }
          var url = 'DayTracker/Posteditrequest'
          this.NgxSpinnerService.show()
          this.service.postparams(url, params).subscribe((result: any) => {
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.NgxSpinnerService.hide();
            }
            else {
              this.ToastrService.success(result.message)
              this.reactiveFormEdit.reset();
              window.location.reload();
              this.NgxSpinnerService.hide();
            }
          });

  }
  otpvefication(parameter:any){
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormotp.get("daytracker_gid")?.setValue(this.parameterValue1.daytracker_gid);
    this.reactiveFormotp.get("tracker_gid")?.setValue(this.parameterValue1.tracker_gid);
    var params={
    daytracker_gid:this.parameterValue1.daytracker_gid,
    }
    var url = 'DayTracker/Getotpverification'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.otpverification_list = result.otpverification_list
    });

  }
  onotpupdate(){
    debugger;
    var params = {
      approval_otp: this.reactiveFormotp.value.otp_input,
      daytracker_gid: this.reactiveFormotp.value.daytracker_gid,
      tracker_gid: this.reactiveFormotp.value.tracker_gid,
    }
          var url = 'DayTracker/Posteditotp'
          this.NgxSpinnerService.show()
          this.service.postparams(url, params).subscribe((result: any) => {
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.NgxSpinnerService.hide()
            }
            else {
              this.ToastrService.success(result.message)
              this.reactiveFormotp.reset();
              window.location.reload();
              this.NgxSpinnerService.hide();
            }
          });
          
  }
  toggleOptions(daytracker_gid: string): void {
    if (this.showOptionsDivId === daytracker_gid) {
        this.showOptionsDivId = null;
    } else {
        this.showOptionsDivId = daytracker_gid;
    }
}
}

