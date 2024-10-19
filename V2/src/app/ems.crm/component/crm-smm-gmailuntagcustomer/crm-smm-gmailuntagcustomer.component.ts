import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Table } from 'primeng/table';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
export class IAssign {

  inbox_id: any;
  Getgmailcustomerunassignedlist: any;


}
@Component({
  selector: 'app-crm-smm-gmailuntagcustomer',
  templateUrl: './crm-smm-gmailuntagcustomer.component.html',
  styleUrls: ['./crm-smm-gmailuntagcustomer.component.scss']
})
export class CrmSmmGmailuntagcustomerComponent {
  unassigncustomer_list: any[] = [];
  CurObj: IAssign = new IAssign();
  pick: Array<any> = [];
  inbox_id: any;
  customer_gid: any;
  encryptparam: any;
  selection = new SelectionModel<IAssign>(true, []);
  lspage: any;
  lspagevalue: any;
  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private router: ActivatedRoute,
    private route: Router,
    private NgxSpinnerService: NgxSpinnerService,
    private sanitizer: DomSanitizer) {
    // this.isRowSelectable = this.isRowSelectable.bind(this);
  }
  ngOnInit(): void {
    debugger
    const inbox_id = this.router.snapshot.paramMap.get('inbox_id');

    this.encryptparam = inbox_id;

    const key = 'storyboard';
    this.inbox_id = AES.decrypt(this.encryptparam, key).toString(enc.Utf8)
    const ls_page = this.router.snapshot.paramMap.get('lspage');
    this.lspagevalue = ls_page;
    const deencryptedParam1 = AES.decrypt(this.lspagevalue, key).toString(enc.Utf8);

    this.lspage = deencryptedParam1;
    let param = { inbox_id: this.inbox_id }
    this.NgxSpinnerService.show();
    var url = 'GmailCampaign/GetCustomerUnAssignedlist';
    this.service.getparams(url, param).subscribe((apiresponse: any) => {
      this.unassigncustomer_list = apiresponse.Getgmailcustomerunassignedlist;
      console.log(this.unassigncustomer_list)
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#unassigncustomer_list').DataTable();
      }, 1);

    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.unassigncustomer_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.unassigncustomer_list.forEach((row: IAssign) => this.selection.select(row));
  }
  untagcustomer() {
    debugger;
    this.pick = this.selection.selected;
    this.CurObj.Getgmailcustomerunassignedlist = this.pick;
    this.CurObj.inbox_id = this.inbox_id;

    if (this.CurObj.Getgmailcustomerunassignedlist.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly Select Atleast One Record to Tag Customer.");
      return;
    } else {
      this.NgxSpinnerService.show();
      var url = 'GmailCampaign/UnTagCustomertoGmail';
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result.status === false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning('Error Occurred While Customer Untag to Gmail.');
        } else {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success('Inbox Mail Untag from Customer Successfully !!');

          // Delay navigation to ensure toastr message is shown
          setTimeout(() => {
            if (this.lspage == "GI") {
              this.route.navigate(['/crm/CrmSmmGmailInboxSummary']);
            }
            else if (this.lspage == "GF") {
              this.route.navigate(['/crm/CrmSmmGmailFolderSummary']);
            }
            else if (this.lspage == "OI") {
              this.route.navigate(['/crm/CrmSmmOutlookInboxSummary']);
            }
            else{
              this.route.navigate(['/crm/CrmSmmOutlookFolderSummary']);
            }
          }, 500); // Adjust the delay as needed
        }
      });
      this.selection.clear();
    }
  }
  onback()
  {
    if (this.lspage == "GI") {
      this.route.navigate(['/crm/CrmSmmGmailInboxSummary']);
    }
    else if (this.lspage == "GF") {
      this.route.navigate(['/crm/CrmSmmGmailFolderSummary']);
    }
    else if (this.lspage == "OI") {
      this.route.navigate(['/crm/CrmSmmOutlookInboxSummary']);
    }
    else{
      this.route.navigate(['/crm/CrmSmmOutlookFolderSummary']);
    }
  }
}
