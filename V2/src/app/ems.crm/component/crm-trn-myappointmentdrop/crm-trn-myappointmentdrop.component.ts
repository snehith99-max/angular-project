import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-crm-trn-myappointmentdrop',
  templateUrl: './crm-trn-myappointmentdrop.component.html',
  styleUrls: ['./crm-trn-myappointmentdrop.component.scss']
})
export class CrmTrnMyappointmentdropComponent {
  responsedata: any;
  getmyappointmenttilescount_lists: any;
  total_count: any;
  assigned_count: any;
  unassigned_count: any;
  completed_count: any;
  today_count: any;
  upcoming_count: any;
  expired_count: any;
  leadbank_name: any;
  leadbank_gid: any;
  gettotalappointmentsummary_lists: any[] = [];
  getmyappointmentlog_lists: any[] = [];
  New_count: any;
  prospect_count: any;
  potentials_count: any;
  drop_count: any;
  closed_count: any;
  showOptionsDivId: any;
  constructor( public service: SocketService,private NgxSpinnerService: NgxSpinnerService,
    private route: Router){

  }
  ngOnInit(){
    this.GetMyAppointmentTilesCount();
    this.GetAppointmentSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetMyAppointmentTilesCount() {
    var url = 'MyAppointment/GetMyAppointmentTilesCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.getmyappointmenttilescount_lists = this.responsedata.getmyappointmenttilescount_lists;
      this.total_count = this.getmyappointmenttilescount_lists[0].total_count;
      this.assigned_count = this.getmyappointmenttilescount_lists[0].assigned_count;
      this.unassigned_count = this.getmyappointmenttilescount_lists[0].unassigned_count;
      this.completed_count = this.getmyappointmenttilescount_lists[0].completed_count;
      this.upcoming_count = this.getmyappointmenttilescount_lists[0].upcoming_count;
      this.expired_count = this.getmyappointmenttilescount_lists[0].expired_count;
      this.today_count = this.getmyappointmenttilescount_lists[0].today_count;
      this.New_count = this.getmyappointmenttilescount_lists[0].New_count;
      this.prospect_count = this.getmyappointmenttilescount_lists[0].prospect_count;
      this.potentials_count = this.getmyappointmenttilescount_lists[0].potentials_count;
      this.drop_count = this.getmyappointmenttilescount_lists[0].drop_count;
      this.closed_count = this.getmyappointmenttilescount_lists[0].closed_count;
      

    });
  }
  GetAppointmentSummary(){
    this.NgxSpinnerService.show();
    var url = 'MyAppointment/GetdropappointmentSummary'
     this.service.get(url).subscribe((result: any) => {
       $('#gettotalappointmentsummary_lists').DataTable().destroy();
       this.responsedata = result;
       this.gettotalappointmentsummary_lists = this.responsedata.gettotalappointmentsummary_lists;
       setTimeout(() => {
         $('#gettotalappointmentsummary_lists').DataTable();
       }, 1);
       this.NgxSpinnerService.hide();
   
   
     });
  }
  Onopen(param1: any,param2:any){
    const secretKey = 'storyboarderp';
    const lspage1 = "My-Appointment-Drop";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const appointment_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, appointment_gid, lspage]);
  }
  Onlogmodal(param1: any,param2: any){
  
    let param = {
      leadbank_gid: param1
    }
      this.leadbank_name=param2
      var url='MyAppointment/GetMyAppointmentlogsummary'
      this.service.getparams(url,param).subscribe((result: any) =>{
        this.responsedata = result;
        this.getmyappointmentlog_lists = this.responsedata.getmyappointmentlog_lists;
      });
  }
  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
}
