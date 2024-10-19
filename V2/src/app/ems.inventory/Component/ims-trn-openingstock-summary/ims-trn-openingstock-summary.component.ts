import { Component } from '@angular/core';
import { FormGroup,FormBuilder,FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-openingstock-summary',
  templateUrl: './ims-trn-openingstock-summary.component.html',
  styleUrls: ['./ims-trn-openingstock-summary.component.scss'],
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
export class ImsTrnOpeningstockSummaryComponent {
  stock_list:any;
  response_data :any;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  invoice:any;
  showOptionsDivId: any; 
  Branchdtl_list:any;
  branch_name1:any;
  FinancialYear_List:any;
  finyear:any;
  constructor(private fb: FormBuilder,public NgxSpinnerService: NgxSpinnerService,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,) {} 
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }

  }
  ngOnInit(): void {
    this.ImsTrnOpeningstockSummary();
    this.runBackgroundApiCall6();
    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      finyear: new FormControl('',Validators.required ),
      entity_name: new FormControl(null,Validators.required),
     
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  async runBackgroundApiCall6() {
    debugger;
    var api6 = 'ImsTrnOpeningStock/MintsoftProductStockDetailsAsync';
    try {
      const result = await this.service.get(api6).toPromise();
      // console.log(result);
    } catch (error) {
      //console.error(error);
    }
  }

  
  ImsTrnOpeningstockSummary(){
debugger;
    var url = 'ImsTrnOpeningStock/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name1 = this.Branchdtl_list[0].branch_gid;

      var url = 'ImsTrnStockSummary/GetFinancialYear'
      this.service.get(url).subscribe((result: any) => {
        this.FinancialYear_List = result.GetFinancialYear;
        this.finyear = this.FinancialYear_List[0].finyear;

        if(this.FinancialYear_List[0].finyear!=null){

          this.finyear=this.FinancialYear_List[0].finyear;

        }
        else{
          this.finyear=null;
        }
        let params = {
          branch_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.finyear
        }
    var api = 'ImsTrnOpeningStock/GetImsTrnOpeningstockSummary';
    this.NgxSpinnerService.show()
    this.service.getparams(api,params).subscribe((result:any) => {
      this.response_data = result;
      this.stock_list = this.response_data.stock_list;
      setTimeout(()=>{  
        $('#stock_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()
    });
  });
});
  
  }
  onadd()
  {
        this.router.navigate(['/ims/ImsTrnOpeningstockAdd'])
  }
  onedit(stockGid: any) {
    // if (this.ShowEditButton(stockGid)) {
       
        const secretKey = 'storyboarderp';
        const param = stockGid;
        const encryptedParam = AES.encrypt(param, secretKey).toString();
        this.router.navigate(['/ims/ImsTrnOpeningstockEdit', encryptedParam]);
    // } else {
       
        
    //     console.log('Issued_qty > 0. So, hiding the Edit button.');
    // }
}
OnChangeFinancialYear(){
debugger
  let params = {
    branch_gid: this.branch_name1,
    finyear: this.finyear
  }
  var api = 'ImsTrnOpeningStock/GetImsTrnOpeningstockSummary';
  this.NgxSpinnerService.show()
  this.service.getparams(api,params).subscribe((result:any) => {
  this.response_data = result;
  this.stock_list = this.response_data.stock_list;
  setTimeout(()=>{  
    $('#stock_list').DataTable();
  }, 1);
  this.NgxSpinnerService.hide()
  });

}

ShowEditButton(stockGid: any): boolean {
  const item = this.stock_list.find((item: { stock_gid: any; issued_qty: number; }) => item.stock_gid === stockGid);
  return item ? item.issued_qty == 0 : false;
}
}

