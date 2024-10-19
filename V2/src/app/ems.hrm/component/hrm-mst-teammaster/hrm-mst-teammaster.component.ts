import { Component, ElementRef, OnInit, } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
interface objInterface { employee_gid: string;} 
@Component({
  selector: 'app-hrm-mst-teammaster',
  templateUrl: './hrm-mst-teammaster.component.html',
})
export class HrmMstTeammasterComponent {
  addTeamFormData  = { 
    txtteam_name: '',
    dpteammanager: {
      employee_name:'',
      employee_gid:''
    },
    dpteammember: '',
  };
  editTeamFormData = {
    txteditteamname: '',
    cboediteammanager: {
      employee_name:'',
      employee_gid:''
    },
  
    cboeditteammembers: '',
   
  };
  statusTeamFormData = {
    txtteam_name: '',
    rbo_status: '',
    txtremarks: ''  
  };
  txtteam_name: any;
  dpteammanager: any;
  dpteammember:any;
  EditForm : FormGroup | any;
  TeamAddForm : FormGroup | any;
  TeamEditForm : FormGroup | any;
  TeamStatusForm : FormGroup | any;
  managerlists: any;
  memberslists: any;
  team_gid: any;
  TeamMaster_lit: any;
  member_name: any;
  membernames: any;
  txteditteamname: any;
  cboediteammanager: any;
  cboeditteammembers: any;
  Teaminactivelog_data: any;
  parametervalue: any;
  TeamMaster_list: any;
  total_employee_count: any;

  constructor(private SocketService: SocketService,private el: ElementRef,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,private FormBuilder: FormBuilder) {
    this.createForm(); 
    this.createEditForm();
    this.createStatusForm();
  }
  createForm(){
    this.TeamAddForm = this.FormBuilder.group({
      txtteam_name: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?!\s*$).+/)
        ]),
      dpteammanager: ['', Validators.required],
      dpteammember: ['' ,Validators.required],
    });
  }
  createEditForm(){
    this.TeamEditForm = this.FormBuilder.group({
      txteditteamname: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?!\s*$).+/)
        ]),
      cboediteammanager: ['',Validators.required],
      cboeditteammembers: ['',Validators.required],
     
    });
  }
  createStatusForm(){
    this.TeamStatusForm = this.FormBuilder.group({
      // txtremarks: ['', [Validators.required,Validators.pattern("^(?!\s*$).+")]],
      txtremarks: new FormControl(null,

        [

          Validators.required,

          Validators.pattern(/^(?!\s*$).+/)

        ]),
      rbo_status: [''] 
    });
  }
  clearForm() {
    this.TeamAddForm.reset(); 
  }

  ngOnInit() {

    var url = 'HrmMaster/GetTeammaster';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
    if(result.teamgroup != null){
      $('#Teamtable').DataTable().destroy();
      this.TeamMaster_list= result.teamgroup;  
      this.NgxSpinnerService.hide();
      setTimeout(()=>{   
        $('#Teamtable').DataTable();
      }, 1);
    }
    else{
      setTimeout(()=>{   
        $('#Teamtable').DataTable();
      }, 1);
      // this.TeamMaster_lit= result.teamgroup;
      this.NgxSpinnerService.hide();
      $('#Teamtable').DataTable().destroy();
    } 


  });
    var url = 'HrmMaster/Employee';
    this.SocketService.get(url).subscribe((result: any) => {
      this.managerlists = result.taskemployee_list;
    });

    var url = 'HrmMaster/TeamEmployee';
    this.SocketService.get(url).subscribe((result: any) => {
      this.memberslists = result.taskmemberemployee_list;
    });
  }
  openteampopup(){
    this.clearForm();
  }
  
    members( team_gid:any){

    var params = {
      team_gid: team_gid
  }
    var url = 'HrmMaster/Getteammastermembers';
    this.SocketService.getparams(url, params).subscribe((result: any) =>{
    this.membernames = result.member_name ;
                   
     });		
					
  }
  addTeam(){
    if (this.TeamAddForm.valid) {

      var params = {
        team_name:this.addTeamFormData.txtteam_name,
        teammanager_name:this.addTeamFormData.dpteammanager.employee_name,
        teammanager_gid:this.addTeamFormData.dpteammanager.employee_gid,
        teammembers:this.addTeamFormData.dpteammember,
      } 
      this.NgxSpinnerService.show();
      var url = 'HrmMaster/PostTeammaster';
      this.SocketService.post(url, params).subscribe((result:any) => {
        if(result.status == true){
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.ngOnInit();
        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
        }
      
      })
    }
  }

  editteam(team_gid:any){
    this.team_gid = team_gid;
   
    var param = {
      team_gid: team_gid
  }
  this.NgxSpinnerService.show();
  var url = 'HrmMaster/GetTeammembersEdit';
  this.SocketService.getparams(url,param).subscribe((result:any) => { 
      this.editTeamFormData.txteditteamname = result.team_name;
      this.editTeamFormData.cboediteammanager=  { employee_name: result.teammanager_name,
        employee_gid: result.teammanager_gid }
      this.editTeamFormData.cboeditteammembers = result.teammembersdtl;
      this.NgxSpinnerService.hide(); 
  });

  }
  

  UpdateTeam(){
 
    var params = {
      team_gid: this.team_gid,
      team_name:this.editTeamFormData.txteditteamname,
      teammanager_name: this.editTeamFormData.cboediteammanager.employee_name,
      teammanager_gid:this.editTeamFormData.cboediteammanager.employee_gid,
      teammembers:this.editTeamFormData.cboeditteammembers,
    } 
   
    this.NgxSpinnerService.show();
    var url = 'HrmMaster/UpdateTeamDtl';
    this.SocketService.post(url, params).subscribe((result:any) => {
      if(result.status == true){
        this.NgxSpinnerService.hide();
        this.ngOnInit();
        this.ToastrService.success(result.message);
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
     
    })
  }

  Status_Click(team_gid: any){
    this.NgxSpinnerService.show();
    this.team_gid = team_gid
    this.TeamStatusForm.reset();
    var params = {
      team_gid: this.team_gid
    }  
    var url = 'HrmMaster/GetTeammembersEdit';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.statusTeamFormData.txtteam_name = result.team_name;
      this.statusTeamFormData.rbo_status = result.Status; 
      this.NgxSpinnerService.hide(); 
    }) 
    this.NgxSpinnerService.show();
    var url = 'HrmMaster/TeamMasterInactiveLogview';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.Teaminactivelog_data = result.master_list; 
      this.NgxSpinnerService.hide(); 
    }) 
  
  }
  update_status(){
    var params = {
      team_gid: this.team_gid,
      team_name: this.statusTeamFormData.txtteam_name,
      remarks: this.statusTeamFormData.txtremarks,
      rbo_status: this.statusTeamFormData.rbo_status 
    } 
  this.NgxSpinnerService.show();
  var url = 'HrmMaster/InactiveTeamMaster';
  this.SocketService.post(url, params).subscribe((result: any) => {
    if (result.status == true) {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  }) 
  }
    delete(team_gid: any){
      this.parametervalue = team_gid 

    }
    ondelete(){
      this.NgxSpinnerService.show();
      var params = {
        team_gid:  this.parametervalue
      }   
      var url = 'HrmMaster/DeleteTeammaster';
      this.SocketService.getparams(url, params).subscribe((result: any) => {
        if (result.status == true) {
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.ngOnInit();
        }
        else {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();   
        }
      }) 
    }
 
  }





