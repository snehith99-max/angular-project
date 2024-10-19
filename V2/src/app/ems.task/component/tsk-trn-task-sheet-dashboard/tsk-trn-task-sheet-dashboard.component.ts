import { Component } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-tsk-trn-task-sheet-dashboard',
  templateUrl: './tsk-trn-task-sheet-dashboard.component.html',
  styleUrls: ['./tsk-trn-task-sheet-dashboard.component.scss']
})
export class TskTrnTaskSheetDashboardComponent {
  searchText =''
  taskshow: boolean =true;
  teamshow: boolean =false;
  deployment: boolean=false
  task_wise: any;
  deploy_list: any;
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  show_stopper: any;
  subList:any[]=[]
  mainList: any[] = [];
  txt_date: any;
  tasksheet: boolean =false;
  Team_list: any;
  txtm_odule: any;
  team_list: any;
  membershow: boolean=false;
  member_list: any;
  txtassigned_member:any
  allmember_list: any;
  member: any;
  constructor(private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
    this.populateYears();
    this.filteredList = this.mainList;
  }
  selectedYear!: number;   
  years: number[] = []; 
  currentDayName: any;
  toDate:any
  time =new Date()
  subfolders:any[]=[]
  filteredList: any[] = [];

  private populateYears(): void {
    const currentYear = new Date().getFullYear();
    const startYear = 1900; // Change this as per your requirement

    for (let year = currentYear; year >= startYear; year--) {
        this.years.push(year);
    }

    // Set default selected year to the current year
    this.selectedYear = currentYear;
}
matchesSearch(item: any): boolean {
  const searchString = this.searchText.toLowerCase();
  return item.created_by.toLowerCase().includes(searchString);
}
filterList() {debugger
  this.filteredList = this.mainList.filter(item => this.matchesSearch(item));
}
ngOnInit(){
  this.filterList();

  var url = 'TskTrnTaskManagement/taskwisesummary';
  this.NgxSpinnerService.show();
  this.SocketService.get(url).subscribe((result: any) => {
    if (result.taskpending_list != null) {
      $('#tasksummary').DataTable().destroy();
      this.task_wise = result.taskpending_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#tasksummary').DataTable();
      }, 1);
    }
    else {
      this.task_wise = result.taskpending_list;
      setTimeout(() => {
        var table = $('#tasksummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      $('#tasksummary').DataTable().destroy();
      this.NgxSpinnerService.hide();
    }
  });
  this.count()
  this.txtm_odule=null
this.team_list=[]
$('#teamsummary').DataTable().destroy();
this.txtassigned_member=null
this.allmember_list=[]
$('#membersummary').DataTable().destroy();
  const options: Options = {
    dateFormat: 'd-m-Y',

  };
  flatpickr('.date-picker', options);

}
task(){
 this.taskshow = true
 this.teamshow = false
 this.deployment = false
 this.tasksheet=false
 this.membershow=false

 this.ngOnInit()
}
summary(){
  var params={
    module_gid:this.txtm_odule.team_gid
  }
  var url = 'TskTrnTaskManagement/teamwisesummary';
  this.NgxSpinnerService.show();
  this.SocketService.getparams(url,params).subscribe((result: any) => {
    if (result.taskpending_list != null) {
      $('#teamsummary').DataTable().destroy();
      this.team_list = result.taskpending_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#teamsummary').DataTable();
      }, 1);
    }
    else {
      this.team_list = result.taskpending_list;
      setTimeout(() => {
        var table = $('#teamsummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      $('#teamsummary').DataTable().destroy();
      this.NgxSpinnerService.hide();
    }
  });
}
teamtask(){
  this.txt_date=null
  this.txtassigned_member=null
  this.allmember_list=[]
  $('#membersummary').DataTable().destroy();
  this.NgxSpinnerService.show()
  var url = 'TskMstCustomer/TeamSummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.Team_list = result.team_list;
    this.NgxSpinnerService.hide()
  });
  setTimeout(() => {
    var table = $('#teamsummary').DataTable();
  }, 1);
  this.taskshow = false
  this.teamshow = true
  this.tasksheet=false
  this.membershow=false

  this.deployment = false
this.summary()
  this.count()

 
}
Deploymenttask(){
  this.taskshow = false
  this.teamshow = false
  this.deployment = true
   this.tasksheet=false
   this.txt_date=null
   this.membershow=false

  var url = 'TskTrnTaskManagement/deploywisesummary';
  this.NgxSpinnerService.show();
  this.SocketService.get(url).subscribe((result: any) => {
    if (result.taskpending_list != null) {
      $('#Deploymentsummary').DataTable().destroy();
      this.deploy_list = result.taskpending_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Deploymentsummary').DataTable();
      }, 1);
    }
    else {
      this.deploy_list = result.taskpending_list;
      setTimeout(() => {
        var table = $('#Deploymentsummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      $('#Deploymentsummary').DataTable().destroy();
      this.NgxSpinnerService.hide();
    }
  });
  this.txtm_odule=null
  this.team_list=[]
  $('#teamsummary').DataTable().destroy();
  this.txtassigned_member=null
  this.allmember_list=[]
  $('#membersummary').DataTable().destroy();
  this.count()
}
count(){
    var url = 'TskTrnTaskManagement/Mandatorysummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.mandatory=result.mandatory
  });
  var url = 'TskTrnTaskManagement/nonmandatorytsummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.non_mandatory=result.non_mandatory
  });
  var url = 'TskTrnTaskManagement/nicetohavesummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.nice_to_count=result.nice_to_count
  });
  var url = 'TskTrnTaskManagement/showstoppersummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.show_stopper=result.show_stopper
  });
}
toggleVisibility(item: any) {debugger
  item.visible = !item.visible;
}
clear(){
  this.txtm_odule=null
  this.team_list=[]
  $('#teamsummary').DataTable().destroy();
  setTimeout(() => {
    var table = $('#teamsummary').DataTable();
  }, 1);
  this.txtassigned_member=null
  this.allmember_list=[]
  $('#membersummary').DataTable().destroy();
  setTimeout(() => {
    var table = $('#membersummary').DataTable();
  }, 1);
}
datewise(txt_date: any) {
  this.txtm_odule=null
  this.team_list=[]
  $('#teamsummary').DataTable().destroy();
  this.txtassigned_member=null
  this.allmember_list=[]
  $('#membersummary').DataTable().destroy();
  debugger;
  this.tasksheet = true;
  this.teamshow = false;
  this.deployment = false;
  this.taskshow = false;
  this.membershow=false

  const params = {
    task_date: txt_date
  };
  this.NgxSpinnerService.show()
  const url = 'TskTrnTaskManagement/gettask';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    if (result && result.mdlheir1 && result.subfolders) {
      this.member =result.member;
      this.mainList = result.mdlheir1.map((item: any) => ({ ...item, visible: false, subfolders: [] }));
      this.subList = result.subfolders.map((item: any) => ({ ...item, visible: false, subfolders: [] }));
      this.filterList();
    } else {
      this.member =result.member;
      this.mainList = [];
      this.subList = [];
    }
    // this.mainList = result.mdlheir1.map((item: any) => ({ ...item, visible: false, subfolders: [] }));
    // this.subList = result.subfolders.map((item: any) => ({ ...item, visible: false, subfolders: [] }));
    this.NgxSpinnerService.hide()
    this.addItemsFromTargetList();
  });
}

addItemsFromTargetList() {
  debugger;
  this.subList.forEach((targetItem: any) => {
    this.recursivelyAddItems(targetItem, this.mainList);
  });

}

recursivelyAddItems(targetItem: any, sourceList: any[]) {
  debugger;
  // const targetId = targetItem.created_by || targetItem.created_by; 
  // const matchingIndex = sourceList.findIndex(sourceItem => 
  //   (sourceItem.task_gid === targetId) || (sourceItem.created_by === targetId)
  // );
   const matchingIndex = sourceList.findIndex(sourceItem => sourceItem.task_gid === targetItem.task_gid);
  if (matchingIndex !== -1) {
    if (!sourceList[matchingIndex].subfolders) {
      sourceList[matchingIndex].subfolders = [];
    }
    console.log(`Adding to subfolders of task_gid: ${sourceList[matchingIndex].task_gid}`);
    sourceList[matchingIndex].subfolders.push({ ...targetItem, visible: false });
  } else {
    sourceList.forEach(sourceItem => {
      if (sourceItem.subfolders && sourceItem.subfolders.length > 0) {
        this.recursivelyAddItems(targetItem, sourceItem.subfolders);
      }
    });
  }
}
membertask(){
  this.txtm_odule=null
  this.team_list=[]
  $('#teamsummary').DataTable().destroy();
  this.txt_date=null

  this.NgxSpinnerService.show()
  var url = 'TskTrnTaskManagement/allmembers';
  this.SocketService.get(url).subscribe((result: any) => {
    this.member_list = result.memberdropdown_list;
    this.NgxSpinnerService.hide()
  });
  this.tasksheet = false;
  this.teamshow = false;
  this.deployment = false;
  this.taskshow = false;
  this.membershow=true
  setTimeout(() => {
    var table = $('#membersummary').DataTable();
  }, 1);
  this.allview()
  this.count()

}
allview(){
  var params={
    assigned_member_gid:this.txtassigned_member.assigned_member_gid,
    // task_gid:this.txtassigned_member.task_gid
  }
  var url = 'TskTrnTaskManagement/allmemebrview';
  this.NgxSpinnerService.show();
  this.SocketService.getparams(url,params).subscribe((result: any) => {
    if (result.allmember_list != null) {
      $('#membersummary').DataTable().destroy();
      this.allmember_list = result.allmember_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#membersummary').DataTable();
      }, 1);
    }
    else {
      this.allmember_list = result.allmember_list;
      setTimeout(() => {
        var table = $('#membersummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      $('#membersummary').DataTable().destroy();
      this.NgxSpinnerService.hide();
    }
  });
}
}

