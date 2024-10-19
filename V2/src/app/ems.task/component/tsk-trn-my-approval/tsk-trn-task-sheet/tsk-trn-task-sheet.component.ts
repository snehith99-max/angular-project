import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
@Component({
  selector: 'app-tsk-trn-task-sheet',
  templateUrl: './tsk-trn-task-sheet.component.html',
  styleUrls: ['./tsk-trn-task-sheet.component.scss'],
  providers: [DatePipe]
})
export class TskTrnTaskSheetComponent {
  txt_process: any
  myDate: any;
  txt_date: any
  Task_type = [
    { Tasktype_name: 'Analysis', Tasktype_gid: 'TTYG_001' },
    { Tasktype_name: 'Design', Tasktype_gid: 'TTYG_002' },
    { Tasktype_name: 'Development', Tasktype_gid: 'TTYG_003' },
    { Tasktype_name: 'Testing', Tasktype_gid: 'TTYG_004' },
    { Tasktype_name: 'Bug Solving', Tasktype_gid: 'TTYG_005' },
    { Tasktype_name: 'Support', Tasktype_gid: 'TTYG_006' },
    { Tasktype_name: 'Meeting', Tasktype_gid: 'TTYG_007' },
    { Tasktype_name: 'Knowledge Transfer', Tasktype_gid: 'TTYG_008' },
    { Tasktype_name: 'Deployment', Tasktype_gid: 'TTYG_009' },


  ];
  status = [
    { status_name: 'Open', status_gid: 'STUN_001' },
    { status_name: 'Close', status_gid: 'STUN_002' },
  ];
  hide: boolean = false;
  Show: boolean = false;
  Team_list: any;
  taskdetail_list: any;
  history: boolean = false
  current: boolean = true
  tasksheet_list: any;
  selectedChartType: any;
  taskhistorysheet_list: any;
  sub: boolean = false;
  module_name: any;
  task_type: any;
  hrs: any;
  Status: any;
  updateshow: boolean = false;
  addshoww: boolean = true;
  task_nameshow: any;
  sub_task: any;
  taskname: any;
  tasksheet_gid: any;
  constructor(private datePipe: DatePipe, public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService,
    private Location: Location, public FormBuilder: FormBuilder
  ) {
    this.createform()
    var date = new Date();
    this.txt_date = this.datePipe.transform(date, 'dd-MM-yyyy');
  }
  AddForm!: FormGroup | any;
  showInput: string = '';
  hold_list: any
  createform() {
    this.AddForm = this.FormBuilder.group({
      text_module: [null, Validators.required],
      txthrs_taken: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      text_status: [null, Validators.required],
      txtsub_task: [''],
      txt_process: ['', [Validators.required]],
      txttask_type: [null, Validators.required],
      txt_date: [''],
      txttask: [null, [Validators.required]],
      text_other: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],

    })
  }
  onInputChange() {
    if (this.showInput === 'Task') {
      const control = this.AddForm.get('text_other');
      const sample = this.AddForm.get('txttask');
      this.sub = true

      if (control && sample) {
        control.markAsUntouched();
        control.clearValidators();
        control.setValue(null);

        sample.setValidators([Validators.required]);
        control.updateValueAndValidity();
        sample.updateValueAndValidity();
      }

    } else if (this.showInput === 'Other') {
      const control = this.AddForm.get('txttask');
      const sample = this.AddForm.get('text_other');
      this.sub = false
      if (control && sample) {
        control.markAsUntouched();
        control.clearValidators();
        control.setValue(null);
        sample.setValidators([Validators.required]);
        control.updateValueAndValidity();
        sample.updateValueAndValidity();
      }
    }

    this.AddForm.updateValueAndValidity();
  }
  backbutton() {
    this.Location.back()
  }
  edit(data: any) {
    this.NgxSpinnerService.show()
    window.scrollTo({
      top: 0,
    });
    this.updateshow = true
    this.addshoww = false
    this.tasksheet_gid=data.tasksheet_gid
    this.module_name = {team_name:data.module_name,team_gid:data.module_gid,teamname_gid:data.teamname_gid}
    this.sub_task=data.sub_task
    this.task_type = { Tasktype_name: data.task_typename, Tasktype_gid: data.task_typegid }
    this.hrs = data.hrs_taken
    this.Status = {status_name:data.status}
    this.showInput = data.task_detail
    if(data.task_gid ==""){
      this.taskname=data.task_name
     this.sub=false
    }
    else{
      this.task_nameshow={task_name:data.task_name,task_gid:data.task_gid}
      this.sub=true

    }
    // this.taskname=data.task_name
    setTimeout(() => {
      this.NgxSpinnerService.hide()
    }, 500);
  }
  close() {
    this.updateshow = false
    this.addshoww = true
    this.sub=false
    this.AddForm.reset()
    const date = new Date();
    this.txt_date = this.datePipe.transform(date, 'dd-MM-yyyy');
    this.AddForm.patchValue({
      txt_date: this.txt_date
    });
  }
  ngOnInit() {
    var url = 'TskMstCustomer/TeamSummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.Team_list = result.team_list;

    });
    var url = 'TskTrnTaskManagement/task_list';
    this.SocketService.get(url).subscribe((result: any) => {
      this.taskdetail_list = result.taskdetail_list;

    });
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/tasksheetsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.tasksheet_list != null) {
        $('#sheetsummary').DataTable().destroy();
        this.tasksheet_list = result.tasksheet_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#sheetsummary').DataTable();
        }, 1);
      }
      else {
        this.tasksheet_list = result.tasksheet_list;
        setTimeout(() => {
          var table = $('#sheetsummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#sheetsummary').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    const options: Options = {
      dateFormat: 'd-m-Y',
      maxDate: 'today',

    };
    flatpickr('.date-picker', options);
  }
  submit() {
    
    let task_name = '';
    let task_gid = '';
    if (this.showInput === 'Task') {
      task_name = this.AddForm.value.txttask.task_name;
      task_gid = this.AddForm.value.txttask.task_gid;
    } else if (this.showInput === 'Other') {
      task_name = this.AddForm.value.text_other;
      task_gid = ''
    }
    var params = {
      module_gid: this.AddForm.value.text_module.team_gid,
      module_name: this.AddForm.value.text_module.team_name,
      module_name_gid: this.AddForm.value.text_module.teamname_gid,
      task_name: task_name,
      task_gid: task_gid,
      task_detail: this.showInput,
      task_date: this.txt_date,
      sub_task: (this.AddForm.value.txtsub_task == undefined) ? '' : this.AddForm.value.txtsub_task,
      task_typename: this.AddForm.value.txttask_type.Tasktype_name,
      task_typegid: this.AddForm.value.txttask_type.Tasktype_gid,
      hrs_taken: this.AddForm.value.txthrs_taken,
      sheetstatus: this.AddForm.value.text_status.status_name,
    };
    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/Tasksheetadd';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        this.NgxSpinnerService.hide()
        this.ToastrService.success("Task Sheet Added successfully");
        this.AddForm.reset()
        const date = new Date();
        this.txt_date = this.datePipe.transform(date, 'dd-MM-yyyy');
        this.AddForm.patchValue({
          txt_date: this.txt_date
        });
        this.sheet()
        this.sub=false
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.sheet()

      }
    });
  }
  update() {
    
    let task_name = '';
    let task_gid = '';
    let sub_task='';
    if (this.showInput === 'Task') {
      task_name = this.AddForm.value.txttask.task_name;
      task_gid = this.AddForm.value.txttask.task_gid;
      sub_task =this.AddForm.value.txtsub_task;
    } else if (this.showInput === 'Other') {
      task_name = this.AddForm.value.text_other;
      task_gid = ''
      sub_task=''

    }
    var params = {
      tasksheet_gid:this.tasksheet_gid,
      module_gid: this.AddForm.value.text_module.team_gid,
      module_name: this.AddForm.value.text_module.team_name,
      module_name_gid: this.AddForm.value.text_module.teamname_gid,
      task_name: task_name,
      task_gid: task_gid,
      task_detail: this.showInput,
      task_date: this.txt_date,
      sub_task: sub_task,
      task_typename: this.AddForm.value.txttask_type.Tasktype_name,
      task_typegid: this.AddForm.value.txttask_type.Tasktype_gid,
      hrs_taken: this.AddForm.value.txthrs_taken,
      sheetstatus: this.AddForm.value.text_status.status_name,
    };
    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/Tasksheetupdate';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        this.NgxSpinnerService.hide()
        this.ToastrService.success("Task Sheet Updated successfully");
        this.AddForm.reset()
        const date = new Date();
        this.txt_date = this.datePipe.transform(date, 'dd-MM-yyyy');
        this.AddForm.patchValue({
          txt_date: this.txt_date
        });
        this.sheet()
        this.sub=false
        this.updateshow=false
        this.addshoww=true
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.sheet()
        this.sub=false
        this.updateshow=false
        this.addshoww=true
      }
    });
  }
  sheet() {
    this.selectedChartType = 'Current';

    this.history = false
    this.current = true
    this.ngOnInit()
  }
  historysheet() {
    this.selectedChartType = 'History';

    this.history = true
    this.current = false
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/tasksheethistorysummary';
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.tasksheet_list != null) {
        $('#historysheetsummary').DataTable().destroy();
        this.taskhistorysheet_list = result.tasksheet_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#historysheetsummary').DataTable();
        }, 1);
      }
      else {
        this.taskhistorysheet_list = result.tasksheet_list;
        setTimeout(() => {
          var table = $('#historysheetsummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#historysheetsummary').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
  }
  subtask() {
    this.sub = true
  }
}
