import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-acc-trn-bankbooksummary',
  templateUrl: './acc-trn-bankbooksummary.component.html',
  styleUrls: ['./acc-trn-bankbooksummary.component.scss']
})
export class AccTrnBankbooksummaryComponent {
  responsedata: any;
  bankbook_list: any[] = [];
  BankBookSummaryForm: FormGroup | any;
   bankstatus_name: any;
  lsbankstatus_name: any;
  banked_type: any;
  showOptionsDivId:any;
  bank_gid: any;
  lsbank_gid: any;
  // bankstatuslist = [     
  //   {id: 1, bankstatus_name:'Active Banks'},  
  //   {id: 2, bankstatus_name:'All Banks'}
  // ];
  // bankstatus_name: any;

  constructor(public service: SocketService, private router: Router, private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {

  }

  ngOnInit(): void {
    // const bank_gid = this.route.snapshot.paramMap.get('bank_gid');
    // this.bank_gid = bank_gid;
    // const secretKey = 'storyboarderp';
    // const deencryptedParam = AES.decrypt(this.bank_gid, secretKey).toString(enc.Utf8);
    // this.bank_gid = deencryptedParam;
    // this.lsbank_gid = deencryptedParam;
    // console.log(deencryptedParam);

    // this.AccTrnBankbooksummary();
    // this.bankstatus_name = 'Active Banks';

    this.BankBookSummaryForm = new FormGroup({
      bank_type: new FormControl('Active Banks')
    });

    this.OnChangeBankName();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }

  AccTrnBankbooksummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetBankBookSummary'
    // let params = {
    //   bank_gid: deencryptedParam,
    // }
    this.service.get(url).subscribe((result: any) => {
      $('#bankbook_list').DataTable().destroy();
      this.responsedata = result;
      this.bankbook_list = this.responsedata.Getbankbook_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#bankbook_list').DataTable(
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
    this.banked_type = this.BankBookSummaryForm.value.bank_type;
    if (this.banked_type === 'All Banks') {
      // console.log("Selected ID is 1");
      this.NgxSpinnerService.show();
      var url = 'AccTrnBankbooksummary/GetAllBankBookSummary'      
      this.service.get(url).subscribe((result: any) => {
        $('#bankbook_list').DataTable().destroy();
        this.responsedata = result;
        this.bankbook_list = this.responsedata.Getbankbook_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#bankbook_list').DataTable(
            {
              // code by snehith for customized pagination
              "pageLength": 50, // Number of rows to display per page
              "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
            }
          );
        }, 1);
      });
    } else {
      // console.log("Selected ID is not 1");
      this.NgxSpinnerService.show();
      var url = 'AccTrnBankbooksummary/GetBankBookSummary'
      this.service.get(url).subscribe((result: any) => {
        $('#bankbook_list').DataTable().destroy();
        this.responsedata = result;
        this.bankbook_list = this.responsedata.Getbankbook_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#bankbook_list').DataTable(
            {
              // code by snehith for customized pagination
              "pageLength": 50, // Number of rows to display per page
              "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
            }
          );
        }, 1);
      });
    }

  }

  onclick(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const finyear_gid = new Date().getFullYear();
    this.router.navigate(['/finance/AccTrnBankbook', encryptedParam, finyear_gid])
  }

  bankstatuslist = [   
    {id: 1, bankstatus_name:'Active Banks'},  
    {id: 2, bankstatus_name:'All Banks'}
  ];

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

}
