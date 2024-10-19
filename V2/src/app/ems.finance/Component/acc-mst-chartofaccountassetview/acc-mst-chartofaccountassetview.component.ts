import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-mst-chartofaccountassetview',
  templateUrl: './acc-mst-chartofaccountassetview.component.html',
  styleUrls: ['./acc-mst-chartofaccountassetview.component.scss']
})
export class AccMstChartofaccountassetviewComponent {
  account_gid:any;
  lsaccount_gid:any;
  Getchartofsubaccount_list: any;
  responsedata: any;
  lsaccount_name:any;
  lsaccount_code:any;
  account_code:any;
  account_name:any;
  ledgerForm!:FormGroup;
  ledgerFormsub!:FormGroup;
  account_subgid:any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.ledgerFormsub = this.fb.group({
      account_gid: new FormControl(null),
      accountcodes: new FormControl(null),
      accountgroups: new FormControl(null),
      accountsubcode: new FormControl(null, [
        Validators.required,
        Validators.pattern("(?=.*[a-zA-Z0-9]).+$"),
      ]),
      accountsubgroup: new FormControl(null, [
        Validators.required,
        Validators.pattern(""),
      ]),
      ledger_flag: ['N']
    });
  }

  ngOnInit(): void {

      this.ledgerForm = this.fb.group({
        ledger_flag: ['No'] 
      });
  

    const secretKey = 'storyboarderp';
    const lsmonth = this.route.snapshot.paramMap.get('account_gid');
    this.lsaccount_gid = lsmonth;
    const deencryptedParam = AES.decrypt(this.lsaccount_gid, secretKey).toString(enc.Utf8);
    this.account_gid = deencryptedParam;
    const lsaccount_code = this.route.snapshot.paramMap.get('account_code');
    this.lsaccount_code = lsaccount_code;
    const deencryptedParam1 = AES.decrypt(this.lsaccount_code, secretKey).toString(enc.Utf8);
    this.account_code = deencryptedParam1;
    const lsaccount_name = this.route.snapshot.paramMap.get('account_name');
    this.lsaccount_name = lsaccount_name;
    const deencryptedParam2 = AES.decrypt(this.lsaccount_name, secretKey).toString(enc.Utf8);
    this.account_name = deencryptedParam2;
  this.getsummary(this.account_gid);
  }
  getsummary(account_gid:any){
    let param = {
      account_gid: account_gid
    }
    var url = 'ChartofAccount/ChartofSubAccountSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#Getchartofsubaccount_list').DataTable().destroy();
      this.responsedata = result;
      this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
     // console.log('list',this.Getchartofsubaccount_list)
      setTimeout(() => {
        $('#Getchartofsubaccount_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  get accountsubcode() {
    return this.ledgerFormsub.get('accountsubcode')!;
  }
  get accountsubgroup() {
    return this.ledgerFormsub.get('accountsubgroup')!;
  }
  childview(account_gid:any,account_code:any,account_name:any){
    this.account_gid=account_gid;
    this.account_code = account_code;
    this.account_name = account_name;
    let param = {
      account_gid: this.account_gid
    }
    var url = 'ChartofAccount/ChartofSubAccountSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#Getchartofsubaccount_list').DataTable().destroy();
      this.responsedata = result;
      this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
      //console.log('list',this.Getchartofsubaccount_list)
      setTimeout(() => {
        $('#Getchartofsubaccount_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
   }

   onsubmitsubgroup() {
    if(this.ledgerFormsub.status =='VALID' )
    {
      this.NgxSpinnerService.show();
      // console.log(this.ledgerFormsub.value)
      var api = 'ChartofAccount/PostAccountSubGroup';
      this.service.post(api, this.ledgerFormsub.value).subscribe(
        (result: any) => {
          this.responsedata = result;

          if (result.status == true) {
            this.NgxSpinnerService.hide();
            this.getsummary(this.account_gid);
            this.ToastrService.success(result.message)   
            this.ledgerFormsub.reset();
            this.ledgerFormsub.get('ledger_flag')?.setValue('N');
            // this.commonsubgetsummary();
            // console.log(this.account_gid)
           // this.getsummaryadd();   
            
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
         

          }

        });
    }
  }
  onaddsubgroup() {
    //  console.log(this.account_code)
    //  console.log(this.account_group)
    //  console.log(this.account_gid)
    this.ledgerFormsub.get("accountcodes")?.setValue(this.account_code);
    this.ledgerFormsub.get("accountgroups")?.setValue(this.account_name);
    this.ledgerFormsub.get("account_gid")?.setValue(this.account_gid);
  }
  openModalsubedit(list: any) {
    this.ledgerFormsub.get("accountsubcode")?.setValue(list.account_code);
    this.ledgerFormsub.get("accountsubgroup")?.setValue(list.account_name);
    this.ledgerFormsub.get("accountcodes")?.setValue(list.account_gid);
    this.ledgerFormsub.get("account_gid")?.setValue(this.account_gid);
 
    const hasChildValue = list.has_child;
    const ledgerFlagValue = hasChildValue === 'N' ? 'Y' : 'N'; // or any other desired value

    // Set the value of ledger_flag form control
    this.ledgerFormsub.get('ledger_flag')?.setValue(ledgerFlagValue);
    // this.ledgerFormsub.get("ledger_flag")?.setValue(list.has_child);
  }
  onupdatesubgroup() {
    if(this.ledgerFormsub.status =='VALID' )
    {
    //console.log(this.ledgerFormsub.value)
    this.NgxSpinnerService.show();
    var url = 'ChartofAccount/UpdateAccountSubGroup'

    this.service.post(url,this.ledgerFormsub.value).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.ledgerFormsub.reset();
        this.ledgerFormsub.get('ledger_flag')?.setValue('N');
      }
      else{
    this.getsummary(this.account_gid);
        this.ToastrService.success(result.message)
        this.ledgerFormsub.reset();
        this.ledgerFormsub.get('ledger_flag')?.setValue('N');

      }
      this.NgxSpinnerService.hide();

      
  }); 
    }
  }
  ondelete()
  {
    //console.log(this.account_gid)
    this.NgxSpinnerService.show();
    var url = 'ChartofAccount/DeleteChartofAccount'
    let param = {
      account_gid : this.account_subgid
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        window.scrollTo({
  
          top: 0, // Code is used for scroll top after event done
  
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)

      }
      else{
        window.scrollTo({
  
          top: 0, // Code is used for scroll top after event done
  
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.getsummary(this.account_gid );
       
      }
      
  
    });
  }
  onclosesubgroup() {
    this.ledgerFormsub.reset();
    this.ledgerFormsub.get("ledger_flag")?.setValue('N');

  }
  handleDeleteClick(event: Event, categoryId: string) {
    this.account_subgid =categoryId;
    //console.log(`Delete clicked for category ID: ${this.account_gid}`);
  }
  
}
