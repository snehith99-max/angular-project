import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-acc-mst-bankmaster',
  templateUrl: './acc-mst-bankmaster.component.html',
  styleUrls: ['./acc-mst-bankmaster.component.scss']
})

export class AccMstBankmasterComponent {
  bankmaster_list: any[] = [];
  responsedata: any;
  parameterValue: any;
  showOptionsDivId: any;
  banked_type: any;
  bankbook_list: any[] = [];
  BankBookSummaryForm!: FormGroup;
  status_flag: any;
  showInputs: boolean = false;

  constructor(public service: SocketService, private route: ActivatedRoute, private router: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) { }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

    this.BankBookSummaryForm = new FormGroup({
      bank_type: new FormControl('Y')
    });

    this.GetBankMasterSummary1();
  }

  GetBankMasterSummary1() {
    var param = {
      default_flag: this.BankBookSummaryForm.value.bank_type
    }

    var url = 'AccMstBankMaster/GetBankMasterSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#bankmaster_list').DataTable().destroy();
      this.responsedata = result;
      this.bankmaster_list = this.responsedata.GetBankMaster_list;

      setTimeout(() => {
        $('#bankmaster_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  OnChangeBankName() {
    var param = {
      default_flag: this.BankBookSummaryForm.value.bank_type
    }

    var url = 'AccMstBankMaster/GetBankMasterSummaryInactive'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#bankmaster_list').DataTable().destroy();
      this.responsedata = result;
      this.bankmaster_list = this.responsedata.GetBankMaster_list;
      setTimeout(() => {
        $('#bankmaster_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  
  onactive() {
    let param = {
      status_flag: "N",
      bank_gid: this.parameterValue,
    }
    
    var url3 = 'AccMstBankMaster/PostBankMasterStatus'
    this.service.getparams(url3, param).subscribe((result: any) => {
      if (result.status == true) {        
        this.ToastrService.success(result.message)
        this.GetBankMasterSummary1();
      }
      else {       
        this.GetBankMasterSummary1();
        this.ToastrService.warning(result.message)
      }
    });
  }

  oninactive() {
    let param = {
      status_flag: "Y",
      bank_gid: this.parameterValue,
    }

    var url3 = 'AccMstBankMaster/PostBankMasterStatus'
    this.service.getparams(url3, param).subscribe((result: any) => {
      if (result.status == true) {
        this.GetBankMasterSummary1();
        this.ToastrService.success(result.message)        
      }
      else {
        this.GetBankMasterSummary1();
        this.ToastrService.warning(result.message)
      }
    });
  }  

  onadd() {
    this.router.navigate(['/finance/AccMstBankMasterAdd'])
  }

  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/finance/AccMstBankmasterEdit', encryptedParam])
  }

  openModalactive(parameter: string) {
    this.parameterValue = parameter
  }

  openModalinactive(parameter: string) {
    this.parameterValue = parameter
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }  
}