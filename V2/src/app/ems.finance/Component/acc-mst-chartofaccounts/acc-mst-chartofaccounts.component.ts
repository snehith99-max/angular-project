import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

interface Row {

}

@Component({
  selector: 'app-acc-mst-chartofaccounts',
  templateUrl: './acc-mst-chartofaccounts.component.html',
  styleUrls: ['./acc-mst-chartofaccounts.component.scss']
})
export class AccMstChartofaccountsComponent {
  depth: any;
  sampleList: any;
  account_gid: any;
  accountGroup_gid: any;
  values: any;
  mainList: any[] = [];
  subList: any[] = [];
  expanded: boolean = false;
  responsedata: any;
  account_code: any;
  account_group: any;
  Getchartofaccountcount_list: any;
  ledgerForm!: FormGroup;
  ledgerFormsub!: FormGroup;
  reactiveform!: FormGroup;
  mainListTwo: any;
  isChecked!: boolean
  switchpage = 'P&L';
  assetfolder: any;
  liabilityfolder: any;
  accountgroup_name: any;
  showOptionsDivId: any;
  currentTab = 'Liability';
  account_type: any;

  constructor(public service: SocketService, private fb: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      account_groupgid: new FormControl(''),
      account_groupcode: new FormControl('', Validators.required),
      account_groupname: new FormControl('', Validators.required),
    })

    this.ledgerFormsub = this.fb.group({
      account_gid: new FormControl(null),
      accountcodes: new FormControl(null),
      accountgroups: new FormControl(null),
      accountsubcode: new FormControl(null, [Validators.required, Validators.pattern("(?=.*[a-zA-Z0-9]).+$"),]),
      accountsubgroup: new FormControl(null, [Validators.required, Validators.pattern(""),]),
      ledger_flag: ['N'],
      accountType: ['PL'], // Default to 'PL' (Profit & Loss)
      displayType: ['expense'],
    });
  }

  ngOnInit(): void {
    this.depth = 0;
    this.initForm();
    this.summary();
    const url2 = 'ChartofAccount/ChartofAccountCountList';
    this.service.get(url2).subscribe((result: any) => {
      this.responsedata = result;
      this.Getchartofaccountcount_list = this.responsedata.Getchartofaccountcount_list;
      this.account_gid = this.Getchartofaccountcount_list[0].account_gid;
      this.account_code = this.Getchartofaccountcount_list[0].account_code;
      this.account_group = this.Getchartofaccountcount_list[0].account_name;
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  summary() {
    this.NgxSpinnerService.show();
    const url1 = 'ChartofAccount/getFolders';
    // $('#Accountgroup_list').DataTable().destroy();
    $('#liabilityfolder').DataTable().destroy();
    $('#assetfolder').DataTable().destroy();
    $('#mainListTwo').DataTable().destroy();
    $('#mainList').DataTable().destroy();
    this.service.get(url1).subscribe((result: any) => {
      
      this.mainList = result.parentfolders.map((item: any) => ({ ...item, visible: false }));
      this.mainListTwo = result.incomefolder.map((item: any) => ({ ...item, visible: false }));
      this.assetfolder = result.assetfolder.map((item: any) => ({ ...item, visible: false }));
      this.liabilityfolder = result.liabilityfolder.map((item: any) => ({ ...item, visible: false }));
      this.subList = result.subfolders.map((item: any) => ({ ...item, visible: false }));
      this.NgxSpinnerService.hide();

      this.addItemsFromTargetList();
      // console.log(this.liabilityfolder,'Liability List:')
      // console.log(this.subList,'SubList:')
      console.log(this.subList, 'sub List:')

    });
  }  

  onSwitchChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const value = inputElement.checked ? 'yes' : 'no';
    console.log('Checkbox value:', value);
  }

  initForm(): void {
    this.ledgerForm = this.fb.group({
      accountcode: new FormControl(null, [Validators.required, Validators.pattern("(?=.*[a-zA-Z0-9]).+$"),]),
      accountgroup: new FormControl(null, [Validators.required, Validators.pattern(""),]),
      accountType: ['PL'], // Default to 'PL' (Profit & Loss)
      displayType: ['expense'], // Default to 'expense' for 'PL'
    });
    // Subscribe to changes in accountType and update displayType options accordingly
    const accountTypeControl = this.ledgerForm.get('accountType');
    if (accountTypeControl) { // Check if accountTypeControl is not null
      accountTypeControl.valueChanges.subscribe((value: string) => {
        const displayTypeControl = this.ledgerForm.get('displayType');
        if (displayTypeControl) { // Check if displayTypeControl is not null
          if (value === 'PL') {
            displayTypeControl.setValue('expense'); // Set default to 'expense' for 'PL'
          } else if (value === 'BS') {
            displayTypeControl.setValue('asset'); // Set default to 'asset' for 'BS'
          }
        }
      });
    }
  }

  addItemsFromTargetList() {
    this.subList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.mainList);
      this.recursivelyAddItems(targetItem, this.mainListTwo);
      this.recursivelyAddItems(targetItem, this.assetfolder);
      this.recursivelyAddItems(targetItem, this.liabilityfolder);
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

  onsubmitexpense() {
    this.ledgerForm.get("accountType")?.setValue('PL');
    this.ledgerForm.get("displayType")?.setValue('expense');
    if (this.ledgerForm.status == 'VALID') {
      //console.log(this.ledgerForm.value)
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountGroup';
      this.service.post(api, this.ledgerForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          // console.log(this.account_gid)
          // this.getsamplesummary();
          this.summary();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          this.ToastrService.warning(result.message)
          this.summary();
        }
      });
    }
  }

  onsubmitincome() {
    this.ledgerForm.get("accountType")?.setValue('PL');
    this.ledgerForm.get("displayType")?.setValue('income');
    if (this.ledgerForm.status == 'VALID') {
      //console.log(this.ledgerForm.value)
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountGroup';
      this.service.post(api, this.ledgerForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          // console.log(this.account_gid)
          // this.getsamplesummary();
          this.summary();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          this.ToastrService.warning(result.message)
          this.summary();
        }
      });
    }
  }

  onsubmitliability() {
    this.ledgerForm.get("accountType")?.setValue('BS');
    this.ledgerForm.get("displayType")?.setValue('liability');
    if (this.ledgerForm.status == 'VALID') {
      //console.log(this.ledgerForm.value)
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountGroup';
      this.service.post(api, this.ledgerForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          // console.log(this.account_gid)
          // this.getsamplesummary();
          this.summary();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          this.ToastrService.warning(result.message)
          this.summary();
        }
      });
    }
  }

  onsubmitasset() {
    this.ledgerForm.get("accountType")?.setValue('BS');
    this.ledgerForm.get("displayType")?.setValue('asset');
    if (this.ledgerForm.status == 'VALID') {
      //console.log(this.ledgerForm.value)
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountGroup';
      this.service.post(api, this.ledgerForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          // console.log(this.account_gid)
          // this.getsamplesummary();
          this.summary();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerForm.reset();
          this.ledgerForm.get("accountType")?.setValue('PL');
          this.ledgerForm.get("displayType")?.setValue('expense');
          this.ToastrService.warning(result.message)
          this.summary();
        }
      });
    }
  }

  oncloseaccountgroup() {
    this.ledgerForm.reset();
    this.ledgerForm.get("accountType")?.setValue('PL');
    this.ledgerForm.get("displayType")?.setValue('expense');
  }

  openModalsubedit(list: any, accountgroup_name: any) {
    this.ledgerFormsub.get("accountsubcode")?.setValue(list.account_code);
    this.ledgerFormsub.get("accountsubgroup")?.setValue(list.account_name);
    this.ledgerFormsub.get("accountcodes")?.setValue(list.account_gid);
    this.ledgerFormsub.get("account_gid")?.setValue(this.account_gid);

    const hasChildValue = list.has_child;
    const ledgerFlagValue = hasChildValue === 'N' ? 'Y' : 'N';

    this.ledgerFormsub.get('ledger_flag')?.setValue(ledgerFlagValue);
    this.accountgroup_name = accountgroup_name
  }

  onsubmitexpensesubgroup() {
    debugger
    this.ledgerFormsub.get("accountType")?.setValue('PL');
    this.ledgerFormsub.get("displayType")?.setValue('expense');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountSubGroup';
      this.service.post(api, this.ledgerFormsub.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerFormsub.reset();
          this.summary()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.warning(result.message)
          this.summary()
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
      });
    }
  }

  onsubmitincomesubgroup() {
    this.ledgerFormsub.get("accountType")?.setValue('PL');
    this.ledgerFormsub.get("displayType")?.setValue('income');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountSubGroup';
      this.service.post(api, this.ledgerFormsub.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerFormsub.reset();
          this.summary()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.warning(result.message)
          this.summary()
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
      });
    }
  }

  onsubmitliabilitysubgroup() {
    debugger
    this.ledgerFormsub.get("accountType")?.setValue('BS');
    this.ledgerFormsub.get("displayType")?.setValue('liability');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      
      var api = 'ChartofAccount/PostAccountSubGroup';
      this.service.post(api, this.ledgerFormsub.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerFormsub.reset();
          this.summary()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.warning(result.message)
          this.summary()
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
        }
      });
    }
  }

  onsubmitassetsubgroup() {
    debugger
    this.ledgerFormsub.get("accountType")?.setValue('BS');
    this.ledgerFormsub.get("displayType")?.setValue('asset');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var api = 'ChartofAccount/PostAccountSubGroup';
      this.service.post(api, this.ledgerFormsub.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == true) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.ledgerFormsub.reset();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary()
        }
        else {
          this.NgxSpinnerService.hide();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.warning(result.message)
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary()
        }
      });
    }
  }

  onclosesubgroup() {
    this.ledgerFormsub.reset();
    this.ledgerFormsub.get("ledger_flag")?.setValue('N');
  }  

  onupdateexpensesubgroup() {
    this.ledgerFormsub.get("accountType")?.setValue('PL');
    this.ledgerFormsub.get("displayType")?.setValue('expense');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'ChartofAccount/UpdateAccountSubGroup'
      this.service.post(url, this.ledgerFormsub.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.ledgerFormsub.reset()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        else {
          this.ledgerFormsub.reset();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.success(result.message)
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        this.NgxSpinnerService.hide();
      });
    }
  }

  onupdateincomesubgroup() {
    this.ledgerFormsub.get("accountType")?.setValue('PL');
    this.ledgerFormsub.get("displayType")?.setValue('income');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'ChartofAccount/UpdateAccountSubGroup'
      this.service.post(url, this.ledgerFormsub.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.ledgerFormsub.reset()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        else {
          this.ledgerFormsub.reset();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.success(result.message)
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        this.NgxSpinnerService.hide();
      });
    }
  }

  onupdateliabilitysubgroup() {
    this.ledgerFormsub.get("accountType")?.setValue('BS');
    this.ledgerFormsub.get("displayType")?.setValue('liability');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'ChartofAccount/UpdateAccountSubGroup'
      this.service.post(url, this.ledgerFormsub.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.ledgerFormsub.reset()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        else {
          this.ledgerFormsub.reset();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.success(result.message)
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        this.NgxSpinnerService.hide();
      });
    }
  }

  onupdateassetsubgroup() {
    this.ledgerFormsub.get("accountType")?.setValue('BS');
    this.ledgerFormsub.get("displayType")?.setValue('asset');
    if (this.ledgerFormsub.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'ChartofAccount/UpdateAccountSubGroup'
      this.service.post(url, this.ledgerFormsub.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.ledgerFormsub.reset()
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        else {
          this.ledgerFormsub.reset();
          this.ledgerFormsub.get('ledger_flag')?.setValue('N');
          this.ToastrService.success(result.message)
          this.ledgerFormsub.get("accountType")?.setValue('PL');
          this.ledgerFormsub.get("displayType")?.setValue('expense');
          this.summary();
        }
        this.NgxSpinnerService.hide();
      });
    }
  }

  get accountcode() {
    return this.ledgerForm.get('accountcode')!;
  }
  get accountgroup() {
    return this.ledgerForm.get('accountgroup')!;
  }
  get accountsubcode() {
    return this.ledgerFormsub.get('accountsubcode')!;
  }
  get accountsubgroup() {
    return this.ledgerFormsub.get('accountsubgroup')!;
  }
  get account_groupcode() {
    return this.reactiveform.get('account_groupcode')!;
  }
  get account_groupname() {
    return this.reactiveform.get('account_groupname')!;
  }


  onaddsubgroup(account_code: any, account_name: any, account_gid: any) {
    this.ledgerFormsub.get("accountcodes")?.setValue(account_code);
    this.ledgerFormsub.get("accountgroups")?.setValue(account_name);
    this.ledgerFormsub.get("account_gid")?.setValue(account_gid);
    this.account_gid = account_gid;
    this.account_code = account_code;
    this.account_group = account_name;
  }

  handleEditClick(event: Event, category: any) {
    event.stopPropagation(); // Prevent card click event from firing
    this.reactiveform.get("account_groupgid")?.setValue(category.account_gid);
    this.reactiveform.get("account_groupcode")?.setValue(category.account_code);
    this.reactiveform.get("account_groupname")?.setValue(category.account_name);
  }

  oneditgroup() {
    if (this.reactiveform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'ChartofAccount/UpdateAccountGroup'
      this.service.post(url, this.reactiveform.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.reactiveform.reset();
          this.summary();
        }
        else {
          this.ToastrService.success(result.message)
          this.reactiveform.reset();
          this.summary();
        }
        this.NgxSpinnerService.hide();
      });
    }
  }
  delete(account_gid: any, account_type: any) {
    this.account_gid = account_gid
    this.account_type = account_type
  }

  ondeletesub() {
    this.NgxSpinnerService.show();
    var url = 'ChartofAccount/DeleteChartofAccount'
    let param = {
      account_gid: this.account_gid,
      account_type: this.account_type
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.summary();
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.summary();
      }
    });
  }

  toggleswitch(event: Event, account_gid: any): void {
    const inputElement = event.target as HTMLInputElement;
    const inputValue = inputElement.checked ? 'N' : 'Y';
    console.log('Checkbox value:', inputValue);
    var api = 'ChartofAccount/UpdateLedger'
    var params = {
      ledger_flag: inputValue,
      accountcodes: account_gid
    }
    console.log(params)
    this.service.post(api, params).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.summary()
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.summary()
      }
    });
  }

  switchsection(value: any) {
    this.switchpage = value
  }

  // getSerialNumber(index: number, level: number): string {
  //   return `${' '.repeat(level - 1)}${index + 1}`;
  // }

  getRowspan(subfolders: any[]): number {
    // Calculate the number of rows this group spans
    let count = 0;
    subfolders.forEach(subfolder => {
      count += 1 + (subfolder.subfolders ? this.getRowspan(subfolder.subfolders) : 0);
    });
    return count;
  }
  
  showTab(tab: string) {
    this.currentTab = tab;
  }
}
