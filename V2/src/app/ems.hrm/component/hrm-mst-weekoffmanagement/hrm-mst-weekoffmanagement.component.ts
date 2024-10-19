import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { AES } from 'crypto-js';

export class IWeekoff {
  employee_gid: any;
}

@Component({
  selector: 'app-hrm-mst-weekoffmanagement',
  templateUrl: './hrm-mst-weekoffmanagement.component.html',
  styleUrls: ['./hrm-mst-weekoffmanagement.component.scss']
})

export class HrmMstWeekoffmanagementComponent {
  // showOptionsDivId: any;
  WeekOffform: FormGroup | any;
  selection = new SelectionModel<IWeekoff>(true, []);
  weekoff_list: any[] = [];
  branch_list: any[] = [];
  department_list: any[] = [];
  weekoff_list1: any[] = [];
  responsedata: any;
  branch_name: any;
  department_name: any;

  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute)
    {
  }

  ngOnInit(): void {
    this.weekoffsummary()

    var url = 'WeekOff/GetBranchdropdownlist';
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.Getbranchdropdownlists;
    });
    
    var url = 'WeekOff/Getdepartmentdropdownlist';
    this.service.get(url).subscribe((result: any) => {
      this.department_list = result.Getdepartmentdropdownlists;
    });

    this.WeekOffform = new FormGroup({
      branch_name: new FormControl(''),
      department_name: new FormControl(''),
      branch_gid: new FormControl(''),
      department_gid: new FormControl(''),
    });
  }

  weekoffsummary() {
    var api = 'WeekOff/GetWeekOffSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#weekoff_list').DataTable().destroy();
      this.responsedata = result;
      this.weekoff_list = this.responsedata.WeekOffLists;
      setTimeout(() => {
        $('#weekoff_list').DataTable();
      },);
    });
  }

  weekoffsummarysearch() {
    var api = 'WeekOff/GetWeekOffSummarySearch';
    this.service.getparams(api, this.WeekOffform.value).subscribe((result: any) => {
      this.weekoff_list = result.WeekOffLists;
      setTimeout(() => {
        $('#weekoff_list').DataTable();
      },);
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.weekoff_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.weekoff_list.forEach((row: IWeekoff) => this.selection.select(row));
  }

  weekoff(employee_gid: any, employee_name: any) {
    const secretkey = 'storyboarderp';
      employee_gid = employee_gid,
      employee_name = employee_name

    const encryptedParam = AES.encrypt(employee_gid, secretkey).toString();
    const encryptedParam1 = AES.encrypt(employee_name, secretkey).toString();
    this.route.navigate(['/hrm/HrmMstWeeklyoff', encryptedParam, encryptedParam1])
  }

  weekoffsubmit() {
    const selectedData = this.selection.selected;
    if (selectedData.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to weekoff");
      return;
    }
    
    for (const data of selectedData) {
      this.weekoff_list1.push(data.employee_gid);
    }

    const secretkey = 'storyboarderp';
    this.weekoff_list1 = this.weekoff_list1
    const jsonString = JSON.stringify(this.weekoff_list1);
    const encryptedParam = AES.encrypt(jsonString, secretkey).toString();
    this.route.navigate(['/hrm/HrmMstWeeklyoffemployees', encryptedParam])
  }

  openModalview(employee_gid: any) {
    const secretkey = 'storyboarderp';
    employee_gid = employee_gid;
    const encryptedParam = AES.encrypt(employee_gid, secretkey).toString();
    this.route.navigate(['/hrm/Weekoffview', encryptedParam])
  }
}
