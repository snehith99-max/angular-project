import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
export class IGSTIN{
  GetInGstManagement_lists: string[] = [];
}
@Component({
  selector: 'app-acc-trn-gstinsummary',
  templateUrl: './acc-trn-gstinsummary.component.html',
  styleUrls: ['./acc-trn-gstinsummary.component.scss']
})
export class AccTrnGstinsummaryComponent {
  pick: Array<any> = [];
  CurObj: IGSTIN = new IGSTIN();
  selection = new SelectionModel<IGSTIN>(true, []);
  lsyear:any;
  responsedata: any;
  year:any;
  lsmonth:any;
  month:any;
  GetInGstManagement_list:any;
 constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit(): void {
   
    const secretKey = 'storyboarderp';
    const lsmonth = this.route.snapshot.paramMap.get('lsmonth');
    this.lsmonth = lsmonth;
    const deencryptedParam = AES.decrypt(this.lsmonth, secretKey).toString(enc.Utf8);
    this.month = deencryptedParam;
    const lsyear = this.route.snapshot.paramMap.get('lsyear');
    this.lsyear = lsyear;
    const deencryptedParam1 = AES.decrypt(this.lsyear, secretKey).toString(enc.Utf8);

    this.year = deencryptedParam1;
    // console.log('month',this.month)
    // console.log('year',this.year)
    let param = {
      month: this.month,
      year:this.year
    }
    var url = 'GstManagement/InGstManagementSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetInGstManagement_list = this.responsedata.GetInGstManagement_list;
      //console.log('list',this.GetInGstManagement_list)
      setTimeout(() => {
        $('#GetInGstManagement_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 200, // Number of rows to display per page
            "lengthMenu": [200, 500, 1000, 1500], // Dropdown to change page length
          }
        );
      }, 1);
    });
     }
     isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.GetInGstManagement_list.length;
      return numSelected === numRows;
    }
  
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.GetInGstManagement_list.forEach((row: IGSTIN) => this.selection.select(row));
    }
	OnSubmit() {

    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.GetInGstManagement_lists = list
    console.log(this.CurObj)
	}
  clear() {
    this.router.navigate(['/finance/AccTrnGstmanagementSummary']);
  }
}
