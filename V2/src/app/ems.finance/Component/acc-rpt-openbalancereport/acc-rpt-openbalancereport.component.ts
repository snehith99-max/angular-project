import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr'; //date-pickr
import { Options } from 'flatpickr/dist/types/options'; //date-pickr options
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-acc-rpt-openbalancereport',
  templateUrl: './acc-rpt-openbalancereport.component.html',
  styleUrls: ['./acc-rpt-openbalancereport.component.scss']
})
export class AccRptOpenbalancereportComponent {
  parentList: any;
  subList: any;
  AssetList: any;
  AssetSubList: any;
  Lib_total_amount: any;
  Asset_total_amount: any;
  Debit_Lib_total_amount: any;
  Credit_Asset_total_amount: any;
  branchname_list: any;
  defaultbranch: any;
  reactiveform!: FormGroup
  libartydiffer: any;
  Entitydtl_list: any;
  Assetdiffer: any;
  FinancialYear_List: any[] = [];
  entity_gid: any;
  finyear: any;
  entity_name: any;

  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService,private formBuilder: FormBuilder,private route: Router,) { 
    this.reactiveform = new FormGroup({
 
      // from_date: new FormControl(this.getPreviousSixMonthsDate(), Validators.required),
      // to_date: new FormControl(this.getCurrentDate(), Validators.required),
      finyear: new FormControl('', Validators.required), 
      entity_name: new FormControl(null,Validators.required),
   })
  }

  

  ngOnInit(): void { 
  
    const urlBranch = 'AccMstBankMaster/GetBranchName';
    this.service.get(urlBranch).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      if (this.branchname_list.length > 0) {
        this.defaultbranch = this.branchname_list[0].branch_gid;
        this.reactiveform.patchValue({ frombranch: this.defaultbranch }); 
       
        this.initializeDataFetching();
      }
  
    });
  
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    var url = 'AccMstOpeningbalance/GetEntityDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Entitydtl_list = result.entitydtl_lists;  
      if (this.Entitydtl_list.length > 0) {
      this.entity_name = this.Entitydtl_list[0].entity_gid;    
      this.reactiveform.patchValue({ entity_name: this.entity_name });   
      this.initializeDataFetching();       
      }  
    });
  
    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear; 
      this.reactiveform.patchValue({ finyear: this.finyear });   
      // console.log(this.FinancialYear_List[0].finyear_gid)   
      this.initializeDataFetching();    
    });

   
   
  }

  onroute(params: any) {
    //debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage ='OB';
    const encryptedParam1 = AES.encrypt(lspage, secretKey).toString();
    this.route.navigate(['/finance/AccRptProfitandLostDetails', encryptedParam,encryptedParam1])
  }

  
  
  
  initializeDataFetching() {
    this.NgxSpinnerService.show();
    let param = {
      entity_name: this.reactiveform.value.entity_name,
      finyear: this.reactiveform.value.finyear,
      // from_date: this.reactiveform.value.from_date,
      // to_date: this.reactiveform.value.to_date
    };
  
    const url1 = 'AccOpeningBalanceReport/getFolders';
    this.service.getparams(url1, param).subscribe((result: any) => {
      // Lib and Asset List in API 
      if (result && result.openingbalanceFolders != null) {
        this.parentList = result.openingbalanceFolders.map((item: any) => ({ ...item, visible: false }));
    }
    else {
      this.parentList=[];
    }    
    
    if (result && result.openingbalanceSubFolders != null) {
        this.subList = result.openingbalanceSubFolders.map((item: any) => ({ ...item, visible: false }));
    }
    else {
      this.subList=[];
    }
    
    if (result && result.openingbalanceAssetFolders != null) {
        this.AssetList = result.openingbalanceAssetFolders.map((item: any) => ({ ...item, visible: false }));
    }
    else {
      this.AssetList=[];
    }
    
    if (result && result.openingbalanceAssetSubFolders != null) {
        this.AssetSubList = result.openingbalanceAssetSubFolders.map((item: any) => ({ ...item, visible: false }));
    }
    else {
      this.AssetSubList=[];
    }
    

      // Lib and Asset Totals
      if(this.parentList !=null) {
        const totals = this.sumOfAllProperties(this.parentList);
        this.Lib_total_amount = totals.totalCreditAmount;
        const Debittotals = this.sumOfAllPropertiesDebit(this.parentList);
        this.Debit_Lib_total_amount = Debittotals.totalCreditAmount;
      }
      if(this.AssetList !=null) {
        const Assettotals = this.sumOfAllPropertiesAsset(this.AssetList);
        this.Asset_total_amount = Assettotals.totalCreditAmount;
        const CreditAssettotals = this.sumOfAllPropertiesAssetcredit(this.AssetList);
        this.Credit_Asset_total_amount = CreditAssettotals.totalCreditAmount;
      }
      // Lib and Asset diffrence


let libTotalAmount = parseFloat(this.Lib_total_amount.replace(/,/g, ''));
let debitLibTotalAmount = parseFloat(this.Debit_Lib_total_amount.replace(/,/g, ''));
let difference = libTotalAmount - debitLibTotalAmount;
this.libartydiffer =  this.formatValue(difference);

let assetTotalAmount = this.Asset_total_amount.replace(/,/g, '');
let creditassetTotalAmount = this.Credit_Asset_total_amount.replace(/,/g, '');
let differenceAsset = assetTotalAmount - creditassetTotalAmount;
this.Assetdiffer = this.formatValue(differenceAsset);


      


      this.addItemsFromTargetList();
      this.NgxSpinnerService.hide();
    });
  }
  
 

  getPreviousSixMonthsDate(): string {
    const today = new Date();
    today.setMonth(today.getMonth() - 6);
    today.setDate(1); 
    return this.formatDate(today);
  }

  formatDate(date: Date): string {
    const dd = String(date.getDate()).padStart(2, '0');
    const mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = date.getFullYear();
  
    return `${dd}-${mm}-${yyyy}`;
  }

  getCurrentDate(): string {
    const today = new Date();
    return this.formatDate(today);
  }


  
  sumOfAllProperties(list: any[]): { 
    totalCreditAmount: string, 
  } 
  
  {
    const result = list.reduce((acc, account) => {
      acc.totalCreditAmount += this.parseValue(account.sum_credit);
      return acc;
    }, 
    
    { 
      totalCreditAmount: 0, 
    });
  
    return {
      totalCreditAmount: this.formatValue(result.totalCreditAmount)
    };
  }

  sumOfAllPropertiesDebit(list: any[]): { 
    totalCreditAmount: string, 
  } 
  
  {
    const result = list.reduce((acc, account) => {
      acc.totalCreditAmount += this.parseValue(account.sum_debit);
      return acc;
    }, 
    
    { 
      totalCreditAmount: 0, 
    });
  
    return {
      totalCreditAmount: this.formatValue(result.totalCreditAmount)
    };
  }

  sumOfAllPropertiesAsset(list: any[]): { 
    totalCreditAmount: string, 
  } 
  
  {
    const result = list.reduce((acc, account) => {
      acc.totalCreditAmount += this.parseValue(account.sum_debit);
      return acc;
    }, 
    
    { 
      totalCreditAmount: 0, 
    });
  
    return {
      totalCreditAmount: this.formatValue(result.totalCreditAmount)
    };
  }

  sumOfAllPropertiesAssetcredit(list: any[]): { 
    totalCreditAmount: string, 
  } 
  
  {
    const result = list.reduce((acc, account) => {
      acc.totalCreditAmount += this.parseValue(account.sum_credit);
      return acc;
    }, 
    
    { 
      totalCreditAmount: 0, 
    });
  
    return {
      totalCreditAmount: this.formatValue(result.totalCreditAmount)
    };
  }

  
  parseValue(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }
  
   formatValue(value: number): string {
    // Format the number as a string with comma separators
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }  
  addItemsFromTargetList() {
    this.subList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.parentList);
  
    });

    this.AssetSubList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.AssetList);
  
    });
  }

  recursivelyAddItems(targetItem: any, sourceList: any[]) {
    const matchingIndex = sourceList.findIndex(sourceItem => sourceItem.account_gid === targetItem.accountgroup_gid);
    if (matchingIndex !== -1) {
      if (!sourceList[matchingIndex].subfolders) {
        sourceList[matchingIndex].subfolders = [];
      }
      sourceList[matchingIndex].subfolders.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.subfolders && sourceItem.subfolders.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.subfolders);
        }
      });
    }

  
  }

  toggleVisibility(item: any) {
    item.visible = !item.visible;

  }




}
