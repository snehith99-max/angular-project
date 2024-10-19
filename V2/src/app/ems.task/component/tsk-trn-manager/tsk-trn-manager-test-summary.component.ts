import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-tsk-trn-manager-test-summary',
  templateUrl: './tsk-trn-manager-test-summary.component.html',
  styles: [`.badge-brown {
    color: #d87040;
    background-color: transparent;
   
    &.badge-outline-brown {
      color: #d87040;
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
  .inline {
    display: inline-block;
    margin-right: 10px; /* Adjust as needed */
  }  
@keyframes slideColors  {
  0%, 100% { background-color: rgb(255, 0, 0); } /* Red */
  25% { background-color: rgb(0, 174, 255); } /* Blue */
  50% { background-color: rgb(187, 187, 21); } /* Yellow */
  75% { background-color: rgb(128, 0, 128); } /* Purple */
}
.animated-label {
border-radius: 24%;
color: white;
font-weight: bold; /* Adjust font weight as needed */
animation: slideColors 10s ease infinite;
}
.ico-card {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
}
.is {
  position: relative;
  right: -50%;
  top: 60%;
  font-size: 12rem;
  line-height: 0;
  opacity: 0.2;
  color: rgba(255, 255, 255, 1);
  z-index: 0;
}`
  ]
})
export class TskTrnManagerTestSummaryComponent {
  pending_count: any;
  assigned_count: any;
  completed_list: any;
  completed_count: any;
  testmanager_count: any;
  testing_list: any;
  show_stopper: any;
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  holdmanager_count: any;
  total: any;
  team_count: any;
  constructor(private Router:Router,public FormBuilder: FormBuilder,private ToastrService:ToastrService,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
  }
  ngOnInit() {
    var url = 'TskTrnTaskManagement/Assignedtestingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#testing').DataTable().destroy();
        this.testing_list = result.taskpending_list;
        this.testmanager_count=result.testmanager_count
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#testing').DataTable();
        }, 1);
      }
      else {
        this.testing_list = result.taskpending_list;
        this.testmanager_count=result.testmanager_count
        setTimeout(() => {
          var table = $('#testing').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#testing').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Assignedholdsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.holdmanager_count=result.holdmanager_count
    })
    var url = 'TskTrnTaskManagement/Assignedpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.assigned_count=result.assigned_count
    })
    var url = 'TskTrnTaskManagement/Managerpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.pending_count=result.pending_count
    })
    var url = 'TskTrnTaskManagement/Assignedcompletedsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.completed_count=result.completed_count
    })
    var url = 'TskTrnTaskManagement/managerallcount';
    this.SocketService.get(url).subscribe((result: any) => {
      this.total = result.row_count
    });
    var url = 'TskTrnTaskManagement/teamlistsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.team_count=result.team_count

    });
    this.count()

  }
  
view(params:any){debugger
  const parameter1 = `${params}&Pending`;
  const secretKey = 'storyboarderp';
  const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
    var url = '/ITS/ItsMstTaskStatusView?hash=' + encodeURIComponent (encryptedParam);
  this.Router.navigateByUrl(url)
  
}
count(){
  var url = 'TskTrnTaskManagement/managercount';
  this.SocketService.get(url).subscribe((result: any) => {
    this.show_stopper=result.show_stopper_count
    this.mandatory=result.mandatory_count
    this.non_mandatory=result.non_mandatory_count
    this.nice_to_count=result.nice_to_have_count
  });
  // var url = 'TskTrnTaskManagement/Mandatorysummary';
  // this.SocketService.get(url).subscribe((result: any) => {
  //   this.mandatory=result.mandatory
  // });
  // var url = 'TskTrnTaskManagement/nonmandatorytsummary';
  // this.SocketService.get(url).subscribe((result: any) => {
  //   this.non_mandatory=result.non_mandatory
  // });
  // var url = 'TskTrnTaskManagement/nicetohavesummary';
  // this.SocketService.get(url).subscribe((result: any) => {
  //   this.nice_to_count=result.nice_to_count
  // });
}

}