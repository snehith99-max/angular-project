import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-acc-mst-chartofaccountincome',
  templateUrl: './acc-mst-chartofaccountincome.component.html',
  styleUrls: ['./acc-mst-chartofaccountincome.component.scss']
})
export class AccMstChartofaccountincomeComponent {
  displayTypeOptions: { value: string; label: string }[] = [];
  displayType_list: any;
  account_subgid: any;
  selectedCategory: any;
  account_gid: any;
  account_giddel: any;
  dataTable: any[] = [];
  GetFundTransfer_list: any;
  responsedata: any;
  selectedCategoryId: string | null = null;
  Getchartofaccountincome_list: any;
  Getchartofsubaccount_list: any;
  branchname_list: any;
  ledgerForm!: FormGroup;
  ledgerFormsub!: FormGroup;
  reactiveform!: FormGroup;
  account_group: any;
  account_code: any;
  account_name: any;
  filteredCategories: any[] = [];
  searchText: string = '';
  Getchartofaccountcount_list:any;
  constructor(private fb: FormBuilder, public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      account_groupgid: new FormControl(''),
      account_groupcode: new FormControl('', Validators.required),
      account_groupname: new FormControl('', Validators.required),
    })
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
    this.initForm();
    this.getsummary();
    var url = 'ChartofAccount/ChartofAccountCountList'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Getchartofaccountcount_list = this.responsedata.Getchartofaccountcount_list;
      //console.log(this.Getchartofaccountcount_list)
      
    });
  }
  getsummary()
  {
    var url = 'ChartofAccount/ChartofAccountIncomeSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Getchartofaccountincome_list = this.responsedata.Getchartofaccountincome_list;
      if (this.Getchartofaccountincome_list.length >= 0) {
       // console.log(this.Getchartofaccountincome_list)
        this.account_gid = this.Getchartofaccountincome_list[0].account_gid;
        this.account_code = this.Getchartofaccountincome_list[0].account_code;
        this.account_group = this.Getchartofaccountincome_list[0].account_name;
        // console.log(this.account_name)
        // console.log(this.account_code)
       //  console.log(this.account_gid)
        this.isCardSelected(this.account_gid);
        let param = {
          account_gid: this.account_gid
        }
        var url = 'ChartofAccount/ChartofSubAccountSummary'
        this.service.getparams(url, param).subscribe((result: any) => {
          $('#Getchartofsubaccount_list').DataTable().destroy();
          this.responsedata = result;
          this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
          this.filteredCategories = this.Getchartofaccountincome_list;
          //console.log('list',this.filteredCategories)
          setTimeout(() => {
            $('#Getchartofsubaccount_list').DataTable(
              {
                // code by snehith for customized pagination
                "pageLength": 25, // Number of rows to display per page
                "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
              }
            );
          }, 1);
          if (this.selectedCategoryId === this.account_gid) {
            // If the same card is clicked again, deselect it
            this.selectedCategoryId = null;
          } else {
            // Otherwise, select the clicked card
            this.selectedCategoryId = this.account_gid;
          }
        });


        // console.log(this.Getchartofaccountincome_list[0].account_gid)
      }
      else {
        let param = {
          account_gid: ''
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
                "pageLength": 25, // Number of rows to display per page
                "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
              }
            );
          }, 1);

        });
      }
      setTimeout(() => {
        $('#Getchartofaccountincome_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 25, // Number of rows to display per page
            "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  getsummaryadd()
  {
    var url = 'ChartofAccount/ChartofAccountIncomeSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Getchartofaccountincome_list = this.responsedata.Getchartofaccountincome_list;
      if (this.Getchartofaccountincome_list.length >= 0) {
        //console.log(this.Getchartofaccountincome_list)
        this.account_gid = this.Getchartofaccountincome_list[0].account_gid;
        this.account_code = this.Getchartofaccountincome_list[0].account_code;
        this.account_group = this.Getchartofaccountincome_list[0].account_name;
        // console.log(this.account_name)
        // console.log(this.account_code)
        // console.log(this.account_gid)
        this.isCardSelected(this.account_gid);
        let param = {
          account_gid: this.account_gid
        }
        var url = 'ChartofAccount/ChartofSubAccountSummary'
        this.service.getparams(url, param).subscribe((result: any) => {
          $('#Getchartofsubaccount_list').DataTable().destroy();
          this.responsedata = result;
          this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
          this.filteredCategories = this.Getchartofaccountincome_list;
          //console.log('list',this.Getchartofsubaccount_list)
          setTimeout(() => {
            $('#Getchartofsubaccount_list').DataTable(
              {
                // code by snehith for customized pagination
                "pageLength": 25, // Number of rows to display per page
                "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
              }
            );
          }, 1);
          if (this.selectedCategoryId === this.account_gid) {
            // If the same card is clicked again, deselect it
            this.selectedCategoryId =this.account_gid;
          } else {
            // Otherwise, select the clicked card
            this.selectedCategoryId = this.account_gid;
          }
        });


        // console.log(this.Getchartofaccountincome_list[0].account_gid)
      }
      else {
        let param = {
          account_gid: ''
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
                "pageLength": 25, // Number of rows to display per page
                "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
              }
            );
          }, 1);

        });
      }
      setTimeout(() => {
        $('#Getchartofaccountincome_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 25, // Number of rows to display per page
            "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  applyFilter(): void {
    if (!this.searchText) {
      this.filteredCategories = this.Getchartofaccountincome_list; // Show all categories if search text is empty
    } else {
      this.filteredCategories = this.Getchartofaccountincome_list.filter((category: { account_name: string; }) =>
        category.account_name.toLowerCase().includes(this.searchText.toLowerCase())
      );
    }
  }
  handleCardClick(categoryId: string, account_code: any, account_name: any) {
    this.account_gid = categoryId;
    this.account_code = account_code;
    this.account_group = account_name;
    let param = {
      account_gid: this.account_gid
    }
    var url = 'ChartofAccount/ChartofSubAccountSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#Getchartofsubaccount_list').DataTable().destroy();
      this.responsedata = result;
      this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
      //console.log('list', this.Getchartofsubaccount_list)
      setTimeout(() => {
        $('#Getchartofsubaccount_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 25, // Number of rows to display per page
            "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
          }
        );
      }, 1);
    });
    if (this.selectedCategoryId === categoryId) {
      // If the same card is clicked again, deselect it
      this.selectedCategoryId = null;
    } else {
      // Otherwise, select the clicked card
      this.selectedCategoryId = categoryId;
    }
  }

  // Method to handle edit action
  handleEditClick(event: Event, category: any) {
    event.stopPropagation(); // Prevent card click event from firing
    //console.log(category)  
    this.reactiveform.get("account_groupgid")?.setValue(category.account_gid);
    this.reactiveform.get("account_groupcode")?.setValue(category.account_code);
    this.reactiveform.get("account_groupname")?.setValue(category.account_name);

  }

  // Method to handle delete action
  handleDeleteClick(event: Event, category: any) {
    event.stopPropagation(); // Prevent card click event from firing
    this.account_subgid =category.account_gid;
    this.account_code=category.account_code;
    this.account_name=category.account_name;
    //console.log(`Delete clicked for category ID: ${this.account_gid}`);
  }
  // Method to check if a card is selected
  isCardSelected(categoryId: string) {
    return this.selectedCategoryId === categoryId;
  }
  childview(account_gid: any, account_code: any, account_name: any) {
    this.account_gid = account_gid;
    this.account_code = account_code;
    this.account_name = account_name;
    const secretKey = 'storyboarderp';
    const lsaccount_gid = AES.encrypt(this.account_gid, secretKey).toString();
    const lsaccount_code = AES.encrypt(this.account_code, secretKey).toString();
    const lsaccount_name = AES.encrypt(this.account_name, secretKey).toString();

    this.router.navigate(['/finance/AccMstChartofAccountIncomeView', lsaccount_gid, lsaccount_code, lsaccount_name]);
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
  initForm(): void {
    this.ledgerForm = this.fb.group({
      accountcode: new FormControl(null, [
        Validators.required,
        Validators.pattern("(?=.*[a-zA-Z0-9]).+$"),
      ]),
      accountgroup: new FormControl(null, [
        Validators.required,
        Validators.pattern(""),
      ]),
      accountType: ['PL'], // Default to 'PL' (Profit & Loss)
      displayType: ['income'], // Default to 'income' for 'PL'

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

  onaddsubgroup() {
    //  console.log(this.account_code)
    //  console.log(this.account_group)
    //  console.log(this.account_gid)
    this.ledgerFormsub.get("accountcodes")?.setValue(this.account_code);
    this.ledgerFormsub.get("accountgroups")?.setValue(this.account_group);
    this.ledgerFormsub.get("account_gid")?.setValue(this.account_gid);
  }
  oneditgroup() {
    if(this.reactiveform.status =='VALID' )
    {
    //console.log(this.reactiveform.value)
    this.NgxSpinnerService.show();
    var url = 'ChartofAccount/UpdateAccountGroup'

    this.service.post(url,this.reactiveform.value).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.reactiveform.reset();

      }
      else{
        this.getsummarydelete();
        this.ToastrService.success(result.message)
        this.reactiveform.reset();

      }
      this.NgxSpinnerService.hide();

      
  }); 
    }
  }

  onsubmit() {

    if(this.ledgerForm.status =='VALID' )
    {
      this.NgxSpinnerService.show();
    //console.log(this.ledgerForm.value)
      var api = 'ChartofAccount/PostAccountGroup';
      this.service.post(api, this.ledgerForm.value).subscribe(
        (result: any) => {
          this.responsedata = result;

          if (result.status == true) {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)   
            this.ledgerForm.reset();
            this.ledgerForm.get("accountType")?.setValue('PL');
            this.ledgerForm.get("displayType")?.setValue('income'); 
            // console.log(this.account_gid)
            this.getsummaryadd();   
            
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
         

          }

        });
    }
  }
  oncloseaccountgroup() {
    this.ledgerForm.reset();
    this.ledgerForm.get("accountType")?.setValue('PL');
    this.ledgerForm.get("displayType")?.setValue('income');

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
            this.ToastrService.success(result.message)   
            this.ledgerFormsub.reset();
            this.ledgerFormsub.get('ledger_flag')?.setValue('N');
            this.commonsubgetsummary();
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
        this.commonsubgetsummary();
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
    // console.log(this.account_gid)
    // console.log(this.account_subgid)
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
        this.getsummarydelete();
        let param = {
          account_gid: this.account_gid
        }
        var url = 'ChartofAccount/ChartofSubAccountSummary'
        this.service.getparams(url, param).subscribe((result: any) => {
          $('#Getchartofsubaccount_list').DataTable().destroy();
          this.responsedata = result;
          this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
          //console.log('list', this.Getchartofsubaccount_list)
          setTimeout(() => {
            $('#Getchartofsubaccount_list').DataTable(
              {
                // code by snehith for customized pagination
                "pageLength": 25, // Number of rows to display per page
                "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
              }
            );
          }, 1);
        });
        
        if (this.selectedCategoryId === this.account_gid) {
          //After deleted record again select previous selected card
          this.selectedCategoryId = null;
        }
        else{
          this.selectedCategoryId = this.account_gid;
        }

        // window.location.reload();
    //     const group_gid ='FCOA000025';
    // this.handleCardClick(group_gid,this.account_code,this.account_name);
    // this.isCardSelected(group_gid);
      }
      
  
    });
  }
  ondeletesub()
  {
    // console.log(this.account_gid)
    // console.log(this.account_subgid)
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
      this.commonsubgetsummary();
        this.getsummarydelete();
        // window.location.reload();
    //     const group_gid ='FCOA000025';
    // this.handleCardClick(group_gid,this.account_code,this.account_name);
    // this.isCardSelected(group_gid);
      }
      
  
    });
  }
  commonsubgetsummary()
  {
    let param = {
      account_gid: this.account_gid
    }
    var url = 'ChartofAccount/ChartofSubAccountSummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#Getchartofsubaccount_list').DataTable().destroy();
      this.responsedata = result;
      this.Getchartofsubaccount_list = this.responsedata.Getchartofsubaccount_list;
      //console.log('list', this.Getchartofsubaccount_list)
      setTimeout(() => {
        $('#Getchartofsubaccount_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 25, // Number of rows to display per page
            "lengthMenu": [25, 50, 100, 200], // Dropdown to change page length
          }
        );
      }, 1);
    });
    
    if (this.selectedCategoryId === this.account_gid) {
      //After deleted record again select previous selected card
      this.selectedCategoryId = this.account_gid;
    }
    else{
      this.selectedCategoryId = null;
    }
  }
  
  getsummarydelete()
  {
    var url = 'ChartofAccount/ChartofAccountIncomeSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Getchartofaccountincome_list = this.responsedata.Getchartofaccountincome_list;
      this.filteredCategories = this.Getchartofaccountincome_list;
     
    });
  }
  onclosesubgroup() {
    this.ledgerFormsub.reset();
    this.ledgerFormsub.get("ledger_flag")?.setValue('N');

  }
 
}
