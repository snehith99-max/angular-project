import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-tsk-mst-completed',
  templateUrl: './tsk-mst-completed.component.html',
  styles: [`.badge-blue {
    color: #2cacbd;
    background-color: transparent;
   
    &.badge-outline-blue {
      color: r#2cacbd;
      border: 1px solid;
      background-color: transparent;
      border-radius: 5px;
      padding: 5px;
      margin-right: 50px;
      padding-top: 2px;
      padding-bottom: 2px;
      // margin: 25px;
    }
  } 
  .cardHeadtime {

    position: relative;
  
    display: flex;
  
    flex-direction: column;
  
    min-width: 0;
  
    word-wrap: break-word;
  
    padding: 18px;
  
    background-clip: border-box;
  
    border: 0px solid rgba(0, 0, 0, 0);
  
    border-radius: .25rem;
  
    margin-bottom: 1.0rem;
  
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
  
    transition: all .3s ease-in-out;
  
    background-color: white;
  
  }`
  ]
})
export class TskMstCompletedComponent {
  nice_to_count: any;
  mandatory: any;
  completed_list: any;
  non_mandatory: any;
  show_stopper: any;
  completed: any;
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  toDate: any;
  timerInterval: any;
  constructor(private ToastrService:ToastrService,private Router:Router,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService) {
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  ngOnInit() {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
    this.rxTime = new Date();
    this.timerInterval = setInterval(() => {
        this.rxTime = new Date();
    }, 1000);
    var url = 'TskTrnTaskManagement/completed';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#completed').DataTable().destroy();
        this.completed_list = result.taskpending_list;
        this.completed=result.completed
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#completed').DataTable();
        }, 1);
      }
      else {
        this.completed_list = result.taskpending_list;
        this.completed=result.completed
        setTimeout(() => {
          var table = $('#completed').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#completed').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Mandatorysummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.mandatory=result.mandatory
    });
    var url = 'TskTrnTaskManagement/showstoppersummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.show_stopper=result.show_stopper
    });
    var url = 'TskTrnTaskManagement/nonmandatorytsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.non_mandatory=result.non_mandatory
    });
    var url = 'TskTrnTaskManagement/nicetohavesummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.nice_to_count=result.nice_to_count
    });
  }
  view(params:any){debugger
    const parameter1 = `${params}&Pending`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/ItsMstTaskStatusView?hash=' + encodeURIComponent (encryptedParam);
    this.Router.navigateByUrl(url)
    
  }
}
