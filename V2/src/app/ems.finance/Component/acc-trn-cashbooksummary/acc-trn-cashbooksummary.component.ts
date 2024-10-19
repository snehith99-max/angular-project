import { Component } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-cashbooksummary',
  templateUrl: './acc-trn-cashbooksummary.component.html',
  styleUrls: ['./acc-trn-cashbooksummary.component.scss']
})

export class AccTrnCashbooksummaryComponent {
  CashBook_list: any;
  response_data: any;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  invoice: any;
  showOptionsDivId: any;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) { }
  ngOnInit(): void {
    this.AccTrnCashbooksummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  AccTrnCashbooksummary() {
    this.NgxSpinnerService.show();
    var api = 'AccTrnCashBookSummary/GetAccTrnCashbooksummary';
    this.service.get(api).subscribe((result: any) => {
      $('#CashBook_list').DataTable().destroy();
      this.response_data = result;
      this.CashBook_list = this.response_data.CashBook_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#CashBook_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  onselect(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const branch_gid = AES.encrypt(param, secretKey).toString();
    const finyear_gid = new Date().getFullYear();
    this.router.navigate(['/finance/AccTrnCashbookSelect', branch_gid, finyear_gid])
  }
  
  edit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const branch_gid = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/finance/AccTrnCashbookedit', branch_gid])
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
}