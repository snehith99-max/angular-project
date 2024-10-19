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
  selector: 'app-otl-trn-toutletmanager',
  templateUrl: './otl-trn-toutletmanager.component.html',
  styleUrls: ['./otl-trn-toutletmanager.component.scss']
})
export class OtlTrnToutletmanagerComponent {

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
  outletCountList: any [] = [];
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
    this.GetRegionSummary();     
    var url  = 'OutletManage/GetOtlTrnOutletCount';
    this.service.get(url).subscribe((result:any) => {
    this.responsedata = result;
    this.outletCountList = this.responsedata.outletCountList;
    });
  }
  GetRegionSummary() {
    var url = 'OutletManager/GetdaymanagerSummary'
    this.service.get(url).subscribe((result: any) => {
      debugger
      $('#daytracker_list').DataTable().destroy();
      this.responsedata = result;
      this.daytracker_list = this.responsedata.daymanagersummary_list;
      this.daytracker_list.sort((a, b) => {
        return new Date(b.created_date).getTime() - new Date(a.created_date).getTime();
      });
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
  Viewedittracker(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/otltrnoutletmanagerview',encryptedParam])
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}

