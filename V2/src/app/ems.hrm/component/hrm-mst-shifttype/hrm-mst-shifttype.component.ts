import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';


interface IShiftType {

}

@Component({
  selector: 'app-hrm-mst-shifttype',
  templateUrl: './hrm-mst-shifttype.component.html',
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})

export class HrmMstShifttypeComponent {
  // showOptionsDivId: any;
  ShiftTypeManagement!: IShiftType;
  responsedata: any;
  shift_list: any[] = [];
  Shifttime_list: any[] = [];
  parameterValue: any;
  data: any;



  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService,) {
    this.ShiftTypeManagement = {} as IShiftType;
  }

  ngOnInit(): void {
    var url = 'ShiftType/GetShiftSummary'

    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.shift_list = this.responsedata.shift_list;
      setTimeout(() => {
        $('#shift_list').DataTable();
      },);
    });
  }

  toggleStatus(data: any) {

    if (data.statuses === 'Y') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }

  Shifttypeadd() {
    this.router.navigate(['/hrm/HrmMstAddShiftType'])
  }

  ShiftTimepopup(data: any): void {
    debugger;
    var api1 = 'ShiftType/GetshiftTimepopup';

    let params = {
      shifttype_gid: data.shifttype_gid,
    };

    this.service.getparams(api1, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Shifttime_list = this.responsedata.Time_list;
    });
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'ShiftType/DeleteShift'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)

      }
      else {
        this.ToastrService.success(result.message)
      }
      this.ngOnInit();
    });
  }

  openModalactive(parameter: string) {
    this.parameterValue = parameter
  }

  onactive() {
    console.log(this.parameterValue);
    var url3 = 'ShiftType/GetshiftActive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)

      }
      else {
        this.ToastrService.success(result.message)
      }  
      this.ngOnInit();
    });
  }

  openModalinactive(parameter: string) {
    this.parameterValue = parameter
  }

  oninactive() {
    console.log(this.parameterValue);
    var url3 = 'ShiftType/GetshiftInActive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)

      }
      else {
        this.ToastrService.success(result.message)

      }
      this.ngOnInit();

    });
  }


  Assign(params: any) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/hrm/HrmMstShiftAssignment', encryptedParam])
  }

  UnAssign(params: any) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/hrm/HrmMstUnShiftAssignment', encryptedParam])
  }

  // Assign(shifttype_gid:any){
  //   //Edit the values in database 
  //   const url = `/hrm/HrmMstShiftAssignment?shifttype_gid=${shifttype_gid}`;
  //   this.router.navigateByUrl(url);
  // }

  edit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/hrm/HrmMstEditShiftType', encryptedParam])
  }
}