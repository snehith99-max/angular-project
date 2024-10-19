import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-tsk-trn-manager-view',
  templateUrl: './tsk-trn-manager-view.component.html',
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
}.card {
  margin: 0;
  border: none;
  border-radius: 0;
  color: rgba(0, 0, 0, 1);
  letter-spacing: 0.05rem;
  // font-family: "Oswald", sans-serif;
  .txt {
    // margin-left: -3rem;
    z-index: 1;
  }
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
  }
  .selected-row {
    background-color: #e7f4ff;
  }
  .selectable-name {
    cursor: pointer;
  }
  .selectable-name:hover{
    background-color: #e7f4ff;
  
  }
  .clickable-label {
    color: blue; /* Text color */
    cursor: pointer; /* Change cursor to pointer */
    text-decoration: underline; /* Underline text to indicate a link */
  }
  
  .clickable-label:hover {
    color: darkblue; /* Change color on hover */
  }
  .selet{
    width: 163px !important;  }
  `
  ]
})
export class TskTrnManagerViewComponent {
  pending_count: any;
  assigned_count: any;
  completed_count: any;
  holdmanager_count: any;
  testmanager_count: any;
  mainList:any[]=[]
  max_team_count: number = 100; // Example maximum team count
  searchText = ''
  total: any;
  teamscount=false
  teammembers=true
  total_team: any;
  total_manager_count: any;
  total_member_count: any;
  teamname: any;
  taskpending_list:any
  show_stopper: any;
  subList:any[]=[]
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  team_list: any;
  team_count: any;
  task_total_count: any;
  memberlist: any;
  selectedData: any;
  employee_gid: any;
  show: boolean=false;
  task_details: any;
  membersummarylist: any;
  team_gid: any;
  count_member: any;
  member_count: any;
  manager_count: any;
  constructor(private Router:Router,public FormBuilder: FormBuilder,private ToastrService:ToastrService,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
  }
  ngOnInit() {
    var url = 'TskTrnTaskManagement/teamlistsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.team_list != null) {
        $('#manager').DataTable().destroy();
        this.team_list = result.team_list;
        this.total_member_count = result.assigned;
        this.task_total_count = result.task_total_count;
        this.team_count=result.team_count
        this.count_member = result.team_list[0].total_member_count
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#manager').DataTable();
        }, 1);
      }
      else {
        this.team_list = result.team_list;
        this.team_count=result.team_count
        this.task_total_count = result.task_total_count;
        this.total_member_count = result.assigned;
        this.count_member = result.team_list[0].total_member_count
        setTimeout(() => {
          var table = $('#manager').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#manager').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Managerpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
        this.pending_count=result.pending_count
    });
    var url = 'TskTrnTaskManagement/Assignedpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.assigned_count=result.assigned_count
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
    this.count()
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
  member(team_gid:any){
    this.team_gid=team_gid
    this.teamscount=true
    this.teammembers=false
    this.show=false
    var param = {
      team_gid: team_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/managerlist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.memberlist = result.assignmem_list
this.NgxSpinnerService.hide()
    });
  }
  onCancel() {
    this.teamscount = false; 
    this.teammembers=true
    this.show=false

    // Example action: hide the component
  }
  onRowClicked(data: any): void {debugger
    
    this.selectedData = data;
    this.employee_gid = data.employee_gid;
    this.show=true
    var params={
      employee_gid:this.employee_gid,
      module_gid:this.team_gid
    }
    this.NgxSpinnerService.show()
    var url = 'TskTrnTaskManagement/detailsview';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      setTimeout(() => {
        $('#membersummary').DataTable();
        window.scrollBy(0, 900); // Scroll after DataTable is initialized
      }, 1);
      if (result.taskpending_list != null) {
        $('#membersummary').DataTable().destroy();
        this.membersummarylist = result.taskpending_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#membersummary').DataTable();
        }, 1);
      }
      else {
        this.membersummarylist = result.taskpending_list;
        setTimeout(() => {
          var table = $('#membersummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#membersummary').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }   
      setTimeout(() => {
        $('#membersummary').DataTable();
        window.scrollBy(0, 900); // Scroll after DataTable is initialized
      }, 1);    });
  }
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.employee_name.toLowerCase().includes(searchString);
  }
}
