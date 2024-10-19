import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';


@Component({
  selector: 'app-hrm-trn-leavebalancesummary',
  templateUrl: './hrm-trn-leavebalancesummary.component.html',
})
export class HrmTrnLeavebalancesummaryComponent {
  // showOptionsDivId: any;
  leavebalance : any;
  response_data : any;
  dataTable:any;
  leavetype_gid: any;



  constructor(private fb: FormBuilder,private excelService : ExcelService,private route: ActivatedRoute,private router: Router,private service: SocketService,) {} 

  ngOnInit(): void {
    var api = 'Leaveopening/GetLeaveopening';
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.leavebalance = this.response_data.leaveopening_list;
      // setTimeout(()=>{  
      //   $('#leavebalance').DataTable();
      // }, 1);
      this.dataTable={
        columns: [],
        rows: []
      };
      try {
        const datatablejson = JSON.parse(result.datatablejson);
        if (Array.isArray(datatablejson) && datatablejson.length > 0) {
            // Get column names from the first row
            this.dataTable.columns = Object.keys(datatablejson[0]);
            // Get values for each row
            this.dataTable.rows = datatablejson.map(row => Object.values(row));
        } else {
            console.log('Parsed datatablejson is not a valid array or is empty:', datatablejson);
        }
    } catch (error) {
        console.log('Error parsing datatablejson:', error);
    }
    });
  }




  onedit(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnLeavebalanceedit',encryptedParam]) 
  }

  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnLeavebalancesummary',encryptedParam]) 
  }

  exportexcel() :void {
    debugger;

    const LeaveBalance = this.leavebalance.map((item: { user_name: any; Casual_Leave: any; Sick_Leave: any; Compensatory_off: any; }) => ({
      
      UserCode_UserName: item.user_name || '', 
      CasualLeave: item.Casual_Leave || '',
      SickLeave: item.Sick_Leave || '',
      Compensatoryoff: item.Compensatory_off || '',
    }));
          this.excelService.exportAsExcelFile(LeaveBalance, 'Leave_Balance');
  }
}


