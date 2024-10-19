import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tsk-trn-live-summary',
  templateUrl: './tsk-trn-live-summary.component.html',
  styles: [`.border-pinkish {
    border-left: 5px solid #d4559b !important; 
  }`
  ]
})
export class TskTrnLiveSummaryComponent {
  live_list: any;
  total: any;
  constructor(private Router:Router,private ToastrService:ToastrService,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){

  }
  pending_count: any;
  complete_count: any;
  hold_count: any;
  progress_count: any;
  live_count: any;
  testing_count: any;
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  ngOnInit(){
    var url = 'TskTrnTaskManagement/Memberlivesummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#live').DataTable().destroy();
        this.live_list = result.taskpending_list;
        this.live_count=result.live_count
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#live').DataTable();
        }, 1);
      }
      else {
        this.live_list = result.taskpending_list;
        this.live_count=result.live_count
        setTimeout(() => {
          var table = $('#live').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#live').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Memberholdsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.hold_count=result.hold_count
    });
    var url = 'TskTrnTaskManagement/Membertestsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.testing_count=result.testing_count
    });
    var url = 'TskTrnTaskManagement/Memberprogresssummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.progress_count=result.progress_count
    });
    var url = 'TskTrnTaskManagement/Memberpendingsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.pending_count=result.pending_count
    });
    var url = 'TskTrnTaskManagement/count';
    this.SocketService.get(url).subscribe((result: any) => {
      this.total = result.row_count
    });
  }

  view(task_gid:any){debugger
    const parameter1 = `${task_gid}`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/ItsTrnMemberView?hash=' + encodeURIComponent (encryptedParam);
    this.Router.navigateByUrl(url)
  }
}
