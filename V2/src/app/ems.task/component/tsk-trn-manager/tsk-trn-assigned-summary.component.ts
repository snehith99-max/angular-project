import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tsk-trn-assigned-summary',
  templateUrl: './tsk-trn-assigned-summary.component.html',
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
}.ico-card {
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
export class TskTrnAssignedSummaryComponent {
  assignteamform!:FormGroup
  taskassigned_list: any;
  assigned_count: any;
  pending_count: any;
  completed_count: any;
  task_title: any;
  task_code: any;
  team_gid: any;
  holdform: FormGroup | any;
  assgn_manager: any;
  assigned_member: any;
  module_name: any;
  client: any;
  txthold_reason:any
  member: any;
  task_gid: any;
  team:any
  holdmanager_count: any;
  show_stopper: any;
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  tasks_gid: any;
  task_name: any;
  testmanager_count: any;
  total: any;
  assigned_member_gid: any;
  team_count: any;
  constructor(private Router:Router,public FormBuilder: FormBuilder,private ToastrService:ToastrService,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
    this.assignform()
this.createstsform()
  }
  createstsform() {
    this.holdform = this.FormBuilder.group({
      txholdtremarks: new FormControl(null,
        [    Validators.required,
          Validators.pattern(/^(?!\s*$).+/)
        ]),
    });
  }
  assignform(){
    this.assignteamform=this.FormBuilder.group({
      team_name: ['' ,Validators.required],
    })
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }

  ngOnInit() {
    var url = 'TskTrnTaskManagement/Assignedpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#assigned').DataTable().destroy();
        this.taskassigned_list = result.taskpending_list;
        this.assigned_count=result.assigned_count
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#assigned').DataTable();
        }, 1);
      }
      else {
        this.taskassigned_list = result.taskpending_list;
        this.assigned_count=result.assigned_count
        setTimeout(() => {
          var table = $('#assigned').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#assigned').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
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
    var url = 'TskTrnTaskManagement/Assignedholdsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.holdmanager_count=result.holdmanager_count
    })
    var url = 'TskTrnTaskManagement/Assignedtestingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.testmanager_count=result.testmanager_count
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
  transfer(data:any){debugger
    this.assignteamform.reset()
  
  this.task_title = data.task_name
  this.task_code = data.task_code
  this.team_gid = data.module_gid;
  this.task_gid = data.task_gid;
  this.assgn_manager =data.assignmanager_name
  this.assigned_member =data.assigned_member
  this.assigned_member_gid =data.assigned_member_gid
  this.module_name=data.module_name
  this.client=data.client_name
  var param = {
    team_gid: this.team_gid,
    employee_gid:this.assigned_member_gid
  }
  var url = 'TskMstCustomer/transferlist';
  this.SocketService.getparams(url, param).subscribe((result: any) => {
    this.member=result.assignmem_list
    this.NgxSpinnerService.hide();
  });
  }
  submitassign(){debugger
    var params={
      task_gid:this.task_gid,
      team_gid: this.team_gid,
      assigned_member_gid:this.assignteamform.value.team_name.employee_gid,
      assigned_member:this.assignteamform.value.team_name.employee_name,
       }
    this.NgxSpinnerService.show();
      var url = 'TskTrnTaskManagement/reassign';
      this.SocketService.post(url, params).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
  
        if (result.status == true) {
          this.ToastrService.success(result.message);
          window.scrollTo({
            top: 0,
          });
          this.ngOnInit()
        } else {
          this.ToastrService.warning(result.message);
          this.ngOnInit()
  
        }
        this.ngOnInit();
      });
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
  }
  holdtask(task_gid:any,task_code:any,task_name:any){
    this.tasks_gid=task_gid
    this.task_code=task_code
    this.task_name=task_name
    this.holdform.reset();

  }
  Submithold(){
    this.NgxSpinnerService.show();
    var params = {
      task_gid: this.tasks_gid,
      taskhold_reason: this.txthold_reason,
      status:'Hold'

  }
  var url = "TskTrnTaskManagement/GetHoldtask";
  this.SocketService.getparams(url,params).subscribe((result:any)=>{
    if(result.status= true){
      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
      window.scrollTo({
        top: 0,
      });
      this.ngOnInit()
    }
    else{
      this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message);
    } 
    });
   }
}
