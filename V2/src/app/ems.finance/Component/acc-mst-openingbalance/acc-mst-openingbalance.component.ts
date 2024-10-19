import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, TitleStrategy } from '@angular/router';
import { AES } from 'crypto-js';
import { data } from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { catchError, finalize } from 'rxjs/operators';
import { Location } from '@angular/common'
@Component({
  selector: 'app-acc-mst-openingbalance',
  templateUrl: './acc-mst-openingbalance.component.html',
  styleUrls: ['./acc-mst-openingbalance.component.scss']
})

export class AccMstOpeningbalanceComponent {
  branch_gid: any
  responsedata: any;
  Openingbalance_lists: any[] = [];
  Openingbalance_list: any[] = [];
  openingbalanceedit: any[] = [];
  totaldebitAmount: any;
  OpeningBalanceForm: FormGroup | any;
  OpeningBalanceEditForm: FormGroup | any;
  OpeningBalanceAssetEditForm: FormGroup | any;
  balanced_type: any;
  showparentname: any;
  disabledparentname: any;
  branchname_list: any;
  ParentNameList: any[] = [];
  ParentNameasset_List: any[] = [];
  liabilityparentname: any;
  liabilityacct_List: any[] = [];
  assetparentname: any;
  assetacct_List: any[] = [];
  FinancialYear_List: any[] = [];
  totalCredit: any;
  totalDebit: any;
  difference_value: any;
  parameterValue1: any;
  parameterValue2: any;
  acctgroupnameview: any;
  accountnameview: any;
  branchview: any;
  editremarks: any;
  status: any;
  assetaccountnameview: any;
  assetacctgroupnameview: any;
  assetbranchview: any;
  Entitydtl_list: any;
  entitynameview: any;
  financialyearview: any;
  entity_gid: any;
  assetentitynameview: any;
  assetfinancialyearview: any;
  showOptionsDivId: any;
  finyear: any;
  reactiveform!: FormGroup
  entity_name: any;
  LiabilityGroupName_lists: any[] = [];
  LiabilitySubGroupName_lists: any[] = [];
  LiabilityLedgerName_lists: any[] = [];
  LedgerAccountList: any[] = [];
  AccountList: any[] = [];
  AssetGroupName_lists: any[] = [];
  AssetSubGroupName_lists: any[] = [];
  AssetLedgerName_lists: any[] = [];
  AssetLedgerAccountList: any[] = [];
  AssetAccountList: any[] = [];
  Branchdtl_list: any[] = [];
  account_gid: any;
  LiabilitySubgroupflag: boolean = false;
  LiabilityLedgerflag: boolean = false;
  AssetSubgroupflag: boolean = false;
  AssetLedgerflag: boolean = false;
  Liability_AccountName: any;
  LiabilityLedger_AccountName: any;
  Asset_AccountName: any;
  AssetLedger_AccountName: any;
  branch_name1: any;
  totalAssetLedgerName: any;
  main: any;
  processedOpeningbalanceLists: any[] = [];
  processedOpeningbalanceList: any[] = [];
  processedData: any[] = [];
  prevMainGroup: any;
  prevSubGroup: any;
  liabilitySummary_lists: any;
  liabilitysubled_Summary_lists: any;
  subList: any[] = [];
  currentTab = 'Liability';

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private Location: Location) {
    this.reactiveform = new FormGroup({
      finyear: new FormControl('', Validators.required),
      entity_name: new FormControl(null, Validators.required),
    })
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);
    this.showparentname = true;
    this.GetOpeningbalance();

    this.OpeningBalanceForm = new FormGroup({
      acctref_no: new FormControl(''),
      entity_name: new FormControl(null,),
      branch_name: new FormControl(null, [Validators.required]),
      balance_type: new FormControl('Liability', Validators.required),
      parent_name: new FormControl(null),
      liabilityaact_name: new FormControl(null, [Validators.required]),
      date_value: new FormControl(this.getCurrentDate(),),
      amount_value: new FormControl('', [Validators.required,]),
      remarks: new FormControl(''),
      finyear: new FormControl(null, [Validators.required]),
    });

    this.OpeningBalanceEditForm = new FormGroup({
      editamount_value: new FormControl('', [Validators.required,]),
      editremarks: new FormControl(null),
      opening_balance_gid: new FormControl(''),
      branch_name: new FormControl(''),
    });

    this.OpeningBalanceAssetEditForm = new FormGroup({
      editassetamount_value: new FormControl('', [Validators.required,]),
      editassetremarks: new FormControl(null),
      opening_balance_gid: new FormControl(''),
      branch_name: new FormControl(''),
    });

    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
    });

    this.OnChangeBalanceType();

    var url = 'AccMstOpeningbalance/GetEntityDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Entitydtl_list = result.entitydtl_lists;
      this.entity_name = this.Entitydtl_list[0].entity_gid;
      this.reactiveform.patchValue({ entity_name: this.entity_name });
    });

    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear;
    });

    var url = 'AccMstOpeningbalance/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name1 = this.Branchdtl_list[0].branch_gid;
      this.reactiveform.patchValue({ branch_name: this.branch_name1 });
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

    this.OnChangeParentName();
    this.OnChangeAssetParentName();
    this.GetLiabilityGroupNameList();
    this.GetAssetGroupNameList();
    this.GetSummary();
  }

  GetOpeningbalance() {
    var url = 'AccMstOpeningbalance/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name1 = this.Branchdtl_list[0].branch_gid;

      var url = 'AccTrnBankbooksummary/GetFinancialYear'
      this.service.get(url).subscribe((result: any) => {
        this.FinancialYear_List = result.GetFinancialYear_List;
        this.finyear = this.FinancialYear_List[0].finyear;

        let params = {
          entity_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.FinancialYear_List[0].finyear
        }

        this.NgxSpinnerService.show();
        this.service.getparams('AccMstOpeningbalance/GetOpeningbalance', params).pipe(
          catchError(error => {
            console.error('Error fetching opening balance (credit):', error);
            return [];
          }),
          finalize(() => {
            this.NgxSpinnerService.hide();
            this.calculateDifference();
          })
        ).subscribe((result: any) => {
          this.Openingbalance_list = result?.Openingbalance_list || [];
          this.totalCredit = this.calculateTotal(this.Openingbalance_list);
          if (this.Openingbalance_list.length <= 0) {
            this.processedOpeningbalanceLists = [];
          }
          else {
            this.processedOpeningbalanceLists = this.preprocessData(this.Openingbalance_list);
          }
        });
        // Fetch opening balance data for debit amounts
        this.service.getparams('AccMstOpeningbalance/GetAccMstOpeningbalance', params).pipe(catchError(error => {
          console.error('Error fetching opening balance (debit):', error);
          return [];
        }),
          finalize(() => {
            this.NgxSpinnerService.hide();
            this.calculateDifference();
          })
        ).subscribe((result: any) => {
          this.Openingbalance_lists = result?.Openingbalance_lists || [];
          this.totalDebit = this.calculateTotals(this.Openingbalance_lists);
          if (this.Openingbalance_lists.length <= 0) {
            this.processedOpeningbalanceList = [];
          }
          else {
            this.processedOpeningbalanceList = this.preprocessData(this.Openingbalance_lists);
          }
        });
      });
    });
  }

  toggleVisibility(item: any) {
    item.visible = !item.visible;
  }

  OnChangeFinancialYear() {
    debugger
    let params = {
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    this.NgxSpinnerService.show();
    this.service.getparams('AccMstOpeningbalance/GetOpeningbalance', params).pipe(
      catchError(error => {
        console.error('Error fetching opening balance (credit):', error);
        return [];
      }),
      finalize(() => {
        this.NgxSpinnerService.hide();
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.Openingbalance_list = result?.Openingbalance_list || [];
      this.totalCredit = this.calculateTotal(this.Openingbalance_list);
      if (this.Openingbalance_list.length <= 0) {
        this.processedOpeningbalanceLists = [];
      }
      else {
        this.processedOpeningbalanceLists = this.preprocessData(this.Openingbalance_list);
      }
    });
    // Fetch opening balance data for debit amounts
    this.service.getparams('AccMstOpeningbalance/GetAccMstOpeningbalance', params).pipe(
      catchError(error => {
        console.error('Error fetching opening balance (debit):', error);
        return [];
      }),
      finalize(() => {
        this.NgxSpinnerService.hide();
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.Openingbalance_lists = result?.Openingbalance_lists || [];
      this.totalDebit = this.calculateTotals(this.Openingbalance_lists);
      if (this.Openingbalance_lists.length <= 0) {
        this.processedOpeningbalanceList = [];
      }
      else {
        this.processedOpeningbalanceList = this.preprocessData(this.Openingbalance_lists);
      }
    });
  }

  calculateDifference() {
    const credit = parseFloat(this.totalCredit.replace(/,/g, '') || '0');
    const debit = parseFloat(this.totalDebit.replace(/,/g, '') || '0');
    const diff = credit - debit;
    this.difference_value = this.formatAmount(diff);
  }

  formatAmount(value: number): string {
    const formatter = new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
    return formatter.format(value);
  }

  OnChangeBalanceType() {
    this.balanced_type = this.OpeningBalanceForm.value.balance_type;
    if (this.balanced_type === 'Liability') {
      this.showparentname = true;
      this.disabledparentname = false;
      var url = 'AccMstOpeningbalance/GetParentName'
      this.service.get(url).subscribe((result: any) => {
        this.ParentNameList = result.GetParentName_List;
      });
    }
    else if (this.balanced_type === 'Asset') {
      this.showparentname = false;
      this.disabledparentname = true;
      var url = 'AccMstOpeningbalance/GetParentNameAsset'
      this.service.get(url).subscribe((result: any) => {
        this.ParentNameasset_List = result.GetParentNameasset_List;
      });
    }
    else {
      this.disabledparentname = false;
      this.showparentname = false;
    }
  }

  onclose() {
    this.OpeningBalanceForm.reset();
    this.OpeningBalanceEditForm.reset();
    this.OpeningBalanceAssetEditForm.reset();
    this.OpeningBalanceForm.get("balance_type")?.setValue('Liability');
    this.OnChangeBalanceType();
  }

  OnChangeParentName() {
    var url = 'AccMstOpeningbalance/GetLiabilityAccountName';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.liabilityacct_List = this.responsedata.Getliabilityacct_List;
    });
  }

  get editassetamount_value() {
    return this.OpeningBalanceAssetEditForm.get('editassetamount_value');
  }

  get editamount_value() {
    return this.OpeningBalanceEditForm.get('editamount_value');
  }

  OnChangeAssetParentName() {
    var url = 'AccMstOpeningbalance/GetAssetAccountName';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.assetacct_List = this.responsedata.Getassetacct_List;
    });
  }

  get amount_value() {
    return this.OpeningBalanceForm.get('amount_value');
  }

  get balance_type() {
    return this.OpeningBalanceForm.get('balance_type')!;
  }
  get liabilityaact_name() {
    return this.OpeningBalanceForm.get('liabilityaact_name')!;
  }
  get branch_name() {
    return this.OpeningBalanceForm.get('branch_name')!;
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }

  submit() {
    this.OpeningBalanceForm.value;
    if (this.OpeningBalanceForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'AccMstOpeningbalance/PostOpeningBalance';
      this.service.post(url, this.OpeningBalanceForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.GetOpeningbalance();
          this.calculateDifference();
          this.OpeningBalanceForm.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.OpeningBalanceForm.get("balance_type")?.setValue('Liability');
          this.OnChangeBalanceType();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.GetOpeningbalance();
          this.calculateDifference();
          this.OpeningBalanceForm.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.OpeningBalanceForm.get("balance_type")?.setValue('Liability');
          this.OnChangeBalanceType();
        }
      });
    }
    else { }
  }

  edit(parameter: any) {
    this.parameterValue1 = parameter
    this.OpeningBalanceEditForm.get("opening_balance_gid")?.setValue(this.parameterValue1.opening_balance_gid);
    this.OpeningBalanceEditForm.get("editamount_value")?.setValue(this.parameterValue1.credit_amount);
    this.accountnameview = this.parameterValue1.account_name;
    this.acctgroupnameview = this.parameterValue1.accountgroup_name;
    this.branchview = this.parameterValue1.branch_name;
    this.financialyearview = this.parameterValue1.openingfinancial_year;
    this.OpeningBalanceEditForm.get("editremarks")?.setValue(this.parameterValue1.remarks);
  }
  //Liability Edit
  update() {
    this.NgxSpinnerService.show();
    var url = 'AccMstOpeningbalance/Getupdateopeningbalance';
    this.service.post(url, this.OpeningBalanceEditForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.GetOpeningbalance();
        this.calculateDifference();
        this.OpeningBalanceEditForm.reset();
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.GetOpeningbalance();
        this.calculateDifference();
        this.ledgerback();
        this.OpeningBalanceEditForm.reset();
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
      }
    });
  }
  //Asset Edit
  edit_asset(parameter: any) {
    this.parameterValue2 = parameter
    this.OpeningBalanceAssetEditForm.get("opening_balance_gid")?.setValue(this.parameterValue2.opening_balance_gid);
    this.OpeningBalanceAssetEditForm.get("editassetamount_value")?.setValue(this.parameterValue2.debit_amount);
    this.assetaccountnameview = this.parameterValue2.account_name;
    this.assetacctgroupnameview = this.parameterValue2.accountgroup_name;
    this.assetbranchview = this.parameterValue2.branch_name;
    this.assetfinancialyearview = this.parameterValue2.openingfinancial_year;
    this.OpeningBalanceAssetEditForm.get("editassetremarks")?.setValue(this.parameterValue2.remarks);
  }

  asset_update() {
    this.OpeningBalanceAssetEditForm.value;
    this.NgxSpinnerService.show();
    var url = 'AccMstOpeningbalance/Getassetupdateopeningbalance';
    this.service.post(url, this.OpeningBalanceAssetEditForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.OpeningBalanceAssetEditForm.get("editassetamount_value")?.setValue(null);
        this.OpeningBalanceAssetEditForm.get("editassetremarks")?.setValue(null);
        this.GetOpeningbalance();
        this.calculateDifference();
        this.OpeningBalanceAssetEditForm.reset();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.OpeningBalanceAssetEditForm.get("editassetamount_value")?.setValue(null);
        this.OpeningBalanceAssetEditForm.get("editassetremarks")?.setValue(null);
        this.Assetledgerback();
        this.GetOpeningbalance();
        this.calculateDifference();
        this.OpeningBalanceAssetEditForm.reset();
      }
      this.NgxSpinnerService.hide();
    });
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

  GetLiabilityGroupNameList() {
    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear;

      var url = 'AccMstOpeningbalance/GetBranchDetails'
      this.service.get(url).subscribe((result: any) => {
        this.Branchdtl_list = result.branchdtl_lists;
        this.branch_name1 = this.Branchdtl_list[0].branch_gid;
        // this.reactiveform.patchValue({ branch_name: this.branch_name });

        let params = {
          entity_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.FinancialYear_List[0].finyear
        }
        var url = 'AccMstOpeningbalance/GetLiabilityGroupNameList';
        this.service.getparams(url, params).subscribe((result: any) => {
          this.responsedata = result;
          this.LiabilityGroupName_lists = this.responsedata.LiabilityGroupName_lists;
        });
      });
    });
  }

  GetLiabilitySubgroupNameList(account_gid: any) {
    this.LiabilitySubgroupflag = true;
    let param = {
      account_gid: account_gid,
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    var url = 'AccMstOpeningbalance/GetLiabilitySubGroupNameList'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.LiabilitySubGroupName_lists = this.responsedata.LiabilitySubGroupName_lists;
      this.AccountList = this.responsedata.AccountList;
      this.Liability_AccountName = this.AccountList[0].account_name
    });
  }

  GetLiabilityLedgerNameList(account_gid: any) {
    this.LiabilityLedgerflag = true;
    let param = {
      account_gid: account_gid,
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    var url = 'AccMstOpeningbalance/GetLiabilityLedgerNameList'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.LiabilityLedgerName_lists = this.responsedata.LiabilityLedgerName_lists;
      this.LedgerAccountList = this.responsedata.LedgerAccountList;
      this.LiabilityLedger_AccountName = this.LedgerAccountList[0].account_name
    });
  }

  GetAssetGroupNameList() {
    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear;

      var url = 'AccMstOpeningbalance/GetBranchDetails'
      this.service.get(url).subscribe((result: any) => {
        this.Branchdtl_list = result.branchdtl_lists;
        this.branch_name1 = this.Branchdtl_list[0].branch_gid;
        // this.reactiveform.patchValue({ branch_name: this.branch_name });

        let params = {
          entity_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.FinancialYear_List[0].finyear
        }
        var url = 'AccMstOpeningbalance/GetAssetGroupNameList';
        this.service.getparams(url, params).subscribe((result: any) => {
          this.responsedata = result;
          this.AssetGroupName_lists = this.responsedata.AssetGroupName_lists;
        })
      });
    });
  }

  GetAssetSubgroupNameList(account_gid: any) {
    this.AssetSubgroupflag = true;
    let param = {
      account_gid: account_gid,
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    var url = 'AccMstOpeningbalance/GetAssetSubGroupNameList'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.AssetSubGroupName_lists = this.responsedata.AssetSubGroupName_lists;
      this.AssetAccountList = this.responsedata.AssetAccountList;
      this.Asset_AccountName = this.AssetAccountList[0].account_name
    });
  }

  GetAssetLedgerNameList(account_gid: any) {
    this.AssetLedgerflag = true;
    let param = {
      account_gid: account_gid,
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    var url = 'AccMstOpeningbalance/GetAssetLedgerNameList'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.AssetLedgerName_lists = this.responsedata.AssetLedgerName_lists;
      this.AssetLedgerAccountList = this.responsedata.AssetLedgerAccountList;
      this.AssetLedger_AccountName = this.AssetLedgerAccountList[0].account_name
      // this.totalAssetLedgerName = this.calculateTotal(this.AssetLedgerName_lists,'credit_amount');
    });
  }

  calculateTotal(dataList: any[],): string {
    const total = dataList.reduce((sum: number, item: any) => {
      const amount = parseFloat(item?.credit_amount?.replace(/,/g, '') || '0');
      return sum + amount;
    }, 0);
    return this.formatAmount(total);
  }

  calculateTotals(dataList: any[]): string {
    const total = dataList.reduce((sum: number, item: any) => {
      const amount = parseFloat(item?.debit_amount?.replace(/,/g, '') || '0');
      return sum + amount;
    }, 0);
    return this.formatAmount(total);
  }

  subgroupback() {
    this.LiabilitySubgroupflag = false;
    this.LiabilityLedgerflag = false;
  }

  ledgerback() {
    this.LiabilitySubgroupflag = true;
    this.LiabilityLedgerflag = false;
  }

  Assetsubgroupback() {
    this.AssetSubgroupflag = false;
    this.AssetLedgerflag = false;
  }

  Assetledgerback() {
    this.AssetSubgroupflag = true;
    this.AssetLedgerflag = false;
  }
  GetOnSearchGroupName() {
    let params = {
      entity_gid: this.branch_name1,
      finyear: this.finyear
    }
    var url = 'AccMstOpeningbalance/GetAssetGroupNameList';
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.AssetGroupName_lists = this.responsedata.AssetGroupName_lists;
    })
    var url = 'AccMstOpeningbalance/GetLiabilityGroupNameList';
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.LiabilityGroupName_lists = this.responsedata.LiabilityGroupName_lists;
    });
  }
  preprocessData(data: any[]): any[] {
    this.processedData = [];

    let mainGroupSpan = 1;
    let subGroupSpan = 1;
    this.prevMainGroup = null;
    this.prevSubGroup = null;

    data.forEach((item, index) => {
      // Check MainGroup_name
      if (this.prevMainGroup !== item.MainGroup_name) {
        if (this.prevMainGroup !== null) {
          // Set the rowspan for the previous MainGroup_name
          for (let i = index - mainGroupSpan; i < index; i++) {
            this.processedData[i].mainGroupSpan = mainGroupSpan;
          }
        }
        mainGroupSpan = 1;
        this.prevMainGroup = item.MainGroup_name;
      } else {
        mainGroupSpan++;
      }

      // Check subgroup_name
      if (this.prevSubGroup !== item.subgroup_name) {
        if (this.prevSubGroup !== null) {
          // Set the rowspan for the previous subgroup_name
          for (let i = index - subGroupSpan; i < index; i++) {
            this.processedData[i].subGroupSpan = subGroupSpan;
          }
        }
        subGroupSpan = 1;
        this.prevSubGroup = item.subgroup_name;
      } else {
        subGroupSpan++;
      }

      this.processedData.push({ ...item });
    });

    // Set the rowspan for the last MainGroup_name and subgroup_name
    for (let i = data.length - mainGroupSpan; i < data.length; i++) {
      this.processedData[i].mainGroupSpan = mainGroupSpan;
    }
    for (let i = data.length - subGroupSpan; i < data.length; i++) {
      this.processedData[i].subGroupSpan = subGroupSpan;
    }

    return this.processedData;
  }
  
  GetSummary() {
    var url = 'AccMstOpeningbalance/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name1 = this.Branchdtl_list[0].branch_gid;

      var url = 'AccTrnBankbooksummary/GetFinancialYear'
      this.service.get(url).subscribe((result: any) => {
        this.FinancialYear_List = result.GetFinancialYear_List;
        this.finyear = this.FinancialYear_List[0].finyear;

        let params = {
          branch_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.FinancialYear_List[0].finyear
        }
        var url = 'AccMstOpeningbalance/GetLiabilitySummary'
        this.service.getparams(url, params).subscribe((result: any) => {
          this.responsedata = result;
          this.liabilitySummary_lists = this.responsedata.liabilitySummary_lists;
          // this.liabilitysubled_Summary_lists = this.responsedata.liabilitysubled_Summary_lists;
          // this.totalAssetLedgerName = this.calculateTotal(this.AssetLedgerName_lists,'credit_amount');
          this.subList = result.liabilitysubled_Summary_lists.map((item: any) => ({ ...item, visible: false }));
        });
      });
    });
  }

  addItemsFromTargetList() {
    this.subList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.liabilitySummary_lists);
      // this.recursivelyAddItems(targetItem, this.liabilitysubled_Summary_lists);
    });    
  }

  recursivelyAddItems(targetItem: any, sourceList: any[]) {
    const matchingIndex = sourceList.findIndex(sourceItem => sourceItem.account_gid === targetItem.accountgroup_gid);
    if (matchingIndex !== -1) {
      if (!sourceList[matchingIndex].liabilitysubled_Summary_lists) {
        sourceList[matchingIndex].liabilitysubled_Summary_lists = [];
      }
      sourceList[matchingIndex].liabilitysubled_Summary_lists.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.liabilitysubled_Summary_lists && sourceItem.liabilitysubled_Summary_lists.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.liabilitysubled_Summary_lists);
        }
      });
    }
  }

  showTab(tab: string) {
    this.currentTab = tab;
  }
  validateNumericInput(event: KeyboardEvent): void {
    const inputChar = String.fromCharCode(event.charCode);
    
    // Allow only numeric input and decimal point
    if (!/[0-9.]/.test(inputChar)) {
      event.preventDefault();
    }
  }
  formatCurrency(event: any): void {
    let input = event.target.value;
    if (input === '') {
      event.target.value = '';
      return;
    }
    const cursorPosition = event.target.selectionStart;
    let cleanedInput = input.replace(/[^0-9.]/g, '');
    const parts = cleanedInput.split('.');
    if (parts.length > 2) {
      cleanedInput = `${parts[0]}.${parts.slice(1).join('')}`;
    }
    const integerPart = parts[0];
    let formattedInteger = parseInt(integerPart, 10).toLocaleString('en-US');
    let formattedInput = formattedInteger;
    if (parts.length > 1) {
      const fractionalPart = parts[1];
      formattedInput += '.' + fractionalPart.slice(0, 2);
    }
    event.target.value = formattedInput;
    const newCursorPosition = cursorPosition + (formattedInput.length - input.length);
    event.target.setSelectionRange(newCursorPosition, newCursorPosition);
  }
}
